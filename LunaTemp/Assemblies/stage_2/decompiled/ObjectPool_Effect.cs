using System.Collections;
using UnityEngine;

public class ObjectPool_Effect : MonoBehaviour, IPoolObject
{
	public float waitTime = 0f;

	private GameObject prefab = null;

	public GameObject Prefab
	{
		get
		{
			return prefab;
		}
		set
		{
			prefab = value;
		}
	}

	public void Start()
	{
		if (waitTime != 0f && base.gameObject.activeSelf && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(AutoPushToPool(waitTime));
		}
	}

	public void OnPushToPool()
	{
	}

	public IEnumerator AutoPushToPool(float timeDead)
	{
		yield return new WaitForSeconds(timeDead);
		ObjectPool.Instance.PushToPool(this, base.gameObject);
	}
}
