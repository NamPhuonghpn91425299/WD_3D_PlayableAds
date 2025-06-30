using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System;


#if UNITY_EDITOR
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
#endif

[ExecuteAlways]
public class WoolControl : MonoBehaviour
{
    #region PROPERTIES

    public bool debugUV;

    [Header("Mesh Object")] public MeshObjectData    MeshObjectData;
    public                         Renderer      TopMeshRenderer;
    public                         Renderer      HideMeshRenderer;
    public                         Collider          BoxCollider;
    public                         Material          MainMaterial;
    public                         Material          TranparentMaterial;
    public                         WoolAnimationData WoolAnimationData;

    public List<DecoreControl> DecoreControls;
    public List<DecoreControl> RemovedDecoreControls;



    public MeshFilter MeshFilter;

    [HideInInspector] public bool IsSetColorHightest;

    private MaterialPropertyBlock _topMaterialPropertyBlock;
    private MaterialPropertyBlock _hideMaterialPropertyBlock;

    private Color _currentColor;

    private int _indexLayer;

    [SerializeField] private List<Vector3> _spiralPath    = new ();
    [SerializeField] private List<float>   _spiralPathUVY = new ();

    #region custom attributes

    [Serializable] //temporary
    public class DecorObjectSetting
    {
        public                DecoreControl DecorObject;
        [Range(0, 1f)] public float         WoolProgressStartSrop = 0.5f;
    }

    #endregion

    #endregion

#if UNITY_EDITOR
    private void OnValidate()
    {
        TopMeshRenderer  ??= GetComponent<MeshRenderer>();
        HideMeshRenderer ??= transform.GetChild(0).GetComponent<MeshRenderer>();
        BoxCollider      ??= GetComponent<BoxCollider>();
    }
#endif

    private void OnEnable() { DisplayColor(); }


    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            DisplayColor();
        }
