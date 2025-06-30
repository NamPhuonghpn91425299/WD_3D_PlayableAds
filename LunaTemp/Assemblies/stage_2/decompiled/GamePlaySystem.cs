using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Luna.Unity;
using UnityEngine;

public class GamePlaySystem : Singleton<GamePlaySystem>
{
	[SerializeField]
	private HandController handController;

	private const string AndroidRatingURI = "http://play.google.com/store/apps/details?id={0}";

	private string url;

	[SerializeField]
	private int cubeCountClaimed = 15;

	[SerializeField]
	private GameObject endGamePanel;

	[SerializeField]
	private int LoseOffer = 1;

	public CameraController CameraController;

	public BoxChainReaction3D BoxChainReaction3D;

	public List<CubeTargetControl> CurrentCubeTargets = new List<CubeTargetControl>();

	public List<QueueTargetControl> CurrentQueueTargets = new List<QueueTargetControl>();

	public GameObject YarnWoolPrefab;

	public GameObject RollWoolPrefab;

	private GamePlayMeshController _meshController;

	private Color[] _colorTargets = new Color[4]
	{
		Color.black,
		Color.black,
		Color.black,
		Color.black
	};

	private List<Color> _colorTargetList = new List<Color>();

	private List<RollWoolAnimation> _broomBoosterPool = new List<RollWoolAnimation>();

	private int _currentColorClickedCount;

	private int _currentColorCollected;

	[SerializeField]
	private int _cubeTargetCountDefault = 4;

	private int _queueCount;

	[SerializeField]
	private GameObject _levelPrefab;

	private bool _isPLayAnimUsingRainBow;

	private readonly int maxCubeTarget = 4;

	private Vector3 _scaleDefaultRollWool;

	[HideInInspector]
	public int TotalCubeActive;

	[HideInInspector]
	public int CubeReadyCount;

	public Action<bool> LockRainBowBooster;

	public AudioClip woolXoayClip;

	public AudioClip wool1Clip;

	public AudioClip loseSound;

	private int _currentOpenCubeTargetCost;

	private static int _replayCount = 0;

	public bool IsGoToStore;

	private bool _isUseBroomBooster;

	private Color _nextColor;

	private bool _hasCube;

	private bool _isWinGame;

	private static string REPLAY_COUNT = "REPLAY_COUNT";

	private static DateTime _timeStart;

	private List<CubeTargetControl> _activeObjects = new List<CubeTargetControl>();

	private Vector3 offset = new Vector3(1.2f, 0f, 0f);

	[SerializeField]
	private float spacingCubeTarget = 1f;

	private Vector3 center = new Vector3(0f, 0.45f, 0f);

	public int TotalColor => _meshController.TotalColor;

	public int QueueCount => _queueCount;

	public int MeshCountClick { get; private set; } = 0;


	public bool HasCube => _hasCube;

	public static string Play_ID { get; private set; }

	public override void Awake()
	{
		base.Awake();
		IsGoToStore = false;
	}

	private void Start()
	{
		LoadLevel();
	}

	public void Reset()
	{
		_replayCount = 0;
		CameraController.ResetCamearState();
	}

	public void ActiveHandController(bool isActive)
	{
		handController.SetActiveAnim(isActive);
	}

	public void GoToStore()
	{
		Playable.InstallFullGame();
		IsGoToStore = true;
	}

