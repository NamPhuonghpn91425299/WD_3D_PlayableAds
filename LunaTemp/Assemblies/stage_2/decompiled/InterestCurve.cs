using UnityEngine;

public abstract class InterestCurve : ScriptableObject
{
	public abstract float GetPriorityCount(int currentProcess, int totalProcess);
}
