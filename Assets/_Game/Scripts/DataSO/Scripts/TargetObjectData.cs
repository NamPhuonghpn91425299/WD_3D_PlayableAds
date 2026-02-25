using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/TargetObject", menuName = "ScriptableObjects/TargetObject")]
public class TargetObjectData : ScriptableObject
{
    public Vector3 CubeScreenPosition;
    public float   CubeInitialDistance;
    public Vector3 CubeInitialScale;
    public Vector3 QueueScreenPosition;
    public float   QueueInitialDistance;
    public Vector3 QueueInitialScale;
    public Vector2 ScreenSize;
}
