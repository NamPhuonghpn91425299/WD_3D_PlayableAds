using static PaintingSharedAttributes;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "PaintingConfig", menuName = "ScriptableObjects/PaintingConfig")]
public class PaintingConfig : ScriptableObject
{
    public ColorPalleteData ColorPalette;
    public List<PaintingPartBasedOnColor> PaintingParts = new List<PaintingPartBasedOnColor>();
    public List<PaintingPartBasedOnColor> PaintingSeparateParts = new List<PaintingPartBasedOnColor>();
    [NonSerialized] public List<PaintingCell> AllCellsAvailable = new List<PaintingCell>();

    public PaintingConfig(PaintingConfig config)
    {
        ColorPalette = config.ColorPalette;
        PaintingParts = new List<PaintingPartBasedOnColor>(config.PaintingParts);
        PaintingSeparateParts = new List<PaintingPartBasedOnColor>(config.PaintingSeparateParts);
        AllCellsAvailable = GetSortedPaintingPart(PaintingParts);
        ValidatePaintingParts();
    }

    public void SetUp(ColorPalleteData colorPalette, List<PaintingPartBasedOnColor> paintingParts, List<int> colorCountList = null)
    {
        ColorPalette = colorPalette;
        AllCellsAvailable = GetSortedPaintingPart(paintingParts);
        PaintingParts = new List<PaintingPartBasedOnColor>(paintingParts);

        List<int> smallerPartCountEachColor = ValidateSeparatePartCellCount(colorCountList);

        if (smallerPartCountEachColor != null)
        {
            SeparatePartToSmallerParts(smallerPartCountEachColor);
        }
        ValidatePaintingParts();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    public void ValidatePaintingParts()
    {
        foreach (var part in PaintingParts) part.ValidateColor(ColorPalette);
        if (PaintingSeparateParts != null) foreach (var part in PaintingSeparateParts) part.ValidateColor(ColorPalette);
        AllCellsAvailable = GetSortedPaintingPart(PaintingParts);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    public void UpdateColors(ColorPalleteData colorPalette, List<string> newColorsKey, List<int> colorCountList = null)
    {
        ColorPalette = colorPalette;

        foreach (var part in PaintingParts)
        {
            if (part.PartID < 0 || part.PartID >= newColorsKey.Count)
            {
                continue;
            }
            part.ColorKey = newColorsKey[part.PartID];
        }

        List<int> smallerPartCountEachColor = ValidateSeparatePartCellCount(colorCountList);

        if (smallerPartCountEachColor != null)
        {
            SeparatePartToSmallerParts(smallerPartCountEachColor);
        }
        AllCellsAvailable = GetSortedPaintingPart(PaintingParts);
        ValidatePaintingParts();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    /// <summary>
    /// Pixel style painting only
    /// Apply full color to all painting cells based on the provided texture.
    /// </summary>
    /// <param name="paintingTexture"></param>
    public void ApplyFullColor(Texture2D paintingTexture)
    {
        foreach (var part in PaintingParts)
        {
            foreach (var cell in part.PaintingCells)
            {
                cell.ApplyPixelColor(paintingTexture);
            }
        }
    }

    public void SeparatePartToSmallerParts(List<int> smallerPartsCounts)
    {
        if (PaintingSeparateParts == null) PaintingSeparateParts = new List<PaintingPartBasedOnColor>();
        PaintingSeparateParts.Clear();

        for (int partIndex = 0; partIndex < PaintingParts.Count; partIndex++)
        {
            if (partIndex >= smallerPartsCounts.Count) continue;
            if (smallerPartsCounts[partIndex] <= 0) continue;
            var part = PaintingParts[partIndex];
            int clusterCount = smallerPartsCounts[partIndex]/3;
            var cells = part.PaintingCells;

            if (cells.Count == 0 || clusterCount <= 1 || cells.Count <= clusterCount)
            {
                PaintingPartBasedOnColor singlePart = new PaintingPartBasedOnColor();
                PaintingSeparateParts.Add(new PaintingPartBasedOnColor
                {
                    PartID = PaintingSeparateParts.Count,
                    ColorKey = part.ColorKey,
                    PaintingCells = new List<PaintingCell> (cells)
                });
                continue;
            }

            var shuffled = cells.OrderBy(c => Random.value).ToList();
            var centers = new List<Vector2>();
            var clusters = new Dictionary<int, List<PaintingCell>>();

            for (int i = 0; i < clusterCount; i++)
            {
                centers.Add(new Vector2(shuffled[i].Row, shuffled[i].Column));
                clusters[i] = new List<PaintingCell> { shuffled[i] };
            }

            for (int i = clusterCount; i < shuffled.Count; i++)
            {
                var cell = shuffled[i];
                int bestIndex = 0;
                float bestDist = float.MaxValue;

                for (int j = 0; j < centers.Count; j++)
                {
                    float dist = Vector2.SqrMagnitude(new Vector2(cell.Row, cell.Column) - centers[j]);
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        bestIndex = j;
                    }
                }

                clusters[bestIndex].Add(cell);
            }

            for (int i = 0; i < clusterCount; i++)
            {
                var cluster = clusters[i];
                float avgRow = (float)cluster.Average(c => c.Row);
                float avgCol = (float)cluster.Average(c => c.Column);
                centers[i] = new Vector2(avgRow, avgCol);
            }

            foreach (var kvp in clusters)
            {
                PaintingSeparateParts.Add(new PaintingPartBasedOnColor
                {
                    PartID = PaintingSeparateParts.Count,
                    ColorKey = part.ColorKey,
                    PaintingCells = kvp.Value
                });
            }
        }

        foreach (var part in PaintingSeparateParts)
        {
            part.PaintingCells = GetSortedPaintingPart(part.PaintingCells);
        }
    }

    /// <summary>
    /// Make sure every part of the painting is valid
    /// </summary>
    /// <param name="smallerPartsCounts"></param>
    private List<int> ValidateSeparatePartCellCount(List<int> smallerPartsCounts)
    {
        for (int i = 0; i < smallerPartsCounts.Count; i++)
        {
            if (PaintingParts[i].PaintingCells.Count <= 0)
            {
                smallerPartsCounts[i] = 0;
            }
        }

        return smallerPartsCounts;
    }

    public List<PaintingCell> GetSortedPaintingPart(List<PaintingPartBasedOnColor> inputParts)
    {
        return inputParts
            .SelectMany(part => part.PaintingCells)
            .OrderBy(cell => cell.Row)
            .ThenBy(cell => cell.Column)
            .ToList();
    }

    public List<PaintingCell> GetSortedPaintingPart(List<PaintingCell> inputCells)
    {
        return inputCells
            .OrderBy(cell => cell.Row)
            .ThenBy(cell => cell.Column)
            .ToList();
    }
}