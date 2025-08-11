using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Quản lý toàn bộ hoạt động của camera trong màn chơi.
/// Bao gồm các chức năng: xoay đối tượng 3D, kéo-thả, zoom bằng Slider và cảm ứng đa điểm (pinch-to-zoom),
/// xử lý tương tác chạm (tap/hold) và reset trạng thái.
/// </summary>
public class CameraController : Singleton<CameraController>
{
    #region PROPERTIES

    [Tooltip("Component xử lý các sự kiện input thô từ người dùng (tap, hold, drag).")]
    public Interactable InputInteractable;

    [Tooltip("Nút bấm để xoay đối tượng về vị trí ban đầu.")]
    public Button btnReCenterModel;
    
    [Tooltip("Cờ kiểm soát, ngăn người dùng bấm nút Reset liên tục trong khi animation đang chạy.")]
    [SerializeField] private bool canResset;

    [Tooltip("Thanh trượt UI để điều khiển mức độ zoom của camera.")]
    public Slider ZoomSlider;
    
    [Tooltip("Giới hạn Field of View (FOV) khi zoom ra xa nhất. Giá trị càng lớn càng xa.")]
    public float maxZoom;
    
    [Tooltip("Giới hạn Field of View (FOV) khi zoom vào gần nhất. Giá trị càng nhỏ càng gần.")]
    public float minZoom;
    
    [Tooltip("ScriptableObject chứa các dữ liệu cấu hình mặc định cho camera (FOV, giới hạn...).")]
    public ZoomCameraData ZoomCameraData;
    
    [Tooltip("Đối tượng background để tạo hiệu ứng parallax khi zoom.")]
    public Transform BackGround;

    [Tooltip("Điểm (Transform) mà đối tượng 3D sẽ được tạo ra và xoay quanh đó.")]
    public Transform SpawnPoint;
    
    [Tooltip("Prefab của đối tượng 3D sẽ được hiển thị và tương tác.")]
    public GameObject ModelPrefab;
    
    private Transform modelTransfrom;
    
    [Tooltip("Góc xoay mục tiêu mà đối tượng sẽ hướng tới khi ở chế độ xoay mượt.")]
    public Quaternion targetRotation;

    [Tooltip("Tốc độ suy giảm của quán tính xoay (không dùng trong logic hiện tại).")]
    public float Friction = 3f; // The speed of decay of inertia
    
    [Tooltip("Độ nhạy khi xoay.")]
    public Vector2 RotationSensitivity = new(1f, 1f);
    
    [Tooltip("Khoảng tăng tốc khi xoay (không dùng trong logic hiện tại).")]
    public Vector2 AccelerationRange = new(0.1f, 1f);
    
    [Tooltip("Tốc độ xoay đối tượng khi người dùng kéo (chế độ Instantly).")]
    public float RotationSpeed = 5f;
    
    [Tooltip("Tốc độ xoay tự động của đối tượng khi người dùng không tương tác.")]
    public float RotationAutoSpeed = 0.5f;
    
    [Tooltip("Thời gian làm mượt khi xoay (không dùng trong logic hiện tại).")]
    public float SmoothingTime = 0.05f;
    
    [Header("Rotation")] 
    [Tooltip("Thời gian (giây) không tương tác trước khi đối tượng bắt đầu tự xoay.")]
    public float TimeAFKToAutoRotation = 10f;
    
    private RaycastHit[] _woolHits = new RaycastHit[10];

    [Header("Tapping Settings")]
    [Tooltip("Bán kính của vùng SphereCast để tìm đối tượng lân cận khi người dùng chạm trượt.")]
    [SerializeField]
    private float _tapRadius = 0.2f;

    // ===== BỔ SUNG: CÁC THUỘC TÍNH CHO ZOOM 2 NGÓN TAY =====
    [Header("TOUCH ZOOM SETTINGS")]
    [SerializeField]
    [Tooltip("Độ nhạy khi zoom bằng 2 ngón tay (1.0f = bình thường, 2.0f = nhạy gấp đôi)")]
    private float touchZoomSensitivity = 2f;

