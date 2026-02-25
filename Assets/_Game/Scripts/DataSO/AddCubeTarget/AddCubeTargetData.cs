using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/AddCubeTargetData", menuName = "ScriptableObjects/AddCubeTargetData")]
public class AddCubeTargetData : ScriptableObject
{
    public Vector4 LightDirection = new Vector4(-7, -2, -5, 0);
    public int FirstCubeTargetCost;
    public int SecondCubeTargetCost;
    public int LevelOpen;
    public Sprite AddCubeTargetIcon;
}