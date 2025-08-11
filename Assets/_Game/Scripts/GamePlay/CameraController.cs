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

    public ZoomCameraData ZoomCameraData;
    public Button         ButtonCenter;
    public Slider         SliderZoom;
    public Transform      BackGround;

    public  Transform  SpawnPoint;
    public  GameObject ModelPrefab;
    private Transform  modelTransfrom;
    public  Quaternion targetRotation;

    public float      Friction            = 3f;             // The speed of decay of inertia
    public Vector2    RotationSensitivity = new (1f,   1f); // Giới hạn tốc độ xoay
    public Vector2    AccelerationRange   = new (0.1f, 1f); // Giới hạn tốc độ xoay
    public float      RotationSpeed       = 5f;             // Tốc độ xoay
    public float      RotationAutoSpeed   = 0.5f;           // Tốc độ xoay tự động
    public float      SmoothingTime       = 0.05f;
    [Header("Rotation")]
    
    public float TimeAFKToAutoRotation = 10f;
// Thêm các thuộc tính này vào khu vực PROPERTIES ở đầu class

    private RaycastHit[] _woolHits = new RaycastHit[10]; 

    [Header("Tapping Settings")]
    [Tooltip("Bán kính của vùng tìm kiếm lân cận khi người chơi tap trượt.")]
    [SerializeField] private float _tapRadius = 0.2f;


    private float   _acceleration;
    private float   _timerAfterMouseUp   = 0f;
    private float   _timerAfterMouseDown = 0f;
    private Vector2 _previousDelta       = Vector2.zero;
    private Vector3 _lastMousePosition;

    private Vector2 _sceenRate;

    private Camera     _mainCamera;
    private Camera     _fakeUICamera;
    private GameObject _targetObject;


    private bool  _isIdling = false;
    private float _timeIdle = 0f;

    private bool _isActive = false;

    private bool  _isClickOnMesh;
    private float _timerHoldClick;

    private bool _isClicking;
    private bool _isDragging;
    private bool _isHolding;
    [SerializeField] private bool _isRotateObjectInMainMenu;

    private WoolControl _targetWool;

    [SerializeField] private Vector3 _cameraPosMainMenuDefault  = new Vector3(0,   1.8f, -11);
    [SerializeField] private Vector3 _cameraRoteMainMenuDefault = new Vector3(15f, 0,    0);
    [SerializeField] private Vector3 _cameraPosGamePlayDefault  = new Vector3(0,   1,    -10);
    [SerializeField] private Vector3 _cameraRoteGamePlayDefault = new Vector3(6,   0,    0);

    [Header("INTRO SETTING(s)")] public float     IntroLenght               = 2f;
    public                              float     ModelRotationIntroSpeed   = 0.5f;
    public                              float     IntroCameraZoomInDuration = 0.5f;
    public                              int       IntroStartFOV             = 65;
    public                              int       IntroEndFOV               = 65;
    private                             Coroutine introCoroutine;

    [Header("DRAGGING SETTING(s)")] public DraggingStyle DragStyle;
    public                                 float         DraggingSpeed = 0.25f;
    public                                 float         SmoothFactor  = 7;

    [Header("DRAGGING SETTING(s)")] public ZoomCameraStyle ZoomStyle;
    private                                bool            isZooming = false;
    private                                float           targetFOV;
    private                                float           currentFOV;
    private                                float           previousDistance;
    private                                float           zoomLerpSpeed = 7f;

    // Đống này của thằng Đồng nó vất linh tinh đm đéo biết để đâu
    private bool         BlockRotation;
    private bool         BlockZoom;
    public  Action<bool> OnHandleMouseAction;
    private bool         BlockHandTap;
    private bool         _blockHold;
    public  Action       OnHandleHoldWoolAction;
    private bool         _blockDrag;
    public  Action       OnHandleDragWoolAction;
    private Vector3      LocalScaleBackGroundDefault = new (40f, 40f, 1);

    #endregion

    #region UNITY_METHODS

    public override void Awake()
    {
        base.Awake();
        _mainCamera   = CameraContainer.Instance.MainCamera;
        _fakeUICamera = CameraContainer.Instance.FakeUICamera;
        if (ZoomCameraData != null && SliderZoom != null && _mainCamera != null)
        {
            // Lấy giá trị FOV mặc định
            float defaultFOV = ZoomCameraData.DefaultFOV;
            
            // Đặt trực tiếp giá trị cho FOV của camera và FOV mục tiêu
            _mainCamera.fieldOfView = defaultFOV;
            targetFOV = defaultFOV;

            // Tính toán giá trị tương ứng cho slider (từ 0 đến 1)
            float sliderValue = Mathf.InverseLerp(ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV, defaultFOV);

            // "Lặng lẽ" đặt giá trị cho slider mà không kích hoạt sự kiện
            SliderZoom.SetValueWithoutNotify(sliderValue);
        }
    }

    private void OnEnable()
    {
        _timerAfterMouseUp = 0;
        _acceleration      = AccelerationRange.x;
        _timeIdle          = 0f;
        ResetCamearState();
    }

    private void Start()
    {
        InputInteractable.OnTap  += HandleTap;
        InputInteractable.OnHold += HandleHold;
        //InputInteractable.OnDragAction += HandleDrag;
        InputInteractable.OnDragAction += HandleDragSmoothly;
        InputInteractable.OnMouseDown  += HandleMouse;
        ButtonCenter.onClick.AddListener(HandleClickButtonCenter);
        SliderZoom.onValueChanged.AddListener(HandleSliderZoom);
        
    }
    /// <summary>
    /// Cập nhật tỷ lệ và vị trí của background dựa trên Field of View của camera.
    /// </summary>
    /// <param name="currentFov">Giá trị FOV hiện tại của camera.</param>
    private void UpdateBackgroundScale(float fov)
    {
        // Clamp giá trị fov để đảm bảo an toàn trong tính toán
        fov = Mathf.Clamp(fov, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);

        // Tính toán tỷ lệ co giãn dựa trên sự thay đổi của góc nhìn (FOV)
        float scaleRatio = Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) /
                           Mathf.Tan(ZoomCameraData.MaxFOV * 0.5f * Mathf.Deg2Rad);
                       
        BackGround.localScale = LocalScaleBackGroundDefault * scaleRatio;
    
        // Đảm bảo background luôn ở phía sau và hướng vào camera
        BackGround.transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 25;
        BackGround.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
    }
    /// <summary>
    /// Xử lý sự kiện khi giá trị của Slider thay đổi.
    /// Ánh xạ giá trị của slider (0-1) vào khoảng FOV (Min-Max).
    /// </summary>
    /// <param name="sliderValue">Giá trị từ slider, trong khoảng 0 đến 1.</param>
    public void HandleSliderZoom(float sliderValue)
    {
        if (!ZoomCameraData || !_mainCamera) return;

        // Sử dụng Lerp để ánh xạ tuyến tính giá trị slider [0, 1] tới khoảng [MinFOV, MaxFOV]
        // Đây là logic cốt lõi để slider hoạt động đúng
        targetFOV = Mathf.Lerp(ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV, sliderValue);

        // Không cần kẹp (Clamp) ở đây nữa vì Lerp với sliderValue từ 0-1 đã đảm bảo giá trị nằm trong khoảng min-max.
    }
    private void HandleClickButtonCenter()
    {
        ReCenterModel();
    }

    private void Update()
    {
        // --- THÊM ĐOẠN CODE TEST NÀY VÀO ĐẦU HÀM UPDATE ---
#if UNITY_EDITOR
        // Giả lập Pinch-to-Zoom bằng phím Ctrl Trái + Lăn chuột
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // Input.GetAxis("Mouse ScrollWheel") sẽ trả về giá trị > 0 khi lăn lên, < 0 khi lăn xuống
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            // Chỉ thực hiện khi có hành động lăn chuột
            if (Mathf.Abs(scrollInput) > 0.01f)
            {
                // Lăn lên (zoom in) -> scrollInput > 0 -> giảm targetFOV
                // Lăn xuống (zoom out) -> scrollInput < 0 -> tăng targetFOV
                targetFOV -= scrollInput * 20f; // Số 20f là độ nhạy, bạn có thể thay đổi
                targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
            }
        }
