using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using static T_Utilities;
[ExecuteAlways]
public class DecoreControl : MonoBehaviour
{
    #region PROPERTIES

    public Rigidbody Rigid;
    public MeshRenderer MeshRenderer;
    public Color _color = Color.black;

    public TypeOfDecore DecoreType = TypeOfDecore.None;

    private Material _runtimeMaterial;

    [SerializeField] private DecoreState _decoreState = DecoreState.None;

    [SerializeField] private bool _executeRotation = false;

    [Range(0f, 1f)] public float WoolProgressStartDrop = 0.5f;

    private Transform _parent;
    private Transform _thisTransform => transform;
    private Vector3 _startScale;
    private Vector3 _startPosition;
    private Vector3 _startLocelEuler;

    [Space]
    public Transform ForceSourceCenter = null;

    [SerializeField]
    private Vector3 _decoreCenterPos;

    private bool _isActive;

    #endregion

    #region UNITY_METHODS
    private void Awake()
    {
        _parent = _thisTransform.parent;
        _startScale = _thisTransform.localScale;
        _startPosition = _thisTransform.localPosition;
        _startLocelEuler = _thisTransform.localEulerAngles;
    }
    private void OnEnable()
    {
        if (_decoreState == DecoreState.OnlyUsePhysic) return;
        SetColor();
    }

    private void OnDisable()
    {
        ResetDecorTransformStatus();
    }

    private void OnDestroy()
    {
        if (Application.isPlaying && _runtimeMaterial != null)
        {
            Destroy(_runtimeMaterial);
            _runtimeMaterial = null;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (MeshRenderer)
            _decoreCenterPos = transform.parent != null
                ? transform.parent.InverseTransformPoint(MeshRenderer.bounds.center)
                : MeshRenderer.bounds.center;
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            if (_decoreState == DecoreState.OnlyUsePhysic) return;
            SetColor();
        }
    }
#else
    private void Update()
    {
    }
#endif
    #endregion

    #region MAIN_METHODS

    #region PULSE DECOR OBJECT

    public void PulseOutOfParrentWool()
    {
        UseGravity(true);
        //transform.parent.parent -> to get the ObjectSpawner -_-
        Vector3 baseDirection = _thisTransform.forward.normalized;
        try
        {
            baseDirection = (Rigid.position - _thisTransform.parent.parent.position).normalized;
        }
        catch
        {
            Debug.Log("Check prefab decor objects: " + gameObject.name);
        }
        Vector3 randomOffset = new Vector3(
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.2f, 0.2f)
            );

