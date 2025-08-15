using static PaintingSharedAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System;

public class PaintingOverviewAnimation : MonoBehaviour
{
    #region PROPERTIES
    [Header("CONTROLLER(s)")]
    public FishScalesPaintingController PaintingObject;

    [Header("CONTROLLER(s)")]
    public Transform PaintingTransform;

    [Header("INTRO ANIMATION - PAINTING")]
    public float IntroAnimationDuration = 1.0f;
    public float PaintingYPositionStart;
    public float PaintingYPositionEnd;
    public float PaintingYScaleStart;
    public float PaintingYScaleEnd;
    public Ease IntroEaseType;
    private Vector3 paintingDefaultScale;
    private Vector3 paintingDefaultPosition;

    [Header("INTRO ANIMATION - CELLS")]
    public float IntroCellAnimationDuration = 1.0f;
    public float IntroCellBumpAnimationDuration = 1.0f;
    
    [Header("OUTRO ANIMATION - PAINTING")]
    public float OutroAnimationDuration = 1.0f;
    public float YPositionShowPainting;

    [Header("OVERVIEW ANIMATION")]
    public OverviewAnimationType OverviewType;
    public Color OverviewWaveColor = Color.white;
    public Vector3 OverviewWaveScale = Vector3.one;
    public float CellBumpAnimationDuration = 1.0f;
    public float OverviewAnimationDuration = 1.0f;

    private Coroutine currentAnimationCoroutine;
    private readonly Dictionary<int, List<PaintingFishScaleSpriteCell>> cachedRippleLayers = new Dictionary<int, List<PaintingFishScaleSpriteCell>>();
    private readonly Dictionary<int, List<PaintingFishScaleSpriteCell>> cachedRowCells = new Dictionary<int, List<PaintingFishScaleSpriteCell>>();
    private readonly Dictionary<int, List<PaintingFishScaleSpriteCell>> cachedColumnCells = new Dictionary<int, List<PaintingFishScaleSpriteCell>>();
    private readonly Dictionary<(int, bool), List<PaintingFishScaleSpriteCell>> cachedDiagonalCells = new Dictionary<(int, bool), List<PaintingFishScaleSpriteCell>>();

    private int centerRow;
    private int centerCol;
    private bool cacheInitialized = false;

    public enum OverviewAnimationType
    {
        Random,
        RippleDiamond,
        RippleSquare,
        VerticalSweapUp,
        VerticalSweapDown,
        HorizontalSweapLeft,
        HorizontalSweapRight,
        DiagonalSweepLeft,
        DiagonalSweepRight,
    }
    #endregion

    #region UNITY CORE
    private void Awake()
    {
        paintingDefaultScale = PaintingTransform.localScale;
        paintingDefaultPosition = PaintingTransform.localPosition;
    }
    #endregion

    #region MAIN

    #region _animate painting intro
    [ContextMenu("PREVIEW: INTRO")]
    public void StartIntroAnimation()
    {
        SetUpPreIntro();

        Vector3 endScale = paintingDefaultScale;
        endScale.y = PaintingYScaleEnd;

        Vector3 endPosition = paintingDefaultPosition;
        endPosition.y = PaintingYPositionEnd;

        Sequence paintingIntroSequence = DOTween.Sequence();
        paintingIntroSequence.Append(PaintingTransform.DOLocalMoveY(PaintingYPositionEnd, IntroAnimationDuration).SetEase(IntroEaseType));
        paintingIntroSequence.Join(PaintingTransform.DOScaleY(PaintingYScaleEnd, IntroAnimationDuration * 1.25f).SetEase(IntroEaseType));
        paintingIntroSequence.OnComplete(() =>
        {
            StartOverviewAnimation(isIntro: true);
            PaintingTransform.localScale = endScale;
            PaintingTransform.localPosition = endPosition;
        });
    }

    public void StopIntroAnimation()
    {
        Vector3 endScale = paintingDefaultScale;
        endScale.y = PaintingYScaleEnd;

        Vector3 endPosition = paintingDefaultPosition;
        endPosition.y = PaintingYPositionEnd;

        DOTween.Kill(PaintingTransform);
        PaintingTransform.localScale = endScale;
        PaintingTransform.localPosition = endPosition;
    }

    public void SetUpPreIntro()
    {
        DOTween.Kill(PaintingTransform);

        Vector3 startScale = paintingDefaultScale;
        startScale.y = PaintingYScaleStart;

        Vector3 startPosition = paintingDefaultPosition;
        startPosition.y = PaintingYPositionStart;

        PaintingTransform.localScale = startScale;
        PaintingTransform.localPosition = startPosition;
    }
    #endregion

