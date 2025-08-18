using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if UNITY_EDITOR
using JetBrains.Annotations;
using UnityEditor;
#endif
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;
using Random = UnityEngine.Random;


public class GamePlayMeshController : MonoBehaviour
{
    #region PROPERTIES
    [Header("------------------------------------")]
    [SerializeField] Transform CenterTransform;

    public int               LevelId;
    public bool              IrgnoreLevelId = true;
    public LevelData         LevelData;
    public List<WoolControl> WoolControls;

    public                    InterestCurve               InterestCurveData;
    public                    WoolAnimationData           WoolAnimationData;

    private List<string> _currentColorList      = new List<string>();
    public int TotalColor;

    public Material WoolMaterial;
    public Material WoolChildMaterial;

    public Dictionary<string, float> _colorPriority = new Dictionary<string, float>();
    public Dictionary<string, int>   CubeCount      = new Dictionary<string, int>();
    public int MaxLayerHasThreeSameColor = 2;

    private Dictionary<int, List<ColorDistribution>> _colorDistribution = new Dictionary<int, List<ColorDistribution>>();

    private int _currentLayer = 0;

    private int _colorRemainCount;
    private int _maxLayer = int.MinValue;
    private int _colorCurrentIndex;

    [SerializeField] private float           _maxDistanceFromCetner;

    #endregion
    [SerializeField] public Animator        MainMotionAnimator;
    [SerializeField] public AudioSource MainAudioSource;
    public AudioClip[] MotionAudioClips;

    #region UNITY_METHODS

#if UNITY_EDITOR
    private void OnValidate()
    {
        var pathA_WoolMaterial      = "Assets/_Game/Resources/Materials/M_A_Wool.mat";
        var pathA_WoolChildMaterial = "Assets/_Game/Resources/Materials/M_WoolChild.mat";
        var woolMaterial            = AssetDatabase.LoadAssetAtPath<Material>(pathA_WoolMaterial);
        var woolChildMaterial       = AssetDatabase.LoadAssetAtPath<Material>(pathA_WoolChildMaterial);

        WoolMaterial      = woolMaterial;
        WoolChildMaterial = woolChildMaterial;
        MainMotionAnimator ??= GetComponent<Animator>();
    }
#endif

    #endregion

    #region MAIN_METHODS

    
    public void RaiseVoice(int voiceIndex)
    {

        if (MotionAudioClips != null)
        {
            MainAudioSource.clip = MotionAudioClips[voiceIndex - 1];
            MainAudioSource.Play();
        }
        else
        {
            Debug.LogWarning($"No audio clip found for voice index: {voiceIndex}");
        }
    }
    public void LoadcolorForMesh()
    {
        LoadLevel();
        GenRandomColor();
    }

