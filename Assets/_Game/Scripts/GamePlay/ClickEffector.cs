using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ClickEffector : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform targetTransform;
    [SerializeField] private CanvasGroup canvasGroup; // có thể bỏ nếu không dùng UI nữa
    [SerializeField] private Image clickEffectImage;

    [Header("Animation Settings")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private float delay = 0.3f;

    private Coroutine _playAnimCoroutine;
    private RectTransform _targetRectTransform;
    private Canvas _parentCanvas;

    private void Awake()
    {
        _targetRectTransform = targetTransform as RectTransform;
        _parentCanvas = GetComponentInParent<Canvas>();
        StopAnim();
    }

    public void PlayAnim(Action onComplete = null)
    {
        PlayAnimAtScreenPosition(Input.mousePosition, onComplete);
    }

    public void PlayAnimAtScreenPosition(Vector2 screenPosition, Action onComplete = null)
    {
        StopAnim();
        UpdateEffectPosition(screenPosition);
        _playAnimCoroutine = StartCoroutine(PlayAnimCoroutine(onComplete));
    }

    private void UpdateEffectPosition(Vector2 screenPosition)
    {
        if (_targetRectTransform != null && _parentCanvas != null)
        {
            var canvasRect = _parentCanvas.transform as RectTransform;
            var uiCamera = _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _parentCanvas.worldCamera;

            if (canvasRect != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, uiCamera, out var localPoint))
            {
                _targetRectTransform.anchoredPosition = localPoint;
                return;
            }
        }

        Vector3 worldPos = new Vector3(screenPosition.x, screenPosition.y, 10f);
        if (ClickEffectManager.Instance != null && ClickEffectManager.Instance.camMain != null)
        {
            worldPos = ClickEffectManager.Instance.camMain.ScreenToWorldPoint(worldPos);
        }
        targetTransform.position = worldPos;
    }

    private IEnumerator PlayAnimCoroutine(Action onComplete)
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 1;

        targetTransform.localScale = Vector3.one * 0.4f;

        yield return null;

        targetTransform.DOScale(Vector3.one, duration / 2f).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(delay);

        if (canvasGroup != null)
        {
            canvasGroup.DOFade(0, duration).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        }
        else
        {
            onComplete?.Invoke();
        }

        yield return new WaitForSeconds(duration);
        _playAnimCoroutine = null;
    }

    public void StopAnim()
    {
        if (_playAnimCoroutine != null)
        {
            StopCoroutine(_playAnimCoroutine);
            _playAnimCoroutine = null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.DOKill();
            canvasGroup.alpha = 0;
        }
        targetTransform.DOKill();
    }

    public void SetColor(Color color)
    {
        if (clickEffectImage != null)
            clickEffectImage.color = color;
    }
}
