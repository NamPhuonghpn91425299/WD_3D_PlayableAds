using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RollWoolAnimation : MonoBehaviour
{
	public AudioClip woolClip1;

	public AudioClip woolClip2;

	public WoolAnimationData WoolAnimationData;

	public List<MeshRenderer> MeshRenderers;

	internal MaterialPropertyBlock materialProperty;

	internal Color _currentColor = Color.black;

	private readonly Quaternion DefaultRotation = Quaternion.Euler(30f, 0f, 0f);

	private readonly Vector3 DefaultLocalPositionInTarget = new Vector3(0f, -0.3f, 0.4f);

	private readonly Vector3 DefaultLocalPositionInQueue = new Vector3(0f, -0.23f, 0.2f);

	private readonly Vector3 DefaultLocalPositionAtDisplay = new Vector3(0f, 0f, -0.5f);

	private Vector3 DefaultLocalPosition;

	public Vector3 _localScale;

	public void ResetData()
	{
		materialProperty = new MaterialPropertyBlock();
		foreach (MeshRenderer mesh in MeshRenderers)
		{
			mesh.enabled = false;
		}
	}

	internal IEnumerator Anim(RollWoolAnimationExtensions.ParentType parentType)
	{
		if (_currentColor == Color.black)
		{
			yield return null;
		}
		base.transform.localPosition = DefaultLocalPositionAtDisplay;
		base.transform.localRotation = DefaultRotation;
		float timePerRoll = (WoolAnimationData.Duration + WoolAnimationData.DurationHideWool) / (float)(MeshRenderers.Count + 1);
		base.transform.DOShakeRotation(timePerRoll * 7f, 10f, 10, 10f, true, ShakeRandomnessMode.Harmonic);
		for (int i = 0; i < MeshRenderers.Count; i++)
		{
			Transform meshTransform = MeshRenderers[i].transform;
			Vector3 originalScale = meshTransform.localScale;
			meshTransform.localScale = originalScale * 1.3f;
			MeshRenderers[i].enabled = true;
			meshTransform.DOScale(originalScale, timePerRoll * 0.6f).SetEase(Ease.OutBack);
			yield return Yielders.Get(timePerRoll);
		}
		if (parentType == RollWoolAnimationExtensions.ParentType.CubeQueue)
		{
			DefaultLocalPosition = DefaultLocalPositionInQueue;
			if (woolClip2 != null)
			{
				Singleton<SoundManager>.Instance.PlayOneShot(woolClip2);
			}
		}
		else
		{
			DefaultLocalPosition = DefaultLocalPositionInTarget;
			if (woolClip1 != null)
			{
				Singleton<SoundManager>.Instance.PlayOneShot(woolClip1);
			}
		}
		SnapToHole();
	}

	public void SnapToHole()
	{
		float timePerRoll = (WoolAnimationData.Duration + WoolAnimationData.DurationHideWool) / (float)(MeshRenderers.Count + 1);
		base.transform.DOLocalMove(DefaultLocalPosition, timePerRoll).SetEase(Ease.OutQuad).OnComplete(delegate
		{
			TargetBoxAnimation componentInParent = GetComponentInParent<TargetBoxAnimation>();
			if (componentInParent != null)
			{
				componentInParent.Hop(timePerRoll * 2f);
			}
			else
			{
				Debug.LogWarning("TargetBoxAnimation component not found in parent.");
			}
		});
	}

	public void SetParentType(RollWoolAnimationExtensions.ParentType parentType)
	{
		DefaultLocalPosition = ((parentType == RollWoolAnimationExtensions.ParentType.CubeQueue) ? DefaultLocalPositionInQueue : DefaultLocalPositionInTarget);
	}
}
