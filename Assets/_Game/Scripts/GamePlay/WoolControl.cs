using System;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class WoolControl : MonoBehaviour
{
    #region PROPERTIES

    [Header("WOOL ID")] public long WoolID;

    public ColorPalleteData_new colorPalleteData;

    public bool debugUV;

    [Header("Mesh Object")] public MeshRenderer TopMeshRenderer;
    public MeshRenderer HideMeshRenderer;
    public MeshCollider MeshCollider;
    public Material MainMaterial;
    public Material TranparentMaterial;
    public WoolAnimationData WoolAnimationData;

    public List<DecoreControl> DecoreControls;
    public List<DecoreControl> RemovedDecoreControls;

    public List<WoolControl> _childWoolControls = new();

    public MeshFilter MeshFilter;

    [HideInInspector] public bool IsSetColorHightest;

    private MaterialPropertyBlock _topMaterialPropertyBlock;
    private MaterialPropertyBlock _hideMaterialPropertyBlock;

    private string _currentColor;

    private int _indexLayer;

    [SerializeField] private List<Vector3> _spiralPath = new();
    [SerializeField] private List<float> _spiralPathUVY = new();

    [SerializeField] private float _uvMin;
    [SerializeField] private float _uvMax;
    public int WeightOrder;

    public static bool IsIgnoreXRay;

    private List<string> _colorStack = new();
    public List<string> ColorStack => _colorStack;

    [Space]
    [Header("DYNAMIC SCALE")]
    public float WoolMinimumScaleValue = 0.85f;
    private bool IsDynamicScale = true;
    private float MaximumScaleValue = 1f;
    private float scaleStep;
    private float currentScaleValue;
    private float layerCount = 1f;
    private static readonly float MinUvRange = 0.0001f;

    #region custom attributes

    [Serializable] //temporary
    public class DecorObjectSetting
    {
        public DecoreControl DecorObject;
        [Range(0, 1f)] public float WoolProgressStartSrop = 0.5f;
    }

    #endregion

    #endregion

#if UNITY_EDITOR
    private bool _editorMeshSyncScheduled;

    private void OnValidate()
    {
        TopMeshRenderer ??= GetComponent<MeshRenderer>();
        HideMeshRenderer ??= transform.GetChild(0).GetComponent<MeshRenderer>();
        MeshCollider ??= GetComponent<MeshCollider>();
        bool sameMesh = HideMeshRenderer != null && HideMeshRenderer == TopMeshRenderer;
        if (sameMesh) HideMeshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        // Avoid assigning sharedMesh directly inside OnValidate, Unity can throw
        // "SendMessage cannot be called during ... OnValidate".
        ScheduleEditorMeshSync();

        EditorUtility.SetDirty(this);
    }

    private void ScheduleEditorMeshSync()
    {
        if (_editorMeshSyncScheduled) return;
        _editorMeshSyncScheduled = true;
        EditorApplication.delayCall += DelayedEditorMeshSync;
    }

    private void DelayedEditorMeshSync()
    {
        _editorMeshSyncScheduled = false;
        if (this == null) return;
        if (MeshFilter == null || MeshFilter.sharedMesh == null) return;

        if (MeshCollider is MeshCollider meshCol)
        {
            meshCol.sharedMesh = MeshFilter.sharedMesh;
        }

        if (HideMeshRenderer != null && HideMeshRenderer.TryGetComponent<MeshFilter>(out var childFilter))
        {
            childFilter.sharedMesh = MeshFilter.sharedMesh;
        }
    }
#endif

    private void Awake()
    {
        EnsureMaterialPropertyBlocks();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayColor();
        }
    }
