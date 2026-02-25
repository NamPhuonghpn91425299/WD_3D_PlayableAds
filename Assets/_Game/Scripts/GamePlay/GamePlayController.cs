using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    [SerializeField] private GamePlaySetupActive m_gamePlaySetupActive;
    private void Awake()
    {
        GameEventManager.OnEndGameAction += OnEndGameAction;
    }
    private void Start()
    {
        Play();
    }
    private void OnDestroy()
    {
        GameEventManager.OnEndGameAction -= OnEndGameAction;
    }
    private void OnEndGameAction(bool isWin)
    {
        Debug.Log($"OnEndGameAction: {isWin}");

        //CoroutineBridge.Run(CoroutineBridge.WaitFor(UILayerManager.Instance.ShowEndGameLayer(data)));
    }
    public void Play()
    {
        GamePlayManager.Instance.ResetLevelModel();
    }
    public void Close()
    {
        GameEventManager.SetupObjectTargetActive?.Invoke(false);
        GameEventManager.PlayAnimPreLose?.Invoke(false);
        GameEventManager.OnEndGameUIDeactive?.Invoke();
        //m_gamePlaySetupActive.SetTargetObjectActive(false);
    }
}
