using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnWoolAnimation : MonoBehaviour
{
    #region PROPERTIES

    public WoolAnimationData WoolAnimationData;
    public LineRenderer      LineRenderer;
    
    private Transform             _headParent;
    private Transform             _tailParent;
    private List<Vector3>         _pointList;
    private MaterialPropertyBlock _propertyBlock;
    private const float           HeadOffset = 0.2f;

    #endregion

    #region UNITY_METHODS

#if UNITY_EDITOR
    private void OnValidate()
    {
        LineRenderer = GetComponentInChildren<LineRenderer>();
    }
#endif

    private void OnEnable()
    {
        if (LineRenderer == null) LineRenderer = GetComponentInChildren<LineRenderer>();
        LineRenderer.enabled = true;
        InitPropertyBlock();
        LineRenderer.positionCount = 2;
    }

    #endregion

    #region MAIN_METHODS

    public void SetParent(Transform head, Transform tail)
    {
        _headParent          = head;
        _tailParent          = tail;
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
        _headParent          = null;
        _tailParent          = null;
        LineRenderer.enabled = false;
    }
    
    private IEnumerator AsyncWoolRotation()
    {
        if (_headParent == null || _tailParent == null || _pointList.Count == 0 || !WoolAnimationData) yield break;
        
        UpdateHeadPosition(0);
        SetDisplay(1f);

        float timer    = 0f;
        float duration = WoolAnimationData.Duration;
        while (timer < duration)
        {
            var t = timer / duration;
            UpdateHeadPosition(t);
            UpdateTailPosition(t);
            
            timer += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo kết thúc ở điểm cuối
        var tailEnd = _tailParent.TransformPoint(_pointList[^1]);
        LineRenderer.SetPosition(1, tailEnd);

        // Fade out animation
        var totalTimeHide = WoolAnimationData.DurationHideWool;
        var hideTimer     = totalTimeHide;
        while (hideTimer > 0)
        {
            UpdateHeadPosition(1);
            SetDisplay(hideTimer / totalTimeHide);
            
            hideTimer -= Time.deltaTime;
            yield return null;
        }

        Reset();
    }

    private void UpdateHeadPosition(float percent)
    {
        Vector3 headPos;
        
        if (CameraContainer.Instance != null)
        {
            // Get head position in world space with offset
            Vector3 origHeadPos = _headParent.position - _headParent.forward * HeadOffset * percent;
            
            // Convert between cameras
            Vector3 screenPos = CameraContainer.Instance.FakeUICamera.WorldToScreenPoint(origHeadPos);
            headPos = CameraContainer.Instance.MainCamera.ScreenToWorldPoint(screenPos);
        }
        else
        {
            // Fallback to original calculation
            headPos = _headParent.position - _headParent.forward * HeadOffset;
        }
        
        LineRenderer.SetPosition(0, headPos);
    }
    
    private void UpdateTailPosition(float t)
    {
        if (_pointList == null || _pointList.Count == 0) return;
        if (_tailParent == null) return;
        // Calculate interpolated position from points list
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
            _propertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, display);
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