    [SerializeField] [Tooltip("Bật/tắt tính năng zoom bằng 2 ngón tay")]
    private bool enableTouchZoom = true;

    [SerializeField] [Tooltip("Thời gian làm mượt khi zoom bằng cảm ứng (không dùng trong logic hiện tại)")]
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
    
    [Tooltip("Có cho phép đối tượng tự xoay ở màn hình chính (Main Menu) không.")]
    [SerializeField] private bool _isRotateObjectInMainMenu;

    private WoolControl _targetWool;

    [Tooltip("Vị trí camera mặc định ở màn hình chính.")]
    [SerializeField] private Vector3 _cameraPosMainMenuDefault = new Vector3(0, 1.8f, -11);
    
    [Tooltip("Góc xoay camera mặc định ở màn hình chính.")]
    [SerializeField] private Vector3 _cameraRoteMainMenuDefault = new Vector3(15f, 0, 0);
    
    [Tooltip("Vị trí camera mặc định trong màn chơi.")]
    [SerializeField] private Vector3 _cameraPosGamePlayDefault = new Vector3(0, 1, -10);
    
    [Tooltip("Góc xoay camera mặc định trong màn chơi.")]
    [SerializeField] private Vector3 _cameraRoteGamePlayDefault = new Vector3(6, 0, 0);

    [Header("INTRO SETTING(s)")] 
    [Tooltip("Độ dài của màn Intro (giây).")]
    public float IntroLenght = 2f;
    
    [Tooltip("Tốc độ xoay của đối tượng trong màn Intro.")]
    public float ModelRotationIntroSpeed = 0.5f;
    
    [Tooltip("Thời gian (giây) để camera zoom vào ở cuối màn Intro.")]
    public float IntroCameraZoomInDuration = 0.5f;
    
    [Tooltip("FOV của camera khi bắt đầu Intro.")]
    public int IntroStartFOV = 65;
    
    [Tooltip("FOV của camera khi kết thúc Intro.")]
    public int IntroEndFOV = 65;
    
    private Coroutine introCoroutine;

    [Header("DRAGGING SETTING(s)")] 
    [Tooltip("Kiểu kéo/xoay đối tượng (ngay lập tức hoặc mượt mà).")]
    public DraggingStyle DragStyle;
    
    [Tooltip("Tốc độ xoay đối tượng khi người dùng kéo (chế độ Smoothly).")]
    public float DraggingSpeed = 0.25f;
    
    [Tooltip("Hệ số làm mượt khi xoay. Giá trị càng lớn, đối tượng xoay theo càng nhanh.")]
    public float SmoothFactor = 7;

    [Header("DRAGGING SETTING(s)")] // Có vẻ bị trùng Header
    [Tooltip("Kiểu zoom của camera (ngay lập tức hoặc mượt mà).")]
    public ZoomCameraStyle ZoomStyle;
    
    private bool isZooming = false;
    private float targetFOV;
    private float currentFOV;
    private float previousDistance;
    private float zoomLerpSpeed = 7f;

    // Đống này của thằng Đồng nó vất linh tinh đm đéo biết để đâu
    private bool BlockRotation;
    private bool BlockZoom;
    public Action<bool> OnHandleMouseAction;
    private bool BlockHandTap;
    private bool _blockHold;
    public Action OnHandleHoldWoolAction;
    private bool _blockDrag;
    public Action OnHandleDragWoolAction;
    private Vector3 LocalScaleBackGroundDefault = new(40f, 40f, 1);

    // Lưu rotation ban đầu để reset
    private Quaternion initialRotation;
    private bool hasStoredInitialRotation = false;

    [Header("Reset Settings")] 
    [Tooltip("Thời gian (giây) để hoàn thành animation xoay về vị trí ban đầu.")]
    public float ResetRotationDuration = 1f;
    
