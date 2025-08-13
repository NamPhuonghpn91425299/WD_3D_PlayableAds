using UnityEngine;
using System.Collections;


public class SimplePaintingTester : MonoBehaviour
{
    [Header("REFERENCES")]
    public SimplePaintingController paintingController;
    

    [ContextMenu("Test Basic Functions")]
    public void TestBasicFunctions()
    {
        StartCoroutine(TestBasicFunctionsCoroutine());
    }
    
    [ContextMenu("Test Paint By Color")]
    public void TestPaintByColor()
    {
        if (paintingController == null) return;
        
        // Test paint specific colors
        paintingController.PaintPartByColor("Brown1");
        StartCoroutine(DelayedAction(1f, () => paintingController.PaintPartByColor("Green3")));
        StartCoroutine(DelayedAction(2f, () => paintingController.PaintPartByColor("White")));
    }
    
    [ContextMenu("Test Random Painting")]
    public void TestRandomPainting()
    {
        StartCoroutine(TestRandomPaintingCoroutine());
    }
    
    [ContextMenu("Test Manual Cell Painting")]
    public void TestManualCellPainting()
    {
        if (paintingController == null) return;
        
        // Paint some specific cells
        paintingController.PaintCellAt(5, 5);
        paintingController.PaintCellAt(10, 10);
        paintingController.PaintCellAt(15, 15);
        paintingController.PaintCellAt(20, 20);
        paintingController.PaintCellAt(25, 25);
    }
    

    [ContextMenu("Test Grid Patterns")]
    public void TestGridPatterns()
    {
        StartCoroutine(TestGridPatternsCoroutine());
    }
    
    [ContextMenu("Test Performance")]
    public void TestPerformance()
    {
        if (paintingController == null) return;
        
        var startTime = Time.realtimeSinceStartup;
        
        // Clear and paint multiple times
        for (int i = 0; i < 5; i++)
        {
            paintingController.ClearAllCells();
            paintingController.PaintAllCells();
        }
        
        var endTime = Time.realtimeSinceStartup;
        Debug.Log($"Performance test completed in {(endTime - startTime) * 1000:F2}ms");
    }

    [ContextMenu("Test by color Blue3")]
    public void TestByColor()
    {
        if (paintingController == null) return;
        
        // Test paint specific colors
        paintingController.PaintPartByColor("Blue3");
       
    }

    private void Start()
    {
        if (paintingController == null)
        {
            paintingController = FindObjectOfType<SimplePaintingController>();
            if (paintingController == null)
            {
                Debug.LogError("No SimplePaintingController found in scene!");
            }
        }
    }

    private IEnumerator TestBasicFunctionsCoroutine()
    {
        if (paintingController == null) yield break;
        
        Debug.Log("=== STARTING BASIC FUNCTION TEST ===");
        
        // 1. Clear all
        Debug.Log("1. Clearing all cells...");
        paintingController.ClearAllCells();
        yield return new WaitForSeconds(1f);
        
        // 2. Load config
        Debug.Log("2. Loading config...");
        paintingController.LoadConfig();
        yield return new WaitForSeconds(1f);
        
        // // 3. Paint one part
        // Debug.Log("3. Painting one random part...");
        // paintingController.PaintRandomPart();
        // yield return new WaitForSeconds(2f);
        
        // // 4. Paint another part
        // Debug.Log("4. Painting another random part...");
        // paintingController.PaintRandomPart();
        // yield return new WaitForSeconds(2f);
        
        // // 5. Paint all instantly
        // Debug.Log("5. Painting all instantly...");
        // paintingController.PaintAllInstantly();
        // yield return new WaitForSeconds(1f);
        
        // 6. Clear and animate
        Debug.Log("6. Clearing and starting animation...");
        paintingController.ClearAllCells();
        yield return new WaitForSeconds(1f);
        paintingController.PaintAllWithAnimation();
        
        Debug.Log("=== BASIC FUNCTION TEST COMPLETE ===");
    }
    
    private IEnumerator TestRandomPaintingCoroutine()
    {
        if (paintingController == null) yield break;
        
        Debug.Log("=== STARTING RANDOM PAINTING TEST ===");
        
        paintingController.ClearAllCells();
        yield return new WaitForSeconds(0.5f);
        
        // Paint parts randomly until all done
        int maxAttempts = 50; // Prevent infinite loop
        int attempts = 0;
        
        while (paintingController.GetUnpaintedPartsCount() > 0 && attempts < maxAttempts)
        {
            paintingController.PaintRandomPart();
            paintingController.PrintDebugInfo();
            
            yield return new WaitForSeconds(0.5f);
            attempts++;
        }
        
        Debug.Log($"=== RANDOM PAINTING TEST COMPLETE (Attempts: {attempts}) ===");
    }
    
    private IEnumerator TestGridPatternsCoroutine()
    {
        if (paintingController == null) yield break;
        
        Debug.Log("=== STARTING GRID PATTERN TEST ===");
        
        // Pattern 1: Diagonal
        Debug.Log("Pattern 1: Diagonal line");
        paintingController.ClearAllCells();
        for (int i = 0; i < 30; i++)
        {
            paintingController.PaintCellAt(i, i);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1f);
        
        // Pattern 2: Border
        Debug.Log("Pattern 2: Border");
        paintingController.ClearAllCells();
        // Top and bottom
        for (int x = 0; x < 30; x++)
        {
            paintingController.PaintCellAt(0, x);
            paintingController.PaintCellAt(29, x);
            yield return new WaitForSeconds(0.02f);
        }
        // Left and right
        for (int y = 1; y < 29; y++)
        {
            paintingController.PaintCellAt(y, 0);
            paintingController.PaintCellAt(y, 29);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1f);
        
        // Pattern 3: Cross
        Debug.Log("Pattern 3: Cross");
        paintingController.ClearAllCells();
        for (int i = 0; i < 30; i++)
        {
            paintingController.PaintCellAt(15, i); // Horizontal line
            paintingController.PaintCellAt(i, 15); // Vertical line
            yield return new WaitForSeconds(0.03f);
        }
        
        Debug.Log("=== GRID PATTERN TEST COMPLETE ===");
    }
    
    private IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
    

    [ContextMenu("Show Runtime Stats")]
    public void ShowRuntimeStats()
    {
        if (paintingController == null) return;
        
        paintingController.PrintDebugInfo();
        
        // Additional stats
        var unpaintedParts = paintingController.GetUnpaintedPartsCount();

   
    }
}
