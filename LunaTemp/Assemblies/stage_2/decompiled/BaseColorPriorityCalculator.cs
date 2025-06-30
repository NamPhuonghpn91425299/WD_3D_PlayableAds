using UnityEngine;

public abstract class BaseColorPriorityCalculator : ScriptableObject
{
	public ColorPriorityCalculatorData ColorPriorityData;

	public void Calculate()
	{
		if (!(ColorPriorityData == null) && ColorPriorityData.IsInitData)
		{
			ColorPriorityCalculator();
		}
	}

	protected abstract void ColorPriorityCalculator();
}
