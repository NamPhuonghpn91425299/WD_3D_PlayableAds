using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool : MonoBehaviour
{
	private readonly Dictionary<GameObject, Queue<GameObject>> container = new Dictionary<GameObject, Queue<GameObject>>();

	private static ObjectPool instance;

	public static ObjectPool Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<ObjectPool>();
			}
			return instance;
		}
	}

	private void Awake()
	{
		SceneManager.sceneLoaded += OnSceneChanged;
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded += OnSceneChanged;
	}

	private void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
	{
		ReleasePool();
	}

	public void Reset()
	{
		instance = null;
	}

	public bool AddToPool(GameObject prefab, int count, Transform parent = null)
	{
		if (prefab == null || count <= 0)
		{
			return false;
		}
		for (int i = 0; i < count; i++)
		{
			GameObject obj = PopFromPool(prefab, true, false, parent);
			PushToPool(ref obj, true, parent);
		}
		return true;
	}

	public GameObject PopFromPool(GameObject prefab, bool forceInstantiate = false, bool instantiateIfNone = false, Transform container = null)
	{
		GameObject obj = null;
		if (forceInstantiate)
		{
			obj = CreateObject(prefab, null);
		}
		else
		{
			Queue<GameObject> queue = FindInContainer(prefab);
			if (queue == null)
			{
				return null;
			}
			if (queue.Count > 0)
			{
				obj = queue.Dequeue();
				if (obj != null)
				{
					obj.SetActive(true);
					obj.transform.SetParent(container, false);
				}
			}
		}
		if (obj == null && instantiateIfNone)
		{
			obj = CreateObject(prefab, container);
		}
		return obj;
	}

	private Queue<GameObject> FindInContainer(GameObject prefab)
	{
		if (prefab == null)
		{
			return null;
		}
		if (!container.ContainsKey(prefab))
		{
			container.Add(prefab, new Queue<GameObject>());
		}
		return container[prefab];
	}

	private GameObject CreateObject(GameObject prefab, Transform parent)
	{
		IPoolObject poolObjectPrefab = prefab.GetComponent<IPoolObject>();
		if (poolObjectPrefab == null)
		{
			Debug.Log("Wrong type of object");
			return null;
		}
		GameObject obj = Object.Instantiate(prefab);
		IPoolObject poolObject = obj.GetComponent<IPoolObject>();
		poolObject.Prefab = prefab;
		obj.transform.SetParent(parent, false);
		return obj;
	}

	public void PushToPool(ref GameObject obj, bool retainObject = true, Transform newParent = null)
	{
		if (this == null || obj == null)
		{
			return;
		}
		if (!retainObject)
		{
			Object.Destroy(obj);
			obj = null;
			return;
		}
		if (newParent != null)
		{
			obj.transform.SetParent(newParent, false);
		}
		IPoolObject poolObject = obj.GetComponent<IPoolObject>();
		if (poolObject != null)
		{
			GameObject prefab = poolObject.Prefab;
			FindInContainer(prefab)?.Enqueue(obj);
			poolObject.OnPushToPool();
			obj.SetActive(false);
		}
		obj = null;
	}

	public void PushToPool(IPoolObject target, GameObject gameObj)
	{
		if (target != null)
		{
			GameObject prefab = target.Prefab;
			FindInContainer(prefab)?.Enqueue(gameObj);
			target.OnPushToPool();
			gameObj.SetActive(false);
		}
	}

	public void ReleaseItems(GameObject prefab, bool destroyObject = false)
	{
		if (prefab == null)
		{
			return;
		}
		Queue<GameObject> queue = FindInContainer(prefab);
		if (queue == null)
		{
			return;
		}
		while (queue.Count > 0)
		{
			GameObject obj = queue.Dequeue();
			if (destroyObject)
			{
				Object.Destroy(obj);
			}
		}
	}

	public void ReleasePool()
	{
		foreach (KeyValuePair<GameObject, Queue<GameObject>> item in container)
		{
			Queue<GameObject> queue = item.Value;
			while (queue.Count > 0)
			{
				GameObject obj = queue.Dequeue();
				Object.Destroy(obj);
			}
		}
		container.Clear();
	}
}
