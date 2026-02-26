using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using DG.Tweening;


public partial class GamePlayManager : SingletonBase<GamePlayManager>
{
    #region <====================| Properties |====================>

#if UNITY_EDITOR
    [Header("Auto play")] public float FrameCount = 10;
#endif

    public ColorPalleteData_new colorPalleteData;
    [Tooltip("Tham chiếu đến HandController để hiển thị/ẩn hoạt ảnh hướng dẫn.")]
    [SerializeField]
    private HandController handController;
    [SerializeField] private int LoseOffer = 1;
    [SerializeField] private LevelConfigSO levelConfigSO;
    [SerializeField] private AddCubeTargetData AddCubeDataSO;
    [SerializeField] private BoosterDataSO broomDataSO;

    public Transform ParentObject;

    public CameraController CameraController;
    public BoxChainReactionController BoxChainReactionController;

    public List<CubeTargetControl> CurrentCubeTargets = new List<CubeTargetControl>();
    public List<QueueTargetControl> CurrentQueueTargets = new List<QueueTargetControl>();
    // public CubeTargetControl RainBowTargetControl;
    public GameObject WoolBasket;

    public GameObject YarnWoolPrefab;
    public GameObject RollWoolPrefab;

    public int MaxQueueTargetCount = 7;
    public int MaxCubeTargetCount = 4;


    private LevelController _levelController;
    public LevelController LevelController => _levelController;

    private List<string> _colorTargets = new List<string>()
    {
        ShaderPropertiesLib.IgnoredWoolColorKey,
        ShaderPropertiesLib.IgnoredWoolColorKey,
        ShaderPropertiesLib.IgnoredWoolColorKey,
        ShaderPropertiesLib.IgnoredWoolColorKey
    };

    private List<float> _currentCubesPrio = new List<float>()
    {
        0f,
        0f,
        0f,
        0f
    };

    private List<int> _cubeTargetPrio;


    private List<WoolRollAnimator> _broomBoosterPool = new List<WoolRollAnimator>();

    private int _currentColorCollected;
    private int _currentColorDistributed;
    [SerializeField] private int _cubeTargetCountDefault = 2;
    private bool _isInitCube;

    private int _queueCount;

    private GameObject _levelPrefab;
    private Sprite _offerIcon;

    private bool _isUseOpenCubeoffer;

    private bool _isUsingRainBowBooster;
    private bool _isPLayAnimUsingRainBow;

    private readonly int maxCubeTarget = 4;
    private Vector3 _scaleDefaultRollWool;


    [HideInInspector] public int TotalCubeActive;
    [HideInInspector] public int CubeReadyCount;
    [HideInInspector] public int TotalQueueActiveCount;

    public int LastLevel => levelConfigSO.levelConfigDataList.Count;
    public int CurrentCubeCollected => _currentColorCollected;
    public int TotalColor => _levelController?.TotalColor ?? 0;
    public int CubeTargetCount => _levelController?.CubeCount?.Count ?? 0;
    public int QueueCount => _queueCount;

    public int CurrentColorDistributed => _currentColorDistributed;

    public bool IsFinishedUsingRainBow => _isPLayAnimUsingRainBow;

    public Action<bool> LockVacuumCleaner;

    //public         SoundSO soundDict;
    private int _purchaseOfferCost;
    private static int _replayCount = 0;

    [SerializeField] private Transform cubeTargetParent;
    [SerializeField] private Transform queueTargetParent;
    [SerializeField] private float deltaCubeWhenRabitEars = 0.5f;
    [SerializeField] private float deltaQueueWhenRabitEars = 1.5f;
    private Vector3 CubeTargetDefaultPos = new Vector3(0, 3.4f, -5);
    private Vector3 QueueTargetDefaultPos = new Vector3(0, 2.1f, -5);
    private List<Vector3> _cubeTargetDefaultPos = new List<Vector3>();

    private Camera m_renderCamera;

    private Camera RenderCamera
    {
        get
        {
            m_renderCamera = CameraContainer.Instance.FakeUICamera;
            return m_renderCamera;
        }
    }

    public bool IsWaitingForFakeLoading;