#endif

    #region MAIN_METHODS

    public void SetColorStack(List<string> colors)
    {
        _colorStack = new(colors);
        if (_colorStack.Count > 0) _hightestColor = _colorStack[0];
    }

    public void InitMesh()
    {
        _indexLayer = 0;
        MeshCollider.enabled = true;
        if (_colorStack.Count > 0) _currentColor = _colorStack[0];
        _isTransparent = false;
        DisplayColor();
        PushColor();
        InitializeDynamicScale();
    }

    public void SetActiveDecor(bool isActive)
    {
        foreach (var decor in DecoreControls)
        {
            decor?.gameObject.SetActive(isActive);
        }
    }

    public void PushColor()
    {
        if (!HideMeshRenderer || _indexLayer >= _colorStack.Count || _hideMaterialPropertyBlock == null) return;

        var totalColor = _colorStack.Count;
        _indexLayer++;
        if (totalColor > 1 && _indexLayer == 2)
        {
            TryApplyHideMaterial(_colorStack[1]);
        }
        HideMeshRenderer.enabled = totalColor > 1;
    }

    private bool _isTransparent;

    public void SetTranparentWool(bool isTranparent)
    {
#if USE_ACCOUNT_TOOL
        if (IsIgnoreXRay) return;
#endif
        if (_isPlayAnim) return;

        try
        {
            HideMeshRenderer.gameObject.SetActive(isTranparent);
            if (_colorStack.Count > 1)
            {
                scaleStep = (MaximumScaleValue - WoolMinimumScaleValue) / layerCount;
                float nextScale = Mathf.Clamp(currentScaleValue - scaleStep * 2, WoolMinimumScaleValue, MaximumScaleValue);
                if (WoolMinimumScaleValue >= MaximumScaleValue) nextScale = MaximumScaleValue - 0.05f;
                _hideMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, nextScale);
                ApplyHideRendererProperties();
                TryApplyHideMaterial(_colorStack[1]);
            }
            if (_colorStack.Count > 0 && colorPalleteData.colorPallete_New.TryGetValue(_colorStack[0], out var matTop))
            {
                TopMeshRenderer.sharedMaterial = isTranparent ? TranparentMaterial : matTop;
            }
            EnsureTopMaterialPropertyBlock();
            TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
            if (IsDynamicScale)
            {
                float scaleFactor = WoolMinimumScaleValue >= MaximumScaleValue ? MaximumScaleValue + 0.025f : currentScaleValue;
                _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, isTranparent ? scaleFactor : currentScaleValue);
            }
            else
            {
                _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleThreshold, isTranparent ? TranparentMaterial.GetFloat(ShaderPropertiesLib.ScaleThreshold) : 0f);
            }
            ApplyTopRendererProperties();
            _isTransparent = isTranparent;
        }
        catch (Exception ex)
        {
            Debug.LogError($"SetTranparentWool: {ex.Message}");
        }
    }

    public void WoolRotation()
    {
        StartCoroutine(AsyncWoolRotation());
    }

    public void ReFillMesh(string colorPush)
    {
        _isPlayAnim = true;
        List<string> newColorStack = new();
        newColorStack.Add(colorPush);
        foreach (var color in _colorStack)
        {
            newColorStack.Add(color);
        }
        _colorStack = newColorStack;
        StartCoroutine(AsyncWoolRotationReFill());
    }

    private bool _isPlayAnim;
    public bool IsPlayAnim => _isPlayAnim;

    private bool _isVacuumChose;

    public bool IsCheckClick;

    public void SetIsVacuumChose(bool isChose) => _isVacuumChose = isChose;

    private IEnumerator AsyncWoolRotation()
    {
        if (Time.deltaTime <= 0) yield break;
        if (_isPlayAnim || _isVacuumChose) yield break;
        IsCheckClick = true;
        if (IsQueueFull()) yield break;
        if (!HasValidWoolAnimationState()) yield break;
        if (!GamePlayManager.Instance.OnChoseColor(this, _spiralPath, _currentColor)) yield break;
        GamePlayManager.Instance.ActiveHandController(false);
        _isPlayAnim = true;

        try
        {
            _colorStack.RemoveAt(0);
            IsCheckClick = false;
        }
        catch { }

        string nextColor;
        var totalColor = PrepareHideRendererForNextLayer(out nextColor);
        MeshCollider.enabled = totalColor > 0;

        var totalTime = WoolAnimationData.Duration + WoolAnimationData.OffSet;
        float timer = 0f;

        //DeviceVibrationManager.Instance?.ExecuteWoolVibration(totalTime);
        //UserBehaviorTracker.SendMoveWoolTracking();

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = ShouldSkipAnimationDelays();

        InitializeUvContext(out float minUVY, out float uvRange, out int spiralPathCount);
        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);

        while (timer < totalTime)
        {
            if (_spiralPathUVY.Count == 0) break;
            float t = timer / totalTime;
            float normalizedUVY = GetNormalizedUVY(t, spiralPathCount, minUVY, uvRange);
            UpdateTopDisplay(normalizedUVY);

            DecorObjectCheckAlongWoolRotation(t);

            timer += Time.deltaTime;
            //Debug.Log($"Deltatime: {Time.deltaTime}, Timer: {timer}, Display: {normalizedUVY}");

            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
                // Skip animation by jumping to end
                timer = totalTime;
            }
        }

        PulseAllDecorObjects();

        if (!nextColor.Equals(ShaderPropertiesLib.IgnoredWoolColorKey))
        {
            try
            {
                TryApplyTopMaterial(nextColor);
            }
            catch { Debug.LogError(name + "- Check colorbalette for key - " + nextColor); }

            SetTopDisplay(1f);
            _currentColor = nextColor;
            ApplyTopRendererProperties();

            scaleStep = (MaximumScaleValue - WoolMinimumScaleValue) / layerCount;
            float startScale = Mathf.Clamp(currentScaleValue - scaleStep * 2f, WoolMinimumScaleValue, MaximumScaleValue);
            currentScaleValue = Mathf.Clamp(currentScaleValue - scaleStep, WoolMinimumScaleValue, MaximumScaleValue);
            float nextScale = Mathf.Clamp(currentScaleValue - scaleStep, WoolMinimumScaleValue, MaximumScaleValue);
            _hideMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, nextScale);
            float midscale = currentScaleValue + scaleStep / 3f;
            if (midscale <= currentScaleValue) midscale = currentScaleValue + 0.025f;
            ApplyHideRendererProperties();
            HideMeshRenderer.gameObject.SetActive(false);
            yield return StartCoroutine(PumpMeshAnimation(startScale, midscale, currentScaleValue, WoolAnimationData.MeshPumpAnimDuration));

            _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, currentScaleValue); //just to make sure lmao
            ApplyTopRendererProperties();
        }

        ApplyTopRendererProperties();
        _indexLayer--;

        if (totalColor == 1) HideMeshRenderer.enabled = false;
        if (totalColor == 0)
        {
            TopMeshRenderer.enabled = false;
            DecreaseWeightForMeshChild();
            GameEventManager.OnAWoolMeshCompleted?.Invoke();
        }

        _isPlayAnim = false;
        if (!_isTransparent)
            HideMeshRenderer.gameObject.SetActive(false);
    }

    private IEnumerator AsyncWoolRotationReFill()
    {
        _currentColor = _colorStack[0];
        TopMeshRenderer.enabled = true;
        MeshCollider.enabled = true;
        TryApplyTopMaterial(_colorStack[0]);
        ApplyTopRendererProperties();
        if (_colorStack.Count > 1)
        {
            HideMeshRenderer.enabled = true;
            HideMeshRenderer.gameObject.SetActive(true);
            TryApplyHideMaterial(_colorStack[1]);
            ApplyHideRendererProperties();
        }
        else HideMeshRenderer.gameObject.SetActive(false);

        var totalTime = WoolAnimationData.Duration + WoolAnimationData.OffSet;
        float timer = 0f;

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = ShouldSkipAnimationDelays();

        InitializeUvContext(out float minUVY, out float uvRange, out int spiralPathCount);
        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);

        while (timer < totalTime)
        {
            float t = 1 - timer / totalTime;
            float normalizedUVY = GetNormalizedUVY(t, spiralPathCount, minUVY, uvRange);
            UpdateTopDisplay(normalizedUVY);

            DecorObjectCheckAlongWoolRotation(t);

            timer += Time.deltaTime;
            Debug.Log($"Deltatime: {Time.deltaTime}, Timer: {timer}, Display: {normalizedUVY}");
            ;

            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
                // Skip animation by jumping to end
                timer = totalTime;
            }
        }

        _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, 1);
        ApplyTopRendererProperties();


        HideMeshRenderer.gameObject.SetActive(false);
        HideMeshRenderer.enabled = _colorStack.Count > 1;
        _isPlayAnim = false;
    }

    public void PLayAnim(string color)
    {
        try
        {
            //if (CoreGameplayManager.Instance.CurrentGameState != GameState.InGame) return;
            StartCoroutine(ExecuteAnim(color));
        }
        catch { }
    }

    private IEnumerator ExecuteAnim(string color)
    {
        if (Time.deltaTime <= 0) yield break;
        if (!HasValidWoolAnimationState()) yield break;
        _isPlayAnim = true;

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

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = ShouldSkipAnimationDelays();

        cubeCount++;
        for (var index = 0; index < _colorStack.Count; index++)
        {
            var c = _colorStack[index];
            if (!c.Equals(color)) continue;
            _colorStack.RemoveAt(index);
            break;
        }
        string nextColor;
        var totalColor = PrepareHideRendererForNextLayer(out nextColor);

        var totalTime = WoolAnimationData.Duration + WoolAnimationData.OffSet;
        float timer = 0f;

        InitializeUvContext(out float minUVY, out float uvRange, out int spiralPathCount);
        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
        while (timer < totalTime)
        {
            float t = timer / totalTime;
            float normalizedUVY = GetNormalizedUVY(t, spiralPathCount, minUVY, uvRange);
            UpdateTopDisplay(normalizedUVY);

            timer += Time.deltaTime;

            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
                // Skip animation by jumping to end
                timer = totalTime;
            }
        }

        if (!nextColor.Equals(ShaderPropertiesLib.IgnoredWoolColorKey))
        {
            try
            {
                TryApplyTopMaterial(nextColor);
            }
            catch { Debug.LogError(name + "- Check colorbalette for key - " + nextColor); }

            SetTopDisplay(1f);
            _currentColor = nextColor;
            ApplyTopRendererProperties();

            scaleStep = (MaximumScaleValue - WoolMinimumScaleValue) / layerCount;
            float startScale = Mathf.Clamp(currentScaleValue - scaleStep * 2f, WoolMinimumScaleValue, MaximumScaleValue);
            currentScaleValue = Mathf.Clamp(currentScaleValue - scaleStep, WoolMinimumScaleValue, MaximumScaleValue);
            float nextScale = Mathf.Clamp(currentScaleValue - scaleStep, WoolMinimumScaleValue, MaximumScaleValue);
            _hideMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, nextScale);
            float midscale = currentScaleValue + scaleStep / 3f;
            if (midscale <= currentScaleValue) midscale = currentScaleValue + 0.025f;
            ApplyHideRendererProperties();
            HideMeshRenderer.gameObject.SetActive(false);
            yield return StartCoroutine(PumpMeshAnimation(startScale, midscale, currentScaleValue, WoolAnimationData.MeshPumpAnimDuration));

            _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, currentScaleValue); //just to make sure lmao
            ApplyTopRendererProperties();
        }

        ApplyTopRendererProperties();
        _indexLayer--;
        MeshCollider.enabled = totalColor > 0;
        if (totalColor == 1)
            HideMeshRenderer.enabled = false;
        if (totalColor == 0)
        {
            TopMeshRenderer.enabled = false;
            DecreaseWeightForMeshChild();
        }
        _isVacuumChose = false;
        _isPlayAnim = false;
        HideMeshRenderer.gameObject.SetActive(false);

        if (!skipAnimationDelays)
        {
            yield return null;
        }
    }

    private IEnumerator PumpMeshAnimation(float startScale, float midScale, float endScale, float duration)
    {
        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = ShouldSkipAnimationDelays();

        float firstHalfDuration = duration / 1.5f;
        float secondHalfDuration = duration / 3f;

        float t = 0f;
        float scaleValue = startScale;
        if (_topMaterialPropertyBlock == null)
        {
            EnsureTopMaterialPropertyBlock();
            TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
        }
        _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, scaleValue);

        if (skipAnimationDelays)
        {
            // Skip animation by setting directly to final state
            _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, endScale);
            ApplyTopRendererProperties();
            yield break;
        }

        while (t < firstHalfDuration)
        {
            if (!TopMeshRenderer) break;
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / firstHalfDuration);
            scaleValue = Mathf.Lerp(startScale, midScale, progress);
            _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, scaleValue);
            ApplyTopRendererProperties();

            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
                // Skip animation by jumping to end
                t = firstHalfDuration;
            }
        }

        t = 0f;
        while (t < secondHalfDuration)
        {
            if (!TopMeshRenderer) break;
            t += Time.deltaTime;
            float progress = Mathf.Clamp01(t / secondHalfDuration);
            scaleValue = Mathf.Lerp(midScale, endScale, progress);
            _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, scaleValue);
            ApplyTopRendererProperties();

            if (!skipAnimationDelays)
            {
                yield return null;
            }
            else
            {
                // Skip animation by jumping to end
                t = secondHalfDuration;
            }
        }
        _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, endScale);
        ApplyTopRendererProperties();
    }

    private void DecreaseWeightForMeshChild()
    {
        if (_childWoolControls.Count == 0) return;
        //
        foreach (var child in _childWoolControls)
        {
            if (child == null) continue;
            child.WeightOrder -= 1;
            child.DecreaseWeightForMeshChild();
        }
    }

    private static int cubeCount = 0;


    private void DisplayColor()
    {
        try
        {
            if (_colorStack == null) return;
            EnsureMaterialPropertyBlocks();
            TryApplyTopMaterial(_colorStack[0]);
            TopMeshRenderer?.GetPropertyBlock(_topMaterialPropertyBlock);
            HideMeshRenderer?.GetPropertyBlock(_hideMaterialPropertyBlock);
            SetTopDisplay(1f);
            if (HideMeshRenderer) HideMeshRenderer.enabled = true;
            if (TopMeshRenderer) TopMeshRenderer.enabled = true;
        }
        catch { }
    }

    public void DisplayColor(Color albedo)
    {
        // if (_colorStack == null) return;
        // if (_topMaterialPropertyBlock == null) _topMaterialPropertyBlock = new MaterialPropertyBlock();
        // if (TopMeshRenderer == null) return; // Ngăn NullReferenceException nếu TopMeshRenderer đã bị destroy
        //     
        // TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
        // _topMaterialPropertyBlock.SetColor(ShaderPropertiesLib.Color, albedo);
        // _topMaterialPropertyBlock.SetFloat(ShaderPropertiesLib.Display,      1);
        // TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
        //
        // TopMeshRenderer.enabled = true; // Đã kiểm tra null ở trên nên không cần check lại
    }

    public void DisplayColorSmoothly()
    {
        if (_colorStack == null) return;
        EnsureTopMaterialPropertyBlock();
        if (TopMeshRenderer == null) return; // Ngăn NullReferenceException nếu TopMeshRenderer đã bị destroy

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = ShouldSkipAnimationDelays();

        Color currnetColor = Color.gray;
        string targetColor = _colorStack[0];

        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);

        _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, 1);

        ApplyTopRendererProperties();

        try
        {
            if (skipAnimationDelays)
            {
                // Skip animation by setting directly to final color
                TryApplyTopMaterial(targetColor);
                ApplyTopRendererProperties();
            }
            else { }
        }
        catch
        {
            // Xử lý trường hợp colorPalleteData hoặc colorPallete bị null hoặc thiếu key
            Debug.LogError($"{name} - Check colorPallete for key - {targetColor}");
        }

        TopMeshRenderer.enabled = true; // Đã kiểm tra null ở trên
    }

    public void BuildUpModelSmoothly(float duration)
    {
        if (_colorStack == null) return;
        EnsureTopMaterialPropertyBlock();

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = ShouldSkipAnimationDelays();

        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);

        _topMaterialPropertyBlock.SetColor(ShaderPropertiesLib.Color, Color.gray);

        ApplyTopRendererProperties();

        if (skipAnimationDelays)
        {
            // Skip animation by setting directly to final state
            _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, 1);
            ApplyTopRendererProperties();
        }
        else
        {
            float currentDisplay = 0;
            DOTween.To(() => currentDisplay, x =>
            {
                currentDisplay = x;
                _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, currentDisplay);
                ApplyTopRendererProperties();
            }
                  , 1, duration
                );
        }

        TopMeshRenderer.enabled = true;
    }


    public void ClearThisWool()
    {
        if (_colorStack == null) return;
        EnsureTopMaterialPropertyBlock();
        HideMeshRenderer.enabled = false;
        TopMeshRenderer.enabled = true;

        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);

        _topMaterialPropertyBlock.SetColor(ShaderPropertiesLib.Color, Color.gray);
        _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, 0);

        ApplyTopRendererProperties();

        _hideMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, 0);
    }

    public void HideInnerMesh()
    {
        if (_colorStack == null) return;
        HideMeshRenderer.enabled = false;
    }

    public void ChangeLayer(string layer)
    {
        TopMeshRenderer.gameObject.layer = LayerMask.NameToLayer(layer);
        HideMeshRenderer.gameObject.layer = LayerMask.NameToLayer(layer);
        foreach (var decor in DecoreControls)
        {
            if (decor == null) continue;
            decor.ChangeLayer(layer);
        }

        foreach (var decor in RemovedDecoreControls)
        {
            if (decor == null) continue;
            decor.ChangeLayer(layer);
        }
    }

    public List<Vector3> GetSpiralPath() => _spiralPath;


