using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
public static class RollWoolAnimationExtensions
{
    public static WoolRollAnimator SetColor(this WoolRollAnimator woolRollAnimator, string mainColor)
    {
        woolRollAnimator._currentColor = mainColor;
        if (!woolRollAnimator.colorPalleteData.colorPallete_New.TryGetValue(mainColor, out var mat)) return null;
        if (mat == null) return null;

        var meshRenderers = woolRollAnimator.MeshRenderers;
        Color baseColor = mat.color;
        Color aocolor = mat.HasProperty(ShaderPropertiesLib.AOColor) ? mat.GetColor(ShaderPropertiesLib.AOColor) : Color.white;
        Color darkColor = mat.HasProperty(ShaderPropertiesLib.DarkThreadColor) ? mat.GetColor(ShaderPropertiesLib.DarkThreadColor) : Color.white;
        Color shadowColor = mat.HasProperty(ShaderPropertiesLib.ShadowColor) ? mat.GetColor(ShaderPropertiesLib.ShadowColor) : Color.white;
        float saturation = mat.HasProperty(ShaderPropertiesLib.Saturation) ? mat.GetFloat(ShaderPropertiesLib.Saturation) : 1f;
        float brightness = mat.HasProperty(ShaderPropertiesLib.Brightness) ? mat.GetFloat(ShaderPropertiesLib.Brightness) : 3f;
        float diffuse = mat.HasProperty(ShaderPropertiesLib.DiffusePower) ? mat.GetFloat(ShaderPropertiesLib.DiffusePower) : 0.5f;
        float shadowStrength = mat.HasProperty(ShaderPropertiesLib.ShadowStrength) ? mat.GetFloat(ShaderPropertiesLib.ShadowStrength) : 1f;
        float shadowExposure = mat.HasProperty(ShaderPropertiesLib.ShadowExposure) ? mat.GetFloat(ShaderPropertiesLib.ShadowExposure) : 1f;

        for (var i = 0; i < meshRenderers.Count; i++)
        {
            if (meshRenderers[i] == null) continue;
            var runtimeMaterial = meshRenderers[i].material;
            if (runtimeMaterial == null) continue;

            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.Color))
            {
                runtimeMaterial.SetColor(ShaderPropertiesLib.Color, baseColor);
            }
            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.AOColor))
            {
                runtimeMaterial.SetColor(ShaderPropertiesLib.AOColor, aocolor);
            }
            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.Saturation))
            {
                runtimeMaterial.SetFloat(ShaderPropertiesLib.Saturation, saturation);
            }
            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.Brightness))
            {
                runtimeMaterial.SetFloat(ShaderPropertiesLib.Brightness, brightness);
            }
            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.DiffusePower))
            {
                runtimeMaterial.SetFloat(ShaderPropertiesLib.DiffusePower, diffuse);
            }
            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.DarkThreadColor))
            {
                runtimeMaterial.SetColor(ShaderPropertiesLib.DarkThreadColor, darkColor);
            }
            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.ShadowColor))
            {
                runtimeMaterial.SetColor(ShaderPropertiesLib.ShadowColor, shadowColor);
            }
            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.ShadowStrength))
            {
                runtimeMaterial.SetFloat(ShaderPropertiesLib.ShadowStrength, shadowStrength);
            }
            if (runtimeMaterial.HasProperty(ShaderPropertiesLib.ShadowExposure))
            {
                runtimeMaterial.SetFloat(ShaderPropertiesLib.ShadowExposure, shadowExposure);
            }
        }

        return woolRollAnimator;
    }

    public static WoolRollAnimator SetWoolRedo(this WoolRollAnimator woolRollAnimator, WoolControl woolRedo)
    {
        woolRollAnimator._woolRedo = woolRedo;
        return woolRollAnimator;
    }

    public static WoolRollAnimator SetParent(this WoolRollAnimator woolRollAnimator, Transform parentTrans)
    {
        woolRollAnimator.transform.SetParent(parentTrans, true);
        return woolRollAnimator;
    }

    public static WoolRollAnimator ResetMesh(this WoolRollAnimator woolRollAnimator)
    {
        woolRollAnimator.ResetData();
        return woolRollAnimator;
    }

    public static void PlayAnimAddToQueue(this WoolRollAnimator woolRollAnimator, ParentType parentType)
    {
        if (!woolRollAnimator.isActiveAndEnabled) return;
        woolRollAnimator.StartCoroutine(woolRollAnimator.AnimAddToQueue(parentType));
    }

    public static void PlayAnimRedo(this WoolRollAnimator woolRollAnimator)
    {
        if (!woolRollAnimator.isActiveAndEnabled) return;
        woolRollAnimator.StartCoroutine(woolRollAnimator.AnimRedo());
    }

    public enum ParentType
    {
        CubeTarget,
        CubeQueue,
    }
}

