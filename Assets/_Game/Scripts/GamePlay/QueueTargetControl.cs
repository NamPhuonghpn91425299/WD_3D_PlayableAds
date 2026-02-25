
using UnityEngine;
using UnityEngine.UI;

public class QueueTargetControl : MonoBehaviour, IPoolObject
{
    #region PROPERTIES

    [SerializeField]
    private SpriteRenderer iconPlus;

    [SerializeField]
    private int maxQueueTargetCount;

    [SerializeField]
    private BoosterDataSO boosterData;
    [SerializeField]
    private Sprite addHoldIcon;


    [SerializeField]
    private GameObject _lockCap;
    [SerializeField]
    private GameObject _unlockCap;

    [SerializeField]
    private Text _lockCapText;

    [SerializeField]
    private Renderer _renderer;

    public Renderer Renderer => _renderer;
    private string _currentColor = ShaderPropertiesLib.IgnoredWoolColorKey;
    private bool _isActive;
    public bool IsActive => _isActive;

    private WoolRollAnimator _woolRollAnimator;

    #endregion

    private void Start() { } //_lockCapText.text = string.Format("level {0}", boosterData.LevelOpen); }

    #region MAIN_METHODS

    public void SetActive(bool isActive)
    {
        _isActive = !isActive;
        iconPlus.enabled = !isActive;
        // _lockCap.SetActive(!isActive && DataManager.PlayerInfoData.Level < boosterData.LevelOpen);
    }

    public bool AddChild(string color)
    {
        if (_isActive) return false;
        _currentColor = color;
        _isActive = true;
        return true;
    }

    public bool CheckCurrentColor(string color) { return _currentColor.Equals(color); }

    public void SetWoolRollAnimator(WoolRollAnimator woolRollAnimator)
    {
        _woolRollAnimator = woolRollAnimator;
        _woolRollAnimator?.ResetAnim();
    }

    public WoolRollAnimator GetWoolRollAnimator() { return _woolRollAnimator; }


    public void GetNewQueue()
    {
        if (!iconPlus.enabled) return;
        AddHold();
    }


    public void ResetDefault()
    {
        _holeAmount = 0;
        _woolRollAnimator = null;
        _isActive = false;
        _currentColor = ShaderPropertiesLib.IgnoredWoolColorKey;
        if (transform.childCount < 3) return;
        var roll = transform.GetChild(2);
        roll.SetParent(null);
        roll.localScale = Vector3.one;
        GenericObjectPool.Instance.PushToPool(this, roll.gameObject);
    }

    public string GetColorQueue() { return _currentColor; }

    public bool IsAtive() { return _isActive || iconPlus.enabled; }


    public bool IsHasWoolRool() { return _woolRollAnimator; }

    public bool IsPopToBroomPool() { return _woolRollAnimator._isPopToBroomPool; }

    public bool IsReDo() { return _woolRollAnimator && _woolRollAnimator.IsRedo; }

    public bool IsPlayAnim() { return _woolRollAnimator.IsPlayAnim; }

    public bool IsWoolToCube() { return _woolRollAnimator != null && _woolRollAnimator.IsWoolToCube; }

    private int _holeAmount;

    #endregion



    #region USE_BOOSTER

    private void ExecuteBooster()
    {
        GameEventManager.OnAddHoldBoosterComplete?.Invoke();
        AddHold();

    }

    private void AddHold()
    {
        try
        {
            //GameEventManager.OnUseSaveBooster?.Invoke();
            GamePlayManager.Instance?.AddQueueTarget();
            _holeAmount--;
            iconPlus.enabled = false;
            ResetDefault();
        }
        catch { }
    }



    #endregion

    public GameObject Prefab { get; set; }

    public void OnPushToPool() { }
}
