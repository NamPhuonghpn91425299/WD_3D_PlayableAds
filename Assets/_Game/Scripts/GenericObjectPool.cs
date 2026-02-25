using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericObjectPool : MonoBehaviour
{
    public Dictionary<GameObject, PoolEntity> container = new Dictionary<GameObject, PoolEntity>();

    [System.Serializable]
    public class PoolEntity
    {
        private Queue<GameObject> Pool;

        [SerializeField]
        private int count;

        public bool CanDequeue => count > 0;

        public PoolEntity()
        {
            Pool = new Queue<GameObject>();
            count = 0;
        }

        public void Enqueue(GameObject gameObject)
        {
            Pool.Enqueue(gameObject);
            count++;
        }

        public GameObject Dequeue()
        {
            count--;
            return Pool.Dequeue();
        }
    }

    private static GenericObjectPool instance = null;

    public static GenericObjectPool Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GenericObjectPool>();

            return instance;
        }
    }

    public void InitPool(GameObject prefab, int count = 3)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = CreateObject(prefab, null);
            if (!obj) break;
            PushToPool_Object(ref obj);
        }
    }

    // private void Awake()
    // {
    //     SceneManager.sceneLoaded += OnSceneChanged;
    // }
    //
    // private void OnDestroy()
    // {
    //     SceneManager.sceneLoaded += OnSceneChanged;
    // }

    private void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
    {
        ReleasePool();
    }

    /// <summary>
    /// Reset the pool but does not destroy the content.
    /// </summary>
    public void Reset()
    {
        instance = null;
    }

    // /// <summary>
    // /// Adds to pool.
    // /// </summary>
    // /// <returns><c>true</c>, if item was successfully created, <c>false</c> otherwise.</returns>
    // /// <param name="prefab">The prefab to instantiate new items.</param>
    // /// <param name="count">The amount of instances to be created.</param>
    // /// <param name="parent">The Transform container to store the items. If null, items are placed as parent</param>
    // public bool AddToPool(GameObject prefab, int count, Transform parent = null)
    // {
    //     if (prefab == null || count <= 0)
    //     {
    //         return false;
    //     }
    //
    //     for (int i = 0; i < count; i++)
    //     {
    //         GameObject obj = PopFromPool(prefab, true, false, parent);
    //         PushToPool(ref obj, parent);
    //     }
    //
    //     return true;
    // }

    /// <summary>
    /// Pops item from pool.
    /// </summary>
    /// <returns>The from pool.</returns>
    /// <param name="prefab">Prefab to be used. Matches the prefab used to create the instance</param>
    /// <param name="forceInstantiate">If set to <c>true</c> force instantiate regardless the pool already contains the same item.</param>
    /// <param name="instantiateIfNone">If set to <c>true</c> instantiate if no item is found in the pool.</param>
    /// <param name="container">The Transform container to store the popped item.</param>
    public GameObject PopFromPool(GameObject prefab, bool forceInstantiate = false, bool instantiateIfNone = false,
        Transform container = null)
    {
        //Debug.Log("PopFromPool " + prefab);
        GameObject obj = null;

        if (forceInstantiate == true)
        {
            obj = CreateObject(prefab, null);
        }
        else
        {
            var queue = FindInContainer(prefab);
            if (queue == null)
                return null;
            if (queue.CanDequeue)
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

        // if (obj != null)
        // {
        //     obj.GetComponent<IPoolObject>().Init();
        // }

        return obj;
    }

    private PoolEntity FindInContainer(GameObject prefab)
    {
        if (prefab == null)
            return null;

        if (!container.TryGetValue(prefab, out var queque))
        {
            queque = new();
            container.Add(prefab, queque);
        }

        return queque;
    }

    private GameObject CreateObject(GameObject prefab, Transform parent)
    {
        IPoolObject poolObjectPrefab = prefab.GetComponent<IPoolObject>();
        if (poolObjectPrefab == null)
        {
            Debug.Log("Wrong type of object");
            return null;
        }

        GameObject obj = Instantiate(prefab);
        IPoolObject poolObject = obj.GetComponent<IPoolObject>();
#if UNITY_EDITOR
        obj.name = prefab.name;
#endif
        poolObject.Prefab = prefab;

        obj.transform.SetParent(parent, false);
        return obj;
    }

    /// <summary>
    /// Pushs back the item to the pool.
    /// </summary>
    /// <param name="obj">A reference to the item to be pushed back.</param>
    /// <param name="retainObject">If set to <c>true</c> retain object.</param>
    /// <param name="newParent">The Transform container to store the item.</param>
    public void PushToPool_Object(ref GameObject obj, Transform newParent = null)
    {
        if (this == null || obj == null)
        {
            return;
        }

        if (newParent != null)
        {
            obj.transform.SetParent(newParent, false);
        }

        if (obj.TryGetComponent(out IPoolObject poolObject))
        {
            PushToPool(poolObject, obj);
        }

        obj = null;
    }

    public void PushToPool(IPoolObject target, GameObject gameObj)
    {
        if (target == null)
            return;

        GameObject prefab = target.Prefab;
        var queue = FindInContainer(prefab);
        queue?.Enqueue(gameObj);

        target.OnPushToPool();
        gameObj.SetActive(false);
    }

    /// <summary>
    /// Releases all items from the pool and destroys them.
    /// </summary>
    public void ReleasePool()
    {
        foreach (var kvp in container)
        {
            var queue = kvp.Value;
            while (queue.CanDequeue)
            {
                GameObject obj = queue.Dequeue();
                Destroy(obj);
            }
        }

        container.Clear();
    }
}

public interface IPoolObject
{
    GameObject Prefab { get; set; }
    void OnPushToPool();
}
