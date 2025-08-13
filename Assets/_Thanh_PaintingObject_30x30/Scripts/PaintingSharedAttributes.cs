using System.Collections.Generic;
using UnityEngine;
using System;

public static class PaintingSharedAttributes
{
    public static bool UsingBarAsTargetWool = true;
    public static bool BakedPaintingBackground = true;

    public const int PaintingSizeX = 300;
    public const int PaintingSizeY = 300;
    public const int PaintingGridSizeX = 30;
    public const int PaintingGridSizeY = 30;

    public const string MainTexKey = "_MainTex";
    public const string CellColorKey = "_Color";
    public const string BrightnessKey = "_Brightness";

    [System.Serializable]
    public class PaintingPartBasedOnColor
    {
        public int PartID = 0;
        public string ColorKey;
        public List<PaintingCell> PaintingCells = new List<PaintingCell>();
        public bool Painted = false;

        public void ValidateColor(ColorPalleteData colorPalette)
        {
            if (colorPalette == null)
            {
                Debug.LogError("ValidateColor: ColorPalette is null.");
                return;
            }

            if (string.IsNullOrEmpty(ColorKey))
            {
                Debug.LogError("ValidateColor: ColorKey is null or empty.");
                return;
            }

            var dict = colorPalette.colorPallete;
            if (dict == null)
            {
                Debug.LogError("ValidateColor: colorPallete dictionary is null.");
                return;
            }

            // 1) Try exact key
            if (dict.TryGetValue(ColorKey, out var color))
            {
                foreach (var cell in PaintingCells) cell.CellColor = color;
                return;
            }

            // 2) Try case-insensitive match
            foreach (var kv in dict)
            {
                if (string.Equals(kv.Key, ColorKey, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var cell in PaintingCells) cell.CellColor = kv.Value;
                    return;
                }
            }

            // 3) Not found: log all available keys
            Debug.LogError($"ValidateColor: Missing color key '{ColorKey}'. Available keys: {string.Join(", ", dict.Keys)}");
        }
    }

    [System.Serializable]
    public class PaintingCell
    {
        public int Row = 0;
        public int Column = 0;

        [NonSerialized] public Color CellColor;
        private RectInt CellRect;

        #region _pixel painting style
        public void PixelInitialize()
        {
            CellRect = GetCellPixelRect(Column, Row);
        }

        public void ApplyPixelColor(Texture2D paintingTexture)
        {
            SetPixelColor(CellColor, paintingTexture);
        }

        public void RemovePixelColor(Texture2D paintingTexture)
        {
            SetPixelColor(Color.white, paintingTexture);
        }
        #endregion

        #region _fish scales painting style
        public void ApplyFishScaleCellColor(PaintingFishScaleSpriteCell cell)
        {
            if (cell == null) return;
            cell.Apply();
        }
        #endregion
        
        #region _supportive
        public RectInt GetCellPixelRect(int col, int row)
        {
            int texWidth = PaintingSizeX;
            int texHeight = PaintingSizeY;

            int cellWidth = texWidth / PaintingGridSizeX;
            int cellHeight = texHeight / PaintingGridSizeY;

            int x = Mathf.Clamp(col * cellWidth, 0, texWidth - 1);
            int y = Mathf.Clamp(row * cellHeight, 0, texHeight - 1);

            return new RectInt(x, y, cellWidth, cellHeight);
        }

        private void SetPixelColor(Color color, Texture2D paintingTexture)
        {
            if (paintingTexture == null) return;

            if (CellRect.height <= 0 || CellRect.width <= 0) PixelInitialize();

            for (int y = 0; y < CellRect.height; y++)
            {
                for (int x = 0; x < CellRect.width; x++)
                {
                    paintingTexture.SetPixel(CellRect.x + x, CellRect.y + y, color);
                }
            }

            paintingTexture.Apply();
        }
        #endregion
    }

    public static float GetColorBrightness(Color c)
    {
        return Mathf.Sqrt(
            0.299f * c.r * c.r +
            0.587f * c.g * c.g +
            0.114f * c.b * c.b);
    }

    #region _extensions & sub-methods
    public static float ColorDiffirence(this Color c1, Color c2)
    {
        var r_diff = c1.r - c2.r;
        var g_diff = c1.g - c2.g;
        var b_diff = c1.b - c2.b;
        return Mathf.Sqrt(r_diff * r_diff + g_diff * g_diff + b_diff * b_diff);
    }
    public static Color Round(this Color target, int decimalPlaces = 2)
    {
        float factor = Mathf.Pow(10, decimalPlaces);
        return new Color(
            Mathf.Round(target.r * factor) / factor,
            Mathf.Round(target.g * factor) / factor,
            Mathf.Round(target.b * factor) / factor,
            Mathf.Round(target.a * factor) / factor);
    }
    public static T GetRandom<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    #endregion
}