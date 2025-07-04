using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Action<Vector2> OnTap;
    public Action<Vector2> OnHold;
    public Action<Vector2> OnDragAction;
    public Action<bool>    OnMouseDown;

    private Vector2 _startPos;
    private Vector2 _lastSwipePos; // NEW: để theo dõi điểm trước đó trong swipe liên tục
    private float   _holdTime;
    private bool    _isHolding;
    private bool    _hasSwiped;

    [Header("Settings")]
    public float HoldThreshold  = 0.3f;   // Giữ bao lâu thì tính là hold
    public  float   SwipeThreshold = 100f; // Vuốt ít nhất bao xa để tính là swipe
    private Vector2 _lastDragPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        _startPos         = eventData.position;
        _lastSwipePos     = _startPos;
        _holdTime         = 0f;
        _isHolding        = true;
        _hasSwiped        = false;
        _lastDragPosition = _startPos;
        OnMouseDown?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isHolding && !_hasSwiped)
        {
            float totalTime = _holdTime;

            if (totalTime < HoldThreshold && Vector3.Distance(_startPos, eventData.position) < SwipeThreshold)
            {
                // Tap
                OnTap?.Invoke(eventData.position);
            }
        }
        OnMouseDown?.Invoke(false);
        _isHolding = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - _lastDragPosition;
        _lastDragPosition = eventData.position;

        OnDragAction?.Invoke(delta);
    }

    private void Update()
    {
        if (_isHolding)
        {
            _holdTime += Time.deltaTime;
            if(_holdTime >= HoldThreshold && Vector2.Distance(_startPos, _lastDragPosition) < SwipeThreshold)
            {
                //Hold
                OnHold?.Invoke(_startPos);
                _isHolding = false;
            }
        }
    }
}
