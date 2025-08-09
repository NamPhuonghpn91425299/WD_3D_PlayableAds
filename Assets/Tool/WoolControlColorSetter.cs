using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ========== DATA STRUCTURES ==========

[Serializable]
public class WoolColorGroup
{
    public Color Color = Color.white;
    public List<WoolControl> WoolControls = new List<WoolControl>();
    public bool foldout = true;
    public int Count => WoolControls.Count(w => w != null);
}

[Serializable]
public class WoolColorData
{
    public string objectName;
    public string colorHex;
    public Color color;
    
    public WoolColorData(string name, Color col)
    {
        objectName = name;
        color = col;
        colorHex = "#" + ColorUtility.ToHtmlStringRGBA(col);
    }
}

[Serializable]
public class WoolColorDatabase
{
    public List<WoolColorData> colorDataList = new List<WoolColorData>();
}

[Serializable]
public class GameObjectColorEntry
{
    public GameObject gameObject;
    public Color assignedColor = Color.white;
    public bool isValid => gameObject != null && gameObject.GetComponent<WoolControl>() != null;
}

// ========== MAIN TOOL ==========

public class WoolControlColorSetter : EditorWindow
{
    private List<WoolColorGroup> colorGroups = new List<WoolColorGroup>();
    private Vector2 scroll;
    private GameObject targetGameObject;
    private string exportedData = "";
    private string importData = "";
    private Vector2 dataScroll;
    private GameObject importTargetGameObject;
    
    // Sequential GameObject List
    private List<GameObjectColorEntry> gameObjectSequence = new List<GameObjectColorEntry>();
    private Vector2 sequenceScroll;
    
    // Tabs
    private int selectedTab = 0;
    private string[] tabNames = { "Color Groups", "Auto Organize", "Data Export/Import" };

    [MenuItem("Tools/Wool Control Color Setter")]
    public static void ShowWindow()
    {
        GetWindow<WoolControlColorSetter>("Wool Control Color Setter");
    }

    private void OnEnable()
    {
        LoadFromEditorPrefs();
    }

    private void OnDisable()
    {
        SaveToEditorPrefs();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Wool Control Color Setter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Tab system
        selectedTab = GUILayout.Toolbar(selectedTab, tabNames);
        EditorGUILayout.Space();

        switch (selectedTab)
        {
            case 0:
                DrawColorGroupsTab();
                break;
            case 1:
                DrawAutoOrganizeTab();
                break;
            case 2:
                DrawDataTab();
                break;
        }
    }

    // ========== TAB 1: COLOR GROUPS ==========
    
