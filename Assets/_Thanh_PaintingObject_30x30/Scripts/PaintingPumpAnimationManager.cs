using System.Collections.Generic;
using System.Collections; // for IEnumerator
using UnityEngine;

public class PaintingPumpAnimationManager : MonoBehaviour
{
    #region PROPERTIES
    public int PrewarmCount = 50;
    private readonly Queue<PaintingCellPumpUpAnimation> availableAnimationItem = new Queue<PaintingCellPumpUpAnimation>();
    public PaintingCellPumpUpAnimation EffectPrefab;
    public Transform EffectContainer;
    #endregion

    #region UNITY CORE
    private void Awake()
    {
        // Prewarm the pool over a few frames without UniTask
        StartCoroutine(PreSpawn());
    }
    #endregion

    #region MAIN
    public void SpawnEffectAt(Vector3 position, Color color)
    {
        var effect = availableAnimationItem.Count > 0 ? availableAnimationItem.Dequeue() : CreateNew();
        effect.StartAnimation(color, position);
    }
    public void ReturnToPool(PaintingCellPumpUpAnimation effect)
    {
        availableAnimationItem.Enqueue(effect);
    }

    private PaintingCellPumpUpAnimation CreateNew()
    {
        var obj = Instantiate(EffectPrefab, EffectContainer);
        obj.Init(this);
        return obj;
    }

    // Coroutine-based pre-spawn (no UniTask/UniTaskVoid)
    private IEnumerator PreSpawn()
    {
        for (int i = 0; i < PrewarmCount; i++)
        {
            var obj = CreateNew();
            obj.gameObject.SetActive(false);
            availableAnimationItem.Enqueue(obj);

            // Wait ~2 frames similar to UniTask.DelayFrame(2)
            yield return null;
            yield return null;
        }
    }
    #endregion
}