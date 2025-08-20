using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class EndGameUI : MonoBehaviour
{
    [Tooltip("Panel UI hiển thị khi kết thúc game (thắng/thua).")] 
    [SerializeField]
    private CanvasGroup winGamePanel;
    [SerializeField] 
    private CanvasGroup loseGamePanel;
    [SerializeField] 
    private CanvasGroup btnPlay;
    [SerializeField] 
    private Renderer  renderer;
    [SerializeField]
    private PaintingOverviewAnimation paintingOverviewAnimation;
    public AudioClip loseSound;
    public AudioClip winSound;
    private Material _material;
    public static EndGameUI Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        
        _material = new Material(renderer.material);
        _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _material.SetInt("_ZWrite", 0);
        _material.EnableKeyword("_ALPHABLEND_ON");
        _material.renderQueue = 3000;
        renderer.material = _material;
        renderer.enabled = false;
    }
    
    public void ShowWinGamePanel()
    {
        StartCoroutine(WinGame());
    }
    
    public void ShowLoseGamePanel()
    {
        StartCoroutine(LoseGame());
    }
    
    
    private IEnumerator WinGame()
    {
        //Debug.Log("=== STARTING WIN ANIMATION ===");
        
        // Bước 1: Switch camera
        var instance = CameraContainer.Instance;
        if (instance != null)
        {
            //Debug.Log("Switching cameras...");
            instance.FakeUICamera.enabled = false;
            instance.EndgameModelCamera.enabled = true;
        }
        
        // Bước 2: Phát âm thanh
        if (winSound != null)
        {
            //Debug.Log("Playing win sound...");
            SoundManager.Instance.PlayOneShot(winSound, 1f);
        }
        
        // Bước 3: Bắt đầu outro animation
        if (paintingOverviewAnimation != null)
        {
            //Debug.Log("Starting painting outro animation...");
            paintingOverviewAnimation.StartOutroAnimation();
        }
        
        // Bước 4: Chờ animation painting hoàn tất
        yield return new WaitForSeconds(1.6f); // Tăng thêm thời gian để đảm bảo outro hoàn tất
        
        // Bước 5: Bắt đầu fade background
        //Debug.Log("Starting fade background...");
        renderer.enabled = true;
        FadeTo(.7f, 1f); // Làm chậm fade để mượt hơn
        
        // Bước 6: Chờ fade background gần xong rồi hiện UI
        //yield return new WaitForSeconds(0.8f);
        
        //Debug.Log("Starting UI fade in...");
        yield return StartCoroutine(FadeBetweenCanvasGroups(winGamePanel, btnPlay, 1.2f, .8f));
        
        Debug.Log("=== WIN ANIMATION COMPLETED ===");
    }
    
    private IEnumerator LoseGame()
    {
        
        if (loseSound != null)
            SoundManager.Instance.PlayOneShot(loseSound, 1f);
        yield return StartCoroutine(FadeBetweenCanvasGroups(loseGamePanel, btnPlay, 1f, .6f));
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
    /// <summary>
    /// Fade material tới alpha target trong thời gian duration
    /// </summary>
    /// <param name="targetAlpha">Alpha mục tiêu (0-1)</param>
    /// <param name="duration">Thời gian fade (giây)</param>
    public void FadeTo(float targetAlpha, float duration)
    {
        if (_material == null)
        {
            Debug.LogError("Material is null!");
            return;
        }
        
        // Kill tween cũ nếu có
        _material.DOKill();
        
        Debug.Log($"Fading from {_material.color.a} to {targetAlpha} in {duration}s");
        
        // Dùng DOTween.To thay vì DOFade để chắc chắn
        DOTween.To(() => _material.color.a, 
                x => {
                    Color color = _material.color;
                    color.a = x;
                    _material.color = color;
                }, 
                targetAlpha, 
                duration)
            .SetEase(Ease.OutQuad);
    }
    // Test function - gọi này để test
    [ContextMenu("Test Fade Out")]
    void TestFadeOut()
    {
        FadeTo(0f, 1f);
    }
    
    [ContextMenu("Test Fade In")]
    void TestFadeIn()
    {
        FadeTo(1f, 1f);
    }
}