    private void LoadLevel()
    {
//         if (DataManager.LevelList.TryGetValue(PlayerConfig.player.Level, out var level))
//         {
//             _levelData = level;
//         }
//         if (_levelData == null)
//         {
// #if UNITY_EDITOR
//             Debug.LogError($"LevelId {PlayerConfig.player.Level} not found in LevelList");
// #endif
//             return;
//         }

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

            var meshIndex = 0;
            foreach (var color in WoolControls)
            {
                if (_maxLayer < color.MeshObjectData.TotalLayer)
                {
                    _maxLayer = color.MeshObjectData.TotalLayer;
                }

                for (int i = 0; i < LevelData.ColorList.Count; i++)
                {
                    // if (color.MeshObjectData.HightestColor == LevelData.ColorList[i])
                    if (color.MeshObjectData.HightestColor == LevelData.NameColorList[i])
                    {
                        LevelData.ColorCountList[i]--;
                        break;
                    }
                }

                _colorDistribution.Add(meshIndex, new List<ColorDistribution>());
                for (int i = 0; i < color.MeshObjectData.TotalLayer; i++)
                {
                    _colorDistribution[meshIndex]
                       .Add(new ColorDistribution()
                            {
                                Color      = "",
                                LayerIndex = i,
                                MeshIndex  = meshIndex,
                                IsSetted   = false
                            }
                            );
                }

                meshIndex++;
            }

            for (int i = 0; i < LevelData.ColorList.Count; i++)
            {
                for (int j = 0; j < LevelData.ColorCountList[i]; j++)
                {
                    _currentColorList.Add(LevelData.ColorList[i]);
                }

                _colorRemainCount += LevelData.ColorCountList[i];
            }

        } catch (Exception e)
        {
            Debug.LogError("Check lại xem gán tên màu trong GamePlayMeshControl chưa");
            Debug.LogError($"LoadLevel error: {e}");
        }
        //Load mesh object
    }

    public void ColorPriorityCalculator()
    {
        _colorPriority.Clear();
        Dictionary<string, List<int>> colorSteps = new Dictionary<string, List<int>>();
        for (int i = 0; i < _maxLayer; i++)
        {
            foreach (var wool in WoolControls)
            {
                if(wool.MeshObjectData.ColorStack.Count <= i) continue;
                if(!colorSteps.ContainsKey(wool.MeshObjectData.ColorStack[i]))
                    colorSteps.Add(wool.MeshObjectData.ColorStack[i], new List<int>());
                colorSteps[wool.MeshObjectData.ColorStack[i]].Add(i+1);
            }
        }

        foreach (var color in colorSteps)
        {
            var currentColorCount = color.Value.Count;
            int sum               = 0;
            float index             = 0;
            for (int i = 0; i < currentColorCount; i++)
            {
                index = i;
                if (i > 2 && color.Value[i] > 1f) break;
                sum += (int)color.Value[i];
            }
            _colorPriority.Add(color.Key, 3 * sum / index + 1 - index / currentColorCount);
        }
    }
    

    private void GenRandomColor()
    {
        RandomDownTopColor();
        for (int i = 0; i < WoolControls.Count; i++)
        {
            WoolControls[i]
               .InitMesh();
        }
        for (int i = 0; i < _colorDistribution.Count; i++)
        {
            foreach (var color in _colorDistribution[i])
            {
                if (!color.IsSetted) continue;
                WoolControls[i]
                   .PushColor(color.Color);
            }
        }
    }

    private void RandomDownTopColor()
    {
        _currentLayer = _maxLayer - 1;
        var                     colorCount            = LevelData.ColorList.Count;
        var                     colorDistribution     = GetNextColorToFill();
        var                     meshCount             = WoolControls.Count;
        var                     minLayer              = Mathf.Clamp(_currentLayer - MaxLayerHasThreeSameColor + 1, 1, _maxLayer);
        List<ColorDistribution> colorDistributionList = new List<ColorDistribution>();
        int countBreak = 0;
        while (_colorRemainCount > 0)
        {
            if (countBreak++ > 100000)
                break;
            for (int i = 0; i < meshCount; i++)
            {
                var maxLayer = WoolControls[i].MeshObjectData.TotalLayer - 1;
                if (maxLayer < minLayer) continue;
                for (int j = _currentLayer; j >= minLayer; j--)
                {
                    if (maxLayer < j) continue;
                    if (_colorDistribution[i][j].IsSetted) continue;
                    var data = new ColorDistribution()
                               {
                                   Color      = colorDistribution,
                                   LayerIndex = j,
                                   MeshIndex  = i,
                                   IsSetted   = true
                               };
                    colorDistributionList.Add(data);
                    var tempData = _colorDistribution[i][j];
                    tempData.IsSetted        = true;
                    _colorDistribution[i][j] = tempData;
                }
            }
            while (colorDistributionList.Count > 0)
            {
                colorDistribution = GetNextColorToFill();
                for (int i = 0; i < 3; i++)
                {
                    var colorCountPool = colorDistributionList.Count;
                    if (colorCountPool == 0) break;
                    var randomIndex = Random.Range(0, colorCountPool - 1);

                    var dataTemmp = colorDistributionList[randomIndex];
                    var data      =  _colorDistribution[dataTemmp.MeshIndex][dataTemmp.LayerIndex];
                    data.Color      = colorDistribution;
                    data.IsSetted   = true;
                    data.LayerIndex = dataTemmp.LayerIndex;
                    data.MeshIndex  = dataTemmp.MeshIndex;
                    colorDistributionList.RemoveAt(randomIndex);
                    _colorDistribution[dataTemmp.MeshIndex][dataTemmp.LayerIndex] = data;
                    _colorRemainCount--;
                    LevelData.ColorCountList[_colorCurrentIndex]--;
                    if (LevelData.ColorCountList[_colorCurrentIndex] == 0)
                    {
                        LevelData.ColorCountList.Remove(0);
                        LevelData.ColorList.Remove(colorDistribution);
                        colorCount        = LevelData.ColorCountList.Count;
                        if (colorCount == 0) break;
                        colorDistribution = GetNextColorToFill();
                    }
                }

                if (colorCount == 0) break;

                if (minLayer != 1 && colorDistributionList.Count < 3)
                {
                    _currentLayer = Mathf.Clamp(_currentLayer - 1,                             1, _maxLayer - 1);
                    minLayer      = Mathf.Clamp(_currentLayer - MaxLayerHasThreeSameColor + 1, 1, _maxLayer - MaxLayerHasThreeSameColor - 1);
                    break;
                }
            }
        }
    }

    #endregion

    #region HELPER

    private string GetNextColorToFill()
    {
        var colorResult    = "";
        var maxRemainColor = int.MinValue;
        for (int i = 0; i < LevelData.ColorCountList.Count; i++)
        {
            if (maxRemainColor < LevelData.ColorCountList[i])
            {
                maxRemainColor     = LevelData.ColorCountList[i];
                colorResult        = LevelData.ColorList[i];
                _colorCurrentIndex = i;
            }
        }
        return colorResult;
    }

    private string StringColor(string content, Color color)
    {
        return new StringBuilder()
           .Append("<color=#")
           .Append(ColorUtility.ToHtmlStringRGB(color))
           .Append(">")
           .Append(content)
           .Append("</color>")
           .ToString();
    }

    public void ClearModel()
    {
        foreach (var wool in WoolControls)
        {
            wool.ClearThisWool();
        }
    }
    public void HideInnerMeshes()
    {
        foreach (var wool in WoolControls)
        {
            wool.HideInnerMesh();
        }
    }
    public void ResetPrefabModelState()
    {
        foreach (var wool in WoolControls)
        {
            wool.ResetWoolState();
        }
    }
    public void FadePrefabModelColors()
    {
        foreach (var wool in WoolControls)
        {
            wool.DisplayColorSmoothly();
        }
    }
    public void SetPrefabModelInUnavailableState()
    {
        foreach (var wool in WoolControls)
        {
            wool.DisplayColor(Color.gray);
        }
    }
    public void BuildUpPrefabModelDisplay(float duration)
    {
        foreach (var wool in WoolControls)
        {
            wool.BuildUpModelSmoothly(duration);
        }
    }
    public void SetModelShaderEffect(bool useRim, bool useHalo)
    {
        foreach (var wool in WoolControls)
        {
            wool.SetModelShaderEffect(useRim, useHalo);
        }
    }

    #endregion


}
