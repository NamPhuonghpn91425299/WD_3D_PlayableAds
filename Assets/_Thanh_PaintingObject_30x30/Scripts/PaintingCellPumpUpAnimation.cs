using static PaintingSharedAttributes;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class PaintingCellPumpUpAnimation : MonoBehaviour
{
    #region PROPERTIES
    [Header("OBJECT")]
    public Image StockCellImage;
    public ParticleSystem WoolTrailParticle;
    private bool initialized = false;
    private ParticleSystem.MainModule mainModule;
    public GameObject ThisGameObject;

    [Header("ANIMATION")]
    private RectTransform CellRect;
    public Ease IncreaseEaseType = Ease.InOutBounce;
    public Ease DecreaseEaseType = Ease.InOutBounce;
    public Vector3 DefaultScale = Vector3.one;
    public Vector3 PulseScale = new Vector3(1.2f, 1.2f, 1);
    public float IncreaseDuration = 0.5f;
    public float DecreaseDuration = 0.5f;
    private Color lastColor = Color.green;

    private Sequence pumpSequence;
    private PaintingPumpAnimationManager pool;
    #endregion

    #region UNITY CORE

    #endregion

    #region MAIN
    public void Init(PaintingPumpAnimationManager pool)
    {
        if (initialized) return;
        this.pool = pool;

        ThisGameObject = gameObject;
        CellRect = StockCellImage.rectTransform;

        CellRect = StockCellImage.rectTransform;
        StockCellImage.material = new Material(StockCellImage.material);
        mainModule = WoolTrailParticle.main;

        pumpSequence = DOTween.Sequence()
            .Append(CellRect.DOScale(PulseScale, IncreaseDuration).SetEase(IncreaseEaseType))
            .Append(CellRect.DOScale(DefaultScale, DecreaseDuration).SetEase(DecreaseEaseType))
            .Pause()
            .SetAutoKill(false)
            .OnComplete(() => StopPumpAnimation());

        initialized = true;
    }
    public void StartAnimation(Color color, Vector3 position)
    {
        if (lastColor != color)
        {
            lastColor = color;
            SetTrailColor(color);
            StockCellImage.material.SetColor(CellColorKey, color);
        }
        
        CellRect.position = position;
        StartPumpAnimation();
    }

    #region _animation
    public void StartPumpAnimation()
    {
        ThisGameObject.SetActive(true);
        CellRect.DOKill();
        CellRect.localScale = DefaultScale;
        pumpSequence.Restart();
    }

    public void StopPumpAnimation()
    {
        pumpSequence.Pause();
        CellRect.localScale = DefaultScale;
        pool.ReturnToPool(this);
        ThisGameObject.SetActive(false);
    }

    [ContextMenu("RESTART ANIMATION")]
    public void RestartHeartAnimation()
    {
        if (!initialized) Init(null);
        CellRect.localScale = DefaultScale;
        pumpSequence.Restart();
    }


    #endregion

    #endregion

    #region SUPPORTIVE
    public void SetTrailColor(Color newColor)
    {
        mainModule.startColor = newColor;
    }
    #endregion
}