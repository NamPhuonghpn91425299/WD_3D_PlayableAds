using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GamePlayManager
{
    public bool IsWinGame;

    public void CheckEndGame()
    {
        if (_currentColorCollected == _levelController?.TotalColor && _levelController?.TotalColor != 0)
        {
            //Win
            StartCoroutine(OnEndGameAction(true));
        }
        if (_queueCount >= TotalQueueActiveCount && TotalCubeActive == CubeReadyCount)
        {
            //Lose
            StartCoroutine(OnEndGameAction(false));
        }
    }

    public void CheckTurnOffCube(int indexCube, bool active)
    {
        if (!active)
        {
            CurrentCubeTargets[indexCube]
               .gameObject
               .SetActive(false);
            CurrentCubeTargets[indexCube].IsActive = false;
            TotalCubeActive--;
            CubeReadyCount--;
            SmoothRepositioner();
        }
    }

    private IEnumerator OnEndGameAction(bool isWin)
    {
        if (_isUseBroomBooster) yield break;
        //UIFullScreenBlocker.Instance.Lock(4);
        IsEndGame = true;
        if (!isWin)
            yield return new WaitForSeconds(1.2f);
        if (isWin)
        {
            if (!IsWinGame)
            {
                IsWinGame = true;
                GameEventManager.OnEndGameAction?.Invoke(true);
            }
        }
        else
        {
            // if (_replayCount <= LoseOffer && TotalCubeActive != maxCubeTarget)
            if (false)
            {
                _purchaseOfferCost = _replayCount == 0
                    ? AddCubeDataSO.FirstCubeTargetCost
                    : AddCubeDataSO.SecondCubeTargetCost;
                _offerIcon = AddCubeDataSO.AddCubeTargetIcon;
                _isUseOpenCubeoffer = true;
                _replayCount++;
                GameEventManager.ShowPopupOfferEndGame?.Invoke();
            }
            else
            {
                _purchaseOfferCost = broomDataSO.BoosterCost;
                _offerIcon = broomDataSO.BoosterIcon;
                _isUseOpenCubeoffer = false;
                GameEventManager.ShowPopupOfferEndGame?.Invoke();

            }

        }

        yield return null;
        //UIFullScreenBlocker.Instance.Unlock(4);
    }
}
