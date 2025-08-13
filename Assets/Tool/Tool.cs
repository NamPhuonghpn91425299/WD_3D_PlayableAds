#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class ColorPalleteWindow : EditorWindow
{
    private ColorPalleteData targetData;

    private string searchKey = "";
    private string searchHexColor = "";
    private string foundKey = "";
    private Color foundColor = Color.clear;
    private bool colorFound = false;

    [MenuItem("Tools/Color Pallete Sync Tool")]
    public static void ShowWindow()
    {
        GetWindow<ColorPalleteWindow>("Color Pallete Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Color Pallete Sync Tool", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        targetData = (ColorPalleteData)EditorGUILayout.ObjectField("ColorPalleteData", targetData, typeof(ColorPalleteData), false);

        if (targetData == null)
        {
            EditorGUILayout.HelpBox("Drag a ColorPalleteData asset here.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Sync Dictionary → Lists"))
        {
            SyncDictionaryToLists(targetData);
        }

        if (GUILayout.Button("Sync Lists → Dictionary"))
        {
            SyncListsToDictionary(targetData);
        }

        EditorGUILayout.Space();
        GUILayout.Label("🔍 Tìm kiếm", EditorStyles.boldLabel);

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

    private bool ApproximatelyEqual(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }
}
#endif
