using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfoManager: SingletonBase<LevelInfoManager>
{
    [SerializeField] private LevelConfigSO levelConfigSO;

    public int GetLevelCountTotal()
    {
        return levelConfigSO.Datas.Count;
    }

    public int GetLevelToLoad(int level)
    {
        return levelConfigSO.GetLevelToLoad(level);
    }
}
