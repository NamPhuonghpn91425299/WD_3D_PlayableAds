using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CubeTargetControl : MonoBehaviour
{
    #region PROPERTIES

    [SerializeField] private int VibrationStrength = 50;

    public                   List<Transform> TargetChildren;
    [SerializeField] private Sprite          AddCubeIcon;
    public                   MeshRenderer[]  MeshRenderer;
    public                   bool            IsActive;
    

    [SerializeField] private TargetBoxAnimation _boxAnimation;
    [SerializeField] private float              _boxMoveAnimation = 0.5f;
    [SerializeField] private BoxCollider        _boxCollider;

    public float RollWoolTime = 0.5f;
    public float DelayTime    = 0.3f;


    private bool  _isActiveGenNew;
    private bool  _alowSameColor;
    private int   _indexCube;
    private int   _indexChild   = 0;
    private Color _currentColor = Color.black;

    private const int TotalChild = 3;

    private bool _isReady;

    public bool IsReady => _isReady;


    private readonly Color _defaultColor = new Color(0, 0.759f, 0.6667294f, 1f);

    #endregion

    #region MAIN_METHODS

#if UNITY_EDITOR
    private void OnValidate() { _boxCollider = GetComponent<BoxCollider>(); }
#endif

    // public void OnEnable()
    // {
    //     PriorityColor.enabled = 0 == PlayerPrefs.GetInt(AccountTool.HideCubeStepPref, 0);
    // }

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

        _indexChild++;
    }

    public void SetColor(Color color)
    {
        _currentColor = color;
    }


    public Color GetColor() => _currentColor;

    public void SetActiveCubeTarget(int indexCube, bool active)
    {
        _isReady             = active;
        _indexCube           = indexCube;
        _boxCollider.enabled = !active;
        _alowSameColor = !active; // mặc định mở 2 cube đầu sẽ không được phép xuất hiện 2 màu trùng nhau
        // 2 cube sau sẽ được phép xuất hiện 2 màu trùng nhau
        IsActive = active;
        if (active && indexCube != -1)
        {
            GamePlaySystem.Instance.CubeReadyCount++;
            GamePlaySystem.Instance.TotalCubeActive++;
        }
    }

    public bool CheckColor(Color color) { return color == _currentColor; }

    private IEnumerator WaitingAnim(int indexCube)
    {
        GamePlaySystem.Instance.GenNewCube(indexCube);
        
        _isActiveGenNew = GamePlaySystem.Instance.HasCube;
        GamePlaySystem.Instance.CubeReadyCount--;
        _isReady = false;
        yield return new WaitForSeconds(RollWoolTime + DelayTime);
        
        
        _boxAnimation.CloseAndMoveOut();
        
        
        yield return new WaitForSeconds(_boxAnimation.CloseDuration);

        yield return new WaitForSeconds(_boxAnimation.MoveOutDuration);

        SetDefault();
        GamePlaySystem.Instance.CheckTurnOffCube(indexCube, _isActiveGenNew);
        GamePlaySystem.Instance.FinishedCollectingCube();
        ChangeColor();
        if(indexCube != -1) _boxAnimation.FlyIn();
        
        yield return new WaitForSeconds(_boxAnimation.FlyInDuration);
        
        GamePlaySystem.Instance.CubeReadyCount++;
        _isReady = true;
        GamePlaySystem.Instance.UseQueueTarget(_currentColor, indexCube);
        GamePlaySystem.Instance.CheckEndGame();
    }

    public void SetDefault()
    {
        _isReady    = true;
        _indexChild = 0;
        gameObject.SetActive(true);
        foreach (var child in TargetChildren)
        {
            if (child.childCount < 1) continue;
            var rollWool = child.GetChild(0);
            rollWool.parent = null;
            rollWool.gameObject.SetActive(false);
        }
        if (_indexCube == -1) return;
        ResetDefaultColor();
    }

    public void DisplayRainBowBoxAnimation()
    {
        if (transform.localPosition.x < 0)
        {
            transform.DOKill();
            transform
               .DOLocalMoveX(transform.localPosition.x - 0.6f, _boxMoveAnimation)
               .SetEase(Ease.OutBack)
               .OnComplete(() => { });
        }
        else if (transform.localPosition.x > 0)
        {
            transform.DOKill();
            transform
               .DOLocalMoveX(transform.localPosition.x + 0.6f, _boxMoveAnimation)
               .SetEase(Ease.OutBack)
               .OnComplete(() => { });
        }
        else
        {
            transform.DOKill();
            transform
               .DOLocalMoveY(-0.45f, _boxMoveAnimation)
               .SetEase(Ease.OutBack)
               .OnComplete(() => { });
            transform
               .DOLocalMoveZ(0, _boxMoveAnimation)
               .SetEase(Ease.OutBack)
               .OnComplete(() => { });
        }
    }

    public void HideRainBowBoxAnimation()
    {
        if (transform.localPosition.x < 0)
        {
            transform.DOKill();
            transform
               .DOLocalMoveX(transform.localPosition.x + 0.6f, _boxMoveAnimation)
               .SetEase(Ease.OutBack)
               .OnComplete(() => { });
        }
        else if (transform.localPosition.x > 0)
        {
            transform.DOKill();
            transform
               .DOLocalMoveX(transform.localPosition.x - 0.6f, _boxMoveAnimation)
               .SetEase(Ease.OutBack)
               .OnComplete(() => { });
        }
        else
        {
            transform.DOKill();
            transform
               .DOLocalMoveZ(-20f, _boxMoveAnimation)
               .SetEase(Ease.OutBack)
               .OnComplete(() => { });
        }
    }

    public void ChangeColor()
    {
        if (_currentColor == Color.black) return;

        foreach (var meshRenderer in MeshRenderer)
        {
            var propertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, _currentColor);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void ResetDefaultColor()
    {
        foreach (var meshRenderer in MeshRenderer)
        {
            var propertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, _defaultColor);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void ActiveOpenCube(bool isActive)
    {
        _boxCollider.enabled = isActive;
    }

    public void BakeAnimPosition() { _boxAnimation.BakePrePos(); }
    

    #endregion
}
