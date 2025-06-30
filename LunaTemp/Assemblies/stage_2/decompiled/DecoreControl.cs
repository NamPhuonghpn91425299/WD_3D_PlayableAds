using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class DecoreControl : MonoBehaviour
{
	public enum DecoreState
	{
		None,
		OnlyUseColor,
		OnlyUsePhysic,
		UseBold
	}

	public Rigidbody Rigid;

	public MeshRenderer MeshRenderer;

	public Color _color = Color.black;

	private MaterialPropertyBlock _materialPropertyBlock;

	[SerializeField]
	private DecoreState _decoreState = DecoreState.None;

	[Range(0f, 1f)]
	public float WoolProgressStartDrop = 0.5f;

	private Transform _parent;

	private Vector3 _startScale;

	private Vector3 _startPosition;

	private Vector3 _startLocelEuler;

	private Transform _thisTransform => base.transform;

	private void Awake()
	{
		_parent = _thisTransform.parent;
		_startScale = _thisTransform.localScale;
		_startPosition = _thisTransform.localPosition;
		_startLocelEuler = _thisTransform.localEulerAngles;
	}

	private void OnEnable()
	{
		if (_decoreState != DecoreState.OnlyUsePhysic)
		{
			SetColor();
		}
	}

	private void OnDisable()
	{
		ResetDecorTransformStatus();
	}

	private void Update()
	{
	}

	public void PulseOutOfParrentWool(float forcevalue, float randomDirrectionFactor)
	{
		UseGravity(true);
		Vector3 baseDirection = _thisTransform.forward.normalized;
		try
		{
			baseDirection = (Rigid.position - _thisTransform.parent.parent.position).normalized;
		}
		catch
		{
		}
		Vector3 randomOffset = new Vector3(Random.Range(0f - randomDirrectionFactor, randomDirrectionFactor), Random.Range(0f - randomDirrectionFactor, randomDirrectionFactor), Random.Range(0f - randomDirrectionFactor, randomDirrectionFactor));
		Vector3 finalDirection = (baseDirection + randomOffset).normalized;
		_thisTransform.SetParent(null);
		Rigid.AddForce(finalDirection * forcevalue, ForceMode.Impulse);
		if (base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(DisablePhysicComponent());
		}
	}

	public void PulseOutOfParrentWool(Vector3 forceSource, float forcevalue, float randomDirrectionFactor)
	{
		if ((bool)Rigid)
		{
			UseGravity(true);
			Vector3 baseDirection = _thisTransform.forward.normalized;
			try
			{
				baseDirection = (Rigid.position - forceSource).normalized;
			}
			catch
			{
			}
			Vector3 randomOffset = new Vector3(Random.Range(0f - randomDirrectionFactor, randomDirrectionFactor), Random.Range(0f - randomDirrectionFactor, randomDirrectionFactor), Random.Range(0f - randomDirrectionFactor, randomDirrectionFactor));
			Vector3 finalDirection = (baseDirection + randomOffset).normalized;
			_thisTransform.SetParent(null);
			Rigid.AddForce(finalDirection * forcevalue, ForceMode.Impulse);
			if (base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
			{
				StartCoroutine(DisablePhysicComponent());
			}
		}
	}

	public void UseGravity(bool isUseGravity)
	{
		if (_decoreState != 0 && _decoreState != DecoreState.OnlyUseColor)
		{
			Rigid.useGravity = isUseGravity;
			Rigid.isKinematic = !isUseGravity;
		}
	}

	public void SetColor(Color color)
	{
		_color = color;
	}

	private IEnumerator DisablePhysicComponent()
	{
		yield return new WaitForSeconds(5f);
		Rigid.useGravity = false;
		Rigid.isKinematic = true;
		_thisTransform.SetParent(_parent);
		yield return null;
		base.gameObject.SetActive(false);
	}

	public void ResetDecorTransformStatus()
	{
		_thisTransform.localScale = _startScale;
		_thisTransform.localPosition = _startPosition;
		_thisTransform.localEulerAngles = _startLocelEuler;
	}

	public void ResetDecorTransformStatusAsync()
	{
		StopAllCoroutines();
		Rigid.useGravity = false;
		Rigid.isKinematic = true;
		_thisTransform.SetParent(_parent);
		if (base.gameObject.activeSelf || base.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(true);
		}
		ResetDecorTransformStatus();
	}

	public void ChangeLayer(string layer)
	{
		base.gameObject.layer = LayerMask.NameToLayer(layer);
		foreach (Transform child in _thisTransform)
		{
			child.gameObject.layer = LayerMask.NameToLayer(layer);
		}
	}

	private void SetColor()
	{
		if (_decoreState == DecoreState.None)
		{
			return;
		}
		try
		{
			_materialPropertyBlock = new MaterialPropertyBlock();
			MeshRenderer.GetPropertyBlock(_materialPropertyBlock);
			_materialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, _color);
			MeshRenderer.SetPropertyBlock(_materialPropertyBlock);
		}
		catch
		{
		}
	}
}
