using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HandController : MonoBehaviour
{
	[SerializeField]
	private List<Sprite> handSprites = new List<Sprite>();

	[SerializeField]
	private SpriteRenderer handSpriteRenderer;

	[SerializeField]
	private Vector3 positionShow = new Vector3(0.2f, -0.2f, -1.5f);

	[SerializeField]
	private Vector3 positionHide = new Vector3(0.2f, 10f, 0f);

	[SerializeField]
	private float delayTime = 0.5f;

	private Coroutine _playAnimCoroutine;

	private void Start()
	{
		base.transform.position = positionHide;
	}

	private void OnDisable()
	{
		if (_playAnimCoroutine != null)
		{
			StopCoroutine(_playAnimCoroutine);
		}
	}

	public void SetActiveAnim(bool isActive)
	{
		if (_playAnimCoroutine != null)
		{
			StopCoroutine(_playAnimCoroutine);
		}
		if (isActive)
		{
			base.transform.DOMove(positionShow, delayTime).SetEase(Ease.OutQuad);
			_playAnimCoroutine = StartCoroutine(PlayAnim());
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private IEnumerator PlayAnim()
	{
		int index = 0;
		int cound = 2;
		while (true)
		{
			handSpriteRenderer.sprite = handSprites[index];
			yield return new WaitForSeconds(delayTime - 0.2f);
			index++;
			if (index >= cound)
			{
				index = 0;
			}
			yield return null;
		}
	}
}
