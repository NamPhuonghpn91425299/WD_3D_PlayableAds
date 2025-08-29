using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimCubeQueuePreLose : Singleton<AnimCubeQueuePreLose>
{
    [SerializeField] private AudioClip soundFx;
    public float queueDuration = 0.5f;
    private int s_HashProgress = Shader.PropertyToID("_Progress");
    private Tween _queueTargetColorSequence;
    MaterialPropertyBlock _propertyBlock;
    Renderer _lastQueueTargetRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayAnimQueueColor()
    {
        if(!GamePlaySystem.Instance.CheckQueuePreLose())return;
        StopAnimQueueColor();
        _lastQueueTargetRenderer = GamePlaySystem.Instance.GetEmptyQueueTarget().Renderer;
        if(!_lastQueueTargetRenderer)return;
        _propertyBlock = new MaterialPropertyBlock();
        _lastQueueTargetRenderer.GetPropertyBlock(_propertyBlock);
        _queueTargetColorSequence = DOTween.To(()=>0f, (x) =>
        {
            _propertyBlock.SetFloat(s_HashProgress, x);
            _lastQueueTargetRenderer.SetPropertyBlock(_propertyBlock);
        }, 0.7f, queueDuration).SetLoops(-1, LoopType.Yoyo);
    }
    public void StopAnimQueueColor()
    {
        _queueTargetColorSequence?.Kill();
        if(_propertyBlock == null || !_lastQueueTargetRenderer)return;
        _lastQueueTargetRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat(s_HashProgress, 0f);
        _lastQueueTargetRenderer.SetPropertyBlock(_propertyBlock);
    }
}
