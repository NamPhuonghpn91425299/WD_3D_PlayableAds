using UnityEngine;

public class ItemObjectPool : MonoBehaviour, IPoolObject
{
    public GameObject Prefab { get; set; }

    public void OnPushToPool()
    {
    }
}
