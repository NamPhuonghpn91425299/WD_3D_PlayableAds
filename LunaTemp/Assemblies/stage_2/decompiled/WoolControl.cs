using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[ExecuteAlways]
public class WoolControl : MonoBehaviour
{
	[Serializable]
	public class DecorObjectSetting
	{
		public DecoreControl DecorObject;

		[Range(0f, 1f)]
		public float WoolProgressStartSrop = 0.5f;
	}

	public bool debugUV;

	[Header("Mesh Object")]
	public MeshObjectData MeshObjectData;

	public MeshRenderer TopMeshRenderer;

	public MeshRenderer HideMeshRenderer;

	public Collider BoxCollider;

	public Material MainMaterial;

	public Material TranparentMaterial;

	public WoolAnimationData WoolAnimationData;

	public List<DecoreControl> DecoreControls;

	public List<DecoreControl> RemovedDecoreControls;

	public MeshFilter MeshFilter;

	[HideInInspector]
	public bool IsSetColorHightest;

	private MaterialPropertyBlock _topMaterialPropertyBlock;

	private MaterialPropertyBlock _hideMaterialPropertyBlock;

	private Color _currentColor;

	private int _indexLayer;

	[SerializeField]
	private List<Vector3> _spiralPath = new List<Vector3>();

	[SerializeField]
	private List<float> _spiralPathUVY = new List<float>();

	private bool _isPlayAnim;

	private void OnEnable()
	{
		DisplayColor();
	}

	private void Update()
	{
	}

	public void InitMesh()
	{
		MeshObjectData.ColorStack.Clear();
		_indexLayer = 0;
		BoxCollider.enabled = true;
		_currentColor = MeshObjectData.HightestColor;
		PushColor(MeshObjectData.HightestColor);
	}

