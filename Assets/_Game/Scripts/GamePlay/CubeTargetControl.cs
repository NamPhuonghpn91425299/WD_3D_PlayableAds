using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class CubeTargetControl : MonoBehaviour
{
    #region PROPERTIES

    [Header("CONTROLLER(s)")] 
    public PaintingLineRendererHandler WoolLineHandler;
    public TargetBarAnimator ThisBarAnimatorController;
    
    public GameObject group;
    [SerializeField] private int VibrationStrength = 50;

    public                   List<Transform> TargetChildrens;
    [SerializeField] private Sprite          AddCubeIcon;
    [FormerlySerializedAs("MeshRenderer")] public                   MeshRenderer[]  meshRenderers;
    public                   bool            IsActive;
    

    [SerializeField] private TargetBoxAnimation _boxAnimation;
    [SerializeField] private float              _boxMoveAnimation = 0.5f;
    [SerializeField] private BoxCollider        _boxCollider;

    public float RollWoolTime = 0.5f;
    public float DelayTime    = 0.3f;
    public ParticleSystem vfxExplosonStar;


    private bool  _isActiveGenNew;
    private bool  _alowSameColor;
    private int   _indexCube;
    private int   _indexChild   = 0;
    public Color _currentColor = Color.black;
    public string nameColor;

    private const int TotalChild = 3;

    private bool _isReady;

    public bool IsReady => _isReady;


    private readonly Color _defaultColor = new Color(0, 0.759f, 0.6667294f, 1f);

    
    [Header("LINE RENDERER")]
    public List<PaintingLineRendererHandler> WoolLineRenderers = new List<PaintingLineRendererHandler>();
    
    private readonly Queue<PaintingLineRendererHandler> linePool = new Queue<PaintingLineRendererHandler>();
    
    [Header("SPIRAL ANIMATION")]
    public float RollInDuration = .25f;
    
    [Header("WOOL SPIRAL ITEMS")]
    public List<GameObject> SpiralItems = new List<GameObject>();

    [Header("WOOL AUDIO")]
    public AudioClip InTro;
    public AudioClip OutTro;

    [Header("Point Move Wool Line")] 
    public Transform WoolLineLeftPoint;
    public Transform WoolLineRightPoint;
    public Transform WoolLineMovePoint;
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
        GameObject availableSpiralItem = SpiralItems.FirstOrDefault(x => !x.activeSelf);
        if (availableSpiralItem != null)
        {
            ThisBarAnimatorController.StartScrollInSpiralItem(SpiralItems.IndexOf(availableSpiralItem));
            availableSpiralItem.SetActive(true);
            child = availableSpiralItem.transform;
        }
        else
        {
            child = null;
        }
        //child = TargetChildrens[_indexChild];
        if (_indexChild + 1 == TotalChild)
        {
            // Check if the game object is active before starting the coroutine
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(WaitingAnim(indexCube));
            }
            else
            {
                Debug.LogWarning($"Cannot start coroutine on inactive game object {gameObject.name}");
            }
        }

        _indexChild++;
    }

    public void SetColor(string color)
    {
        _currentColor = GamePlaySystem.Instance._colorPalleteData.colorPallete[color];
        nameColor = color;
        WoolLineHandler.SetupColor(nameColor,_currentColor);
        foreach (PaintingLineRendererHandler VARIABLE in WoolLineRenderers)
            VARIABLE.SetupColor(nameColor, _currentColor);
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

    public bool CheckColor(string color) { return color == nameColor; }

    private IEnumerator WaitingAnim(int indexCube)
    {
        FishScalesPaintingController fishScalesPaintingController = FishScalesPaintingController.Instance;
        GamePlaySystem.Instance.CubeReadyCount--;
        yield return new WaitForSeconds(1f); // Thời gian chờ giữa mỗi vòng, tuỳ chỉnh
        fishScalesPaintingController.WoolLinePaintingStart(this,indexCube);
    }

    public void StartOuttroGenColorCubeTarget(int indexCube) => StartCoroutine(StartOuttroGenColor(indexCube));
    private IEnumerator StartOuttroGenColor(int indexCube)
    {
        
        yield return new WaitForSeconds(.25f); // Thời gian chờ giữa mỗi vòng, tuỳ chỉnh
        SoundManager.Instance.PlayOneShot(OutTro);
        vfxExplosonStar.Play();
        ThisBarAnimatorController.StartOutro();
        yield return new WaitForSeconds(.5f); // Thời gian chờ giữa mỗi vòng, tuỳ chỉnh
        GamePlaySystem.Instance.GenNewCube(indexCube);
        
        _isActiveGenNew = GamePlaySystem.Instance.HasCube;
        _isReady = false;

        SetDefault();
        GamePlaySystem.Instance.CheckTurnOffCube(indexCube, _isActiveGenNew);
        GamePlaySystem.Instance.FinishedCollectingCube();//sử lí kiểm tra end game qua số lượng hộp, số lượng cuộn len
        ChangeColor();
        if(indexCube != -1)
        {
            // Bake position before starting intro animation to ensure correct positioning
            SoundManager.Instance.PlayOneShot(InTro);
            if (_boxAnimation != null)
                _boxAnimation.BakePrePos();
            
            ThisBarAnimatorController.StartIntro();
        }
        
        yield return new WaitForSeconds(_boxAnimation.FlyInDuration);
        
        GamePlaySystem.Instance.CubeReadyCount++;
        _isReady = true;
        GamePlaySystem.Instance.UseQueueTarget(_currentColor, indexCube);
        
        // yield return new WaitForSeconds(1.5f); // Thời gian chowf 1 lucs mis check end game
        GamePlaySystem.Instance.CheckEndGame();
        // Debug.Log("CubeReadyCount: " + GamePlaySystem.Instance.CubeReadyCount + " TotalCubeActive: " + GamePlaySystem.Instance.TotalCubeActive + " indexCube: " + indexCube);
    }
    

    public void SetDefault()
    {
        for (int i = 2; i < meshRenderers.Length; i++)
            meshRenderers[i].gameObject.SetActive(false);
        
        foreach (GameObject VARIABLE in SpiralItems)
            VARIABLE.SetActive(false);
        _isReady    = true;
        _indexChild = 0;
        
        // Only set active if not already active to avoid position reset
        if (!gameObject.activeInHierarchy)
        {
            // Bake the current position before reactivating to preserve animation state
            if (_boxAnimation != null)
            {
                _boxAnimation.BakePrePos();
            }
            gameObject.SetActive(true);
        }
        
        foreach (var child in TargetChildrens)
        {
            if (child == null)
            {
                print("Null child in CubeTargetControl");
                return;
            }
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
        foreach (var line in WoolLineRenderers)
        {
            if (line.Available()) linePool.Enqueue(line);
        }
        foreach (var meshRenderer in meshRenderers)
        {
            var propertyBlock = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, _currentColor);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }

    public void ResetDefaultColor()
    {
        foreach (var line in WoolLineRenderers) line.ClearLine();
        foreach (var meshRenderer in meshRenderers)
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
    
    public void ConnectWoolLine(Vector3 origin, Vector3 target, Color color, bool firstCell = false)
    {
        if (linePool.Count == 0) return;

        var availableLine = linePool.Dequeue();
        
        // Check if the game object is active before starting the coroutine
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(LiningCoroutine(availableLine, origin, target, color, firstCell));
        }
        else
        {
            Debug.LogWarning($"Cannot start LiningCoroutine on inactive game object {gameObject.name}");
            // Return the line back to the pool if we can't start the coroutine
            linePool.Enqueue(availableLine);
        }
    }
    private IEnumerator LiningCoroutine(PaintingLineRendererHandler lineRendererHandler, Vector3 origin, Vector3 target, Color color, bool firstCell = false)
    {
        lineRendererHandler.ConnectLine(origin, target, color, firstCell);
        yield return new WaitForSeconds(RollInDuration);
        lineRendererHandler.ClearLineLinearFollowUp();

        linePool.Enqueue(lineRendererHandler);
    }

    public string GetPreviousColor()
    {
        return nameColor;
    }
}