	public bool OnClickMesh(Transform startPoint, List<Vector3> spiralPath, Color colorClick)
	{
		MeshCountClick++;
		if (woolXoayClip != null)
		{
			Singleton<SoundManager>.Instance.PlayOneShot(woolXoayClip);
		}
		for (int i = 0; i < CurrentCubeTargets.Count; i++)
		{
			if (CurrentCubeTargets[i].IsActive && CurrentCubeTargets[i].CheckColor(colorClick) && CurrentCubeTargets[i].IsReady)
			{
				CurrentCubeTargets[i].AddChild(i, out var headtrans);
				if (headtrans == null)
				{
					break;
				}
				GameObject rollWool = UnityEngine.Object.Instantiate(RollWoolPrefab);
				rollWool.GetComponent<RollWoolAnimation>().ResetMesh().SetParent(headtrans)
					.SetColor(colorClick)
					.PlayAnim(RollWoolAnimationExtensions.ParentType.CubeTarget);
				ChoseYarnWool(rollWool.transform, startPoint, spiralPath, colorClick);
				_currentColorClickedCount++;
				return true;
			}
		}
		for (int j = 0; j < CurrentQueueTargets.Count; j++)
		{
			if (CurrentQueueTargets[j].AddChild(colorClick))
			{
				GameObject rollWool2 = UnityEngine.Object.Instantiate(RollWoolPrefab);
				rollWool2.GetComponent<RollWoolAnimation>().ResetMesh().SetParent(CurrentQueueTargets[j].transform)
					.SetColor(colorClick)
					.PlayAnim(RollWoolAnimationExtensions.ParentType.CubeQueue);
				ChoseYarnWool(rollWool2.transform, startPoint, spiralPath, colorClick);
				_currentColorClickedCount++;
				_queueCount++;
				if (_queueCount == CurrentQueueTargets.Count - 1)
				{
				}
				if (_queueCount >= CurrentQueueTargets.Count && CubeReadyCount == TotalCubeActive)
				{
					StartCoroutine(OnEndGameAction(false));
					LifeCycle.GameEnded();
				}
				return true;
			}
		}
		return false;
	}

	public void TrackingEndGame(bool isQuit, bool isWin)
	{
		if (isWin)
		{
			PlayerPrefs.SetInt(REPLAY_COUNT, 0);
		}
		string state = (isWin ? "win" : "lose");
		int totalColor = Math.Max(1, TotalColor);
		int timePlay = (int)(DateTime.Now - _timeStart).TotalSeconds;
	}

	public void GenNewCube(int indexCube)
	{
		if (indexCube == -1)
		{
			return;
		}
		_meshController.ColorPriorityCalculator();
		if (_meshController.CubeCount.Count > 1)
		{
			_colorTargetList = RemoveColorInList(_meshController._colorPriority, _colorTargets);
			_nextColor = GetColorByPriority(0f);
		}
		else
		{
			_nextColor = _meshController.CubeCount.FirstOrDefault().Key;
		}
		_hasCube = _meshController.CubeCount.TryGetValue(_nextColor, out var _);
		if (_hasCube)
		{
			_meshController.CubeCount[_nextColor]--;
			if (_meshController.CubeCount[_nextColor] == 0)
			{
				_meshController.CubeCount.Remove(_nextColor);
			}
		}
		CurrentCubeTargets[indexCube].SetColor(_nextColor);
		_colorTargets[indexCube] = _nextColor;
		CheckLockRainBowBooster();
		if (_meshController.CubeCount.Count == 0)
		{
			LockOpenCube();
		}
	}

	public void CheckTurnOffCube(int indexCube, bool active)
	{
		if (!active)
		{
			CurrentCubeTargets[indexCube].gameObject.SetActive(false);
			TotalCubeActive--;
			CubeReadyCount--;
			SmoothRepositioner();
		}
	}

	public void FinishedCollectingCube()
	{
		_currentColorCollected += 3;
		if (_currentColorCollected / 3 == cubeCountClaimed)
		{
			LifeCycle.GameEnded();
			GoToStore();
		}
	}