#if UNITY_EDITOR

    public void SetWeightForChild()
    {
        foreach (var child in _childWoolControls)
        {
            if (child == null) continue;
            child.WeightOrder = WeightOrder + 1;
            child.SetWeightForChild();
        }
    }

    public void SetCenterDecores(Transform center)
    {
        foreach (var decore in DecoreControls)
        {
            decore.SetCenterDecore(center);
        }
    }

    // [ContextMenu("Bake Spiral Path")]
    // public void BakeSpiralPath()
    // {
    //     if (!gameObject.TryGetComponent<MeshSlicer>(out var meshSlicer))
    //     {
    //         meshSlicer = gameObject.AddComponent<MeshSlicer>();
    //     }
    //     meshSlicer.GenerateSlicesAndSpiral();
    //     _spiralPath = meshSlicer.GetSpiralPositions();
    //     _spiralPathUVY = meshSlicer.spiralPathUVY;
    //     _spiralPath.Reverse();
    //     _spiralPathUVY.Reverse();
    //     _uvMin = _spiralPathUVY.Min();
    //     _uvMax = _spiralPathUVY.Max();


    //     try
    //     {
    //         if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
    //         {
    //             // Prefab instance
    //             DestroyImmediate(meshSlicer);
    //             PrefabUtility.ApplyRemovedComponent(gameObject, meshSlicer, InteractionMode.UserAction);
    //         }
    //         else
    //         {
    //             // Regular GameObject
    //             DestroyImmediate(meshSlicer);
    //         }
    //     }
    //     catch { }
    // }

    [ContextMenu("Smooth Spiral Path")]
    public void SmoothSpiralPath()
    {
        if (_spiralPath == null || _spiralPathUVY == null || _spiralPath.Count < 2 ||
            _spiralPath.Count != _spiralPathUVY.Count)
            return;
        var mesh = MeshFilter.sharedMesh;
        var vertices = mesh.vertices;
        var uvs = mesh.uv2;

        if (mesh.vertices.Length != mesh.uv2.Length)
        {
            Debug.LogError($"{mesh.name} chưa có uv 2");
            return;
        }

        int N = _spiralPath.Count;
        if (N < 2) return;

        // Tính tổng chiều dài path
        float totalLen = 0f;
        float[] segLen = new float[N - 1];
        for (int i = 0; i < N - 1; i++)
        {
            segLen[i] = Vector3.Distance(_spiralPath[i], _spiralPath[i + 1]);
            totalLen += segLen[i];
        }

        // Tính các mốc đều
        float[] targetDist = new float[N];
        for (int i = 0; i < N; i++)
            targetDist[i] = i * totalLen / (N - 1);

        // Tạo path mới với khoảng cách đều
        List<Vector3> newPath = new List<Vector3>(N);
        List<float> newUVY = new List<float>(N);
        int segIdx = 0;
        float currDist = 0f;
        newPath.Add(_spiralPath[0]);
        newUVY.Add(_spiralPathUVY[0]);
        for (int i = 1; i < N - 1; i++)
        {
            float d = targetDist[i];
            // Tìm đoạn chứa d
            while (segIdx < segLen.Length - 1 && currDist + segLen[segIdx] < d)
            {
                currDist += segLen[segIdx];
                segIdx++;
            }

            float t = (d - currDist) / segLen[segIdx];
            Vector3 pos = Vector3.Lerp(_spiralPath[segIdx], _spiralPath[segIdx + 1], t);

            // Tìm vertex mesh gần nhất để lấy lại UVY
            float minDist = float.MaxValue;
            float uvY = 0f;
            for (int k = 0; k < vertices.Length; k++)
            {
                float dist = Vector3.Distance(pos, vertices[k]);
                if (dist < minDist)
                {
                    minDist = dist;
                    uvY = uvs[k].y;
                }
            }

            newPath.Add(pos);
            newUVY.Add(uvY);
        }

        newPath.Add(_spiralPath[N - 1]);
        newUVY.Add(_spiralPathUVY[N - 1]);

        _spiralPath = newPath;
        _spiralPathUVY = newUVY;

        // Smooth lại UVY: giảm dần đều từ đầu đến cuối
        float startUVY = _spiralPathUVY[0];
        float endUVY = _spiralPathUVY[_spiralPathUVY.Count - 1];
        for (int i = 0; i < _spiralPathUVY.Count; i++)
        {
            _spiralPathUVY[i] = Mathf.Lerp(startUVY, endUVY, i / (float)(N - 1));
        }
    }

    // private void OnDrawGizmosSelected()
    // {
    //     if (_spiralPath == null || _spiralPath.Count < 2) return;
    //     // Vẽ line spiral path
    //     Gizmos.color = Color.cyan;
    //     for (int i = 0; i < _spiralPath.Count - 1; i++)
    //     {
    //         Gizmos.DrawLine(transform.TransformPoint(_spiralPath[i]), transform.TransformPoint(_spiralPath[i + 1]));
    //     }

    //     // Vẽ điểm với màu theo UVY
    //     if (!debugUV) return;
    //     float minUVY = _spiralPathUVY.Min();
    //     float maxUVY = _spiralPathUVY.Max();
    //     for (int i = 0; i < _spiralPath.Count; i++)
    //     {
    //         float t = Mathf.InverseLerp(minUVY, maxUVY, _spiralPathUVY[i]);
    //         Color c = Color.Lerp(Color.blue, Color.red, t);
    //         Gizmos.color = c;
    //         Gizmos.DrawSphere(transform.TransformPoint(_spiralPath[i]), 0.01f);
    //     }
    // }

