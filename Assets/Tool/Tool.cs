#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class ColorPalleteWindow : EditorWindow
{
    private ColorPalleteData targetData;

    private string searchKey = "";
    private string searchHexColor = "";
    private string foundKey = "";
    private Color foundColor = Color.clear;
    private bool colorFound = false;

    // Multi-search functionality
    [SerializeField]
    private List<string> searchKeysList = new List<string>();
    [SerializeField]
    private List<Color> foundColorsList = new List<Color>();
    [SerializeField]
    private List<string> foundKeysList = new List<string>();
    private Vector2 searchListScrollPos;
    private Vector2 resultListScrollPos;
    
    // Serialized objects for proper list display
    private SerializedObject serializedObject;
    private SerializedProperty searchKeysProperty;
    private SerializedProperty foundColorsProperty;

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        searchKeysProperty = serializedObject.FindProperty("searchKeysList");
        foundColorsProperty = serializedObject.FindProperty("foundColorsList");
    }

    [MenuItem("Tools/Color Pallete Sync Tool")]
    public static void ShowWindow()
    {
        GetWindow<ColorPalleteWindow>("Color Pallete Tool");
    }

    private void OnGUI()
    {
        if (serializedObject == null)
        {
            serializedObject = new SerializedObject(this);
            searchKeysProperty = serializedObject.FindProperty("searchKeysList");
            foundColorsProperty = serializedObject.FindProperty("foundColorsList");
        }

        serializedObject.Update();

        GUILayout.Label("Color Pallete Sync Tool", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        targetData = (ColorPalleteData)EditorGUILayout.ObjectField("ColorPalleteData", targetData, typeof(ColorPalleteData), false);

        if (targetData == null)
        {
            EditorGUILayout.HelpBox("Drag a ColorPalleteData asset here.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();

        // Sync buttons
        if (GUILayout.Button("Sync Dictionary → Lists"))
        {
            SyncDictionaryToLists(targetData);
        }

        if (GUILayout.Button("Sync Lists → Dictionary"))
        {
            SyncListsToDictionary(targetData);
        }

        EditorGUILayout.Space();
        
        // Single search section
        GUILayout.Label("🔍 Tìm kiếm đơn", EditorStyles.boldLabel);

        searchKey = EditorGUILayout.TextField("Tìm theo tên (key):", searchKey);
        if (GUILayout.Button("Tìm màu từ key"))
        {
            FindColorByKey(searchKey);
        }

        searchHexColor = EditorGUILayout.TextField("Tìm theo mã màu (#rrggbb):", searchHexColor);
        if (GUILayout.Button("Tìm key từ mã màu"))
        {
            FindKeyByColorHex(searchHexColor);
        }

        if (colorFound)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("🎯 Kết quả:");
            EditorGUILayout.LabelField("Tên:", foundKey);
            EditorGUILayout.ColorField("Màu:", foundColor);

            if (GUILayout.Button("📋 Copy màu vào clipboard"))
            {
                CopyColorToClipboard(foundKey);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // Multi-search section
        GUILayout.Label("🔍 Tìm kiếm nhiều màu", EditorStyles.boldLabel);
        
        DrawSearchKeysList();
        
        EditorGUILayout.Space();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("🔍 Tìm tất cả màu"))
        {
            SearchMultipleColors();
        }
        if (GUILayout.Button("🗑️ Xóa danh sách"))
        {
            searchKeysList.Clear();
            foundColorsList.Clear();
            foundKeysList.Clear();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        DrawFoundColorsList();
        
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSearchKeysList()
    {
        EditorGUILayout.Space();
        
        // Draw the search keys list exactly like Inspector
        searchListScrollPos = EditorGUILayout.BeginScrollView(searchListScrollPos, GUILayout.Height(150));
        EditorGUILayout.PropertyField(searchKeysProperty, new GUIContent("🔍 Danh sách key tìm kiếm"), true);
        EditorGUILayout.EndScrollView();
        
        // Copy/Paste buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("📋 Copy List"))
        {
            CopySearchListToClipboard();
        }
        if (GUILayout.Button("📄 Paste List"))
        {
            PasteSearchListFromClipboard();
        }
        if (GUILayout.Button("🗑️ Clear List"))
        {
            searchKeysList.Clear();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawFoundColorsList()
    {
        if (foundColorsList.Count == 0 && foundKeysList.Count == 0) return;

        EditorGUILayout.Space();
        
        // Draw the found colors list exactly like Inspector
        resultListScrollPos = EditorGUILayout.BeginScrollView(resultListScrollPos, GUILayout.Height(250));
        
        // Draw as proper Inspector list
        EditorGUI.BeginDisabledGroup(true); // Make it read-only since it's search results
        EditorGUILayout.PropertyField(foundColorsProperty, new GUIContent("🎯 Kết quả tìm kiếm (Colors)"), true);
        EditorGUI.EndDisabledGroup();
        
        // Draw corresponding keys list (read-only)
        if (foundKeysList.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("📝 Tên keys tương ứng:", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            for (int i = 0; i < foundKeysList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Element {i}", GUILayout.Width(80));
                EditorGUILayout.TextField(foundKeysList[i]);
                if (GUILayout.Button("📋", GUILayout.Width(25)))
                {
                    EditorGUIUtility.systemCopyBuffer = foundKeysList[i];
                    Debug.Log($"📋 Copied key '{foundKeysList[i]}' to clipboard");
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();
        }
        
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.Space();
        
        // Copy buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("📋 Copy All Keys"))
        {
            string allKeys = string.Join("\n", foundKeysList);
            EditorGUIUtility.systemCopyBuffer = allKeys;
            Debug.Log($"📋 Copied all keys to clipboard");
        }
        
        if (GUILayout.Button("📋 Copy All Hex"))
        {
            var allHex = foundColorsList.Select(c => $"#{ColorUtility.ToHtmlStringRGB(c)}");
            string allHexString = string.Join("\n", allHex);
            EditorGUIUtility.systemCopyBuffer = allHexString;
            Debug.Log($"📋 Copied all hex values to clipboard");
        }
        
        if (GUILayout.Button("📋 Copy Color List"))
        {
            CopyColorListToClipboard();
        }
        
        if (GUILayout.Button("📄 Paste Colors"))
        {
            PasteColorListFromClipboard();
        }
        EditorGUILayout.EndHorizontal();
        
        // Clear results button
        if (GUILayout.Button("🗑️ Clear Results"))
        {
            foundColorsList.Clear();
            foundKeysList.Clear();
        }
    }

    private void SearchMultipleColors()
    {
        foundColorsList.Clear();
        foundKeysList.Clear();
        
        foreach (string key in searchKeysList)
        {
            if (string.IsNullOrEmpty(key)) continue;
            
            int index = targetData.colorKeys.IndexOf(key);
            if (index >= 0 && index < targetData.colorsValues.Count)
            {
                foundKeysList.Add(key);
                foundColorsList.Add(targetData.colorsValues[index]);
            }
            else
            {
                Debug.LogWarning($"❌ Key '{key}' not found in colorKeys list.");
            }
        }
        
        Debug.Log($"✅ Found {foundColorsList.Count} colors from {searchKeysList.Count} search keys.");
    }

    private void SyncDictionaryToLists(ColorPalleteData data)
    {
        data.colorKeys.Clear();
        data.colorsValues.Clear();

        foreach (var kvp in data.colorPallete)
        {
            data.colorKeys.Add(kvp.Key);
            data.colorsValues.Add(kvp.Value);
        }

        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        Debug.Log("✅ Synced dictionary to lists.");
    }

    private void SyncListsToDictionary(ColorPalleteData data)
    {
        data.colorPallete.Clear();

        int count = Mathf.Min(data.colorKeys.Count, data.colorsValues.Count);
        for (int i = 0; i < count; i++)
        {
            if (!data.colorPallete.ContainsKey(data.colorKeys[i]))
                data.colorPallete.Add(data.colorKeys[i], data.colorsValues[i]);
        }

        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        Debug.Log("✅ Synced lists to dictionary.");
    }

    private void FindColorByKey(string key)
    {
        int index = targetData.colorKeys.IndexOf(key);
        if (index >= 0 && index < targetData.colorsValues.Count)
        {
            foundKey = key;
            foundColor = targetData.colorsValues[index];
            colorFound = true;
            Debug.Log($"✅ Found color at index {index} for key '{key}': {foundColor}");
        }
        else
        {
            colorFound = false;
            Debug.LogWarning($"❌ Key '{key}' not found in colorKeys list.");
        }
    }

    private void FindKeyByColorHex(string hex)
    {
        if (!hex.StartsWith("#")) hex = "#" + hex;

        if (ColorUtility.TryParseHtmlString(hex, out Color colorToFind))
        {
            for (int i = 0; i < targetData.colorsValues.Count; i++)
            {
                if (ApproximatelyEqual(targetData.colorsValues[i], colorToFind))
                {
                    foundKey = i < targetData.colorKeys.Count ? targetData.colorKeys[i] : "[Không có key]";
                    foundColor = targetData.colorsValues[i];
                    colorFound = true;
                    Debug.Log($"✅ Found key '{foundKey}' at index {i} for color {hex}");
                    return;
                }
            }

            colorFound = false;
            Debug.LogWarning($"❌ Không tìm thấy key nào với mã màu {hex}");
        }
        else
        {
            colorFound = false;
            Debug.LogWarning($"❌ Mã màu không hợp lệ: {hex}");
        }
    }

    private void CopyColorToClipboard(string color)
    {
        EditorGUIUtility.systemCopyBuffer = color;
        Debug.Log($"📋 Copied {color} to clipboard");
    }

    private void CopySearchListToClipboard()
    {
        string listData = string.Join("\n", searchKeysList.Where(s => !string.IsNullOrEmpty(s)));
        EditorGUIUtility.systemCopyBuffer = listData;
        Debug.Log($"📋 Copied search list to clipboard ({searchKeysList.Count} items)");
    }

    private void PasteSearchListFromClipboard()
    {
        string clipboardText = EditorGUIUtility.systemCopyBuffer;
        if (!string.IsNullOrEmpty(clipboardText))
        {
            string[] lines = clipboardText.Split('\n');
            searchKeysList.Clear();
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    searchKeysList.Add(trimmedLine);
                }
            }
            Debug.Log($"📄 Pasted {searchKeysList.Count} items from clipboard");
        }
        else
        {
            Debug.LogWarning("❌ Clipboard is empty or invalid");
        }
    }

    private void CopyColorListToClipboard()
    {
        List<string> colorData = new List<string>();
        for (int i = 0; i < foundColorsList.Count; i++)
        {
            string hexValue = ColorUtility.ToHtmlStringRGB(foundColorsList[i]);
            colorData.Add($"#{hexValue}");
        }
        
        string listData = string.Join("\n", colorData);
        EditorGUIUtility.systemCopyBuffer = listData;
        Debug.Log($"📋 Copied color list to clipboard ({foundColorsList.Count} colors)");
    }

    private void PasteColorListFromClipboard()
    {
        string clipboardText = EditorGUIUtility.systemCopyBuffer;
        if (!string.IsNullOrEmpty(clipboardText))
        {
            string[] lines = clipboardText.Split('\n');
            foundColorsList.Clear();
            foundKeysList.Clear();
            
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    // Try to parse as hex color
                    if (trimmedLine.StartsWith("#") || trimmedLine.Length == 6 || trimmedLine.Length == 8)
                    {
                        string hexColor = trimmedLine.StartsWith("#") ? trimmedLine : "#" + trimmedLine;
                        if (ColorUtility.TryParseHtmlString(hexColor, out Color parsedColor))
                        {
                            foundColorsList.Add(parsedColor);
                            foundKeysList.Add($"Pasted_{foundColorsList.Count - 1}");
                        }
                    }
                    // Try to parse as "key:hex" format
                    else if (trimmedLine.Contains(":"))
                    {
                        string[] parts = trimmedLine.Split(':');
                        if (parts.Length >= 2)
                        {
                            string key = parts[0].Trim();
                            string hex = parts[1].Trim();
                            if (!hex.StartsWith("#")) hex = "#" + hex;
                            
                            if (ColorUtility.TryParseHtmlString(hex, out Color parsedColor))
                            {
                                foundColorsList.Add(parsedColor);
                                foundKeysList.Add(key);
                            }
                        }
                    }
                }
            }
            Debug.Log($"📄 Pasted {foundColorsList.Count} colors from clipboard");
        }
        else
        {
            Debug.LogWarning("❌ Clipboard is empty or invalid");
        }
    }

    private bool ApproximatelyEqual(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }
}
#endif