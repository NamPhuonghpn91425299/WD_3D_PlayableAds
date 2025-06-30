using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class T_Utitilies
{
}


public struct ColorDistribution
{
    public Color Color;
    public int   MeshIndex;
    public int   LayerIndex;
    public bool  IsSetted;
}

[Serializable]
public class MeshObjectData
{
    public int         TotalLayer;
    public Color       HightestColor;
    public List<Color> ColorStack = new List<Color>();
}

[Serializable]
public struct SupportData
{
    public int    SupportId;
    public int    SupportPrice;
    public int    SupportLevelUnlock;
    public Sprite SupportSprite;
}

[Serializable]
public class LevelData
{
    public int         LevelId;
    public int         CurrentcyLevel;
    public float       DynamicDif;
    public List<Color> ColorList      = new List<Color>();
    public List<int>   ColorCountList = new List<int>();
}

public abstract class BaseColorPriorityCalculator : ScriptableObject
{
    public          ColorPriorityCalculatorData ColorPriorityData;

    public void Calculate()
    {
        if (ColorPriorityData == null || !ColorPriorityData.IsInitData)
        {
#if UNITY_EDITOR
            Debug.LogError("ColorPriorityData is null or not initialized");
#endif
            return;
        }
        ColorPriorityCalculator();
    }

    protected abstract void                        ColorPriorityCalculator();
}


public abstract class ColorPriorityCalculatorData : ScriptableObject
{
    public          bool                     IsInitData     { get; set; }
    public abstract List<WoolControl>        WoolControls   { get; set; }
    public abstract Dictionary<Color, int>   CubeColorCount { get; set; }
    public abstract Dictionary<Color, float> ColorPriority  { get; set; }
    public abstract List<Color>              QueueColor     { get; set; }
    public abstract List<Color>              BroomColorList { get; set; }
    public abstract void                     InitData(List<WoolControl> woolControls, Dictionary<Color,int> cubeColorCount);
    public abstract void                     AddColorToQueue(Color color);
    public abstract void                     AddColorToBroom(Color color);
}

public abstract class InterestCurve : ScriptableObject
{
    public abstract float GetPriorityCount(int currentProcess, int totalProcess);
}
