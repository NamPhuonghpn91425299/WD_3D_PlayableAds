// using UnityEngine;
// using UnityEditor;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
//
// // ========== DATA STRUCTURES ==========
//
// [System.Serializable]
// public class WoolControlColorKeyEntry
// {
//     public GameObject gameObject;
//     public WoolControl woolControl;
//     public string colorKey = "";
//     public bool hasValidWoolControl = false;
//     public bool hasValidColorKey = false;
//     
//     public WoolControlColorKeyEntry(GameObject go)
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
//         }
//     }
// }
//
// // ========== MAIN TOOL ==========
//
// public class WoolControlColorKeyExtractor : EditorWindow
// {
//     private List<WoolControlColorKeyEntry> woolControlEntries = new List<WoolControlColorKeyEntry>();
//     private Vector2 scrollPosition;
//     private string exportedData = "";
//     private Vector2 dataScrollPosition;
//     private string exportFileName = "WoolControlColorKeys";
//     
//     // Filter options
//     private bool showOnlyValidWoolControls = true;
//     private string colorKeyFilter = "";
//     
//     // Export options
//     private bool includeComments = true;
//     private bool sortByName = false;
//     private bool includeAdditionalInfo = true;
//
//     [MenuItem("Tools/WoolControl Color Key Extractor")]
//     public static void ShowWindow()
//     {
//         GetWindow<WoolControlColorKeyExtractor>("WoolControl Color Key Extractor");
//     }
//
//     private void OnGUI()
//     {
//         EditorGUILayout.LabelField("WoolControl Color Key Extractor", EditorStyles.boldLabel);
//         EditorGUILayout.HelpBox("Extract HightestColor keys from WoolControl components and export in compatible format", MessageType.Info);
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
//         showOnlyValidWoolControls = EditorGUILayout.Toggle("Show Only Valid WoolControls", showOnlyValidWoolControls);
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
//         includeAdditionalInfo = EditorGUILayout.Toggle("Include Additional Info", includeAdditionalInfo);
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
//         if (GUILayout.Button("Refresh All"))
//         {
//             RefreshAllEntries();
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
//         int validColorKeyCount = woolControlEntries.Count(e => e.hasValidColorKey);
//         
//         EditorGUILayout.LabelField($"Objects: {totalCount} ({validCount} with WoolControl, {validColorKeyCount} with valid color keys)", EditorStyles.miniLabel);
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
//     private void DrawWoolControlEntry(WoolControlColorKeyEntry entry, int index)
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
//         // Color key field (read-only)
//         EditorGUI.BeginDisabledGroup(true);
//         EditorGUILayout.TextField(entry.colorKey, GUILayout.Width(120));
//         EditorGUI.EndDisabledGroup();
//         
//         // Refresh button
//         if (GUILayout.Button("↻", GUILayout.Width(25)))
//         {
//             entry.UpdateWoolControlInfo();
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
//             string status = entry.hasValidColorKey ? "✓ Valid" : "✗ Empty";
//             Color statusColor = entry.hasValidColorKey ? Color.green : Color.red;
//             
//             var oldColor = GUI.color;
//             GUI.color = statusColor;
//             EditorGUILayout.LabelField(status, EditorStyles.miniLabel, GUILayout.Width(60));
//             GUI.color = oldColor;
//             
//             EditorGUILayout.EndHorizontal();
//             
//             // Show additional WoolControl info
//             if (entry.woolControl.MeshObjectData != null && includeAdditionalInfo)
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
//         if (GUILayout.Button("Export Color Keys"))
//         {
//             ExportColorKeys();
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
//                                     var newEntry = new WoolControlColorKeyEntry(gameObject);
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
//     private void RefreshAllEntries()
//     {
//         foreach (var entry in woolControlEntries)
//         {
//             entry.UpdateWoolControlInfo();
//         }
//         Debug.Log($"Refreshed {woolControlEntries.Count} WoolControl entries");
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
//                 var newEntry = new WoolControlColorKeyEntry(go);
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
//     private void ExportColorKeys()
//     {
//         StringBuilder sb = new StringBuilder();
//         
//         if (includeComments)
//         {
//             sb.AppendLine("# WoolControl Color Key Data");
//             sb.AppendLine($"# Generated on: {System.DateTime.Now}");
//             sb.AppendLine("# Format: ObjectName | ColorKey");
//             sb.AppendLine();
//         }
//         
//         var validEntries = woolControlEntries.Where(e => 
//             e.gameObject != null && 
//             e.hasValidWoolControl && 
//             e.hasValidColorKey).ToList();
//         
//         if (sortByName)
//         {
//             validEntries = validEntries.OrderBy(e => e.gameObject.name).ToList();
//         }
//         
//         foreach (var entry in validEntries)
//         {
//             if (includeAdditionalInfo && entry.woolControl.MeshObjectData != null)
//             {
//                 sb.AppendLine($"{entry.gameObject.name} | {entry.colorKey} | Layers: {entry.woolControl.MeshObjectData.TotalLayer}");
//             }
//             else
//             {
//                 sb.AppendLine($"{entry.gameObject.name} | {entry.colorKey}");
//             }
//         }
//         
//         if (includeComments)
//         {
//             sb.AppendLine();
//             sb.AppendLine($"# Total objects exported: {validEntries.Count}");
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
//         string path = EditorUtility.SaveFilePanel("Save WoolControl Color Key Data", "", exportFileName, "txt");
//         if (!string.IsNullOrEmpty(path))
//         {
//             System.IO.File.WriteAllText(path, exportedData);
//             Debug.Log($"Saved WoolControl color key data to: {path}");
//         }
//         
//         Debug.Log($"Exported color key data for {validEntries.Count} WoolControl objects");
//     }
//
//     private void ExportAsJSON()
//     {
//         var database = new WoolColorKeyDatabase();
//         
//         var validEntries = woolControlEntries.Where(e => 
//             e.gameObject != null && 
//             e.hasValidWoolControl && 
//             e.hasValidColorKey);
//         
//         foreach (var entry in validEntries)
//         {
//             var data = new WoolColorKeyData(entry.gameObject.name, entry.colorKey);
//             if (includeAdditionalInfo && entry.woolControl.MeshObjectData != null)
//             {
//                 data.totalLayers = entry.woolControl.MeshObjectData.TotalLayer;
//                 data.colorStack = new List<string>(entry.woolControl.MeshObjectData.ColorStack);
//             }
//             database.colorKeyDataList.Add(data);
//         }
//         
//         string json = JsonUtility.ToJson(database, true);
//         exportedData = json;
//         
//         // Save JSON file
//         string path = EditorUtility.SaveFilePanel("Save Color Key Data as JSON", "", exportFileName, "json");
//         if (!string.IsNullOrEmpty(path))
//         {
//             System.IO.File.WriteAllText(path, json);
//             Debug.Log($"Saved JSON color key data to: {path}");
//         }
//     }
//
//     private void OnSelectionChange()
//     {
//         // Auto-add selected objects if they have WoolControl components
//         foreach (var selectedObject in Selection.gameObjects)
//         {
//             if (selectedObject.GetComponent<WoolControl>() != null)
//             {
//                 bool alreadyExists = woolControlEntries.Any(e => e.gameObject == selectedObject);
//                 if (!alreadyExists)
//                 {
//                     var newEntry = new WoolControlColorKeyEntry(selectedObject);
//                     if (newEntry.hasValidWoolControl)
//                     {
//                         woolControlEntries.Add(newEntry);
//                     }
//                 }
//             }
//         }
//         
//         Repaint();
//     }
// }
//
// // ========== DATA CLASSES FOR JSON EXPORT ==========
//
// [System.Serializable]
// public class WoolColorKeyData
// {
//     public string objectName;
//     public string colorKey;
//     public int totalLayers;
//     public List<string> colorStack = new List<string>();
//     
//     public WoolColorKeyData(string name, string key)
//     {
//         objectName = name;
//         colorKey = key;
//     }
// }
//
// [System.Serializable]
// public class WoolColorKeyDatabase
// {
//     public List<WoolColorKeyData> colorKeyDataList = new List<WoolColorKeyData>();
// }