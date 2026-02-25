using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayUIManager : SingletonBase<GamePlayUIManager>
{
    #region PROPERTIES

    public RectTransform WoolBasket;
    [Header("Level")] public Text LevelText;

    [Header("Target")] public Text TargetPass;

    [Header("ZOOM OLD")] public Slider ZomSlider;


    public ZoomCameraData ZoomCameraData;
    public float ZoomDistance = 2f;

    [Header("ZOOM NEW")]
    public float ZoomStep = 5f;
    public Button ZoomInButton;
    public Button ZoomOutButton;
    public Button ReCenterButton;

    public RectTransform levelProgress;

    //[Header("Effect")] public ParticleFlyEffectController starFlyEffectPrefab;

    private int _totalTargetPassed = 0;
    private int _indexColorCubeTarget = -1;
    private int _indexColorQueueTarget = 0;

    //[Header("Broom booster")] [SerializeField]    private BroomBooster   broomBooster;
    //[Header("Add Hold booster")] [SerializeField] private RedoBooster redoBooster;

    private MaterialPropertyBlock _iconPercentPropertyBlock;


    private readonly Vector3 _displayWoolBasketPosition = new Vector3(-34, -534, 0);
    private readonly Vector3 _hideWoolBasketPosition = new Vector3(120, -534, 0);
    #endregion

    #region UNITY_METHODS

    private void Start()
    {
        GameEventManager.OnLoadLevelDone += () =>
        {
            WoolBasket.anchoredPosition = _hideWoolBasketPosition;
        };
    }

    private void OnEnable()
    {
        RegisterButton();
        //SetBroomBoosterInteractable(true);
        //SetRedoBoosterInteractable(true);
        //SetLevelText(DataManager.PlayerData.Level);
        _iconPercentPropertyBlock = new MaterialPropertyBlock();
    }

    private void OnDisable() { UnRegisterButton(); }

    #endregion

    #region MAIN_METHODS


    // public void UpdateProgressVisual(int? indexCube = null)
    // {
    //     try
    //     {
    //         if (indexCube != null)
    //         {
    //             var flyEffect = Instantiate(starFlyEffectPrefab, transform);
    //             flyEffect.SetStart(indexCube == -1
    //                     ? GamePlaySystem.Instance.RainBowTargetControl.transform
    //                     : GamePlaySystem.Instance.CurrentCubeTargets[indexCube.Value].transform
    //                 );
    //             flyEffect.SetEnd(iconPercent.rectTransform);
    //             flyEffect.Init();
    //             flyEffect.Play();
    //             WaitUpdateVisual(flyEffect.FlyDuration)
    //                .Forget();
    //         }
    //         else
    //         {
    //             UpdateProgressTextAndIcon();
    //         }
    //     } catch (Exception e)
    //     {
    //         Debug.LogError("GamePlayUI: UpdateProgressVisual error: " + e);
    //     }
    // }

    // public void SetBroomBoosterInteractable(bool isInteractable)
    // {
    //     if (broomBooster)
    //     {
    //         broomBooster.SetEffectInteractable(isInteractable);
    //         broomBooster.SetInteractableButton(true);
    //     }
    // }

    // public void SetBroomButtonInteractable(bool interactable)
    // {
    //     if (broomBooster)
    //     {
    //         broomBooster.SetInteractableButton(interactable);
    //         broomBooster.SetEffectInteractable(interactable);
    //     }
    // }

    // public void OfferUseBroomBooster()
    // {
    //     if (broomBooster)
    //     {
    //         broomBooster.OfferUseBroomBooster();
    //     }
    // }

    // public void SetRedoBoosterInteractable(bool isInteractable)
    // {
    //     if (redoBooster)
    //     {
    //         redoBooster.SetRedoInteractable(isInteractable);
    //         redoBooster.SetInteractableButton(true);
    //     }
    // }

    // public void SetRedoButtonInteractable(bool interactable)
    // {
    //     if (redoBooster)
    //     {
    //         redoBooster.SetInteractableButton(interactable);
    //         redoBooster.SetRedoInteractable(interactable);
    //     }
    // }

    public void UpdateProgressTextAndIcon()
    {
        try
        {
            var targetPassed = GamePlayManager.Instance.CurrentCubeCollected;
            var totalTarget = GamePlayManager.Instance.TotalColor;
            _totalTargetPassed = targetPassed;
            TargetPass.text = $"{_totalTargetPassed}/{totalTarget}";

        }
        catch { }
    }

    float _lastValueZoom = -1f;
    public void ChangeValueZoom(float value)
    {
        if (Mathf.Approximately(_lastValueZoom, value)) return;
        _lastValueZoom = value;
        var changeToValue = 1 - (value - ZoomCameraData.MinFOV) / (ZoomCameraData.MaxFOV - ZoomCameraData.MinFOV);
        ZomSlider.value = changeToValue;
    }

    public void ChangeValueZoomNoNotify(float value)
    {
        if (Mathf.Approximately(_lastValueZoom, value)) return;
        _lastValueZoom = value;
        var changeToValue = 1 - (value - ZoomCameraData.MinFOV) / (ZoomCameraData.MaxFOV - ZoomCameraData.MinFOV);
        ZomSlider.SetValueWithoutNotify(changeToValue);
    }

    public void SetLevelText(int level) { LevelText.text = $"Level {level}"; }

    public void ActiveWoolBasket(bool isActive)
    {
        var nextPos = isActive ? _displayWoolBasketPosition : _hideWoolBasketPosition;
        WoolBasket.DOKill();
        WoolBasket
           .DOAnchorPos(nextPos, 1.2f)
           .SetEase(Ease.OutBack);
    }

    #endregion

    #region HELPER

    private void RegisterButton()
    {
        ZomSlider.onValueChanged.AddListener(OnZomSilder);

        ZoomInButton.onClick.AddListener(() => ZoomCameraExecute(true));
        ZoomOutButton.onClick.AddListener(() => ZoomCameraExecute(false));
        ReCenterButton.onClick.AddListener(() => GameEventManager.ReCenterModelThroughButton?.Invoke());
    }

    private void UnRegisterButton() { ZomSlider.onValueChanged.RemoveListener(OnZomSilder); }

    #endregion

    #region BUTTON_EVENTS

    private void OnZomSilder(float value)
    {
        var changeTotalFov = Mathf.Lerp(ZoomCameraData.MaxFOV, ZoomCameraData.MinFOV, value);
        GamePlayManager.Instance.CameraController.ZoomCamera(changeTotalFov);
    }

    public void ZoomCameraExecute(bool zoomIn)
    {
        GameEventManager.ChangeCameraFOVThroughButton?.Invoke(zoomIn ? -ZoomStep : +ZoomStep);
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ResetData() { ZomSlider.value = 0; }

    #endregion
}