#endif

    #endregion

    #region SUPPORTIVE
    private void InitializeDynamicScale()
    {
        layerCount = (float)_colorStack.Count;
        layerCount = Mathf.Max(1f, layerCount);
        currentScaleValue = MaximumScaleValue;
        scaleStep = (MaximumScaleValue - WoolMinimumScaleValue) / layerCount;
        if (IsDynamicScale)
        {
            float nextScale = Mathf.Clamp(currentScaleValue - scaleStep * 2, WoolMinimumScaleValue, MaximumScaleValue);
            _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, MaximumScaleValue);
            _hideMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, nextScale);
            ApplyTopRendererProperties();
            ApplyHideRendererProperties();
        }
    }
    public void DecorObjectCheckAlongWoolRotation(float progress)
    {
        try
        {
            if (DecoreControls == null || DecoreControls.Count == 0 || !WoolAnimationData) return;
            progress = Mathf.Clamp01((float)progress);
            List<DecoreControl> decorObjectToDrop = DecoreControls
               .Where(x => x != null && x.WoolProgressStartDrop <= progress)
               .ToList();
            if (decorObjectToDrop.Count == 0) return;
            for (int i = 0; i < decorObjectToDrop.Count; i++)
            {
                if (decorObjectToDrop[i] == null) continue;
                decorObjectToDrop[i].PulseOutofParrentWool(WoolAnimationData.ForceValue, WoolAnimationData.RandomDirrectionFactor, WoolAnimationData.SpeedRotation);

                DecoreControls.Remove(decorObjectToDrop[i]);
                RemovedDecoreControls.Add(decorObjectToDrop[i]);
            }
        }
        catch { }
    }

    public void PulseAllDecorObjects()
    {
        if (DecoreControls.Count == 0) return;
        for (int i = 0; i < DecoreControls.Count; i++)
        {
            if (DecoreControls[i] == null) continue;
            var renderer = DecoreControls[i].GetComponent<Renderer>();
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

    private string _hightestColor;
    [ContextMenu("RESET MODEL")]
    public void ResetWoolState()
    {
        DisplayColor();
        EnsureTopMaterialPropertyBlock();
        if (TopMeshRenderer != null) TopMeshRenderer.enabled = true;
        if (!string.IsNullOrEmpty(_hightestColor)) TryApplyTopMaterial(_hightestColor);
        TopMeshRenderer?.GetPropertyBlock(_topMaterialPropertyBlock);

        if (false)
        {
            SetTopDisplay(Mathf.Clamp01(1f));
        }
        else
        {
            SetTopDisplay(1f);
            SetTopScaleFactor(1f);
        }
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ResetDecorObjects());
        }
    }

    private IEnumerator ResetDecorObjects()
    {
        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = ShouldSkipAnimationDelays();

        foreach (var decoreControl in RemovedDecoreControls)
        {
            decoreControl.gameObject.SetActive(true);

            if (!skipAnimationDelays)
            {
                yield return null;
            }

            decoreControl.ResetDecorTransformStatusAsync();
        }
    }

    private void ApplyTopRendererProperties()
    {
        TopMeshRenderer?.SetPropertyBlock(_topMaterialPropertyBlock);
    }

    private void ApplyHideRendererProperties()
    {
        HideMeshRenderer?.SetPropertyBlock(_hideMaterialPropertyBlock);
    }

    private void InitializeUvContext(out float minUVY, out float uvRange, out int spiralPathCount)
    {
        minUVY = _uvMin;
        float maxUVY = _uvMax;
        uvRange = Mathf.Max(MinUvRange, maxUVY - minUVY);
        spiralPathCount = _spiralPath.Count;
    }

    private void UpdateTopDisplay(float normalizedUVY)
    {
        _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, Mathf.Clamp01(normalizedUVY));
        ApplyTopRendererProperties();
    }

    private float GetNormalizedUVY(float progress, int pathCount, float minUVY, float uvRange)
    {
        float idx = progress * (pathCount - 1);
        int idx0 = Mathf.Clamp(Mathf.FloorToInt(idx), 0, pathCount - 1);
        int idx1 = Mathf.Clamp(idx0 + 1, 0, pathCount - 1);
        float lerpT = idx - idx0;

        float uvy0 = _spiralPathUVY[idx0];
        float uvy1 = _spiralPathUVY[idx1];
        float uvy = Mathf.Lerp(uvy0, uvy1, lerpT);
        return (uvy - minUVY) / uvRange;
    }

    private bool IsQueueFull()
    {
        return GamePlayManager.Instance &&
               GamePlayManager.Instance.QueueCount == GamePlayManager.Instance.TotalQueueActiveCount;
    }

    private bool HasValidWoolAnimationState()
    {
        return HideMeshRenderer && WoolAnimationData && TopMeshRenderer &&
               _spiralPathUVY != null && _topMaterialPropertyBlock != null &&
               MeshCollider && _colorStack != null && _colorStack.Count > 0;
    }

    private int PrepareHideRendererForNextLayer(out string nextColor)
    {
        nextColor = ShaderPropertiesLib.IgnoredWoolColorKey;
        var totalColor = _colorStack.Count;

        HideMeshRenderer.gameObject.SetActive(true);

        if (totalColor == 0)
        {
            HideMeshRenderer.enabled = false;
        }
        else
        {
            nextColor = _colorStack.Count > 0 ? _colorStack[0] : ShaderPropertiesLib.IgnoredWoolColorKey;
            float nextScale = Mathf.Clamp(currentScaleValue - scaleStep * 2f, WoolMinimumScaleValue, MaximumScaleValue);
            TryApplyHideMaterial(nextColor);
            UpdateHideRendererForNextLayer(nextScale);
        }

        ApplyHideRendererProperties();
        return totalColor;
    }

    private bool TryApplyTopMaterial(string colorKey)
    {
        if (colorPalleteData.colorPallete_New.TryGetValue(colorKey, out var mat))
        {
            TopMeshRenderer.sharedMaterial = mat;
            return true;
        }

        return false;
    }

    private bool TryApplyHideMaterial(string colorKey)
    {
        if (colorPalleteData.colorPallete_New.TryGetValue(colorKey, out var mat))
        {
            HideMeshRenderer.sharedMaterial = mat;
            return true;
        }

        return false;
    }

    private void SetTopDisplay(float display)
    {
        _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, display);
        ApplyTopRendererProperties();
    }

    private void SetTopScaleFactor(float scaleFactor)
    {
        _topMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, scaleFactor);
        ApplyTopRendererProperties();
    }

    private void UpdateHideRendererForNextLayer(float nextScale)
    {
        _hideMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.Display, 1);
        _hideMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleFactor, nextScale);
        if (WoolMinimumScaleValue >= MaximumScaleValue)
            _hideMaterialPropertyBlock?.SetFloat(ShaderPropertiesLib.ScaleThreshold, -0.01f);
    }

    private void EnsureTopMaterialPropertyBlock()
    {
        if (_topMaterialPropertyBlock == null) _topMaterialPropertyBlock = new MaterialPropertyBlock();
    }

    private void EnsureHideMaterialPropertyBlock()
    {
        if (_hideMaterialPropertyBlock == null) _hideMaterialPropertyBlock = new MaterialPropertyBlock();
    }

    private void EnsureMaterialPropertyBlocks()
    {
        EnsureTopMaterialPropertyBlock();
        EnsureHideMaterialPropertyBlock();
    }

    private static bool ShouldSkipAnimationDelays()
    {
        return false;
    }
    #endregion

}
