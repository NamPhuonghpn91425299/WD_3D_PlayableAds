using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnWoolAnimation : MonoBehaviour
{
	public WoolAnimationData WoolAnimationData;

	public LineRenderer LineRenderer;

	private Transform _headParent;

	private Transform _tailParent;

	private List<Vector3> _pointList;

	private MaterialPropertyBlock _propertyBlock;

	private const float HeadOffset = 0.2f;

	private void OnEnable()
	{
		if (LineRenderer == null)
		{
			LineRenderer = GetComponentInChildren<LineRenderer>();
		}
		LineRenderer.enabled = true;
		InitPropertyBlock();
		LineRenderer.positionCount = 2;
	}

	public void SetParent(Transform head, Transform tail)
	{
		_headParent = head;
		_tailParent = tail;
		LineRenderer.enabled = true;
		StartCoroutine(AsyncWoolRotation());
	}

	public void SetColor(Color color)
	{
		if (LineRenderer != null)
		{
			InitPropertyBlock();
			_propertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, color);
			LineRenderer.SetPropertyBlock(_propertyBlock);
		}
	}

	public void SetPoints(List<Vector3> points)
	{
		_pointList = points;
	}

	public void Reset()
	{
		_headParent = null;
		_tailParent = null;
		LineRenderer.enabled = false;
	}

	private IEnumerator AsyncWoolRotation()
	{
		if (!(_headParent == null) && !(_tailParent == null) && _pointList.Count != 0 && (bool)WoolAnimationData)
		{
			UpdateHeadPosition(0f);
			SetDisplay(1f);
			float timer = 0f;
			float duration = WoolAnimationData.Duration;
			while (timer < duration)
			{
				float t = timer / duration;
				UpdateHeadPosition(t);
				UpdateTailPosition(t);
				timer += Time.deltaTime;
				yield return null;
			}
			Transform tailParent = _tailParent;
			List<Vector3> pointList = _pointList;
			Vector3 tailEnd = tailParent.TransformPoint(pointList[pointList.Count - 1]);
			LineRenderer.SetPosition(1, tailEnd);
			float totalTimeHide = WoolAnimationData.DurationHideWool;
			float hideTimer = totalTimeHide;
			while (hideTimer > 0f)
			{
				UpdateHeadPosition(1f);
				SetDisplay(hideTimer / totalTimeHide);
				hideTimer -= Time.deltaTime;
				yield return null;
			}
			Reset();
		}
	}

	private void UpdateHeadPosition(float percent)
	{
		Vector3 headPos;
		if (CameraContainer.Instance != null)
		{
			Vector3 origHeadPos = _headParent.position - _headParent.forward * 0.2f * percent;
			Vector3 screenPos = CameraContainer.Instance.FakeUICamera.WorldToScreenPoint(origHeadPos);
			headPos = CameraContainer.Instance.MainCamera.ScreenToWorldPoint(screenPos);
		}
		else
		{
			headPos = _headParent.position - _headParent.forward * 0.2f;
		}
		LineRenderer.SetPosition(0, headPos);
	}

	private void UpdateTailPosition(float t)
	{
		if (_pointList != null && _pointList.Count != 0 && !(_tailParent == null))
		{
			float idx = t * (float)(_pointList.Count - 1);
			idx = Mathf.Clamp(idx, 0f, _pointList.Count - 1);
			int idx2 = Mathf.FloorToInt(idx);
			int idx3 = Mathf.Clamp(idx2 + 1, 0, _pointList.Count - 1);
			float lerpT = idx - (float)idx2;
			idx2 = Mathf.Clamp(idx2, 0, _pointList.Count - 1);
			idx3 = Mathf.Clamp(idx3, 0, _pointList.Count - 1);
			Vector3 tail0 = _tailParent.TransformPoint(_pointList[idx2]);
			Vector3 tail1 = _tailParent.TransformPoint(_pointList[idx3]);
			Vector3 tailPos = Vector3.Lerp(tail0, tail1, lerpT);
			LineRenderer.SetPosition(1, tailPos);
		}
	}

	private void SetDisplay(float display)
	{
		if (LineRenderer != null)
		{
			InitPropertyBlock();
			_propertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, display);
			LineRenderer.SetPropertyBlock(_propertyBlock);
		}
	}

	private void InitPropertyBlock()
	{
		if (_propertyBlock == null)
		{
			_propertyBlock = new MaterialPropertyBlock();
		}
		LineRenderer.GetPropertyBlock(_propertyBlock);
	}
}
