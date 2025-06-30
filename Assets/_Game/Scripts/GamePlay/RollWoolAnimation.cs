using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class RollWoolAnimationExtensions
{
    public static RollWoolAnimation SetColor(this RollWoolAnimation rollWoolAnimation, Color color)
    {
        rollWoolAnimation._currentColor = color;
        var materialProperty = rollWoolAnimation.materialProperty;
        var meshRenderers = rollWoolAnimation.MeshRenderers;
        for (var i = 0; i < meshRenderers.Count; i++)
        {
            meshRenderers[i]
                .GetPropertyBlock(materialProperty);
            materialProperty.SetColor(T_Utilities.ShaderPropertiesLib.Color, color);
            meshRenderers[i]
                .SetPropertyBlock(materialProperty);
        }

        return rollWoolAnimation;
    }

    public static RollWoolAnimation SetParent(this RollWoolAnimation rollWoolAnimation, Transform parentTrans)
    {
        rollWoolAnimation.transform.SetParent(parentTrans, true);
        return rollWoolAnimation;
    }

    public static RollWoolAnimation ResetMesh(this RollWoolAnimation rollWoolAnimation)
    {
        rollWoolAnimation.ResetData();
        return rollWoolAnimation;
    }

    public static void PlayAnim(this RollWoolAnimation rollWoolAnimation, ParentType parentType)
    {
        if (!rollWoolAnimation.isActiveAndEnabled) return;
        rollWoolAnimation.StartCoroutine(rollWoolAnimation.Anim(parentType));
    }

    public enum ParentType
    {
        CubeTarget,
        CubeQueue,
    }
}

public class RollWoolAnimation : MonoBehaviour
{
    #region PROPERTIES

    //public SoundSO            soundData;
    public AudioClip          woolClip1;
    public AudioClip          woolClip2; 
    public WoolAnimationData  WoolAnimationData;
    public List<MeshRenderer> MeshRenderers;

    internal MaterialPropertyBlock materialProperty;
    internal Color _currentColor = Color.black;

    private readonly Quaternion DefaultRotation = Quaternion.Euler(30, 0, 0);
    private readonly Vector3 DefaultLocalPositionInTarget = new(0, -0.3f, 0.4f);
    private readonly Vector3 DefaultLocalPositionInQueue = new(0, -0.23f, 0.2f);
    private readonly Vector3 DefaultLocalPositionAtDisplay = new(0, 0, -0.5f);

    private Vector3 DefaultLocalPosition;
    
    #endregion

    

    #region MAIN_METHODS

    public void ResetData()
    {
        materialProperty = new MaterialPropertyBlock();
        foreach (var mesh in MeshRenderers)
        {
            mesh.enabled = false;
        }
    }

    public Vector3 _localScale;

    internal IEnumerator Anim(RollWoolAnimationExtensions.ParentType parentType)
    {
        if (_currentColor == Color.black) yield return null;

        transform.localPosition = DefaultLocalPositionAtDisplay;
        transform.localRotation = DefaultRotation;
        // _localScale = transform.localScale;
        var timePerRoll = (WoolAnimationData.Duration + WoolAnimationData.DurationHideWool) / (MeshRenderers.Count + 1);

        transform.DOShakeRotation(timePerRoll * 7, 10, 10, 10, true, ShakeRandomnessMode.Harmonic);

        for (int i = 0; i < MeshRenderers.Count; i++)
        {
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

            yield return Yielders.Get(timePerRoll);
        }

        if (parentType == RollWoolAnimationExtensions.ParentType.CubeQueue)
        {
            DefaultLocalPosition = DefaultLocalPositionInQueue;
            if (woolClip2 != null) SoundManager.Instance.PlayOneShot(woolClip2, 1);
        }
        else
        {
            DefaultLocalPosition = DefaultLocalPositionInTarget;
            if (woolClip1 != null) SoundManager.Instance.PlayOneShot(woolClip1, 1);
        }

        SnapToHole();
    }

    public void SnapToHole()
    {
        // transform.localScale = _localScale;
        var timePerRoll = (WoolAnimationData.Duration + WoolAnimationData.DurationHideWool) / (MeshRenderers.Count + 1);

        // Move and wait until complete before hopping
        transform
            .DOLocalMove(DefaultLocalPosition, timePerRoll)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
                {
                    // Call Hop only after the wool has finished falling
                    var targetBoxAnimation = GetComponentInParent<TargetBoxAnimation>();
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
    
    #endregion
}