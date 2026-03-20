using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/LevelConfig", menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    //[SerializeField] int levelStartLoop = 100;
    [SerializeField] public List<LevelConfigData> levelConfigDataList;

    private string _levelConfigPath => "StreamingAssets/LevelConfigs.json";

    public List<LevelConfigData> Datas => levelConfigDataList;
    public LevelConfigData GetLevelConfigData(int levelId)
    {
        int originalId = levelId;
        levelId = GetLevelToLoad(levelId);
        if (levelId == 0) levelId = levelConfigDataList.Count;

        int index = levelId - 1;
        if (index >= 0 && index < levelConfigDataList.Count)
        {
            var data = levelConfigDataList[index];
            ///Debug.Log($"[LevelConfigSO] Mapping Level {originalId} -> Index {index} (LevelID in data: {data.Level}). Prefab: {data.MainPrefabPath}");
            return data;
        }

        //Debug.LogError($"[LevelConfigSO] Level {originalId} (mapped to index {index}) NOT FOUND! List count: {levelConfigDataList.Count}");
        return null;
    }

    public bool TryAdd(LevelConfigData levelData)
    {
        if (levelConfigDataList == null)
            levelConfigDataList = new List<LevelConfigData>();
        if (levelConfigDataList.Any(x => x.MainPrefabPath.Equals(levelData.MainPrefabPath))) return false;
        levelConfigDataList.Add(levelData);
        return true;
    }

    public void ClearLevels()
    {
        if (levelConfigDataList != null)
        {
            levelConfigDataList.Clear();
        }
    }

    public int GetLevelToLoad(int playerCurrentLevel)
    {
        int levelToLoad = playerCurrentLevel;
        var levelCount = levelConfigDataList.Count;
        // if (levelToLoad > levelCount)
        // {
        //     levelToLoad = (levelToLoad - levelStartLoop) % (levelConfigDataList.Count - levelStartLoop + 1) + levelStartLoop;
        // }

        return levelToLoad;
    }

# if UNITY_EDITOR
    [ContextMenu("Import All Levels")]
    public void ImportLevelsFromJSON()
    {
        string path = "Assets/_Game/Resources_moved/Json";
        if (!System.IO.Directory.Exists(path))
        {
            Debug.LogError($"Directory not found: {path}");
            return;
        }

        string[] files = System.IO.Directory.GetFiles(path, "*.json");
        if (files.Length == 0)
        {
            Debug.LogWarning("No .json files found in " + path);
            return;
        }

        levelConfigDataList ??= new List<LevelConfigData>();
        levelConfigDataList.Clear();

        // Sort files by level number in filename (Level_1.bytes, Level_2.bytes, ...)
        var sortedFiles = files.OrderBy(f =>
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(f);
            if (int.TryParse(fileName.Replace("Level_", ""), out int levelNum))
                return levelNum;
            return int.MaxValue;
        }).ToList();

        foreach (string file in sortedFiles)
        {
            string json = System.IO.File.ReadAllText(file);
            try
            {
                LevelConfigData data = JsonUtility.FromJson<LevelConfigData>(json);
                if (data != null)
                {
                    data.LevelName = $"Level {data.Level} - {System.IO.Path.GetFileNameWithoutExtension(data.MainPrefabPath)}";
                    if (data.CubeTargetCount == 0) data.CubeTargetCount = 2; // Default if not set in JSON or initialized
                    levelConfigDataList.Add(data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing {file}: {e.Message}");
            }
        }

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"Successfully imported {levelConfigDataList.Count} levels into asset [{this.name}] from {path}");
    }
#endif
}

[Serializable]
public class LevelConfigData
{
    public string LevelName; // Add this for Inspector visibility
    public int Level;
    public int GoldReward;
    public string MainPrefabPath;
    public GameObject MainPrefab;
    public LevelType LevelType;
    public int CubeTargetCount; // Number of unlocked cube targets on start
    public List<string> BoxsQueue;
    public List<WoolProperties> WoolPropertiesList;
}

[Serializable]
public class WoolProperties
{
    public long WoolId;
    public int WeightOrder;
    public bool ActiveState;
    public List<string> Color;
}

public enum LevelType
{
    None = 0,
    Normal = 1,
    Medium = 2,
    Hard = 3,
    Extreme = 4
}