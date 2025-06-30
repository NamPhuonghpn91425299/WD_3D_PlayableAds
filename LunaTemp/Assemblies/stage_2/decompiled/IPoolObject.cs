using UnityEngine;

public interface IPoolObject
{
	GameObject Prefab { get; set; }

	void OnPushToPool();
}
