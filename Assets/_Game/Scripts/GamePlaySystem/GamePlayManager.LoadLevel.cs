using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public partial class GamePlayManager
{
    private LevelConfigData _currentLevelConfigData;
    public int CurrentLevelIndex = 1;
    public int GetLevelType() => (int)_currentLevelConfigData.LevelType;

    public void ResetLevelModel()
    {
        if (_levelPrefab == null) return;
        _levelPrefab.transform.localRotation = Quaternion.identity;
        ParentObject.localRotation = Quaternion.identity;
        CameraController.StartIntro();
        CameraController.Instance.BlockRotate(false);
        GameEventManager.OnEndGameUIDeactive?.Invoke();
    }

    public Coroutine ReLoadLevel()
    {
        return StartCoroutine(ReLoadLevelCoroutine());
    }

    private IEnumerator ReLoadLevelCoroutine()
    {
        _replayCount = 0;
        if (_levelPrefab != null)
        {
            Destroy(_levelPrefab);
            _levelPrefab = null;
        }
        CameraController.ResetCameraState();
        GamePlayUIManager.Instance?.ResetData();
        yield return StartCoroutine(LoadLevelCoroutine(true, true));
        CameraController.Instance?.BlockRotate(false);
        GameEventManager.OnEndGameUIDeactive?.Invoke();
    }

    public void LoadNewLevel()
    {
        StartCoroutine(LoadNewLevelCoroutine());
    }

    private IEnumerator LoadNewLevelCoroutine()
    {
        int levelToLoad = CurrentLevelIndex;
        var configData = levelConfigSO.GetLevelConfigData(levelToLoad);
        if (configData == null)
        {
            Debug.LogError($"[LoadNewLevel] Could not find data for level {levelToLoad} in {levelConfigSO.name}. Please check if you have Imported Levels.");
            yield break;
        }

        Debug.Log($"[LoadNewLevel] Loading Level {levelToLoad} using Prefab: {configData.MainPrefabPath} from {levelConfigSO.name}");
        _replayCount = 0;

        yield return ReLoadLevel();
    }

    public Coroutine LoadLevel(bool startIntro = true, bool isShowLevel = true)
    {
        return StartCoroutine(LoadLevelCoroutine(startIntro, isShowLevel));
    }

    private IEnumerator LoadLevelCoroutine(bool startIntro = true, bool isShowLevel = true)
    {
        GameEventManager.PlayAnimPreLose?.Invoke(false);
        //GameEventManager.SetupObjectTargetActive?.Invoke(false);
        GameEventManager.SetupObjectTargetActive?.Invoke(true);
        GameEventManager.OnLoadLevelDone?.Invoke();
        GameEventManager.OnNewLevel?.Invoke();
        yield return null;
        LockVacuumCleaner?.Invoke(false);
        StartCoroutine(ResetLevelIntoDefaultCoroutine());
        yield return null;

        var configData = levelConfigSO.GetLevelConfigData(CurrentLevelIndex);
        if (configData != null)
        {
            Debug.Log($"[LoadLevel] Starting load for Level {CurrentLevelIndex} using Prefab: {configData.MainPrefabPath} from asset [{levelConfigSO.name}]");
        }

        yield return StartCoroutine(LoadAsyncLevelCoroutine(CurrentLevelIndex));

        if (!_levelPrefab) yield break;

        CameraController.Setup(_levelPrefab);
        if (startIntro) CameraController.StartIntro();
        if (!isShowLevel) _levelPrefab.SetActive(false);

        for (int i = 0; i < _cubeTargetCountDefault; i++)
        {
            yield return null;
            GenColorForDefaultCube(i);
            CurrentCubeTargets[i].ChangeColor();
        }
        yield return null;
        SaveOriginalCameraStates();
    }

    private Coroutine ResetLevelIntoDefault()
    {
        return StartCoroutine(ResetLevelIntoDefaultCoroutine());
    }

    private IEnumerator ResetLevelIntoDefaultCoroutine()
    {
        _isInitCube = true;
        IsUsingBroom = false;
        _isAutoPlay = false;
        IsWinGame = false;
        _replayCount = 0;
        _currentColorCollected = 0;
        _currentColorDistributed = 0;
        _queueCount = 0;
        MeshCountClick = 0;
        _currentIndexColor = 0;
        _isInitCube = false;
        _purchaseOfferCost = AddCubeDataSO.FirstCubeTargetCost;
        IsEndGame = false;

        int targetCount = _cubeTargetCountDefault;
        if (_currentLevelConfigData != null && _currentLevelConfigData.CubeTargetCount > 0)
        {
            targetCount = _currentLevelConfigData.CubeTargetCount;
        }
        UnLockCubeTarget(targetCount);
        yield return null;
        ResetCubeTarget();
        yield return null;
        ResetQueueTarget();
        yield return null;
        ResetBroomBooster();
        PreWarmGamePools();
        yield return null;
        CameraController.SpawnPoint.rotation = Quaternion.identity;
        CameraController.HoldClickTime = 0;
        GetCutOuts();
        GameEventManager.OnMeshCountClickChange?.Invoke();
    }

    #region HELPER

    private void UnLockCubeTarget(int newCubeCount)
    {
        TotalCubeActive = 0;
        CubeReadyCount = 0;
        _isOpenFullCube = false;
        _currentCubesPrio = new List<float>() { 0, 0, 0, 0 };
        for (int i = 0; i < CurrentCubeTargets.Count; i++)
        {
            CurrentCubeTargets[i].ResetAnim();
            CurrentCubeTargets[i].gameObject.SetActive(true);
            CurrentCubeTargets[i].SetActiveCubeTarget(i, i + 1 <= newCubeCount);
            CurrentCubeTargets[i].ActiveOpenCube(i + 1 > newCubeCount);
        }
        _cubeTargetCountDefault = newCubeCount;
        _colorTargets = new List<string>() { ShaderPropertiesLib.IgnoredWoolColorKey, ShaderPropertiesLib.IgnoredWoolColorKey, ShaderPropertiesLib.IgnoredWoolColorKey, ShaderPropertiesLib.IgnoredWoolColorKey };
        SmoothRepositioner();
    }

    public Coroutine LoadAsyncLevel(int level, bool onDestroyLevel = false)
    {
        return StartCoroutine(LoadAsyncLevelCoroutine(level, onDestroyLevel));
    }

    private IEnumerator LoadAsyncLevelCoroutine(int level, bool onDestroyLevel = false)
    {
        _currentLevelConfigData = levelConfigSO.GetLevelConfigData(level);

        if (_currentLevelConfigData == null)
        {
            Debug.LogError($"Level {level} not found");
            yield break;
        }

        if (_levelPrefab) Destroy(_levelPrefab);

        GameEventManager.OnInstanceNewLevel?.Invoke();

        // Load prefab từ Resources_moved folder (dùng AssetDatabase cho Editor, Resources cho Build)
        #if UNITY_EDITOR
        string prefabPath = $"Assets/_Game/Resources_moved/Levels/{System.IO.Path.GetFileName(_currentLevelConfigData.MainPrefabPath)}.prefab";
        var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[LoadAsyncLevel] Prefab NOT FOUND at path: {prefabPath}. Check if the file exists or if the MainPrefabPath in LevelConfig is correct.");
        }
        #else
        var prefab = Resources.Load<GameObject>(_currentLevelConfigData.MainPrefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[LoadAsyncLevel] Prefab NOT FOUND in Resources: {_currentLevelConfigData.MainPrefabPath}");
        }
        #endif
        if (prefab != null)
        {
            Debug.Log($"[LoadAsyncLevel] Spawning prefab: {prefab.name}");
            _levelPrefab = GameObject.Instantiate(prefab, ParentObject);
        }

        if (onDestroyLevel)
        {
            Destroy(_levelPrefab);
            _levelPrefab = null;
        }
        if (!_levelPrefab || onDestroyLevel) yield break;

        _levelController = _levelPrefab.GetComponent<LevelController>();
        yield return null;
        _levelController.InitData(_currentLevelConfigData);
        DestroyLevelMiss();
        GameEventManager.OnDoneLoadLevel?.Invoke(_levelController);
    }

    private void DestroyLevelMiss()
    {
        if (!this || gameObject == null || !isActiveAndEnabled)
        {
            Debug.LogWarning("[DestroyLevelMiss] GamePlayManager is null, destroyed, or inactive. Skipping cleanup.");
            return;
        }

        try
        {
            var oldLevels = GetComponentsInChildren<LevelController>();
            if (oldLevels == null || oldLevels.Length == 0) return;

            foreach (var oldLevel in oldLevels)
            {
                if (oldLevel == null) continue;
                if (oldLevel == _levelController) continue;
                if (oldLevel.gameObject != null)
                {
                    Debug.Log($"[DestroyLevelMiss] Destroying old level: {oldLevel.gameObject.name}");
                    Destroy(oldLevel.gameObject);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[DestroyLevelMiss] Exception while cleaning up old levels: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }
    }

    private void ResetCubeTarget()
    {
        CubeTargetControl.WaitingAnimTokenSource?.Cancel();
        for (var index = 0; index < CurrentCubeTargets.Count; index++)
        {
            var cube = CurrentCubeTargets[index];
            cube.KillAllAnim();
            cube.SetDefault(true);
            cube.transform.localPosition = _cubeTargetDefaultPos[index];
        }
        _cubeTargetPrio = new List<int>() { 0, 1 };
        _isUsingRainBowBooster = false;
        _isPLayAnimUsingRainBow = false;
    }

    private void ResetQueueTarget()
    {
        _queueCount = 0;
        if (CurrentQueueTargets.Count != BoxChainReactionController.InitialBoxCount + 1)
        {
            BoxChainReactionController.InitializeBoxes();
            return;
        }
        for (var index = 0; index < CurrentQueueTargets.Count; index++)
        {
            QueueTargetControl queue = CurrentQueueTargets[index];
            queue.ResetDefault();
            queue.SetActive(index != BoxChainReactionController.InitialBoxCount);
        }
    }

    private void ResetBroomBooster()
    {
        _broomBoosterPool.Clear();
    }

    #endregion
}
