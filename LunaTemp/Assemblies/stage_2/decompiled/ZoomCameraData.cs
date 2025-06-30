using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/ZoomCamera", menuName = "ScriptableObjects/ZoomCamera")]
public class ZoomCameraData : ScriptableObject
{
	public float ZoomSpeed = 0.5f;

	public float MinFOV = 30f;

	public float MaxFOV = 60f;

	public float DefaultFOV = 60f;
}
