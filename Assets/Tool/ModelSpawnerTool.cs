using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ModelSpawnerTool : EditorWindow
{
    private string prefabFolder = "Assets/_Game/Resources_moved/Levels";
    private string configFolder = "Assets/_Game/Resources_moved/PaintingSetups";

    private List<GameObject> prefabs = new List<GameObject>();
    private string[] prefabNames;
    private int selectedIndex = 0;

    private Transform spawnPoint;

    private PaintingConfig paintingConfig; // dùng đúng type

    // Lưu prefab spawn trước đó để xóa khi spawn mới
    private GameObject lastSpawnedObject;

    [MenuItem("Tools/Model Spawner Tool")]
    public static void ShowWindow()
    {
        GetWindow<ModelSpawnerTool>("Model Spawner Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Model Spawner Tool", EditorStyles.boldLabel);

        // Prefab folder
        prefabFolder = EditorGUILayout.TextField("Prefab Folder", prefabFolder);
        configFolder = EditorGUILayout.TextField("Config Folder", configFolder);

        if (GUILayout.Button("Load Prefabs from Folder"))
        {
            LoadPrefabs();
        }

        if (prefabs.Count > 0)
        {
            int newIndex = EditorGUILayout.Popup("Select Model", selectedIndex, prefabNames);
            if (newIndex != selectedIndex)
            {
                selectedIndex = newIndex;
                LoadPaintingConfigForPrefab(prefabNames[selectedIndex]);
            }
        }

        // Spawn point
        spawnPoint = (Transform)EditorGUILayout.ObjectField("Spawn Point", spawnPoint, typeof(Transform), true);

        // Hiển thị config
        if (paintingConfig != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Loaded Config:", paintingConfig.name);
            EditorGUILayout.ObjectField("Config Asset", paintingConfig, typeof(PaintingConfig), false);
        }

        if (GUILayout.Button("Spawn Selected Prefab"))
        {
            SpawnPrefab();
        }
    }

    private void LoadPrefabs()
    {
        prefabs.Clear();

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolder });
        prefabNames = new string[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                prefabs.Add(prefab);
                prefabNames[i] = prefab.name;
            }
        }

        if (prefabs.Count == 0)
        {
            Debug.LogWarning("No prefabs found in folder: " + prefabFolder);
        }
    }

    private void LoadPaintingConfigForPrefab(string prefabName)
    {
        string configPath = $"{configFolder}/{prefabName}_PaintingConfig.asset";
        paintingConfig = AssetDatabase.LoadAssetAtPath<PaintingConfig>(configPath);

        if (paintingConfig == null)
        {
            Debug.LogWarning($"Could not find config for {prefabName} at {configPath}");
        }
        else
        {
            Debug.Log($"Loaded PaintingConfig for {prefabName}: {paintingConfig.name}");
        }
    }

    private void SpawnPrefab()
    {
        if (prefabs.Count == 0 || spawnPoint == null)
        {
            Debug.LogWarning("No prefab selected or spawn point not set!");
            return;
        }

        // ❌ Xóa prefab cũ nếu có
        if (lastSpawnedObject != null)
        {
            Undo.DestroyObjectImmediate(lastSpawnedObject);
            lastSpawnedObject = null;
        }

        // ✅ Spawn prefab mới
        GameObject prefab = prefabs[selectedIndex];
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        obj.transform.SetParent(spawnPoint, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        Undo.RegisterCreatedObjectUndo(obj, "Spawn Prefab");
        Selection.activeObject = obj;

        lastSpawnedObject = obj;

        Debug.Log($"Spawned prefab: {prefab.name} as child of {spawnPoint.name}");

        // 🔥 Gán config vào SimplePaintingController trong scene
        if (paintingConfig != null)
        {
            SimplePaintingController controller = Object.FindObjectOfType<SimplePaintingController>();
            FishScalesPaintingController fishScalesPaintingController = Object.FindObjectOfType<FishScalesPaintingController>();
            CameraController cameraController = Object.FindObjectOfType<CameraController>();
            GamePlaySystem gamePlaySystem = Object.FindObjectOfType<GamePlaySystem>();

            List<int> colors = obj.GetComponent<GamePlayMeshController>().LevelData.ColorCountList;
            int sum = 0;
            foreach (var count in colors)
            {
                sum += count;
            }
            gamePlaySystem.cubeCountClaimed = sum / 3;
            cameraController.ModelPrefab = obj;
            gamePlaySystem._levelPrefab = obj;
            fishScalesPaintingController.CurrentLevelPrefab = obj.GetComponent<GamePlayMeshController>();

            if (controller != null)
            {
                Undo.RecordObject(controller, "Assign PaintingConfig");
                controller.CurrentPaintingConfig = paintingConfig;
                EditorUtility.SetDirty(controller);

                Debug.Log($"Assigned config {paintingConfig.name} to {controller.gameObject.name}");
            }
            else
            {
                Debug.LogWarning("No SimplePaintingController found in scene!");
            }

            if (fishScalesPaintingController != null)
            {
                Undo.RecordObject(fishScalesPaintingController, "Assign PaintingConfig");
                fishScalesPaintingController.CurrentPaintingConfig = paintingConfig;
                EditorUtility.SetDirty(fishScalesPaintingController);

                Debug.Log($"Assigned config {paintingConfig.name} to {fishScalesPaintingController.gameObject.name}");
            }
            else
            {
                Debug.LogWarning("No SimplePaintingController found in scene!");
            }
        }
    }
}
