using System;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffectManager : Singleton<ClickEffectManager>
{
    [Header("References")]
    public Camera camMain;
    [SerializeField] private ClickEffector _clickEffectorPrefab;

    [Header("Effect Pool Settings")]
    [SerializeField] private int initialPoolSize = 2;

    [Header("COLOR STAT")]
    public float BrightnessDifference = 1f;

    private List<ClickEffector> _effectPool = new List<ClickEffector>();
    private Transform _poolParent;

    private void Start()
    {
        _poolParent = transform;
        InitPool();
    }

    private void InitPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewEffector();
        }
    }

    private ClickEffector CreateNewEffector()
    {
        var effector = Instantiate(_clickEffectorPrefab, _poolParent);
        effector.gameObject.SetActive(false);
        _effectPool.Add(effector);
        return effector;
    }

    private ClickEffector GetAvailableEffector()
    {
        foreach (var effector in _effectPool)
        {
            if (!effector.gameObject.activeInHierarchy)
                return effector;
        }

        // Nếu không còn effector nào đang rảnh → tạo mới
        return CreateNewEffector();
    }

    public void PlayClickEffect(Color color)
    {
        var effector = GetAvailableEffector();
        effector.gameObject.SetActive(true);

        color = AdjustBrightness(color, BrightnessDifference);
        effector.SetColor(color);
        effector.PlayAnim(() =>
        {
            effector.gameObject.SetActive(false); // "trả lại" effector
        });
    }

    public Color AdjustBrightness(Color color, float factor)
    {
        float r = Mathf.Clamp01(color.r * factor);
        float g = Mathf.Clamp01(color.g * factor);
        float b = Mathf.Clamp01(color.b * factor);
        return new Color(r, g, b, color.a);
    }
}