#endif
        // --- KẾT THÚC ĐOẠN CODE TEST ---
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
                modelTransfrom.rotation = Quaternion.Slerp(modelTransfrom.rotation, targetRotation, Time.deltaTime * SmoothFactor);
            }
        }

        if (!BlockZoom)
        {
            // Xử lý zoom bằng hai ngón tay
            OnZoomCameraSmoothly();
            
            // Kiểm tra nếu FOV hiện tại và FOV mục tiêu có chênh lệch đáng kể
            if (Mathf.Abs(_mainCamera.fieldOfView - targetFOV) > 0.01f)
            {
                // Lerp mượt mà tới FOV mục tiêu
                float newFov = Mathf.Lerp(_mainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomLerpSpeed);
                _mainCamera.fieldOfView = newFov;

                // Cập nhật lại giá trị cho slider để đồng bộ (ví dụ: khi zoom bằng tay)
                float sliderValue = Mathf.InverseLerp(ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV, newFov);
                SliderZoom.SetValueWithoutNotify(sliderValue);

                // Luôn cập nhật background khi FOV thay đổi
                UpdateBackgroundScale(newFov);
            }
        }

        if (_isClicking) return;
    
        if (_timeIdle <= 0f)
        {
            // Logic xoay tự động khi AFK
        }
        else 
        {
            _timeIdle -= Time.deltaTime;
        }
    }

    #endregion

    #region MAIN_METHODS

    public void SetActive(bool isActive) { _isActive = isActive; }

    public void Setup(GameObject levelObjectPrefab)
    {
        _isActive      = true;
        BlockZoom      = false;
        ModelPrefab    = levelObjectPrefab;
        modelTransfrom = ModelPrefab.transform;
        targetRotation = modelTransfrom.rotation;
    }

    #region _input

    public void SetBlockHandTap(bool isBlock)
    {
        BlockHandTap = isBlock;
        Debug.Log("Block hand tap: " + isBlock);
    }

    public void BlockRotate(bool isBlock)
    {
        BlockRotation  = isBlock;
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
        }
    }

    /// <summary>
    /// Tạo một phương thức phụ để xử lý WoolControl, tránh lặp code.
    /// </summary>
    private void HandleFoundWool(WoolControl wool)
    {
        // Đây là logic gốc từ HandleTap của bạn
        wool.WoolRotation();
        //GamePlaySystem.Instance.RaiseMotion(EMotionType.Shy, Random.value);
        //Debug.Log("Tapped on Wool: " + wool.name);
    }

    private void HandleMouse(bool isPointerDown)
    {
        _isClicking = isPointerDown;
        _timeIdle   = TimeAFKToAutoRotation;
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
        float screenWidth  = Screen.width;
        float screenHeight = Screen.height;

        // Luna thường sử dụng tỷ lệ canvas khác với kích thước thực tế
        // Các giá trị offset này cần được điều chỉnh dựa trên thử nghiệm
        float offsetX = 0f; // Thử các giá trị khác nhau
        float offsetY = 0f; // Thử các giá trị khác nhau

        // Điều chỉnh tọa độ
        return new Vector2(originalPos.x * screenWidth / 886, originalPos.y * screenHeight / 1920) + new Vector2(offsetX, offsetY);
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
        ButtonCenter.interactable = false;
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
                        ButtonCenter.interactable = true;
                    }
                );
    }

    #endregion

    #region _zoom camera

    private bool OnZoomCamera()
    {
        if (!ZoomCameraData || !_mainCamera) return false;

        if (Input.touchCount >= 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                previousDistance = currentDistance;
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
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
    if (ZoomCameraData == null || _mainCamera == null) return false;

    // Chỉ thực hiện khi có đúng 2 ngón tay chạm màn hình
    if (Input.touchCount == 2)
    {
        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        // Tính khoảng cách hiện tại giữa 2 ngón tay
        float currentDistance = Vector2.Distance(touch0.position, touch1.position);

        // Kiểm tra xem có ngón tay nào vừa bắt đầu chạm không
        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        {
            // Frame đầu tiên của hành động zoom - khởi tạo
            previousDistance = currentDistance;
            isZooming = true;
            Debug.Log("Zoom started. Initial distance: " + currentDistance);
            return true;
        }

        // Chỉ xử lý zoom khi cả 2 ngón tay đang di chuyển
        if ((touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved) && isZooming)
        {
            // Tính toán sự thay đổi khoảng cách
            float deltaDistance = currentDistance - previousDistance;
            
            // Chỉ xử lý nếu có sự thay đổi đáng kể để tránh rung lắc
            if (Mathf.Abs(deltaDistance) > 1f) // Threshold để tránh nhiễu
            {
                // Cập nhật targetFOV
                float oldTargetFOV = targetFOV;
                // Khi deltaDistance > 0 (2 ngón xa nhau hơn) -> zoom out (FOV tăng)
                // Khi deltaDistance < 0 (2 ngón gần nhau hơn) -> zoom in (FOV giảm)
                targetFOV += deltaDistance * ZoomCameraData.ZoomSpeed;
                
                // Clamp giá trị trong giới hạn cho phép
                targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
                
                // Cập nhật slider để đồng bộ (không trigger event)
                if (SliderZoom != null)
                {
                    float sliderValue = Mathf.InverseLerp(ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV, targetFOV);
                    SliderZoom.SetValueWithoutNotify(sliderValue);
                }

                // Debug log (có thể tắt trong production)
                #if UNITY_EDITOR
                Debug.Log($"Zoom: Distance={currentDistance:F1}, Delta={deltaDistance:F1}, FOV={oldTargetFOV:F1}->{targetFOV:F1}");
                #endif
            }
            
            // Cập nhật previousDistance cho frame tiếp theo
            previousDistance = currentDistance;
        }

        return true; // Đang trong quá trình zoom
    }
    else
    {
        // Không có đủ 2 ngón tay - kết thúc zoom
        if (isZooming)
        {
            isZooming = false;
            Debug.Log("Zoom ended");
        }
        return false;
    }
}

    public void ZoomCamera(float fovCam)
    {
        fovCam =  Mathf.Clamp(fovCam, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);

        float scaleRatio = Mathf.Tan(fovCam * 0.5f * Mathf.Deg2Rad) /
            Mathf.Tan(ZoomCameraData.MaxFOV * 0.5f * Mathf.Deg2Rad);
        BackGround.localScale         = LocalScaleBackGroundDefault * scaleRatio;
        BackGround.transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 25;
        BackGround.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
        if (ZoomStyle == ZoomCameraStyle.Smoothly) targetFOV = fovCam;
        _mainCamera.fieldOfView = fovCam;
    }

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
            cameraPullBack.Append(_mainCamera.DOFieldOfView(inBoundFOV,    0.25f));
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

        // Cập nhật lại hàm Reset để nó đồng bộ mọi thứ, bao gồm cả slider
        float defaultFOV = ZoomCameraData.DefaultFOV;
        _mainCamera.fieldOfView = defaultFOV;
        targetFOV = defaultFOV; // Rất quan trọng: phải reset cả targetFOV

        if (SliderZoom != null)
        {
            float sliderValue = Mathf.InverseLerp(ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV, defaultFOV);
            SliderZoom.SetValueWithoutNotify(sliderValue);
        }

        // Cập nhật lại background
        UpdateBackgroundScale(defaultFOV);
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
        bool  introEnded = false;

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
    private bool IsVectorInRangeUpward(Vector3 original, Vector3 target, float offset) { return original.y > target.y - offset && original.y < target.y + offset; }

    #endregion

    #endregion
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
