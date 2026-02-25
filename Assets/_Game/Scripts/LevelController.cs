using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class LevelController : MonoBehaviour
{
    #region PROPERTIES

    [Header("Color Pallete")][SerializeField] private ColorPalleteData colorPalleteData;

    // [SerializeField]
    // Transform CenterTransform;

    public int LevelId;
    public bool IrgnoreLevelId = true;
    public List<WoolControl> WoolControls;

    public WoolAnimationData WoolAnimationData;

    private List<string> _currentColorList = new();
    public int TotalColor { get; private set; }

    public Material WoolMaterial;
    public Material WoolChildMaterial;

    public Dictionary<string, float> _colorPriority = new();
    public Dictionary<string, int> CubeCount = new();

    public int MaxLayerHasThreeSameColor = 2;

    private Dictionary<int, List<ColorDistribution>> _colorDistribution = new();
    private Dictionary<string, int> _colorCountMap = new();

    private int _currentLayer = 0;

    private int _colorRemainCount;
    private int _colorCurrentIndex;

    public int MaxLayer;
    public int MaxColorCount;

    public List<Vector3> CirclePoint = new();
    public List<int> CirclePointIndex = new();

    private LevelConfigData _LevelData;

    [Header("Auto-Zoom Optimization")][Tooltip("Cache các center của WoolControl để tối ưu hiệu suất auto-zoom")][SerializeField] private List<Vector3> bakedWoolCenters = new List<Vector3>();


    /// <summary>
    /// Kiểm tra xem đã bake wool centers chưa
    /// </summary>
    public bool IsWoolCentersBaked => bakedWoolCenters.Count == WoolControls.Count;

    #endregion

    #region UNITY_METHODS

#if UNITY_EDITOR
    private void OnValidate()
    {
        var pathA_WoolMaterial = "Assets/_Game/Resources_moved/Materials/M_A_Wool.mat";
        var woolMaterial = AssetDatabase.LoadAssetAtPath<Material>(pathA_WoolMaterial);
        WoolMaterial = woolMaterial;

        var pathA_WoolChildMaterial = "Assets/_Game/Resources_moved/Materials/M_WoolChild.mat";
        var woolChildMaterial = AssetDatabase.LoadAssetAtPath<Material>(pathA_WoolChildMaterial);
        WoolChildMaterial = woolChildMaterial;


        var ColorPalletePath = "Assets/_Game/Scripts/DataSO/ColorPallete/ColorPalleteData.asset";
        var colorPallete = AssetDatabase.LoadAssetAtPath<ColorPalleteData>(ColorPalletePath);
        colorPalleteData = colorPallete;


        var woolAnimationDataPath = "Assets/_Game/Scripts/DataSO/WoolAnimation/ScriptableObjects_WoolAnimation.asset";
        var woolAnimationData = AssetDatabase.LoadAssetAtPath<WoolAnimationData>(woolAnimationDataPath);
        WoolAnimationData = woolAnimationData;

        // foreach (var wool in WoolControls)
        // {
        //     if(wool == null) continue;
        //     if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(wool.GetInstanceID(), out string guid, out long fileId) || wool.WoolID != 0) continue;
        //     wool.WoolID = fileId;
        // }
    }
#endif

    #endregion

    #region MAIN_METHODS

    public void InitData(LevelConfigData levelData)
    {
        _LevelData = levelData;
        var totalColor = 0;
        var woolCount = WoolControls.Count;
        for (int i = 0; i < woolCount; i++)
        {
            var woolControl = WoolControls[i];
            if (woolControl == null) continue;
            var woolProperties = levelData.WoolPropertiesList.FirstOrDefault(x => x.WoolId == woolControl.WoolID);

            if (woolProperties == null) continue;

            if (woolControl == null) continue;
            woolControl.SetActiveDecor(woolProperties.ActiveState);
            woolControl.gameObject.SetActive(woolProperties.ActiveState);
            if (!woolProperties.ActiveState) continue;
            totalColor += woolProperties.Color.Count;
            woolControl.SetColorStack(woolProperties.Color);
            woolControl.InitMesh();
        }
        TotalColor = totalColor;
    }

    #endregion

    #region HELPER

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

    public void ChangeLayer(string layer)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
        foreach (var wool in WoolControls)
        {
            wool.ChangeLayer(layer);
        }
    }
    public Vector3 GetEnabledWoolCenters(bool useRendererBoundsCenter, out WoolControl farthestWool, out WoolControl nearestWool)
    {
        var enabledWools = WoolControls.Where(w => w != null && w.TopMeshRenderer != null && w.TopMeshRenderer.enabled).ToList();
        if (enabledWools.Count == 0)
        {
            farthestWool = null;
            nearestWool = null;
            return transform.position;
        }

        Vector3 center;
        if (useRendererBoundsCenter)
        {
            if (enabledWools == null || enabledWools.Count == 0)
            {
                farthestWool = null;
                nearestWool = null;
                return Vector3.zero;
            }

            Bounds bounds = new Bounds(enabledWools[0].transform.position, Vector3.zero);

            foreach (var t in enabledWools)
            {
                bounds.Encapsulate(t.transform.position);
            }

            center = bounds.center;
        }
        else
        {
            Vector3 sum = Vector3.zero;
            foreach (var wool in enabledWools)
            {
                if (useRendererBoundsCenter && wool.TopMeshRenderer != null)
                {
                    sum += wool.TopMeshRenderer.bounds.center;
                }
                else
                {
                    sum += wool.transform.position;
                }
            }
            center = sum / enabledWools.Count;
        }

        float maxDistance = 0f;
        float minDistance = float.MaxValue;
        farthestWool = null;
        nearestWool = null;
        foreach (var wool in enabledWools)
        {
            float distance = Vector3.Distance(wool.transform.position, center);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestWool = wool;
            }
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWool = wool;
            }
        }

        return center;
    }
    public void BuildUpPrefabModelDisplay(float duration)
    {
        foreach (var wool in WoolControls)
        {
            wool.BuildUpModelSmoothly(duration);
        }
    }

    #endregion


    #region UNITY_EDITOR_METHODS