	public void UseQueueTarget(Color nextColor, int indexCube)
	{
		try
		{
			foreach (QueueTargetControl t in CurrentQueueTargets)
			{
				if (!t.CheckCurrentColor(nextColor) || !t.IsAtive())
				{
					continue;
				}
				Transform rollWoolChild = t.transform.GetChild(1);
				if (rollWoolChild != null)
				{
					RollWoolAnimation anim = rollWoolChild.GetComponent<RollWoolAnimation>();
					anim.SetColor(nextColor);
					Vector3 start = t.transform.position;
					CurrentCubeTargets[indexCube].AddChild(indexCube, out var headTrans);
					if (headTrans == null)
					{
						break;
					}
					rollWoolChild.SetParent(headTrans);
					Vector3 mid = (start + headTrans.position) * 0.5f + Vector3.up * 0.5f;
					rollWoolChild.DOKill();
					rollWoolChild.DOPath(new Vector3[3] { start, mid, headTrans.position }, 0.6f, PathType.CatmullRom).SetEase(Ease.InOutCubic).OnComplete(delegate
					{
						anim.SnapToHole();
						if (wool1Clip != null)
						{
							Singleton<SoundManager>.Instance.PlayOneShot(wool1Clip);
						}
					});
				}
				t.ResetDefault();
				_queueCount--;
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"Error in UseQueueTarget: {e}");
		}
	}

	public void CheckEndGame()
	{
		if (!_isPLayAnimUsingRainBow)
		{
			if (_currentColorCollected == _meshController.TotalColor && TotalCubeActive == CubeReadyCount)
			{
				StartCoroutine(OnEndGameAction(true));
				LifeCycle.GameEnded();
			}
			if (_queueCount >= CurrentQueueTargets.Count && TotalCubeActive == CubeReadyCount)
			{
				StartCoroutine(OnEndGameAction(false));
				LifeCycle.GameEnded();
			}
		}
	}

	private IEnumerator OnEndGameAction(bool isWin)
	{
		if (_isUseBroomBooster)
		{
			yield break;
		}
		if (!isWin)
		{
			yield return new WaitForSeconds(1.2f);
		}
		if (isWin)
		{
			if (!_isWinGame)
			{
				_isWinGame = true;
				TrackingEndGame(false, true);
			}
			yield break;
		}
		Debug.Log("Lose sound play");
		if (loseSound != null)
		{
			Singleton<SoundManager>.Instance.PlayOneShot(loseSound);
		}
		endGamePanel.SetActive(true);
	}

	private void CheckLockRainBowBooster()
	{
		bool onLockRainBowBooster = true;
		foreach (KeyValuePair<Color, int> item in _meshController.CubeCount)
		{
			if (item.Value > 1)
			{
				onLockRainBowBooster = false;
			}
		}
		LockRainBowBooster?.Invoke(onLockRainBowBooster);
	}

	private void UnLockCubeTarget(int newCubeCount)
	{
		TotalCubeActive = 0;
		CubeReadyCount = 0;
		for (int i = 0; i < CurrentCubeTargets.Count; i++)
		{
			CurrentCubeTargets[i].gameObject.SetActive(true);
			CurrentCubeTargets[i].SetActiveCubeTarget(i, i + 1 <= newCubeCount);
			CurrentCubeTargets[i].ActiveOpenCube(i + 1 > newCubeCount);
		}
		_cubeTargetCountDefault = newCubeCount;
		SmoothRepositioner();
	}

	public void LockOpenCube()
	{
		foreach (CubeTargetControl cube in CurrentCubeTargets)
		{
			if (!cube.IsActive)
			{
				cube.ActiveOpenCube(false);
			}
		}
	}

	private void ChoseYarnWool(Transform head, Transform tail, List<Vector3> spiralPath, Color color)
	{
		GameObject yarnWool = UnityEngine.Object.Instantiate(YarnWoolPrefab);
		YarnWoolAnimation YarnWoolScript = yarnWool.GetComponent<YarnWoolAnimation>();
		YarnWoolScript.SetColor(color);
		YarnWoolScript.SetPoints(spiralPath);
		YarnWoolScript.SetParent(head, tail);
	}

