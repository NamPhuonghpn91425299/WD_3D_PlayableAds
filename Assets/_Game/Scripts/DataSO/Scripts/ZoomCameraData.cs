using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/ZoomCamera", menuName = "ScriptableObjects/ZoomCamera")]
public class ZoomCameraData : ScriptableObject
{
    [Header("Basic Zoom Settings")]
    public float ZoomSpeed      = 0.5f; // tốc độ zoom (càng lớn càng nhanh)
    public float MinFOV         = 30f;  // khoảng cách gần nhất được phép
    public float MaxFOV         = 60f;  // khoảng cách xa nhất được phép
    public float DefaultFOV     = 60f;  // khoảng cách mặc định
    public float MainMenuFOV     = 60f;  // khoảng cách mặc định
    
    [Header("Smart Camera Focus System")]
    [Tooltip("Bật/tắt hệ thống điều chỉnh camera thông minh")]
    public bool enableSmartFocus = true;
    
    [Tooltip("Điều chỉnh camera mỗi X% len thu thập (ví dụ: 10 = điều chỉnh mỗi 10%)")]
    [Range(5f, 50f)]
    public float zoomStepPercentage = 10f;
    
    [Tooltip("Vị trí tâm focus trên màn hình (0.5, 0.5 là trung tâm màn hình)")]
    public Vector2 cameraClipPos = new Vector2(0.5f, 0.5f);
    
    [Tooltip("Tốc độ di chuyển camera khi điều chỉnh focus")]
    [Range(0.5f, 5f)]
    public float focusSpeed = 1.5f;
    
    [Tooltip("Thời gian animation khi điều chỉnh camera (giây)")]
    [Range(0.5f, 3f)]
    public float focusSmoothTime = 1.0f;
    
    [Tooltip("Khoảng cách tối thiểu để kích hoạt điều chỉnh camera")]
    [Range(0.5f, 5f)]
    public float minimumAdjustmentDistance = 1.0f;
}
