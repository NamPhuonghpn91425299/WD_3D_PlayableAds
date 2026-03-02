using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OrientationManager : MonoBehaviour
{
    public static OrientationManager Instance { get; private set; }

    private CanvasScaler scaler;
    private bool isPortrait;
    private List<OrientationElement> elements = new List<OrientationElement>();

    [Header("Canvas Scaler Settings")]
    [Tooltip("Match khi dọc (1 = theo chiều cao)")]
    [Range(0f, 1f)] public float portraitMatch = 1f;
    
    [Tooltip("Match khi ngang (0 = theo chiều rộng)")]
    [Range(0f, 1f)] public float landscapeMatch = 0.5f;
    
    [Header("Performance Settings")]
    [Tooltip("Tần suất kiểm tra hướng màn hình (giây/lần).")]
    public float checkInterval = 0.5f;

    private float nextCheckTime;

    void Awake()
    {
        Instance = this;

        scaler = GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = gameObject.AddComponent<CanvasScaler>();
        }
    }

    void Start()
    {
        UpdateOrientation();
    }

    void Update()
    {
        if (Time.unscaledTime < nextCheckTime)
            return;
            
        nextCheckTime = Time.unscaledTime + checkInterval;
        
        bool currentPortrait = Screen.height > Screen.width;
        if (currentPortrait != isPortrait)
            UpdateOrientation();
    }

    void UpdateOrientation()
    {
        isPortrait = Screen.height > Screen.width;
        
        // Update CanvasScaler settings
        if (scaler != null)
        {
            scaler.matchWidthOrHeight = isPortrait ? portraitMatch : landscapeMatch;
        }
        
        // Update all registered elements
        foreach (var element in elements)
        {
            if (element != null)
            {
                element.ApplyOrientation(isPortrait);
            }
        }
    }

    public void RegisterElement(OrientationElement element)
    {
        if (!elements.Contains(element))
        {
            elements.Add(element);
            element.ApplyOrientation(isPortrait);
        }
    }

    public void UnregisterElement(OrientationElement element)
    {
        elements.Remove(element);
    }

    public void ForceUpdateOrientation()
    {
        UpdateOrientation();
    }

    // Cleanup null elements
    private void CleanupElements()
    {
        elements.RemoveAll(e => e == null);
    }

    void OnValidate()
    {
        if (Application.isPlaying && scaler != null)
        {
            scaler.matchWidthOrHeight = isPortrait ? portraitMatch : landscapeMatch;
        }
    }
}