    public bool IsEndGame;

    #endregion <=============================================>


    #region UNITY_METHODS
    void OnEnable()
    {

    }
    private void Start()
    {
        GameEventManager.ShowPopupOfferEndGame += ShowPopupOfferEndGame;
    }

    public override void Awake()
    {
        base.Awake();
        if (colorPalleteData != null)
        {
            colorPalleteData.BuildDictionary();
        }

        foreach (var cube in CurrentCubeTargets)
        {
            _cubeTargetDefaultPos.Add(cube.transform.localPosition);
        }
        OnGameStateChange();
        LoadNewLevel();
    }

    private void GetCutOuts()
    {

    }

    public float GetHeightOfQueue => queueTargetParent.position.y;
    private bool _isAutoPlay;
#if UNITY_EDITOR

    private void OnValidate()
    {
        cubeTargetParent = transform.Find("CurrentCubeTarget")
            ?.transform;
        queueTargetParent = transform.Find("CurrentQueueTarget")
            ?.transform;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnEndGameAction(true);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            _isAutoPlay = !_isAutoPlay;
        }

        if (_isAutoPlay && Time.frameCount % FrameCount == 0)
        {
            //StartCoroutine(UseVacuumCleanerBooster());
        }
    }
#endif

    private void OnDestroy()
    {
        GameEventManager.ShowPopupOfferEndGame -= ShowPopupOfferEndGame;
        // GameEventManager.OnUseSawAdsBooster -= OnSawAdsBooster;
        // GameEventManager.OnUseSaveBooster -= OnUseSaveBooster;
        // GameEventManager.OnSaveBooster -= OnSaveBooster;
        // GameEventManager.OnGameStateChange -= OnGameStateChange;
    }

    #endregion

    #region MAIN_METHODS
    /// <summary>
    /// Kích hoạt hoặc vô hiệu hóa hoạt ảnh hướng dẫn của bàn tay.
    /// </summary>
    /// <param name="isActive">`true` để kích hoạt, `false` để tắt.</param>
    public void ActiveHandController(bool isActive)
    {
        handController.SetActiveAnim(isActive);
    }
    public void Reset()
    {
        _replayCount = 0;

        // Dừng tất cả DOTween animations trên camera trước
        Camera mainCamera = CameraContainer.Instance?.MainCamera;
        if (mainCamera != null)
        {
            mainCamera.transform.DOKill();
            mainCamera.DOKill();
        }

        // Khôi phục camera về trạng thái ban đầu trước khi chuyển level
        if (_cameraStatesSaved)
        {
            RestoreOriginalCameraStates(0.5f);
            StartCoroutine(WaitForCameraRestore());
        }
        else
        {
            Debug.LogWarning("Camera states were not saved, skipping camera restoration");
        }

        CameraController.ResetCameraStateMainMenu();
        Destroy(_levelPrefab);
        _levelPrefab = null;
    }

    private IEnumerator WaitForCameraRestore()
    {
        yield return new WaitForSeconds(0.6f);
    }

    // public IEnumerator InitData()
    // {
    //     return LoadLevel(startIntro: false);
    // }

    // public IEnumerator StartIntro()
    // {
    //     return StartLevelWhenHasLevel();
    // }

    public IEnumerator PreloadLoadLevel(bool isDestroy)
    {
        BoxChainReactionController.Instance.OnInitialized();
        yield return null;

        GenericObjectPool.Instance.InitPool(RollWoolPrefab, 10);
        yield return null;
        GenericObjectPool.Instance.InitPool(YarnWoolPrefab, 10);

        yield return null;
        //yield return StartCoroutine(LoadAsyncLevel(DataManager.PlayerData.LevelProgress, isDestroy));
    }

    public int MeshCountClick { get; set; } = 0;

    public bool IsRainBowBoosterTut;


    /// <summary>
    /// Tra ve -1 neu chua vao level loop
    /// 
    /// </summary>
    /// <returns></returns>


    public int CubeMoveGreatValueCount(float valueCompare)
    {
        int result = 0;
        foreach (var cubePrio in _currentCubesPrio)
        {
            if (cubePrio >= valueCompare) result++;
        }
        return result;
    }

    public float GetCurrentQueueFillPercent()
    {
        return (float)_queueCount / TotalQueueActiveCount * 100f;
    }

    public bool OnChoseColor(WoolControl startPoint, List<Vector3> spiralPath, string colorClick)
    {
        // if (IsRainBowBoosterTut)
        // {
        //     if (startPoint.name.Equals("A_Rocket_09"))
        //     {
        //         if (soundDict.GetAudioClip("wool_xoay") != null) GameAudioManager.Instance.PlayOneShot(soundDict.GetAudioClip("wool_xoay"), soundDict.GetSoundVolume("wool_xoay"));
        //         GameEventManager.OnRainbowBoxBoosterComplete?.Invoke();
        //         return UseRainBowBooster(startPoint, spiralPath, colorClick);
        //     }
        //     else return false;
        // }
        //tHÁO LEN
        MeshCountClick++;
        GameEventManager.OnMeshCountClickChange?.Invoke();
        //if (soundDict.GetAudioClip("wool_xoay") != null) GameAudioManager.Instance.PlayOneShot(soundDict.GetAudioClip("wool_xoay"), soundDict.GetSoundVolume("wool_xoay"));
        SoundManager.Instance.PlayOneShot("wool_xoay");
        //Debug.Log("Play sound wool_xoay");
        if (CheckChosenColorInCubeTarget(startPoint.transform, spiralPath, colorClick)) return true;

        if (_isUsingRainBowBooster)
        {
            GameEventManager.OnRainbowBoxBoosterComplete?.Invoke();
            return UseRainBowBooster(startPoint.transform, spiralPath, colorClick);
        }

        if (CheckChosenColorInQueueTarget(startPoint, spiralPath, colorClick)) return true;

        return false;
    }

    public void OnlyUseBroomTut(WoolControl startPoint, List<Vector3> spiralPath, string colorClick)
    {
        foreach (QueueTargetControl queue in CurrentQueueTargets)
        {
            if (!queue.AddChild(colorClick)) continue;
            var rollWool = GenericObjectPool.Instance.PopFromPool(RollWoolPrefab, instantiateIfNone: true);
            rollWool.transform.SetParent(queue.transform, true);
            var rollWoolAnimator = rollWool.GetComponent<WoolRollAnimator>();
            queue.SetWoolRollAnimator(rollWoolAnimator);
            rollWoolAnimator
                .ResetMesh()
                .SetWoolRedo(startPoint)
                .SetColor(colorClick)
                .PlayAnimAddToQueue(RollWoolAnimationExtensions.ParentType.CubeQueue);
            _queueCount++;
            return;
        }
    }


    public void CheckColorInBroomPool(string color, int indexCube)
    {
        if (_isInitCube) return;
        if (string.IsNullOrEmpty(color)) return;
        for (var index = 0; index < _broomBoosterPool.Count; index++)
        {
            var broomCount = _broomBoosterPool.Count;
            if (broomCount == 0) break;
            index = Mathf.Clamp(index, 0, broomCount - 1);
            var rollWool = _broomBoosterPool[index];
            if (color.Equals(rollWool._currentColor) && CurrentCubeTargets.Count > 0)
            {
                index = Mathf.Clamp(index, 0, broomCount - 1);
                CurrentCubeTargets[indexCube]
                    .AddChild(indexCube, out var headTrans);
                if (headTrans == null) continue;
                rollWool.transform.localScale = Vector3.one * _scaleDefaultRollWool.x;
                rollWool.SetParent(headTrans);
                Vector3 start = rollWool.transform.position;
                Vector3 mid = (start + headTrans.position) * 0.5f + Vector3.up * 0.5f;
                var index1 = index;
                rollWool.transform.DOKill();
                rollWool
                    .transform
                    .DOPath(new[]
                        {
                            start,
                            mid,
                            headTrans.position
                        }, 0.6f, PathType.CatmullRom
                    )
                    .SetEase(Ease.InOutCubic)
                    .OnComplete(() =>
                        {
                            var anim = rollWool.GetComponent<WoolRollAnimator>();
                            anim.SetParentType(RollWoolAnimationExtensions.ParentType.CubeTarget);
                            rollWool.SnapToHole();
                            SoundManager.Instance.PlayOneShot("wool1");
                            Debug.Log("Play sound wool1");

                        }
                    );

                _broomBoosterPool.RemoveAt(index);
                index--;
            }
        }

        if (_broomBoosterPool.Count == 0)
        {
            GamePlayUIManager.Instance.ActiveWoolBasket(false);
        }
    }

    public void CheckColorInQueuePool(string nextColor, int indexCube)
    {
        if (string.IsNullOrEmpty(nextColor)) return;
        // Sử dụng target trên queue
        try
        {
            //GameEventManager.PlayAnimPreLose?.Invoke(false);
            foreach (QueueTargetControl t in CurrentQueueTargets)
            {
                if (!t.CheckCurrentColor(nextColor) || !t.IsAtive() || !t.IsHasWoolRool() || t.IsReDo()) continue;
                // if (t.transform.childCount < 2) continue;

                // Hiệu ứng cọc bay từ queue lên box
                // var rollWoolChild = t.transform.GetChild(1);
                //
                // if (rollWoolChild == null) continue;

                // var anim  = rollWoolChild.GetComponent<WoolRollAnimator>();
                var anim = t.GetWoolRollAnimator();
                anim.SetColor(nextColor);

                Vector3 start = t.transform.position;
                CurrentCubeTargets[indexCube]
                    .AddChild(indexCube, out var headTrans);
                if (headTrans == null) return;

                anim.transform.SetParent(headTrans);
                Vector3 mid = (start + headTrans.position) * 0.5f + Vector3.up * 0.5f;
                anim.transform.DOKill();
                anim.transform
                    .DOPath(new[]
                        {
                            start,
                            mid,
                            headTrans.position
                        }, 0.6f, PathType.CatmullRom
                    )
                    .SetEase(Ease.InOutCubic)
                    .OnComplete(() =>
                        {
                            anim.SnapToHole();
                            SoundManager.Instance.PlayOneShot("wool1");
                            Debug.Log("Play sound wool1");
                        }
                    ).OnStart(() =>
                    {
                        anim.IsWoolToCube = true;
                    }).OnKill(() =>
                    {
                        anim.IsWoolToCube = false;
                    });
                // Logic cũ
                t.ResetDefault();
                _queueCount--;
            }
            GameEventManager.PlayAnimPreLose?.Invoke(_queueCount == TotalQueueActiveCount - 1);
            GameEventManager.ReplayAnimHole?.Invoke();

        }
        catch (Exception e)
        {
            Debug.LogError($"Error in UseQueueTarget: {e}");
        }

    }

    public void CheckLockBroomBooster()
    {
        // if (TotalCubeActive == CubeReadyCount)
        // {
        //     GamePlayUIManager.Instance?.SetBroomBoosterInteractable(true);
        //     GamePlayUIManager.Instance?.SetRedoBoosterInteractable(true);
        // }
        // else
        // {
        //     GamePlayUIManager.Instance?.SetBroomButtonInteractable(false);
        //     GamePlayUIManager.Instance?.SetRedoButtonInteractable(false);
        // }
    }

    public void CheckLockVacuumCleaner()
    {
        LockVacuumCleaner?.Invoke(CubeReadyCount == 0);
    }

    public void CheckLockRedoBooster()
    {
        //GamePlayUIManager.Instance.SetRedoBoosterInteractable(true);
    }

    public void AddQueueTarget()
    {
        TotalQueueActiveCount++;
        if (CurrentQueueTargets.Count == MaxQueueTargetCount)
            return;
        BoxChainReactionController?.TriggerAnimation();
    }

    public bool IsQueueTargetFull()
    {
        return _queueCount == TotalQueueActiveCount;
    }
    public bool IsCubeTargetFull()
    {
        return TotalCubeActive == MaxCubeTargetCount;
    }

    #region CAMERA_STATE_MANAGEMENT

    // Lưu trạng thái camera ban đầu để reset khi cần
    private Vector3 _originalCameraPosition;
    private Quaternion _originalCameraRotation;
    private float _originalCameraFOV;
    private bool _cameraStatesSaved = false;


    /// <summary>
    /// Lưu trạng thái camera ban đầu khi bắt đầu level
    /// </summary>
    private void SaveOriginalCameraStates()
    {
        Camera mainCamera = CameraContainer.Instance?.MainCamera;
        if (mainCamera != null)
        {
            _originalCameraPosition = mainCamera.transform.position;
            _originalCameraRotation = mainCamera.transform.rotation;
            _originalCameraFOV = mainCamera.fieldOfView;
            _cameraStatesSaved = true;

            //Debug.Log($"Camera states saved: Position={_originalCameraPosition}, Rotation={_originalCameraRotation.eulerAngles}, FOV={_originalCameraFOV}");
        }
    }

    /// <summary>
    /// Khôi phục camera về trạng thái ban đầu
    /// </summary>
    private void RestoreOriginalCameraStates(float smoothTime = 1.0f)
    {
        if (!_cameraStatesSaved) return;

        Camera mainCamera = CameraContainer.Instance?.MainCamera;
        if (mainCamera == null) return;

        //Debug.Log($"Restoring camera to original states: Position={_originalCameraPosition}, Rotation={_originalCameraRotation.eulerAngles}, FOV={_originalCameraFOV}");

        // Tạo sequence để restore camera một cách mượt mà
        Sequence restoreSequence = DOTween.Sequence();

        // Animation FOV
        restoreSequence.Join(mainCamera.DOFieldOfView(_originalCameraFOV, smoothTime).SetEase(Ease.InOutCubic));

        // Animation vị trí camera
        restoreSequence.Join(mainCamera.transform.DOMove(_originalCameraPosition, smoothTime).SetEase(Ease.InOutCubic));

        // Animation rotation camera
        restoreSequence.Join(mainCamera.transform.DORotateQuaternion(_originalCameraRotation, smoothTime).SetEase(Ease.InOutCubic));

        // Update callbacks
        restoreSequence.OnUpdate(() =>
        {
            // Cập nhật ZoomCamera để đồng bộ background và UI
            if (CameraController.Instance != null)
            {
                CameraController.Instance.ZoomCamera(mainCamera.fieldOfView);
            }
        });

        restoreSequence.OnComplete(() =>
        {
            //Debug.Log($"Camera restoration completed. Final Position: {mainCamera.transform.position}, Rotation: {mainCamera.transform.rotation.eulerAngles}, FOV: {mainCamera.fieldOfView}");
        });
    }

    #endregion

    public void FinishedCollectingCube()
    {
        _currentColorCollected += 3;
    }

    public void InstantlyWin() => OnEndGameAction(true);

    public void OnOpenCubeByGoldOrAds()
    {
        _replayCount++;
    }
    public bool IsQueueTargetOpenAll()
    {
        return TotalQueueActiveCount == CurrentQueueTargets.Count;
    }


    #endregion

    #region EDITOR

