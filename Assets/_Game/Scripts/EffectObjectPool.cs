using System.Collections;
using Unity.Collections;
using UnityEngine;

public class EffectObjectPool : MonoBehaviour, IPoolObject
{
    #region IPoolObject
    public float waitTime = 0.0f;

    [field: SerializeField, ReadOnly] public GameObject Prefab { get; set; }


    public void OnEnable()
    {
        if (waitTime > 0 && gameObject.activeInHierarchy)
            StartCoroutine(AutoPushToPool(waitTime));
    }

    public void OnPushToPool()
    {
    }

    public IEnumerator AutoPushToPool(float timeDead)
    {
        yield return new WaitForSeconds(timeDead);
        GameObject o = gameObject;
        if (!o.activeSelf) yield break;
        GenericObjectPool.Instance.PushToPool_Object(ref o);
    }
    #endregion
}
