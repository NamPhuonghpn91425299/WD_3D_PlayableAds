using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public partial class GamePlayManager
{
    private bool _isUseBroomBooster;

    #region USE_RAINBOW_BOOSTER

    public void AddRainBowBox()
    {
        return;
        _isUsingRainBowBooster = true;
        _isPLayAnimUsingRainBow = true;
        LockVacuumCleaner?.Invoke(true);
        DisplayRainBowBox();
    }

    private bool UseRainBowBooster(Transform startPoint, List<Vector3> spiralPath, string colorClick)
    {
        return false;
        //         if (!_levelController.CubeCount.TryGetValue(colorClick, out var count))
        //         {
        //             UILayerManager.Instance.ShowPopupFeedBackLayer(TextContentDisplay.GetI2("ui_chose_other_color"));
        // #if UNITY_EDITOR
        //             Debug.LogError("Hay chon mau khac");
        // #endif
        //             return false;
        //         }
        //         List<WoolControl> woolChose   = new ();
        //         List<int>         indexsChose = new ();
        //
        //         var wools         = _levelController.WoolControls;
        //         var maxIndexLayer = _levelController.MaxLayer;
        //
        //         int  countRemove     = 0;
        //         for (int i = 0; i <= maxIndexLayer; i++)
        //         {
        //             foreach (var wool in wools)
        //             {
        //                 if (wool.MeshObjectData.ColorStack.Count < i + 1) continue;
        //                 if (wool.transform.GetInstanceID() == startPoint.GetInstanceID()) continue;
        //                 var color = wool.MeshObjectData.ColorStack[i];
        //                 if (color.Equals(colorClick))
        //                 {
        //                     countRemove++;
        //                     woolChose.Add(wool);
        //                     indexsChose.Add(i);
        //                 }
        //                 if (countRemove == 2) break;
        //             }
        //             if (countRemove == 2) break;
        //         }
        //
        //         if (countRemove < 2) return false;
        //
        //         var rollWool          = GenericObjectPool.Instance.PopFromPool(RollWoolPrefab, instantiateIfNone: true);
        //         var currentColorClick = colorPalleteData.colorPallete[colorClick];
        //         RainBowTargetControl
        //            .AddChild(-1, out var headtrans);
        //         rollWool
        //            .GetComponent<WoolRollAnimator>()
        //            .ResetMesh()
        //            .SetParent(headtrans)
        //            .SetColor(colorClick)
        //            .PlayAnimAddToQueue(RollWoolAnimationExtensions.ParentType.CubeTarget);
        //         ChoseYarnWool(rollWool.transform, startPoint, spiralPath, currentColorClick);
        //
        //         for (var i = 0; i < 2; i++)
        //         {
        //             rollWool = GenericObjectPool.Instance.PopFromPool(RollWoolPrefab, instantiateIfNone: true);
        //             RainBowTargetControl
        //                .AddChild(-1, out var headtrans1);
        //             rollWool
        //                .GetComponent<WoolRollAnimator>()
        //                .ResetMesh()
        //                .SetParent(headtrans1)
        //                .SetColor(colorClick)
        //                .PlayAnimAddToQueue(RollWoolAnimationExtensions.ParentType.CubeQueue);
        //             ChoseYarnWool(rollWool.transform, woolChose[i].transform, woolChose[i]
        //                        .GetSpiralPath(),      currentColorClick
        //                 );
        //             woolChose[i]
        //                .PLayAnim(colorClick);
        //         }
        //
        //         _isUsingRainBowBooster = false;
        //         _levelController.CubeCount[colorClick]--;
        //         if (_levelController.CubeCount[colorClick] == 0)
        //             _levelController.CubeCount.Remove(colorClick);
        //
        //         HideRainBow()
        //            .Forget();
        //
        //         return true;
    }

    #endregion



    #region USE_VACUUM_CLEANER_BOOSTER

    // public IEnumerator UseVacuumCleanerBooster()
    // {
    //     CubeTargetControl target = null;
    //     string colorTarget = null;
    //     int needChildCount = 0;
    //     int indexTarget = 0;
    //     for (var index = 0; index < CurrentCubeTargets.Count; index++)
    //     {
    //         CubeTargetControl cube = CurrentCubeTargets[index];
    //         if (!cube.IsActive || cube.IsPlayAnim()) continue;
    //         indexTarget = index;
    //         target = cube;
    //         needChildCount = cube.TakeChildMissPartCount();
    //         colorTarget = cube.GetColor();
    //         break;
    //     }

    //     if (target == null || needChildCount <= 0)
    //     {
    //         StartCoroutine(CoroutineBridge.WaitFor(UILayerManager
    //            .Instance
    //            .ShowPopupFeedBackLayer(TextContentDisplay.GetI2("ui_vacuum_cleaner_cant_use"))));
    //         return;
    //     }


    //     DataManager.ChangeVacuumCleaner(-1);
    //     //UserBehaviorTracker.SendUseItemTracking(FirebaseEventName.ClearBox_Booster, 1);

    //     target.SetACtiveVacuumCleanerAnimation(true);
    //     GameAudioManager.Instance.PlayOneShot("sfx_meohutbui");
    //     yield return new WaitForSeconds(LevelController.WoolAnimationData.TimeDelayVacuumCleaner);
    //     if (target == null) yield break;


    //     List<WoolControl> woolChose = new();
    //     List<int> indexsChose = new();

    //     var wools = _levelController?.WoolControls;
    //     var maxIndexLayer = _levelController?.MaxLayer;
    //     if (wools == null || string.IsNullOrEmpty(colorTarget)) return;
    //     int countRemove = 0;
    //     for (int i = 0; i <= maxIndexLayer; i++)
    //     {
    //         foreach (var wool in wools)
    //         {
    //             if (wool == null) continue;
    //             if (wool.ColorStack.Count < i + 1) continue;

    //             var color = wool.ColorStack[i];
    //             if (!color.Equals(colorTarget)) continue;
    //             countRemove++;
    //             wool.SetIsVacuumChose(true);
    //             woolChose.Add(wool);
    //             indexsChose.Add(i);
    //             if (countRemove == needChildCount) break;
    //         }
    //         if (countRemove == needChildCount) break;
    //     }

    //     if (countRemove < needChildCount)
    //     {
    //         foreach (var wool in woolChose)
    //         {
    //             wool.SetIsVacuumChose(false);
    //         }
    //         return;
    //     }

    //     for (int i = 0; i < needChildCount; i++)
    //     {
    //         target
    //            .AddChild(indexTarget, out var headtrans);
    //         if (headtrans == null)
    //         {
    //             woolChose[i].SetIsVacuumChose(false);
    //             continue;
    //         }
    //         var rollWool = GenericObjectPool.Instance.PopFromPool(RollWoolPrefab, instantiateIfNone: true);
    //         rollWool
    //            .GetComponent<WoolRollAnimator>()
    //            .ResetMesh()
    //            .SetParent(headtrans)
    //            .SetColor(colorTarget)
    //            .PlayAnimAddToQueue(RollWoolAnimationExtensions.ParentType.CubeTarget);
    //         ChoseYarnWool(rollWool?.transform, woolChose[i]?.transform, woolChose[i]?
    //            .GetSpiralPath(), colorTarget);

    //         woolChose[i]
    //            .PLayAnim(colorTarget);
    //     }
    // }

    #endregion

    #region USE_BROOM_BOOSTER

    public bool IsBroomTut, IsUsingBroom;

    private float _waitTimeCatFly = 1f;

    public IEnumerator UseBroomBoosterCleanUpQueue()
    {
        yield return null;
        IsUsingBroom = true;
        //GamePlayUIManager.Instance.SetBroomBoosterInteractable(true);
        //GamePlayUIManager.Instance.SetRedoBoosterInteractable(true);
        // WoolBasket.transform.DOKill();
        // WoolBasket
        //    .transform
        //    .DOMove(_displayPosWoolBasket, 1.2f)
        //    .SetEase(Ease.OutBounce);
        GamePlayUIManager.Instance?.ActiveWoolBasket(true);
        _isUseBroomBooster = true;
        List<QueueTargetControl> QueueTargets = new();
        foreach (var queue in this.CurrentQueueTargets)
        {
            if (!queue.IsAtive()) continue;
            QueueTargets.Add(queue);
        }
        //SoundManager.Instance.PlayOneShotDelayed("sfx_meobay", 0.5f);

        yield return new WaitForSeconds(_waitTimeCatFly);
        GameEventManager.PlayAnimPreLose?.Invoke(false);
        foreach (var queue in QueueTargets)
        {
            if (!queue.IsAtive() || !queue.IsHasWoolRool() || queue.IsReDo()) continue;
            var rollWoolChild = queue.transform.GetChild(2);
            _scaleDefaultRollWool = rollWoolChild.lossyScale;
            var rollWoolAnimator = rollWoolChild.GetComponent<WoolRollAnimator>();
            rollWoolAnimator._isPopToBroomPool = true;
            _broomBoosterPool.Add(rollWoolAnimator);
            rollWoolChild.SetParent(null);
            var startPos = rollWoolChild.position;
            if (GamePlayUIManager.Instance == null) yield break;
            var endPos = GetUIWorldPosition(GamePlayUIManager.Instance.WoolBasket);
            var midPos = (startPos + endPos) * 0.5f + Vector3.up * 0.5f;
            rollWoolChild.DOKill();
            rollWoolChild
               .DOPath(new[]
                       {
                           rollWoolChild.position,
                           midPos,
                           endPos
                       }, 0.6f, PathType.CatmullRom
                    )
               .SetEase(Ease.OutCubic)
               .OnComplete(() =>
                        {
                            rollWoolChild.DOKill();
                            rollWoolChild.DOScale(Vector3.zero, 0.3f);
                        }
                    );
            queue.ResetDefault();
            _queueCount--;
        }
        yield return null;
        _isUseBroomBooster = false;
        IsUsingBroom = false;
        IsBroomBoosterTutorial = false;
    }

    #endregion

    #region BROOM_BOOSTER_TUT

    public bool IsBroomBoosterTutorial;
    private readonly int woolChoseCount = 4;

    public void OnChoseRandomWoolForUseBroomBooster(int countMax, bool isBroomTut = false, long woolId = -1)
    {
        if (!_levelController) return;
        int count = 0;
        for (int i = 0; i < _levelController.MaxLayer; i++)
        {
            foreach (var wool in _levelController.WoolControls)
            {
                if (wool.WoolID == woolId)
                {
                    wool.WoolRotation();
                    IsBroomBoosterTutorial = isBroomTut;
                    return;
                }
                if (wool.ColorStack.Count < i + 1 || woolId != -1) continue;
                if (count >= countMax) break;
                if (_colorTargets.Contains(wool.ColorStack[i])) continue;
                count++;
                wool.WoolRotation();
            }
            if (count >= countMax) break;
        }
        IsBroomBoosterTutorial = isBroomTut;
    }

    #endregion


    #region REDO

    public void UseRedoBooster()
    {
        var queueCount = CurrentQueueTargets.Count;
        WoolRollAnimator rollWool = null;
        for (int i = queueCount - 1; i >= 0; i--)
        {
            if (CurrentQueueTargets[i].IsReDo() || !CurrentQueueTargets[i].IsHasWoolRool() || CurrentQueueTargets[i].IsPopToBroomPool()) continue;
            if (CurrentQueueTargets[i].IsPlayAnim()) continue;
            rollWool = CurrentQueueTargets[i]
               .transform
               .GetChild(2)
               .GetComponent<WoolRollAnimator>();
            if (rollWool._woolRedo.IsPlayAnim) continue;
            break;
        }
        if (rollWool == null || rollWool.IsRedo || rollWool._woolRedo.IsPlayAnim) return;
        rollWool.IsRedo = true;
        //DataManager.ChangeRedo(-1);
        _queueCount--;
        var rollColor = colorPalleteData.colorPallete_New[rollWool._currentColor];
        ChoseYarnWool(rollWool._woolRedo.transform, rollWool.transform, rollWool._woolRedo.GetSpiralPath(), rollWool._currentColor, true);
        rollWool._woolRedo.ReFillMesh(rollWool._currentColor);
        
        //SoundManager.Instance.PlayOneShot("redo_wool");

        rollWool.PlayAnimRedo();
        GameEventManager.PlayAnimPreLose?.Invoke(false);
    }

    #endregion
    #region HELPER

    public IEnumerator ActiveHelperItem()
    {
        //UIFullScreenBlocker.Instance?.Lock(5);
        foreach (var cube in CurrentCubeTargets)
        {
            if (cube.IsActive) continue;
            yield return StartCoroutine(cube.OnOpenCube());
            //UIFullScreenBlocker.Instance?.Unlock(5);
            yield break;
        }

        yield return StartCoroutine(UseBroomBoosterCleanUpQueue());
        //UIFullScreenBlocker.Instance?.Unlock(5);
    }

    public void OnCompleteBroomBoosterTutorial()
    {
        IsBroomBoosterTutorial = false;
        GameEventManager.OnBroomBoosterComplete -= OnCompleteBroomBoosterTutorial;
    }

    private IEnumerator DisplayRainBowBox()
    {
        yield break;
        // GameAudioManager.Instance.PlayOneShot("box_whoosh");
        // //if (soundDict.GetAudioClip("box_whoosh") != null) GameAudioManager.Instance.PlayOneShot(soundDict.GetAudioClip("box_whoosh"), soundDict.GetSoundVolume("box_whoosh"));
        // foreach (CubeTargetControl t in CurrentCubeTargets)
        // {
        //     t.DisplayRainBowBoxAnimation();
        // }
        //
        // RainBowTargetControl
        //    .DisplayRainBowBoxAnimation();
        // yield return new WaitForSeconds(0.5f);
        // foreach (var target in CurrentCubeTargets)
        // {
        //     target.BakeAnimPosition();
        // }
    }

    private IEnumerator HideRainBowBox()
    {
        yield break;
        // GameAudioManager.Instance.StopSpecialSoundLoop();
        // yield return new WaitForSeconds(0.5f);
        // foreach (CubeTargetControl t in CurrentCubeTargets)
        // {
        //     t.HideRainBowBoxAnimation();
        // }
        // yield return new WaitForSeconds(0.15f);
        // RainBowTargetControl.HideRainBowBoxAnimation();
        // yield return new WaitForSeconds(0.5f);
        // SmoothRepositioner();
        // yield return new WaitForSeconds(0.3f);
        // foreach (var target in CurrentCubeTargets)
        // {
        //     target.BakeAnimPosition();
        // }
        // yield return new WaitForSeconds(0.3f);
        // RainBowTargetControl.SetDefault();
    }

    private Vector3 GetWorldPosUIElement(RectTransform rectTransform)
    {
        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        Vector3 screenPos;

        switch (canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                screenPos = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
                break;

            case RenderMode.ScreenSpaceCamera:
                screenPos = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTransform.position);
                break;

            case RenderMode.WorldSpace:
                // Đối với World Space UI, có thể trực tiếp sử dụng world position
                return rectTransform.position;

            default:
                screenPos = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
                break;
        }
        Vector3 worldPos = CameraContainer.Instance.MainCamera.ScreenToWorldPoint(
                new Vector3(screenPos.x, screenPos.y, -5)
            );

        return worldPos;
    }

    Vector3 GetUIWorldPosition(RectTransform uiTarget)
    {
        if (uiTarget != null)
        {
            var point = (Vector2)uiTarget.position + uiTarget.rect.center;
            var viewportPosition = RectTransformUtility.ScreenPointToRay(CameraContainer.Instance?.FakeUICamera, point);
            return viewportPosition.origin;
        }

        return new Vector3(0, 3.9f, 0);// Default position if uiTarget is null
    }

    #endregion
}
