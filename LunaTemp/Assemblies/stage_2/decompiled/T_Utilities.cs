using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class T_Utilities
{
	public static class DefaultValue
	{
		public const int MaxEnergyDefault = 10;

		public const float MaxZomInDefault = 5f;

		public const float MinZomInDefault = 1f;

		public const string AddressableItemLable = "ItemProperty";
	}

	public static class PlayerPrefKey
	{
		public static string InitializedPlayer = "InitializedPlayer";
	}

	public static class AnimatorHashKey
	{
		public const string IndexBottomHashKey = "IndexBottom";

		public const string ShowHideWool = "ShowHideWool";

		public static int ShowHashKey = Animator.StringToHash("Show");

		public static int HideHashKey = Animator.StringToHash("Hide");

		public static int TutLevel1HashKey = Animator.StringToHash("TutLevel1");
	}

	public static class ShaderPropertiesLib
	{
		public static int Color = Shader.PropertyToID("_Color");

		public static int Display = Shader.PropertyToID("_Display");

		public static int FresnelColor = Shader.PropertyToID("_FresnelColor");

		public static int Percent = Shader.PropertyToID("_Percent");

		public static int Progress = Shader.PropertyToID("_Progress");

		public static int UseRim = Shader.PropertyToID("_UseRim");

		public static int UseHaloOuter = Shader.PropertyToID("_UseHaloGlow");

		public static string GameplayModelLayer = "LevelModel";

		public static string EndgameModelLayer = "EndgameModel";
	}

	[Serializable]
	public enum MenuType
	{
		Shop,
		Home,
		Rank
	}

	public static Vector3 Mul_Vector3(this Vector3 vector1, Vector3 vector2)
	{
		return new Vector3(vector1.x * vector2.x, vector1.y * vector2.y, vector1.z * vector2.z);
	}

	public static void SetActive(this CanvasGroup canvasGroup, bool isActive)
	{
		if (!(canvasGroup == null))
		{
			canvasGroup.interactable = isActive;
			canvasGroup.blocksRaycasts = isActive;
			canvasGroup.alpha = (isActive ? 1 : 0);
		}
	}

	public static Coroutine Delay(this MonoBehaviour host, float delayTime, Action action)
	{
		if (Precondition(host, action))
		{
			return host.StartCoroutine(DelayCall(delayTime, action));
		}
		return null;
	}

	public static IEnumerator DelayCall(float delayTime, Action action)
	{
		yield return Yielders.Get(delayTime);
		action();
	}

	private static bool Precondition(MonoBehaviour host, Action action)
	{
		return host != null && host.gameObject != null && host.gameObject.activeInHierarchy && host.enabled && action != null;
	}

	public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
	{
		if (source == null)
		{
			return true;
		}
		if (source is ICollection<T> collection)
		{
			return collection.Count == 0;
		}
		return !source.GetEnumerator().MoveNext();
	}

	public static string NullReplace(this string str, string replace = "null")
	{
		return string.IsNullOrEmpty(str) ? replace : str;
	}
}
