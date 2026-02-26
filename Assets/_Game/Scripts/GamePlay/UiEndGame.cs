using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiEndGame : MonoBehaviour
{
    [Tooltip("Panel UI hiển thị khi kết thúc game (thắng/thua).")]
    [SerializeField]
    private CanvasGroup winGamePanel;

    [SerializeField]
    private CanvasGroup loseGamePanel;
    [SerializeField]
    private CanvasGroup group;

    private void Start()
    {
        // Ẩn cả 2 panel khi bắt đầu
        if (winGamePanel != null) winGamePanel.gameObject.SetActive(false);
        if (loseGamePanel != null) loseGamePanel.gameObject.SetActive(false);
    }

    public void SetAlphaPanel( float alpha)
    {
        if (group != null)
        {
            group.alpha = alpha;
        }
    }
    [ContextMenu("Test Show Win Panel")]
    public void ShowWinPanel()
    {
        if (winGamePanel != null)
        {
            SoundManager.Instance.PlayOneShot("level_win");
            StartCoroutine(FadeBetweenCanvasGroups(winGamePanel, group, 1f, 0.5f));
            Luna.Unity.LifeCycle.GameEnded();
        }
    }
    [ContextMenu("Test Show Lose Panel")]
    public void ShowLosePanel()
    {
        if (loseGamePanel != null)
        {
            SoundManager.Instance.PlayOneShot("level_lose");
            StartCoroutine(FadeBetweenCanvasGroups(loseGamePanel, group, 1f, 0.5f));
            Luna.Unity.LifeCycle.GameEnded();
        }
    }
    private IEnumerator FadeBetweenCanvasGroups(CanvasGroup fadeIn, CanvasGroup fadeOut, float fadeInDuration,
        float fadeOutDuration)
    {
        // Chuẩn bị fade-in
        fadeIn.gameObject.SetActive(true);
        fadeIn.alpha = 0f;
        fadeIn.interactable = true;
        fadeIn.blocksRaycasts = true;

        // Chuẩn bị fade-out
        fadeOut.alpha = 1f;
        fadeOut.interactable = false;
        fadeOut.blocksRaycasts = false;

        float elapsed = 0f;
        float maxDuration = Mathf.Max(fadeInDuration, fadeOutDuration);

        while (elapsed < maxDuration)
        {
            // Tính alpha fade-in
            if (elapsed < fadeInDuration)
            {
                float tIn = elapsed / fadeInDuration;
                fadeIn.alpha = Mathf.Lerp(0f, 1f, tIn);
            }

            // Tính alpha fade-out
            if (elapsed < fadeOutDuration)
            {
                float tOut = elapsed / fadeOutDuration;
                fadeOut.alpha = Mathf.Lerp(1f, 0f, tOut);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Kết thúc chính xác
        fadeIn.alpha = 1f;
        fadeOut.alpha = 0f;
        fadeOut.gameObject.SetActive(false);
    }
}