public class WoolRollAnimator : MonoBehaviour, IPoolObject
{
    #region PROPERTIES

    public ColorPalleteData_new colorPalleteData;
    public SoundSO soundData;
    public WoolAnimationData WoolAnimationData;
    public List<MeshRenderer> MeshRenderers;

    internal string _currentColor = ShaderPropertiesLib.IgnoredWoolColorKey;
    internal bool _isPopToBroomPool;

    internal WoolControl _woolRedo;

    private readonly Quaternion DefaultRotation = Quaternion.Euler(30, 0, 0);
    private readonly Vector3 DefaultLocalPositionInTarget = new(0, -0.2f, 0.3f);
    private readonly Vector3 DefaultLocalPositionInQueue = new(0, -0.23f, 0.2f);
    private readonly Vector3 DefaultLocalPositionAtDisplay = new(0, 0, -0.5f);

    private Vector3 DefaultLocalPosition;

    #endregion
    #region MAIN_METHODS

    public void ResetData()
    {
        foreach (var mesh in MeshRenderers)
        {
            mesh.enabled = false;
        }
    }

    public Vector3 _localScale;

    public bool IsRedo;
    public bool IsPlayAnim { get; private set; }
    public bool IsWoolToCube;

    public void ResetAnim()
    {
        IsRedo = false;
        IsPlayAnim = false;
        IsWoolToCube = false;
    }