        Vector3 finalDirection = (baseDirection + randomOffset).normalized;
        transform.SetParent(null);
        Rigid.AddForce(finalDirection * 2, ForceMode.Impulse);
        if (gameObject.activeSelf && gameObject.activeInHierarchy) StartCoroutine(DisablePhysicComponent());
    }

    public void PulseOutOfParrentWool(float forcevalue, float randomDirrectionFactor)
    {
        UseGravity(true);
        //transform.parent.parent -> to get the ObjectSpawner -_-
        Vector3 baseDirection = _thisTransform.forward.normalized;
        try
        {
            if (ForceSourceCenter != null) baseDirection = (Rigid.position - ForceSourceCenter.position).normalized;
            else baseDirection = (Rigid.position - _thisTransform.parent.parent.position).normalized;
        }
        catch { Debug.Log("Check prefab decor objects: " + gameObject.name); }

        Vector3 randomOffset = new Vector3(
                Random.Range(-randomDirrectionFactor, randomDirrectionFactor),
                Random.Range(-randomDirrectionFactor, randomDirrectionFactor),
                Random.Range(-randomDirrectionFactor, randomDirrectionFactor)
            );

        Vector3 finalDirection = (baseDirection + randomOffset).normalized;
        _thisTransform.SetParent(null);
        Rigid.AddForce(finalDirection * forcevalue, ForceMode.Impulse);
        if (gameObject.activeSelf && gameObject.activeInHierarchy) StartCoroutine(DisablePhysicComponent());
    }

    public void PulseOutOfParrentWool(Vector3 forceSource, float forcevalue, float randomDirrectionFactor)
    {
        if (!Rigid) return;
        UseGravity(true);
        //transform.parent.parent -> to get the ObjectSpawner -_-
        Vector3 baseDirection = _thisTransform.forward.normalized;
        try { baseDirection = (Rigid.position - forceSource).normalized; } catch { }

        Vector3 randomOffset = new Vector3(
                Random.Range(-randomDirrectionFactor, randomDirrectionFactor),
                Random.Range(-randomDirrectionFactor, randomDirrectionFactor),
                Random.Range(-randomDirrectionFactor, randomDirrectionFactor)
            );

        Vector3 finalDirection = (baseDirection + randomOffset).normalized;
        _thisTransform.SetParent(null);
        Rigid.AddForce(finalDirection * forcevalue, ForceMode.Impulse);
        if (gameObject.activeSelf && gameObject.activeInHierarchy) StartCoroutine(DisablePhysicComponent());
    }

    public void PulseOutofParrentWool(float forceValue, float randomDirrectionFactor, float speedRotation)
    {
        UseGravity(true);
        //transform.parent.parent -> to get the ObjectSpawner -_-
        Vector3 baseDirection = _thisTransform.forward.normalized;
        try
        {
            baseDirection = ForceSourceCenter != null
                ? (_decoreCenterPos - ForceSourceCenter.position).normalized
                : (_decoreCenterPos - _thisTransform.parent.position).normalized;
        }
        catch { Debug.Log("Check prefab decor objects: " + gameObject.name); }

        Vector3 randomOffset = new Vector3
            (
                Random.Range(0, randomDirrectionFactor) * baseDirection.x,
                Random.Range(0, randomDirrectionFactor),
                Random.Range(0, randomDirrectionFactor) * baseDirection.z
            );

        Vector3 finalDirection = randomOffset.normalized;
        _thisTransform.SetParent(null);
        Rigid.AddForce(finalDirection * forceValue, ForceMode.Impulse);
        if (gameObject.activeSelf && gameObject.activeInHierarchy)
        {
            StartCoroutine(DisablePhysicComponent());
            if (_executeRotation)
                StartCoroutine(CalculateImpactPoint(randomOffset, speedRotation));
        }
    }

    public void UseGravity(bool isUseGravity)
    {
        if (_decoreState == DecoreState.None)
        {
            return;
        }
        if (_decoreState == DecoreState.OnlyUseColor) return;
        Rigid.useGravity = isUseGravity;
        Rigid.isKinematic = !isUseGravity;
    }
    #endregion

    public void SetColor(Color color) { _color = color; SetColor(); }

    public void SetMaterial(Material material)
    {
        if (MeshRenderer != null)
        {
            MeshRenderer.material = material;
            // Cập nhật lại property block nếu cần hiển thị
            _runtimeMaterial = MeshRenderer.material;
            _runtimeMaterial?.SetFloat(ShaderPropertiesLib.Display, 1);
        }
    }

    public void SetColor(string colorName)
    {
        if (GamePlayManager.Instance != null && GamePlayManager.Instance.colorPalleteData != null)
        {
            SetMaterial(GamePlayManager.Instance.colorPalleteData.GetMaterial(colorName));
        }
    }

    public IEnumerator DisablePhysicComponent()
    {
        yield return new WaitForSeconds(5);
        try
        {
            if (Rigid)
            {
                Rigid.useGravity = false;
                Rigid.isKinematic = true;
            }
            _thisTransform?.SetParent(_parent);
        }
        catch { }
        yield return null;
        gameObject.SetActive(false);
    }

    private IEnumerator CalculateImpactPoint(Vector3 forceVector, float speedRota)
    {
        if (!_executeRotation) yield break;
        var randomPoint = Random.Range(-1f, 1f) * Vector3.one;
        var torQue = Vector3.Cross(randomPoint, forceVector);
        var axis = torQue.normalized;
        var timer = 0f;
        while (timer < 5f)
        {
            if (!transform) break;
            transform.Rotate(axis, speedRota * Time.deltaTime, Space.World);
            timer += Time.deltaTime;
            yield return null;
        }
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
        if (gameObject.activeSelf || gameObject.activeInHierarchy) gameObject.SetActive(true);
        ResetDecorTransformStatus();
    }
    public void ChangeLayer(string layer)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
        foreach (Transform child in _thisTransform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }

#if UNITY_EDITOR
    public void SetUseOnlyColor()
    {
        if (_decoreState == DecoreState.OnlyUsePhysic)
        {
            _decoreState = DecoreState.UseBold;
            return;
        }
        _decoreState = DecoreState.OnlyUseColor;
    }
    public void SetCenterDecore(Transform center) { ForceSourceCenter = center; }
    public void SetUseOnlyPhysic()
    {
        if (_decoreState == DecoreState.OnlyUseColor)
        {
            _decoreState = DecoreState.UseBold;
            return;
        }
        _decoreState = DecoreState.OnlyUsePhysic;
    }


#endif

    #endregion

    #region HELPER

    private void SetColor()
    {
        if (_decoreState == DecoreState.None)
        {
            return;
        }

        try
        {
            EnsureRuntimeMaterial()?.SetColor(ShaderPropertiesLib.Color, _color);
        }
        catch
        {
        }
    }

    private Material EnsureRuntimeMaterial()
    {
        if (MeshRenderer == null) return null;
        if (_runtimeMaterial == null) _runtimeMaterial = MeshRenderer.material;
        return _runtimeMaterial;
    }

    public enum DecoreState
    {
        None,
        OnlyUseColor,
        OnlyUsePhysic,
        UseBold
    }

    #endregion


}

public enum TypeOfDecore
{
    None,
    Wool,
    Wood,
    Stone,
    Metal,
    Plastic,
    Glass,
    Fabric
}
