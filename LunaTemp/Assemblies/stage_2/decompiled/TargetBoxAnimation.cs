using DG.Tweening;
using UnityEngine;

public class TargetBoxAnimation : MonoBehaviour
{
	[SerializeField]
	private Transform _cap;

	[SerializeField]
	private ParticleSystem _closeParticle;

	[SerializeField]
	private float _boxMoveDistance = 3f;

	[Tooltip("Duration of box movement animation")]
	[SerializeField]
	public float _boxMoveTime = 0.4f;

	[SerializeField]
	private float _capMoveDistance = 1f;

	[SerializeField]
	public float _capMoveTime = 0.3f;

	[SerializeField]
	private float _capScaleTime = 0.15f;

	[SerializeField]
	private float _hopScale = 0.8f;

	[SerializeField]
	public float _hopDownTime = 0.08f;

	[SerializeField]
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

	private readonly string _hopSequenceId = "hop_sequence";

	private readonly string _moveOutSequenceId = "move_out_sequence";

	private readonly string _capSequenceId = "cap_sequence";

	private readonly string _flyInSequenceId = "fly_in_sequence";

	public AudioClip boxWhooshClip;

	[SerializeField]
	private float offsetMoveY = 0.17f;

	public float CloseDuration => _capScaleTime + _capMoveTime + OffsetDuration;

	public float MoveOutDuration
	{
		get
		{
			float hopTime = _hopDownTime + _hopUpTime;
			return hopTime + 0.05f + _boxMoveTime + OffsetDuration;
		}
	}

	public float FlyInDuration => _boxMoveTime + _hopDownTime + _hopUpTime + OffsetDuration;

	private void Awake()
	{
		BakePreLocalScale();
		_boxOriginalLocalPosition = base.transform.localPosition;
		_capOriginalLocalPosition = _cap.localPosition;
		BakePrePos();
	}

	private void OnDestroy()
	{
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
		_boxOriginalLocalPosition.x = base.transform.localPosition.x;
		_capOriginalLocalPosition.x = _cap.localPosition.x;
		_boxUpLocalPosition = new Vector3(_boxOriginalLocalPosition.x, _boxOriginalLocalPosition.y + _boxMoveDistance, _boxOriginalLocalPosition.z);
		_capUpLocalPosition = new Vector3(_capOriginalLocalPosition.x, _capOriginalLocalPosition.y + _capMoveDistance, _capOriginalLocalPosition.z);
	}

	private void BakePreLocalScale()
	{
		_boxOriginalScale = base.transform.localScale;
		_capOriginalScale = _cap.localScale;
	}

	public void Hop(float duration)
	{
		DOTween.Kill(_hopSequenceId);
		base.transform.localScale = _boxOriginalScale;
		_boxSequence = DOTween.Sequence().OnKill(delegate
		{
			base.transform.localScale = _boxOriginalScale;
		});
		_boxSequence.Append(base.transform.DOScale(_boxOriginalScale * _hopScale, duration / 2f).SetEase(Ease.OutQuad)).Append(base.transform.DOScale(_boxOriginalScale, duration / 2f).SetEase(Ease.OutBack)).SetId(_hopSequenceId)
			.OnKill(delegate
			{
				base.transform.localScale = _boxOriginalScale;
			});
	}

	public void CloseAndMoveOut()
	{
		if (_isMovingOut)
		{
			return;
		}
		_isMovingOut = true;
		_cap.gameObject.SetActive(true);
		_cap.localPosition = _capUpLocalPosition;
		_cap.localScale = _capOriginalScale * 0.1f;
		_boxSequence = DOTween.Sequence();
		_capSequence = DOTween.Sequence();
		_capSequence.Append(_cap.DOScale(_capOriginalScale, _capScaleTime).SetEase(Ease.OutBounce)).Append(_cap.DOLocalMove(_capOriginalLocalPosition, _capMoveTime).SetEase(Ease.InQuad)).SetId(_capSequenceId)
			.OnComplete(delegate
			{
				if (_closeParticle != null)
				{
					_closeParticle.Play();
				}
				if (boxWhooshClip != null)
				{
					Singleton<SoundManager>.Instance.PlayOneShotDelayed(boxWhooshClip, 0.5f);
				}
				_boxSequence = DOTween.Sequence();
				_boxSequence.Append(base.transform.DOScale(_boxOriginalScale * _hopScale, (_hopDownTime + _hopUpTime) / 2f).SetEase(Ease.OutQuad)).Append(base.transform.DOScale(_boxOriginalScale, (_hopDownTime + _hopUpTime) / 2f).SetEase(Ease.OutBack)).AppendInterval(0.05f)
					.Append(base.transform.DOLocalMoveY(_boxUpLocalPosition.y, _boxMoveTime).SetEase(Ease.InBack))
					.SetId(_moveOutSequenceId)
					.OnComplete(delegate
					{
						_isMovingOut = false;
					});
			});
	}

	public void FlyIn()
	{
		if (!_isFlyingIn)
		{
			_isFlyingIn = true;
			_cap.gameObject.SetActive(false);
			float downY = _boxOriginalLocalPosition.y;
			base.transform.localPosition = _boxUpLocalPosition;
			_boxSequence = DOTween.Sequence();
			if (boxWhooshClip != null)
			{
				Singleton<SoundManager>.Instance.PlayOneShotDelayed(boxWhooshClip, 0.05f);
			}
			_boxSequence.Append(base.transform.DOLocalMoveY(_boxOriginalLocalPosition.y + offsetMoveY, _boxMoveTime).SetEase(Ease.OutBack)).Append(base.transform.DOLocalMoveY(downY, _hopDownTime).SetEase(Ease.OutQuad)).Append(base.transform.DOLocalMoveY(_boxOriginalLocalPosition.y, _hopUpTime).SetEase(Ease.OutBack))
				.SetId(_flyInSequenceId)
				.OnComplete(delegate
				{
					_isFlyingIn = false;
				});
		}
	}
}