    private void DrawColorGroupsTab()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        // Vẽ các color groups
        for (int i = 0; i < colorGroups.Count; i++)
        {
            DrawColorGroup(i);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        // Buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New Color Group"))
        {
            colorGroups.Add(new WoolColorGroup());
        }

        if (GUILayout.Button("Find All WoolControls in Scene"))
        {
            FindAllWoolControlsInScene();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Apply All Colors"))
        {
            ApplyAllColors();
        }
    }

    private void DrawColorGroup(int index)
    {
        var group = colorGroups[index];
        
        EditorGUILayout.BeginVertical("box");

        // Header với foldout và count
        EditorGUILayout.BeginHorizontal();
        group.foldout = EditorGUILayout.Foldout(group.foldout, 
            $"Color Group {index + 1} ({group.Count} objects)", true);
        
        // Color preview
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(20, 20), group.Color);
        
        if (GUILayout.Button("X", GUILayout.Width(20)))
        {
            colorGroups.RemoveAt(index);
            return;
        }
        EditorGUILayout.EndHorizontal();

        if (group.foldout)
        {
            EditorGUILayout.Space();

            // Color picker
            group.Color = EditorGUILayout.ColorField("Color", group.Color);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Wool Controls: {group.Count}", EditorStyles.boldLabel);

            // Drop area để kéo thả WoolControl
            Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drag WoolControl GameObjects here", EditorStyles.helpBox);

            // Xử lý drag and drop
            HandleDragAndDrop(dropArea, group);

            // Hiển thị danh sách WoolControls
            DrawWoolControlsList(group);

            EditorGUILayout.Space();

            // Apply color button cho group này
            if (GUILayout.Button($"Apply Color to Group {index + 1} ({group.Count} objects)"))
            {
                ApplyColorToGroup(group);
            }
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
    }

    // ========== TAB 2: AUTO ORGANIZE ==========
    
    private void DrawAutoOrganizeTab()
    {
        EditorGUILayout.LabelField("Auto Organize by Existing Colors", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Drop a GameObject here to automatically find all WoolControls and organize them by their current colors.", MessageType.Info);
        
        EditorGUILayout.Space();
        
        // GameObject field
        targetGameObject = (GameObject)EditorGUILayout.ObjectField("Target GameObject", 
            targetGameObject, typeof(GameObject), true);
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Auto Organize by Colors") && targetGameObject != null)
        {
            AutoOrganizeByColors();
        }
        
        if (GUILayout.Button("Clear All Groups"))
        {
            if (EditorUtility.DisplayDialog("Clear All Groups", 
                "Are you sure you want to clear all color groups?", "Yes", "No"))
            {
                colorGroups.Clear();
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // Statistics
        if (colorGroups.Count > 0)
        {
            EditorGUILayout.LabelField("Statistics:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Total Groups: {colorGroups.Count}");
            EditorGUILayout.LabelField($"Total Objects: {colorGroups.Sum(g => g.Count)}");
            
            foreach (var group in colorGroups.Where(g => g.Count > 0))
            {
                int groupIndex = colorGroups.IndexOf(group);
                EditorGUILayout.BeginHorizontal();
                EditorGUI.DrawRect(GUILayoutUtility.GetRect(15, 15), group.Color);
                EditorGUILayout.LabelField($"Group {groupIndex + 1}: {group.Count} objects ({group.Color})");
                EditorGUILayout.EndHorizontal();
            }
        }
    }
    
    private void AutoOrganizeByColors()
    {
        // Tìm tất cả WoolControl trong target GameObject và children
        WoolControl[] woolControls = targetGameObject.GetComponentsInChildren<WoolControl>();
        
        if (woolControls.Length == 0)
        {
            EditorUtility.DisplayDialog("No WoolControls Found", 
                "No WoolControl components found in the selected GameObject and its children.", "OK");
            return;
        }
        
        // Clear existing groups
        colorGroups.Clear();
        
        // Dictionary để group theo màu
        Dictionary<Color, List<WoolControl>> colorDict = new Dictionary<Color, List<WoolControl>>();
        
        foreach (var woolControl in woolControls)
        {
            if (woolControl.MeshObjectData != null)
            {
                Color currentColor = woolControl.MeshObjectData.HightestColor;
                
                if (!colorDict.ContainsKey(currentColor))
                {
                    colorDict[currentColor] = new List<WoolControl>();
                }
                
                colorDict[currentColor].Add(woolControl);
            }
        }
        
        // Tạo color groups từ dictionary
        foreach (var kvp in colorDict.OrderByDescending(x => x.Value.Count))
        {
            var newGroup = new WoolColorGroup
            {
                Color = kvp.Key,
                WoolControls = kvp.Value,
                foldout = true
            };
            colorGroups.Add(newGroup);
        }
        
        Debug.Log($"Auto-organized {woolControls.Length} WoolControls into {colorGroups.Count} color groups");
        
        // Switch to color groups tab
        selectedTab = 0;
    }

    // ========== TAB 3: DATA EXPORT/IMPORT ==========
    
    private void DrawDataTab()
    {
        EditorGUILayout.LabelField("Data Export/Import", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        // Horizontal layout cho 2 cột
        EditorGUILayout.BeginHorizontal();
        
        // Cột trái: Sequential GameObject List
        EditorGUILayout.BeginVertical("box", GUILayout.Width(position.width * 0.4f));
        DrawSequentialGameObjectList();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space();
        
        // Cột phải: Export/Import
        EditorGUILayout.BeginVertical("box");
        DrawExportImportSection();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void DrawSequentialGameObjectList()
    {
        EditorGUILayout.LabelField("Sequential GameObject List", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Drag GameObjects here in order. Colors will be applied based on imported data sequence.", MessageType.Info);
        
        EditorGUILayout.Space();
        
        // Drop area
        Rect dropArea = GUILayoutUtility.GetRect(0, 60, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag GameObjects here\n(Must have WoolControl component)", EditorStyles.helpBox);
        HandleSequentialDragAndDrop(dropArea);
        
        EditorGUILayout.Space();
        
        // Buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear List"))
        {
            gameObjectSequence.Clear();
        }
        if (GUILayout.Button("Load from Import Data"))
        {
            LoadSequenceFromImportData();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // List hiển thị
        EditorGUILayout.LabelField($"Objects in Sequence: {gameObjectSequence.Count}", EditorStyles.boldLabel);
        
        sequenceScroll = EditorGUILayout.BeginScrollView(sequenceScroll, GUILayout.Height(300));
        
        for (int i = gameObjectSequence.Count - 1; i >= 0; i--)
        {
            var entry = gameObjectSequence[i];
            
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            
            // Index
            EditorGUILayout.LabelField($"{i + 1}.", GUILayout.Width(30));
            
            // GameObject field
            entry.gameObject = (GameObject)EditorGUILayout.ObjectField(
                entry.gameObject, typeof(GameObject), true, GUILayout.ExpandWidth(true));
            
            // Color field
            entry.assignedColor = EditorGUILayout.ColorField(entry.assignedColor, GUILayout.Width(50));
            
            // Remove button
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                gameObjectSequence.RemoveAt(i);
                continue;
            }
            
            EditorGUILayout.EndHorizontal();
            
            // Validation
            if (entry.gameObject == null)
            {
                EditorGUILayout.HelpBox("GameObject is null", MessageType.Warning);
            }
            else if (entry.gameObject.GetComponent<WoolControl>() == null)
            {
                EditorGUILayout.HelpBox("No WoolControl component found", MessageType.Error);
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(2);
        }
        
        EditorGUILayout.EndScrollView();
        
        // Apply colors từ sequence
        EditorGUILayout.Space();
        if (GUILayout.Button($"Apply Colors to Sequence ({gameObjectSequence.Count(e => e.isValid)} valid objects)"))
        {
            ApplyColorsToSequence();
        }
    }
    
    private void DrawExportImportSection()
    {
        EditorGUILayout.LabelField("Data Export/Import", EditorStyles.boldLabel);
        
        // Export section
        EditorGUILayout.LabelField("Export Data:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Export Color Data"))
        {
            ExportColorData();
        }
        if (GUILayout.Button("Copy to Clipboard"))
        {
            EditorGUIUtility.systemCopyBuffer = exportedData;
            Debug.Log("Data copied to clipboard!");
        }
        EditorGUILayout.EndHorizontal();
        
        if (!string.IsNullOrEmpty(exportedData))
        {
            EditorGUILayout.LabelField("Exported Data:");
            dataScroll = EditorGUILayout.BeginScrollView(dataScroll, GUILayout.Height(120));
            EditorGUILayout.TextArea(exportedData, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }
        
        EditorGUILayout.Space();
        
        // Import section
        EditorGUILayout.LabelField("Import Data:", EditorStyles.boldLabel);
        
        // Target GameObject for import
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Target GameObject (Optional):");
        importTargetGameObject = (GameObject)EditorGUILayout.ObjectField("Import Target", 
            importTargetGameObject, typeof(GameObject), true);
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Paste data here:");
        importData = EditorGUILayout.TextArea(importData, GUILayout.Height(80));
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Paste from Clipboard"))
        {
            importData = EditorGUIUtility.systemCopyBuffer;
        }
        if (GUILayout.Button("Import and Apply Colors"))
        {
            ImportAndApplyColorData();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // JSON Export/Import
        EditorGUILayout.LabelField("JSON Format:");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Export as JSON"))
        {
            ExportAsJSON();
        }
        if (GUILayout.Button("Import from JSON"))
        {
            ImportFromJSON();
        }
        EditorGUILayout.EndHorizontal();
        
        // Preview section
        if (!string.IsNullOrEmpty(importData) && importTargetGameObject != null)
        {
            EditorGUILayout.Space();
            DrawImportPreview();
        }
    }
    
    private void HandleSequentialDragAndDrop(Rect dropArea)
    {
        Event currentEvent = Event.current;
        
        if (dropArea.Contains(currentEvent.mousePosition))
        {
            if (currentEvent.type == EventType.DragUpdated || currentEvent.type == EventType.DragPerform)
            {
                bool hasValidObject = false;
                
                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    if (draggedObject is GameObject gameObject)
                    {
                        WoolControl woolControl = gameObject.GetComponent<WoolControl>();
                        if (woolControl != null)
                        {
                            hasValidObject = true;
                            break;
                        }
                    }
                }

                if (hasValidObject)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    
                    if (currentEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        
                        foreach (var draggedObject in DragAndDrop.objectReferences)
                        {
                            if (draggedObject is GameObject gameObject)
                            {
                                WoolControl woolControl = gameObject.GetComponent<WoolControl>();
                                if (woolControl != null)
                                {
                                    // Check if not already in list
                                    bool alreadyExists = gameObjectSequence.Any(e => e.gameObject == gameObject);
                                    if (!alreadyExists)
                                    {
                                        gameObjectSequence.Add(new GameObjectColorEntry 
                                        { 
                                            gameObject = gameObject,
                                            assignedColor = Color.white
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                }
                
                currentEvent.Use();
            }
        }
    }
    
    private void LoadSequenceFromImportData()
    {
        if (string.IsNullOrEmpty(importData))
        {
            EditorUtility.DisplayDialog("No Data", "Please paste import data first.", "OK");
            return;
        }
        
        string[] lines = importData.Split('\n');
        gameObjectSequence.Clear();
        
        WoolControl[] targetWoolControls = GetWoolControlsForImport();
        HashSet<WoolControl> usedWoolControls = new HashSet<WoolControl>();
        
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                continue;
                
            string[] parts = line.Split('|');
            if (parts.Length >= 2)
            {
                string objectName = parts[0].Trim();
                string colorHex = parts[1].Trim();
                
                // Tìm WoolControl chưa được sử dụng với tên phù hợp
                WoolControl targetWoolControl = targetWoolControls
                    .Where(w => w.name == objectName && !usedWoolControls.Contains(w))
                    .FirstOrDefault();
                
                if (targetWoolControl != null && ColorUtility.TryParseHtmlString(colorHex, out Color color))
                {
                    gameObjectSequence.Add(new GameObjectColorEntry 
                    { 
                        gameObject = targetWoolControl.gameObject,
                        assignedColor = color
                    });
                    
                    // Đánh dấu đã sử dụng
                    usedWoolControls.Add(targetWoolControl);
                }
            }
        }
        
        Debug.Log($"Loaded {gameObjectSequence.Count} objects into sequence from import data");
    }
    
    private void ApplyColorsToSequence()
    {
        int applied = 0;
        
        foreach (var entry in gameObjectSequence)
        {
            if (entry.isValid)
            {
                WoolControl woolControl = entry.gameObject.GetComponent<WoolControl>();
                if (woolControl.MeshObjectData != null)
                {
                    Undo.RecordObject(woolControl, "Apply Sequential Color");
                    woolControl.MeshObjectData.HightestColor = entry.assignedColor;
                    EditorUtility.SetDirty(woolControl);
                    applied++;
                }
            }
        }
        
        string message = $"Applied colors to {applied} objects in sequence.";
        EditorUtility.DisplayDialog("Sequential Apply Complete", message, "OK");
        Debug.Log(message);
    }

    private void DrawImportPreview()
    {
        EditorGUILayout.LabelField("Import Preview:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        
        WoolControl[] targetWoolControls = GetWoolControlsForImport();
        string[] lines = importData.Split('\n');
        int matchCount = 0;
        int duplicateCount = 0;
        HashSet<WoolControl> previewProcessed = new HashSet<WoolControl>();
        
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                continue;
                
            string[] parts = line.Split('|');
            if (parts.Length >= 2)
            {
                string objectName = parts[0].Trim();
                string colorHex = parts[1].Trim();
                
                // Tìm object chưa được xử lý trong preview
                WoolControl targetWoolControl = targetWoolControls
                    .Where(w => w.name == objectName && !previewProcessed.Contains(w))
                    .FirstOrDefault();
                
                EditorGUILayout.BeginHorizontal();
                if (targetWoolControl != null && ColorUtility.TryParseHtmlString(colorHex, out Color color))
                {
                    EditorGUILayout.LabelField("✓", GUILayout.Width(20));
                    EditorGUI.DrawRect(GUILayoutUtility.GetRect(15, 15), color);
                    EditorGUILayout.LabelField($"{objectName} -> {colorHex}");
                    
                    // Đánh dấu đã xử lý trong preview
                    previewProcessed.Add(targetWoolControl);
                    matchCount++;
                }
                else if (targetWoolControls.Any(w => w.name == objectName && previewProcessed.Contains(w)))
                {
                    EditorGUILayout.LabelField("⚠", GUILayout.Width(20));
                    EditorGUILayout.LabelField($"{objectName} -> {colorHex} (Duplicate, will be skipped)", EditorStyles.miniLabel);
                    duplicateCount++;
                }
                else
                {
                    EditorGUILayout.LabelField("✗", GUILayout.Width(20));
                    EditorGUILayout.LabelField($"{objectName} -> {colorHex} (Not Found)", EditorStyles.miniLabel);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        EditorGUILayout.EndVertical();
        
        int totalDataLines = lines.Length - CountCommentLines(lines);
        string previewSummary = $"Will apply: {matchCount}/{totalDataLines} objects";
        if (duplicateCount > 0)
        {
            previewSummary += $", {duplicateCount} duplicates will be skipped";
        }
        
        EditorGUILayout.LabelField(previewSummary);
    }
    
    private int CountCommentLines(string[] lines)
    {
        return lines.Count(line => string.IsNullOrEmpty(line) || line.StartsWith("#"));
    }
    
    private WoolControl[] GetWoolControlsForImport()
    {
        if (importTargetGameObject != null)
        {
            // Tìm trong GameObject được chỉ định và children
            return importTargetGameObject.GetComponentsInChildren<WoolControl>();
        }
        else
        {
            // Tìm trong toàn bộ scene
            return FindObjectsOfType<WoolControl>();
        }
    }
    
    private void ExportColorData()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("# Wool Control Color Data");
        sb.AppendLine("# Format: ObjectName | ColorHex | ColorRGBA");
        sb.AppendLine();
        
        foreach (var group in colorGroups)
        {
            foreach (var woolControl in group.WoolControls)
            {
                if (woolControl != null)
                {
                    string colorHex = "#" + ColorUtility.ToHtmlStringRGBA(group.Color);
                    sb.AppendLine($"{woolControl.name} | {colorHex} | {group.Color}");
                }
            }
        }
        
        exportedData = sb.ToString();
        Debug.Log($"Exported color data for {colorGroups.Sum(g => g.Count)} objects");
    }
    
    private void ImportAndApplyColorData()
    {
        if (string.IsNullOrEmpty(importData))
        {
            EditorUtility.DisplayDialog("No Data", "Please paste data to import.", "OK");
            return;
        }
        
        string[] lines = importData.Split('\n');
        int applied = 0;
        int notFound = 0;
        int duplicateSkipped = 0;
        
        // Lấy WoolControls từ target GameObject hoặc toàn scene
        WoolControl[] targetWoolControls = GetWoolControlsForImport();
        HashSet<WoolControl> processedWoolControls = new HashSet<WoolControl>();
        
        string searchScope = importTargetGameObject != null ? 
            $"within '{importTargetGameObject.name}' and its children" : "in entire scene";
        
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                continue;
                
            string[] parts = line.Split('|');
            if (parts.Length >= 2)
            {
                string objectName = parts[0].Trim();
                string colorHex = parts[1].Trim();
                
                // Tìm object chưa được xử lý trong target WoolControls
                WoolControl targetWoolControl = targetWoolControls
                    .Where(w => w.name == objectName && !processedWoolControls.Contains(w))
                    .FirstOrDefault();
                
                if (targetWoolControl != null && ColorUtility.TryParseHtmlString(colorHex, out Color color))
                {
                    if (targetWoolControl.MeshObjectData != null)
                    {
                        Undo.RecordObject(targetWoolControl, "Import Color Data");
                        targetWoolControl.MeshObjectData.HightestColor = color;
                        EditorUtility.SetDirty(targetWoolControl);
                        
                        // Đánh dấu đã xử lý
                        processedWoolControls.Add(targetWoolControl);
                        applied++;
                    }
                }
                else if (targetWoolControls.Any(w => w.name == objectName && processedWoolControls.Contains(w)))
                {
                    // Object cùng tên đã được xử lý
                    duplicateSkipped++;
                    Debug.LogWarning($"Object '{objectName}' skipped - already processed an object with this name");
                }
                else
                {
                    notFound++;
                    Debug.LogWarning($"Object '{objectName}' not found {searchScope}");
                }
            }
        }
        
        string message = $"Applied colors to {applied} objects {searchScope}.";
        if (notFound > 0)
        {
            message += $"\n{notFound} objects not found.";
        }
        if (duplicateSkipped > 0)
        {
            message += $"\n{duplicateSkipped} duplicate objects skipped.";
        }
        
        EditorUtility.DisplayDialog("Import Complete", message, "OK");
        Debug.Log(message);
    }
    
    private void ExportAsJSON()
    {
        var database = new WoolColorDatabase();
        
        foreach (var group in colorGroups)
        {
            foreach (var woolControl in group.WoolControls)
            {
                if (woolControl != null)
                {
                    database.colorDataList.Add(new WoolColorData(woolControl.name, group.Color));
                }
            }
        }
        
        string json = JsonUtility.ToJson(database, true);
        exportedData = json;
        
        // Lưu file JSON
        string path = EditorUtility.SaveFilePanel("Save Color Data", "", "WoolColorData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            System.IO.File.WriteAllText(path, json);
            Debug.Log($"Saved color data to: {path}");
        }
    }
    
    private void ImportFromJSON()
    {
        string path = EditorUtility.OpenFilePanel("Load Color Data", "", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = System.IO.File.ReadAllText(path);
            importData = json;
            
            try
            {
                var database = JsonUtility.FromJson<WoolColorDatabase>(json);
                ImportFromDatabase(database);
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("Import Error", $"Failed to parse JSON: {e.Message}", "OK");
            }
        }
    }
    
    private void ImportFromDatabase(WoolColorDatabase database)
    {
        int applied = 0;
        int notFound = 0;
        int duplicateSkipped = 0;
        
        // Lấy WoolControls từ target GameObject hoặc toàn scene
        WoolControl[] targetWoolControls = GetWoolControlsForImport();
        HashSet<WoolControl> processedWoolControls = new HashSet<WoolControl>();
        
        string searchScope = importTargetGameObject != null ? 
            $"within '{importTargetGameObject.name}' and its children" : "in entire scene";
        
        foreach (var colorData in database.colorDataList)
        {
            // Tìm object chưa được xử lý
            WoolControl targetWoolControl = targetWoolControls
                .Where(w => w.name == colorData.objectName && !processedWoolControls.Contains(w))
                .FirstOrDefault();
            
            if (targetWoolControl != null && targetWoolControl.MeshObjectData != null)
            {
                Undo.RecordObject(targetWoolControl, "Import JSON Color Data");
                targetWoolControl.MeshObjectData.HightestColor = colorData.color;
                EditorUtility.SetDirty(targetWoolControl);
                
                // Đánh dấu đã xử lý
                processedWoolControls.Add(targetWoolControl);
                applied++;
            }
            else if (targetWoolControls.Any(w => w.name == colorData.objectName && processedWoolControls.Contains(w)))
            {
                // Object cùng tên đã được xử lý
                duplicateSkipped++;
                Debug.LogWarning($"Object '{colorData.objectName}' skipped - already processed an object with this name");
            }
            else
            {
                notFound++;
                Debug.LogWarning($"Object '{colorData.objectName}' not found {searchScope}");
            }
        }
        
        string message = $"Applied colors to {applied} objects {searchScope}.";
        if (notFound > 0)
        {
            message += $"\n{notFound} objects not found.";
        }
        if (duplicateSkipped > 0)
        {
            message += $"\n{duplicateSkipped} duplicate objects skipped.";
        }
        
        EditorUtility.DisplayDialog("JSON Import Complete", message, "OK");
        Debug.Log(message);
    }

    // ========== SHARED METHODS ==========
    
    private void HandleDragAndDrop(Rect dropArea, WoolColorGroup group)
    {
        Event currentEvent = Event.current;
        
        if (dropArea.Contains(currentEvent.mousePosition))
        {
            if (currentEvent.type == EventType.DragUpdated || currentEvent.type == EventType.DragPerform)
            {
                bool hasValidObject = false;
                
                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    if (draggedObject is GameObject gameObject)
                    {
                        WoolControl woolControl = gameObject.GetComponent<WoolControl>();
                        if (woolControl != null)
                        {
                            hasValidObject = true;
                            break;
                        }
                    }
                }

                if (hasValidObject)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    
                    if (currentEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        
                        foreach (var draggedObject in DragAndDrop.objectReferences)
                        {
                            if (draggedObject is GameObject gameObject)
                            {
                                WoolControl woolControl = gameObject.GetComponent<WoolControl>();
                                if (woolControl != null && !group.WoolControls.Contains(woolControl))
                                {
                                    group.WoolControls.Add(woolControl);
                                }
                            }
                        }
                    }
                }
                else
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                }
                
                currentEvent.Use();
            }
        }
    }

    private void DrawWoolControlsList(WoolColorGroup group)
    {
        for (int i = group.WoolControls.Count - 1; i >= 0; i--)
        {
            if (group.WoolControls[i] == null)
            {
                group.WoolControls.RemoveAt(i);
                continue;
            }

            EditorGUILayout.BeginHorizontal();
            
            // Object field (read-only để hiển thị)
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(group.WoolControls[i], typeof(WoolControl), true);
            EditorGUI.EndDisabledGroup();

            // Remove button
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                group.WoolControls.RemoveAt(i);
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }

    private void ApplyColorToGroup(WoolColorGroup group)
    {
        int appliedCount = 0;
        
        foreach (var woolControl in group.WoolControls)
        {
            if (woolControl != null && woolControl.MeshObjectData != null)
            {
                if (woolControl.MeshObjectData.HightestColor != group.Color)
                {
                    Undo.RecordObject(woolControl, "Set Highest Color");
                    woolControl.MeshObjectData.HightestColor = group.Color;
                    EditorUtility.SetDirty(woolControl);
                    appliedCount++;
                }
            }
        }

        if (appliedCount > 0)
        {
            Debug.Log($"Applied color {group.Color} to {appliedCount} WoolControls");
        }
    }

    private void ApplyAllColors()
    {
        foreach (var group in colorGroups)
        {
            ApplyColorToGroup(group);
        }
    }

    private void FindAllWoolControlsInScene()
    {
        WoolControl[] allWoolControls = FindObjectsOfType<WoolControl>();
        
        if (colorGroups.Count == 0)
        {
            colorGroups.Add(new WoolColorGroup());
        }

        var firstGroup = colorGroups[0];
        firstGroup.WoolControls.Clear();
        
        foreach (var woolControl in allWoolControls)
        {
            firstGroup.WoolControls.Add(woolControl);
        }

        Debug.Log($"Found {allWoolControls.Length} WoolControls in scene");
    }

    private void SaveToEditorPrefs()
    {
        EditorPrefs.SetInt("WoolColorSetter_GroupCount", colorGroups.Count);
        
        for (int i = 0; i < colorGroups.Count; i++)
        {
            var group = colorGroups[i];
            string colorKey = $"WoolColorSetter_Group{i}_Color";
            
            EditorPrefs.SetString(colorKey, ColorUtility.ToHtmlStringRGBA(group.Color));
        }
    }

    private void LoadFromEditorPrefs()
    {
        int groupCount = EditorPrefs.GetInt("WoolColorSetter_GroupCount", 0);
        colorGroups.Clear();
        
        for (int i = 0; i < groupCount; i++)
        {
            string colorKey = $"WoolColorSetter_Group{i}_Color";
            string colorString = EditorPrefs.GetString(colorKey, "FFFFFF");
            
            Color color;
            if (ColorUtility.TryParseHtmlString("#" + colorString, out color))
            {
                colorGroups.Add(new WoolColorGroup { Color = color });
            }
        }

        if (colorGroups.Count == 0)
        {
            colorGroups.Add(new WoolColorGroup());
        }
    }
}