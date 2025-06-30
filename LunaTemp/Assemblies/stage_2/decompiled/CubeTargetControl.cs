using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CubeTargetControl : MonoBehaviour
{
	[SerializeField]
	private int VibrationStrength = 50;

	public List<Transform> TargetChildren;

	[SerializeField]
	private Sprite AddCubeIcon;

	public MeshRenderer[] MeshRenderer;

	public bool IsActive;

	[SerializeField]
	private TargetBoxAnimation _boxAnimation;

	[SerializeField]
	private float _boxMoveAnimation = 0.5f;

	[SerializeField]
	private BoxCollider _boxCollider;

	public float RollWoolTime = 0.5f;

	public float DelayTime = 0.3f;

	private bool _isActiveGenNew;

	private bool _alowSameColor;

	private int _indexCube;

	private int _indexChild = 0;

	private Color _currentColor = Color.black;

	private const int TotalChild = 3;

	private bool _isReady;

	private readonly Color _defaultColor = new Color(0f, 0.759f, 0.6667294f, 1f);

	public bool IsReady => _isReady;

	public void AddChild(int indexCube, out Transform child)
	{
		if (_indexChild == 3)
		{
			child = null;
			return;
		}
		child = TargetChildren[_indexChild];
		if (_indexChild + 1 == 3)
		{
			StartCoroutine(WaitingAnim(indexCube));
		}
		_indexChild++;
	}

	public void SetColor(Color color)
	{
		_currentColor = color;
	}

	public Color GetColor()
	{
		return _currentColor;
	}

	public void SetActiveCubeTarget(int indexCube, bool active)
	{
		_isReady = active;
		_indexCube = indexCube;
		_boxCollider.enabled = !active;
		_alowSameColor = !active;
		IsActive = active;
		if (active && indexCube != -1)
		{
			Singleton<GamePlaySystem>.Instance.CubeReadyCount++;
			Singleton<GamePlaySystem>.Instance.TotalCubeActive++;
		}
	}

	public bool CheckColor(Color color)
	{
		return color == _currentColor;
	}

	private IEnumerator WaitingAnim(int indexCube)
	{
		Singleton<GamePlaySystem>.Instance.GenNewCube(indexCube);
		_isActiveGenNew = Singleton<GamePlaySystem>.Instance.HasCube;
		Singleton<GamePlaySystem>.Instance.CubeReadyCount--;
		_isReady = false;
		yield return new WaitForSeconds(RollWoolTime + DelayTime);
		_boxAnimation.CloseAndMoveOut();
		yield return new WaitForSeconds(_boxAnimation.CloseDuration);
		yield return new WaitForSeconds(_boxAnimation.MoveOutDuration);
		SetDefault();
		Singleton<GamePlaySystem>.Instance.CheckTurnOffCube(indexCube, _isActiveGenNew);
		Singleton<GamePlaySystem>.Instance.FinishedCollectingCube();
		ChangeColor();
		if (indexCube != -1)
		{
			_boxAnimation.FlyIn();
		}
		yield return new WaitForSeconds(_boxAnimation.FlyInDuration);
		Singleton<GamePlaySystem>.Instance.CubeReadyCount++;
		_isReady = true;
		Singleton<GamePlaySystem>.Instance.UseQueueTarget(_currentColor, indexCube);
		Singleton<GamePlaySystem>.Instance.CheckEndGame();
	}

	public void SetDefault()
	{
		_isReady = true;
		_indexChild = 0;
		base.gameObject.SetActive(true);
		foreach (Transform child in TargetChildren)
		{
			if (child.childCount >= 1)
			{
				Transform rollWool = child.GetChild(0);
				rollWool.parent = null;
				ObjectPool.Instance.AddToPool(rollWool.gameObject, 1);
				rollWool.gameObject.SetActive(false);
			}
		}
		if (_indexCube != -1)
		{
			ResetDefaultColor();
		}
	}

	public void DisplayRainBowBoxAnimation()
	{
		if (base.transform.localPosition.x < 0f)
		{
			base.transform.DOKill();
			base.transform.DOLocalMoveX(base.transform.localPosition.x - 0.6f, _boxMoveAnimation).SetEase(Ease.OutBack).OnComplete(delegate
			{
			});
		}
		else if (base.transform.localPosition.x > 0f)
		{
			base.transform.DOKill();
			base.transform.DOLocalMoveX(base.transform.localPosition.x + 0.6f, _boxMoveAnimation).SetEase(Ease.OutBack).OnComplete(delegate
			{
			});
		}
		else
		{
			base.transform.DOKill();
			base.transform.DOLocalMoveY(-0.45f, _boxMoveAnimation).SetEase(Ease.OutBack).OnComplete(delegate
			{
			});
			base.transform.DOLocalMoveZ(0f, _boxMoveAnimation).SetEase(Ease.OutBack).OnComplete(delegate
			{
			});
		}
	}

	public void HideRainBowBoxAnimation()
	{
		if (base.transform.localPosition.x < 0f)
		{
			base.transform.DOKill();
			base.transform.DOLocalMoveX(base.transform.localPosition.x + 0.6f, _boxMoveAnimation).SetEase(Ease.OutBack).OnComplete(delegate
			{
			});
		}
		else if (base.transform.localPosition.x > 0f)
		{
			base.transform.DOKill();
			base.transform.DOLocalMoveX(base.transform.localPosition.x - 0.6f, _boxMoveAnimation).SetEase(Ease.OutBack).OnComplete(delegate
			{
			});
		}
		else
		{
			base.transform.DOKill();
			base.transform.DOLocalMoveZ(-20f, _boxMoveAnimation).SetEase(Ease.OutBack).OnComplete(delegate
			{
			});
		}
	}

	public void ChangeColor()
	{
		if (!(_currentColor == Color.black))
		{
			MeshRenderer[] meshRenderer2 = MeshRenderer;
			foreach (MeshRenderer meshRenderer in meshRenderer2)
			{
				MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
				meshRenderer.GetPropertyBlock(propertyBlock);
				propertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, _currentColor);
				meshRenderer.SetPropertyBlock(propertyBlock);
			}
		}
	}

	public void ResetDefaultColor()
	{
		MeshRenderer[] meshRenderer2 = MeshRenderer;
		foreach (MeshRenderer meshRenderer in meshRenderer2)
		{
			MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
			meshRenderer.GetPropertyBlock(propertyBlock);
			propertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, _defaultColor);
			meshRenderer.SetPropertyBlock(propertyBlock);
		}
	}

	public void ActiveOpenCube(bool isActive)
	{
		_boxCollider.enabled = isActive;
	}

	public void BakeAnimPosition()
	{
		_boxAnimation.BakePrePos();
	}
}
