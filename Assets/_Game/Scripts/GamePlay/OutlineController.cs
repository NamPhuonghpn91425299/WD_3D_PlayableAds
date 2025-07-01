using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineController : MonoBehaviour
{
    private MaterialPropertyBlock _mpb;
    private Renderer _renderer;

    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineWidth = 0.03f;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        SetOutlineVisible(true);
    }

    private void OnDisable()
    {
        SetOutlineVisible(false);
    }

    public void SetOutlineVisible(bool visible, float fadeValue = 1f)
    {
        _renderer.GetPropertyBlock(_mpb);

        _mpb.SetFloat("_EnableOutline", visible ? fadeValue : 0f);
        _mpb.SetColor("_OutlineColor", outlineColor);
        _mpb.SetFloat("_OutlineWidth", outlineWidth);

        _renderer.SetPropertyBlock(_mpb);
    }

    public void FadeOutline(float from, float to, float duration)
    {
        StartCoroutine(FadeRoutine(from, to, duration));
    }

    private System.Collections.IEnumerator FadeRoutine(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(from, to, t / duration);
            SetOutlineVisible(true, val);
            yield return null;
        }
        SetOutlineVisible(to > 0, to);
    }
}