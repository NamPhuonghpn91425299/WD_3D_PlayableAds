using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TargetBarAnimator : MonoBehaviour
{
    #region PROPERTIES
    public WoolAnimationData AnimationData;
    public Transform[] SpiralRingItems;

    [Header("BAR IN/OUTTRO: OBJECT(s)")]
    public BarPivotSide Side = BarPivotSide.Right;
    public Transform BarTransform;

    [Header("BAR IN/OUTTRO: STAT(s)")]
    public Vector3 BarOriginalScale;
    public Vector3 BarOutroScale;
    public float IntroDuration;
    public float OutroDuration;
    public Ease IntroEaseType;
    public Ease OutroEaseType;
    private Vector3 originalLocalPos;

    [Header("LINE RENDERER")]
    public List<PaintingLineRendererHandler> WoolLineRenderers = new List<PaintingLineRendererHandler>();
    public Transform LeftPivot;
    public Transform RightPivot;
    private Vector3 rightPivotPosition = Vector3.zero;
    private Vector3 leftPivotPosition = Vector3.zero;

    [Header("SPIRAL ANIMATION")]
    public int NumberOfSpinningTimes = 3;
    public float RollInDuration = 1f;
    public float RollOutDuration = 1f;
    public Vector3 UnRolledScale = Vector3.one;
    public Vector3 RolledScale = Vector3.one;
    public Vector3 HiddenScale = Vector3.one;
    public float SpinningDuration = 1f;
    public Vector3 OriginalSpinRotation;
    public Vector3 TargetSpinRotation;
    public Vector3 OutSpinRotation;

    private Sequence introSequence;
    private Sequence outroSequence;
    private List<Sequence> rollingSequences = new List<Sequence>();

    // Performance optimizations - cached values
    private readonly Dictionary<int, Transform> spiralCache = new Dictionary<int, Transform>();
    private readonly Queue<PaintingLineRendererHandler> linePool = new Queue<PaintingLineRendererHandler>();

    public enum BarPivotSide { Right, Left }
    #endregion

    #region UNITY CORE
    private void Awake()
    {
        originalLocalPos = BarTransform.localPosition;
        rightPivotPosition = RightPivot.localPosition;
        leftPivotPosition = LeftPivot.localPosition;

        RegisterAllEvents();
        InitializeCaches();
        CreateTweens();
    }

    private void OnEnable()
    {
       // StartIntroAsync().Forget();
    }

    private void OnDestroy()
    {
        UnregisterAllEvents();
        CleanupTweens();
    }
    #endregion

    #region INITIALIZATION & CLEANUP
    private void InitializeCaches()
    {
        for (int i = 0; i < SpiralRingItems.Length; i++)
        {
            spiralCache[i] = SpiralRingItems[i];
        }

        foreach (var line in WoolLineRenderers)
        {
            if (line.Available()) linePool.Enqueue(line);
        }
    }

    private void CleanupTweens()
    {
        introSequence?.Kill();
        outroSequence?.Kill();
        foreach (var seq in rollingSequences) seq?.Kill();
        rollingSequences.Clear();
    }
    #endregion

    #region MAIN

    #region _events
    private void RegisterAllEvents()
    {
        //GameEventManager.OnIntroComplete += OnIntroDone;
    }

    private void UnregisterAllEvents()
    {
        //GameEventManager.OnIntroComplete -= OnIntroDone;
    }

    private void OnIntroDone(bool done)
    {
        if (!done) return;
        if (!gameObject.activeSelf) gameObject.SetActive(true);
    }
    #endregion

    public void StartScrollInSpiralItem(int index)
    {
        if (index > (SpiralRingItems.Length / 3)) return;
        ScrollInPartByPart(index);
    }

    public void StartScrollOutSpiralItem(int index)
    {
        if (index > (SpiralRingItems.Length - 1)) return;

        if (spiralCache.TryGetValue(index, out Transform targetSpiral))
        {
            ResetSpiralItem(index);
            targetSpiral.gameObject.SetActive(true);
            targetSpiral.DOScale(UnRolledScale, RollOutDuration);
            StartCoroutine(SpinningSpiralItems(targetSpiral, RollInDuration, clockwise: false));
        }
    }

    public void ResetSpiralItem(int index)
    {
        if (index > (SpiralRingItems.Length - 1)) return;

        if (spiralCache.TryGetValue(index, out Transform targetSpiral))
        {
            targetSpiral.localScale = UnRolledScale;
            DOTween.Kill(targetSpiral);
        }
    }

    public void ResetAllSpiralItems()
    {
        foreach (var spiralItem in SpiralRingItems)
        {
            spiralItem.localScale = UnRolledScale;
            DOTween.Kill(spiralItem);
        }
    }

    public void ConnectWoolLine(Vector3 origin, Vector3 target, Color color, bool firstCell = false)
    {
        if (linePool.Count == 0) return;

        var availableLine = linePool.Dequeue();
        StartCoroutine(LiningCoroutine(availableLine, origin, target, color, firstCell));
    }

    private IEnumerator LiningCoroutine(PaintingLineRendererHandler lineRendererHandler, Vector3 origin, Vector3 target, Color color, bool firstCell = false)
    {
        lineRendererHandler.ConnectLine(origin, target, color, firstCell);
        yield return new WaitForSeconds(RollInDuration);
        lineRendererHandler.ClearLineLinearFollowUp();

        linePool.Enqueue(lineRendererHandler);
    }

    private void CreateTweens()
    {
        introSequence = DOTween.Sequence()
            .Append(BarTransform.DOLocalMoveX(originalLocalPos.x, IntroDuration).SetEase(IntroEaseType))
            .Join(BarTransform.DOScaleZ(BarOriginalScale.z, IntroDuration * 1.2f).SetEase(IntroEaseType))
            .Pause()
            .SetAutoKill(false)
            .OnComplete(() => {
                introSequence.Pause();
                BarTransform.localPosition = originalLocalPos;
            });

        Vector3 backPos = originalLocalPos + new Vector3(0, 0, 1);
        Vector3 sidePos = backPos + new Vector3(Side == BarPivotSide.Left ? -10 : 10, 0, 0);
        outroSequence = DOTween.Sequence()
            .Append(BarTransform.DOLocalMove(sidePos, OutroDuration).SetEase(OutroEaseType))
            .Join(BarTransform.DOScaleZ(BarOutroScale.z, OutroDuration / 2f).SetEase(OutroEaseType))
            .Pause()
            .SetAutoKill(false)
            .OnComplete(() => outroSequence.Pause());
    }
    

    // public async UniTaskVoid StartIntroAsync()
    // {
    //     Vector3 sidePos = originalLocalPos + new Vector3(Side == BarPivotSide.Left ? -10 : 10, 0, 1);
    //     BarTransform.localPosition = sidePos;
    //     BarTransform.localScale = BarOutroScale;
    //
    //     await UniTask.DelayFrame(5);
    //
    //     introSequence.Pause();
    //     introSequence.Restart();
    // }

    [ContextMenu("ANIM: INTRO")]
    public void StartIntro()
    {
        Vector3 sidePos = originalLocalPos + new Vector3(Side == BarPivotSide.Left ? -10 : 10, 0, 1);
        BarTransform.localPosition = sidePos;
        BarTransform.localScale = BarOutroScale;

        introSequence.Pause();
        introSequence.Restart();
    }

    [ContextMenu("ANIM: OUTRO")]
    public void StartOutro()
    {
        Vector3 backPos = originalLocalPos + new Vector3(0, 0, 1);
        BarTransform.localPosition = backPos;
        BarTransform.localScale = BarOriginalScale;

        outroSequence.Pause();
        outroSequence.Restart();
    }

    private IEnumerator SpinningSpiralItems(Transform item, float duration, bool clockwise = true)
    {
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            item.Rotate(Vector3.forward, (clockwise ? SpinningDuration : -SpinningDuration) * Time.deltaTime);
            yield return null;
        }
    }

    public void RollOutAllSpiralItems(float duration)
    {
        foreach (var sequence in rollingSequences) sequence?.Kill();
        rollingSequences.Clear();
        RollOutFull(duration);
    }

    public void ScrollInPartByPart(int partIndex)
    {
        Sequence spiralTotalSequence = DOTween.Sequence();
        rollingSequences.Add(spiralTotalSequence);
        int startIndex = partIndex * 3;
        int endIndex = startIndex + 3;

        for (int i = startIndex; i < endIndex && i < SpiralRingItems.Length; i++)
        {
            if (!spiralCache.TryGetValue(i, out Transform ring)) continue;

            DOTween.Kill(ring);
            ring.gameObject.SetActive(true);
            ring.localScale = UnRolledScale;
            ring.localEulerAngles = OriginalSpinRotation;

            Sequence ringSequence = DOTween.Sequence()
                .Append(ring.DOScale(RolledScale, GetRollInDuration()).SetEase(Ease.OutBack))
                .Join(ring.DORotate(TargetSpinRotation, GetRollInDuration(), RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine));

            spiralTotalSequence.Append(ringSequence);
        }
    }

    public void RollOutFull(float duration)
    {
        foreach (Sequence sq in rollingSequences) sq?.Kill();
        rollingSequences.Clear();

        Sequence spiralTotalSequence = DOTween.Sequence();
        rollingSequences.Add(spiralTotalSequence);

        float eachRingDuration = duration / SpiralRingItems.Length;

        for (int i = SpiralRingItems.Length - 1; i >= 0; i--)
        {
            if (!spiralCache.TryGetValue(i, out Transform ring)) continue;

            DOTween.Kill(ring);
            ring.gameObject.SetActive(true);

            Sequence ringSequence = DOTween.Sequence()
                .Append(ring.DORotate(OutSpinRotation, eachRingDuration, RotateMode.LocalAxisAdd).SetEase(Ease.InOutSine))
                .OnComplete(() => ring.DOScale(HiddenScale, eachRingDuration * 1.1f).SetEase(Ease.OutBack));

            spiralTotalSequence.Append(ringSequence);
        }
    }
    #endregion

    #region SUPPORTIVE
    private float GetRollInDuration()
    {
       return AnimationData?.RollInDuration ?? RollInDuration;
    }

    public void ResetAnimator()
    {
        foreach (var line in WoolLineRenderers) line.ClearLine();
        foreach (Sequence sq in rollingSequences) sq?.Kill();
        rollingSequences.Clear();
        ResetWoolRingsDefault();

        InitializeCaches();
    }

    public void ResetWoolRingsDefault()
    {
        foreach (var ring in SpiralRingItems)
        {
            DOTween.Kill(ring);
            ring.gameObject.SetActive(false);
        }
        ResetPivotPosition();
    }

    public void ResetPivotPosition()
    {
        RightPivot.localPosition = rightPivotPosition;
        LeftPivot.localPosition = leftPivotPosition;
    }
    #endregion
}