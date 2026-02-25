using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class CameraController : SingletonBase<CameraController>
{
    #region Serialized Fields

    [SerializeField]
    private AudioClip introRotateSound;

    [SerializeField]
    private AudioClip introFinishRotateSound;
    [SerializeField]
    private Vector3 _cameraPosGamePlayDefault = new(0, 1, -10);

    [SerializeField]
    private Vector3 _cameraRoteGamePlayDefault = new(6, 0, 0);

    [Header("Intro Settings")]
    public float IntroLenght = 2f;

    [Tooltip("Model will rotate exactly 360 degrees during intro duration")]
    public bool enableExact360Rotation = true;

    public float ModelRotationIntroSpeed = 0.5f;

    public float IntroCameraZoomInDuration = 0.5f;

    public int IntroStartFOV = 65;

    public int IntroEndFOV = 65;

    [Header("Dragging Settings")]
    public DraggingStyle DragStyle;


    public float DraggingSpeed = 0.25f;


    public float SmoothFactor = 7;

    [Header("Zoom Settings")]
    public ZoomCameraStyle ZoomStyle;

    [Header("Auto - center model along gameplay progress :D")]
    public bool AutoCenterModel = true;
    public bool UseRendererBoundsCenter = true;
    public float ThresholdToChangeCenter = 1f;
    public float WoolPercentLeftStopAutoZoom = 20f;
    private Vector3 originalSpawnPos;
    private float farthestWoolDistance;
    private float nearestWoolDistance;

    #endregion

    #region Public Properties
    [Space]
    public Interactable InputInteractable;
    public TargetObjectData TargetObjectData;
    public ZoomCameraData ZoomCameraData;
    public Transform BackGround;
    public Transform SpawnPoint;
    public GameObject ModelPrefab;
    public Quaternion targetRotation;
    public LevelController CurrentLevel;
    public float Friction = 3f;
    public Vector2 RotationSensitivity = new(1f, 1f);
    public Vector2 AccelerationRange = new(0.1f, 1f);
    public float RotationSpeed = 5f;
    public float RotationAutoSpeed = 0.5f;
    public float SmoothingTime = 0.05f;
    public float TimeAFKToAutoRotation = 10f;

    public Action OnHandleTapWoolAction;
    public Action<bool> OnHandleMouseAction;
    public Action OnHandleHoldWoolAction;
    public Action OnHandleDragWoolAction;

    public static int HoldClickTime = 0;

    #endregion

    #region Private Fields

    private Transform modelTransfrom;
    private float _acceleration;
    private Camera _mainCamera;
    private Camera _fakeUICamera;
    private float _timeIdle = 0f;
    private static bool _isActive = false;
    private bool _isClicking;
    private bool _isDragging;
    private bool _isHolding;
    private WoolControl _targetWool;
    private float totalIntroRotation;
    private Dictionary<int, Touch> activeTouches = new();
    private Touch firstTouch;
    private Touch secondTouch;
    private int firstTouchID = 0;
    private int secondTouchID = 0;
    private CancellationTokenSource _cts;
    private bool cameraForZoomReady = false;
    private bool isZooming = false;
    private float targetFOV;
    private float currentFOV;
    private float previousDistance;
    private float zoomLerpSpeed = 7f;
    private bool BlockRotation;
    private bool BlockZoom;
    private bool BlockHandTap;
    private bool _blockHold;
    private bool _blockDrag;
    private bool _isRecenteringModel;
    private Vector3 LocalScaleBackGroundDefault = new(60f, 60f, 1);

    private bool _introEnded = true;
    public float _introTimer = 0f;
    private RaycastHit[] hits = new RaycastHit[5];
    private Vector3 modelOriginalEA = Vector3.zero;
    #endregion

    #region Unity Lifecycle

    public override void Awake()
    {
        base.Awake();
        _mainCamera = CameraContainer.Instance.MainCamera;
        _fakeUICamera = CameraContainer.Instance.FakeUICamera;
        targetFOV = ZoomCameraData.DefaultFOV;
        cameraForZoomReady = ZoomCameraData != null && _mainCamera != null;
        originalSpawnPos = SpawnPoint.position;
    }

    private void OnEnable()
    {
        _acceleration = AccelerationRange.x;
        _timeIdle = 0f;
        ResetCameraStateMainMenu();
    }

    private void Start()
    {
        InputInteractable.OnTap += HandleTap;
        InputInteractable.OnHold += HandleHold;
        InputInteractable.OnDragAction += HandleDragSmoothly;
        InputInteractable.OnMouseDown += HandleMouse;
        //GameEventManager.OnGameStateChange += OnGameStateChange;
        OnGameStateChange();
        InputInteractable.enabled = true;
        GameEventManager.ChangeCameraFOVThroughButton += ZoomCameraAdditional;
        GameEventManager.ReCenterModelThroughButton += ReCenterModel;
        GameEventManager.OnAWoolMeshCompleted += () => { OnChangingCenterBaseOnModel(false); };
        GameEventManager.OnAWoolMeshRedo += () => { OnChangingCenterBaseOnModel(false); };
    }

    private void FixedUpdate()
    {
        if (!SpawnPoint || !modelTransfrom) return;

        UpdateRotateIntro();

        if (BlockRotation || !_introEnded) return;
        if (!_isActive) return;

        HandleModelRotation();
        HandleCameraZoom();
        HandleAutoRotation();
    }

    private void OnDestroy()
    {
        //GameEventManager.OnGameStateChange -= OnGameStateChange;
        GameEventManager.ChangeCameraFOVThroughButton -= ZoomCameraAdditional;
        GameEventManager.ReCenterModelThroughButton -= ReCenterModel;
        GameEventManager.OnAWoolMeshCompleted -= () => { OnChangingCenterBaseOnModel(false); };
        GameEventManager.OnAWoolMeshRedo -= () => { OnChangingCenterBaseOnModel(false); };
    }

    #endregion

    #region Public Methods
    public void Setup(GameObject levelObjectPrefab)
    {
        _isActive = true;
        BlockZoom = false;
        ModelPrefab = levelObjectPrefab;
        modelTransfrom = ModelPrefab.transform;
        targetRotation = SpawnPoint.rotation;
        modelOriginalEA = modelTransfrom.localEulerAngles;
        ModelPrefab.transform.localPosition = Vector3.zero;
        CurrentLevel = levelObjectPrefab.GetComponent<LevelController>();
        SpawnPoint.localEulerAngles = Vector3.zero;
        OnChangingCenterBaseOnModel(true);
    }

    public void SetBlockHandTap(bool isBlock) { BlockHandTap = isBlock; }

    public void BlockRotate(bool isBlock)
    {
        BlockRotation = isBlock;
        if (SpawnPoint) targetRotation = SpawnPoint.rotation;
    }

    public void SetBlockHold(bool isBlock) { _blockHold = isBlock; }

    public void SetBlockDrag(bool isBlock) { _blockDrag = isBlock; }

    public void ZoomCamera(float fovCam)
    {
        fovCam = Mathf.Clamp(fovCam, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
        var camForward = _mainCamera.transform.forward;
        var deltaFOV = fovCam - _mainCamera.fieldOfView;
        GameEventManager.ChangeCameraFOVThroughButton?.Invoke(deltaFOV);
        float scaleRatio = Mathf.Tan(fovCam * 0.5f * Mathf.Deg2Rad) /
            Mathf.Tan(ZoomCameraData.MaxFOV * 0.5f * Mathf.Deg2Rad);
        BackGround.localScale = LocalScaleBackGroundDefault * scaleRatio;
        BackGround.transform.position = _mainCamera.transform.position + camForward * 25;
        BackGround.transform.rotation = Quaternion.LookRotation(camForward);
        GamePlayUIManager.Instance.ChangeValueZoom(fovCam);
        if (ZoomStyle == ZoomCameraStyle.Smoothly) targetFOV = fovCam;
        _mainCamera.fieldOfView = fovCam;
    }

    public void ZoomCameraAdditional(float additionalFOV)
    {
        if (!ZoomCameraData || !_mainCamera) return;
        targetFOV = _mainCamera.fieldOfView + additionalFOV;
        targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
        return;
    }

    public void ResetCameraState()
    {
        if (!_mainCamera || !ZoomCameraData || !BackGround) return;

        _mainCamera.fieldOfView = ZoomCameraData.DefaultFOV;
        targetFOV = ZoomCameraData.DefaultFOV;

        _mainCamera.transform.position = _cameraPosGamePlayDefault;
        _mainCamera.transform.rotation = Quaternion.Euler(_cameraRoteGamePlayDefault);

        BackGround.localScale = LocalScaleBackGroundDefault;

        try
        {
            GamePlayUIManager.Instance?.ChangeValueZoom(_mainCamera.fieldOfView);
        }
        catch { }

        //Debug.Log($"Camera reset completed: Position={_mainCamera.transform.position}, Rotation={_mainCamera.transform.rotation.eulerAngles}, FOV={_mainCamera.fieldOfView}");
    }

    public void ResetCameraStateMainMenu()
    {
        if (!_mainCamera || !ZoomCameraData || !BackGround) return;
        _mainCamera.fieldOfView = ZoomCameraData.MainMenuFOV;
        BackGround.localScale = LocalScaleBackGroundDefault;
        GamePlayUIManager.Instance?.ChangeValueZoomNoNotify(_mainCamera.fieldOfView);

    }

    public void StartIntro()
    {
        //UIFullScreenBlocker.Instance.Lock(10);
        _isActive = false;
        GameEventManager.OnCountYarnComplete?.Invoke(false);
        GameEventManager.OnIntroComplete?.Invoke(_isActive);
        _introEnded = true;
        if (_cts != null && _cts.Token.CanBeCanceled)
            _cts.Cancel();
        _cts = new CancellationTokenSource();
        StartCoroutine(IntroExecuteAsync(_cts.Token));
    }

    #endregion

    #region Private Methods - Core Logic

    // private void SetActiveInteractable(GameState currentGameState)
    // {
    //     InputInteractable.enabled = currentGameState == GameState.InGame;
    // }

    private void OnGameStateChange()
    {
        try
        {

            StartCoroutine(SetCameraPos());
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in OnGameStateChange: {e.Message}");
        }
    }

    private void HandleModelRotation()
    {
        if (_isDragging || _timeIdle > 0)
        {
            if (DragStyle == DraggingStyle.Smoothly)
            {
                //modelTransfrom.rotation = Quaternion.Slerp(modelTransfrom.rotation, targetRotation,
                //        Time.fixedDeltaTime * SmoothFactor
                //    );
                SpawnPoint.rotation = Quaternion.Slerp(SpawnPoint.rotation, targetRotation,
                        Time.fixedDeltaTime * SmoothFactor
                    );
            }
        }
    }

    private void HandleCameraZoom()
    {
        if (BlockZoom) return;

        if (OnZoomCameraSmoothly())
        {
            _mainCamera.fieldOfView =
                Mathf.Lerp(_mainCamera.fieldOfView, targetFOV, Time.fixedDeltaTime * zoomLerpSpeed);
            ZoomCamera(_mainCamera.fieldOfView);
        }
        else
        {
            currentFOV = _mainCamera.fieldOfView;
            if (MathF.Abs(currentFOV - targetFOV) > 0.15f)
            {
                currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.fixedDeltaTime * zoomLerpSpeed);
                _mainCamera.fieldOfView = currentFOV;
            }
        }
    }

    private void HandleAutoRotation()
    {
        if (_isClicking) return;
        if (_timeIdle <= 0f)
        {
            SpawnPoint.Rotate(0f, RotationAutoSpeed * _acceleration * RotationSensitivity.x, 0f, Space.World);
            targetRotation = SpawnPoint.rotation;
        }
        else _timeIdle -= Time.fixedDeltaTime;
    }

    #endregion

    #region Private Methods - Input Handling

    private void HandleTap(Vector2 pos)
    {
        if (!_isActive) return;
        if (BlockHandTap) return;

        Ray ray = _mainCamera.ScreenPointToRay(pos);
        Ray rayFakeUI = _fakeUICamera.ScreenPointToRay(pos);

        if (Physics.Raycast(rayFakeUI, out RaycastHit hitFakeUI))
        {
            if (HandleFakeUITap(hitFakeUI)) return;
        }

        ClickEffectManager.Instance?.PlayClickEffect(pos, Color.white);
        TryTap(ray, pos);
    }

    private bool HandleFakeUITap(RaycastHit hitFakeUI)
    {
        var cubeTarget = hitFakeUI.collider.GetComponent<CubeTargetControl>();
        if (cubeTarget != null)
        {
            if (cubeTarget.IsActive) return false;
            //cubeTarget.OnOpenCubeByAds();
            return true;
        }

        var queueTarget = hitFakeUI.collider.GetComponent<QueueTargetControl>();
        if (queueTarget != null)
        {
            queueTarget.GetNewQueue();
            return true;
        }
        return false;
    }

    private void TryTap(Ray ray, Vector2 screenPosition, float radius = .1f)
    {
        try
        {
            if (Physics.Raycast(ray, out RaycastHit directHit))
            {
                if (HandleDirectHit(directHit, screenPosition)) return;
            }

            var wool = FindBestWoolHit(ray, radius);
            if (wool != null)
            {
                wool.WoolRotation();
                OnHandleTapWoolAction?.Invoke();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in TryTap: {e.Message}");
        }
    }

    private bool HandleDirectHit(RaycastHit hit, Vector2 screenPosition)
    {
        var decor = hit.collider.GetComponent<DecoreControl>();
        if (decor != null && decor.DecoreType == TypeOfDecore.Glass) return true;

        var wool = hit.collider.GetComponent<WoolControl>();
        if (wool != null)
        {
            wool.WoolRotation();
            OnHandleTapWoolAction?.Invoke();
            ClickEffectManager.Instance?.PlayClickEffect(screenPosition, Color.white);
            return true;
        }

        return false;
    }

    private void HandleMouse(bool isPointerDown)
    {
        if (!modelTransfrom) return;

        _isClicking = isPointerDown;
        _timeIdle = TimeAFKToAutoRotation;

        if (isPointerDown)
        {
            targetRotation = SpawnPoint.rotation;
        }

        if (_isHolding && !isPointerDown && _targetWool != null)
        {
            _isHolding = false;
            _targetWool.SetTranparentWool(false);
            _targetWool = null;
        }

        OnHandleMouseAction?.Invoke(isPointerDown);
        if (_isDragging)
            _isDragging = false;
    }

    private void HandleHold(Vector2 pos)
    {
        if (_blockHold) return;
        TryRayCastHold(pos);
    }

    private void TryRayCastHold(Vector2 pos, float radius = .025f)
    {
        try
        {
            Ray ray = _mainCamera.ScreenPointToRay(pos);

            if (Physics.Raycast(ray, out RaycastHit directionHit))
            {
                var wool = directionHit.collider.GetComponent<WoolControl>();
                if (wool != null)
                {
                    ActivateHoldOnWool(wool);
                    return;
                }
            }

            var bestWool = FindBestWoolHit(ray, radius);
            if (bestWool != null)
            {
                ActivateHoldOnWool(bestWool);
            }
            else
            {
                _targetWool = null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in TryRayCastHold: {e.Message}");
        }
    }

    private void ActivateHoldOnWool(WoolControl wool)
    {
        if (_targetWool != null)
            _targetWool.SetTranparentWool(false);
        _targetWool = wool;
        _isHolding = true;
        _targetWool.SetTranparentWool(true);
        OnHandleHoldWoolAction?.Invoke();
        HoldClickTime++;
    }

    private void HandleDragSmoothly(Vector2 pos)
    {
        if (!_isActive) return;
        if (_blockDrag) return;

        if (ZoomStyle == ZoomCameraStyle.Instantly)
        {
            if (OnZoomCamera()) return;
        }
        else if (isZooming) return;

        _isDragging = true;

        Vector3 rotateDir = new Vector3(pos.y, -pos.x, 0f) * DraggingSpeed;
        Quaternion delta = Quaternion.Euler(rotateDir);
        targetRotation = delta * targetRotation;

        if (rotateDir.sqrMagnitude > 5f) OnHandleDragWoolAction?.Invoke();
    }

    #endregion

    #region Private Methods - Camera Zoom

    private bool OnZoomCamera()
    {
        if (!ZoomCameraData || !_mainCamera) return false;

        if (Input.touchCount >= 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(t0.position, t1.position);

            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                previousDistance = currentDistance;
            }
            else if (t0.phase == TouchPhase.Moved || t1.phase == TouchPhase.Moved)
            {
                float deltaDistance = currentDistance - previousDistance;
                previousDistance = currentDistance;

                float fov = _mainCamera.fieldOfView;
                fov -= deltaDistance * ZoomCameraData.ZoomSpeed;

                fov = Mathf.Clamp(fov, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);

                ZoomCamera(fov);
            }

            return true;
        }

        return false;
    }

    private bool OnZoomCameraSmoothly()
    {
        if (!cameraForZoomReady) return false;

        if (Input.touchCount >= 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(t0.position, t1.position);

            if (!isZooming)
            {
                InitializeZoomGesture(t0, t1, currentDistance);
            }
            else
            {
                UpdateZoomGesture(t0, t1);
            }

            return true;
        }

        isZooming = false;
        return false;
    }

    private void InitializeZoomGesture(Touch t0, Touch t1, float currentDistance)
    {
        activeTouches = new Dictionary<int, Touch>();
        firstTouchID = t0.fingerId;
        secondTouchID = t1.fingerId;
        activeTouches.Add(firstTouchID, t0);
        activeTouches.Add(secondTouchID, t1);

        previousDistance = currentDistance;
        isZooming = true;
    }

    private void UpdateZoomGesture(Touch t0, Touch t1)
    {
        Vector2 lastDragPosition_1st = activeTouches[firstTouchID].position;
        Vector2 lastDragPosition_2nd = activeTouches[secondTouchID].position;

        UpdateActiveTouches(t0, t1);

        firstTouch = activeTouches[firstTouchID];
        secondTouch = activeTouches[secondTouchID];

        float currentDistance = Vector2.Distance(firstTouch.position, secondTouch.position);
        float deltaDistance = currentDistance - previousDistance;
        previousDistance = currentDistance;

        float screenScale = Mathf.Min(Screen.width, Screen.height);
        float normalizedDelta = deltaDistance / screenScale;

        targetFOV -= normalizedDelta * ZoomCameraData.ZoomSpeed * 500f;
        targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);

        HandleTouchRotation(lastDragPosition_1st, lastDragPosition_2nd);
    }

    private void UpdateActiveTouches(Touch t0, Touch t1)
    {
        if (t0.fingerId == firstTouchID)
        {
            activeTouches[firstTouchID] = t0;
            activeTouches[secondTouchID] = t1;
        }
        else if (t1.fingerId == firstTouchID)
        {
            activeTouches[firstTouchID] = t1;
            activeTouches[secondTouchID] = t0;
        }
    }

    private void HandleTouchRotation(Vector2 lastDragPosition_1st, Vector2 lastDragPosition_2nd)
    {
        if (firstTouch.phase == TouchPhase.Moved)
        {
            Vector2 delta = firstTouch.position - lastDragPosition_1st;
            InputInteractable.UpdateLastDragPosition(firstTouch.position);
            HandleRotation(delta);
        }
        else if (secondTouch.phase == TouchPhase.Moved)
        {
            Vector2 delta = secondTouch.position - lastDragPosition_2nd;
            InputInteractable.UpdateLastDragPosition(secondTouch.position);
            HandleRotation(delta);
        }
    }

    private void HandleRotation(Vector3 pos)
    {
        Vector3 rotateDir = new Vector3(pos.y, -pos.x, 0f) * DraggingSpeed;
        Quaternion delta = Quaternion.Euler(rotateDir);
        targetRotation = delta * targetRotation;
    }

    private void ReCenterModel()
    {
        if (_isRecenteringModel) return;
        if (SpawnPoint == null) return;

        _isRecenteringModel = true;
        GameEventManager.SetReCenterButtonInteractable?.Invoke(false);
        _timeIdle = TimeAFKToAutoRotation;
        BlockRotation = true;
        SpawnPoint
           .DOLocalRotate(Vector3.zero, 0.5f)
           .SetEase(Ease.InOutCubic)
           .OnComplete(() =>
           {
               SpawnPoint.rotation = Quaternion.identity;
               targetRotation = SpawnPoint.rotation;
               BlockRotation = false;
               _isRecenteringModel = false;
               GameEventManager.SetReCenterButtonInteractable?.Invoke(true);
           });
        //modelTransfrom
        //   .DOLocalRotate(modelOriginalEA, 0.5f)
        //   .SetEase(Ease.InOutCubic)
        //   .OnComplete(() =>
        //   {
        //       BlockRotation = false;
        //       targetRotation = modelTransfrom.rotation;
        //   });
    }

    #endregion

    #region Private Methods - Intro

    private void UpdateRotateIntro()
    {
        if (_introEnded) return;
        if (GamePlayManager.Instance.IsWaitingForFakeLoading) return;

        if (_introTimer >= IntroLenght)
        {
            CompleteIntroRotation();
            return;
        }

        _introTimer += Time.fixedDeltaTime;

        float rotationThisFrame = CalculateIntroRotationFrame();
        modelTransfrom.Rotate(0f, rotationThisFrame, 0f, Space.World);

        if (enableExact360Rotation)
        {
            totalIntroRotation += rotationThisFrame;
        }
    }

    private float CalculateIntroRotationFrame()
    {
        if (enableExact360Rotation)
        {
            return (360f / IntroLenght) * Time.fixedDeltaTime;
        }
        else
        {
            return ModelRotationIntroSpeed * Time.fixedDeltaTime;
        }
    }

    private void CompleteIntroRotation()
    {
        _introEnded = true;

        if (enableExact360Rotation)
        {
            float remainingRotation = 360f - totalIntroRotation;
            if (Mathf.Abs(remainingRotation) > 0.1f)
            {
                modelTransfrom.Rotate(0f, remainingRotation, 0f, Space.World);
            }
        }

        if (modelTransfrom != null)
        {
            targetRotation = SpawnPoint.rotation;
        }
    }

    private IEnumerator IntroExecuteAsync(CancellationToken token)
    {
        if (!_mainCamera || !modelTransfrom || !GamePlayManager.Instance) yield break;

        BlockHandTap = true;
        targetFOV = IntroEndFOV;
        _mainCamera.fieldOfView = IntroStartFOV;

        _introTimer = 0;
        _introEnded = false;
        totalIntroRotation = 0;

        SoundManager.Instance.PlayOneShot(introRotateSound);
        yield return new WaitForSeconds(IntroLenght);
        if (token.IsCancellationRequested) yield break;
        SoundManager.Instance.PlayOneShot(introFinishRotateSound);

        GameEventManager.OnIntroComplete?.Invoke(true);
        yield return null;
        yield return null;

        _mainCamera
           .DOFieldOfView(IntroEndFOV, IntroCameraZoomInDuration)
           .SetEase(Ease.InOutSine);
        GamePlayManager.Instance.ActiveHandController(true);
        BlockRotate(true);
        yield return new WaitForSeconds(IntroCameraZoomInDuration);
        if (token.IsCancellationRequested) yield break;

        GamePlayUIManager.Instance?.ChangeValueZoom(IntroEndFOV);

        if (modelTransfrom != null)
        {
            targetRotation = SpawnPoint.rotation;
        }

        _isActive = true;
        GameEventManager.OnIntroComplete2?.Invoke(true);
        yield return new WaitForSeconds(0.3f);
        if (token.IsCancellationRequested) yield break;
        BlockHandTap = false;
        //UIFullScreenBlocker.Instance?.Unlock(10);
    }

    #endregion

    #region Private Methods - Camera Position

    private IEnumerator SetCameraPos()
    {
        yield return new WaitForSeconds(0.02f);

        _mainCamera.transform.position = _cameraPosGamePlayDefault;
        _mainCamera.transform.rotation = Quaternion.Euler(_cameraRoteGamePlayDefault);


    }

    #endregion

    #region SUPPORTIVE
    private WoolControl FindBestWoolHit(Ray ray, float radius)
    {
        var size = Physics.SphereCastNonAlloc(ray, radius, hits);

        if (size == 0) return null;

        WoolControl bestWool = null;
        float bestScore = float.MaxValue;

        for (var index = 0; index < size; index++)
        {
            RaycastHit hit = hits[index];
            var wool = hit.collider.GetComponent<WoolControl>();
            if (wool == null) continue;

            Vector3 pointToOrigin = hit.point - ray.origin;
            float projectionDistance = Vector3.Dot(pointToOrigin, ray.direction);
            float score = projectionDistance;

            if (score < bestScore)
            {
                bestScore = score;
                bestWool = wool;
            }
        }

        return bestWool;
    }

    private void OnChangingCenterBaseOnModel(bool immediately)
    {
        if (!AutoCenterModel) return;
        if (!AllowToAutoCenter()) return;
        if (CurrentLevel == null) return;

        WoolControl farthestWool, nearestWool;

        Vector3 modelCurrentlyCenterPos = CurrentLevel.GetEnabledWoolCenters(UseRendererBoundsCenter, out farthestWool, out nearestWool);
        if (immediately)
        {
            nearestWoolDistance = Vector3.Distance(nearestWool.transform.position, modelCurrentlyCenterPos);
            farthestWoolDistance = Vector3.Distance(farthestWool.transform.position, modelCurrentlyCenterPos);
        }

        bool needToChangeCenter = Vector3.Distance(SpawnPoint.position, modelCurrentlyCenterPos) >= ThresholdToChangeCenter;

        if (!needToChangeCenter) return;

        if (!immediately && farthestWool) ZoomCameraBasedOnModel(farthestWool, modelCurrentlyCenterPos);

        Transform child = CurrentLevel.transform;

        Transform oldParent = child.parent;

        child.SetParent(null, true);

        SpawnPoint.position = modelCurrentlyCenterPos;

        child.SetParent(oldParent, true);

        if (immediately)
        {
            SpawnPoint.position = originalSpawnPos;
        }
        else
        {
            SpawnPoint.DOMove(originalSpawnPos, 0.5f).SetEase(Ease.InOutSine);
        }
    }

    private void ZoomCameraBasedOnModel(WoolControl curentFarthestWool, Vector3 center)
    {
        currentFOV = _mainCamera.fieldOfView;
        float distanceToCurrentFarthestWool = Vector3.Distance(curentFarthestWool.transform.position, center);
        float distanceRatio = Mathf.InverseLerp(nearestWoolDistance, farthestWoolDistance, distanceToCurrentFarthestWool);
        float targetFOVBasedOnModel = Mathf.Lerp(ZoomCameraData.MinFOV, IntroEndFOV, distanceRatio);
        targetFOV = (int)targetFOVBasedOnModel;
        GamePlayUIManager.Instance.ChangeValueZoomNoNotify(targetFOV);
    }

    private bool AllowToAutoCenter()
    {
        if (CurrentLevel == null) return false;
        int completedWools = 0;
        int totalWool = CurrentLevel.WoolControls.Count;
        foreach (var wool in CurrentLevel.WoolControls)
        {
            if (wool == null) continue;
            if (!wool.TopMeshRenderer.enabled || !wool.gameObject.activeSelf) completedWools++;
        }

        if ((float)completedWools / (float)totalWool * 100f <= (100f - WoolPercentLeftStopAutoZoom)) return true;
        return false;
    }
    #endregion
}

public enum DraggingStyle
{
    Instantly,
    Smoothly
}

public enum ZoomCameraStyle
{
    Instantly,
    Smoothly
}
