using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/Boosters/BoosterDataSO", menuName = "ScriptableObjects/Boosters/BoosterDataSO")]
public class BoosterDataSO : ScriptableObject
{
    public int BoosterCost;
    public int LevelOpen;
    public Sprite BoosterIcon;
}