#if UNITY_EDITOR
    [ContextMenu("Bake Wool Spiral Path")]
    public void BakeWoolSpiralPath()
    {
        WoolControls = GetComponentsInChildren<WoolControl>().ToList();
        foreach (var wool in WoolControls)
        {
            //wool.BakeSpiralPath();
            //EditorUtility.SetDirty(wool);
        }
        EditorUtility.SetDirty(this);
        PrefabUtility.SavePrefabAsset(gameObject);
    }
#endif

    #endregion


    #region <====================| Debugger |====================>

    //[DebugCondition("DecoreControl Count is not equal to Total DecoreControl", LogType.Error)]
    private bool CheckDecoreControl()
    {
        var countComponentAvailable = transform
           .GetComponentsInChildren<DecoreControl>()
           .Length;
        int countWoolControl = WoolControls?.Sum(e => e?.DecoreControls?.Count ?? 0) ?? 0;

        return countWoolControl != countComponentAvailable;
    }

    //[DebugCondition("DecoreControl with Name Mesh is not equal to Total DecoreControl", LogType.Error)]
    private bool CheckDecoreControlWithNameMesh()
    {
        string nameMesh = "Item_";
        int countMeshAvailable = transform
           .GetComponentsInChildren<MeshRenderer>()
           .Select(e => e.gameObject.name.StartsWith(nameMesh, StringComparison.CurrentCultureIgnoreCase))
           .Count(e => e);

        int countWoolControl = WoolControls?.Sum(e => e?.DecoreControls?.Count ?? 0) ?? 0;

        return countMeshAvailable != countWoolControl;
    }

    #endregion <=============================================>
}
