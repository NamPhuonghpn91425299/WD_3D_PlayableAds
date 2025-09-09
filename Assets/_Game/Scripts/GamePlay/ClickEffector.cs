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

    private void Awake()
    {
        StopAnim();
    }

    public void PlayAnim(Action onComplete = null)
    {
        StopAnim();
        _playAnimCoroutine = StartCoroutine(PlayAnimCoroutine(onComplete));
    }

    private IEnumerator PlayAnimCoroutine(Action onComplete)
    {
        // Lấy vị trí chuột trong world space
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f; // khoảng cách tới camera, chỉnh theo nhu cầu
        Vector3 worldPos = ClickEffectManager.Instance.camMain.ScreenToWorldPoint(mouseScreenPos);

        targetTransform.position = worldPos;

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