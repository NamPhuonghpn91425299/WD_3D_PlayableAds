using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayUIManager : SingletonBase<GamePlayUIManager>
{
    #region PROPERTIES
    [SerializeField] private UiEndGame uiEndGame;
    [Header("ZOOM OLD")] public Slider ZomSlider;
    public Text ProgressText;
    public ZoomCameraData ZoomCameraData;
    public float ZoomDistance = 2f;

    [Header("ZOOM NEW")]
    public float ZoomStep = 5f;
    public Button ReCenterButton;
    //[Header("Effect")] public ParticleFlyEffectController starFlyEffectPrefab;
    //[Header("Broom booster")] [SerializeField]    private BroomBooster   broomBooster;
    //[Header("Add Hold booster")] [SerializeField] private RedoBooster redoBooster;

    private MaterialPropertyBlock _iconPercentPropertyBlock;
    private int _totalTargetPassed = -1;
    private int _totalTarget = -1;


    private readonly Vector3 _displayWoolBasketPosition = new Vector3(-34, -534, 0);
    private readonly Vector3 _hideWoolBasketPosition = new Vector3(120, -534, 0);
    #endregion

    #region UNITY_METHODS

    private void Start()
    {

    }


    private void OnEnable()
    {
        RegisterButton();

        _iconPercentPropertyBlock = new MaterialPropertyBlock();
    }

    private void OnDisable() { UnRegisterButton(); }

    private void LateUpdate()
    {
        UpdateProgressTextAndIcon();
    }

    #endregion

    #region MAIN_METHODS
    public void SetAlphaEndGamePanel(float alpha)
    {
        if (uiEndGame != null) uiEndGame.SetAlphaPanel(alpha);
    }

    public void ShowWinPanel()
    {
        if (uiEndGame != null) uiEndGame.ShowWinPanel();
    }

    public void ShowLosePanel()
    {
        if (uiEndGame != null) uiEndGame.ShowLosePanel();
    }

    public void UpdateProgressTextAndIcon()
    {
        var gamePlayManager = GamePlayManager.Instance;
        if (gamePlayManager == null)
        {
            SetProgressUI(0, 0);
            return;
        }

        var targetPassed = Mathf.Max(0, gamePlayManager.CurrentCubeCollected);
        var totalTarget = Mathf.Max(0, gamePlayManager.TotalColor);
        SetProgressUI(targetPassed, totalTarget);
    }

    private void SetProgressUI(int targetPassed, int totalTarget)
    {
        if (_totalTargetPassed == targetPassed && _totalTarget == totalTarget) return;
        _totalTargetPassed = targetPassed;
        _totalTarget = totalTarget;

        if (ProgressText != null)
        {
            ProgressText.text = $"{targetPassed}/{totalTarget}";
        }
    }

    float _lastValueZoom = -1f;
    public void ChangeValueZoom(float value)
    {
        if (ZomSlider == null || ZoomCameraData == null) return;
        if (Mathf.Approximately(_lastValueZoom, value)) return;
        _lastValueZoom = value;
        if (Mathf.Approximately(ZoomCameraData.MaxFOV, ZoomCameraData.MinFOV)) return;
        var changeToValue = 1 - (value - ZoomCameraData.MinFOV) / (ZoomCameraData.MaxFOV - ZoomCameraData.MinFOV);
        ZomSlider.value = changeToValue;
    }

    public void ChangeValueZoomNoNotify(float value)
    {
        if (ZomSlider == null || ZoomCameraData == null) return;
        if (Mathf.Approximately(_lastValueZoom, value)) return;
        _lastValueZoom = value;
        if (Mathf.Approximately(ZoomCameraData.MaxFOV, ZoomCameraData.MinFOV)) return;
        var changeToValue = 1 - (value - ZoomCameraData.MinFOV) / (ZoomCameraData.MaxFOV - ZoomCameraData.MinFOV);
        ZomSlider.SetValueWithoutNotify(changeToValue);
    }

    public void ActiveWoolBasket(bool isActive)
    {
        var nextPos = isActive ? _displayWoolBasketPosition : _hideWoolBasketPosition;
    }

    #endregion

    #region HELPER

    private void RegisterButton()
    {
        if (ZomSlider != null) ZomSlider.onValueChanged.AddListener(OnZomSilder);
        ReCenterButton?.onClick.AddListener(OnClickReCenterButton);
        GameEventManager.SetReCenterButtonInteractable += SetReCenterButtonInteractable;
    }

    private void UnRegisterButton()
    {
        if (ZomSlider != null) ZomSlider.onValueChanged.RemoveListener(OnZomSilder);
        if (ReCenterButton != null) ReCenterButton.onClick.RemoveListener(OnClickReCenterButton);
        GameEventManager.SetReCenterButtonInteractable -= SetReCenterButtonInteractable;
    }

    #endregion

    #region BUTTON_EVENTS

    private void OnZomSilder(float value)
    {
        if (ZoomCameraData == null) return;
        var gamePlayManager = GamePlayManager.Instance;
        if (gamePlayManager == null || gamePlayManager.CameraController == null) return;
        var changeTotalFov = Mathf.Lerp(ZoomCameraData.MaxFOV, ZoomCameraData.MinFOV, value);
        gamePlayManager.CameraController.ZoomCamera(changeTotalFov);
    }

    public void ZoomCameraExecute(bool zoomIn)
    {
        GameEventManager.ChangeCameraFOVThroughButton?.Invoke(zoomIn ? -ZoomStep : +ZoomStep);
    }

    private void OnClickReCenterButton()
    {
        GameEventManager.ReCenterModelThroughButton?.Invoke();
    }

    private void SetReCenterButtonInteractable(bool isInteractable)
    {
        if (ReCenterButton == null) return;
        ReCenterButton.interactable = isInteractable;
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ResetData()
    {
        if (ZomSlider != null) ZomSlider.value = 0;
        SetProgressUI(0, 0);
    }

    #endregion
}
