using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance { get; protected set; }

	public virtual void Awake()
	{
		if ((Object)Instance == (Object)null)
		{
			Instance = this as T;
		}
		else
		{
			Object.Destroy(this);
		}
	}
}
