using UnityEngine;
using UnityEditor;
using System.Linq;

public class LevelReferenceSetup : EditorWindow
{
    private GameObject levelPrefab;
    private string setupLogs = "";

    [MenuItem("Tools/Level Reference Setup")]
    public static void ShowWindow()
    {
        GetWindow<LevelReferenceSetup>("Level Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Reference Setup Tool", EditorStyles.boldLabel);

        levelPrefab = (GameObject)EditorGUILayout.ObjectField("Level Prefab", levelPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Setup Level"))
        {
            SetupLevel();
        }

        if (!string.IsNullOrEmpty(setupLogs))
        {
            GUILayout.Space(10);
            GUILayout.Label("Logs:", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(setupLogs, MessageType.Info);
        }
    }

    private void SetupLevel()
    {
        setupLogs = "";

        if (levelPrefab == null)
        {
            setupLogs = "Error: Please assign a Level Prefab first.";
            return;
        }

        var gamePlaySystem = FindObjectOfType<GamePlaySystem>();
        if (gamePlaySystem == null)
        {
            setupLogs += "Error: GamePlaySystem not found in scene.\n";
            return;
        }

        var cameraController = FindObjectOfType<CameraController>();
        if (cameraController == null)
        {
            setupLogs += "Error: CameraController not found in scene.\n";
            return;
        }

        // 1. Instantiate in SpawnPoint
        GameObject levelInstance = null;
        if (cameraController.SpawnPoint == null)
        {
            setupLogs += "Error: CameraController.SpawnPoint is null.\n";
            return;
        }
        else
        {
            levelInstance = InstantiateUnderSpawnPoint(cameraController.SpawnPoint);
        }

        if (levelInstance == null)
        {
            setupLogs += "Error: Failed to instantiate level instance.\n";
            return;
        }

        // 2. Assign Instance references (NOT the Prefab Asset)
        SetupGamePlaySystemReference(gamePlaySystem, levelInstance);
        SetupCameraControllerReference(cameraController, levelInstance);

        // 3. Calculate Cube Count
        CalculateAndSetCubeCount(gamePlaySystem, levelInstance);

        EditorUtility.SetDirty(gamePlaySystem);
        EditorUtility.SetDirty(cameraController);

        // Ensure scene is marked dirty so changes persist
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gamePlaySystem.gameObject.scene);

        setupLogs += "Setup Complete! Don't forget to Save the Scene.\n";
    }

    private void SetupGamePlaySystemReference(GamePlaySystem gamePlaySystem, GameObject instance)
    {
        SerializedObject so = new SerializedObject(gamePlaySystem);
        SerializedProperty levelPrefabProp = so.FindProperty("_levelPrefab");
        if (levelPrefabProp != null)
        {
            levelPrefabProp.objectReferenceValue = instance;
            so.ApplyModifiedProperties();
            setupLogs += "Assigned GamePlaySystem._levelPrefab (Scene Instance).\n";
        }
        else
        {
            setupLogs += "Warning: Could not find property '_levelPrefab' in GamePlaySystem.\n";
        }
    }

    private void SetupCameraControllerReference(CameraController cameraController, GameObject instance)
    {
        cameraController.ModelPrefab = instance;
        setupLogs += "Assigned CameraController.ModelPrefab (Scene Instance).\n";
    }

    private GameObject InstantiateUnderSpawnPoint(Transform spawnPoint)
    {
        // Clear children
        var children = new System.Collections.Generic.List<GameObject>();
        foreach (Transform child in spawnPoint) children.Add(child.gameObject);
        foreach (var child in children) Undo.DestroyObjectImmediate(child);

        // Instantiate Prefab
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(levelPrefab, spawnPoint);
        // Transform is preserved from Prefab (User Request)
        Undo.RegisterCreatedObjectUndo(instance, "Instantiate Level Prefab");

        setupLogs += "Instantiated Prefab under SpawnPoint.\n";
        return instance;
    }

    private void CalculateAndSetCubeCount(GamePlaySystem gamePlaySystem, GameObject instance)
    {
        var meshController = instance.GetComponent<GamePlayMeshController>();
        if (meshController == null)
        {
            setupLogs += "Error: GamePlayMeshController component missing on Level Instance.\n";
            return;
        }

        if (meshController.LevelData == null)
        {
            // Try to find LevelData if it's null (sometimes needing logic to init?)
            // But actually LevelData is usually serialized. If it's pure data class, maybe it's fine.
            // Checking source code earlier: LevelData is a public field in GamePlayMeshController.
            setupLogs += "Error: LevelData is null on GamePlayMeshController.\n";
            return;
        }

        int totalColor = 0;
        if (meshController.LevelData.ColorCountList != null)
        {
            foreach (var count in meshController.LevelData.ColorCountList)
            {
                totalColor += count;
            }
        }

        int cubeCount = totalColor / 3;

        // gamePlaySystem.cubeCountClaimed is public [SerializeField]
        gamePlaySystem.cubeCountClaimed = cubeCount;

        setupLogs += $"Calculated TotalColor: {totalColor}. Set cubeCountClaimed to {cubeCount}.\n";
    }

}
