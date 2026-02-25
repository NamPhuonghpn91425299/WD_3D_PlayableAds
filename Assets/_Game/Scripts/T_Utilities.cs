using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class T_Utilities
{
    #region CanvasGroup

    public static void SetActive(this CanvasGroup canvasGroup, bool isActive)
    {
        if (canvasGroup == null) return;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
        canvasGroup.alpha = isActive ? 1 : 0;
    }

    #endregion

    #region Coroutine

    public static Coroutine Delay(this MonoBehaviour host, float delayTime, Action action)
    {
        if (host != null && host.gameObject != null && host.gameObject.activeInHierarchy && host.enabled && action != null)
            return host.StartCoroutine(DelayCall(delayTime, action));
        return default;
    }

    public static IEnumerator DelayCall(float delayTime, Action action)
    {
        yield return new WaitForSeconds(delayTime);
        action();
    }

    #endregion

    #region NullCheck

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
    {
        if (source == null) return true;
        if (source is ICollection<T> collection) return collection.Count == 0;
        return !source.GetEnumerator().MoveNext();
    }

    public static bool IsNullOrEmpty(this string source) => string.IsNullOrEmpty(source);

    public static int ZeroReplace(this int value, int replace = 0)
    {
        return value > 0 ? value : replace;
    }

    public static string NullReplace(this string str, string replace = "null")
    {
        return string.IsNullOrEmpty(str) ? replace : str;
    }

    #endregion

    #region GameObject

    public static void SetActive(this List<GameObject> gameObjects, bool isActive)
    {
        if (gameObjects == null || gameObjects.Count == 0) return;
        foreach (var go in gameObjects)
        {
            go?.SetActive(isActive);
        }
    }

    #endregion

    #region Button

    public static void RegisterListener(this Button button, Action onClick)
    {
        if (button != null)
            button.onClick.AddListener(() => onClick?.Invoke());
    }

    #endregion
}