    internal IEnumerator AnimRedo()
    {
        IsRedo = true;
        transform.localPosition = DefaultLocalPositionAtDisplay;
        transform.localRotation = DefaultRotation;
        // _localScale = transform.localScale;
        var timePerRoll = (WoolAnimationData.Duration + WoolAnimationData.DurationHideWool) / (MeshRenderers.Count + 1);
        transform.DOShakeRotation(timePerRoll * 7, 10, 10, 10, true, ShakeRandomnessMode.Harmonic);

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = false;

        var meshCount = MeshRenderers.Count;
        for (int i = meshCount - 1; i >= 0; i--)
        {
            if (MeshRenderers[i] == null) yield break;
            // Get the mesh transform
            Transform meshTransform = MeshRenderers[i].transform;

            // Store original scale
            Vector3 originalScale = meshTransform.localScale * 1.3f;

            // Enable the mesh
            MeshRenderers[i].enabled = true;

            // Animate scale down to original size
            meshTransform
               .DOScale(originalScale, timePerRoll * 0.6f)
               .SetEase(Ease.OutBack);

            if (!skipAnimationDelays)
            {
                yield return new WaitForSeconds(timePerRoll);
                if (MeshRenderers[i] == null) yield break;
            }
            MeshRenderers[i].enabled = false;
        }

        if (!skipAnimationDelays)
        {
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.GetComponentInParent<QueueTargetControl>()?.ResetDefault();
        IsRedo = false;
    }
    internal IEnumerator AnimAddToQueue(RollWoolAnimationExtensions.ParentType parentType)
    {
        if (_currentColor.Equals(ShaderPropertiesLib.IgnoredWoolColorKey)) yield break;

        IsPlayAnim = true;
        _isPopToBroomPool = false;

        transform.localPosition = DefaultLocalPositionAtDisplay;
        transform.localRotation = DefaultRotation;
        // _localScale = transform.localScale;
        var rollCount = MeshRenderers.Count;
        var timePerRoll = WoolAnimationData?.Duration / (rollCount - 1) ?? 0.3f;

        transform.DOShakeRotation(timePerRoll * 7, 10, 10, 10, true, ShakeRandomnessMode.Harmonic);

        // Skip animation delays for AI_AGENT testing when IsNative is true
        bool skipAnimationDelays = false;
        for (int i = 0; i < rollCount - 1; i++)
        {
            if (MeshRenderers[i] == null) yield break;
            // Get the mesh transform
            Transform meshTransform = MeshRenderers[i].transform;

            // Store original scale
            Vector3 originalScale = meshTransform.localScale;

            // Set initial larger scale (30% bigger)
            meshTransform.localScale = originalScale * 1.3f;

            // Enable the mesh
            MeshRenderers[i].enabled = true;

            // Animate scale down to original size
            meshTransform
                .DOScale(originalScale, timePerRoll * 0.6f)
                .SetEase(Ease.OutBack);

            if (!skipAnimationDelays)
            {
                yield return new WaitForSeconds(timePerRoll);
            }
        }

        if (MeshRenderers[rollCount - 1] == null) yield break;
        // Get the mesh transform
        Transform meshTransformRollLast = MeshRenderers[rollCount - 1].transform;

        // Store original scale
        Vector3 originalScaleRollLast = meshTransformRollLast.localScale;

        // Set initial larger scale (30% bigger)
        meshTransformRollLast.localScale = originalScaleRollLast * 1.3f;

        // Enable the mesh
        MeshRenderers[rollCount - 1].enabled = true;

        var hideTime = WoolAnimationData?.DurationHideWool ?? 0.2f;
        // Animate scale down to original size
        meshTransformRollLast
           .DOScale(originalScaleRollLast, hideTime * 0.6f)
           .SetEase(Ease.OutBack);

        if (!skipAnimationDelays)
        {
            yield return new WaitForSeconds(timePerRoll);
        }

        if (parentType == RollWoolAnimationExtensions.ParentType.CubeQueue)
        {
            DefaultLocalPosition = DefaultLocalPositionInQueue;
            //if (soundData.GetAudioClip("wool_click2") != null) GameAudioManager.Instance.PlayOneShot(soundData.GetAudioClip("wool_click2"), soundData.GetSoundVolume("wool_click2"));
            SoundManager.Instance.PlayOneShot("wool_click2");
        }
        else
        {
            DefaultLocalPosition = DefaultLocalPositionInTarget;
            //if (soundData.GetAudioClip("wool_click1") != null) GameAudioManager.Instance.PlayOneShot(soundData.GetAudioClip("wool_click1"), soundData.GetSoundVolume("wool_click1"));
            SoundManager.Instance.PlayOneShot("wool_click1");
        }
        if (_isPopToBroomPool) yield break;
        SnapToHole();
    }

    public void SnapToHole()
    {
        // transform.localScale = _localScale;
        var timePerRoll = (WoolAnimationData.Duration + WoolAnimationData.DurationHideWool) / (MeshRenderers.Count + 1);
        transform.DOKill(); // Ensure any previous animations are stopped
        // Move and wait until complete before hopping
        transform
            .DOLocalMove(DefaultLocalPosition, timePerRoll)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
                {
                    IsPlayAnim = false;
                    // Call Hop only after the wool has finished falling
                    var targetBoxAnimation = GetComponentInParent<TargetBoxAnimator>();
                    if (targetBoxAnimation != null)
                    {
                        targetBoxAnimation.Hop(timePerRoll * 2);
                    }
                    else
                    {
                        Debug.LogWarning("TargetBoxAnimation component not found in parent.");
                    }
                }
            );

    }

    public void SetParentType(RollWoolAnimationExtensions.ParentType parentType)
    {
        DefaultLocalPosition = parentType == RollWoolAnimationExtensions.ParentType.CubeQueue
            ? DefaultLocalPositionInQueue
            : DefaultLocalPositionInTarget;
    }

    #region IPoolObject
    public GameObject Prefab { get; set; }

    public void OnPushToPool()
    {
        transform.DOKill();
        ResetData();
    }
    #endregion
    #endregion
}
