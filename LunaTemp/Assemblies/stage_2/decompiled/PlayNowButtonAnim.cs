using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayNowButtonAnim : MonoBehaviour
{
	[SerializeField]
	private Button playerNowButton;

	[SerializeField]
	private Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);

	[SerializeField]
	private Vector3 minScale = new Vector3(1f, 1f, 1f);

	[SerializeField]
	private float scaleDuration = 0.5f;

	private void Start()
	{
		playerNowButton.onClick.AddListener(GotoStore);
		StartScalingAnimation();
	}

	private void OnDestroy()
	{
		playerNowButton.onClick.RemoveListener(GotoStore);
	}

	public void GotoStore()
	{
		Singleton<GamePlaySystem>.Instance.GoToStore();
	}

	private void StartScalingAnimation()
	{
		playerNowButton.transform.DOScale(maxScale, scaleDuration).SetEase(Ease.InOutSine).OnComplete(delegate
		{
			playerNowButton.transform.DOScale(minScale, scaleDuration).SetEase(Ease.InOutSine).OnComplete(StartScalingAnimation);
		});
	}
}
