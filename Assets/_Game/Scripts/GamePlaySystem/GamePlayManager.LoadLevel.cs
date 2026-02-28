using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GamePlayManager
{
    private LevelConfigData _currentLevelConfigData;
    public int CurrentLevelIndex = 1;
    public int GetLevelType() => _currentLevelConfigData != null ? (int)_currentLevelConfigData.LevelType : 0;

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
        _replayCount = 0;
        CameraController.ResetCameraState();
        GamePlayUIManager.Instance?.ResetData();
        return LoadLevel(true, true);
    }

    public void LoadNewLevel()
    {
        if (!TryResolveLevelConfig(CurrentLevelIndex, out _))
        {
            return;
        }

        _replayCount = 0;
        ReLoadLevel();
    }

    public Coroutine LoadLevel(bool startIntro = true, bool isShowLevel = true)
    {
        return StartCoroutine(LoadLevelCoroutine(startIntro, isShowLevel));
    }

    private IEnumerator LoadLevelCoroutine(bool startIntro = true, bool isShowLevel = true)
    {
        if (!TryResolveLevelConfig(CurrentLevelIndex, out _currentLevelConfigData))
        {
            yield break;
        }

        GameEventManager.PlayAnimPreLose?.Invoke(false);
        GameEventManager.SetupObjectTargetActive?.Invoke(true);
        GameEventManager.OnLoadLevelDone?.Invoke();
        GameEventManager.OnNewLevel?.Invoke();

        LockVacuumCleaner?.Invoke(false);
        yield return ResetLevelIntoDefaultCoroutine();

        if (_levelPrefab != null)
        {
            Destroy(_levelPrefab);
            _levelPrefab = null;
        }

        if (!SpawnLevelPrefab())
        {
            yield break;
        }

        InitializeSpawnedLevel();
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
        CameraController.Instance?.BlockRotate(false);
        GameEventManager.OnEndGameUIDeactive?.Invoke();
    }

    private Coroutine ResetLevelIntoDefault()
    {
        return StartCoroutine(ResetLevelIntoDefaultCoroutine());
    }

    private IEnumerator ResetLevelIntoDefaultCoroutine()
    {
        _isInitCube = true;
        _isAutoPlay = false;
        IsWinGame = false;
        _replayCount = 0;
        _currentColorCollected = 0;
        _currentColorDistributed = 0;
        _queueCount = 0;
        MeshCountClick = 0;
        _currentIndexColor = 0;
        _isInitCube = false;
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
        PreWarmGamePools();
        yield return null;
        CameraController.SpawnPoint.rotation = Quaternion.identity;
        CameraController.HoldClickTime = 0;
        GetCutOuts();
        GameEventManager.OnMeshCountClickChange?.Invoke();
    }

    #region HELPER

    private bool TryResolveLevelConfig(int level, out LevelConfigData configData)
    {
        configData = levelConfigSO.GetLevelConfigData(level);
        if (configData != null)
        {
            return true;
        }

        Debug.LogError($"[LevelLoad] Level {level} not found in {levelConfigSO.name}");
        return false;
    }

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
        if (!TryResolveLevelConfig(level, out _currentLevelConfigData))
        {
            yield break;
        }

        if (_levelPrefab != null)
        {
            Destroy(_levelPrefab);
            _levelPrefab = null;
        }

        if (!SpawnLevelPrefab())
        {
            yield break;
        }

        if (onDestroyLevel)
        {
            Destroy(_levelPrefab);
            _levelPrefab = null;
            yield break;
        }

        yield return null;
        InitializeSpawnedLevel();
    }

    private bool SpawnLevelPrefab()
    {
        GameEventManager.OnInstanceNewLevel?.Invoke();

        var prefab = LoadLevelPrefab(_currentLevelConfigData);
        if (prefab == null)
        {
            _levelPrefab = null;
            return false;
        }

        _levelPrefab = Instantiate(prefab, ParentObject);
        return _levelPrefab != null;
    }

    private void InitializeSpawnedLevel()
    {
        if (_levelPrefab == null || _currentLevelConfigData == null)
        {
            return;
        }

        _levelController = _levelPrefab.GetComponent<LevelController>();
        _levelController.InitData(_currentLevelConfigData);
        GameEventManager.OnDoneLoadLevel?.Invoke(_levelController);
    }

    private GameObject LoadLevelPrefab(LevelConfigData configData)
    {
        if (configData == null)
        {
            Debug.LogError("[LevelLoad] Level config is null");
            return null;
        }

        if (configData.MainPrefab != null)
        {
            return configData.MainPrefab;
        }

        var prefab = Resources.Load<GameObject>(configData.MainPrefabPath);
        if (prefab != null)
        {
            return prefab;
        }

        Debug.LogError($"[LevelLoad] Prefab NOT FOUND. MainPrefab is null and Resources path invalid: {configData.MainPrefabPath}");
        return null;
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

    #endregion
}
