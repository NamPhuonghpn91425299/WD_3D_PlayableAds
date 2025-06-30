using UnityEngine;

public class CameraContainer : MonoBehaviour
{
	public Camera MainCamera;

	public Camera FakeUICamera;

	public Camera EndgameModelCamera;

	public static CameraContainer Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}
}
