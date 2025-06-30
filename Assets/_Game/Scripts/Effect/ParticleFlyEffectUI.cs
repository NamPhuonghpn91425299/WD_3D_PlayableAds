using System;
using DG.Tweening;
using UnityEngine;

public class ParticleFlyEffectUI : MonoBehaviour
{
    [SerializeField] private Transform      _start;
    [SerializeField] private RectTransform  _end;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float          _duration = 1f;
    [SerializeField] private Camera         _uiCamera; // Camera rendering the canvas

    private Canvas        _canvas;
    private RectTransform _canvasRect;
    private RectTransform _rectTransform;

    private Vector3 _endPos;
    private Vector3 _startPos;

    public void SetStart(Transform start)
    {
        _start = start;
        if (_canvas != null)
        {
            _startPos = WorldToCanvasPosition(_start.position);
        }
    }

    public void SetEnd(RectTransform end)
    {
        _end    = end;
        _endPos = GetWorldPositionFromRectTransform(_end);
    }

    public void Init()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        // Get the canvas containing the end RectTransform
        if (_end != null)
        {
            _canvas = _end.GetComponentInParent<Canvas>();
            if (_canvas != null)
            {
                _canvasRect = _canvas.GetComponent<RectTransform>();

                // If camera not assigned, try to get it from the canvas
                if (_uiCamera == null && _canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    _uiCamera = CameraContainer.Instance.FakeUICamera;
                }
            }
        }

        if (_uiCamera == null)
        {
            _uiCamera = Camera.main;
        }
        
        // Calculate start position if start is already set
        if (_start != null)
        {
            _startPos = WorldToCanvasPosition(_start.position);
        }
    }

    private Vector3 WorldToCanvasPosition(Vector3 worldPosition)
    {
        if (_canvas == null) return worldPosition;

        Vector3 screenPos;
        Vector3 canvasPos;

        switch (_canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                screenPos = Camera.main.WorldToScreenPoint(worldPosition);
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPos, null, out var localPoint))
                {
                    canvasPos = _canvasRect.TransformPoint(localPoint);
                    return canvasPos;
                }
                break;

            case RenderMode.ScreenSpaceCamera:
                screenPos = _uiCamera.WorldToScreenPoint(worldPosition);
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPos, _uiCamera, out var localPointCamera))
                {
                    canvasPos = _canvasRect.TransformPoint(localPointCamera);
                    return canvasPos;
                }
                break;

            case RenderMode.WorldSpace:
                return worldPosition;
        }

        return worldPosition;
    }

    private Vector3 GetWorldPositionFromRectTransform(RectTransform rectTransform)
    {
        if (_canvas == null) return rectTransform.position;

        Vector3 position = rectTransform.position;

        switch (_canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                position   = RectTransformUtility.PixelAdjustPoint(position, rectTransform, _canvas);
                position.z = _start != null ? _start.position.z : position.z;
                return position;

            case RenderMode.ScreenSpaceCamera:
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect,
                    RectTransformUtility.WorldToScreenPoint(_uiCamera, rectTransform.position),
                    _uiCamera, out var screenPoint);

                Vector3 worldPos = _uiCamera.ScreenToWorldPoint(new Vector3(
                    screenPoint.x + _canvasRect.rect.width  / 2,
                    screenPoint.y + _canvasRect.rect.height / 2,
                    _canvas.planeDistance));

                return worldPos;

            case RenderMode.WorldSpace:
                return rectTransform.position;
        }

        return rectTransform.position;
    }

    public void Play()
    {
        // Ensure we have a valid start position
        if (_start != null)
        {
            _startPos = WorldToCanvasPosition(_start.position);
        }
        
        // Set the RectTransform position in canvas space
        _rectTransform.position = _startPos;
        _particleSystem.Play(true);

        transform.DOMove(_endPos, _duration)
                 .SetEase(Ease.Linear)
                 .OnComplete(_particleSystem.Stop);
    }

    public float FlyDuration => _duration;
}