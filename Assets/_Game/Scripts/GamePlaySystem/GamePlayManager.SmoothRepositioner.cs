using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class GamePlayManager
{
    [SerializeField] float spacingCubeTarget = 1f;
    private List<CubeTargetControl> _activeObjects = new ();
    private void SmoothRepositioner()
    {
        return;
        CacheActiveObjects();
        Reposition();
    }

    private void CacheActiveObjects()
    {
        _activeObjects.Clear();
        foreach (var obj in CurrentCubeTargets)
        {
            if (obj != null && obj.gameObject.activeSelf)
                _activeObjects.Add(obj);
        }
    }
    
    Sequence _repositionSequence;

    private void Reposition()
    {
        _repositionSequence?.Kill();
        _repositionSequence = DOTween.Sequence();
        int activeCount = _activeObjects.Count;
        if (activeCount == 0) return;

        // Tính tổng chiều rộng cần để căn giữa
        float totalWidth = (activeCount - 1) * spacingCubeTarget;
        float startX     = -totalWidth / 2f;

        for (int i = 0; i < activeCount; i++)
        {
            var     obj        = _activeObjects[i];
            Vector3 currentPos = obj.transform.position;
            Vector3 targetPos  = new Vector3(startX + i * spacingCubeTarget, currentPos.y, currentPos.z);
            var     i1         = i;
            _repositionSequence.Join(obj
                   .transform
                   .DOMoveX(targetPos.x, 0.3f)
                   .SetEase(Ease.OutQuad)
                );
        }
        _repositionSequence.SetLink(gameObject);
        _repositionSequence.Play();
    }
}