    #region _animate painting outro
    [ContextMenu("PREVIEW: OUTTRO")]
    public void StartOutroAnimation()
    {
        //if (!PaintingObject.PlayOutroAnimation) return;
        SetUpPreOutro();

        Vector3 endPosition = paintingDefaultPosition;
        endPosition.y = YPositionShowPainting;

        Sequence paintingIntroSequence = DOTween.Sequence();
        paintingIntroSequence.Append(PaintingTransform.DOLocalMoveY(YPositionShowPainting, OutroAnimationDuration).SetEase(IntroEaseType));
        paintingIntroSequence.OnComplete(() =>
        {
            StartOverviewAnimation(isIntro: false);
            PaintingTransform.localPosition = endPosition;
        });
    }

    public void StopOutroAnimation()
    {
        Vector3 endPosition = paintingDefaultPosition;
        endPosition.y = YPositionShowPainting;

        DOTween.Kill(PaintingTransform);
        PaintingTransform.localPosition = endPosition;
    }

    public void SetUpPreOutro()
    {
        DOTween.Kill(PaintingTransform);
        Vector3 startPosition = paintingDefaultPosition;

        PaintingTransform.localPosition = startPosition;
    }
    #endregion

    #region _animate painting overview
    [ContextMenu("PREVIEW: OVERVIEW")]
    public void StartOverviewAnimation(bool clearBeforeOverview = false, bool isIntro = false)
    {
        StopOverviewAnimation();
        if (clearBeforeOverview) PaintingObject.ClearPainting();

        OverviewAnimationType type = OverviewType;
        if (type == OverviewAnimationType.Random)
            type = GetRandomEnumExcludingFirst<OverviewAnimationType>();

        switch (type)
        {
            case OverviewAnimationType.RippleDiamond:
                currentAnimationCoroutine = StartCoroutine(AnimatePaintingRippleDiamond(isIntro)); break;
            case OverviewAnimationType.RippleSquare:
                currentAnimationCoroutine = StartCoroutine(AnimatePaintingRippleSquare(isIntro)); break;
            case OverviewAnimationType.HorizontalSweapLeft:
                currentAnimationCoroutine = StartCoroutine(AnimatePaintingHorizontalSweap(true, isIntro)); break;
            case OverviewAnimationType.HorizontalSweapRight:
                currentAnimationCoroutine = StartCoroutine(AnimatePaintingHorizontalSweap(false, isIntro)); break;
            case OverviewAnimationType.VerticalSweapUp:
                currentAnimationCoroutine = StartCoroutine(AnimatePaintingVerticalSweap(true, isIntro)); break;
            case OverviewAnimationType.VerticalSweapDown:
                currentAnimationCoroutine = StartCoroutine(AnimatePaintingVerticalSweap(false, isIntro)); break;
            case OverviewAnimationType.DiagonalSweepLeft:
                currentAnimationCoroutine = StartCoroutine(AnimatePaintingDiagonalSweep(true, isIntro)); break;
            case OverviewAnimationType.DiagonalSweepRight:
                currentAnimationCoroutine = StartCoroutine(AnimatePaintingDiagonalSweep(false, isIntro)); break;
        }
    }

    private IEnumerator SpiralCoroutine(bool inward, bool forceEnable = false)
    {
        float animDuration = forceEnable ? IntroCellAnimationDuration : OverviewAnimationDuration;
        float cellDuration = forceEnable ? IntroCellBumpAnimationDuration : CellBumpAnimationDuration;
        List<PaintingFishScaleSpriteCell> spiralCells = GetCellsInSpiralOrder(inward);
        
        for (int i = 0; i < spiralCells.Count; i += 10)
        {
            for (int j = 0; j < 10 && i + j < spiralCells.Count; j++)
            {
                spiralCells[i + j].CreatePumpTween(OverviewWaveScale, OverviewWaveColor, cellDuration, forceEnable);
            }

            yield return new WaitForSeconds(OverviewAnimationDuration);
        }
    }

    public IEnumerator AnimatePaintingRippleDiamond(bool forceEnable = false)
    {
        int centerRow = PaintingGridSizeX / 2;
        int centerCol = PaintingGridSizeY / 2;

        float animDuration = forceEnable ? IntroCellAnimationDuration : OverviewAnimationDuration;
        float cellDuration = forceEnable ? IntroCellBumpAnimationDuration : CellBumpAnimationDuration;

        var rippleLayers = new Dictionary<int, List<PaintingFishScaleSpriteCell>>();
        foreach (var cell in PaintingObject.PaintingCellsInUse)
        {
            int distance = Mathf.Abs(cell.Row - centerRow) + Mathf.Abs(cell.Column - centerCol);
            if (!rippleLayers.ContainsKey(distance))
                rippleLayers[distance] = new List<PaintingFishScaleSpriteCell>();
            rippleLayers[distance].Add(cell);
        }

        foreach (var layer in rippleLayers.OrderBy(kv => kv.Key))
        {
            Sequence wave = DOTween.Sequence();
            foreach (var cell in layer.Value) 
                wave.Join(cell.CreatePumpTween(OverviewWaveScale, OverviewWaveColor, cellDuration, forceEnable));
            
            yield return new WaitForSeconds(animDuration);
        }
    }

