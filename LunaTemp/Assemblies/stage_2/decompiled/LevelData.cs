using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
	public int LevelId;

	public int CurrentcyLevel;

	public float DynamicDif;

	public List<Color> ColorList = new List<Color>();

	public List<int> ColorCountList = new List<int>();
}
