using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimCubeQueuePreLose : SingletonBase<AnimCubeQueuePreLose>
{
    [SerializeField] private AudioClip soundFx;
    public float queueTargetScaleValue = 2.0f;
    public float queueDuration = 0.5f;
    public Ease queueEase = Ease.OutBack;

    public float cubeTargetScaleValue = 0.9f;
    public float cubeDuration = 0.5f;
    public Ease cubeEase = Ease.OutBack;

    private Transform _lastQueueTarget;
    private Sequence _queueTargetSequence;
    public float queueTargetInterval = 1f;

    private Vector3 _cubeOriginalScale;
    private Vector3 _iconPlusOriginalScale;

    public override void Awake()
    {
        base.Awake();
        //return;
        GameEventManager.PlayAnimPreLose += PlayAnimPreLose;
        GameEventManager.ReplayAnimHole += ReplayAnimHole;
        OnChangeGameState();
    }

    private void OnDestroy()
    {
        GameEventManager.PlayAnimPreLose -= PlayAnimPreLose;
        GameEventManager.ReplayAnimHole -= ReplayAnimHole;

    }

    private void OnChangeGameState()
    {
        var queueTargets = GamePlayManager.Instance?.CurrentQueueTargets;
        if (queueTargets == null || queueTargets.Count == 0)
        {
            _cubeOriginalScale = new Vector3(1, 0.9f, 1);
            _iconPlusOriginalScale = new Vector3(0.35f, 0.35f, 0.35f);
            return;
        }

        // Kiểm tra object đầu tiên có hợp lệ không trước khi truy cập transform
        if (queueTargets[0] != null)
        {
            _cubeOriginalScale = queueTargets[0].transform.localScale;
        }
        else
        {
            _cubeOriginalScale = new Vector3(1, 0.9f, 1);
        }

        // Kiểm tra object cuối cùng và con của nó có hợp lệ không
        var lastQueue = queueTargets[queueTargets.Count - 1];
        if (lastQueue != null && lastQueue.transform.childCount > 1)
        {
            var iconPlus = lastQueue.transform.GetChild(1);
            if (iconPlus != null)
            {
                _iconPlusOriginalScale = iconPlus.localScale;
            }
            else
            {
                _iconPlusOriginalScale = new Vector3(0.35f, 0.35f, 0.35f);
            }
        }
        else
        {
            _iconPlusOriginalScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
    }

    private void ReplayAnimHole()
    {
        if (!_isAnimPlaying) return;
        PlayAnimQueueColor();
    }

    private bool _isAnimPlaying = false;

    private void PlayAnimPreLose(bool isShow)
    {
        if (gameObject.activeInHierarchy == false) return;
        if (_isAnimPlaying == isShow) return;
        _isAnimPlaying = isShow;
        if (isShow)
        {
            StartCoroutine(PlayAnimationAsync());
        }
        else
        {
            StopAnimation();
        }
    }

    private void StopAnimation()
    {
        _mainSequence?.Kill();
        StopAnimQueueTarget();
        StopAnimCubeTarget();
        StopAnimQueueColor();
    }

    private Sequence _mainSequence;
    private IEnumerator PlayAnimationAsync()
    {
        yield return null;
        if (!_isAnimPlaying) yield break;
        _mainSequence?.Kill();
        PlayAnimQueueTarget();
        PlayAnimCubeTarget();
        PlayAnimQueueColor();
        _mainSequence = DOTween.Sequence();
        _mainSequence.AppendInterval(queueTargetInterval);
        _mainSequence.Join(_cubeTargetSequence);
        _mainSequence.Join(_queueTargetSequence);
        _mainSequence.SetLoops(-1, LoopType.Restart);
        _mainSequence.Play();
        _queueTargetColorSequence.Play();
        SoundManager.Instance.PlayOneShot(soundFx);
    }

    public void PlayAnimQueueTarget()
    {
        if (GamePlayManager.Instance.IsQueueTargetOpenAll()) return;
        StopAnimQueueTarget();
        _queueTargetSequence = DOTween.Sequence();

        var queueTargets = GamePlayManager.Instance?.CurrentQueueTargets;
        if (queueTargets == null || queueTargets.Count == 0) return;

        var lastTargetObj = queueTargets[^1];
        if (lastTargetObj == null || lastTargetObj.transform.childCount < 2) return;

        _lastQueueTarget = lastTargetObj.transform.GetChild(1).transform;
        if (!_lastQueueTarget) return;
        _queueTargetSequence.Append(_lastQueueTarget.DOScale(_iconPlusOriginalScale, queueDuration / 4f).SetEase(queueEase));
        _queueTargetSequence.Append(_lastQueueTarget.DOScale(_iconPlusOriginalScale * queueTargetScaleValue, queueDuration / 4f));
        _queueTargetSequence.Append(_lastQueueTarget.DOScale(_iconPlusOriginalScale, queueDuration / 4f).SetEase(queueEase));
        _queueTargetSequence.Append(_lastQueueTarget.DOScale(_iconPlusOriginalScale * queueTargetScaleValue, queueDuration / 4f));
    }
    public void StopAnimQueueTarget()
    {
        if (!_lastQueueTarget) return;
        _queueTargetSequence?.Kill();
        _lastQueueTarget.localScale = _iconPlusOriginalScale;
        _lastQueueTarget = null;
    }


    private Sequence _cubeTargetSequence;
    private List<Transform> _lastCubeTarget = new();
    public void PlayAnimCubeTarget()
    {
        if (GamePlayManager.Instance?.IsQueueTargetFull() ?? true) return;
        StopAnimCubeTarget();
        _cubeTargetSequence = DOTween.Sequence();
        var cubeTargets = GamePlayManager.Instance?.CurrentCubeTargets;
        if (cubeTargets == null) return;

        for (int i = 0; i < cubeTargets.Count; i++)
        {
            var cubeTarget = cubeTargets[i];
            if (cubeTarget == null) continue; // Kiểm tra object hợp lệ
            if (cubeTarget.IsActive) continue;
            var lastCubeTarget = cubeTarget.transform;
            var subSequence = DOTween.Sequence();
            subSequence.Append(lastCubeTarget.DOScale(cubeTargetScaleValue, cubeDuration / 4f).SetEase(cubeEase));
            subSequence.Append(lastCubeTarget.DOScale(_cubeOriginalScale, cubeDuration / 4f));
            subSequence.Append(lastCubeTarget.DOScale(cubeTargetScaleValue, cubeDuration / 4f).SetEase(cubeEase));
            subSequence.Append(lastCubeTarget.DOScale(_cubeOriginalScale, cubeDuration / 4f));
            _lastCubeTarget.Add(lastCubeTarget);
            _cubeTargetSequence.Join(subSequence);
        }
    }
    public void StopAnimCubeTarget()
    {
        if (_lastCubeTarget.Count == 0) return;
        _cubeTargetSequence?.Kill();
        for (var i = 0; i < _lastCubeTarget.Count; i++)
        {
            _lastCubeTarget[i].localScale = _cubeOriginalScale;
        }
        _lastCubeTarget.Clear();
    }

    private int s_HashProgress = Shader.PropertyToID("_Progress");
    private Tween _queueTargetColorSequence;
    MaterialPropertyBlock _propertyBlock;
    Renderer _lastQueueTargetRenderer;
    public void PlayAnimQueueColor()
    {
        if (!GamePlayManager.Instance || GamePlayManager.Instance.IsQueueTargetFull()) return;
        StopAnimQueueColor();
        var emptyQueue = GamePlayManager.Instance.GetEmptyQueueTarget();
        if (emptyQueue == null) return; // Kiểm tra object hợp lệ
        _lastQueueTargetRenderer = emptyQueue.Renderer;
        if (!_lastQueueTargetRenderer) return;
        _propertyBlock = new MaterialPropertyBlock();
        _lastQueueTargetRenderer.GetPropertyBlock(_propertyBlock);
        _queueTargetColorSequence = DOTween.To(() => 0f, (x) =>
        {
            if (!_lastQueueTargetRenderer) return; // Kiểm tra thêm trong tween callback
            _propertyBlock.SetFloat(s_HashProgress, x);
            _lastQueueTargetRenderer.SetPropertyBlock(_propertyBlock);
        }, 0.8f, queueDuration).SetLoops(-1, LoopType.Yoyo);
    }
    public void StopAnimQueueColor()
    {
        _queueTargetColorSequence?.Kill();
        if (_propertyBlock == null || !_lastQueueTargetRenderer) return;
        _lastQueueTargetRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat(s_HashProgress, 0f);
        _lastQueueTargetRenderer.SetPropertyBlock(_propertyBlock);
    }
}