    public IEnumerator AnimatePaintingRippleSquare(bool forceEnable = false)
    {
        int centerRow = PaintingGridSizeX / 2;
        int centerCol = PaintingGridSizeY / 2;
        int maxDistance = Mathf.Max(
            Mathf.Max(centerCol, PaintingGridSizeX - 1 - centerCol),
            Mathf.Max(centerRow, PaintingGridSizeY - 1 - centerRow)
        );

        float animDuration = forceEnable ? IntroCellAnimationDuration : OverviewAnimationDuration;
        float cellDuration = forceEnable ? IntroCellBumpAnimationDuration : CellBumpAnimationDuration;

        for (int distance = 0; distance <= maxDistance; distance++)
        {
            Sequence wave = DOTween.Sequence();
            List<PaintingFishScaleSpriteCell> rippleCells = GetCellsAtDistance(centerCol, centerRow, distance);
            foreach (var cell in rippleCells) 
                wave.Join(cell.CreatePumpTween(OverviewWaveScale, OverviewWaveColor, cellDuration, forceEnable));

            yield return new WaitForSeconds(animDuration);
        }
    }

    public IEnumerator AnimatePaintingVerticalSweap(bool topToBottom, bool forceEnable = false)
    {
        float animDuration = forceEnable ? IntroCellAnimationDuration : OverviewAnimationDuration;
        float cellDuration = forceEnable ? IntroCellBumpAnimationDuration : CellBumpAnimationDuration;
        
        for (int row = 0; row < PaintingGridSizeY; row++)
        {
            int currentRow = topToBottom ? row : PaintingGridSizeY - 1 - row;
            var rowCells = GetCellsAtRow(currentRow);
            Sequence wave = DOTween.Sequence();
            foreach (var cell in rowCells) 
                wave.Join(cell.CreatePumpTween(OverviewWaveScale, OverviewWaveColor, cellDuration, forceEnable));

            yield return new WaitForSeconds(animDuration);
        }
    }

    public IEnumerator AnimatePaintingHorizontalSweap(bool leftToRight, bool forceEnable = false)
    {
        float animDuration = forceEnable ? IntroCellAnimationDuration : OverviewAnimationDuration;
        float cellDuration = forceEnable ? IntroCellBumpAnimationDuration : CellBumpAnimationDuration;
        
        for (int col = 0; col < PaintingGridSizeX; col++)
        {
            int currentCol = leftToRight ? col : PaintingGridSizeX - 1 - col;
            var columnCells = GetCellsAtColumn(currentCol);
            Sequence wave = DOTween.Sequence();
            foreach (var cell in columnCells) 
                wave.Join(cell.CreatePumpTween(OverviewWaveScale, OverviewWaveColor, cellDuration, forceEnable));

            yield return new WaitForSeconds(animDuration);
        }
    }

    public IEnumerator AnimatePaintingDiagonalSweep(bool topLeftToBottomRight, bool forceEnable = false)
    {
        int maxDiagonal = PaintingGridSizeX + PaintingGridSizeY - 1;
        float animDuration = forceEnable ? IntroCellAnimationDuration : OverviewAnimationDuration;
        float cellDuration = forceEnable ? IntroCellBumpAnimationDuration : CellBumpAnimationDuration;
        
        for (int d = 0; d < maxDiagonal; d++)
        {
            var diagonalCells = GetCellsInDiagonal(d, topLeftToBottomRight);
            Sequence wave = DOTween.Sequence();
            foreach (var cell in diagonalCells) 
                wave.Join(cell.CreatePumpTween(OverviewWaveScale, OverviewWaveColor, cellDuration, forceEnable));

            yield return new WaitForSeconds(animDuration);
        }
    }

    private IEnumerator SparkleCoroutine(int sparkleCount, float duration, bool forceEnable = false)
    {
        float endTime = Time.time + duration;
        float animDuration = forceEnable ? IntroCellAnimationDuration : OverviewAnimationDuration;
        float cellDuration = forceEnable ? IntroCellBumpAnimationDuration : CellBumpAnimationDuration;
        
        while (Time.time < endTime)
        {
            for (int i = 0; i < sparkleCount; i++)
            {
                if (PaintingObject.PaintingCellsInUse.Count > 0)
                {
                    var randomCell = PaintingObject.PaintingCellsInUse[UnityEngine.Random.Range(0, PaintingObject.PaintingCellsInUse.Count)];
                    randomCell.CreatePumpTween(OverviewWaveScale, OverviewWaveColor, cellDuration, forceEnable);
                }
            }

            yield return new WaitForSeconds(animDuration);
        }
    }
    #endregion

