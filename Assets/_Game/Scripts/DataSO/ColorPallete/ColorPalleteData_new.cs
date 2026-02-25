using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPallete_New", menuName = "ScriptableObjects/ColorPallete/ColorPallete_New")]
public class ColorPalleteData_new : ScriptableObject
{
    public ColorPallete_New[] m_colorPallete;
    
    private Dictionary<string, Material> _colorPalleteDict;
    public Dictionary<string, Material> colorPallete_New
    {
        get
        {
            InitializeDictionary();
            return _colorPalleteDict;
        }
    }

    private bool isInitialized = false;

    // Gọi method này từ Awake() của manager
    public void BuildDictionary()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        if (isInitialized && _colorPalleteDict != null) return;

        _colorPalleteDict = new Dictionary<string, Material>();

        if (m_colorPallete != null)
        {
            foreach (var color in m_colorPallete)
            {
                if (!string.IsNullOrEmpty(color.colorName) && !_colorPalleteDict.ContainsKey(color.colorName))
                {
                    _colorPalleteDict.Add(color.colorName, color.material);
                }
            }
        }

        isInitialized = true;
        Debug.Log($"[ColorPallete] Đã khởi tạo dictionary với {_colorPalleteDict.Count} màu từ {this.name}");
    }

    public bool HasColor(string colorName)
    {
        InitializeDictionary();
        return colorPallete_New.ContainsKey(colorName);
    }

    public Material GetMaterial(string colorName)
    {
        InitializeDictionary();

        if (colorPallete_New.TryGetValue(colorName, out var material))
        {
            return material;
        }

        Debug.LogWarning($"Color '{colorName}' not found in {this.name}");
        return null;
    }

    public Color GetColor(string colorName)
    {
        InitializeDictionary();

        if (colorPallete_New.TryGetValue(colorName, out var material))
        {
            if (material != null)
            {
                return material.color;
            }
        }

        Debug.LogWarning($"Color '{colorName}' not found or material is null in {this.name}");
        return Color.white;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        isInitialized = false;
        _colorPalleteDict = null;
    }
#endif
}


[Serializable]
public class ColorPallete_New
{
    public string colorName;
    public Material material;
}
