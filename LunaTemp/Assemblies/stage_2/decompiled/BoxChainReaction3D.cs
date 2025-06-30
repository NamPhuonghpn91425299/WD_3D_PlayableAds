using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BoxChainReaction3D : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Parent container for all box objects")]
	private Transform boxContainer;

	[SerializeField]
	[Tooltip("Prefab used to create boxes (must be a 3D object)")]
	private GameObject boxPrefab;

	[Tooltip("Number of boxes to create at start")]
	public int InitialBoxCount = 4;

	[Header("Animation Settings")]
	[SerializeField]
	[Tooltip("Initial space between boxes (will decrease as boxes are added)")]
	private float initialSpacing = 1.5f;

	[SerializeField]
	[Tooltip("Width of each box")]
	private float boxWidth = 1f;

	[SerializeField]
	[Tooltip("Duration of fly-in animation for new box")]
	private float flyInDuration = 0.5f;

	[SerializeField]
	[Tooltip("Duration of collision/impact animation")]
	private float collisionDuration = 0.2f;

	[SerializeField]
	[Tooltip("Duration of boxes repositioning animation")]
	private float repositionDuration = 0.3f;

	[SerializeField]
	[Tooltip("How far a box moves when hit during collision")]
	private float collisionOffset = 0.5f;

	[SerializeField]
	[Tooltip("Initial position for new boxes (off-screen)")]
	private Vector3 flyInStartPosition = new Vector3(10f, 0f, 0f);

	[Header("Squash & Stretch")]
	[SerializeField]
	[Tooltip("How much to stretch horizontally during fly-in")]
	private float flyInStretch = 1.3f;

	[SerializeField]
	[Tooltip("How much to squash when landing after fly-in")]
	private float landingSquash = 0.7f;

	[SerializeField]
	[Tooltip("How much to squash horizontally when hit")]
	private float collisionSquash = 0.6f;

	[SerializeField]
	[Tooltip("How much to stretch vertically when hit")]
	private float collisionStretch = 1.2f;

	private List<Transform> boxes = new List<Transform>();

	private float initialTotalWidth;

	private Vector3 originalScale;

	public AudioClip boosterWhoosh1Clip;

	public AudioClip boosterImpact1Clip;

	private void Start()
	{
		initialTotalWidth = (float)InitialBoxCount * boxWidth + (float)(InitialBoxCount - 1) * initialSpacing;
		InitializeBoxes();
	}

	private float CalculateDynamicSpacing(int boxCount)
	{
		if (boxCount <= 1)
		{
			return initialSpacing;
		}
		float availableSpace = initialTotalWidth - (float)boxCount * boxWidth;
		return Mathf.Max(0.001f, availableSpace / (float)(boxCount - 1));
	}

	public void InitializeBoxes()
	{
		foreach (Transform box in boxes)
		{
			if (box != null)
			{
				Object.Destroy(box.gameObject);
			}
		}
		boxes.Clear();
		Singleton<GamePlaySystem>.Instance.CurrentQueueTargets.Clear();
		for (int i = 0; i < InitialBoxCount; i++)
		{
			CreateBox();
		}
		RepositionAllBoxes(0f);
	}

	public Transform CreateBox()
	{
		Transform box = Object.Instantiate(boxPrefab, boxContainer).transform;
		originalScale = box.localScale;
		boxes.Add(box);
		QueueTargetControl queueTarget = box.GetComponent<QueueTargetControl>();
		Singleton<GamePlaySystem>.Instance.CurrentQueueTargets.Add(queueTarget);
		return box;
	}

	public IEnumerator AddBoxWithAnimationCoroutine()
	{
		Transform newBox = CreateBox();
		if (boosterWhoosh1Clip != null)
		{
			Singleton<SoundManager>.Instance.PlayOneShot(boosterWhoosh1Clip);
		}
		CalculatePositions(out var oldPositions, out var newPositions);
		newBox.position = flyInStartPosition + new Vector3(0f, 0f, -0.25f);
		newBox.localScale = new Vector3(flyInStretch * originalScale.x, 1f / (flyInStretch * originalScale.y), originalScale.z);
		Sequence flyInSequence = DOTween.Sequence();
		List<Vector3> list = oldPositions;
		flyInSequence.Append(newBox.DOMove(list[list.Count - 1], flyInDuration).SetEase(Ease.OutQuint));
		flyInSequence.Join(newBox.DOScale(new Vector3(landingSquash * originalScale.x, 1f / landingSquash * originalScale.y, landingSquash * originalScale.z), flyInDuration).SetEase(Ease.InOutQuint));
		flyInSequence.Append(newBox.DOScale(originalScale, 0.15f).SetEase(Ease.OutBack));
		yield return flyInSequence.WaitForCompletion();
		yield return StartCoroutine(StartChainReactionCoroutine(oldPositions, newPositions));
	}

	private void CalculatePositions(out List<Vector3> oldPositions, out List<Vector3> newPositions)
	{
		oldPositions = new List<Vector3>();
		newPositions = new List<Vector3>();
		int oldCount = boxes.Count - 1;
		int newCount = boxes.Count;
		Vector3 centerPoint = boxContainer.position;
		float oldSpacing = CalculateDynamicSpacing(oldCount);
		float newSpacing = CalculateDynamicSpacing(newCount);
		float oldTotalWidth = (float)oldCount * boxWidth + (float)(oldCount - 1) * oldSpacing;
		float oldStartX = centerPoint.x - oldTotalWidth / 2f;
		for (int j = 0; j < oldCount; j++)
		{
			float xPos = oldStartX + (float)j * (boxWidth + oldSpacing) + boxWidth / 2f;
			oldPositions.Add(new Vector3(xPos, centerPoint.y, centerPoint.z));
		}
		if (oldCount > 0)
		{
			oldPositions.Add(oldPositions[oldPositions.Count - 1] + new Vector3(boxWidth + oldSpacing, 0f, 0f));
		}
		else
		{
			oldPositions.Add(centerPoint);
		}
		float newTotalWidth = (float)newCount * boxWidth + (float)(newCount - 1) * newSpacing;
		float newStartX = centerPoint.x - newTotalWidth / 2f;
		for (int i = 0; i < newCount; i++)
		{
			float xPos2 = newStartX + (float)i * (boxWidth + newSpacing) + boxWidth / 2f;
			newPositions.Add(new Vector3(xPos2, centerPoint.y, centerPoint.z));
		}
	}

	private void RepositionAllBoxes(float duration)
	{
		int count = boxes.Count;
		if (count == 0)
		{
			return;
		}
		Vector3 centerPoint = boxContainer.position;
		float spacing = CalculateDynamicSpacing(count);
		float totalWidth = (float)count * boxWidth + (float)(count - 1) * spacing;
		float startX = centerPoint.x - totalWidth / 2f;
		for (int i = 0; i < count; i++)
		{
			float xPos = startX + (float)i * (boxWidth + spacing) + boxWidth / 2f;
			if (duration <= 0f)
			{
				boxes[i].position = new Vector3(xPos, centerPoint.y, centerPoint.z);
				boxes[i].localScale = originalScale;
			}
			else
			{
				boxes[i].DOMove(new Vector3(xPos, centerPoint.y, centerPoint.z), duration).SetEase(Ease.OutBack);
				boxes[i].DOScale(originalScale, duration).SetEase(Ease.OutBack);
			}
		}
	}

	private IEnumerator StartChainReactionCoroutine(List<Vector3> oldPositions, List<Vector3> newPositions)
	{
		int lastIndex = boxes.Count - 1;
		Transform boxToMove = boxes[lastIndex];
		boxToMove.position += new Vector3(0f, 0f, -0.25f);
		Vector3 targetPosition = newPositions[lastIndex] + new Vector3(0f, 0f, -0.25f);
		Tween settleTween = boxToMove.DOMove(targetPosition, repositionDuration).SetEase(Ease.OutBack);
		yield return settleTween.WaitForCompletion();
		for (int i = lastIndex - 1; i >= 0; i--)
		{
			if (boosterImpact1Clip != null)
			{
				Singleton<SoundManager>.Instance.PlayOneShot(boosterImpact1Clip);
			}
			Vector3 collisionPos = oldPositions[i] + new Vector3(0f - collisionOffset, 0f, -0.25f);
			Sequence hitSequence = DOTween.Sequence();
			hitSequence.Append(boxes[i].DOMove(collisionPos, collisionDuration).SetEase(Ease.OutQuint));
			hitSequence.Join(boxes[i].DOScale(new Vector3(collisionSquash * originalScale.x, collisionStretch * originalScale.y, collisionSquash * originalScale.z), collisionDuration / 2f).SetEase(Ease.OutQuint));
			yield return hitSequence.WaitForCompletion();
			Sequence returnSequence = DOTween.Sequence();
			returnSequence.Append(boxes[i].DOMove(newPositions[i] + new Vector3(0f, 0f, -0.25f), repositionDuration).SetEase(Ease.OutBack));
			returnSequence.Join(boxes[i].DOScale(originalScale, repositionDuration).SetEase(Ease.OutElastic, 0.5f, 0.3f));
			yield return returnSequence.WaitForCompletion();
		}
		foreach (Transform box in boxes)
		{
			box.position += new Vector3(0f, 0f, 0.25f);
		}
	}

	[ContextMenu("Trigger Animation")]
	public void TriggerAnimation()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		StartCoroutine(AddBoxWithAnimationCoroutine());
	}
}
