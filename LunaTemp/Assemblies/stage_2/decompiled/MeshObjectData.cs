using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeshObjectData
{
	public int TotalLayer;

	public Color HightestColor;

	public List<Color> ColorStack = new List<Color>();
}
