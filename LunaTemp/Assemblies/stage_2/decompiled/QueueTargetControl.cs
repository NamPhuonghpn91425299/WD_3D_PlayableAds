using UnityEngine;

public class QueueTargetControl : MonoBehaviour
{
	private Color _currentColor = Color.black;

	private bool _isActive;

	public bool AddChild(Color color)
	{
		if (_isActive)
		{
			return false;
		}
		_currentColor = color;
		_isActive = true;
		return true;
	}

	public bool CheckCurrentColor(Color color)
	{
		return _currentColor == color;
	}

	public void ResetDefault()
	{
		_isActive = false;
		_currentColor = Color.black;
		if (base.transform.childCount >= 2)
		{
			Transform roll = base.transform.GetChild(1);
			roll.SetParent(null);
			ObjectPool.Instance.AddToPool(roll.gameObject, 1);
			roll.gameObject.SetActive(false);
		}
	}

	public Color GetColorQueue()
	{
		return _currentColor;
	}

	public bool IsAtive()
	{
		return _isActive;
	}
}
