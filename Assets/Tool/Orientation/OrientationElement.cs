using UnityEngine;

public class OrientationElement : MonoBehaviour
{
    [Header("Position Settings")]
    public bool adjustPosition = false;
    public Vector3 portraitPosition;
    public Vector3 landscapePosition;
    
    [Header("Scale Settings")]
    public bool adjustScale = false;
    public Vector3 portraitScale = Vector3.one;
    public Vector3 landscapeScale = Vector3.one;
    
    
    public bool adjustAnchoredPosition = false;
    public Vector2 portraitAnchoredPosition;
    public Vector2 landscapeAnchoredPosition;
    
    public bool adjustSizeDelta = false;
    public Vector2 portraitSizeDelta;
    public Vector2 landscapeSizeDelta;
    
    public bool adjustLocalScale = false;
    public Vector3 portraitLocalScale = Vector3.one;
    public Vector3 landscapeLocalScale = Vector3.one;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Vector2 originalAnchoredPosition;
    private Vector2 originalSizeDelta;
    private RectTransform rectTransform;
    private bool initialized = false;

    void Awake()
    {
        if (!initialized)
        {
            InitializeValues();
        }
    }

    void Start()
    {
        // Apply initial orientation
        OrientationManager.Instance?.RegisterElement(this);
    }

    void InitializeValues()
    {
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;
        rectTransform = GetComponent<RectTransform>();
        
        if (rectTransform != null)
        {
            originalAnchoredPosition = rectTransform.anchoredPosition;
            originalSizeDelta = rectTransform.sizeDelta;
        }
        
        initialized = true;
    }

    public void ApplyOrientation(bool isPortrait)
    {
        if (!initialized)
        {
            InitializeValues();
        }

        try
        {
            // Apply position changes for regular Transform
            if (adjustPosition)
            {
                transform.localPosition = isPortrait ? portraitPosition : landscapePosition;
            }
            
            // Apply scale changes
            if (adjustScale)
            {
                transform.localScale = isPortrait ? portraitScale : landscapeScale;
            }
            
            // Apply RectTransform specific changes
            if (rectTransform != null)
            {
                // Apply anchored position changes
                if (adjustAnchoredPosition)
                {
                    rectTransform.anchoredPosition = isPortrait ? portraitAnchoredPosition : landscapeAnchoredPosition;
                }
                
                // Apply size delta changes
                if (adjustSizeDelta)
                {
                    rectTransform.sizeDelta = isPortrait ? portraitSizeDelta : landscapeSizeDelta;
                }
                
                // Apply local scale changes
                if (adjustLocalScale)
                {
                    rectTransform.localScale = isPortrait ? portraitLocalScale : landscapeLocalScale;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Failed to apply orientation on {gameObject.name}: {e.Message}");
        }
    }

    void OnDestroy()
    {
        OrientationManager.Instance?.UnregisterElement(this);
    }

}
