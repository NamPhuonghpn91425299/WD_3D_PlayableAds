using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySetupActive : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectSpawns;
    [SerializeField] private List<GameObject> targetGameObjects;
    
    private void Awake()
    {
        GameEventManager.SetupObjectSpawnActive += SetupObjectSpawnActive;
        //GameEventManager.SetupObjectTargetActive += SetTargetObjectActive;
    }
    private void OnDestroy()
    {
        GameEventManager.SetupObjectSpawnActive -= SetupObjectSpawnActive;
        //GameEventManager.SetupObjectTargetActive -= SetTargetObjectActive;
    }
    public void SetupObjectSpawnActive(bool isActive)
    {
        foreach (var objectSpawn in objectSpawns)
        {
            objectSpawn.SetActive(isActive);
        }
    }
    private bool _isTargetObjectActive = true;
    public void SetTargetObjectActive(bool isActive)
    {
        _isTargetObjectActive = isActive;
        foreach (var targetGameObject in targetGameObjects)
        {
            targetGameObject.SetActive(isActive);
        }
    }
}
