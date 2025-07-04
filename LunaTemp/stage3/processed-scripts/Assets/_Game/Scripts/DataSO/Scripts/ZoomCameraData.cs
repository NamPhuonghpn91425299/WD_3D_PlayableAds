using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/ZoomCamera", menuName = "ScriptableObjects/ZoomCamera")]
public class ZoomCameraData : ScriptableObject
{
    public float ZoomSpeed      = 0.5f; // tốc độ zoom (càng lớn càng nhanh)
    public float MinFOV         = 30f;  // khoảng cách gần nhất được phép
    public float MaxFOV         = 60f;  // khoảng cách xa nhất được phép
    public float DefaultFOV     = 60f;  // khoảng cách mặc định
}
