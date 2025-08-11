using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using JetBrains.Annotations;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class DecoreControl : MonoBehaviour
{
    #region PROPERTIES

    public Rigidbody    Rigid;
    public MeshRenderer MeshRenderer;
    public Color        _color = Color.black;

    private MaterialPropertyBlock _materialPropertyBlock;

    [SerializeField] private DecoreState _decoreState = DecoreState.None;

    [Range(0f, 1f)] public float WoolProgressStartDrop = 0.5f;

    private Transform _parent;
    private Transform _thisTransform => transform;
    private Vector3 _startScale;
    private Vector3 _startPosition;
    private Vector3 _startLocelEuler;

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

    private void Update()
    {
#if UNITY_EDITOR
        if (_decoreState == DecoreState.OnlyUsePhysic) return;
        SetColor();
#endif
    }
    #endregion

    #region MAIN_METHODS

    #region PULSE DECOR OBJECT

    public void PulseOutOfParrentWool(float forcevalue, float randomDirrectionFactor)
    {
        UseGravity(true);
        //transform.parent.parent -> to get the ObjectSpawner -_-
        Vector3 baseDirection = _thisTransform.forward.normalized;
        try { baseDirection = (Rigid.position - _thisTransform.parent.parent.position).normalized; } catch { }

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
        if(!Rigid) return;
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

    public void UseGravity(bool isUseGravity)
    {
        if (_decoreState == DecoreState.None)
        {
            return;
        }
        if (_decoreState == DecoreState.OnlyUseColor) return;
        Rigid.useGravity  = isUseGravity;
        Rigid.isKinematic = !isUseGravity;
    }
    #endregion

    public void SetColor(Color color) { _color = color; }

    private IEnumerator DisablePhysicComponent()
    {
        yield return new WaitForSeconds(5);
        Rigid.useGravity  = false;
        Rigid.isKinematic = true;
        _thisTransform.SetParent(_parent);
        yield return null;
        gameObject.SetActive(false);
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
            _materialPropertyBlock = new MaterialPropertyBlock();
            MeshRenderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, _color);
            MeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
        catch
        {
        }
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
