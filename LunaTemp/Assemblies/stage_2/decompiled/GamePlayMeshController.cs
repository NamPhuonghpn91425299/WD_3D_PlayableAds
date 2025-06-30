using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GamePlayMeshController : MonoBehaviour
{
	[SerializeField]
	private Transform CenterTransform;

	public int LevelId;

	public bool IrgnoreLevelId = true;

	public LevelData LevelData;

	public List<WoolControl> WoolControls;

	public InterestCurve InterestCurveData;

	public WoolAnimationData WoolAnimationData;

	private List<Color> _currentColorList = new List<Color>();

	public Material WoolMaterial;

	public Material WoolChildMaterial;

	public Dictionary<Color, float> _colorPriority = new Dictionary<Color, float>();

	public Dictionary<Color, int> CubeCount = new Dictionary<Color, int>();

	public int MaxLayerHasThreeSameColor = 2;

	private Dictionary<int, List<ColorDistribution>> _colorDistribution = new Dictionary<int, List<ColorDistribution>>();

	private int _currentLayer = 0;

	private int _colorRemainCount;

	private int _maxLayer = int.MinValue;

	private int _colorCurrentIndex;

	[SerializeField]
	private float _maxDistanceFromCetner;

	public int TotalColor { get; private set; }

	public void LoadcolorForMesh()
	{
		LoadLevel();
		GenRandomColor();
	}

	private void LoadLevel()
	{
		try
		{
			CubeCount.Clear();
			_colorDistribution.Clear();
			_colorRemainCount = 0;
			for (int i = 0; i < LevelData.ColorList.Count; i++)
			{
				CubeCount.Add(LevelData.ColorList[i], LevelData.ColorCountList[i] / 3);
				TotalColor += LevelData.ColorCountList[i];
			}
			int meshIndex = 0;
			foreach (WoolControl color in WoolControls)
			{
				if (_maxLayer < color.MeshObjectData.TotalLayer)
				{
					_maxLayer = color.MeshObjectData.TotalLayer;
				}
				for (int l = 0; l < LevelData.ColorList.Count; l++)
				{
					if (color.MeshObjectData.HightestColor == LevelData.ColorList[l])
					{
						LevelData.ColorCountList[l]--;
						break;
					}
				}
				_colorDistribution.Add(meshIndex, new List<ColorDistribution>());
				for (int k = 0; k < color.MeshObjectData.TotalLayer; k++)
				{
					_colorDistribution[meshIndex].Add(new ColorDistribution
					{
						Color = Color.white,
						LayerIndex = k,
						MeshIndex = meshIndex,
						IsSetted = false
					});
				}
				meshIndex++;
			}
			for (int j = 0; j < LevelData.ColorList.Count; j++)
			{
				for (int m = 0; m < LevelData.ColorCountList[j]; m++)
				{
					_currentColorList.Add(LevelData.ColorList[j]);
				}
				_colorRemainCount += LevelData.ColorCountList[j];
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"LoadLevel error: {e}");
		}
	}

	public void ColorPriorityCalculator()
	{
		_colorPriority.Clear();
		Dictionary<Color, List<int>> colorSteps = new Dictionary<Color, List<int>>();
		for (int j = 0; j < _maxLayer; j++)
		{
			foreach (WoolControl wool in WoolControls)
			{
				if (wool.MeshObjectData.ColorStack.Count > j)
				{
					if (!colorSteps.ContainsKey(wool.MeshObjectData.ColorStack[j]))
					{
						colorSteps.Add(wool.MeshObjectData.ColorStack[j], new List<int>());
					}
					colorSteps[wool.MeshObjectData.ColorStack[j]].Add(j + 1);
				}
			}
		}
		foreach (KeyValuePair<Color, List<int>> color in colorSteps)
		{
			int sum = 0;
			for (int i = 0; i < color.Value.Count && i <= 2; i++)
			{
				sum += color.Value[i];
			}
			_colorPriority.Add(color.Key, sum);
		}
	}

	private void GenRandomColor()
	{
		RandomDownTopColor();
		for (int j = 0; j < WoolControls.Count; j++)
		{
			WoolControls[j].InitMesh();
		}
		for (int i = 0; i < _colorDistribution.Count; i++)
		{
			foreach (ColorDistribution color in _colorDistribution[i])
			{
				if (color.IsSetted)
				{
					WoolControls[i].PushColor(color.Color);
				}
			}
		}
	}

	private void RandomDownTopColor()
	{
		_currentLayer = _maxLayer - 1;
		int colorCount = LevelData.ColorList.Count;
		Color colorDistribution = GetNextColorToFill();
		int meshCount = WoolControls.Count;
		int minLayer = Mathf.Clamp(_currentLayer - MaxLayerHasThreeSameColor + 1, 1, _maxLayer);
		List<ColorDistribution> colorDistributionList = new List<ColorDistribution>();
		while (_colorRemainCount > 0)
		{
			for (int j = 0; j < meshCount; j++)
			{
				int maxLayer = WoolControls[j].MeshObjectData.TotalLayer - 1;
				if (maxLayer < minLayer)
				{
					continue;
				}
				for (int k = _currentLayer; k >= minLayer; k--)
				{
					if (maxLayer >= k && !_colorDistribution[j][k].IsSetted)
					{
						ColorDistribution colorDistribution2 = default(ColorDistribution);
						colorDistribution2.Color = colorDistribution;
						colorDistribution2.LayerIndex = k;
						colorDistribution2.MeshIndex = j;
						colorDistribution2.IsSetted = true;
						ColorDistribution data = colorDistribution2;
						colorDistributionList.Add(data);
						ColorDistribution tempData = _colorDistribution[j][k];
						tempData.IsSetted = true;
						_colorDistribution[j][k] = tempData;
					}
				}
			}
			while (colorDistributionList.Count > 0)
			{
				colorDistribution = GetNextColorToFill();
				for (int i = 0; i < 3; i++)
				{
					int colorCountPool = colorDistributionList.Count;
					if (colorCountPool == 0)
					{
						break;
					}
					int randomIndex = UnityEngine.Random.Range(0, colorCountPool - 1);
					ColorDistribution dataTemmp = colorDistributionList[randomIndex];
					ColorDistribution data2 = _colorDistribution[dataTemmp.MeshIndex][dataTemmp.LayerIndex];
					data2.Color = colorDistribution;
					data2.IsSetted = true;
					data2.LayerIndex = dataTemmp.LayerIndex;
					data2.MeshIndex = dataTemmp.MeshIndex;
					colorDistributionList.RemoveAt(randomIndex);
					_colorDistribution[dataTemmp.MeshIndex][dataTemmp.LayerIndex] = data2;
					_colorRemainCount--;
					LevelData.ColorCountList[_colorCurrentIndex]--;
					if (LevelData.ColorCountList[_colorCurrentIndex] == 0)
					{
						LevelData.ColorCountList.Remove(0);
						LevelData.ColorList.Remove(colorDistribution);
						colorCount = LevelData.ColorCountList.Count;
						if (colorCount == 0)
						{
							break;
						}
						colorDistribution = GetNextColorToFill();
					}
				}
				if (colorCount == 0)
				{
					break;
				}
				if (minLayer != 1 && colorDistributionList.Count < 3)
				{
					_currentLayer = Mathf.Clamp(_currentLayer - 1, 1, _maxLayer - 1);
					minLayer = Mathf.Clamp(_currentLayer - MaxLayerHasThreeSameColor + 1, 1, _maxLayer - MaxLayerHasThreeSameColor - 1);
					break;
				}
			}
		}
	}

	private Color GetNextColorToFill()
	{
		Color colorResult = Color.black;
		int maxRemainColor = int.MinValue;
		for (int i = 0; i < LevelData.ColorCountList.Count; i++)
		{
			if (maxRemainColor < LevelData.ColorCountList[i])
			{
				maxRemainColor = LevelData.ColorCountList[i];
				colorResult = LevelData.ColorList[i];
				_colorCurrentIndex = i;
			}
		}
		return colorResult;
	}

	private string StringColor(string content, Color color)
	{
		return new StringBuilder().Append("<color=#").Append(ColorUtility.ToHtmlStringRGB(color)).Append(">")
			.Append(content)
			.Append("</color>")
			.ToString();
	}

	public void ClearModel()
	{
		foreach (WoolControl wool in WoolControls)
		{
			wool.ClearThisWool();
		}
	}

	public void HideInnerMeshes()
	{
		foreach (WoolControl wool in WoolControls)
		{
			wool.HideInnerMesh();
		}
	}

	public void ResetPrefabModelState()
	{
		foreach (WoolControl wool in WoolControls)
		{
			wool.ResetWoolState();
		}
	}

	public void FadePrefabModelColors()
	{
		foreach (WoolControl wool in WoolControls)
		{
			wool.DisplayColorSmoothly();
		}
	}

	public void SetPrefabModelInUnavailableState()
	{
		foreach (WoolControl wool in WoolControls)
		{
			wool.DisplayColor(Color.gray);
		}
	}

	public void BuildUpPrefabModelDisplay(float duration)
	{
		foreach (WoolControl wool in WoolControls)
		{
			wool.BuildUpModelSmoothly(duration);
		}
	}

	public void SetModelShaderEffect(bool useRim, bool useHalo)
	{
		foreach (WoolControl wool in WoolControls)
		{
			wool.SetModelShaderEffect(useRim, useHalo);
		}
	}
}
