using static PaintingSharedAttributes;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System;

public class FishScalesPaintingController : Singleton<FishScalesPaintingController>
{
    #region PROPERTIES

    [Header("CONTROLLER(s)")]
    //[SerializeField] private LevelConfigSO levelConfigSO;
    public WoolAnimationData AnimationData;

    public PaintingLineRendererHandler LineConnectWoolHandler;
    public PaintingPumpAnimationManager PumpAnimationManager;
    public PaintingOverviewAnimation OverviewAnimationManager;

    [Header("ANIMATION OPTION")] public bool PlayIntroAnimation = false;
    public bool PlayOutroAnimation = false;

    [Header("SOUND FX")] public AudioSource PaintingAudioSource;
    public List<AudioClip> PaintingCellClips;
    public float SoundFxPlayRate = 0.1f;

    [Header("INPUT - COLOR PALETTE")] public bool IgnoreColorPalette = false;
    public ColorPalleteData ColorPalette;
    private Dictionary<string, Color> ColorCodeInUse = new Dictionary<string, Color>();

    [Header("INPUT - SETUP LEVEL")] public GamePlayMeshController CurrentLevelPrefab;
    public Texture2D TargetPaintingTexture;

    public PaintingConfig CurrentPaintingConfig;

    [Header("DEBUG")] public List<string> ColorKeyUsedInThisSample = new List<string>();
    public List<PaintingPartBasedOnColor> CurrentLevelPaintingColorParts = new List<PaintingPartBasedOnColor>();
    private List<PaintingCell> paintingCells = new List<PaintingCell>();
    public List<PaintingPartBasedOnColor> CurrentLevelPaintingSeparateParts = new List<PaintingPartBasedOnColor>();

    [Header("WOOL DRAWING TEST")] public CubeTargetControl CurrentBox;
    public List<Color> CurrentColorInUse = new List<Color>();
    private CancellationTokenSource paintingCTS;

    [Header("DEFAULT VALUES")] public Transform PaintingCellContainer;
    public List<PaintingFishScaleSpriteCell> PaintingCellsInUse = new List<PaintingFishScaleSpriteCell>();

    public Dictionary<Vector2Int, PaintingFishScaleSpriteCell> AllCellsMap =
        new Dictionary<Vector2Int, PaintingFishScaleSpriteCell>();

    public PaintingConfig TemporaryPaintingConfig;
    public PaintingConfig DefaultPaintingConfig;

    #endregion

    #region UNITY CORE

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (PaintingCellContainer)
        {
            PaintingCellsInUse.Clear();
            foreach (Transform child in PaintingCellContainer)
            {
                if (child.TryGetComponent<PaintingFishScaleSpriteCell>(out PaintingFishScaleSpriteCell cell))
                {
                    PaintingCellsInUse.Add(cell);
                }
            }
        }
    }
