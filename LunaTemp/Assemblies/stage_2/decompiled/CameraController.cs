using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
	public Interactable InputInteractable;

	public ZoomCameraData ZoomCameraData;

	public Transform BackGround;

	public Transform SpawnPoint;

	public GameObject ModelPrefab;

	private Transform modelTransfrom;

	public Quaternion targetRotation;

	public float Friction = 3f;

	public Vector2 RotationSensitivity = new Vector2(1f, 1f);

	public Vector2 AccelerationRange = new Vector2(0.1f, 1f);

	public float RotationSpeed = 5f;

	public float RotationAutoSpeed = 0.5f;

	public float SmoothingTime = 0.05f;

	public float TimeAFKToAutoRotation = 10f;

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

	private bool _isRotateObjectInMainMenu;

	private WoolControl _targetWool;

	[SerializeField]
	private Vector3 _cameraPosMainMenuDefault = new Vector3(0f, 1.8f, -11f);

	[SerializeField]
	private Vector3 _cameraRoteMainMenuDefault = new Vector3(15f, 0f, 0f);

	[SerializeField]
	private Vector3 _cameraPosGamePlayDefault = new Vector3(0f, 1f, -10f);

	[SerializeField]
	private Vector3 _cameraRoteGamePlayDefault = new Vector3(6f, 0f, 0f);

	[Header("INTRO SETTING(s)")]
	public float IntroLenght = 2f;

	public float ModelRotationIntroSpeed = 0.5f;

	public float IntroCameraZoomInDuration = 0.5f;

	public int IntroStartFOV = 65;

	public int IntroEndFOV = 65;

	private Coroutine introCoroutine;

	[Header("DRAGGING SETTING(s)")]
	public DraggingStyle DragStyle;

	public float DraggingSpeed = 0.25f;

	public float SmoothFactor = 7f;

	[Header("DRAGGING SETTING(s)")]
	public ZoomCameraStyle ZoomStyle;

	private bool isZooming = false;

	private float targetFOV;

	private float currentFOV;

	private float previousDistance;

	private float zoomLerpSpeed = 7f;

	private bool BlockRotation;

	private bool BlockZoom;

	public Action<bool> OnHandleMouseAction;

	private bool BlockHandTap;

	private bool _blockHold;

	public Action OnHandleHoldWoolAction;

	private bool _blockDrag;

	public Action OnHandleDragWoolAction;

	private Vector3 LocalScaleBackGroundDefault = new Vector3(40f, 40f, 1f);

	public static int HoldClickTime;

	public override void Awake()
	{
		base.Awake();
		_mainCamera = CameraContainer.Instance.MainCamera;
		_fakeUICamera = CameraContainer.Instance.FakeUICamera;
		targetFOV = ZoomCameraData.DefaultFOV;
	}

	private void OnEnable()
	{
		_timerAfterMouseUp = 0f;
		_acceleration = AccelerationRange.x;
		_timeIdle = 0f;
		ResetCamearState();
	}

	private void Start()
	{
		Interactable inputInteractable = InputInteractable;
		inputInteractable.OnTap = (Action<Vector2>)Delegate.Combine(inputInteractable.OnTap, new Action<Vector2>(HandleTap));
		Interactable inputInteractable2 = InputInteractable;
		inputInteractable2.OnHold = (Action<Vector2>)Delegate.Combine(inputInteractable2.OnHold, new Action<Vector2>(HandleHold));
		Interactable inputInteractable3 = InputInteractable;
		inputInteractable3.OnDragAction = (Action<Vector2>)Delegate.Combine(inputInteractable3.OnDragAction, new Action<Vector2>(HandleDragSmoothly));
		Interactable inputInteractable4 = InputInteractable;
		inputInteractable4.OnMouseDown = (Action<bool>)Delegate.Combine(inputInteractable4.OnMouseDown, new Action<bool>(HandleMouse));
	}

	private void Update()
	{
		if (BlockRotation || !SpawnPoint || !modelTransfrom)
		{
			return;
		}
		if (_isRotateObjectInMainMenu)
		{
			SpawnPoint.Rotate(0f, RotationAutoSpeed * _acceleration * RotationSensitivity.x, 0f, Space.World);
		}
		else
		{
			if (!_isActive)
			{
				return;
			}
			if ((_isDragging || _timeIdle > 0f) && DragStyle == DraggingStyle.Smoothly)
			{
				modelTransfrom.rotation = Quaternion.Slerp(modelTransfrom.rotation, targetRotation, Time.deltaTime * SmoothFactor);
			}
			if (!BlockZoom)
			{
				if (OnZoomCameraSmoothly())
				{
					_mainCamera.fieldOfView = Mathf.Lerp(_mainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomLerpSpeed);
					ZoomCamera(_mainCamera.fieldOfView);
				}
				else
				{
					currentFOV = _mainCamera.fieldOfView;
					if (MathF.Abs(currentFOV - targetFOV) > 0.15f)
					{
						currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * zoomLerpSpeed);
						_mainCamera.fieldOfView = currentFOV;
					}
				}
			}
			if (!_isClicking)
			{
				if (_timeIdle <= 0f)
				{
					SpawnPoint.Rotate(0f, RotationAutoSpeed * _acceleration * RotationSensitivity.x, 0f, Space.World);
				}
				if (_timeIdle <= 0f)
				{
					SpawnPoint.Rotate(0f, RotationAutoSpeed * _acceleration * RotationSensitivity.x, 0f, Space.World);
				}
				else
				{
					_timeIdle -= Time.deltaTime;
				}
			}
		}
	}

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
	}

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

	private void HandleTap(Vector2 pos)
	{
		if (!BlockHandTap)
		{
			Ray ray = _mainCamera.ScreenPointToRay(pos);
			Ray rayFakeUI = _fakeUICamera.ScreenPointToRay(pos);
			if (Physics.Raycast(ray, out var hit) && hit.collider.gameObject.TryGetComponent(typeof(WoolControl), out var wool))
			{
				WoolControl objectTarget = (WoolControl)wool;
				objectTarget.WoolRotation();
			}
		}
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
		{
			_isDragging = false;
		}
	}

	private void HandleHold(Vector2 pos)
	{
		if (_blockHold)
		{
			return;
		}
		Ray ray = _mainCamera.ScreenPointToRay(pos);
		if (Physics.Raycast(ray, out var hit))
		{
			if (hit.collider.gameObject.TryGetComponent(typeof(WoolControl), out var wool))
			{
				_targetWool = (WoolControl)wool;
				_isHolding = true;
				_targetWool.SetTranparentWool(true);
				OnHandleHoldWoolAction?.Invoke();
				HoldClickTime++;
			}
			else
			{
				_targetWool = null;
			}
		}
	}

	private Vector2 AdjustLunaMousePosition(Vector2 originalPos)
	{
		float screenWidth = Screen.width;
		float screenHeight = Screen.height;
		float offsetX = 0f;
		float offsetY = 0f;
		return new Vector2(originalPos.x * screenWidth / 886f, originalPos.y * screenHeight / 1920f) + new Vector2(offsetX, offsetY);
	}

	private void HandleDrag(Vector2 pos)
	{
		if (_isActive && !OnZoomCamera() && !_blockDrag && DragStyle == DraggingStyle.Instantly)
		{
			Vector3 rotateDir = new Vector3(pos.y, 0f - pos.x, 0f) * RotationSpeed;
			ModelPrefab.transform.Rotate(rotateDir, Space.World);
			OnHandleDragWoolAction?.Invoke();
		}
	}

	private void HandleDragSmoothly(Vector2 pos)
	{
		if (!_isActive || _blockDrag)
		{
			return;
		}
		if (ZoomStyle == ZoomCameraStyle.Instantly)
		{
			if (OnZoomCamera())
			{
				return;
			}
		}
		else if (isZooming)
		{
			return;
		}
		_isDragging = true;
		Vector3 rotateDir = new Vector3(pos.y, 0f - pos.x, 0f) * DraggingSpeed;
		Quaternion delta = Quaternion.Euler(rotateDir);
		targetRotation = delta * targetRotation;
		OnHandleDragWoolAction?.Invoke();
	}

	private void ReCenterModel()
	{
		BlockRotation = true;
		bool flag = false;
		modelTransfrom.DORotate(Vector3.zero, 0.5f).SetEase(Ease.InOutCubic).OnComplete(delegate
		{
			BlockRotation = false;
			targetRotation = modelTransfrom.rotation;
		});
	}

	private bool OnZoomCamera()
	{
		if (!ZoomCameraData || !_mainCamera)
		{
			return false;
		}
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
		if (ZoomCameraData == null || _mainCamera == null)
		{
			return false;
		}
		if (Input.touchCount >= 2)
		{
			Touch touch0 = Input.GetTouch(0);
			Touch touch1 = Input.GetTouch(1);
			float currentDistance = Vector2.Distance(touch0.position, touch1.position);
			if (!isZooming)
			{
				previousDistance = currentDistance;
				isZooming = true;
			}
			else
			{
				float deltaDistance = currentDistance - previousDistance;
				previousDistance = currentDistance;
				float screenScale = Mathf.Min(Screen.width, Screen.height);
				float normalizedDelta = deltaDistance / screenScale;
				targetFOV -= normalizedDelta * ZoomCameraData.ZoomSpeed * 500f;
				targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
			}
			return true;
		}
		isZooming = false;
		return false;
	}

	public void ZoomCamera(float fovCam)
	{
		fovCam = Mathf.Clamp(fovCam, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
		float scaleRatio = Mathf.Tan(fovCam * 0.5f * (3.14159265f / 180f)) / Mathf.Tan(ZoomCameraData.MaxFOV * 0.5f * (3.14159265f / 180f));
		BackGround.localScale = LocalScaleBackGroundDefault * scaleRatio;
		BackGround.transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 25f;
		BackGround.transform.rotation = Quaternion.LookRotation(_mainCamera.transform.forward);
		if (ZoomStyle == ZoomCameraStyle.Smoothly)
		{
			targetFOV = fovCam;
		}
		_mainCamera.fieldOfView = fovCam;
	}

	public void ZoomCameraAdditional(float additionalFOV)
	{
		if ((bool)ZoomCameraData && (bool)_mainCamera)
		{
			float currentTargetFOV = targetFOV;
			targetFOV = _mainCamera.fieldOfView + additionalFOV;
			targetFOV = Mathf.Clamp(targetFOV, ZoomCameraData.MinFOV, ZoomCameraData.MaxFOV);
		}
	}

	public void ResetCamearState()
	{
		if ((bool)_mainCamera && (bool)ZoomCameraData && (bool)BackGround)
		{
			_mainCamera.fieldOfView = ZoomCameraData.DefaultFOV;
			BackGround.localScale = LocalScaleBackGroundDefault;
		}
	}

	public void StartIntro()
	{
		_isActive = false;
		if (introCoroutine != null)
		{
			StopCoroutine(introCoroutine);
		}
		introCoroutine = StartCoroutine(IntroExecuteAsync());
	}

	private IEnumerator IntroExecuteAsync()
	{
		if (!_mainCamera || !modelTransfrom)
		{
			yield return null;
		}
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
					Singleton<GamePlaySystem>.Instance.ActiveHandController(true);
				}
			}
			if (modelTransfrom != null)
			{
				modelTransfrom.Rotate(0f, ModelRotationIntroSpeed * Time.deltaTime, 0f, Space.World);
			}
			yield return null;
		}
		_isActive = true;
		_mainCamera.DOFieldOfView(IntroEndFOV, IntroCameraZoomInDuration).SetEase(Ease.InOutSine);
		yield return Yielders.Get(IntroCameraZoomInDuration);
		_isActive = true;
	}

	private bool IsVectorInRangeUpward(Vector3 original, Vector3 target, float offset)
	{
		return original.y > target.y - offset && original.y < target.y + offset;
	}
}
