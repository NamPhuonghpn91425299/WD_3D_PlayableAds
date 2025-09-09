using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/ColorPallete", menuName = "ScriptableObjects/ColorPallete")]
public class ColorPalleteData : ScriptableObject
{
    
    public Dictionary<string, Color> colorPallete = new Dictionary<string, Color>();
    public List<string> colorKeys = new List<string>();
    public List<Color> colorsValues = new List<Color>();
    
    [ContextMenu("SetupColor")]
    public void SetupColor()
    {
        colorPallete.Clear();
        for (int i = 0; i < colorKeys.Count; i++)
        {
            colorPallete.Add(colorKeys[i], colorsValues[i]);
        }
        Debug.Log(colorPallete.Count+ " colors added to the palette.");
    }
    
    public string FindKeyByColor(Color colorToFind)
    {
        Debug.Log(colorsValues.Count);
        for (int i = 0; i < colorsValues.Count; i++)
        {
            
            Debug.Log($"Comparing: {colorsValues[i]} vs {colorToFind}");
            if(colorsValues[i] == colorToFind)
                return colorKeys[i];
        }

        return null; // Không tìm thấy
    }
}