using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ColorPaletteImportExportTool : EditorWindow
{
    private ColorPalleteData_new colorPaletteData;
    private string jsonPath = "Assets/_Game/Scripts/DataSO/ColorPallete/ColorPaletteData.json";
    private string materialSearchFolder = "Assets/Kho/Wool_new/Materials";

    [MenuItem("Tools/Color Palette Import/Export")]
    public static void ShowWindow()
    {
        GetWindow<ColorPaletteImportExportTool>("Color Palette Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Color Palette Import/Export Settings", EditorStyles.boldLabel);

        colorPaletteData = (ColorPalleteData_new)EditorGUILayout.ObjectField(
            "Color Palette Data", colorPaletteData, typeof(ColorPalleteData_new), false);

        EditorGUILayout.Space();

        GUILayout.Label("Paths", EditorStyles.boldLabel);
        jsonPath = EditorGUILayout.TextField("JSON Path", jsonPath);
        materialSearchFolder = EditorGUILayout.TextField("Material Folder", materialSearchFolder);

        EditorGUILayout.Space();

        GUILayout.Label("Export", EditorStyles.boldLabel);
        if (GUILayout.Button("Export to JSON (from SO)"))
        {
            ExportToJson();
        }

        if (GUILayout.Button("Generate JSON from Material Folder"))
        {
            GenerateJsonFromMaterials();
        }

        EditorGUILayout.Space();

        GUILayout.Label("Import", EditorStyles.boldLabel);
        if (GUILayout.Button("Import from JSON"))
        {
            ImportFromJson();
        }

        EditorGUILayout.Space();

        if (colorPaletteData != null && colorPaletteData.m_colorPallete != null)
        {
            GUILayout.Label($"Current Colors: {colorPaletteData.m_colorPallete.Length}", EditorStyles.miniLabel);
        }
    }

    private void GenerateJsonFromMaterials()
    {
        if (!Directory.Exists(materialSearchFolder))
        {
            EditorUtility.DisplayDialog("Error", $"Material folder not found: {materialSearchFolder}", "OK");
            return;
        }

        try
        {
            JsonData jsonData = new JsonData();
            jsonData.data = new List<JsonColorEntry>();

            // Find all .mat files in folder
            var matFiles = Directory.GetFiles(materialSearchFolder, "*.mat");
            
            foreach (var matFile in matFiles)
            {
                string relativePath = matFile.Replace("\\", "/");
                string fileName = Path.GetFileNameWithoutExtension(matFile);
                
                // Extract color name from filename (e.g., "M_woolnew_black" -> "black")
                string colorName = ExtractColorName(fileName);
                
                if (!string.IsNullOrEmpty(colorName))
                {
                    jsonData.data.Add(new JsonColorEntry
                    {
                        colorName = colorName,
                        material = new JsonMaterialRef { path = relativePath }
                    });
                }
            }

            // Sort by colorName
            jsonData.data = jsonData.data.OrderBy(x => x.colorName).ToList();

            // Serialize and write to file
            string json = JsonUtility.ToJson(jsonData, true);
            File.WriteAllText(jsonPath, json);

            AssetDatabase.Refresh();
            Debug.Log($"<color=green>Generated JSON with {jsonData.data.Count} colors from {materialSearchFolder}</color>");
            EditorUtility.DisplayDialog("Success", $"Generated {jsonData.data.Count} colors from materials", "OK");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error generating JSON: {e.Message}");
            EditorUtility.DisplayDialog("Error", $"Generate failed: {e.Message}", "OK");
        }
    }

    private string ExtractColorName(string fileName)
    {
        // M_woolnew_black -> black
        // M_woolnew_blue_dark -> blue_dark
        if (fileName.StartsWith("M_woolnew_"))
        {
            return fileName.Substring("M_woolnew_".Length);
        }
        return fileName;
    }

    private void ExportToJson()
    {
        if (colorPaletteData == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign ColorPalleteData_new", "OK");
            return;
        }

        if (colorPaletteData.m_colorPallete == null || colorPaletteData.m_colorPallete.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", "ColorPalleteData_new has no colors", "OK");
            return;
        }

        try
        {
            // Build JSON data
            JsonData jsonData = new JsonData();
            jsonData.data = new List<JsonColorEntry>();

            foreach (var colorEntry in colorPaletteData.m_colorPallete)
            {
                string materialPath = AssetDatabase.GetAssetPath(colorEntry.material);
                jsonData.data.Add(new JsonColorEntry
                {
                    colorName = colorEntry.colorName,
                    material = new JsonMaterialRef { path = materialPath }
                });
            }

            // Serialize and write to file
            string json = JsonUtility.ToJson(jsonData, true);
            File.WriteAllText(jsonPath, json);

            AssetDatabase.Refresh();
            Debug.Log($"<color=green>Successfully exported {jsonData.data.Count} colors to {jsonPath}</color>");
            EditorUtility.DisplayDialog("Success", $"Exported {jsonData.data.Count} colors to JSON", "OK");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error exporting: {e.Message}");
            EditorUtility.DisplayDialog("Error", $"Export failed: {e.Message}", "OK");
        }
    }

    private void ImportFromJson()
    {
        if (colorPaletteData == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign ColorPalleteData_new", "OK");
            return;
        }

        if (!File.Exists(jsonPath))
        {
            EditorUtility.DisplayDialog("Error", $"JSON file not found: {jsonPath}", "OK");
            return;
        }

        try
        {
            // Read and parse JSON
            string jsonContent = File.ReadAllText(jsonPath);
            JsonData jsonData = JsonUtility.FromJson<JsonData>(jsonContent);

            if (jsonData.data == null)
            {
                EditorUtility.DisplayDialog("Error", "Invalid JSON format", "OK");
                return;
            }

            // Build color palette list
            var colorList = new List<ColorPallete_New>();
            int successCount = 0;
            int failCount = 0;

            foreach (var jsonEntry in jsonData.data)
            {
                if (string.IsNullOrEmpty(jsonEntry.colorName))
                {
                    Debug.LogWarning("Skipping entry with empty colorName");
                    failCount++;
                    continue;
                }

                Material material = null;
                if (!string.IsNullOrEmpty(jsonEntry.material.path))
                {
                    material = AssetDatabase.LoadAssetAtPath<Material>(jsonEntry.material.path);
                    if (material == null)
                    {
                        Debug.LogWarning($"Material not found at path: {jsonEntry.material.path}");
                        failCount++;
                    }
                    else
                    {
                        successCount++;
                    }
                }

                colorList.Add(new ColorPallete_New
                {
                    colorName = jsonEntry.colorName,
                    material = material
                });
            }

            // Update ScriptableObject
            Undo.RecordObject(colorPaletteData, "Import Color Palette");
            colorPaletteData.m_colorPallete = colorList.ToArray();
            EditorUtility.SetDirty(colorPaletteData);

            Debug.Log($"<color=green>Successfully imported {successCount} colors from {jsonPath}</color>");
            if (failCount > 0)
            {
                Debug.LogWarning($"<color=yellow>Failed to load {failCount} materials</color>");
            }

            EditorUtility.DisplayDialog("Success", 
                $"Imported {successCount} colors\nFailed: {failCount}", "OK");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error importing: {e.Message}");
            EditorUtility.DisplayDialog("Error", $"Import failed: {e.Message}", "OK");
        }
    }

    [System.Serializable]
    private class JsonData
    {
        public List<JsonColorEntry> data;
    }

    [System.Serializable]
    private class JsonColorEntry
    {
        public string colorName;
        public JsonMaterialRef material;
    }

    [System.Serializable]
    private class JsonMaterialRef
    {
        public string path;
    }
}
