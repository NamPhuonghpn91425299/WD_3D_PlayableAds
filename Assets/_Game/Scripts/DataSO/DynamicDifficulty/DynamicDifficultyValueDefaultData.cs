using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/DynamicDifficulttyData", menuName = "ScriptableObjects/DynamicDifficulttyData")]
public class DynamicDifficultyValueDefaultData : ScriptableObject
{
    [Header("Player facter value")]
    public int PlayerFatorValueIfUseBoosterInPreGame;
    public int PLayerFatorValueIfDontUseBoosterInPreGame;

    [Header("Booster factor value")]
    public    int AdsBoosterFactorValueWhenUseAds;
    
    [Header("Save Booster factor value")]
    public int SavedBoosterFactorValueWhenUseSavedBooster;
    public int MaxSaveBoosterFactor;

    [Header("Used Booster factor value")]
    public int UsedBoosterFactorValueWhenUseBoosterReady;

    [Header("Time change factor value")]
    public int TimeFactorValue;
    public int MaxTimeFactor;

    [Header("Move factor value")]
    public int MoveFaxtorValue;
    public int MaxMoveFactor;

    [Header("Streak factor value")]
    public int StreakFactorValue;
    public int WinStreakCountCheck;
    
    [Header("Offline factor value")]
    public int OfflineFactorValue;
    public int OfflineDayCountCheck;
    
    [Header("Difficulty factor value")]
    public int EaseOfPlayFactorValue;
    public int MediumOfPlayFactorValue;
    public int HardOfPlayFactorValue;
}