	public bool PushColor(Color color)
	{
		if (MeshObjectData == null || !HideMeshRenderer || _indexLayer >= MeshObjectData.TotalLayer || _hideMaterialPropertyBlock == null)
		{
			return false;
		}
		MeshObjectData meshObjectData = MeshObjectData;
		if (meshObjectData.ColorStack == null)
		{
			meshObjectData.ColorStack = new List<Color>();
		}
		MeshObjectData.ColorStack.Add(color);
		_indexLayer++;
		if (MeshObjectData.ColorStack.Count > 1)
		{
			_hideMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, MeshObjectData.ColorStack[1]);
			HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);
		}
		HideMeshRenderer.enabled = MeshObjectData.ColorStack.Count > 1;
		return true;
	}

	public void SetTranparentWool(bool isTranparent)
	{
		if (MeshObjectData.ColorStack.Count > 1)
		{
			_hideMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, MeshObjectData.ColorStack[1]);
			HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);
		}
		TopMeshRenderer.sharedMaterial = (isTranparent ? TranparentMaterial : MainMaterial);
		TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
	}

	public void WoolRotation()
	{
		if (Singleton<GamePlaySystem>.Instance.IsGoToStore)
		{
			Singleton<GamePlaySystem>.Instance.GoToStore();
		}
		else
		{
			StartCoroutine(AsyncWoolRotation());
		}
	}

	public void SetColor(Color color)
	{
		MeshObjectData.ColorStack.Add(color);
		MeshObjectData.TotalLayer++;
	}

	private IEnumerator AsyncWoolRotation()
	{
		if (Time.deltaTime <= 0f)
		{
			yield return null;
		}
		if (!_isPlayAnim && (!Singleton<GamePlaySystem>.Instance || Singleton<GamePlaySystem>.Instance.QueueCount != Singleton<GamePlaySystem>.Instance.CurrentQueueTargets.Count) && (bool)HideMeshRenderer && (bool)WoolAnimationData && (bool)TopMeshRenderer && _spiralPathUVY != null && _topMaterialPropertyBlock != null && (bool)BoxCollider && MeshObjectData != null && MeshObjectData.ColorStack != null && MeshObjectData.ColorStack.Count != 0 && Singleton<GamePlaySystem>.Instance.OnClickMesh(base.transform, _spiralPath, _currentColor))
		{
			_isPlayAnim = true;
			Singleton<GamePlaySystem>.Instance.ActiveHandController(false);
			Color nextColor = Color.black;
			try
			{
				MeshObjectData.ColorStack.RemoveAt(0);
			}
			catch
			{
			}
			int totalColor = MeshObjectData.ColorStack.Count;
			if (totalColor == 0)
			{
				HideMeshRenderer.enabled = false;
			}
			else
			{
				nextColor = MeshObjectData.ColorStack[0];
				_hideMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, nextColor);
				_hideMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1f);
			}
			HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);
			float totalTime = WoolAnimationData.Duration + WoolAnimationData.OffSet;
			float timer = 0f;
			float minUVY = _spiralPathUVY.Min();
			float maxUVY = _spiralPathUVY.Max();
			float uvRange = Mathf.Max(0.0001f, maxUVY - minUVY);
			MaterialPropertyBlock woolProperties = new MaterialPropertyBlock();
			TopMeshRenderer.GetPropertyBlock(woolProperties);
			while (timer < totalTime)
			{
				float t = timer / totalTime;
				float idx = t * (float)(_spiralPath.Count - 1);
				int idx2 = Mathf.Clamp(Mathf.FloorToInt(idx), 0, _spiralPath.Count - 1);
				int idx3 = Mathf.Clamp(idx2 + 1, 0, _spiralPath.Count - 1);
				float lerpT = idx - (float)idx2;
				float uvy2 = _spiralPathUVY[idx2];
				float uvy3 = _spiralPathUVY[idx3];
				float uvy = Mathf.Lerp(uvy2, uvy3, lerpT);
				float normalizedUVY = (uvy - minUVY) / uvRange;
				woolProperties.SetFloat(T_Utilities.ShaderPropertiesLib.Display, Mathf.Clamp01(normalizedUVY));
				TopMeshRenderer.SetPropertyBlock(woolProperties);
				DecorObjectCheckAlongWoolRotation(t);
				timer += Time.deltaTime;
				yield return null;
			}
			PulseAllDecorObjects();
			if (nextColor != Color.black)
			{
				_topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, nextColor);
				_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1f);
				_currentColor = nextColor;
			}
			TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			_indexLayer--;
			BoxCollider.enabled = totalColor > 0;
			if (totalColor == 1)
			{
				HideMeshRenderer.enabled = false;
			}
			if (totalColor == 0)
			{
				TopMeshRenderer.enabled = false;
			}
			yield return null;
			_isPlayAnim = false;
		}
	}

	public void PLayAnim(int index)
	{
		StartCoroutine(ExecuteAnim(index));
	}

	private IEnumerator ExecuteAnim(int index)
	{
		if (Time.deltaTime <= 0f)
		{
			yield return null;
		}
		if (DecoreControls.Count != 0 || DecoreControls != null)
		{
			int i = 0;
			while (i < DecoreControls.Count && DecoreControls.Count != 0)
			{
				RemovedDecoreControls.Add(DecoreControls[i]);
				DecoreControls[i].UseGravity(true);
				DecoreControls.RemoveAt(i);
				i--;
				i++;
			}
		}
		Color nextColor = Color.black;
		MeshObjectData.ColorStack.RemoveAt(index);
		int totalColor = MeshObjectData.ColorStack.Count;
		if (totalColor == 0)
		{
			HideMeshRenderer.enabled = false;
		}
		else
		{
			nextColor = MeshObjectData.ColorStack[0];
			_hideMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, nextColor);
			_hideMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1f);
		}
		HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);
		float totalTime = WoolAnimationData.Duration + WoolAnimationData.OffSet;
		float timer = 0f;
		float minUVY = _spiralPathUVY.Min();
		float maxUVY = _spiralPathUVY.Max();
		float uvRange = Mathf.Max(0.0001f, maxUVY - minUVY);
		while (timer < totalTime)
		{
			float t = timer / totalTime;
			float idx = t * (float)(_spiralPath.Count - 1);
			int idx2 = Mathf.FloorToInt(idx);
			int idx3 = Mathf.Clamp(idx2 + 1, 0, _spiralPath.Count - 1);
			float lerpT = idx - (float)idx2;
			float uvy2 = _spiralPathUVY[idx2];
			float uvy3 = _spiralPathUVY[idx3];
			float uvy = Mathf.Lerp(uvy2, uvy3, lerpT);
			float normalizedUVY = (uvy - minUVY) / uvRange;
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, Mathf.Clamp01(normalizedUVY));
			TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			timer += Time.deltaTime;
			yield return null;
		}
		if (nextColor != Color.black)
		{
			_topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, nextColor);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1f);
			_currentColor = nextColor;
		}
		TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
		_indexLayer--;
		BoxCollider.enabled = totalColor > 0;
		if (totalColor == 1)
		{
			HideMeshRenderer.enabled = false;
		}
		if (totalColor == 0)
		{
			TopMeshRenderer.enabled = false;
		}
		yield return null;
	}

	private void DisplayColor()
	{
		if (MeshObjectData != null)
		{
			if (_topMaterialPropertyBlock == null)
			{
				_topMaterialPropertyBlock = new MaterialPropertyBlock();
			}
			if (_hideMaterialPropertyBlock == null)
			{
				_hideMaterialPropertyBlock = new MaterialPropertyBlock();
			}
			TopMeshRenderer?.GetPropertyBlock(_topMaterialPropertyBlock);
			HideMeshRenderer?.GetPropertyBlock(_hideMaterialPropertyBlock);
			_topMaterialPropertyBlock?.SetColor(T_Utilities.ShaderPropertiesLib.Color, MeshObjectData.HightestColor);
			_topMaterialPropertyBlock?.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1f);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim, 0f);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 0f);
			TopMeshRenderer?.SetPropertyBlock(_topMaterialPropertyBlock);
			if ((bool)HideMeshRenderer)
			{
				HideMeshRenderer.enabled = true;
			}
			if ((bool)TopMeshRenderer)
			{
				TopMeshRenderer.enabled = true;
			}
		}
	}

	public void DisplayColor(Color albedo)
	{
		if (MeshObjectData != null)
		{
			if (_topMaterialPropertyBlock == null)
			{
				_topMaterialPropertyBlock = new MaterialPropertyBlock();
			}
			TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
			_topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, albedo);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1f);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim, 0f);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 1f);
			TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			if ((bool)TopMeshRenderer)
			{
				TopMeshRenderer.enabled = true;
			}
		}
	}

	public void DisplayColorSmoothly()
	{
		if (MeshObjectData != null)
		{
			if (_topMaterialPropertyBlock == null)
			{
				_topMaterialPropertyBlock = new MaterialPropertyBlock();
			}
			Color currnetColor = Color.grey;
			Color targetColor = MeshObjectData.HightestColor;
			TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1f);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim, 0f);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 1f);
			TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			DOTween.To(() => currnetColor, delegate(Color x)
			{
				currnetColor = x;
				_topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, currnetColor);
				TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			}, targetColor, 1f);
			TopMeshRenderer.enabled = true;
		}
	}

	public void BuildUpModelSmoothly(float duration)
	{
		if (MeshObjectData != null)
		{
			if (_topMaterialPropertyBlock == null)
			{
				_topMaterialPropertyBlock = new MaterialPropertyBlock();
			}
			TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim, 0f);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 0f);
			_topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, Color.gray);
			TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			float currentDisplay = 0f;
			DOTween.To(() => currentDisplay, delegate(float x)
			{
				currentDisplay = x;
				_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, currentDisplay);
				TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			}, 1f, duration);
			TopMeshRenderer.enabled = true;
		}
	}

	public void SetModelShaderEffect(bool useRim, bool useHalo)
	{
		if (MeshObjectData != null)
		{
			if (_topMaterialPropertyBlock == null)
			{
				_topMaterialPropertyBlock = new MaterialPropertyBlock();
			}
			TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim, useRim ? 1 : 0);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, useHalo ? 1 : 0);
			TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			TopMeshRenderer.enabled = true;
		}
	}

	public void ClearThisWool()
	{
		if (MeshObjectData != null)
		{
			if (_topMaterialPropertyBlock == null)
			{
				_topMaterialPropertyBlock = new MaterialPropertyBlock();
			}
			TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim, 0f);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 0f);
			_topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, Color.gray);
			_topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 0f);
			TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
			_hideMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 0f);
			HideMeshRenderer.enabled = false;
			TopMeshRenderer.enabled = true;
		}
	}

	public void HideInnerMesh()
	{
		if (MeshObjectData != null)
		{
			HideMeshRenderer.enabled = false;
		}
	}

	public List<Vector3> GetSpiralPath()
	{
		return _spiralPath;
	}

	public void DecorObjectCheckAlongWoolRotation(float progress)
	{
		if (DecoreControls == null || DecoreControls.Count == 0 || !WoolAnimationData)
		{
			return;
		}
		progress = Mathf.Clamp01(progress);
		List<DecoreControl> decorObjectToDrop = DecoreControls.Where((DecoreControl x) => x.WoolProgressStartDrop <= progress).ToList();
		if (decorObjectToDrop.Count == 0)
		{
			return;
		}
		for (int i = 0; i < decorObjectToDrop.Count; i++)
		{
			if (!(decorObjectToDrop[i] == null))
			{
				Renderer renderer = decorObjectToDrop[i].GetComponent<Renderer>();
				if (renderer == null)
				{
					decorObjectToDrop[i].PulseOutOfParrentWool(WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor);
				}
				else
				{
					decorObjectToDrop[i].PulseOutOfParrentWool(renderer.bounds.center, WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor);
				}
				DecoreControls.Remove(decorObjectToDrop[i]);
				RemovedDecoreControls.Add(decorObjectToDrop[i]);
			}
		}
	}

	public void PulseAllDecorObjects()
	{
		if (DecoreControls.Count == 0)
		{
			return;
		}
		for (int i = 0; i < DecoreControls.Count; i++)
		{
			Renderer renderer = DecoreControls[i].GetComponent<Renderer>();
			if (renderer == null)
			{
				DecoreControls[i].PulseOutOfParrentWool(WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor);
			}
			else
			{
				DecoreControls[i].PulseOutOfParrentWool(renderer.bounds.center, WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor);
			}
		}
	}

	public void ResetWoolState()
	{
		DisplayColor();
		MaterialPropertyBlock woolProperties = new MaterialPropertyBlock();
		TopMeshRenderer.GetPropertyBlock(woolProperties);
		bool flag = false;
		woolProperties.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1f);
		TopMeshRenderer.SetPropertyBlock(woolProperties);
		StartCoroutine(ResetDecorObjects());
	}

	private IEnumerator ResetDecorObjects()
	{
		foreach (DecoreControl decoreControl in RemovedDecoreControls)
		{
			decoreControl.gameObject.SetActive(true);
			yield return null;
			decoreControl.ResetDecorTransformStatusAsync();
		}
	}

	private static bool PointInTriangle(Vector3 p, Vector3 v0_, Vector3 v1_, Vector3 v2_)
	{
		Vector3 v0v1 = v1_ - v0_;
		Vector3 v0v2 = v2_ - v0_;
		Vector3 v0p = p - v0_;
		float d0 = Vector3.Dot(v0v1, v0v1);
		float d = Vector3.Dot(v0v1, v0v2);
		float d2 = Vector3.Dot(v0v2, v0v2);
		float d3 = Vector3.Dot(v0p, v0v1);
		float d4 = Vector3.Dot(v0p, v0v2);
		float denom = d0 * d2 - d * d;
		if (Mathf.Abs(denom) < 1E-06f)
		{
			return false;
		}
		float v = (d2 * d3 - d * d4) / denom;
		float w = (d0 * d4 - d * d3) / denom;
		float u = 1f - v - w;
		return u >= 0f && v >= 0f && w >= 0f;
	}

	private static Vector3 ClosestPointOnSegment(Vector3 p, Vector3 a, Vector3 b)
	{
		Vector3 ab = b - a;
		float t = Vector3.Dot(p - a, ab) / ab.sqrMagnitude;
		t = Mathf.Clamp01(t);
		return a + ab * t;
	}
}
