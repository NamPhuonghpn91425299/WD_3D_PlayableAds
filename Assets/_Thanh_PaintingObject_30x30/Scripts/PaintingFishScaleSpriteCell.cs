using static PaintingSharedAttributes;
using UnityEngine;
using DG.Tweening;

public class PaintingFishScaleSpriteCell : MonoBehaviour
{
    #region PROPERTIES
    public int Row = 0;
    public int Column = 0;
    public float Brightness = 2;

    public SpriteRenderer SpriteRenderer;
    public Color BaseColor;
    public Color DesiredColor;

    private MaterialPropertyBlock _propertyBlock;

    [HideInInspector] public Transform thisTransform;
    [HideInInspector] public Vector3 WorldPosition;

    private GameObject _gameObject;

    [Header("ANIMATION")]
    public Vector3 OriginalScale;
    public Vector3 PumpScale;
    Sequence pumpSequence;

    private bool _initialized = false;
    #endregion

    #region UNITY CORE
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (SpriteRenderer == null)
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
#endif

    private void Awake()
    {
        Initialize();
    }
    #endregion

    #region MAIN
    public void Setup(Sprite sprite, Color targetColor, Color baseColor, int row, int column, float brightness = 2f)
    {
        Row = row;
        Column = column;
        BaseColor = baseColor;
        Brightness = brightness;
        DesiredColor = targetColor;
        if (_propertyBlock == null) _propertyBlock = new MaterialPropertyBlock();

        SpriteRenderer.sprite = sprite;
        SetColorAndBrightness(targetColor);

        SpriteRenderer.enabled = !BakedPaintingBackground;
    }
     
    public void SetDesiredColor(Color color)
    {
        CheckInitialize();
        if (color == DesiredColor) return;
        DesiredColor = color;
        SetColorAndBrightness(color);
    }

    public void Apply()
    {
        CheckInitialize();
        CheckActive();
        if (BakedPaintingBackground)
        {
            SpriteRenderer.enabled = true;
        }
        else SetColorAndBrightness(DesiredColor);
    }
    public void Clear()
    {
        CheckInitialize();
        if (BakedPaintingBackground)
        {
            SpriteRenderer.enabled = false;
            _gameObject.SetActive(false);
        }
        else SetColorAndBrightness(BaseColor);
    }

    private void SetColorAndBrightness(Color color)
    {
        CheckInitialize();
        //_propertyBlock.Clear();
        _propertyBlock.SetColor(CellColorKey, color);
        _propertyBlock.SetFloat(BrightnessKey, Brightness);
        SpriteRenderer.SetPropertyBlock(_propertyBlock);
    }

    #region _animation
    public Tween CreatePumpTween(Vector3 targetScale, Color color, float duration, bool forceEnable = false)
    {
        CheckInitialize();
        CheckActive();

        if (forceEnable) SpriteRenderer.enabled = true;
        SetColorAndBrightness(color);

        pumpSequence = DOTween.Sequence();
        pumpSequence.Append(thisTransform.DOScale(targetScale, duration / 2f));
        pumpSequence.Append(thisTransform.DOScale(OriginalScale, duration / 2f));
        pumpSequence.OnComplete(() =>
        {
            if (forceEnable) SpriteRenderer.enabled = false;
            SetColorAndBrightness(DesiredColor);
            thisTransform.localScale = OriginalScale;
        });

        return pumpSequence;
    }
    #endregion
    #endregion

    #region SUPPORTIVE
    private void CheckInitialize()
    {
        if (_initialized) return;
        Initialize();
    }
    private void Initialize()
    {
        _gameObject = gameObject;
        thisTransform = transform;
        WorldPosition = thisTransform.position;
        OriginalScale = thisTransform.localScale;
        _propertyBlock = new MaterialPropertyBlock();
        _propertyBlock.SetTexture(MainTexKey, SpriteRenderer.sprite.texture);
        _initialized = true;
    }
    private void CheckActive()
    {
        if (_gameObject.activeInHierarchy) return;
        _gameObject.SetActive(true);
    }
    public void ResetScale()
    {
        pumpSequence.Pause();
        thisTransform.localScale = OriginalScale;
    }
    #endregion
}
