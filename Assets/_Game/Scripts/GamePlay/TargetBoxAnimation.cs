using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TargetBoxAnimation : MonoBehaviour
{
    [SerializeField] private Transform      _cap;
    [SerializeField] private ParticleSystem _closeParticle;

    [SerializeField] private float _boxMoveDistance = 3f;

    [Tooltip("Duration of box movement animation")] [SerializeField] public float _boxMoveTime = 0.4f;

    [SerializeField] private float _capMoveDistance = 1f;

    [SerializeField] public float _capMoveTime = 0.3f;

    [SerializeField] private float _capScaleTime = 0.15f;

    [SerializeField] private float _hopScale = 0.8f;

    [SerializeField] public float _hopDownTime = 0.08f;

    [SerializeField] public float _hopUpTime = 0.08f;

    private Vector3 _boxOriginalScale;
    private Vector3 _capOriginalScale;

    private Vector3 _boxOriginalLocalPosition;
    private Vector3 _capOriginalLocalPosition;

    private Vector3 _boxUpLocalPosition;
    private Vector3 _capUpLocalPosition;

    private Sequence _boxSequence;
    private Sequence _capSequence;

    private bool _isMovingOut;
    private bool _isFlyingIn;

    private readonly float OffsetDuration = 0.1f;

    // Unique IDs for different sequences
    private readonly string _hopSequenceId     = "hop_sequence";
    private readonly string _moveOutSequenceId = "move_out_sequence";
    private readonly string _capSequenceId     = "cap_sequence";
    private readonly string _flyInSequenceId   = "fly_in_sequence";

    //[SerializeField] private SoundSO   soundData;
    public                   AudioClip boxWhooshClip;
    private void Awake()
    {
        BakePreLocalScale();
        _boxOriginalLocalPosition = transform.localPosition;
        _capOriginalLocalPosition = _cap.localPosition;
        BakePrePos();
    }

    private void OnDestroy()
    {
        // Kill all sequences when object is destroyed
        KillAllSequences();
    }

    private void KillAllSequences()
    {
        if (_boxSequence != null && _boxSequence.IsActive())
        {
            _boxSequence.Kill();
            _boxSequence = null;
        }

        if (_capSequence != null && _capSequence.IsActive())
        {
            _capSequence.Kill();
            _capSequence = null;
        }
    }

    public void BakePrePos()
    {
        _boxOriginalLocalPosition.x = transform.localPosition.x;
        _capOriginalLocalPosition.x = _cap.localPosition.x;

        _boxUpLocalPosition = new Vector3(_boxOriginalLocalPosition.x, _boxOriginalLocalPosition.y + _boxMoveDistance,
                _boxOriginalLocalPosition.z
            );
        _capUpLocalPosition = new Vector3(_capOriginalLocalPosition.x, _capOriginalLocalPosition.y + _capMoveDistance,
                _capOriginalLocalPosition.z
            );
    }

    private void BakePreLocalScale()
    {
        _boxOriginalScale = transform.localScale;
        _capOriginalScale = _cap.localScale;
    }

    public void Hop(float duration)
    {
        // Kill existing hop if running
        DOTween.Kill(_hopSequenceId);

        transform.localScale = _boxOriginalScale;

        _boxSequence = DOTween
           .Sequence()
           .OnKill(() =>
                    {
                        transform.localScale = _boxOriginalScale;
                    }
                );
        ;
        _boxSequence
           .Append(transform
                   .DOScale(_boxOriginalScale * _hopScale, duration / 2)
                   .SetEase(Ease.OutQuad)
                )
           .Append(transform
                   .DOScale(_boxOriginalScale, duration / 2)
                   .SetEase(Ease.OutBack)
                )
           .SetId(_hopSequenceId)
           .OnKill(() =>
                    {
                        // Ensure box is at its final scale when killed
                        transform.localScale = _boxOriginalScale;
                    }
                );
    }

    public void CloseAndMoveOut()
    {
        if (_isMovingOut) return;
        _isMovingOut = true;
        // Kill existing sequences if running
        // DOTween.Kill(_moveOutSequenceId);
        // DOTween.Kill(_capSequenceId);

        _cap.gameObject.SetActive(true);

        _cap.localPosition = _capUpLocalPosition;
        _cap.localScale    = _capOriginalScale * 0.1f;

        _boxSequence = DOTween.Sequence();

        _capSequence = DOTween.Sequence();

        // Cap animations
        _capSequence
           .Append(_cap
                   .DOScale(_capOriginalScale, _capScaleTime)
                   .SetEase(Ease.OutBounce)
                )
           .Append(_cap
                   .DOLocalMove(_capOriginalLocalPosition, _capMoveTime)
                   .SetEase(Ease.InQuad)
                )
           .SetId(_capSequenceId)
           .OnComplete(() =>
                    {
                        if (_closeParticle != null) _closeParticle.Play();
                        if (boxWhooshClip != null) SoundManager.Instance.PlayOneShotDelayed(boxWhooshClip, 0.5f, 1);
                        // Create the box sequence only after cap sequence is complete
                        _boxSequence = DOTween.Sequence();
                        _boxSequence
                           .Append(transform
                                   .DOScale(_boxOriginalScale * _hopScale, (_hopDownTime + _hopUpTime) / 2)
                                   .SetEase(Ease.OutQuad)
                                )
                           .Append(transform
                                   .DOScale(_boxOriginalScale, (_hopDownTime + _hopUpTime) / 2)
                                   .SetEase(Ease.OutBack)
                                )
                           .AppendInterval(0.05f) // Small buffer
                           .Append(transform
                                   .DOLocalMoveY(_boxUpLocalPosition.y, _boxMoveTime)
                                   .SetEase(Ease.InBack)
                                )
                           .SetId(_moveOutSequenceId)
                           .OnComplete(() =>
                                    {
                                        _isMovingOut = false;
                                    }
                                );
                    }
                );
    }

    [SerializeField] private float offsetMoveY = 0.17f;

    public void FlyIn()
    {
        if (_isFlyingIn) return;
        _isFlyingIn = true;

        // Kill existing sequence if running
        // DOTween.Kill(_flyInSequenceId);

        _cap.gameObject.SetActive(false);
        float downY = _boxOriginalLocalPosition.y;

        transform.localPosition = _boxUpLocalPosition;

        _boxSequence = DOTween.Sequence();
        if (boxWhooshClip != null) SoundManager.Instance.PlayOneShotDelayed(boxWhooshClip, 0.05f, 1);
        _boxSequence
           .Append(transform
                   .DOLocalMoveY(_boxOriginalLocalPosition.y + offsetMoveY, _boxMoveTime)
                   .SetEase(Ease.OutBack)
                )
           .Append(transform
                   .DOLocalMoveY(downY,                       _hopDownTime)
                   .SetEase(Ease.OutQuad)
                )
           .Append(transform
                   .DOLocalMoveY(_boxOriginalLocalPosition.y, _hopUpTime)
                   .SetEase(Ease.OutBack)
                )
           .SetId(_flyInSequenceId)
           .OnComplete(() =>
                    {
                        _isFlyingIn = false;
                    }
                );
    }

    public float CloseDuration => _capScaleTime + _capMoveTime + OffsetDuration;

    public float MoveOutDuration
    {
        get
        {
            float hopTime     = _hopDownTime + _hopUpTime;
            float boxAnimTime = hopTime      + 0.05f + _boxMoveTime + OffsetDuration;

            return boxAnimTime;
        }
    }

    /// <summary>
    /// Returns the total duration of the FlyIn animation sequence in seconds
    /// </summary>
    public float FlyInDuration => _boxMoveTime + _hopDownTime + _hopUpTime + OffsetDuration;
}