#if UNITY_EDITOR

    public void SaveLevelName()
    {
        var path = "Assets/_Game/Resources/Levels";
        var prefabs = AssetDatabase
            .FindAssets("t:Prefab", new[]
                {
                    path
                }
            )
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
            .ToArray();

        foreach (var prefab in prefabs)
        {
            levelConfigSO.TryAdd(new LevelConfigData()
            {
                Level = 0,
                MainPrefabPath = $"Levels/{prefab.name}"
            }
            );
        }

        Debug.Log($"Load thành công {prefabs.Length} levels");
    }
#endif

    #endregion

    #region HELPER_METHODS

    private void OnGameStateChange()
    {
        GetCutOuts();
    }

    public QueueTargetControl GetEmptyQueueTarget()
    {
        for (int i = 0; i < CurrentQueueTargets.Count; i++)
        {
            if (CurrentQueueTargets[i].IsActive && CurrentQueueTargets[i].IsHasWoolRool()
                                                && !CurrentQueueTargets[i].IsReDo()
                                                && !CurrentQueueTargets[i].IsWoolToCube()) continue;
            return CurrentQueueTargets[i];
        }
        return null;
    }

    private bool CheckChosenColorInCubeTarget(Transform startPoint, List<Vector3> spiralPath, string colorClick)
    {
        var targetMat = colorPalleteData.colorPallete_New[colorClick];
        Color aoColor = Color.white;
        foreach (var cubePrio in _cubeTargetPrio)
        {
            var cube = CurrentCubeTargets[cubePrio];
            if (cube == null) continue;
            if (!cube.IsActive || !cube.IsReady) continue;

            if (!cube.CheckColor(colorClick)) continue;

            cube.AddChild(cubePrio, out var headtrans);

            if (headtrans == null) continue;

            var rollWool = GenericObjectPool.Instance.PopFromPool(RollWoolPrefab, instantiateIfNone: true);
            var animator = rollWool.GetComponentInChildren<WoolRollAnimator>();
            if (animator == null)
            {
                var allComponents = string.Join(", ", rollWool.GetComponents<Component>().Select(c => c?.GetType().Name ?? "null"));
                Debug.LogError($"[GamePlayManager] PopFromPool returned object [{rollWool.name}] WITHOUT WoolRollAnimator (even in children!). Root components: {allComponents}. Prefab: {RollWoolPrefab.name}");
                continue;
            }

            animator
               .ResetMesh()
               .SetParent(headtrans)
               .SetColor(colorClick)
               .PlayAnimAddToQueue(RollWoolAnimationExtensions.ParentType.CubeTarget);

            ChoseYarnWool(rollWool.transform, startPoint, spiralPath, colorClick);
            return true;
        }
        // for (int i = 0; i < CurrentCubeTargets.Count; i++)
        // {
        //     
        // }

        return false;
    }

    private bool CheckChosenColorInQueueTarget(WoolControl startPoint, List<Vector3> spiralPath, string colorClick)
    {
        var currentColorClick = colorPalleteData.colorPallete_New[colorClick];
        foreach (QueueTargetControl queue in CurrentQueueTargets)
        {
            if (queue.IsAtive() || queue.IsReDo()) continue;
            if (!queue
                    .AddChild(colorClick)) continue;

            // if (TotalCubeActive == CubeReadyCount)
            // {
            //     GamePlayUIManager.Instance?.SetBroomBoosterInteractable(true);
            //     GamePlayUIManager.Instance?.SetRedoBoosterInteractable(true);
            // }
            // else
            // {
            //     GamePlayUIManager.Instance?.SetBroomButtonInteractable(false);
            //     GamePlayUIManager.Instance?.SetRedoButtonInteractable(false);
            // }
            var rollWool = GenericObjectPool.Instance.PopFromPool(RollWoolPrefab, instantiateIfNone: true);
            var rollWoolAnimator = rollWool.GetComponentInChildren<WoolRollAnimator>();
            if (rollWoolAnimator == null)
            {
                var allComponents = string.Join(", ", rollWool.GetComponents<Component>().Select(c => c?.GetType().Name ?? "null"));
                Debug.LogError($"[GamePlayManager] PopFromPool returned object [{rollWool.name}] WITHOUT WoolRollAnimator in Queue (even in children!). Root components: {allComponents}. Prefab: {RollWoolPrefab.name}");
                continue;
            }

            rollWool.transform.SetParent(queue.transform, true);
            queue.SetWoolRollAnimator(rollWoolAnimator);
            rollWoolAnimator
                .ResetMesh()
                .SetWoolRedo(startPoint)
                .SetColor(colorClick)
                .PlayAnimAddToQueue(RollWoolAnimationExtensions.ParentType.CubeQueue);

            ChoseYarnWool(rollWool.transform, startPoint.transform, spiralPath, colorClick);
            _queueCount++;
            if (_queueCount == TotalQueueActiveCount - 1 && !IsBroomBoosterTutorial)
            {
                GameEventManager.PlayAnimPreLose?.Invoke(true);
            }

            if (_queueCount >= TotalQueueActiveCount && CubeReadyCount == TotalCubeActive)
            {
                GameEventManager.PlayAnimPreLose?.Invoke(false);
                //Lose
                StartCoroutine(OnEndGameAction(false));
                Debug.Log("Lose Game");
            }

            return true;
        }

        return false;
    }

    private void ChoseYarnWool(Transform head, Transform tail, List<Vector3> spiralPath, string color,
        bool isRedo = false, bool isUse = true)
    {
        if (YarnWoolPrefab == null)
        {
            Debug.LogError("[GamePlayManager] YarnWoolPrefab is NULL!");
            return;
        }

        var yarnWool = GenericObjectPool.Instance.PopFromPool(YarnWoolPrefab, instantiateIfNone: true, forceInstantiate: !isUse);
        if (yarnWool == null)
        {
            Debug.LogError("[GamePlayManager] PopFromPool returned NULL for YarnWoolPrefab!");
            return;
        }

        if (!isUse)
        {
            var animator = yarnWool.GetComponentInChildren<YarnWoolAnimation>();
            if (animator != null)
            {
                GenericObjectPool.Instance.PushToPool(animator, yarnWool);
            }
            else
            {
                GenericObjectPool.Instance.PushToPool_Object(ref yarnWool);
            }
            return;
        }

        if (colorPalleteData == null)
        {
            Debug.LogError("[GamePlayManager] colorPalleteData is NULL!");
            return;
        }

        if (string.IsNullOrEmpty(color) || !colorPalleteData.colorPallete_New.TryGetValue(color, out var mat))
        {
            // If color is null/empty during pre-warm, it's expected if we use null for logic.
            // But if isUse is true, we need a color.
            return;
        }

        var yarnWoolScript = yarnWool.GetComponentInChildren<YarnWoolAnimation>();
        if (yarnWoolScript == null)
        {
            var allComponents = string.Join(", ", yarnWool.GetComponents<Component>().Select(c => c?.GetType().Name ?? "null"));
            Debug.LogError($"[GamePlayManager] PopFromPool returned object [{yarnWool.name}] WITHOUT YarnWoolAnimation (even in children!). Root components: {allComponents}. Prefab: {YarnWoolPrefab.name}");
            return;
        }

        yarnWoolScript.SetColor(mat);
        yarnWoolScript.SetPoints(spiralPath);
        yarnWoolScript.SetParent(head, tail, isRedo);
    }

    private void PreWarmGamePools()
    {
        for (int i = 0; i < 10; i++)
        {
            // Pre-warm YarnWool
            ChoseYarnWool(null, null, null, null, false, false);

            // Pre-warm RollWool
            var rollWool = GenericObjectPool.Instance.PopFromPool(RollWoolPrefab, instantiateIfNone: true);
            var animator = rollWool.GetComponentInChildren<WoolRollAnimator>();
            if (animator != null)
            {
                GenericObjectPool.Instance.PushToPool(animator, rollWool);
            }
            else
            {
                GameObject obj = rollWool;
                GenericObjectPool.Instance.PushToPool_Object(ref obj);
            }
        }
    }

    private IEnumerator OnOpenCube()
    {
        //UIFullScreenBlocker.Instance.Lock(6);
        IsEndGame = false;
        for (var i = 0; i < 4; i++)
        {
            if (CurrentCubeTargets[i].IsActive) continue;
            yield return StartCoroutine(CurrentCubeTargets[i].OnOpenCube());
            break;
        }

        //UIFullScreenBlocker.Instance.Unlock(6);
    }

    private void OnUseBroomBooster()
    {
        //DataManager.ChangeBroom(1, false, false);
        //GamePlayUIManager.Instance.OfferUseBroomBooster();
    }

    private bool _isOpenFullCube;

    public void SetOpenFullCube()
    {
        _isOpenFullCube = TotalCubeActive == 4;
    }

    private void ShowPopupOfferEndGame()
    {
        Debug.Log("ShowPopupOfferEndGame");
    }

    private IEnumerator ShowPopupEndGame()
    {
        //yield return StartCoroutine(UILayerManager.Instance.ShowUIInGameAsync());
        yield return null;
        GameEventManager.OnEndGameAction?.Invoke(false);
        Debug.Log("End Game");
    }

    #endregion
}
