using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContainer : SingletonBase<CameraContainer>
{
    public Camera MainCamera;
    public Camera FakeUICamera;

#if UNITY_EDITOR
    private void OnValidate()
    {
        var allCamInScene = FindObjectsByType<Camera>(FindObjectsSortMode.None);
        foreach (var cam in allCamInScene)
        {
            switch (cam.gameObject.name)
            {
                case "Main Camera":
                    MainCamera = cam;
                    break;
                case "FakeUI_Camera":
                    FakeUICamera = cam;
                    break;

            }
        }
    }
#endif


    public void SetFakeUICamera(bool isActive)
    {
        FakeUICamera.enabled = isActive;
    }

    public override void Awake()
    {
        base.Awake();
        //GameEventManager.OnGameStateChange += OnChangeGameState;
        GameEventManager.SetupObjectTargetActive += SetTargetObjectActive;
    }

    private void OnDestroy()
    {
        //GameEventManager.OnGameStateChange -= OnChangeGameState;
        GameEventManager.SetupObjectTargetActive -= SetTargetObjectActive;
    }

    private void SetTargetObjectActive(bool isActive)
    {
        FakeUICamera.cullingMask = isActive ? 1 << 6 : 0;
    }

    // private void OnChangeGameState(GameState gameState)
    // {
    //     FakeUICamera.enabled = gameState == GameState.InGame;
    //     MainCamera.enabled = gameState == GameState.InGame;
    // }
}