#endif
    }

    #region MAIN_METHODS

    public void InitMesh()
    {
        MeshObjectData.ColorStack.Clear();
        _indexLayer         = 0;
        BoxCollider.enabled = true;
        _currentColor       = MeshObjectData.HightestColor;
        PushColor(MeshObjectData.HightestColor);
    }

    public bool PushColor(Color color)
    {
        if (MeshObjectData == null || !HideMeshRenderer || _indexLayer >= MeshObjectData.TotalLayer || _hideMaterialPropertyBlock == null) return false;

        MeshObjectData.ColorStack ??= new ();
        MeshObjectData.ColorStack.Add(color);
        _indexLayer++;
        if (MeshObjectData.ColorStack.Count > 1)
        {
            _hideMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, MeshObjectData.ColorStack[1]);
            HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);
        }
        HideMeshRenderer.enabled = MeshObjectData.ColorStack.Count > 1;
        return true;
    }

    public void SetTranparentWool(bool isTranparent)
    {
        if (MeshObjectData.ColorStack.Count > 1)
        {
            _hideMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, MeshObjectData.ColorStack[1]);
            HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);
        }
        TopMeshRenderer.sharedMaterial = isTranparent
            ? TranparentMaterial
            : MainMaterial;
        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
    }

    public void WoolRotation()
    {
        if (GamePlaySystem.Instance.IsGoToStore)
        {
            GamePlaySystem.Instance.GoToStore();
            return;
        }
        StartCoroutine(AsyncWoolRotation());
    }

    public void SetColor(Color color)
    {
        MeshObjectData.ColorStack.Add(color);
        MeshObjectData.TotalLayer++;
    }

    private bool            _isPlayAnim;

    IEnumerator AsyncWoolRotation()
    {
        if (Time.deltaTime <= 0) yield return null;
        if (_isPlayAnim) yield break;
        if (GamePlaySystem.Instance && GamePlaySystem.Instance.QueueCount == GamePlaySystem.Instance.CurrentQueueTargets.Count) yield break;
        if (!HideMeshRenderer || !WoolAnimationData || !TopMeshRenderer  ||
            _spiralPathUVY == null || _topMaterialPropertyBlock == null || !BoxCollider || MeshObjectData == null
            || MeshObjectData.ColorStack == null || MeshObjectData.ColorStack.Count == 0) yield break;
        if (!GamePlaySystem.Instance.OnClickMesh(transform, _spiralPath, _currentColor)) yield break;
        _isPlayAnim = true;
        GamePlaySystem.Instance.ActiveHandController(false);
        Color nextColor = Color.black;
        try
        {
            MeshObjectData.ColorStack.RemoveAt(0);
        } catch { }
        var totalColor = MeshObjectData.ColorStack.Count;

        if (totalColor == 0)
        {
            HideMeshRenderer.enabled = false;
        }
        else
        {
            nextColor = MeshObjectData.ColorStack[0];
            _hideMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, nextColor);
            _hideMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1);
        }

        HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);

        var   totalTime = WoolAnimationData.Duration + WoolAnimationData.OffSet;
        float timer     = 0f;



        float minUVY  = _spiralPathUVY.Min();
        float maxUVY  = _spiralPathUVY.Max();
        float uvRange = Mathf.Max(0.0001f, maxUVY - minUVY);

        MaterialPropertyBlock woolProperties = new MaterialPropertyBlock();
        TopMeshRenderer.GetPropertyBlock(woolProperties);

        while (timer < totalTime)
        {
            float t     = timer / totalTime;
            float idx   = t * (_spiralPath.Count - 1);
            int   idx0  = Mathf.Clamp(Mathf.FloorToInt(idx), 0, _spiralPath.Count - 1);
            int   idx1  = Mathf.Clamp(idx0 + 1,              0, _spiralPath.Count - 1);
            float lerpT = idx - idx0;

            // Nội suy UVY
            float uvy0          = _spiralPathUVY[idx0];
            float uvy1          = _spiralPathUVY[idx1];
            float uvy           = Mathf.Lerp(uvy0, uvy1, lerpT);
            float normalizedUVY = (uvy - minUVY) / uvRange;

            if (false)
            {
                // Set _Display theo UVY nội suy
                _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display,
                        Mathf.Clamp01(normalizedUVY)
                    );
                TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
            }
            else
            {
                woolProperties.SetFloat(T_Utilities.ShaderPropertiesLib.Display, Mathf.Clamp01(normalizedUVY));
                TopMeshRenderer.SetPropertyBlock(woolProperties);
            }

            DecorObjectCheckAlongWoolRotation(t);

            timer += Time.deltaTime;
            yield return null;
        }

        PulseAllDecorObjects();

        if (nextColor != Color.black)
        {
            _topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, nextColor);
            _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1);
            _currentColor = nextColor;
        }

        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
        _indexLayer--;
        BoxCollider.enabled = totalColor > 0;
        if (totalColor == 1)
            HideMeshRenderer.enabled = false;
        if (totalColor == 0)
            TopMeshRenderer.enabled = false;

        yield return null;
        _isPlayAnim = false;
    }

    public void PLayAnim(int index) { StartCoroutine(ExecuteAnim(index)); }

    private IEnumerator ExecuteAnim(int index)
    {
        if (Time.deltaTime <= 0) yield return null;
        if (DecoreControls.Count != 0 || DecoreControls != null)
        {
            for (int i = 0; i < DecoreControls.Count; i++)
            {
                if (DecoreControls.Count == 0) break;
                RemovedDecoreControls.Add(DecoreControls[i]);
                DecoreControls[i]
                   .UseGravity(true);
                DecoreControls.RemoveAt(i);
                i--;
            }
        }

        Color nextColor = Color.black;
        MeshObjectData.ColorStack.RemoveAt(index);
        var totalColor = MeshObjectData.ColorStack.Count;

        if (totalColor == 0)
        {
            HideMeshRenderer.enabled = false;
        }
        else
        {
            nextColor = MeshObjectData.ColorStack[0];
            _hideMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, nextColor);
            _hideMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1);
        }

        HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);

        var   totalTime = WoolAnimationData.Duration + WoolAnimationData.OffSet;
        float timer     = 0f;

        float minUVY  = _spiralPathUVY.Min();
        float maxUVY  = _spiralPathUVY.Max();
        float uvRange = Mathf.Max(0.0001f, maxUVY - minUVY);

        while (timer < totalTime)
        {
            float t     = timer / totalTime;
            float idx   = t     * (_spiralPath.Count - 1);
            int   idx0  = Mathf.FloorToInt(idx);
            int   idx1  = Mathf.Clamp(idx0 + 1, 0, _spiralPath.Count - 1);
            float lerpT = idx - idx0;

            // Nội suy UVY
            float uvy0          = _spiralPathUVY[idx0];
            float uvy1          = _spiralPathUVY[idx1];
            float uvy           = Mathf.Lerp(uvy0, uvy1, lerpT);
            float normalizedUVY = (uvy - minUVY) / uvRange;

            // Set _Display theo UVY nội suy
            _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display,
                    Mathf.Clamp01(normalizedUVY)
                );
            TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);

            timer += Time.deltaTime;
            yield return null;
        }

        if (nextColor != Color.black)
        {
            _topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, nextColor);
            _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1);
            _currentColor = nextColor;
        }

        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
        _indexLayer--;
        BoxCollider.enabled = totalColor > 0;
        if (totalColor == 1)
            HideMeshRenderer.enabled = false;
        if (totalColor == 0)
            TopMeshRenderer.enabled = false;

        yield return null;
    }


    private void DisplayColor()
    {
        if (MeshObjectData             == null) return;
        _topMaterialPropertyBlock  ??= new MaterialPropertyBlock();
        _hideMaterialPropertyBlock ??= new MaterialPropertyBlock();
        TopMeshRenderer?.GetPropertyBlock(_topMaterialPropertyBlock);
        HideMeshRenderer?.GetPropertyBlock(_hideMaterialPropertyBlock);
        _topMaterialPropertyBlock?.SetColor(T_Utilities.ShaderPropertiesLib.Color, MeshObjectData.HightestColor);
        _topMaterialPropertyBlock?.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim,       0);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 0);
        TopMeshRenderer?.SetPropertyBlock(_topMaterialPropertyBlock);
        if (HideMeshRenderer) HideMeshRenderer.enabled = true;
        if (TopMeshRenderer) TopMeshRenderer.enabled   = true;
    }

    public void DisplayColor(Color albedo)
    {
        if (MeshObjectData == null) return;
        if (_topMaterialPropertyBlock == null) _topMaterialPropertyBlock = new MaterialPropertyBlock();
        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
        _topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, albedo);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display,      1);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim,       0);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 1);
        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);

        if (TopMeshRenderer) TopMeshRenderer.enabled = true;
    }

    public void DisplayColorSmoothly()
    {
        if (MeshObjectData == null) return;
        if (_topMaterialPropertyBlock == null) _topMaterialPropertyBlock = new MaterialPropertyBlock();

        Color currnetColor = Color.grey;
        Color targetColor  = MeshObjectData.HightestColor;

        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);

        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display,      1);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim,       0);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 1);
        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);

        DOTween.To(() => currnetColor, x =>
                {
                    currnetColor = x;
                    _topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, currnetColor);
                    TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
                }
              , targetColor, 1
            );

        TopMeshRenderer.enabled  = true;
    }

    public void BuildUpModelSmoothly(float duration)
    {
        if (MeshObjectData == null) return;
        if (_topMaterialPropertyBlock == null) _topMaterialPropertyBlock = new MaterialPropertyBlock();

        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);

        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim,       0);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 0);
        _topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, Color.gray);
        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);

        float currentDisplay = 0;
        DOTween.To(() => currentDisplay, x =>
                {
                    currentDisplay = x;
                    _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, currentDisplay);
                    TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
                }
              , 1, duration
            );

        TopMeshRenderer.enabled  = true;
    }

    public void SetModelShaderEffect(bool useRim, bool useHalo)
    {
        if (MeshObjectData == null) return;
        if (_topMaterialPropertyBlock == null) _topMaterialPropertyBlock = new MaterialPropertyBlock();

        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);

        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim, useRim
                ? 1
                : 0
            );
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, useHalo
                ? 1
                : 0
            );
        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);

        TopMeshRenderer.enabled  = true;
    }

    public void ClearThisWool()
    {
        if (MeshObjectData == null) return;
        if (_topMaterialPropertyBlock == null) _topMaterialPropertyBlock = new MaterialPropertyBlock();

        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseRim,       0);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.UseHaloOuter, 0);
        _topMaterialPropertyBlock.SetColor(T_Utilities.ShaderPropertiesLib.Color, Color.gray);
        _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 0);
        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);

        _hideMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 0);

        HideMeshRenderer.enabled = false;
        TopMeshRenderer.enabled  = true;
    }

    public void HideInnerMesh()
    {
        if (MeshObjectData == null) return;
        HideMeshRenderer.enabled = false;
    }

    public List<Vector3> GetSpiralPath() => _spiralPath;

    #endregion

    #region SUPPORTIVE

    public void DecorObjectCheckAlongWoolRotation(float progress)
    {
        if (DecoreControls == null || DecoreControls.Count == 0 || !WoolAnimationData) return;
        progress = Mathf.Clamp01((float)progress);
        List<DecoreControl> decorObjectToDrop = DecoreControls
           .Where(x => x.WoolProgressStartDrop <= progress)
           .ToList();
        if (decorObjectToDrop.Count == 0) return;
        for (int i = 0; i < decorObjectToDrop.Count; i++)
        {
            if (decorObjectToDrop[i] == null) continue;
            var renderer = decorObjectToDrop[i]
               .GetComponent<Renderer>();
            if (renderer == null)
            {
                decorObjectToDrop[i]
                   .PulseOutOfParrentWool(WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor);
            }
            else
            {
                decorObjectToDrop[i]
                   .PulseOutOfParrentWool(renderer.bounds.center, WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor);
            }

            DecoreControls.Remove(decorObjectToDrop[i]);
            RemovedDecoreControls.Add(decorObjectToDrop[i]);
        }
    }

    public void PulseAllDecorObjects()
    {
        if (DecoreControls.Count == 0) return;
        for (int i = 0; i < DecoreControls.Count; i++)
        {
            var renderer = DecoreControls[i]
               .GetComponent<Renderer>();
            if (renderer == null)
            {
                DecoreControls[i]
                   .PulseOutOfParrentWool(WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor);
            }
            else
            {
                DecoreControls[i]
                   .PulseOutOfParrentWool(renderer.bounds.center, WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor);
            }
        }
    }

    public void ResetWoolState()
    {
        DisplayColor();
        MaterialPropertyBlock woolProperties = new MaterialPropertyBlock();
        TopMeshRenderer.GetPropertyBlock(woolProperties);

        if (false)
        {
            _topMaterialPropertyBlock.SetFloat(T_Utilities.ShaderPropertiesLib.Display,
                    Mathf.Clamp01(1)
                );
            TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
        }
        else
        {
            woolProperties.SetFloat(T_Utilities.ShaderPropertiesLib.Display, 1);
            TopMeshRenderer.SetPropertyBlock(woolProperties);
        }

        StartCoroutine(ResetDecorObjects());
    }

    private IEnumerator ResetDecorObjects()
    {
        foreach (var decoreControl in RemovedDecoreControls)
        {
            decoreControl.gameObject.SetActive(true);
            yield return null;
            decoreControl.ResetDecorTransformStatusAsync();
        }
    }

    #endregion

    // Kiểm tra điểm p có nằm trong tam giác (v0,v1,v2) không (barycentric)
    private static bool PointInTriangle(Vector3 p, Vector3 v0_, Vector3 v1_, Vector3 v2_)
    {
        // Sử dụng tọa độ barycentric để kiểm tra
        Vector3 v0v1  = v1_ - v0_;
        Vector3 v0v2  = v2_ - v0_;
        Vector3 v0p   = p   - v0_;
        float   d00   = Vector3.Dot(v0v1, v0v1);
        float   d01   = Vector3.Dot(v0v1, v0v2);
        float   d11   = Vector3.Dot(v0v2, v0v2);
        float   d20   = Vector3.Dot(v0p,  v0v1);
        float   d21   = Vector3.Dot(v0p,  v0v2);
        float   denom = d00 * d11 - d01 * d01;
        if (Mathf.Abs(denom) < 1e-6f) return false;
        float v = (d11 * d20 - d01 * d21) / denom;
        float w = (d00 * d21 - d01 * d20) / denom;
        float u = 1.0f - v - w;
        return (u >= 0) && (v >= 0) && (w >= 0);
    }

    // Tìm điểm gần nhất trên đoạn thẳng ab tới p
    private static Vector3 ClosestPointOnSegment(Vector3 p, Vector3 a, Vector3 b)
    {
        // Chiếu p lên đoạn ab, clamp t trong [0,1]
        Vector3 ab = b - a;
        float   t  = Vector3.Dot(p - a, ab) / ab.sqrMagnitude;
        t = Mathf.Clamp01(t);
        return a + ab * t;
    }
    
    
}