	private List<Color> RemoveColorInList(Dictionary<Color, float> listRemove, Color[] listReference, bool alowSameColor = false)
	{
		List<Color> result = new List<Color>();
		if (!alowSameColor)
		{
			for (int i = 0; i < 4; i++)
			{
				if (!(listReference[i] == Color.black))
				{
					listRemove.Remove(listReference[i]);
				}
			}
		}
		if (listRemove.Count == 0 && _meshController.CubeCount.Count >= 1 && _meshController.CubeCount.First().Value >= 1)
		{
			foreach (KeyValuePair<Color, float> color in listRemove)
			{
				if (color.Value != 0f)
				{
					result.Add(color.Key);
				}
			}
		}
		else
		{
			foreach (KeyValuePair<Color, float> item in listRemove)
			{
				result.Add(item.Key);
			}
		}
		return result;
	}

	private Color GetColorByPriority(float priority)
	{
		if (_colorTargetList.Count == 1)
		{
			return _meshController?._colorPriority?.FirstOrDefault().Key ?? Color.black;
		}
		Color result = Color.black;
		try
		{
			foreach (KeyValuePair<Color, float> colorPriority in _meshController._colorPriority)
			{
				result = colorPriority.Key;
				if (priority <= colorPriority.Value)
				{
					break;
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"Error in GetColorByPriority: {e}");
		}
		return result;
	}

	private void LoadLevel()
	{
		LifeCycle.GameUpdated();
		_meshController = _levelPrefab.GetComponent<GamePlayMeshController>();
		CameraController.ResetCamearState();
		UnLockCubeTarget(_cubeTargetCountDefault);
		ResetCubeTarget();
		ResetQueueTarget();
		CameraController.SpawnPoint.rotation = Quaternion.identity;
		_isWinGame = false;
		_meshController?.LoadcolorForMesh();
		CameraController.Setup(_levelPrefab);
		Singleton<CameraController>.Instance?.BlockRotate(false);
		_currentColorClickedCount = 0;
		_currentColorCollected = 0;
		_queueCount = 0;
		MeshCountClick = 0;
		for (int i = 0; i < _cubeTargetCountDefault; i++)
		{
			GenNewCube(i);
			CurrentCubeTargets[i].ChangeColor();
		}
		CameraController.HoldClickTime = 0;
		CameraController.StartIntro();
	}

	private void ResetCubeTarget()
	{
		foreach (CubeTargetControl cube in CurrentCubeTargets)
		{
			cube.SetDefault();
		}
		_isPLayAnimUsingRainBow = false;
	}

	private void ResetQueueTarget()
	{
		_queueCount = 0;
		if (CurrentQueueTargets.Count != BoxChainReaction3D.InitialBoxCount)
		{
			BoxChainReaction3D.InitializeBoxes();
			return;
		}
		foreach (QueueTargetControl queue in CurrentQueueTargets)
		{
			queue.ResetDefault();
		}
	}

	private void SmoothRepositioner()
	{
		CacheActiveObjects();
		Reposition();
	}

	private void CacheActiveObjects()
	{
		_activeObjects.Clear();
		foreach (CubeTargetControl obj in CurrentCubeTargets)
		{
			if (obj != null && obj.gameObject.activeSelf)
			{
				_activeObjects.Add(obj);
			}
		}
	}

	private void Reposition()
	{
		int activeCount = _activeObjects.Count;
		if (activeCount == 0)
		{
			return;
		}
		float totalWidth = (float)(activeCount - 1) * spacingCubeTarget;
		float startX = (0f - totalWidth) / 2f;
		for (int i = 0; i < activeCount; i++)
		{
			CubeTargetControl obj = _activeObjects[i];
			Vector3 currentPos = obj.transform.position;
			Vector3 targetPos = new Vector3(startX + (float)i * spacingCubeTarget, currentPos.y, currentPos.z);
			obj.transform.DOMoveX(targetPos.x, 0.3f).SetEase(Ease.OutQuad).OnComplete(delegate
			{
				obj.BakeAnimPosition();
			});
		}
	}
}
