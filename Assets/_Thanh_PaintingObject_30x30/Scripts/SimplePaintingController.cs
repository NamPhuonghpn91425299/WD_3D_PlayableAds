using static PaintingSharedAttributes;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimplePaintingController : MonoBehaviour
{
    #region PROPERTIES
    [Header("CONFIG")]
    public PaintingConfig CurrentPaintingConfig;
    public ColorPalleteData ColorPalette;
    
    [Header("PAINTING GRID")]
    public Transform PaintingCellContainer;
    public List<PaintingFishScaleSpriteCell> PaintingCellsInUse = new List<PaintingFishScaleSpriteCell>();
    
    [Header("EFFECTS")]
    public PaintingPumpAnimationManager PumpAnimationManager;
    
    [Header("DEBUG")]
    public List<PaintingPartBasedOnColor> CurrentPaintingParts = new List<PaintingPartBasedOnColor>();
     public List<PaintingPartBasedOnColor> CurrentSeparateParts = new List<PaintingPartBasedOnColor>();
    
    [Header("TEST INPUTS")]
    public string DebugColorKey = "Blue3";

    #region Test Painting
    #if UNITY_EDITOR
    [CustomEditor(typeof(SimplePaintingController))]
    public class SimplePaintingControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Clear Painting"))
            {
                (target as SimplePaintingController).ClearAllCells();
            }
            if (GUILayout.Button("Load Config"))
            {
                (target as SimplePaintingController).LoadFromConfig();
            }
            if (GUILayout.Button("Paint Random Part"))
            {
                (target as SimplePaintingController).PaintNextAvailablePart();
            }
            if (GUILayout.Button("Paint All Instantly"))
            {
                (target as SimplePaintingController).PaintAllCells();
            }
            if (GUILayout.Button("Paint All With Animation"))
            {
                (target as SimplePaintingController).StartCoroutine((target as SimplePaintingController).PaintAllCellsAnimated());
            }
            if (GUILayout.Button("Paint Path By Color (DebugColorKey)"))
            {
                (target as SimplePaintingController).PaintSeparatePartPath((target as SimplePaintingController).DebugColorKey);
            }
        }
    }
    
    #endif
    [ContextMenu("Clear Painting")]
    public void ClearPainting() => ClearAllCells();

    [ContextMenu("Load Config")]
    public void LoadConfig() => LoadFromConfig();
    
    [ContextMenu("Paint Random Part")]
    public void PaintRandomPart() => PaintNextAvailablePart();
    
    [ContextMenu("Paint All Instantly")]
    public void PaintAllInstantly() => PaintAllCells();
    
    [ContextMenu("Paint All With Animation")]
    public void PaintAllWithAnimation() => StartCoroutine(PaintAllCellsAnimated());

    [ContextMenu("Paint Path By Color (DebugColorKey)")]
    public void PaintPathByColorButton() => PaintSeparatePartPath(DebugColorKey);

    private Dictionary<Vector2Int, PaintingFishScaleSpriteCell> cellsMap = new Dictionary<Vector2Int, PaintingFishScaleSpriteCell>();
    private int currentPartIndex = 0;
    // Prevent concurrent animations of the same part
    private HashSet<int> partsPaintingInProgress = new HashSet<int>();
    #endregion
    #endregion

    #region UNITY CORE
    private void Awake()
    {
        if(ColorPalette.colorPallete.Count<=0)
            ColorPalette.SetupColor();
        InitializeCellsMap();
    }

    private void Start()
    {
        if (CurrentPaintingConfig != null)
        {
            LoadFromConfig();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Auto-populate cells from container
        if (PaintingCellContainer != null)
        {
            PaintingCellsInUse.Clear();
            foreach (Transform child in PaintingCellContainer)
            {
                if (child.TryGetComponent<PaintingFishScaleSpriteCell>(out var cell))
                {
                    PaintingCellsInUse.Add(cell);
                }
            }
        }
    }
#endif
    #endregion

    #region CORE FUNCTIONS
    private void InitializeCellsMap()
    {
        cellsMap.Clear();
        foreach (var cell in PaintingCellsInUse)
        {
            var key = new Vector2Int(cell.Row, cell.Column);
            if (!cellsMap.ContainsKey(key))
            {
                cellsMap[key] = cell;
            }
        }
//        Debug.Log($"Initialized {cellsMap.Count} cells in map");
    }

    public void LoadFromConfig()
    {
        if (CurrentPaintingConfig == null)
        {
            Debug.LogError("No painting config assigned!");
            return;
        }

        // Clear current state
        ClearAllCells();
        currentPartIndex = 0;

        // Validate config
        CurrentPaintingConfig.ValidatePaintingParts();

        // Load painting parts
        CurrentPaintingParts = new List<PaintingPartBasedOnColor>(CurrentPaintingConfig.PaintingParts);
        CurrentSeparateParts = new List<PaintingPartBasedOnColor>(CurrentPaintingConfig.PaintingSeparateParts);

        // Reset painted status
        foreach (var part in CurrentSeparateParts)
        {
            part.Painted = false;
        }

        // Set desired colors for all cells
        SetupCellColors();

//        Debug.Log($"Loaded config with {CurrentPaintingParts.Count} main parts and {CurrentSeparateParts.Count} separate parts");
    }

    private void SetupCellColors()
    {
        if (CurrentPaintingConfig.AllCellsAvailable != null)
        {
            foreach (var cell in CurrentPaintingConfig.AllCellsAvailable)
            {
                var spriteCell = GetCellAt(cell.Row, cell.Column);
                if (spriteCell != null)
                {
                    spriteCell.SetDesiredColor(cell.CellColor);
                }
            }
        }
    }
    #endregion

    #region PAINTING FUNCTIONS
    public void ClearAllCells()
    {
        foreach (var cell in PaintingCellsInUse)
        {
            cell.Clear();
        }
//        Debug.Log("Cleared all cells");
    }

    public void PaintAllCells()
    {
        foreach (var cell in PaintingCellsInUse)
        {
            cell.Apply();
        }
        Debug.Log("Painted all cells instantly");
    }

    public void PaintNextAvailablePart()
    {
        if (CurrentSeparateParts == null || CurrentSeparateParts.Count == 0)
        {
            Debug.LogWarning("No separate parts available to paint");
            return;
        }

        // Find next unpainted part
        var availablePart = CurrentSeparateParts.FirstOrDefault(part => !part.Painted);
        if (availablePart == null)
        {
            Debug.Log("All parts already painted!");
            return;
        }

        PaintPart(availablePart);
    }

    public void PaintPart(PaintingPartBasedOnColor part)
    {
        if (part == null || part.PaintingCells == null)
        {
            Debug.LogWarning("Invalid part to paint");
            return;
        }

        part.Painted = true;
        int paintedCells = 0;

        foreach (var cell in part.PaintingCells)
        {
            var spriteCell = GetCellAt(cell.Row, cell.Column);
            if (spriteCell != null)
            {
                spriteCell.Apply();
                if (PumpAnimationManager != null)
                {
                    PumpAnimationManager.SpawnEffectAt(spriteCell.transform.position, cell.CellColor);
                }
                paintedCells++;
            }
        }

        Debug.Log($"Painted part {part.PartID} with color {part.ColorKey} - {paintedCells} cells");
    }

    public void PaintPartByColor(string colorKey)
    {
        print("Tô màu "+ colorKey);
        // Default to animated path painting
        PaintPartByColor(colorKey, animated: true, cellDelay: 0.02f);
    }

    // Overload to control animation and delay
    public void PaintPartByColor(string colorKey, bool animated, float cellDelay = 0.02f)
    {
        var part = CurrentSeparateParts.FirstOrDefault(p => !p.Painted && p.ColorKey.Equals(colorKey));
        if (part != null)
        {
            if (animated)
            {
                if (partsPaintingInProgress.Contains(part.PartID))
                {
                    Debug.LogWarning($"Part {part.PartID} ({part.ColorKey}) is already animating.");
                    return;
                }
                StartCoroutine(PaintSeparatePartPathCoroutine(part, cellDelay));
            }
            else
            {
                PaintPart(part);
            }
        }
        else
        {
            Debug.LogWarning($"No available part with color {colorKey}");
        }
    }

    // Paint a separate part (by color key) following its cell path order
    public void PaintSeparatePartPath(string colorKey, float cellDelay = 0.02f)
    {
        StartCoroutine(PaintSeparatePartPathCoroutine(colorKey, cellDelay));
    }

    private System.Collections.IEnumerator PaintSeparatePartPathCoroutine(string colorKey, float delay)
    {
        if (CurrentSeparateParts == null || CurrentSeparateParts.Count == 0)
        {
            Debug.LogWarning("No separate parts available. Make sure a PaintingConfig is loaded.");
            yield break;
        }

        var targetPart = CurrentSeparateParts.FirstOrDefault(p => !p.Painted && p.ColorKey.Equals(colorKey));
        if (targetPart == null)
        {
            Debug.LogWarning($"No unpainted separate part found for color '{colorKey}'.");
            yield break;
        }

        yield return PaintSeparatePartPathCoroutine(targetPart, delay);
    }

    // Core coroutine that animates painting a specific part along its path
    private System.Collections.IEnumerator PaintSeparatePartPathCoroutine(PaintingPartBasedOnColor targetPart, float delay)
    {
        if (targetPart == null || targetPart.PaintingCells == null)
        {
            yield break;
        }

        if (partsPaintingInProgress.Contains(targetPart.PartID))
        {
            yield break;
        }

        partsPaintingInProgress.Add(targetPart.PartID);

        int painted = 0;
        foreach (var cell in targetPart.PaintingCells)
        {
            var spriteCell = GetCellAt(cell.Row, cell.Column);
            if (spriteCell != null)
            {
                spriteCell.Apply();
                if (PumpAnimationManager != null)
                {
                    PumpAnimationManager.SpawnEffectAt(spriteCell.transform.position, cell.CellColor);
                }
                painted++;
            }
            yield return new WaitForSeconds(delay);
        }

        targetPart.Painted = true;
        partsPaintingInProgress.Remove(targetPart.PartID);
        Debug.Log($"Painted separate part path for color '{targetPart.ColorKey}' (Part {targetPart.PartID}) with {painted} cells.");
    }

    private System.Collections.IEnumerator PaintAllCellsAnimated()
    {
        ClearAllCells();
        
        if (CurrentSeparateParts == null || CurrentSeparateParts.Count == 0)
        {
            Debug.LogWarning("No parts to animate");
            yield break;
        }

        Debug.Log("Starting animated painting...");

        foreach (var part in CurrentSeparateParts)
        {
            if (part.PaintingCells == null) continue;

            foreach (var cell in part.PaintingCells)
            {
                var spriteCell = GetCellAt(cell.Row, cell.Column);
                if (spriteCell != null)
                {
                    spriteCell.Apply();
                    if (PumpAnimationManager != null)
                    {
                        PumpAnimationManager.SpawnEffectAt(spriteCell.transform.position, cell.CellColor);
                    }
                }
                yield return new WaitForSeconds(0.01f); // Small delay for animation effect
            }
            
            yield return new WaitForSeconds(0.1f); // Pause between parts
        }

        Debug.Log("Animated painting complete!");
    }
    #endregion

    #region UTILITY FUNCTIONS
    public PaintingFishScaleSpriteCell GetCellAt(int row, int column)
    {
        var key = new Vector2Int(row, column);
        return cellsMap.TryGetValue(key, out var cell) ? cell : null;
    }

    public void PaintCellAt(int row, int column)
    {
        var cell = GetCellAt(row, column);
        if (cell != null)
        {
            cell.Apply();
            if (PumpAnimationManager != null)
            {
                // Use desired color as a fallback when painting manually
                PumpAnimationManager.SpawnEffectAt(cell.transform.position, cell.DesiredColor);
            }
            Debug.Log($"Painted cell at ({row}, {column})");
        }
        else
        {
            Debug.LogWarning($"No cell found at ({row}, {column})");
        }
    }

    public void ClearCellAt(int row, int column)
    {
        var cell = GetCellAt(row, column);
        if (cell != null)
        {
            cell.Clear();
            Debug.Log($"Cleared cell at ({row}, {column})");
        }
    }

    public int GetTotalCells() => PaintingCellsInUse.Count;
    public int GetUnpaintedPartsCount() => CurrentSeparateParts?.Count(part => !part.Painted) ?? 0;

    [ContextMenu("Print Debug Info")]
    public void PrintDebugInfo()
    {
        Debug.Log($"=== SIMPLE PAINTING CONTROLLER DEBUG ===");
        Debug.Log($"Total Cells: {GetTotalCells()}");
        Debug.Log($"Main Parts: {CurrentPaintingParts?.Count ?? 0}");
        Debug.Log($"Separate Parts: {CurrentSeparateParts?.Count ?? 0}");
        Debug.Log($"Unpainted Parts: {GetUnpaintedPartsCount()}");
        
        if (CurrentPaintingConfig != null)
        {
            Debug.Log($"Config: {CurrentPaintingConfig.name}");
            Debug.Log($"Available Cells in Config: {CurrentPaintingConfig.AllCellsAvailable?.Count ?? 0}");
        }
    }
    #endregion
}
