using System.Collections;
using DG.Tweening;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CameraController : Singleton<CameraController>
{
    #region PROPERTIES

    public Interactable InputInteractable;

    public Button btnReCenterModel;
    private bool canResset;
    public Slider ZoomSlider;
    public float maxZoom;
    public float minZoom;
    public ZoomCameraData ZoomCameraData;
    public Transform BackGround;

    public Transform SpawnPoint;
    public GameObject ModelPrefab;
    private Transform modelTransfrom;
    public Quaternion targetRotation;

    public float Friction = 3f; // The speed of decay of inertia
    public Vector2 RotationSensitivity = new Vector2(1f, 1f); // Giới hạn tốc độ xoay
    public Vector2 AccelerationRange = new Vector2(0.1f, 1f); // Giới hạn tốc độ xoay
    public float RotationSpeed = 5f; // Tốc độ xoay
    public float RotationAutoSpeed = 0.5f; // Tốc độ xoay tự động
    public float SmoothingTime = 0.05f;
    [Header("Rotation")] public float TimeAFKToAutoRotation = 10f;
    // Thêm các thuộc tính này vào khu vực PROPERTIES ở đầu class

    private RaycastHit[] _woolHits = new RaycastHit[10];

    [Header("Tapping Settings")]
    [Tooltip("Bán kính của vùng tìm kiếm lân cận khi người chơi tap trượt.")]
    [SerializeField]
    private float _tapRadius = 0.2f;

    // ===== BỔ SUNG: CÁC THUỘC TÍNH CHO ZOOM 2 NGÓN TAY =====
    [Header("TOUCH ZOOM SETTINGS")]
    [SerializeField]
    [Tooltip("Độ nhạy khi zoom bằng 2 ngón tay (1.0f = bình thường, 2.0f = nhạy gấp đôi)")]
    private float touchZoomSensitivity = 2f;

    [SerializeField] [Tooltip("Bật/tắt tính năng zoom bằng 2 ngón tay")]
    private bool enableTouchZoom = true;

    [SerializeField] [Tooltip("Thời gian smooth khi zoom bằng touch (không sử dụng hiện tại)")]
    private float touchZoomSmoothTime = 0.1f;

    // Biến để theo dõi trạng thái zoom - BỔ SUNG để tránh xung đột giữa touch zoom và slider
    private bool isTouchZooming = false;
    private bool isSliderUpdating = false; // Tránh vòng lặp khi cập nhật slider từ code
    // ===== KẾT THÚC BỔ SUNG =====

    private float _acceleration;
    private float _timerAfterMouseUp = 0f;
    private float _timerAfterMouseDown = 0f;
    private Vector2 _previousDelta = Vector2.zero;
    private Vector3 _lastMousePosition;

    private Vector2 _sceenRate;

    private Camera _mainCamera;
    private Camera _fakeUICamera;
    private GameObject _targetObject;

    private bool _isIdling = false;
    private float _timeIdle = 0f;

    private bool _isActive = false;

    private bool _isClickOnMesh;
    private float _timerHoldClick;

    private bool _isClicking;
    private bool _isDragging;
    private bool _isHolding;
    [SerializeField] private bool _isRotateObjectInMainMenu;

    private WoolControl _targetWool;

    [SerializeField] private Vector3 _cameraPosMainMenuDefault = new Vector3(0, 1.8f, -11);
    [SerializeField] private Vector3 _cameraRoteMainMenuDefault = new Vector3(15f, 0, 0);
    [SerializeField] private Vector3 _cameraPosGamePlayDefault = new Vector3(0, 1, -10);
    [SerializeField] private Vector3 _cameraRoteGamePlayDefault = new Vector3(6, 0, 0);

    [Header("INTRO SETTING(s)")] public float IntroLenght = 2f;
    public float ModelRotationIntroSpeed = 0.5f;
    public float IntroCameraZoomInDuration = 0.5f;
    public int IntroStartFOV = 65;
    public int IntroEndFOV = 65;
    private Coroutine introCoroutine;

    [Header("DRAGGING SETTING(s)")] public DraggingStyle DragStyle;
    public float DraggingSpeed = 0.25f;
    public float SmoothFactor = 7;

    [Header("DRAGGING SETTING(s)")] public ZoomCameraStyle ZoomStyle;
    private bool isZooming = false;
    private float targetFOV;
    private float currentFOV;
    private float previousDistance;
    private float zoomLerpSpeed = 7f;

    // Đống này của thằng Đồng nó vất linh tinh đm đéo biết để đâu
    private bool BlockRotation;
    public  void SetBlockRotation(bool _bool)=> BlockRotation = _bool;
    private bool BlockZoom;
    public Action<bool> OnHandleMouseAction;
    private bool BlockHandTap;
    private bool _blockHold;
    public Action OnHandleHoldWoolAction;
    private bool _blockDrag;
    public Action OnHandleDragWoolAction;
    private Vector3 LocalScaleBackGroundDefault = new Vector3(40f, 40f, 1);

    // Lưu rotation ban đầu để reset
    private Quaternion initialRotation;
    private bool hasStoredInitialRotation = false;

    [Header("Reset Settings")] public float ResetRotationDuration = 1f;
    public Ease ResetRotationEase = Ease.InOutCubic;

    bool isTap;
    #endregion

    #region UNITY_METHODS

    public override void Awake()
    {
        base.Awake();
        _mainCamera = CameraContainer.Instance.MainCamera;
        _fakeUICamera = CameraContainer.Instance.FakeUICamera;
        targetFOV = ZoomCameraData.DefaultFOV;
    }

    private void OnEnable()
    {
        _timerAfterMouseUp = 0;
        _acceleration = AccelerationRange.x;
        _timeIdle = 0f;
        ResetCamearState();
    }

    private void Start()
    {
        InputInteractable.OnTap += HandleTap;
        InputInteractable.OnHold += HandleHold;
        //InputInteractable.OnDragAction += HandleDrag;
        InputInteractable.OnDragAction += HandleDragSmoothly;
        InputInteractable.OnMouseDown += HandleMouse;
    }
    private void LateUpdate()
    {
        if (!isTap && Input.GetMouseButton(0))
        {
            isTap = true;
            GamePlaySystem.Instance.ActiveHandController(false);
        }
    }
    private void Update()
    {
        if (BlockRotation || !SpawnPoint || !modelTransfrom) return;
        if (_isRotateObjectInMainMenu)
        {
            SpawnPoint.Rotate(0f, RotationAutoSpeed * _acceleration * RotationSensitivity.x, 0f, Space.World);
            return;
        }

        if (!_isActive) return;

        if (_isDragging || _timeIdle > 0)
        {
            if (DragStyle == DraggingStyle.Smoothly || DragStyle == DraggingStyle.SmoothlyYAxis)
            {
                modelTransfrom.rotation =
                    Quaternion.Slerp(modelTransfrom.rotation, targetRotation, Time.deltaTime * SmoothFactor);
            }
        }

        if (!BlockZoom)
        {
            if (_mainCamera.fieldOfView <= targetFOV - .1f)
                BlockZoom = true;
            if (OnZoomCameraSmoothly())
            {
                _mainCamera.fieldOfView =
                    Mathf.Lerp(_mainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomLerpSpeed);
                ZoomCamera(_mainCamera.fieldOfView);
            }
            else
            {
                currentFOV = _mainCamera.fieldOfView;
                if (MathF.Abs(currentFOV - targetFOV) > 0.15f)
                {
                    currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * zoomLerpSpeed);
                    _mainCamera.fieldOfView = currentFOV;

                    // ===== BỔ SUNG: Cập nhật slider trong chế độ smooth =====
                    // Khi camera zoom smooth, slider cũng cần được cập nhật để đồng bộ
                    if (ZoomStyle == ZoomCameraStyle.Smoothly && !isTouchZooming)
                    {
                        UpdateSliderFromFOV(currentFOV);
                    }
                    // ===== KẾT THÚC BỔ SUNG =====
                }
            }
        }

        if (_isClicking) return;

        if (_timeIdle <= 0f)
        {
            // if (DragStyle == DraggingStyle.SmoothlyYAxis)
            //     SpawnPoint.Rotate(0f, RotationAutoSpeed * _acceleration * RotationSensitivity.x, 0f, Space.World);
            // else if (DragStyle == DraggingStyle.Smoothly)
            //     SpawnPoint.Rotate(Vector3.up, RotationAutoSpeed * _acceleration * RotationSensitivity.x, Space.World);
        }
        else
        {
            _timeIdle -= Time.deltaTime;
        }
    }

    #endregion

    #region MAIN_METHODS

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    public void Setup(GameObject levelObjectPrefab)
    {
        _isActive = true;
        BlockZoom = false;
        ModelPrefab = levelObjectPrefab;
        modelTransfrom = ModelPrefab.transform;
        targetRotation = modelTransfrom.rotation;
        btnReCenterModel.onClick.AddListener(() => ResetToInitialRotation());
        canResset = true;
        StoreInitialRotation();
        // Thiết lập giá trị slider
        if (ZoomSlider != null)
        {
            ZoomSlider.minValue = 0f;
            ZoomSlider.maxValue = 1f;

            // ===== SỬA ĐỔI: Thay thế cách khởi tạo slider =====
            // CODE CŨ (đã comment): Không khởi tạo giá trị mặc định cho slider
            // float currentFOV = _mainCamera.fieldOfView;
            // float normalizedValue = (currentFOV - minZoom) / (maxZoom - minZoom);
            // ZoomSlider.value = normalizedValue;

            // CODE MỚI: Khởi tạo slider dựa trên FOV hiện tại để đồng bộ
            float currentFOV = _mainCamera != null ? _mainCamera.fieldOfView : ZoomCameraData.DefaultFOV;
            UpdateSliderFromFOV(currentFOV);

            // SỬA ĐỔI: Dọn dẹp listener cũ trước khi thêm mới để tránh duplicate
            //ZoomSlider.onValueChanged.RemoveAllListeners();
            // ===== KẾT THÚC SỬA ĐỔI =====

            // Thêm listener cho sự kiện thay đổi slider
            ZoomSlider.onValueChanged.AddListener(OnZoomSliderChanged);
        }
    }

    #region _input

    #region SLIDER_ZOOM_METHODS

    // ===== SỬA ĐỔI: Cải tiến phương thức OnZoomSliderChanged =====
    // Phương thức được gọi khi slider thay đổi giá trị
    public void OnZoomSliderChanged(float value)
    {
        // THÊM: Kiểm tra isSliderUpdating để tránh vòng lặp khi cập nhật từ code
        if(!canResset) return;

        float newFOV = Mathf.Lerp(minZoom, maxZoom, value);

        // Cập nhật targetFOV để touch zoom tiếp tục hoạt động
        targetFOV = newFOV;

        if (ZoomStyle == ZoomCameraStyle.Smoothly)
        {
            // Smooth: chỉ update targetFOV, phần Update() sẽ Lerp tới giá trị này
        }
        else
        {
            _mainCamera.fieldOfView = newFOV;
        }

        ZoomCamera(newFOV); // Giữ đồng bộ background + slider
    }
    // ===== KẾT THÚC SỬA ĐỔI =====

    // ===== BỔ SUNG: Phương thức mới để cập nhật slider từ FOV =====
    /// <summary>
    /// Cập nhật giá trị slider dựa trên FOV hiện tại của camera
    /// Được gọi khi zoom bằng 2 ngón tay để đồng bộ slider
    /// </summary>
    private void UpdateSliderFromFOV(float currentFOV)
    {
        if (ZoomSlider != null && !isSliderUpdating)
        {
            isSliderUpdating = true;

            // Chuyển đổi FOV thành giá trị slider (0-1)
            float normalizedValue = (currentFOV - minZoom) / (maxZoom - minZoom);
            normalizedValue = Mathf.Clamp01(normalizedValue);

            // Mượt hơn khi thay đổi
            ZoomSlider.value = Mathf.Lerp(ZoomSlider.value, normalizedValue, Time.deltaTime * 10f);

            isSliderUpdating = false;
        }
    }
    // ===== KẾT THÚC BỔ SUNG =====

    // // ===== BỔ SUNG: Các phương thức tiện ích cho zoom =====
    // /// <summary>
    // /// Đặt mức zoom theo giá trị từ 0-1 và cập nhật slider
    // /// </summary>
    // public void SetZoomLevel(float zoomLevel)
    // {
    //     // zoomLevel từ 0 (zoom max) đến 1 (zoom min)
    //     zoomLevel = Mathf.Clamp01(zoomLevel);
    //
    //     float newFOV = Mathf.Lerp(minZoom, maxZoom, zoomLevel);
    //
    //     if (ZoomStyle == ZoomCameraStyle.Smoothly)
    //     {
    //         targetFOV = newFOV;
    //     }
    //     else
    //     {
    //         ZoomCamera(newFOV);
    //     }
    //
    //     // Cập nhật slider
    //     UpdateSliderFromFOV(newFOV);
    // }
    //
    // /// <summary>
    // /// Zoom in một lượng nhất định
    // /// </summary>
    // public void ZoomIn(float amount = 0.1f)
    // {
    //     float currentValue = ZoomSlider != null ? ZoomSlider.value : 0.5f;
    //     SetZoomLevel(currentValue + amount); // + vì slider đảo ngược
    // }
    //
    // /// <summary>
    // /// Zoom out một lượng nhất định
    // /// </summary>
    // public void ZoomOut(float amount = 0.1f)
    // {
    //     float currentValue = ZoomSlider != null ? ZoomSlider.value : 0.5f;
    //     SetZoomLevel(currentValue - amount); // - vì slider đảo ngược
    // }
    //
    // /// <summary>
    // /// Reset zoom về giá trị mặc định
    // /// </summary>
    // public void ResetZoomToDefault()
    // {
    //     float defaultFOV = ZoomCameraData.DefaultFOV;
    //
    //     if (ZoomStyle == ZoomCameraStyle.Smoothly)
    //     {
    //         targetFOV = defaultFOV;
    //     }
    //     else
    //     {
    //         ZoomCamera(defaultFOV);
    //     }
    //
    //     UpdateSliderFromFOV(defaultFOV);
    // }
    // ===== KẾT THÚC BỔ SUNG =====

    // // Phương thức zoom bằng code (tùy chọn) - GIỮ NGUYÊN CODE CŨ (COMMENTED)
    // public void SetZoomLevel(float zoomLevel)
    // {
    //     // zoomLevel từ 0 (zoom max) đến 1 (zoom min)
    //     zoomLevel = Mathf.Clamp01(zoomLevel);
    //     
    //     if (ZoomSlider != null)
    //         ZoomSlider.value = zoomLevel;
    //         
    //     OnZoomSliderChanged(zoomLevel);
    // }
    //
    // // Phương thức zoom in
    // public void ZoomIn(float amount = 0.1f)
    // {
    //     float currentValue = ZoomSlider != null ? ZoomSlider.value : 0.5f;
    //     SetZoomLevel(currentValue - amount);
    // }
    //
    // // Phương thức zoom out
    // public void ZoomOut(float amount = 0.1f)
    // {
    //     float currentValue = ZoomSlider != null ? ZoomSlider.value : 0.5f;
    //     SetZoomLevel(currentValue + amount);
    // }
    //
    // void OnDestroy()
    // {
    //     // Dọn dẹp listener khi object bị destroy
    //     if (ZoomSlider != null)
    //         ZoomSlider.onValueChanged.RemoveListener(OnZoomSliderChanged);
    // }

    #endregion

    #region RESET_ROTATION_METHODS

    /// <summary>
    /// Lưu rotation ban đầu của model
    /// </summary>
    public void StoreInitialRotation()
    {
        if (modelTransfrom != null)
        {
            initialRotation = modelTransfrom.rotation;
            hasStoredInitialRotation = true;
//            Debug.Log("Initial rotation stored: " + initialRotation.eulerAngles);
        }
    }

    /// <summary>
    /// Reset model về rotation ban đầu với animation mượt
    /// </summary>
    public void ResetToInitialRotation()
    {
        if (!hasStoredInitialRotation || modelTransfrom == null || !canResset)
        {
            Debug.LogWarning("Initial rotation not stored or model not found!");
            return;
        }

        canResset = false;
        // Block rotation during reset
        BlockRotation = true;

        // Animate back to initial rotation
        modelTransfrom.DORotate(initialRotation.eulerAngles, ResetRotationDuration)
            .SetEase(ResetRotationEase)
            .OnComplete(() =>
            {
                BlockRotation = false;
                canResset = true;
                targetRotation = initialRotation;
                Debug.Log("Model reset to initial rotation");
            });
    }

    #endregion

    // ===== BỔ SUNG: Các phương thức điều khiển tính năng zoom =====
    /// <summary>
    /// Bật/tắt tính năng zoom bằng 2 ngón tay
    /// </summary>
    public void SetTouchZoomEnabled(bool enabled)
    {
        enableTouchZoom = enabled;
        Debug.Log("Touch zoom enabled: " + enabled);
    }

    /// <summary>
    /// Điều chỉnh độ nhạy zoom bằng 2 ngón tay
    /// </summary>
    public void SetTouchZoomSensitivity(float sensitivity)
    {
        touchZoomSensitivity = Mathf.Clamp(sensitivity, 0.1f, 10f);
        Debug.Log("Touch zoom sensitivity set to: " + touchZoomSensitivity);
    }
    // ===== KẾT THÚC BỔ SUNG =====

    public void SetBlockHandTap(bool isBlock)
    {
        BlockHandTap = isBlock;
        Debug.Log("Block hand tap: " + isBlock);
    }

    public void BlockRotate(bool isBlock)
    {
        BlockRotation = isBlock;
        targetRotation = modelTransfrom.rotation;
    }

    public void SetBlockHold(bool isBlock)
    {
        _blockHold = isBlock;
        Debug.Log("Block hold: " + isBlock);
    }

    public void SetBlockDrag(bool isBlock)
    {
        _blockDrag = isBlock;
        Debug.Log("Block drag: " + isBlock);
    }

    public LayerMask layerMask;

    private void HandleTap(Vector2 pos)
    {
        if (BlockHandTap) return;

        Ray ray = _mainCamera.ScreenPointToRay(pos);

        WoolControl foundWool = FindBestWoolNearRay(ray);

        if (foundWool != null)
        {
            HandleFoundWool(foundWool);
            ClickEffectManager.Instance.PlayClickEffect(foundWool.GetColor());
//            print("-------Click:"+foundWool.GetColor());
        }
        else
        {
//            print("-------Click: Null");
            ClickEffectManager.Instance.PlayClickEffect(Color.white);
        }
        
    }

    /// <summary>
    /// Tạo một phương thức phụ để xử lý WoolControl, tránh lặp code.
    /// </summary>
    private void HandleFoundWool(WoolControl wool)
    {
        // Đây là logic gốc từ HandleTap của bạn
        wool.WoolRotation();
        GamePlaySystem.Instance.RaiseMotion(EMotionType.Shy, Random.value);
        //Debug.Log("Tapped on Wool: " + wool.name);
    }

    private void HandleMouse(bool isPointerDown)
    {
        _isClicking = isPointerDown;
        _timeIdle = TimeAFKToAutoRotation;
        if (_isHolding && !isPointerDown && _targetWool != null)
        {
            _isHolding = false;
            _targetWool.SetTranparentWool(false);
            _targetWool = null;
            Debug.Log("Pointer up");
        }

        OnHandleMouseAction?.Invoke(isPointerDown);
        if (_isDragging)
            _isDragging = false;
    }

    public static int HoldClickTime = 0;

    /// <summary>
    /// Tìm kiếm WoolControl tốt nhất gần một tia Ray.
    /// Ưu tiên tìm kiếm bằng Raycast trực tiếp, nếu thất bại sẽ dùng SphereCast.
    /// </summary>
    /// <param name="ray">Tia ray từ camera theo hướng con trỏ.</param>
    /// <returns>Trả về WoolControl tốt nhất tìm được, hoặc null nếu không có.</returns>
    private WoolControl FindBestWoolNearRay(Ray ray)
    {
        // Bước 1: Ưu tiên Raycast trực tiếp
        if (Physics.Raycast(ray, out RaycastHit directHit, 100f, layerMask))
        {
            if (directHit.collider.gameObject.TryGetComponent<WoolControl>(out var directWool))
            {
                return directWool; // Tìm thấy, trả về ngay lập tức
            }
        }

        // Bước 2: Tìm kiếm lân cận bằng SphereCast nếu Raycast trượt
        int hitCount = Physics.SphereCastNonAlloc(ray, _tapRadius, _woolHits, 100f, layerMask);

        if (hitCount > 0)
        {
            WoolControl bestWool = null;
            float bestScore = float.MaxValue;

            for (int i = 0; i < hitCount; i++)
            {
                if (_woolHits[i].collider.gameObject.TryGetComponent<WoolControl>(out var wool))
                {
                    // Tính điểm dựa trên khoảng cách
                    Vector3 pointToOrigin = _woolHits[i].point - ray.origin;
                    float score = Vector3.Dot(pointToOrigin, ray.direction);

                    if (score < bestScore)
                    {
                        bestScore = score;
                        bestWool = wool;
                    }
                }
            }

            return bestWool; // Trả về đối tượng tốt nhất tìm được trong vùng lân cận
        }

        return null; // Không tìm thấy bất kỳ đối tượng nào
    }

    private void HandleHold(Vector2 pos)
    {
        if (_blockHold) return;

        Ray ray = _mainCamera.ScreenPointToRay(pos);

        // Sử dụng lại logic tìm kiếm thông minh từ HandleTap
        WoolControl foundWool = FindBestWoolNearRay(ray);

        // Nếu tìm thấy một khối len phù hợp (dù là chạm trúng hay chạm gần)
        if (foundWool != null)
        {
            // Nếu chúng ta đang giữ một khối len khác, hãy trả nó về trạng thái bình thường trước
            if (_isHolding && _targetWool != foundWool)
            {
                _targetWool.SetTranparentWool(false);
            }

            // Cập nhật khối len mục tiêu mới
            _targetWool = foundWool;

            // Nếu chưa ở trạng thái "holding", hãy kích hoạt nó
            if (!_isHolding)
            {
                _isHolding = true;
                OnHandleHoldWoolAction?.Invoke();
                HoldClickTime++;
            }

            // Áp dụng hiệu ứng cho khối len đang được giữ
            _targetWool.SetTranparentWool(true);
        }
        else
        {
            // Nếu không tìm thấy khối len nào gần đó,
            // và chúng ta đang trong trạng thái "holding", hãy hủy trạng thái đó.
            if (_isHolding && _targetWool != null)
            {
                _isHolding = false;
                _targetWool.SetTranparentWool(false);
                _targetWool = null;
            }
        }
    }

    private Vector2 AdjustLunaMousePosition(Vector2 originalPos)
    {
        // Điều chỉnh tọa độ dựa trên tỷ lệ khung hình của Luna
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Luna thường sử dụng tỷ lệ canvas khác với kích thước thực tế
        // Các giá trị offset này cần được điều chỉnh dựa trên thử nghiệm
        float offsetX = 0f; // Thử các giá trị khác nhau
        float offsetY = 0f; // Thử các giá trị khác nhau

        // Điều chỉnh tọa độ
        return new Vector2(originalPos.x * screenWidth / 886, originalPos.y * screenHeight / 1920) +
               new Vector2(offsetX, offsetY);
    }

    private void HandleDrag(Vector2 pos)
    {
        if (!_isActive) return;
        if (OnZoomCamera()) return;

        if (_blockDrag || DragStyle != DraggingStyle.Instantly) return;

        /*if (MainTutorialLayer.IsInTutorial)
        {
            ModelPrefab.transform.DORotate(_rotationTargetDefault, 0.8f);
            OnHandleDragWoolAction?.Invoke();
            return;
        }*/

        if (DragStyle == DraggingStyle.SmoothlyYAxis)
        {
            float yRotation = -pos.x * RotationSpeed;
            ModelPrefab.transform.Rotate(0f, yRotation, 0f, Space.World);
        }
        else
        {
            Vector3 rotateDir = new Vector3(pos.y, -pos.x, 0f) * RotationSpeed;
            ModelPrefab.transform.Rotate(rotateDir, Space.World);
        }

        OnHandleDragWoolAction?.Invoke();
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

        // Check if we should handle rotation based on drag style
        bool isYAxisMode = DragStyle == DraggingStyle.SmoothlyYAxis;
        bool isSmoothMode = DragStyle == DraggingStyle.Smoothly || DragStyle == DraggingStyle.SmoothlyYAxis;

        _isDragging = true;

        if (DragStyle == DraggingStyle.SmoothlyYAxis)
        {
            float yRotation = -pos.x * DraggingSpeed;
            Quaternion delta = Quaternion.Euler(0f, yRotation, 0f);
            targetRotation = delta * targetRotation;
        }
        else
        {
            Vector3 rotateDir = new Vector3(pos.y, -pos.x, 0f) * DraggingSpeed;
            Quaternion delta = Quaternion.Euler(rotateDir);
            targetRotation = delta * targetRotation;
        }

        OnHandleDragWoolAction?.Invoke();
    }

    private void ReCenterModel()
    {
        BlockRotation = true;
        if (false)
        {
            _mainCamera
                .DOFieldOfView(ZoomCameraData.DefaultFOV, 0.5f)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() =>
                    {
                        var endFov = ZoomCameraData.DefaultFOV - 5;
                        targetFOV = endFov;
                    }
                );
        }

        modelTransfrom
            .DORotate(Vector3.zero, 0.5f)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
                {
                    BlockRotation = false;
                    targetRotation = modelTransfrom.rotation;
                }
            );
    }

    #endregion

    #region _zoom camera

    // ===== SỬA ĐỔI: Cải tiến phương thức OnZoomCamera cho zoom 2 ngón tay =====
    private bool OnZoomCamera()
    {
        // THÊM: Kiểm tra enableTouchZoom để có thể tắt tính năng
        if (!enableTouchZoom || !ZoomCameraData || !_mainCamera) return false;

        if (Input.touchCount >= 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                previousDistance = currentDistance;
                // THÊM: Đánh dấu đang zoom bằng touch để tránh cập nhật slider không cần thiết
                isTouchZooming = true;
            }
            else if ((touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) && isTouchZooming)
            {
                float deltaDistance = currentDistance - previousDistance;
                previousDistance = currentDistance;

                // SỬA ĐỔI: Tính toán zoom dựa trên kích thước màn hình để có trải nghiệm nhất quán
                // CODE CŨ: float fov = _mainCamera.fieldOfView; fov -= deltaDistance * ZoomCameraData.ZoomSpeed;
                // CODE MỚI: Tính toán dựa trên đường chéo màn hình
                float screenDiagonal = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
                float normalizedDelta = (deltaDistance / screenDiagonal) * touchZoomSensitivity;

                float fov = _mainCamera.fieldOfView - (normalizedDelta * 100f);
                fov = Mathf.Clamp(fov, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);

                ZoomCamera(fov);

                // THÊM: Cập nhật slider để đồng bộ với zoom touch
                UpdateSliderFromFOV(fov);
            }

            // THÊM: Reset trạng thái khi không còn touch
            if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended ||
                touch0.phase == TouchPhase.Canceled || touch1.phase == TouchPhase.Canceled)
            {
                isTouchZooming = false;
            }

            return true;
        }

        // THÊM: Reset trạng thái khi không còn đủ 2 ngón tay
        isTouchZooming = false;
        return false;
    }
    // ===== KẾT THÚC SỬA ĐỔI =====

    // ===== SỬA ĐỔI: Cải tiến phương thức OnZoomCameraSmoothly =====
    private bool OnZoomCameraSmoothly()
    {
        // THÊM: Kiểm tra enableTouchZoom
        if (!enableTouchZoom || ZoomCameraData == null || _mainCamera == null) return false;

        if (Input.touchCount >= 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            if (!isZooming)
            {
                previousDistance = currentDistance;
                isZooming = true;
                // THÊM: Đánh dấu đang zoom bằng touch
                isTouchZooming = true;
            }
            else if (isTouchZooming) // THÊM: Chỉ xử lý khi đang touch zoom
            {
                float deltaDistance = currentDistance - previousDistance;
                previousDistance = currentDistance;

                // SỬA ĐỔI: Cải tiến tính toán zoom với touchZoomSensitivity
                float screenScale = Mathf.Min(Screen.width, Screen.height);
                float normalizedDelta = deltaDistance / screenScale;

                // THÊM: Sử dụng touchZoomSensitivity để điều chỉnh độ nhạy
                float zoomFactor = normalizedDelta * ZoomCameraData.ZoomSpeed * touchZoomSensitivity;
                float newTargetFOV = targetFOV - (zoomFactor * 500f);

                targetFOV = Mathf.Clamp(newTargetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);

                // THÊM: Cập nhật slider để đồng bộ
                UpdateSliderFromFOV(targetFOV);
            }

            return true;
        }

        isZooming = false;
        // THÊM: Reset trạng thái touch zoom
        isTouchZooming = false;
        return false;
    }
    // ===== KẾT THÚC SỬA ĐỔI =====

    // ===== SỬA ĐỔI: Cải tiến phương thức ZoomCamera để cập nhật slider =====
    public void ZoomCamera(float fovCam)
    {
        fovCam = Mathf.Clamp(fovCam, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);

        float scaleRatio = Mathf.Tan(fovCam * 0.5f * Mathf.Deg2Rad) /
                           Mathf.Tan(ZoomCameraData.MaxFOV * 0.5f * Mathf.Deg2Rad);
        BackGround.localScale = LocalScaleBackGroundDefault * scaleRatio;
        BackGround.transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 25;
        BackGround.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);

        if (ZoomStyle == ZoomCameraStyle.Smoothly)
            targetFOV = fovCam;

        _mainCamera.fieldOfView = fovCam;

        // THÊM: Cập nhật slider để phản ánh FOV mới (chỉ khi không phải từ touch zoom)
        // Tránh cập nhật slider khi đang zoom bằng touch để không gây lag
        if (!isTouchZooming)
        {
            UpdateSliderFromFOV(fovCam);
        }
    }
    // ===== KẾT THÚC SỬA ĐỔI =====

    public void ZoomCameraAdditional(float additionalFOV)
    {
        if (!ZoomCameraData || !_mainCamera) return;
        var currentTargetFOV = targetFOV;
        targetFOV = _mainCamera.fieldOfView + additionalFOV;
        targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
        return;

        bool outOfBound = (targetFOV < ZoomCameraData.MinFOV || targetFOV > ZoomCameraData.MaxFOV) &&
                          (currentTargetFOV == ZoomCameraData.MaxFOV || currentTargetFOV == ZoomCameraData.MinFOV);
        if (outOfBound)
        {
            BlockZoom = true;

            Sequence cameraPullBack = DOTween.Sequence();
            var outOfBoundFOV = targetFOV < ZoomCameraData.MinFOV
                ? ZoomCameraData.MinFOV - 1
                : ZoomCameraData.MaxFOV + 1;
            var inBoundFOV = targetFOV < ZoomCameraData.MinFOV
                ? ZoomCameraData.MinFOV
                : ZoomCameraData.MaxFOV;

            cameraPullBack.Append(_mainCamera.DOFieldOfView(outOfBoundFOV, 0.15f));
            cameraPullBack.Append(_mainCamera.DOFieldOfView(inBoundFOV, 0.25f));
            cameraPullBack.SetEase(Ease.InOutCubic);
            cameraPullBack.OnComplete(() =>
                {
                    targetFOV = inBoundFOV;
                    BlockZoom = false;
                }
            );
        }
        else targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
    }

    public void ResetCamearState()
    {
        if (!_mainCamera || !ZoomCameraData || !BackGround) return;
        _mainCamera.fieldOfView = ZoomCameraData.DefaultFOV;
        BackGround.localScale = LocalScaleBackGroundDefault;
    }

    #endregion

    #region _intro handle

    public void StartIntro()
    {
        _isActive = false;
        if (introCoroutine != null) StopCoroutine(introCoroutine);
        introCoroutine = StartCoroutine(IntroExecuteAsync());
    }

    private IEnumerator IntroExecuteAsync()
    {
        if (!_mainCamera || !modelTransfrom) yield return null;
        float introTimer = 0f;
        bool introEnded = false;

        targetFOV = IntroEndFOV;
        _mainCamera.fieldOfView = IntroStartFOV;

        while (!introEnded)
        {
            introTimer += Time.deltaTime;
            if (introTimer >= IntroLenght)
            {
                Vector3 modelLE = modelTransfrom.localEulerAngles;
                if (IsVectorInRangeUpward(modelLE, Vector3.zero, 15f) || introTimer > IntroLenght + 0.5f)
                {
                    introEnded = true;
                    GamePlaySystem.Instance.ActiveHandController(true);
                }
            }

            if (modelTransfrom != null)
                modelTransfrom.Rotate(0f, ModelRotationIntroSpeed * Time.deltaTime, 0f, Space.World);

            yield return null;
        }

        _isActive = true;
        _mainCamera
            .DOFieldOfView(IntroEndFOV, IntroCameraZoomInDuration)
            .SetEase(Ease.InOutSine);
        yield return Yielders.Get(IntroCameraZoomInDuration);
        _isActive = true;
    }

    /// <summary>
    /// Should've been an extension =.=
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    private bool IsVectorInRangeUpward(Vector3 original, Vector3 target, float offset)
    {
        return original.y > target.y - offset && original.y < target.y + offset;
    }

    #endregion

    #endregion

    // ===== BỔ SUNG: Override OnDestroy để dọn dẹp =====
    /// <summary>
    /// Dọn dẹp khi object bị destroy để tránh memory leak
    /// </summary>
    void OnDestroy()
    {
        // Dọn dẹp listener khi object bị destroy
        if (ZoomSlider != null)
            ZoomSlider.onValueChanged.RemoveAllListeners();

        // Dọn dẹp button listener
        if (btnReCenterModel != null)
            btnReCenterModel.onClick.RemoveAllListeners();
    }
    // ===== KẾT THÚC BỔ SUNG =====
}

public enum DraggingStyle
{
    Instantly,
    Smoothly,
    SmoothlyYAxis
}

public enum ZoomCameraStyle
{
    Instantly,
    Smoothly
}