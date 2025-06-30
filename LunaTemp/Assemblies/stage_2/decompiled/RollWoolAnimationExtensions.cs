using System.Collections.Generic;
using UnityEngine;

public static class RollWoolAnimationExtensions
{
	public enum ParentType
	{
		CubeTarget,
		CubeQueue
	}

	public static RollWoolAnimation SetColor(this RollWoolAnimation rollWoolAnimation, Color color)
	{
		rollWoolAnimation._currentColor = color;
		MaterialPropertyBlock materialProperty = rollWoolAnimation.materialProperty;
		List<MeshRenderer> meshRenderers = rollWoolAnimation.MeshRenderers;
		for (int i = 0; i < meshRenderers.Count; i++)
		{
			meshRenderers[i].GetPropertyBlock(materialProperty);
			materialProperty.SetColor(T_Utilities.ShaderPropertiesLib.Color, color);
			meshRenderers[i].SetPropertyBlock(materialProperty);
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
		if (rollWoolAnimation.isActiveAndEnabled)
		{
			rollWoolAnimation.StartCoroutine(rollWoolAnimation.Anim(parentType));
		}
	}
}
