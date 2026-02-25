using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/WoolAnimation", menuName = "ScriptableObjects/WoolAnimation")]
public class WoolAnimationData : ScriptableObject
{
    public float Duration         = 0.5f; // thời gian di chuyển
    public float DurationHideWool = 0.3f; // thời gian ẩn sợi len
    public float OffSet           = 0.1f;

    [Header("DECOR OBJECT FORCE SETTING")]
    public float ForceValue = 2f;
    public float RandomDirrectionFactor = 0.2f;
    public float SpeedRotation          = 15f;
    
    [Header("TimeDeLay Vacuum Cleaner")]
    public float TimeDelayVacuumCleaner = 0.3f;

    [Header("MESH PUMP UP ANIMATION")]
    public float MeshPumpAnimDuration = 0.5f;
    public float StartScaleOffset = -0.03f;
    public float MidScaleOffset   = 0.02f;
    public float EndScaleOffset   = 0f;
}
