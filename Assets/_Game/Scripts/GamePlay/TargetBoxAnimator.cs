using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TargetBoxAnimator : MonoBehaviour
{
    [SerializeField]
    private Transform _cap;
    
    [SerializeField]
    private Transform _cube;

    [SerializeField]
    private ParticleSystem _closeParticle;

    [SerializeField]
    private float _boxMoveDistance = 3f;

    [Tooltip("Duration of box movement animation")] [SerializeField]
    public float _boxMoveTime = 0.4f;
    
    [SerializeField]
    private float _boxScale = 1.2f;
    [SerializeField]
    private float _boxAnimTime = 0.5f;
    
    [SerializeField]
    private float _capMoveDistance = 1f;

    [Tooltip("Duration of cap movement animation")] [SerializeField]
    public float _capMoveTime = 0.3f;

    [Tooltip("Duration of cap scale animation")] [SerializeField]
    private float _capScaleTime = 0.15f;

    [SerializeField]
    private float _hopScale = 0.8f;

    [Tooltip("Duration of hop down animation")] [SerializeField]
    public float _hopDownTime = 0.08f;

    [Tooltip("Duration of hop up animation")] [SerializeField]
    public float _hopUpTime = 0.08f;

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
    private readonly string _hopSequenceId = "hop_sequence";
    private readonly string _capSequenceId = "cap_sequence";
    private readonly string _flyInSequenceId = "fly_in_sequence";
    
    // [SerializeField] private SoundSO soundData;
    
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
    
    public void KillAllSequences()
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
        if(_closeandmoveOutSequence != null && _closeandmoveOutSequence.IsActive())
        {
            _closeandmoveOutSequence.Kill();
            _closeandmoveOutSequence = null;
        }
        
        if(_boxSequence_1 != null && _boxSequence_1.IsActive())
        {
            _boxSequence_1.Kill();
            _boxSequence_1 = null;
        }
        
        if(_capSequence_1 != null && _capSequence_1.IsActive())
        {
            _capSequence_1.Kill();
            _capSequence_1 = null;
        }
    }
    
    public void BakePrePos()
    {
        _boxOriginalLocalPosition.x = transform.localPosition.x;
        _capOriginalLocalPosition.x = _cap.localPosition.x;

        _boxUpLocalPosition = new Vector3(_boxOriginalLocalPosition.x, _boxOriginalLocalPosition.y + _boxMoveDistance,
            _boxOriginalLocalPosition.z);
        _capUpLocalPosition = new Vector3(_capOriginalLocalPosition.x, _capOriginalLocalPosition.y + _capMoveDistance,
            _capOriginalLocalPosition.z);
    }

    private void BakePreLocalScale()
    {
        _boxOriginalScale = _cube.localScale;
        _capOriginalScale = _cap.localScale;
    }

    public void Hop(float duration)
    {
        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = false;
#if AI_AGENT
        skipAnimationDelays = DataManager.PlayerData.IsNative;
#endif
        
        if (skipAnimationDelays)
        {
            // Immediately set to final state without animation
            _cube.localScale = _boxOriginalScale;
            return;
        }
        
        // Kill existing hop if running
        DOTween.Kill(_hopSequenceId);
        _boxSequence?.Kill();
        
        _boxSequence = DOTween.Sequence();
        _cube.localScale = _boxOriginalScale;
        
        _boxSequence.Append(_cube.DOScale(_boxOriginalScale * _hopScale, duration / 2).SetEase(Ease.OutQuad))
                    .Append(_cube.DOScale(_boxOriginalScale, duration / 2).SetEase(Ease.OutBack))
                    .SetId(_hopSequenceId)
                    .OnKill(() => {
                        // Ensure box is at its final scale when killed
                        _cube.localScale = _boxOriginalScale;
                    });
        _boxSequence.SetLink(gameObject);
    }
    
    Sequence _closeandmoveOutSequence;
    Sequence _boxSequence_1;
    Sequence _capSequence_1;

    public void CloseAndMoveOut()
    {
        if (_isMovingOut) return;
        _isMovingOut = true;
        
        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = false;
#if AI_AGENT
        skipAnimationDelays = DataManager.PlayerData.IsNative;
#endif
        
        if (skipAnimationDelays)
        {
            // Immediately set to final state without animation
            _cap.gameObject.SetActive(true);
            _cap.localPosition = _capOriginalLocalPosition;
            _cap.localScale = _capOriginalScale;
            _cube.localScale = _boxOriginalScale;
            transform.localPosition = _boxUpLocalPosition;
            
            if (_closeParticle != null)
            {
                _closeParticle.Play();
            }
            
            _isMovingOut = false;
            return;
        }
        
        // Kill existing sequences if running
        // DOTween.Kill(_moveOutSequenceId);
        // DOTween.Kill(_capSequenceId);
        // DOTween.Kill(_boxSequence);
        _boxSequence?.Kill();
        _closeandmoveOutSequence?.Kill();
        _closeandmoveOutSequence = DOTween.Sequence();

        _cap.gameObject.SetActive(true);

        _cap.localPosition = _capUpLocalPosition;
        _cap.localScale    = _capOriginalScale * 0.1f;

        _capSequence_1?.Kill();
        _capSequence_1 = DOTween.Sequence();
        
        // Cap animations
        //if (_isMovingOut == true) SoundManager.Instance.PlayOneShot("box_full");

        _capSequence_1.Append(_cap.DOScale(_capOriginalScale, _capScaleTime).SetEase(Ease.OutBounce))
                    .Append(_cap.DOLocalMove(_capOriginalLocalPosition, _capMoveTime).SetEase(Ease.InQuad))
                    .OnComplete(() =>
                     {
                         if (_closeParticle != null)
                         {
                             _closeParticle.Play();
                             // GameAudioManager.Instance.PlayOneShot("wool_box_done");
                         }
                         //if (soundData.GetAudioClip("box_whoosh") != null) GameAudioManager.Instance.PlayOneShotDelayed(soundData.GetAudioClip("box_whoosh"),0.4f,soundData.GetSoundVolume("box_whoosh"));
                         
                         //if (_isMovingOut == true) SoundManager.Instance.PlayOneShotDelayed("box_whoosh", 0.2f);

                         // Create the box sequence only after cap sequence is 
                         _boxSequence_1?.Kill();
                         _boxSequence_1 = DOTween.Sequence();
                         _boxSequence_1
                             .Append(_cube.DOScaleX(_boxOriginalScale.x * _boxScale, _boxAnimTime)
                                 .SetEase(Ease.OutQuad))
                             .Append(_cube.DOScaleX(_boxOriginalScale.x, _boxAnimTime)
                                 .SetEase(Ease.OutBack))
                             .AppendInterval(0.05f) // Small buffer
                             .Append(transform.DOLocalMoveY(_boxUpLocalPosition.y, _boxMoveTime).SetEase(Ease.InBack))
                             .OnKill(() =>
                             {
                                 _isMovingOut = false;
                             });
                     });
        // _closeandmoveOutSequence.Append(capSequence)
        //                          .Append(boxSequence)
        //                          .OnKill(() =>
        //                           {
        //                               _isMovingOut = false;
        //                           });
        // _closeandmoveOutSequence.SetLink(gameObject);
    }

    [SerializeField] private float offsetMoveY = 0.17f;

    public void FlyIn()
    {
        if (_isFlyingIn) return;
        _isFlyingIn = true;
        
        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = false;
#if AI_AGENT
        skipAnimationDelays = DataManager.PlayerData.IsNative;
#endif
        
        _cap.gameObject.SetActive(false);
        
        if (skipAnimationDelays)
        {
            // Immediately set to final state without animation
            transform.localPosition = _boxOriginalLocalPosition;
            _isFlyingIn = false;
            return;
        }
        
        // Kill existing sequence if running
        // DOTween.Kill(_flyInSequenceId);
        
        float downY = _boxOriginalLocalPosition.y;

        transform.localPosition = _boxUpLocalPosition;
        _boxSequence?.Kill();
        _boxSequence = DOTween.Sequence();
        //if (soundData.GetAudioClip("box_whoosh") != null) GameAudioManager.Instance.PlayOneShotDelayed(soundData.GetAudioClip("box_whoosh"),0.05f,soundData.GetSoundVolume("box_whoosh") );
        
        
        _boxSequence.Append(transform.DOLocalMoveY(_boxOriginalLocalPosition.y + offsetMoveY, _boxMoveTime).SetEase(Ease.OutBack))
                    .Append(transform.DOLocalMoveY(downY,                       _hopDownTime).SetEase(Ease.OutQuad))
                    .Append(transform.DOLocalMoveY(_boxOriginalLocalPosition.y, _hopUpTime).SetEase(Ease.OutBack))
                    .SetId(_flyInSequenceId)
                    .OnKill(() =>
                     {
                         _isFlyingIn = false;
                     });
        _boxSequence.SetLink(gameObject);
    }
    
    public void ResetToDefault()
    {
        KillAllSequences();
        _isMovingOut = false;
        _isFlyingIn  = false;
        if(_cap != null)
            if(_cap.gameObject != null)
                _cap.gameObject.SetActive(false);
        if(_boxOriginalLocalPosition != Vector3.zero)
            transform.localPosition = _boxOriginalLocalPosition;
    }

    public float testOffset;
    public float CloseDuration => _capScaleTime + _capMoveTime + OffsetDuration + testOffset;
    
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