    [Tooltip("Kiểu chuyển động (Ease) của animation reset.")]
    public Ease ResetRotationEase = Ease.InOutCubic;

    #endregion

    #region UNITY_METHODS

    /// <summary>
    /// Được gọi đầu tiên khi script được khởi tạo. Dùng để lấy các tham chiếu cốt lõi.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        _mainCamera = CameraContainer.Instance.MainCamera;
        _fakeUICamera = CameraContainer.Instance.FakeUICamera;
        targetFOV = ZoomCameraData.DefaultFOV;
    }

    /// <summary>
    /// Được gọi mỗi khi đối tượng được kích hoạt. Dùng để reset các trạng thái.
    /// </summary>
    private void OnEnable()
    {
        _timerAfterMouseUp = 0;
        _acceleration = AccelerationRange.x;
        _timeIdle = 0f;
        ResetCamearState();
    }

    /// <summary>
    /// Được gọi một lần sau Awake. Dùng để đăng ký các sự kiện.
    /// </summary>
    private void Start()
    {
        InputInteractable.OnTap += HandleTap;
        InputInteractable.OnHold += HandleHold;
        //InputInteractable.OnDragAction += HandleDrag;
        InputInteractable.OnDragAction += HandleDragSmoothly;
        InputInteractable.OnMouseDown += HandleMouse;
    }

    /// <summary>
    /// Được gọi mỗi frame. Là nơi xử lý các logic chính như xoay và zoom.
    /// </summary>
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

    /// <summary>
    /// Kích hoạt hoặc vô hiệu hóa các chức năng chính của CameraController.
    /// </summary>
    /// <param name="isActive">Trạng thái mới (true = hoạt động, false = không hoạt động).</param>
    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    /// <summary>
    /// Thiết lập CameraController với một đối tượng 3D mới và khởi tạo các UI liên quan.
    /// </summary>
    /// <param name="levelObjectPrefab">Prefab của đối tượng 3D cần hiển thị.</param>
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
    /// <summary>
    /// Phương thức được gọi khi người dùng kéo thanh Slider UI.
    /// </summary>
    /// <param name="value">Giá trị mới của slider (từ 0 đến 1).</param>
    public void OnZoomSliderChanged(float value)
    {
        // THÊM: Kiểm tra canResset để có thể vô hiệu hóa slider
        if(!canResset) return;

        // Ánh xạ giá trị của slider (0-1) thành giá trị FOV tương ứng
        float newFOV = Mathf.Lerp(minZoom, maxZoom, value);

        // Cập nhật targetFOV để đồng bộ với các cơ chế zoom khác
        targetFOV = newFOV;

        if (ZoomStyle == ZoomCameraStyle.Smoothly)
        {
            // Ở chế độ smooth, chỉ cần đặt target. Hàm Update() sẽ lo phần làm mượt.
        }
        else
        {
            // Ở chế độ Instantly, áp dụng FOV ngay lập tức
            _mainCamera.fieldOfView = newFOV;
        }

        ZoomCamera(newFOV); // Gọi hàm này để cập nhật các thành phần phụ thuộc như background
    }
    // ===== KẾT THÚC SỬA ĐỔI =====

    // ===== BỔ SUNG: Phương thức mới để cập nhật slider từ FOV =====
    /// <summary>
    /// Cập nhật vị trí của thanh Slider UI dựa trên FOV hiện tại của camera.
    /// Dùng để đồng bộ UI khi người dùng zoom bằng 2 ngón tay.
    /// </summary>
    /// <param name="currentFOV">Field of View hiện tại của camera.</param>
    private void UpdateSliderFromFOV(float currentFOV)
    {
        if (ZoomSlider != null && !isSliderUpdating)
        {
            isSliderUpdating = true; // Cắm cờ để tránh vòng lặp

            // Chuyển đổi FOV (vd: 30-90) thành giá trị slider (0-1)
            float normalizedValue = (currentFOV - minZoom) / (maxZoom - minZoom);
            normalizedValue = Mathf.Clamp01(normalizedValue);

            // Gán giá trị cho slider một cách mượt mà
            ZoomSlider.value = Mathf.Lerp(ZoomSlider.value, normalizedValue, Time.deltaTime * 10f);

            isSliderUpdating = false; // Nhổ cờ
        }
    }
    // ===== KẾT THÚC BỔ SUNG =====
    
    #endregion

    #region RESET_ROTATION_METHODS

    /// <summary>
    /// Lưu lại góc xoay ban đầu của đối tượng.
    /// </summary>
    public void StoreInitialRotation()
    {
        if (modelTransfrom != null)
        {
            initialRotation = modelTransfrom.rotation;
            hasStoredInitialRotation = true;
            Debug.Log("Initial rotation stored: " + initialRotation.eulerAngles);
        }
    }

    /// <summary>
    /// Xoay đối tượng trở về góc ban đầu một cách mượt mà bằng DOTween.
    /// </summary>
    public void ResetToInitialRotation()
    {
        if (!hasStoredInitialRotation || modelTransfrom == null || !canResset)
        {
            Debug.LogWarning("Chưa lưu góc xoay ban đầu hoặc không tìm thấy model!");
            return;
        }

        canResset = false; // Ngăn người dùng bấm liên tục
        BlockRotation = true;

        // Sử dụng DOTween để tạo animation
        modelTransfrom.DORotate(initialRotation.eulerAngles, ResetRotationDuration)
            .SetEase(ResetRotationEase)
            .OnComplete(() =>
            {
                BlockRotation = false; // Mở khóa xoay
                canResset = true; // Cho phép bấm nút lại
                targetRotation = initialRotation;
                Debug.Log("Model đã được reset về góc ban đầu.");
            });
    }

    #endregion

    // ===== BỔ SUNG: Các phương thức điều khiển tính năng zoom =====
    /// <summary>
    /// Bật hoặc tắt tính năng zoom bằng hai ngón tay.
    /// </summary>
    /// <param name="enabled">True để bật, False để tắt.</param>
    public void SetTouchZoomEnabled(bool enabled)
    {
        enableTouchZoom = enabled;
        Debug.Log("Touch zoom enabled: " + enabled);
    }

    /// <summary>
    /// Thay đổi độ nhạy của thao tác zoom bằng hai ngón tay.
    /// </summary>
    /// <param name="sensitivity">Giá trị độ nhạy mới.</param>
    public void SetTouchZoomSensitivity(float sensitivity)
    {
        touchZoomSensitivity = Mathf.Clamp(sensitivity, 0.1f, 10f);
        Debug.Log("Touch zoom sensitivity set to: " + touchZoomSensitivity);
    }
    // ===== KẾT THÚC BỔ SUNG =====

    /// <summary>
    /// Bật hoặc tắt chức năng nhận diện Tap.
    /// </summary>
    /// <param name="isBlock">True để chặn, False để cho phép.</param>
    public void SetBlockHandTap(bool isBlock)
    {
        BlockHandTap = isBlock;
        Debug.Log("Block hand tap: " + isBlock);
    }

    /// <summary>
    /// Khóa hoặc mở khóa chức năng xoay đối tượng.
    /// </summary>
    /// <param name="isBlock">True để chặn, False để cho phép.</param>
    public void BlockRotate(bool isBlock)
    {
        BlockRotation = isBlock;
        targetRotation = modelTransfrom.rotation;
    }

    /// <summary>
    /// Bật hoặc tắt chức năng nhận diện Hold.
    /// </summary>
    /// <param name="isBlock">True để chặn, False để cho phép.</param>
    public void SetBlockHold(bool isBlock)
    {
        _blockHold = isBlock;
        Debug.Log("Block hold: " + isBlock);
    }

    /// <summary>
    /// Bật hoặc tắt chức năng nhận diện Drag.
    /// </summary>
    /// <param name="isBlock">True để chặn, False để cho phép.</param>
    public void SetBlockDrag(bool isBlock)
    {
        _blockDrag = isBlock;
        Debug.Log("Block drag: " + isBlock);
    }

    /// <summary>
    /// Lớp LayerMask để xác định các đối tượng có thể tương tác.
    /// </summary>
    public LayerMask layerMask;

    /// <summary>
    /// Xử lý sự kiện khi người dùng Tap vào màn hình.
    /// </summary>
    /// <param name="pos">Tọa độ vị trí tap.</param>
    private void HandleTap(Vector2 pos)
    {
        if (BlockHandTap) return;

        Ray ray = _mainCamera.ScreenPointToRay(pos);

        WoolControl foundWool = FindBestWoolNearRay(ray);

        if (foundWool != null)
        {
            HandleFoundWool(foundWool);
        }
    }

    /// <summary>
    /// Hàm phụ trợ để xử lý khi một <c>WoolControl</c> được tìm thấy.
    /// </summary>
    /// <param name="wool">Đối tượng WoolControl đã được tap.</param>
    private void HandleFoundWool(WoolControl wool)
    {
        // Đây là logic gốc từ HandleTap của bạn
        wool.WoolRotation();
        //GamePlaySystem.Instance.RaiseMotion(EMotionType.Shy, Random.value);
        Debug.Log("Tapped on Wool: " + wool.name);
    }

    /// <summary>
    /// Xử lý sự kiện khi người dùng nhấn hoặc nhả chuột/ngón tay.
    /// </summary>
    /// <param name="isPointerDown">True nếu đang nhấn, False nếu đã nhả.</param>
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

    /// <summary>
    /// Biến tĩnh đếm số lần Hold.
    /// </summary>
    public static int HoldClickTime = 0;

    /// <summary>
    /// Tìm kiếm đối tượng <c>WoolControl</c> tốt nhất gần một tia Ray.
    /// Ưu tiên Raycast trực tiếp, sau đó đến SphereCast để tìm đối tượng lân cận.
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

    /// <summary>
    /// Xử lý sự kiện khi người dùng giữ tay trên màn hình.
    /// </summary>
    /// <param name="pos">Tọa độ vị trí giữ.</param>
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

    /// <summary>
    /// Hàm điều chỉnh tọa độ chuột/chạm cho phù hợp với môi trường Luna.
    /// </summary>
    private Vector2 AdjustLunaMousePosition(Vector2 originalPos)
    {
        // Điều chỉnh tọa độ dựa trên tỷ lệ khung hình của Luna
        float screenWidth  = Screen.width;
        float screenHeight = Screen.height;

        // Luna thường sử dụng tỷ lệ canvas khác với kích thước thực tế
        // Các giá trị offset này cần được điều chỉnh dựa trên thử nghiệm
        float offsetX = 0f; // Thử các giá trị khác nhau
        float offsetY = 0f; // Thử các giá trị khác nhau

        // Điều chỉnh tọa độ
        return new Vector2(originalPos.x * screenWidth / 886, originalPos.y * screenHeight / 1920) + new Vector2(offsetX, offsetY);
    }

    /// <summary>
    /// Xử lý sự kiện kéo với kiểu xoay <c>Instantly</c>.
    /// </summary>
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

    /// <summary>
    /// Xử lý sự kiện kéo với kiểu xoay <c>Smoothly</c>.
    /// </summary>
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

    /// <summary>
    /// Hàm cũ để reset vị trí trung tâm, có thể cần được cập nhật hoặc thay thế bằng <c>ResetToInitialRotation</c>.
    /// </summary>
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
                        BlockRotation  = false;
                        targetRotation = modelTransfrom.rotation;
                    }
                );
    }

    #endregion

    #region _zoom camera

    // ===== SỬA ĐỔI: Cải tiến phương thức OnZoomCamera cho zoom 2 ngón tay =====
    /// <summary>
    /// Hàm xử lý zoom bằng 2 ngón tay cho chế độ <c>Instantly</c>.
    /// </summary>
    /// <returns>Trả về true nếu có hành động zoom.</returns>
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
    /// <summary>
    /// Hàm xử lý zoom bằng 2 ngón tay cho chế độ <c>Smoothly</c>.
    /// </summary>
    /// <returns>Trả về true nếu có hành động zoom.</returns>
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

                // SỬA ĐỔI: Cải tiến tính toán zoom với touchZoomSensitivity (logic này hơi khác OnZoomCamera, cần chú ý)
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
    /// <summary>
    /// Áp dụng giá trị FOV cho camera và cập nhật các thành phần phụ thuộc như background và slider.
    /// </summary>
    /// <param name="fovCam">Giá trị Field of View mới để áp dụng.</param>
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

    /// <summary>
    /// Hàm cũ để zoom thêm một lượng, có thể không còn cần thiết.
    /// </summary>
    /// <param name="additionalFOV">Lượng FOV muốn cộng thêm.</param>
    public void ZoomCameraAdditional(float additionalFOV)
    {
        if (!ZoomCameraData || !_mainCamera) return;
        var currentTargetFOV = targetFOV;
        targetFOV = _mainCamera.fieldOfView + additionalFOV;
        targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
        return;

        // Đoạn code phía dưới không bao giờ được chạy do có 'return' ở trên.
        // bool outOfBound = ...
    }

    /// <summary>
    /// Reset trạng thái của camera về giá trị mặc định.
    /// </summary>
    public void ResetCamearState()
    {
        if (!_mainCamera || !ZoomCameraData || !BackGround) return;
        _mainCamera.fieldOfView = ZoomCameraData.DefaultFOV;
        BackGround.localScale = LocalScaleBackGroundDefault;
    }

    #endregion

    #region _intro handle

    /// <summary>
    /// Bắt đầu chuỗi animation Intro.
    /// </summary>
    public void StartIntro()
    {
        _isActive = false;
        if (introCoroutine != null) StopCoroutine(introCoroutine);
        introCoroutine = StartCoroutine(IntroExecuteAsync());
    }

    /// <summary>
    /// Coroutine thực thi logic của màn Intro.
    /// </summary>
    private IEnumerator IntroExecuteAsync()
    {
        if (!_mainCamera || !modelTransfrom) yield return null;
        float introTimer = 0f;
        bool introEnded = false;

        targetFOV               = IntroEndFOV;
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
                    //GamePlaySystem.Instance.ActiveHandController(true); // Dòng này có thể gây lỗi nếu GamePlaySystem không tồn tại
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
        //yield return Yielders.Get(IntroCameraZoomInDuration); // Dòng này có thể gây lỗi nếu không có class Yielders
        yield return new WaitForSeconds(IntroCameraZoomInDuration);
        _isActive = true;
    }

    /// <summary>
    /// Hàm tiện ích kiểm tra xem một góc có nằm trong một khoảng cho trước không.
    /// </summary>
    private bool IsVectorInRangeUpward(Vector3 original, Vector3 target, float offset) 
    { 
        return original.y > target.y - offset && original.y < target.y + offset; 
    }

    #endregion

    #endregion

    // ===== BỔ SUNG: Override OnDestroy để dọn dẹp =====
    /// <summary>
    /// Được gọi khi đối tượng bị hủy. Dùng để dọn dẹp các listener, tránh memory leak.
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

/// <summary>
/// Các kiểu kéo/xoay đối tượng.
/// </summary>
public enum DraggingStyle
{
    Instantly,
    Smoothly,
    SmoothlyYAxis
}

/// <summary>
/// Các kiểu zoom của camera.
/// </summary>
public enum ZoomCameraStyle
{

    Instantly,
    Smoothly
}