using System.Collections.Generic;
using UnityEngine;

public class ConfigHelper : SingletonBase<ConfigHelper>
{
    [SerializeField] private LevelConfigSO levelConfigSO;

    public bool IsLoopLevel(int level)
    {
        if (levelConfigSO == null) return false;
        if (levelConfigSO.levelConfigDataList == null || levelConfigSO.levelConfigDataList.Count == 0) return false;
        return level > levelConfigSO.levelConfigDataList.Count;
    }

    private Dictionary<int, LevelConfigData> _levelConfigCache = new Dictionary<int, LevelConfigData>();

    public LevelConfigData GetLevelConfigData(int level)
    {
        if (_levelConfigCache.TryGetValue(level, out var cachedData))
        {
            return cachedData;
        }

        if (levelConfigSO == null)
        {
            Debug.LogError("LevelConfigSO is null");
            return null;
        }

        var data = levelConfigSO.GetLevelConfigData(level);
        if (data != null)
        {
            _levelConfigCache[level] = data;
        }
        return data;
    }
}
