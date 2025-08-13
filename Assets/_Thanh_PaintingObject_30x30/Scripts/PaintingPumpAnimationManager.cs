using System.Collections.Generic;

using UnityEngine;

public class PaintingPumpAnimationManager : MonoBehaviour
{
    #region PROPERTIES
    public int PrewarmCount = 50;
    private readonly Queue<PaintingCellPumpUpAnimation> availableAnimationItem = new();
    public PaintingCellPumpUpAnimation EffectPrefab;
    public Transform EffectContainer;
    #endregion

    #region UNITY CORE
    private void Awake()
    {
        //PreSpawn().Forget();
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

    // private async UniTaskVoid PreSpawn()
    // {
    //     for (int i = 0; i < PrewarmCount; i++)
    //     {
    //         var obj = CreateNew();
    //         obj.gameObject.SetActive(false);
    //         availableAnimationItem.Enqueue(obj);

    //         await UniTask.DelayFrame(2);
    //     }
    // }
    #endregion
}