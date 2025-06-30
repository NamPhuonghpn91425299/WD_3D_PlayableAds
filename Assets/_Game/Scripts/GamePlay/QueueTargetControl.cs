using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTargetControl : MonoBehaviour
{
    #region PROPERTIES

    private Color _currentColor = Color.black;
    private bool  _isActive;

    #endregion


    #region MAIN_METHODS

    public bool AddChild(Color color)
    {
        if (_isActive) return false;
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
        _isActive                                                    = false;
        _currentColor                                                = Color.black;
        if(transform.childCount < 2) return;
        var roll = transform.GetChild(1);
        roll.SetParent(null);
        roll.gameObject.SetActive(false);
    }
    public Color GetColorQueue() => _currentColor;
    
    public bool IsAtive() => _isActive;
    
    #endregion
}