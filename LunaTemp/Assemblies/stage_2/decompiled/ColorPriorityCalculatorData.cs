using System.Collections.Generic;
using UnityEngine;

public abstract class ColorPriorityCalculatorData : ScriptableObject
{
	public bool IsInitData { get; set; }

	public abstract List<WoolControl> WoolControls { get; set; }

	public abstract Dictionary<Color, int> CubeColorCount { get; set; }

	public abstract Dictionary<Color, float> ColorPriority { get; set; }

	public abstract List<Color> QueueColor { get; set; }

	public abstract List<Color> BroomColorList { get; set; }

	public abstract void InitData(List<WoolControl> woolControls, Dictionary<Color, int> cubeColorCount);

	public abstract void AddColorToQueue(Color color);

	public abstract void AddColorToBroom(Color color);
}