#endif
    public override void Awake()
    {
        base.Awake();
        InitCellMap();
    }

    private void Start()
    {
        OverviewAnimationManager.StartIntroAnimation();
        LoadFromCurrentPaintingConfig(alsoShow: false);
        SampleColorsFromInputPainting();
    }

    #endregion

    #region MAIN

    #region events

    private void Initialize()
    {
        ClearPainting();
    }

    public void OnLevelLoadDone(LevelData level)
    {
        // paintingCTS?.Cancel();
        // paintingCTS = new CancellationTokenSource();
        // CurrentLevelPrefab = level;
        // string levelPrefabPath = "Levels/" + CurrentLevelPrefab.name.Replace("(Clone)", "");
        // LevelConfigData targetLevel = levelConfigSO.GetLevelConfigData(levelPrefabPath);
        // var levelPaintingConfig = targetLevel.LevelPaintingConfig;
        // if (targetLevel != null && (levelPaintingConfig != null && levelPaintingConfig != TemporaryPaintingConfig &&
        //                             levelPaintingConfig != DefaultPaintingConfig))
        // {
        //     CurrentPaintingConfig = targetLevel.LevelPaintingConfig;
        // }
        // else
        // {
        //     CurrentPaintingConfig = TemporaryPaintingConfig;
        //     SampleColorsFromInputPainting();
        //     SaveToCurrentPaintingConfig();
        // }

        LoadFromCurrentPaintingConfig(alsoShow: false);
    }

    #endregion

    #region _wool painting

    Coroutine lineConnectingCoroutine;

    public void LoadFullPainting()
    {
        if (CurrentPaintingConfig != null)
        {
            for (int i = 0; i < CurrentPaintingConfig.AllCellsAvailable.Count; i++)
            {
                var cell = CurrentPaintingConfig.AllCellsAvailable[i];
                var spriteCell = GetCellAt(cell.Row, cell.Column);
                if (spriteCell == null) continue;
                spriteCell.Apply();
            }
        }
    }

    private void ApplyFullPaintingColor()
    {
        foreach (var cell in PaintingCellsInUse) cell.Apply();
    }

    public void ClearPainting()
    {
        foreach (var cell in PaintingCellsInUse) cell.Clear();
    }

    #region part to part painting

    public void TestLinePainting()
    {
        if (CurrentBox) ClearPainting();

        if (lineConnectingCoroutine != null)
        {
            StopCoroutine(lineConnectingCoroutine);
        }

        lineConnectingCoroutine = StartCoroutine(LineConnectTest(CurrentBox.transform));
    }

    private IEnumerator LineConnectTest(Transform currentCube)
    {
        if (CurrentLevelPaintingColorParts.Count <= 0)
        {
            foreach (var cell in paintingCells)
            {
                if (currentCube != null)
                {
                    var spriteCell = GetCellAt(cell.Row, cell.Column);
                    if (spriteCell == null) continue;
                    spriteCell.Apply();
                    PumpAnimationManager.SpawnEffectAt(GetCellWorldPosition(spriteCell), cell.CellColor);
                    LineConnectWoolHandler.ConnectLine(currentCube.position, GetCellWorldPosition(spriteCell),
                        cell.CellColor);
                    yield return new WaitForSeconds(0.02f);
                }
            }
        }
        else if (CurrentPaintingConfig != null)
        {
            var shuffledParts = CurrentLevelPaintingSeparateParts.OrderBy(x => Random.value).ToList();
            Vector3 lineStartPosition = CurrentBox != null ? currentCube.position : Vector3.zero;
            foreach (var part in shuffledParts)
            {
                for (int i = 0; i < part.PaintingCells.Count; i++)
                {
                    var cell = part.PaintingCells[i];
                    var spriteCell = GetCellAt(cell.Row, cell.Column);
                    if (spriteCell == null) continue;
                    spriteCell.Apply();
                    PumpAnimationManager.SpawnEffectAt(GetCellWorldPosition(spriteCell), cell.CellColor);
                    LineConnectWoolHandler.ConnectLine(lineStartPosition, GetCellWorldPosition(spriteCell),
                        cell.CellColor, firstCell: i == 0);
                    yield return new WaitForSeconds(0.02f);
                }

                yield return 0.25f;
            }
        }
        else
        {
            foreach (var category in CurrentLevelPaintingColorParts)
            {
                foreach (var cell in category.PaintingCells)
                {
                    if (CurrentBox != null)
                    {
                        var spriteCell = GetCellAt(cell.Row, cell.Column);
                        if (spriteCell == null) continue;
                        spriteCell.Apply();
                        PumpAnimationManager.SpawnEffectAt(GetCellWorldPosition(spriteCell), cell.CellColor);
                        LineConnectWoolHandler.ConnectLine(currentCube.position, GetCellWorldPosition(spriteCell),
                            cell.CellColor);
                        yield return new WaitForSeconds(0.02f);
                    }
                }
            }
        }

        LineConnectWoolHandler?.ClearLine();
    }

    // private async UniTaskVoid PartByPartPainting(float duration = 2f)
    // {
    //     if (CurrentPaintingConfig != null)
    //     {
    //         ClearPainting();
    //         var token = paintingCTS.Token;
    //         List<UniTask> paintingTasks = new List<UniTask>();
    //
    //         foreach (var part in CurrentLevelPaintingSeparateParts)
    //         {
    //             paintingTasks.Add(DrawPaintingPart(part, duration, token));
    //         }
    //
    //         await UniTask.WhenAll(paintingTasks);
    //
    //         OverviewAnimationManager.StartOverviewAnimation().Forget();
    //         GameEventManager.OnDrawPaintingDone?.Invoke();
    //     }
    // }
    //
    // private async UniTask DrawPaintingPart(PaintingPartBasedOnColor part, float duration, CancellationToken token)
    // {
    //     float delay = duration / part.PaintingCells.Count;
    //     bool needToPaintEachPart = delay <= 0.017f;
    //     int cellCount = part.PaintingCells.Count;
    //
    //     if (needToPaintEachPart)
    //     {
    //         int cellPerPart = Mathf.Max(2, (int)(0.017f / delay));
    //         for (int i = 0; i < cellCount; i += cellPerPart)
    //         {
    //             token.ThrowIfCancellationRequested();
    //
    //             for (int j = i; j < i + cellPerPart && j < cellCount; j++)
    //             {
    //                 var cell = part.PaintingCells[j];
    //                 var spriteCell = GetCellAt(cell.Row, cell.Column);
    //                 if (spriteCell == null) continue;
    //                 spriteCell.Apply();
    //             }
    //
    //             await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
    //         }
    //     }
    //     else
    //     {
    //         for (int i = 0; i < cellCount; i++)
    //         {
    //             token.ThrowIfCancellationRequested();
    //
    //             var cell = part.PaintingCells[i];
    //             var spriteCell = GetCellAt(cell.Row, cell.Column);
    //             if (spriteCell == null) continue;
    //             spriteCell.Apply();
    //
    //             await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
    //         }
    //     }
    // }

    #endregion

    #region painting as level progress

    public void WoolLinePaintingStart(CubeTargetControl currentCube,int indexThisCube)
    {
        if (currentCube == null || currentCube.WoolLineHandler == null) return;
        StartCoroutine(WoolLinePaintingCoroutine(currentCube,indexThisCube));
    }

    private IEnumerator WoolLinePaintingCoroutine(CubeTargetControl currentCube,int indexThisCube)
    {
        //Debug.Log("WoolLinePaintingCoroutine started");
        float vibrationTimer = Time.time;
        float timerOffset = 0;
        
        if (currentCube == null)
        {
            //Debug.LogError("currentCube is null");
            yield break;
        }

        Vector3 cubePosition = currentCube.transform.position;

        if (CurrentLevelPaintingColorParts == null)
        {
            //Debug.LogError("CurrentLevelPaintingColorParts is null");
            yield break;
        }

        if (CurrentLevelPaintingColorParts.Count <= 0)
        {
            Debug.Log("Branch: 1 paintingCells (CurrentLevelPaintingColorParts.Count <= 0)");

            foreach (var cell in paintingCells)
            {
                //Debug.Log($"Processing paintingCell: Row={cell.Row}, Column={cell.Column}");

                var spriteCell = GetCellAt(cell.Row, cell.Column);
                if (spriteCell == null)
                {
                    //Debug.LogWarning($"spriteCell is null at Row={cell.Row}, Column={cell.Column}");
                    continue;
                }

                spriteCell.Apply();
                PumpAnimationManager.SpawnEffectAt(GetCellWorldPosition(spriteCell), cell.CellColor);
                currentCube.WoolLineHandler.ConnectLine(cubePosition, GetCellWorldPosition(spriteCell), cell.CellColor);

                yield return new WaitForSeconds(0.02f);
            }
        }
        else if (CurrentPaintingConfig != null)
        {
            Debug.Log("Branch: 2 CurrentPaintingConfig != null");

            var shuffledParts = CurrentLevelPaintingSeparateParts.OrderBy(x => Random.value).ToList();
            var targetPart =
                shuffledParts.FirstOrDefault(x => !x.Painted && x.ColorKey.Equals(currentCube.GetPreviousColor()));
            if (targetPart == null)
            {
                //Debug.LogWarning("targetPart is null (no matching color or all painted)");
                yield break;
            }

            targetPart.Painted = true;

            float delay = 1.25f / targetPart.PaintingCells.Count;
            int cellCount = targetPart.PaintingCells.Count;
            AnimationData.GetRollOutAnimationDuration(cellCount, out float paintingDuration, out float durationEachCell);
            currentCube.ThisBarAnimatorController.RollOutAllSpiralItems(paintingDuration - .2f + cellCount * delay);
            for (int i = 0; i < cellCount; i++)
            {
                var cell = targetPart.PaintingCells[i];
                //Debug.Log($"Painting cell {i + 1}/{targetPart.PaintingCells.Count}: Row={cell.Row}, Column={cell.Column}");
                timerOffset = Time.time - vibrationTimer;
                if (timerOffset >= SoundFxPlayRate)
                {
                    vibrationTimer = Time.time;
                    PlayPaintingCellAudioFx();
                }
                var spriteCell = GetCellAt(cell.Row, cell.Column);
                if (spriteCell == null)
                {
                    //Debug.LogWarning($"spriteCell is null at Row={cell.Row}, Column={cell.Column}");
                    continue;
                }

                spriteCell.Apply();
                Vector3 cellPosition = GetCellWorldPosition(spriteCell);
                PumpAnimationManager.SpawnEffectAt(cellPosition, cell.CellColor);
                currentCube.WoolLineHandler.ConnectLine(cubePosition, cellPosition, cell.CellColor, i == 0);
                yield return new WaitForSeconds(delay);
            }
//            Debug.LogError("Đợi xong "+currentCube.gameObject.name+"   "+(paintingDuration + cellCount * delay));
        }
        else
        {
            Debug.Log("Branch: 3 CurrentPaintingConfig == null, using CurrentLevelPaintingColorParts");

            foreach (var category in CurrentLevelPaintingColorParts)
            {
                //Debug.Log($"Processing category with {category.PaintingCells.Count} cells");

                foreach (var cell in category.PaintingCells)
                {
                    var spriteCell = GetCellAt(cell.Row, cell.Column);
                    if (spriteCell == null)
                    {
                        //Debug.LogWarning($"spriteCell is null at Row={cell.Row}, Column={cell.Column}");
                        continue;
                    }

                    spriteCell.Apply();
                    PumpAnimationManager.SpawnEffectAt(GetCellWorldPosition(spriteCell), cell.CellColor);
                    currentCube.WoolLineHandler.ConnectLine(cubePosition, GetCellWorldPosition(spriteCell),
                        cell.CellColor);

                    yield return new WaitForSeconds(0.02f);
                }
            }
        }

        //Debug.Log("Finished painting, clearing line");
        currentCube.WoolLineHandler?.ClearLine();
        currentCube.StartOuttroGenColorCubeTarget(indexThisCube);
    }


    // private async UniTaskVoid WoolLinePaintingStart(BarTargetControl currentbar, CrochetBooster crochet = null)
    // {
    //     if (currentbar == null || currentbar.WoolLineHandler == null) return;
    //
    //     Transform barTransform = currentbar.transform;
    //     Vector3 barPosition = barTransform.position;
    //
    //     Transform leftPivot = currentbar.ThisBarAnimatorController.LeftPivot;
    //     Transform rightPivot = currentbar.ThisBarAnimatorController.RightPivot;
    //     Transform woolTailFollowTarget = crochet == null ? leftPivot : crochet.CrochetHeadPoint;
    //     currentbar.ThisBarAnimatorController.ResetPivotPosition();
    //
    //     if (CurrentPaintingConfig == null) return;
    //
    //     var shuffledParts = CurrentLevelPaintingSeparateParts.OrderBy(x => Random.value).ToList();
    //     Vector3 lineStartPosition = barPosition;
    //
    //     var targetPaintingSeparatePart = shuffledParts.FirstOrDefault(x =>
    //         !x.Painted && x.ColorKey.Equals(currentbar.GetPreviousColor()));
    //     if (targetPaintingSeparatePart == null)
    //     {
    //         GameEventManager.OnStopPaintingAPart?.Invoke(currentbar);
    //         return;
    //     }
    //
    //     targetPaintingSeparatePart.Painted = true;
    //
    //     int cellCount = targetPaintingSeparatePart.PaintingCells.Count;
    //     AnimationData.GetRollOutAnimationDuration(cellCount, out float paintingDuration, out float durationEachCell);
    //
    //     // if (crochet == null)
    //     // {
    //     //     leftPivot.DOLocalMove(rightPivot.localPosition, paintingDuration + 0.4f)
    //     //         .OnUpdate(() => currentbar.WoolLineHandler.TailFollow(leftPivot.position));
    //     // }
    //
    //     GameEventManager.OnStartPaintingAPart?.Invoke(currentbar, paintingDuration);
    //
    //     bool needToPaintEachPart = durationEachCell <= 0.017f;
    //
    //     float vibrationTimer = Time.time;
    //     float timerOffset = 0;
    //
    //     if (needToPaintEachPart)
    //     {
    //         int cellPerPart = (int)(0.017f / durationEachCell);
    //         cellPerPart = Mathf.Max(2, cellPerPart);
    //
    //         for (int i = 0; i < cellCount; i += cellPerPart)
    //         {
    //             for (int j = i; j < i + cellPerPart && j < cellCount; j++)
    //             {
    //                 var cell = targetPaintingSeparatePart.PaintingCells[j];
    //                 var spriteCell = GetCellAt(cell.Row, cell.Column);
    //                 if (spriteCell == null) continue;
    //
    //                 spriteCell.Apply();
    //                 Vector3 cellPosition = GetCellWorldPosition(spriteCell);
    //                 PumpAnimationManager.SpawnEffectAt(cellPosition, cell.CellColor);
    //                 currentbar.WoolLineHandler.ConnectLine(woolTailFollowTarget.position, cellPosition, cell.CellColor, firstCell: i == 0);
    //             }
    //
    //             timerOffset = Time.time - vibrationTimer;
    //             if (timerOffset >= SoundFxPlayRate)
    //             {
    //                 vibrationTimer = Time.time;
    //                 PlayPaintingCellAudioFx();
    //                 DeviceVibrationManager.Instance?.ExecuteVibrationSingle(12);
    //             }
    //
    //             await UniTask.Delay(TimeSpan.FromSeconds(durationEachCell * cellPerPart), DelayType.DeltaTime);
    //         }
    //     }
    //     else
    //     {
    //         for (int i = 0; i < cellCount; i++)
    //         {
    //             var cell = targetPaintingSeparatePart.PaintingCells[i];
    //             var spriteCell = GetCellAt(cell.Row, cell.Column);
    //             if (spriteCell == null) continue;
    //
    //             spriteCell.Apply();
    //             Vector3 cellPosition = GetCellWorldPosition(spriteCell);
    //             PumpAnimationManager.SpawnEffectAt(cellPosition, cell.CellColor);
    //             currentbar.WoolLineHandler.ConnectLine(woolTailFollowTarget.position, cellPosition, cell.CellColor, firstCell: i == 0);
    //
    //             timerOffset = Time.time - vibrationTimer;
    //             if (timerOffset >= SoundFxPlayRate)
    //             {
    //                 vibrationTimer = Time.time;
    //                 PlayPaintingCellAudioFx();
    //                 DeviceVibrationManager.Instance?.ExecuteVibrationSingle(12);
    //             }
    //
    //             await UniTask.Delay(TimeSpan.FromSeconds(durationEachCell), DelayType.DeltaTime);
    //         }
    //     }
    //
    //     DeviceVibrationManager.Instance?.ExecuteVibrationSingle(20);
    //     GameEventManager.OnStopPaintingAPart?.Invoke(currentbar);
    //
    //     currentbar.WoolLineHandler?.ClearLineLinearFollowUp(0.4f);//time delay to clear the line
    // }

    #endregion

    #endregion

    #endregion

    #region SUPPORTIVE

    #region _sample color

    private void InitializeColorToUse()
    {
        if (CurrentLevelPrefab) ColorKeyUsedInThisSample = CurrentLevelPrefab.LevelData.NameColorList;

        if (ColorKeyUsedInThisSample != null && ColorKeyUsedInThisSample.Count > 0)
        {
            int partID = 0;
            CurrentLevelPaintingColorParts.Clear();
            foreach (var color in ColorKeyUsedInThisSample)
            {
                PaintingPartBasedOnColor newPart = new PaintingPartBasedOnColor
                {
                    PartID = partID++,
                    ColorKey = color,
                };
                CurrentLevelPaintingColorParts.Add(newPart);
            }
        }

        ColorCodeInUse.Clear();
        foreach (string colorKey in ColorKeyUsedInThisSample)
        {
            if (ColorPalette.colorPallete.TryGetValue(colorKey, out Color color))
            {
                ColorCodeInUse.Add(colorKey, color);
            }
        }
    }

    public void SampleColorsFromInputPainting()
    {
        SampleColorsBasedOnInputPainting(TargetPaintingTexture);
    }

    private void SampleColorsBasedOnInputPainting(Texture2D sourceImage)
    {
        Initialize();

        paintingCells.Clear();
        CurrentLevelPaintingColorParts.Clear();
        InitializeColorToUse();

        return;
        int width = sourceImage.width;
        int height = sourceImage.height;
        int columns = PaintingGridSizeX;
        int rows = PaintingGridSizeY;

        int blockWidth = width / columns;
        int blockHeight = height / rows;

        int partID = 0;

        CurrentColorInUse.Clear();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int centerX = x * blockWidth + blockWidth / 2;
                int centerY = y * blockHeight + blockHeight / 2;

                centerX = Mathf.Clamp(centerX, 0, width - 1);
                centerY = Mathf.Clamp(centerY, 0, height - 1);

                RectInt curentCell = new RectInt(x * blockWidth, y * blockHeight, blockWidth, blockHeight);
                Color sampledColor = sourceImage.GetPixel(centerX, centerY);

                PaintingCell thisCell = new PaintingCell
                {
                    Row = y,
                    Column = x,
                    CellColor = sampledColor
                };
                thisCell.PixelInitialize();

                string colorKey = "non-defined";

                var curentPixelColor = sampledColor.Round();
                if (!CurrentColorInUse.Contains(curentPixelColor))
                {
                    CurrentColorInUse.Add(curentPixelColor);
                }

                if (!IgnoreColorPalette) sampledColor = GetClosestColor(sampledColor, out colorKey);
                thisCell.CellColor = sampledColor;

                if (true)
                {
                    if (CurrentLevelPaintingColorParts.Any(x => x.ColorKey.Equals(colorKey)))
                    {
                        CurrentLevelPaintingColorParts.First(x => x.ColorKey.Equals(colorKey)).PaintingCells
                            .Add(thisCell);
                    }
                    else
                    {
                        PaintingPartBasedOnColor newCategory = new PaintingPartBasedOnColor
                        {
                            PartID = partID++,
                            ColorKey = colorKey,
                            PaintingCells = new List<PaintingCell>()
                        };
                        newCategory.PaintingCells.Add(thisCell);
                        CurrentLevelPaintingColorParts.Add(newCategory);
                    }
                }

                paintingCells.Add(thisCell);
            }
        }

        foreach (var cell in paintingCells)
        {
            PaintingFishScaleSpriteCell respectlyCell = GetCellAt(cell.Row, cell.Column);
            if (respectlyCell == null) continue;
            respectlyCell.SetDesiredColor(cell.CellColor);
            respectlyCell.Apply();
        }
    }

    #endregion

    #region _painting config

    public void SaveToCurrentPaintingConfig()
    {
        if (CurrentPaintingConfig != null && CurrentLevelPaintingColorParts.Count > 0)
        {
            CurrentPaintingConfig.SetUp(ColorPalette, CurrentLevelPaintingColorParts,
                new List<int>(CurrentLevelPrefab?.LevelData.ColorCountList));
        }
    }

    public void LoadFromCurrentPaintingConfig(bool alsoShow = true)
    {
        ClearPainting();
        if (CurrentPaintingConfig != null)
        {
            CurrentPaintingConfig.ValidatePaintingParts();

            CurrentLevelPaintingColorParts = new List<PaintingPartBasedOnColor>(CurrentPaintingConfig.PaintingParts);
//            print(CurrentPaintingConfig.PaintingSeparateParts.Count);
            CurrentLevelPaintingSeparateParts =
                new List<PaintingPartBasedOnColor>(CurrentPaintingConfig.PaintingSeparateParts);
            foreach (var seperatePart in CurrentLevelPaintingSeparateParts) seperatePart.Painted = false;

            paintingCells.Clear();
            foreach (var part in CurrentLevelPaintingColorParts)
            {
                paintingCells.AddRange(part.PaintingCells);
            }

            if (alsoShow) ApplyFullPaintingColor();

            for (int i = 0; i < CurrentPaintingConfig.AllCellsAvailable.Count; i++)
            {
                var cell = CurrentPaintingConfig.AllCellsAvailable[i];
                var spriteCell = GetCellAt(cell.Row, cell.Column);
                if (spriteCell != null) spriteCell.SetDesiredColor(cell.CellColor);
            }
        }
    }

    #endregion

    private void InitCellMap()
    {
        foreach (var cell in PaintingCellsInUse)
        {
            var key = new Vector2Int(cell.Row, cell.Column);
            if (!AllCellsMap.ContainsKey(key)) AllCellsMap[key] = cell;
        }
    }

    public PaintingFishScaleSpriteCell GetCellAt(int row, int column)
    {
        var key = new Vector2Int(row, column);
        if (AllCellsMap.TryGetValue(key, out var cell)) return cell;
        return null;
    }

    public Color GetClosestColor(Color sampleColor, out string colorkey)
    {
        Color finalColor = sampleColor;
        float minDifference = float.MaxValue;
        colorkey = "";
        foreach (var colorCode in ColorCodeInUse)
        {
            float difference = colorCode.Value.ColorDiffirence(sampleColor);

            if (difference < minDifference)
            {
                minDifference = difference;
                finalColor = colorCode.Value;
                colorkey = colorCode.Key;
            }
        }

        return finalColor;
    }

    public Vector3 GetCellWorldPosition(PaintingFishScaleSpriteCell spriteCell) => spriteCell.WorldPosition;

    private void PlayPaintingCellAudioFx()
    {
        if(PaintingCellClips.Count<=0)
            Debug.LogError("Thiếu âm thanh Hãy kéo âm thanh: Yarn print vào list c# FishScalesPaintingController");
        PaintingAudioSource.PlayOneShot(PaintingCellClips.GetRandom());
    }

    #endregion
}