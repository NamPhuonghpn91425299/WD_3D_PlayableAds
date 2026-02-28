using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class CubeTargetControl : MonoBehaviour
{
    #region PROPERTIES

    [SerializeField]
    ColorPalleteData_new colorPalleteData;

    [SerializeField]
    private int VibrationStrength = 50;
    public Text PriorityColor;

    public List<Transform> TargetChildren;
    public GameObject UnLock;
    public GameObject AdsObj;
    [SerializeField]
    private Sprite AddCubeIcon;
    public MeshRenderer[] MeshRenderer;
    public bool IsActive;

    public AddCubeTargetData AddCubeDataSO;


    [SerializeField]
    private TargetBoxAnimator m_boxAnimator;
    [SerializeField]
    private float _boxMoveAnimation = 0.5f;
    [SerializeField]
    private BoxCollider _boxCollider;

    public float RollWoolTime = 0.5f;
    public float DelayTime = 0.3f;

    [SerializeField]
    private GameObject _lockCap;
    [SerializeField]
    private GameObject _unlockCap;
    // [SerializeField]
    // private TMP_Text _lockCapText;


    private bool _isActiveGenNew;
    private bool _alowSameColor;
    private bool _isPlayAnim;
    private int _indexCube;
    private int _indexChild = 0;
    private string _currentColor = ShaderPropertiesLib.IgnoredWoolColorKey;

    private const int TotalChild = 3;

    private int _openCubeCost;
    private bool _isReady;

    public bool IsReady => _isReady;

    [Header("vfx explosion")]
    [SerializeField]
    private ParticleSystem vfxExplosion;

    private readonly Color _defaultColor = new Color(0.5490196f, 0.5490196f, 0.5490196f, 1f);

    private float _startTime;

    #endregion

    public static CancellationTokenSource WaitingAnimTokenSource = new CancellationTokenSource();
    #region MAIN_METHODS

#if UNITY_EDITOR
    private void OnValidate()
    {
        _boxCollider = GetComponent<BoxCollider>();
        colorPalleteData ??= AssetDatabase.LoadAssetAtPath<ColorPalleteData_new>("Assets/_Game/Scripts/DataSO/ColorPallete/ColorPalleteData.asset");
    }
#endif

    private void Awake()
    {
        // GameEventManager.OnLanguageChanged += ChangeLockText;
        // ChangeLockText();
    }

    //private void OnDestroy() { GameEventManager.OnLanguageChanged -= ChangeLockText; }

    public void AddChild(int indexCube, out Transform child)
    {
        if (_indexChild == TotalChild)
        {
            child = null;
            return;
        }

        child = TargetChildren[_indexChild];
        if (_indexChild + 1 == TotalChild)
        {
            StartCoroutine(WaitingAnim(indexCube));
        }
        if (child != null)
            _indexChild++;
    }

    public void SetPlayAnim(bool isPlayAnim) { _isPlayAnim = isPlayAnim; }

    public void SetColor(string color)
    {
        if (string.IsNullOrEmpty(color))
        {
            _currentColor = ShaderPropertiesLib.IgnoredWoolColorKey;
            return;
        }
        _currentColor = color;
        _startTime = Time.time;
    }

    // public void SetACtiveVacuumCleanerAnimation(bool isActive)
    // {
    //     //_vacuumCleaner.SetActive(isActive);
    //     _isPlayAnim = isActive;
    //     if (isActive)
    //     {
    //         skeletonAnimation.AnimationState.SetAnimation(0, "vacuum_booster", skeletonAnimation.loop);
    //     }
    //     else
    //     {
    //         skeletonAnimation.AnimationState.ClearTrack(0);
    //     }
    // }

    public void SetPriority(float actionAmount)
    {
        PriorityColor.text = Math
           .Round(actionAmount, 2)
           .ToString();
    }

    public string GetColor() => _currentColor;
    public bool IsPlayAnim() => _isPlayAnim;


    public int TakeChildMissPartCount() => 3 - _indexChild;

    public void SetActiveCubeTarget(int indexCube, bool active)
    {
        _currentColor = ShaderPropertiesLib.IgnoredWoolColorKey;
        _isReady = active;
        _indexCube = indexCube;
        _boxCollider.enabled = !active;
        AdsObj.SetActive(!active);
        UnLock.SetActive(!active);
        // _lockCap.SetActive(DataManager.PlayerInfoData.Level < AddCubeDataSO.LevelOpen);
        // _unlockCap.SetActive(DataManager.PlayerInfoData.Level >= AddCubeDataSO.LevelOpen);
        _alowSameColor = !active; // mặc định mở 2 cube đầu sẽ không được phép xuất hiện 2 màu trùng nhau
        // 2 cube sau sẽ được phép xuất hiện 2 màu trùng nhau
        IsActive = active;
        if (active && indexCube != -1)
        {
            GamePlayManager.Instance.CubeReadyCount++;
            GamePlayManager.Instance.TotalCubeActive++;
        }
        if (indexCube == 2)
            _openCubeCost = AddCubeDataSO.FirstCubeTargetCost;
        else if (indexCube == 3)
            _openCubeCost = AddCubeDataSO.SecondCubeTargetCost;
    }

    //private void ChangeLockText(string language = "") { _lockCapText.text = string.Format(TextContentDisplay.GetI2("level"), AddCubeDataSO.LevelOpen); }

    public bool CheckColor(string color) { return color == _currentColor; }


    private IEnumerator WaitingAnim(int indexCube)
    {
        _isPlayAnim = true;
        WaitingAnimTokenSource = new CancellationTokenSource();

        GamePlayManager.Instance.CubeReadyCount--;
        GamePlayManager.Instance.CheckLockVacuumCleaner();
        _isReady = false;

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = false;
        if (!skipAnimationDelays)
        {
            yield return new WaitForSeconds(RollWoolTime + DelayTime);
            if (WaitingAnimTokenSource.Token.IsCancellationRequested) yield break;
        }

        //InGameCongratulationManager.Instance?.TriggerCollection();
        m_boxAnimator?.CloseAndMoveOut();

        GamePlayManager.Instance.GenColorForDefaultCube(indexCube);
        _isActiveGenNew = GamePlayManager.Instance.HasCube;

        vfxExplosion?.Play();

        if (!skipAnimationDelays)
        {
            yield return new WaitForSeconds(m_boxAnimator?.CloseDuration ?? 0.4f);
            if (WaitingAnimTokenSource.Token.IsCancellationRequested) yield break;
        }

        //if (DeviceVibrationManager.Instance != null) DeviceVibrationManager.Instance.ExecuteVibrationSingle(VibrationStrength);

        if (!skipAnimationDelays)
        {
            yield return new WaitForSeconds(m_boxAnimator?.MoveOutDuration ?? 0.95f);
            if (WaitingAnimTokenSource.Token.IsCancellationRequested) yield break;
        }
        yield return null;
        if (WaitingAnimTokenSource.Token.IsCancellationRequested) yield break;
        SetDefault();
        GamePlayManager.Instance.CheckTurnOffCube(indexCube, _isActiveGenNew);
        GamePlayManager.Instance.FinishedCollectingCube();
        //GamePlayUIManager.Instance.UpdateProgressTextAndIcon();
        ChangeColor();
        if (indexCube != -1) m_boxAnimator?.FlyIn();

        SoundManager.Instance?.PlayOneShot("box_whoosh");


        //SetACtiveVacuumCleanerAnimation(false);
        GamePlayManager.Instance.CheckEndGame();

        if (!skipAnimationDelays)
        {
            yield return new WaitForSeconds(m_boxAnimator?.FlyInDuration ?? 0.9f);
            if (WaitingAnimTokenSource.Token.IsCancellationRequested) yield break;
        }

        GamePlayManager.Instance.CheckColorInQueuePool(_currentColor, indexCube);
        _isReady = true;
        GamePlayManager.Instance.CubeReadyCount++;
        GamePlayManager.Instance.CheckLockVacuumCleaner();
        GamePlayManager.Instance.CheckEndGame();
    }

    public void SetDefault(bool isResetLevel = false)
    {
        _isReady = true;
        _isPlayAnim = false;
        _indexChild = 0;
        gameObject.SetActive(true);
        // if (isResetLevel)
        //     SetACtiveVacuumCleanerAnimation(false);
        foreach (var child in TargetChildren)
        {
            if (child.childCount < 1) continue;
            var rollWool = child.GetChild(0)
               .gameObject;
            rollWool.transform.SetParent(null);
            rollWool.transform.localScale = Vector3.one;
            GenericObjectPool.Instance.PushToPool_Object(ref rollWool);
        }
        if (_indexCube == -1) return;
        ResetDefaultColor();
    }

    public void KillAllAnim() => m_boxAnimator.KillAllSequences();

    public void ResetAnim() => m_boxAnimator.ResetToDefault();

    public void DisplayRainBowBoxAnimation()
    {
        if (transform.localPosition.x < 0)
        {
            transform.DOKill();
            transform
               .DOLocalMoveX(transform.localPosition.x - 0.6f, _boxMoveAnimation)
               .SetEase(Ease.OutBack);
        }
        else if (transform.localPosition.x > 0)
        {
            transform.DOKill();
            transform
               .DOLocalMoveX(transform.localPosition.x + 0.6f, _boxMoveAnimation)
               .SetEase(Ease.OutBack);
        }
        else
        {
            transform.DOKill();
            transform
               .DOLocalMoveY(-0.45f, _boxMoveAnimation)
               .SetEase(Ease.OutBack);
            transform
               .DOLocalMoveZ(0, _boxMoveAnimation)
               .SetEase(Ease.OutBack);
        }
    }

    public void HideRainBowBoxAnimation()
    {
        if (transform.localPosition.x < 0)
        {
            transform.DOKill();
            transform
               .DOLocalMoveX(transform.localPosition.x + 0.6f, _boxMoveAnimation)
               .SetEase(Ease.OutBack);
        }
        else if (transform.localPosition.x > 0)
        {
            transform.DOKill();
            transform
               .DOLocalMoveX(transform.localPosition.x - 0.6f, _boxMoveAnimation)
               .SetEase(Ease.OutBack);
        }
        else
        {
            transform.DOKill();
            transform
               .DOLocalMoveZ(-20f, _boxMoveAnimation)
               .SetEase(Ease.OutBack);
        }
    }

    public void ChangeColor()
    {
        if (string.IsNullOrEmpty(_currentColor)) return;
        if (_currentColor.Equals(ShaderPropertiesLib.IgnoredWoolColorKey)) return;
        try
        {
            foreach (var meshRenderer in MeshRenderer)
            {
                if (meshRenderer == null) continue;
                if (!colorPalleteData.colorPallete_New.TryGetValue(_currentColor, out var mat)) continue;
                meshRenderer.material = mat;
                var runtimeMaterial = meshRenderer.material;
                runtimeMaterial.SetVector("_LightDir", AddCubeDataSO.LightDirection);
                runtimeMaterial.SetColor(ShaderPropertiesLib.Color, mat.color);
            }
        }
        catch { }
    }

    public void ResetDefaultColor()
    {
        foreach (var meshRenderer in MeshRenderer)
        {
            if (meshRenderer == null) continue;
            var runtimeMaterial = meshRenderer.material;
            runtimeMaterial.SetColor(ShaderPropertiesLib.Color, _defaultColor);
        }
    }

    private bool _canBeOpen;
    public void ActiveOpenCube(bool isActive) { _canBeOpen = isActive; }

    public void BakeAnimPosition() { m_boxAnimator.BakePrePos(); }
    public IEnumerator OnOpenCube()
    {
        GameEventManager.PlayAnimPreLose?.Invoke(false);
        SetActiveCubeTarget(_indexCube, true);
        _alowSameColor = true;
        m_boxAnimator.FlyIn();
        GamePlayManager.Instance.GenColorForDefaultCube(_indexCube, true);
        ChangeColor();
        GamePlayManager.Instance.SetOpenFullCube();

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = false;
#if AI_AGENT
        skipAnimationDelays = DataManager.PlayerData.IsNative;
#endif

        if (!skipAnimationDelays)
        {
            yield return new WaitForSeconds(m_boxAnimator.FlyInDuration);
        }

        GamePlayManager.Instance.CheckColorInQueuePool(_currentColor, _indexCube);
        GamePlayManager.Instance.CheckEndGame();
        GamePlayManager.Instance.CheckEndGame();
    }

    #endregion
}
