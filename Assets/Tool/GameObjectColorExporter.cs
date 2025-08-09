// using UnityEngine;
// using UnityEditor;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
//
// // ========== DATA STRUCTURES ==========
//
// [System.Serializable]
// public class WoolControlColorEntry
// {
//     public GameObject gameObject;
//     public WoolControl woolControl;
//     public Color extractedColor = Color.white;
//     public string colorKey = "";
//     public bool hasValidWoolControl = false;
//     public bool hasValidColorKey = false;
//     
//     public WoolControlColorEntry(GameObject go)
//     {
//         gameObject = go;
//         UpdateWoolControlInfo();
//     }
//     
//     public void UpdateWoolControlInfo()
//     {
//         if (gameObject == null) return;
//         
//         woolControl = gameObject.GetComponent<WoolControl>();
//         if (woolControl != null && woolControl.MeshObjectData != null)
//         {
//             hasValidWoolControl = true;
//             colorKey = woolControl.MeshObjectData.HightestColor;
//             hasValidColorKey = !string.IsNullOrEmpty(colorKey);
//         }
//         else
//         {
//             hasValidWoolControl = false;
//             hasValidColorKey = false;
//             colorKey = "";
//             extractedColor = Color.white;
//         }
//     }
//     
//     public void UpdateColorFromPalette(ColorPalleteData colorPalette)
//     {
//         if (colorPalette != null && hasValidColorKey && colorPalette.colorPallete.ContainsKey(colorKey))
//         {
//             extractedColor = colorPalette.colorPallete[colorKey];
//         }
//         else
//         {
//             extractedColor = Color.white;
//         }
//     }
// }
//
// // ========== MAIN TOOL ==========
//
// public class WoolControlColorExtractor : EditorWindow
// {
//     private List<WoolControlColorEntry> woolControlEntries = new List<WoolControlColorEntry>();
//     private ColorPalleteData colorPalette = null;
//     private Vector2 scrollPosition;
//     private string exportedData = "";
//     private Vector2 dataScrollPosition;
//     private string exportFileName = "WoolControlColors";
//     
//     // Filter options
//     private bool showOnlyValidWoolControls = true;
//     private bool autoRefreshColors = true;
//     private string colorKeyFilter = "";
//     
//     // Export options
//     private bool includeComments = true;
//     private bool includeRGBAValues = true;
//     private bool sortByName = false;
//
//     [MenuItem("Tools/WoolControl Color Extractor")]
//     public static void ShowWindow()
//     {
//         GetWindow<WoolControlColorExtractor>("WoolControl Color Extractor");
//     }
//
//     private void OnGUI()
//     {
//         EditorGUILayout.LabelField("WoolControl Color Extractor", EditorStyles.boldLabel);
//         EditorGUILayout.HelpBox("Extract colors from WoolControl components using ColorPalleteData and export in compatible format", MessageType.Info);
//         EditorGUILayout.Space();
//
//         DrawOptionsSection();
//         EditorGUILayout.Space();
//         DrawWoolControlListSection();
//         EditorGUILayout.Space();
//         DrawExportSection();
//     }
//
//     private void DrawOptionsSection()
//     {
//         EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
//         EditorGUILayout.BeginVertical("box");
//         
//         // Color Palette Selection
//         EditorGUILayout.LabelField("Color Palette:", EditorStyles.boldLabel);
//         ColorPalleteData newColorPalette = (ColorPalleteData)EditorGUILayout.ObjectField(
//             "Color Palette Data", colorPalette, typeof(ColorPalleteData), false);
//         
//         if (newColorPalette != colorPalette)
//         {
//             colorPalette = newColorPalette;
//             RefreshAllColors();
//         }
//         
//         if (colorPalette == null)
//         {
//             EditorGUILayout.HelpBox("Please assign a ColorPalleteData to extract colors from WoolControl components.", MessageType.Warning);
//         }
//         else
//         {
//             EditorGUILayout.LabelField($"Palette contains {colorPalette.colorPallete.Count} colors", EditorStyles.miniLabel);
//         }
//         
//         EditorGUILayout.Space();
//         
//         showOnlyValidWoolControls = EditorGUILayout.Toggle("Show Only Valid WoolControls", showOnlyValidWoolControls);
//         autoRefreshColors = EditorGUILayout.Toggle("Auto Refresh Colors", autoRefreshColors);
//         
//         EditorGUILayout.BeginHorizontal();
//         EditorGUILayout.LabelField("Color Key Filter:", GUILayout.Width(120));
//         colorKeyFilter = EditorGUILayout.TextField(colorKeyFilter);
//         if (GUILayout.Button("Clear", GUILayout.Width(50)))
//         {
//             colorKeyFilter = "";
//         }
//         EditorGUILayout.EndHorizontal();
//         
//         EditorGUILayout.Space();
//         
//         // Export options
//         EditorGUILayout.LabelField("Export Options:", EditorStyles.miniLabel);
//         includeComments = EditorGUILayout.Toggle("Include Comments", includeComments);
//         includeRGBAValues = EditorGUILayout.Toggle("Include RGBA Values", includeRGBAValues);
//         sortByName = EditorGUILayout.Toggle("Sort by Name", sortByName);
//         
//         EditorGUILayout.EndVertical();
//     }
//
//     private void DrawWoolControlListSection()
//     {
//         EditorGUILayout.LabelField("WoolControl Objects", EditorStyles.boldLabel);
//         
//         // Drop Area
//         Rect dropArea = GUILayoutUtility.GetRect(0, 60, GUILayout.ExpandWidth(true));
//         GUI.Box(dropArea, "Drag GameObjects with WoolControl components here", EditorStyles.helpBox);
//         HandleDragAndDrop(dropArea);
//         
//         EditorGUILayout.Space();
//         
//         // Control buttons
//         EditorGUILayout.BeginHorizontal();
//         if (GUILayout.Button("Clear All"))
//         {
//             woolControlEntries.Clear();
//         }
//         if (GUILayout.Button("Refresh All Colors"))
//         {
//             RefreshAllColors();
//         }
//         if (GUILayout.Button("Remove Invalid"))
//         {
//             RemoveInvalidEntries();
//         }
//         if (GUILayout.Button("Find in Scene"))
//         {
//             FindAllWoolControlsInScene();
//         }
//         EditorGUILayout.EndHorizontal();
//         
//         EditorGUILayout.Space();
//         
//         // Statistics
//         int validCount = woolControlEntries.Count(e => e.hasValidWoolControl);
//         int totalCount = woolControlEntries.Count;
//         int validColorCount = woolControlEntries.Count(e => e.hasValidColorKey && colorPalette != null);
//         
//         EditorGUILayout.LabelField($"Objects: {totalCount} ({validCount} with WoolControl, {validColorCount} with valid colors)", EditorStyles.miniLabel);
//         
//         // WoolControl list
//         scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
//         
//         var entriesToShow = showOnlyValidWoolControls ? 
//             woolControlEntries.Where(e => e.hasValidWoolControl).ToList() : 
//             woolControlEntries;
//         
//         if (!string.IsNullOrEmpty(colorKeyFilter))
//         {
//             entriesToShow = entriesToShow.Where(e => e.colorKey.ToLower().Contains(colorKeyFilter.ToLower())).ToList();
//         }
//         
//         for (int i = entriesToShow.Count - 1; i >= 0; i--)
//         {
//             DrawWoolControlEntry(entriesToShow[i], i);
//         }
//         
//         EditorGUILayout.EndScrollView();
//     }
//
//     private void DrawWoolControlEntry(WoolControlColorEntry entry, int index)
//     {
//         if (entry.gameObject == null)
//         {
//             woolControlEntries.Remove(entry);
//             return;
//         }
//         
//         EditorGUILayout.BeginVertical("box");
//         EditorGUILayout.BeginHorizontal();
//         
//         // Object field
//         entry.gameObject = (GameObject)EditorGUILayout.ObjectField(
//             entry.gameObject, typeof(GameObject), true, GUILayout.ExpandWidth(true));
//         
//         // Color field (read-only, from palette)
//         EditorGUI.BeginDisabledGroup(true);
//         EditorGUILayout.ColorField(entry.extractedColor, GUILayout.Width(50));
//         EditorGUI.EndDisabledGroup();
//         
//         // Refresh button
//         if (GUILayout.Button("↻", GUILayout.Width(25)))
//         {
//             entry.UpdateWoolControlInfo();
//             if (colorPalette != null)
//             {
//                 entry.UpdateColorFromPalette(colorPalette);
//             }
//         }
//         
//         // Remove button
//         if (GUILayout.Button("X", GUILayout.Width(25)))
//         {
//             woolControlEntries.Remove(entry);
//             return;
//         }
//         
//         EditorGUILayout.EndHorizontal();
//         
//         // WoolControl info
//         if (entry.hasValidWoolControl)
//         {
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField($"Color Key: {entry.colorKey}", EditorStyles.miniLabel, GUILayout.ExpandWidth(true));
//             
//             // Show color key validity
//             if (colorPalette != null)
//             {
//                 bool keyExists = colorPalette.colorPallete.ContainsKey(entry.colorKey);
//                 string status = keyExists ? "✓ Found" : "✗ Missing";
//                 Color statusColor = keyExists ? Color.green : Color.red;
//                 
//                 var oldColor = GUI.color;
//                 GUI.color = statusColor;
//                 EditorGUILayout.LabelField(status, EditorStyles.miniLabel, GUILayout.Width(60));
//                 GUI.color = oldColor;
//             }
//             else
//             {
//                 EditorGUILayout.LabelField("No Palette", EditorStyles.miniLabel, GUILayout.Width(60));
//             }
//             
//             EditorGUILayout.EndHorizontal();
//             
//             // Show additional WoolControl info
//             if (entry.woolControl.MeshObjectData != null)
//             {
//                 EditorGUILayout.LabelField($"Total Layers: {entry.woolControl.MeshObjectData.TotalLayer}", EditorStyles.miniLabel);
//                 if (entry.woolControl.MeshObjectData.ColorStack.Count > 0)
//                 {
//                     string colorStack = string.Join(", ", entry.woolControl.MeshObjectData.ColorStack);
//                     EditorGUILayout.LabelField($"Color Stack: {colorStack}", EditorStyles.miniLabel);
//                 }
//             }
//         }
//         else
//         {
//             EditorGUILayout.LabelField("No WoolControl component found", EditorStyles.miniLabel);
//         }
//         
//         EditorGUILayout.EndVertical();
//         EditorGUILayout.Space(2);
//     }
//
//     private void DrawExportSection()
//     {
//         EditorGUILayout.LabelField("Export Data", EditorStyles.boldLabel);
//         
//         EditorGUILayout.BeginHorizontal();
//         EditorGUILayout.LabelField("Export File Name:", GUILayout.Width(100));
//         exportFileName = EditorGUILayout.TextField(exportFileName);
//         EditorGUILayout.EndHorizontal();
//         
//         EditorGUILayout.Space();
//         
//         // Export buttons
//         EditorGUILayout.BeginHorizontal();
//         if (GUILayout.Button("Export for WoolControlColorSetter"))
//         {
//             ExportForWoolControlColorSetter();
//         }
//         if (GUILayout.Button("Export as JSON"))
//         {
//             ExportAsJSON();
//         }
//         if (GUILayout.Button("Copy to Clipboard"))
//         {
//             if (!string.IsNullOrEmpty(exportedData))
//             {
//                 EditorGUIUtility.systemCopyBuffer = exportedData;
//                 Debug.Log("Data copied to clipboard!");
//             }
//         }
//         EditorGUILayout.EndHorizontal();
//         
//         EditorGUILayout.Space();
//         
//         // Export preview
//         if (!string.IsNullOrEmpty(exportedData))
//         {
//             EditorGUILayout.LabelField("Exported Data Preview:", EditorStyles.boldLabel);
//             dataScrollPosition = EditorGUILayout.BeginScrollView(dataScrollPosition, GUILayout.Height(150));
//             EditorGUILayout.TextArea(exportedData, GUILayout.ExpandHeight(true));
//             EditorGUILayout.EndScrollView();
//         }
//     }
//
//     private void HandleDragAndDrop(Rect dropArea)
//     {
//         Event currentEvent = Event.current;
//         
//         if (dropArea.Contains(currentEvent.mousePosition))
//         {
//             if (currentEvent.type == EventType.DragUpdated || currentEvent.type == EventType.DragPerform)
//             {
//                 bool hasValidObject = false;
//                 
//                 foreach (var draggedObject in DragAndDrop.objectReferences)
//                 {
//                     if (draggedObject is GameObject gameObject)
//                     {
//                         WoolControl woolControl = gameObject.GetComponent<WoolControl>();
//                         if (woolControl != null)
//                         {
//                             hasValidObject = true;
//                             break;
//                         }
//                     }
//                 }
//
//                 if (hasValidObject)
//                 {
//                     DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
//                     
//                     if (currentEvent.type == EventType.DragPerform)
//                     {
//                         DragAndDrop.AcceptDrag();
//                         
//                         foreach (var draggedObject in DragAndDrop.objectReferences)
//                         {
//                             if (draggedObject is GameObject gameObject)
//                             {
//                                 // Check if not already in list
//                                 bool alreadyExists = woolControlEntries.Any(e => e.gameObject == gameObject);
//                                 if (!alreadyExists)
//                                 {
//                                     var newEntry = new WoolControlColorEntry(gameObject);
//                                     if (colorPalette != null)
//                                     {
//                                         newEntry.UpdateColorFromPalette(colorPalette);
//                                     }
//                                     woolControlEntries.Add(newEntry);
//                                 }
//                             }
//                         }
//                     }
//                 }
//                 else
//                 {
//                     DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
//                 }
//                 
//                 currentEvent.Use();
//             }
//         }
//     }
//
//     private void RefreshAllColors()
//     {
//         foreach (var entry in woolControlEntries)
//         {
//             entry.UpdateWoolControlInfo();
//             if (colorPalette != null)
//             {
//                 entry.UpdateColorFromPalette(colorPalette);
//             }
//         }
//         Debug.Log($"Refreshed colors for {woolControlEntries.Count} WoolControl entries");
//     }
//
//     private void RemoveInvalidEntries()
//     {
//         int initialCount = woolControlEntries.Count;
//         woolControlEntries.RemoveAll(e => e.gameObject == null || !e.hasValidWoolControl);
//         int removedCount = initialCount - woolControlEntries.Count;
//         if (removedCount > 0)
//         {
//             Debug.Log($"Removed {removedCount} invalid entries");
//         }
//     }
//
//     private void FindAllWoolControlsInScene()
//     {
//         WoolControl[] allWoolControls = FindObjectsOfType<WoolControl>();
//         int addedCount = 0;
//         
//         foreach (var woolControl in allWoolControls)
//         {
//             GameObject go = woolControl.gameObject;
//             bool alreadyExists = woolControlEntries.Any(e => e.gameObject == go);
//             
//             if (!alreadyExists)
//             {
//                 var newEntry = new WoolControlColorEntry(go);
//                 if (colorPalette != null)
//                 {
//                     newEntry.UpdateColorFromPalette(colorPalette);
//                 }
//                 if (newEntry.hasValidWoolControl)
//                 {
//                     woolControlEntries.Add(newEntry);
//                     addedCount++;
//                 }
//             }
//         }
//         
//         Debug.Log($"Found and added {addedCount} new WoolControl components");
//     }
//
//     private void ExportForWoolControlColorSetter()
//     {
//         if (colorPalette == null)
//         {
//             Debug.LogError("Cannot export without a ColorPalleteData assigned!");
//             return;
//         }
//
//         StringBuilder sb = new StringBuilder();
//         
//         if (includeComments)
//         {
//             sb.AppendLine("# WoolControl Color Data - Compatible with WoolControlColorSetter");
//             sb.AppendLine($"# Extracted from ColorPalleteData: {colorPalette.name}");
//             sb.AppendLine($"# Generated on: {System.DateTime.Now}");
//             sb.AppendLine("# Format: ObjectName | ColorHex | ColorRGBA");
//             sb.AppendLine();
//         }
//         
//         var validEntries = woolControlEntries.Where(e => 
//             e.gameObject != null && 
//             e.hasValidWoolControl && 
//             e.hasValidColorKey &&
//             colorPalette.colorPallete.ContainsKey(e.colorKey)).ToList();
//         
//         if (sortByName)
//         {
//             validEntries = validEntries.OrderBy(e => e.gameObject.name).ToList();
//         }
//         
//         foreach (var entry in validEntries)
//         {
//             string colorHex = "#" + ColorUtility.ToHtmlStringRGBA(entry.extractedColor);
//             
//             if (includeRGBAValues)
//             {
//                 sb.AppendLine($"{entry.gameObject.name} | {colorHex} | {entry.extractedColor}");
//             }
//             else
//             {
//                 sb.AppendLine($"{entry.gameObject.name} | {colorHex}");
//             }
//         }
//         
//         if (includeComments)
//         {
//             sb.AppendLine();
//             sb.AppendLine($"# Total objects exported: {validEntries.Count}");
//             sb.AppendLine($"# Color palette: {colorPalette.name} ({colorPalette.colorPallete.Count} colors)");
//             
//             // Color key statistics
//             var colorKeyStats = validEntries.GroupBy(e => e.colorKey)
//                                           .OrderByDescending(g => g.Count());
//             sb.AppendLine("# Color key usage:");
//             foreach (var group in colorKeyStats)
//             {
//                 sb.AppendLine($"#   {group.Key}: {group.Count()} objects");
//             }
//         }
//         
//         exportedData = sb.ToString();
//         
//         // Save to file
//         string path = EditorUtility.SaveFilePanel("Save WoolControl Color Data", "", exportFileName, "txt");
//         if (!string.IsNullOrEmpty(path))
//         {
//             System.IO.File.WriteAllText(path, exportedData);
//             Debug.Log($"Saved WoolControl color data to: {path}");
//         }
//         
//         Debug.Log($"Exported color data for {validEntries.Count} WoolControl objects");
//     }
//
//     private void ExportAsJSON()
//     {
//         if (colorPalette == null)
//         {
//             Debug.LogError("Cannot export without a ColorPalleteData assigned!");
//             return;
//         }
//
//         var database = new WoolColorDatabase1();
//         
//         var validEntries = woolControlEntries.Where(e => 
//             e.gameObject != null && 
//             e.hasValidWoolControl && 
//             e.hasValidColorKey &&
//             colorPalette.colorPallete.ContainsKey(e.colorKey));
//         
//         foreach (var entry in validEntries)
//         {
//             database.colorDataList.Add(new WoolColorData1(entry.gameObject.name, entry.extractedColor));
//         }
//         
//         string json = JsonUtility.ToJson(database, true);
//         exportedData = json;
//         
//         // Save JSON file
//         string path = EditorUtility.SaveFilePanel("Save Color Data as JSON", "", exportFileName, "json");
//         if (!string.IsNullOrEmpty(path))
//         {
//             System.IO.File.WriteAllText(path, json);
//             Debug.Log($"Saved JSON color data to: {path}");
//         }
//     }
//
//     private void OnSelectionChange()
//     {
//         if (autoRefreshColors)
//         {
//             // Auto-add selected objects if they have WoolControl components
//             foreach (var selectedObject in Selection.gameObjects)
//             {
//                 if (selectedObject.GetComponent<WoolControl>() != null)
//                 {
//                     bool alreadyExists = woolControlEntries.Any(e => e.gameObject == selectedObject);
//                     if (!alreadyExists)
//                     {
//                         var newEntry = new WoolControlColorEntry(selectedObject);
//                         if (colorPalette != null)
//                         {
//                             newEntry.UpdateColorFromPalette(colorPalette);
//                         }
//                         if (newEntry.hasValidWoolControl)
//                         {
//                             woolControlEntries.Add(newEntry);
//                         }
//                     }
//                 }
//             }
//             
//             Repaint();
//         }
//     }
// }
//
// // ========== COMPATIBILITY CLASSES ==========
// // These classes ensure compatibility with WoolControlColorSetter
//
// [System.Serializable]
// public class WoolColorData1
// {
//     public string objectName;
//     public string colorHex;
//     public Color color;
//     
//     public WoolColorData1(string name, Color col)
//     {
//         objectName = name;
//         color = col;
//         colorHex = "#" + ColorUtility.ToHtmlStringRGBA(col);
//     }
// }
//
// [System.Serializable]
// public class WoolColorDatabase1
// {
//     public List<WoolColorData1> colorDataList = new List<WoolColorData1>();
// }