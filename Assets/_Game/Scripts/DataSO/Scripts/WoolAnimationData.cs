using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/WoolAnimation", menuName = "ScriptableObjects/WoolAnimation")]
public class WoolAnimationData : ScriptableObject
{
    public float Duration = 0.5f; // thời gian di chuyển
    public float DurationHideWool = 0.3f; // thời gian ẩn sợi len
    public float OffSet  = 0.1f;

    [Header("DECOR OBJECT FORCE SETTING")]
    public float ForceValue = 2f;
    public float RandomDirrectionFactor = 0.2f;
    public float SpeedRotation = 444;
    
    [Header("GROUP PULSE SETTINGS")]
    [Tooltip("Bật văng theo nhóm thay vì từng cái")]
    public bool UseGroupPulse = false;
    
    [Tooltip("Số lượng object trong mỗi nhóm")]
    public int GroupSize = 3;
    
    [Tooltip("Thời gian delay giữa các nhóm (giây)")]
    public float GroupDelayTime = 0.1f;
    
    [Tooltip("Hệ số lực hướng lên")]
    public float UpwardForceMultiplier = 2f;
    
    [Tooltip("Thêm xoay ngẫu nhiên khi văng")]
    public bool AddRandomTorque = true;
    
    [Tooltip("Giá trị torque ngẫu nhiên")]
    public float TorqueValue = 5f;
    
    [Header("MESH PUMP ANIMATION")]
    public float StartScaleOffset = 1.0f;
    public float MidScaleOffset = 1.1f;
    public float EndScaleOffset = 1.0f;
    public float MeshPumpAnimDuration = 0.3f;
    
    [Header("VIBRATION")]
    public float VibrationValue = 0.5f;
}
