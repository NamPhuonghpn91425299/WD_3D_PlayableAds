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
        CameraController.Instance.BlockRotate(true);
        CameraController.Instance.SetBlockHandTap(true);
        CameraController.Instance.SetBlockHold(true);
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
            GamePlayUIManager.Instance?.ShowWinPanel();
            Debug.Log($"[GamePlayManager] Trigger Win Game Action");
        }
        else
        {
            yield return new WaitForSeconds(1.2f);
            GamePlayUIManager.Instance?.ShowLosePanel();
            Debug.Log($"[GamePlayManager] Trigger Lose Game Action");
        }

        yield return null;
        //UIFullScreenBlocker.Instance.Unlock(4);
    }
}