    #endregion

    #region SUPPORTIVE
    public void StopOverviewAnimation()
    {
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
            ResetPumpAnimationInCells();
        }
    }

    private void ResetPumpAnimationInCells()
    {
        foreach (var cell in PaintingObject.PaintingCellsInUse) cell.ResetScale();
    }

    private List<PaintingFishScaleSpriteCell> GetCellsAtRow(int row)
    {
        return PaintingObject.AllCellsMap.Values.Where(cell => cell.Row == row).ToList();
    }

    private List<PaintingFishScaleSpriteCell> GetCellsAtColumn(int col)
    {
        return PaintingObject.AllCellsMap.Values.Where(cell => cell.Column == col).ToList();
    }

    private List<PaintingFishScaleSpriteCell> GetCellsInDiagonal(int diagonalIndex, bool topLeftToBottomRight)
    {
        List<PaintingFishScaleSpriteCell> cells = new List<PaintingFishScaleSpriteCell>();

        if (topLeftToBottomRight)
        {
            for (int col = 0; col < PaintingGridSizeX; col++)
            {
                int row = diagonalIndex - col;
                if (row >= 0 && row < PaintingGridSizeY)
                {
                    var cell = PaintingObject.AllCellsMap.Values.FirstOrDefault(c => c.Column == col && c.Row == row);
                    if (cell != null) cells.Add(cell);
                }
            }
        }
        else
        {
            for (int col = 0; col < PaintingGridSizeX; col++)
            {
                int row = col - diagonalIndex + PaintingGridSizeY - 1;
                if (row >= 0 && row < PaintingGridSizeY)
                {
                    var cell = PaintingObject.AllCellsMap.Values.FirstOrDefault(c => c.Column == col && c.Row == row);
                    if (cell != null) cells.Add(cell);
                }
            }
        }

        return cells;
    }

    private List<PaintingFishScaleSpriteCell> GetCellsAtDistance(int centerCol, int centerRow, int distance)
    {
        List<PaintingFishScaleSpriteCell> cells = new List<PaintingFishScaleSpriteCell>();

        foreach (var cell in PaintingObject.AllCellsMap)
        {
            int cellDistance = Mathf.Max(
                Mathf.Abs(cell.Value.Column - centerCol),
                Mathf.Abs(cell.Value.Row - centerRow)
            );

            if (cellDistance == distance) cells.Add(cell.Value);
        }

        return cells;
    }

    private List<PaintingFishScaleSpriteCell> GetCellsInSpiralOrder(bool inward)
    {
        List<PaintingFishScaleSpriteCell> spiralCells = new List<PaintingFishScaleSpriteCell>();

        int left = 0, right = PaintingGridSizeX - 1;
        int top = 0, bottom = PaintingGridSizeY - 1;

        while (left <= right && top <= bottom)
        {
            for (int col = left; col <= right; col++)
            {
                var cell = PaintingObject.AllCellsMap.Values.FirstOrDefault(c => c.Column == col && c.Row == top);
                if (cell != null) spiralCells.Add(cell);
            }
            top++;

            for (int row = top; row <= bottom; row++)
            {
                var cell = PaintingObject.AllCellsMap.Values.FirstOrDefault(c => c.Column == right && c.Row == row);
                if (cell != null) spiralCells.Add(cell);
            }
            right--;

            if (top <= bottom)
            {
                for (int col = right; col >= left; col--)
                {
                    var cell = PaintingObject.AllCellsMap.Values.FirstOrDefault(c => c.Column == col && c.Row == bottom);
                    if (cell != null) spiralCells.Add(cell);
                }
                bottom--;
            }

            if (left <= right)
            {
                for (int row = bottom; row >= top; row--)
                {
                    var cell = PaintingObject.AllCellsMap.Values.FirstOrDefault(c => c.Column == left && c.Row == row);
                    if (cell != null) spiralCells.Add(cell);
                }
                left++;
            }
        }

        if (!inward)
        {
            spiralCells.Reverse();
        }

        return spiralCells;
    }

    public static T GetRandomEnumExcludingFirst<T>() where T : System.Enum
    {
        var values = System.Enum.GetValues(typeof(T));
        int count = values.Length;

        int index = UnityEngine.Random.Range(1, count);
        return (T)values.GetValue(index);
    }
    #endregion
}