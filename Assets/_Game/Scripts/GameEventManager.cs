using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public static class GameEventManager
{
    #region IAP

    #endregion

    #region SoundManager

    public static UnityEvent<float> OnChangeMainSound     = new UnityEvent<float>();
    public static UnityEvent<float> OnChangeSensitivity   = new UnityEvent<float>();
    public static UnityEvent<float> OnChangeSound         = new UnityEvent<float>();
    public static UnityEvent<float> OnChangeSoundFx       = new UnityEvent<float>();
    public static UnityEvent<bool>  OnEnableSoundFxIngame = new UnityEvent<bool>();
    public static UnityEvent<bool>  OnSetBoolEndGameUI    = new UnityEvent<bool>();

    #endregion

    #region LayerManager

    #endregion

    #region Response Loading


    #endregion

    #region BottomBarLayer



    #endregion

    #region GamePlay Event

    public static Action<bool>                OnEndGameAction;
    public static Action<bool>                SetupObjectSpawnActive;
    public static Action<bool>                SetupObjectTargetActive;
    public static Action                      OnGoldChange;
    public static Action                      OnGainGold;
    public static Action<int>                 OnGainBooster;
    public static Action                      OnWinThisLevel;
    public static Action                      OnLostThisLevel;

    public static Action OnHoleDataChange;
    public static Action OnBroomDataChange;
    public static Action OnRainbowBoxDataChange;
    public static Action OnRedoDataChange;
    public static Action OnVacuumCleanerDataChange;

    public static Action OnInstanceNewLevel;
    public static  Action OnNewLevel;
    public static  Action ShowPopupOfferEndGame;
    public static  Action OnLoadLevelDone;

    public static Action<LevelController> OnDoneLoadLevel;

    public static Action Tool_OnLevelModelSpawned;
    public static Action<LevelConfigData> Tool_OnLevelConfigChanged;
    public static Action<long> Tool_OnWoolControlSelected;
    public static Action<string> Tool_OnBoxSelected;

    public static Action<float> ChangeCameraFOVThroughButton;
    public static Action        ReCenterModelThroughButton;

    #endregion

    #region Endgame UI
    public static Action OnEndGameUIActive;
    public static Action OnEndGameUIDeactive;
    public static Action OnModelStopRotatingInEndgameUI;
    #endregion

    #region Profile

    public static Action<int>    OnAvatarSelected;
    public static Action<string> OnChangeNameSuccess;
    public static Action<int> OnChangeAvatarSuccess;
    public static Action OnFrameChange;
    public static Action<string>    OnLanguageSelected; 
    public static Action<string> OnLanguageChanged;
    public static Action OnHeartDataChange;
    public static Action<int> OnSelectFrameCell;

    #endregion

    #region Tutorial

    public static Action OnGamePlayTutorialComplete;
    public static Action OnAddHoldBoosterComplete;
    public static Action OnBroomBoosterComplete;
    public static Action OnRainbowBoxBoosterComplete;
    
    public static Action<bool> OnShowAddholeBoosterBtn;
    public static Action<bool> OnShowBroomBoosterBtn;
    public static Action<bool> OnShowRainbowBoxBoosterBtn;

    #endregion

    #region Ads


    #endregion
    
    #region DynamicDifficulty
    
    public static Action OnUseSawAdsBooster;
    public static Action OnUseSaveBooster;
    public static Action OnSaveBooster;
    public static Action OnReceiveBooster;

    #endregion
    
    #region Intro
    public static Action<bool> OnIntroComplete;
    public static Action<bool> OnIntroComplete2;
    #endregion
    
    #region Daily Gift

    public static Action<bool> OnActiveNotifyDailyGift;
    public static Action<bool> OnDailyGiftActive;
    public static Action<int>  TestNextDayDailyGift;
    public static Action       OnCountDownFinished;

    #endregion

    #region CURRENCY LAYER

    #region gold fly effect
    public static Action<int> OnFirstCoinArriveAtTopBar;
    #endregion

    #endregion

    public static Action OnOfferPackClosed;

    public static Action<string> OnOfferPackOpened;
    public static Action OnMeshCountClickChange;
    public static Action IgnoreLastPack;
    public static Action CancelIgnoreLastPackShop;

    public static Action<string> OnReceivePackInfoFromStore;

    public static Action<bool> PlayAnimPreLose;
    public static Action ReplayAnimHole;
    
    public static Action<bool> OnCountYarnComplete;
    public static Action OnShowCountYarnComplete;
    public static Action ShowAnimCountYarn;
    
    #region Redo Booster

    public static Action<bool> HighlightRedoButton;
    public static Action<bool> HighLightFirstHole;
    public static Func<Transform> GetHandRedoBoosterTarget;
    public static Func<Transform> GetHandVacuumBoosterTarget;
    public static Func<Transform> GetHandBroomBoosterTarget;
    public static Action OnUseRedoBooster;
    public static Action OnUseVacuumBooster;
    public static Action OnUseCatFlyBooster;
    public static Action<bool> HighlightVacuumCleanerButton;
    public static Action<bool> HighlightFirstCubeBox;
    public static Action OnAWoolMeshRedo;
    #endregion
        #region _gameplay
    public static Action OnAWoolMeshCompleted;
    #endregion
}