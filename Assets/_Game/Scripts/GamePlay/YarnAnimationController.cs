using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnAnimationController : MonoBehaviour, IPoolObject
{
    #region PROPERTIES

    public WoolAnimationData WoolAnimationData;
    public LineRenderer      LineRenderer;
    public ZoomCameraData    ZoomCameraData;

    private Transform             _headParent;
    private Transform             _tailParent;
    private List<Vector3>         _pointList;
    private MaterialPropertyBlock _propertyBlock;
    private const float           HeadOffset = 0.2f;

    private Coroutine _rotationCoroutine;

    public GameObject Prefab { get; set; }

    public void OnPushToPool()
    {
        StopRotationCoroutine();
        ResetToDefault();
    }
    #endregion

    #region UNITY_METHODS

    private void Awake()
    {
    }

    private void OnDisable()
    {
        GameEventManager.OnLoadLevelDone -= OnCancelCoroutine;
        StopRotationCoroutine();
    }

#if UNITY_EDITOR
    private void OnValidate() { LineRenderer = GetComponentInChildren<LineRenderer>(); }
#endif

    private void OnEnable()
    {
        if (LineRenderer == null) LineRenderer = GetComponentInChildren<LineRenderer>();
        LineRenderer.enabled = true;
        GameEventManager.OnLoadLevelDone += OnCancelCoroutine;
        InitPropertyBlock();
        LineRenderer.positionCount = 2;
    }

    private void OnDestroy() 
    { 
        GameEventManager.OnLoadLevelDone -= OnCancelCoroutine;
    }

    #endregion

    #region MAIN_METHODS

    public void SetParent(Transform head, Transform tail, bool isRedo = false)
    {
        _headParent = head;
        _tailParent = tail;
        LineRenderer.enabled = true;
        
        StopRotationCoroutine();
        
        if (isRedo)
            _rotationCoroutine = StartCoroutine(AsyncWoolRotationRedoCoroutine());
        else
            _rotationCoroutine = StartCoroutine(AsyncWoolRotationCoroutine());
    }

    private void OnCancelCoroutine()
    {
        StopRotationCoroutine();
        var go = this.gameObject;
        GenericObjectPool.Instance.PushToPool_Object(ref go);
    }

    private void StopRotationCoroutine()
    {
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = null;
        }
    }

    public void SetColor(Material mat)
    {
        if (LineRenderer != null)
        {
            InitPropertyBlock();

            Color aocolor = Color.white;
            Color darkColor = Color.white;
            Color shadowColor = Color.white;
            float saturaion = 1f;
            float brigtness = 3f;
            float diffuse = 0.5f;
            float shadowStrength = 1f;
            float shadowExposure = 1f;
            if (mat != null)
            {
                aocolor = mat.GetColor(ShaderPropertiesLib.AOColor);
                saturaion = mat.GetFloat(ShaderPropertiesLib.Saturation);
                brigtness = mat.GetFloat(ShaderPropertiesLib.Brightness);
                diffuse = mat.GetFloat(ShaderPropertiesLib.DiffusePower);
                darkColor = mat.GetColor(ShaderPropertiesLib.DarkThreadColor);
                shadowColor = mat.GetColor(ShaderPropertiesLib.ShadowColor);
                shadowStrength = mat.GetFloat(ShaderPropertiesLib.ShadowStrength);
                shadowExposure = mat.GetFloat(ShaderPropertiesLib.ShadowExposure);
            }
            LineRenderer.GetPropertyBlock(_propertyBlock);
            Color finalColor = Color.Lerp(mat.color, darkColor, 0.35f);
            _propertyBlock.SetColor(ShaderPropertiesLib.Color, finalColor);
            _propertyBlock.SetColor(ShaderPropertiesLib.AOColor, aocolor);
            _propertyBlock.SetFloat(ShaderPropertiesLib.Saturation, saturaion);
            _propertyBlock.SetFloat(ShaderPropertiesLib.Brightness, brigtness);
            _propertyBlock.SetFloat(ShaderPropertiesLib.DiffusePower, diffuse);
            _propertyBlock.SetColor(ShaderPropertiesLib.ShadowColor, shadowColor);
            _propertyBlock.SetFloat(ShaderPropertiesLib.ShadowStrength, shadowStrength);
            _propertyBlock.SetFloat(ShaderPropertiesLib.ShadowExposure, shadowExposure);
            LineRenderer.SetPropertyBlock(_propertyBlock);
        }
    }

    public void SetPoints(List<Vector3> points) { _pointList = points; }

    public void ResetToDefault()
    {
        _headParent = null;
        _tailParent = null;
        LineRenderer.enabled = false;
        StopRotationCoroutine();
    }

    private IEnumerator AsyncWoolRotationCoroutine()
    {
        if (_headParent == null || _tailParent == null || _pointList == null || _pointList.Count == 0 || !WoolAnimationData) yield break;

        UpdateHeadPosition(0);
        SetDisplay(1f);

        bool skipAnimationDelays = false;
#if AI_AGENT
        skipAnimationDelays = DataManager.PlayerData.IsNative;
#endif

        float timer = 0f;
        float duration = WoolAnimationData.Duration;
        while (timer < duration)
        {
            var t = timer / duration;
            UpdateHeadPosition(t);
            UpdateTailPosition(t);

            timer += Time.deltaTime;
            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
                timer = duration;
            }
        }

        if (_headParent == null || _tailParent == null) yield break;
        var tailEnd = _tailParent.TransformPoint(_pointList[^1]);
        LineRenderer.SetPosition(1, tailEnd);

        var totalTimeHide = WoolAnimationData.DurationHideWool;
        var hideTimer = totalTimeHide;
        while (hideTimer > 0)
        {
            UpdateHeadPosition(1);
            SetDisplay(hideTimer / totalTimeHide);

            hideTimer = Mathf.Clamp(hideTimer - Time.deltaTime, 0, totalTimeHide);
            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
                hideTimer = 0;
            }
        }

        ResetToDefault();
        var go = gameObject;
        GenericObjectPool.Instance.PushToPool_Object(ref go);
    }

    private IEnumerator AsyncWoolRotationRedoCoroutine()
    {
        if (_headParent == null || _tailParent == null || _pointList == null || _pointList.Count == 0 || !WoolAnimationData) yield break;

        UpdateHeadPositionRedo(0);
        SetDisplay(1f);

        bool skipAnimationDelays = false;
#if AI_AGENT
        skipAnimationDelays = DataManager.PlayerData.IsNative;
#endif

        float timer = 0f;
        float duration = WoolAnimationData.Duration;
        while (timer < duration)
        {
            var t = 1 - timer / duration;
            UpdateHeadPositionRedo(t);
            UpdateTailPositionRedo(t);

            timer += Time.deltaTime;
            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
                timer = duration;
            }
        }
        if (_headParent == null || _tailParent == null) yield break;
        var tailEnd = _headParent.TransformPoint(_pointList[0]);
        LineRenderer.SetPosition(0, tailEnd);

        var totalTimeHide = WoolAnimationData.DurationHideWool;
        var hideTimer = totalTimeHide;
        while (hideTimer > 0)
        {
            UpdateHeadPositionRedo(0);
            SetDisplay(hideTimer / totalTimeHide);

            hideTimer -= Time.deltaTime;
            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
            }
        }
        ResetToDefault();
        var go = gameObject;
        GenericObjectPool.Instance.PushToPool_Object(ref go);
    }

    private void UpdateHeadPositionRedo(float t)
    {
        if (_pointList == null || _pointList.Count == 0) return;
        if (_headParent == null) return;
        float idx = t * (_pointList.Count - 1);
        idx = Mathf.Clamp(idx, 0, _pointList.Count - 1);
        int idx0 = Mathf.FloorToInt(idx);
        int idx1 = Mathf.Clamp(idx0 + 1, 0, _pointList.Count - 1);
        float lerpT = idx - idx0;

        idx0 = Mathf.Clamp(idx0, 0, _pointList.Count - 1);
        idx1 = Mathf.Clamp(idx1, 0, _pointList.Count - 1);
        Vector3 tail0 = _headParent.TransformPoint(_pointList[idx0]);
        Vector3 tail1 = _headParent.TransformPoint(_pointList[idx1]);
        Vector3 tailPos = Vector3.Lerp(tail0, tail1, lerpT);

        LineRenderer.SetPosition(0, tailPos);
    }

    private void UpdateTailPositionRedo(float percent)
    {
        Vector3 headPos;

        if (_headParent == null || _tailParent == null) return;
        if (CameraContainer.Instance != null && CameraContainer.Instance.FakeUICamera != null && CameraContainer.Instance.MainCamera != null)
        {
            Vector3 origHeadPos = _tailParent.position - _tailParent.forward * HeadOffset * percent;
            Vector3 screenPos = CameraContainer.Instance.FakeUICamera.WorldToScreenPoint(origHeadPos);
            headPos = CameraContainer.Instance.MainCamera.ScreenToWorldPoint(screenPos);
        }
        else
        {
            headPos = _tailParent.position - _tailParent.forward * HeadOffset;
        }

        LineRenderer.SetPosition(1, headPos);
    }

    private void UpdateHeadPosition(float percent)
    {
        Vector3 headPos;

        if (CameraContainer.Instance != null && CameraContainer.Instance.FakeUICamera != null && CameraContainer.Instance.MainCamera != null)
        {
            if (_headParent == null || _tailParent == null) return;
            Vector3 origHeadPos = _headParent.position - _headParent.forward * HeadOffset * percent;
            Vector3 screenPos = CameraContainer.Instance.FakeUICamera.WorldToScreenPoint(origHeadPos);
            headPos = CameraContainer.Instance.MainCamera.ScreenToWorldPoint(screenPos);
        }
        else
        {
            if (_headParent == null) return;
            headPos = _headParent.position - _headParent.forward * HeadOffset;
        }

        LineRenderer.SetPosition(0, headPos);
    }

    private void UpdateTailPosition(float t)
    {
        if (_pointList == null || _pointList.Count == 0) return;
        if (_tailParent == null) return;
        float idx = t * (_pointList.Count - 1);
        idx = Mathf.Clamp(idx, 0, _pointList.Count - 1);
        int idx0 = Mathf.FloorToInt(idx);
        int idx1 = Mathf.Clamp(idx0 + 1, 0, _pointList.Count - 1);
        float lerpT = idx - idx0;

        idx0 = Mathf.Clamp(idx0, 0, _pointList.Count - 1);
        idx1 = Mathf.Clamp(idx1, 0, _pointList.Count - 1);
        Vector3 tail0 = _tailParent.TransformPoint(_pointList[idx0]);
        Vector3 tail1 = _tailParent.TransformPoint(_pointList[idx1]);
        Vector3 tailPos = Vector3.Lerp(tail0, tail1, lerpT);

        LineRenderer.SetPosition(1, tailPos);
    }

    private void SetDisplay(float display)
    {
        if (LineRenderer != null)
        {
            InitPropertyBlock();
            _propertyBlock.SetFloat(ShaderPropertiesLib.Display, display);
            LineRenderer.SetPropertyBlock(_propertyBlock);
        }
    }

    private void InitPropertyBlock()
    {
        _propertyBlock ??= new MaterialPropertyBlock();
        LineRenderer.GetPropertyBlock(_propertyBlock);
    }

    #endregion
}
