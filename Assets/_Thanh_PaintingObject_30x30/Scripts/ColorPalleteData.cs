using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/ColorPallete", menuName = "ScriptableObjects/ColorPallete")]
public class ColorPalleteData : ScriptableObject
{
    public Dictionary<string, Color> colorPallete = new Dictionary<string, Color>();
}