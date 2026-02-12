using System;
using UnityEngine;
using UnityEngine.UI;

public class ProcessUI : MonoBehaviour
{
    public Text text;
    public Text shadow;
    public int totalMesh;
    
    private const int MESH_PER_CUBE = 3;

    private void Start()
    {
        // Error handling cho GamePlaySystem.Instance
        if (GamePlaySystem.Instance == null)
        {
            Debug.LogError("GamePlaySystem.Instance is null!");
            return;
        }
        
        // Error handling cho Text components
        if (text == null || shadow == null)
        {
            Debug.LogError("Text components are not assigned!");
            return;
        }
        
        totalMesh = GamePlaySystem.Instance.cubeCountClaimed * MESH_PER_CUBE;
        
        // Cập nhật lần đầu
        UpdateProgressText(GamePlaySystem.Instance.CurrentColorCollected);
        
        // Subscribe to event từ GamePlaySystem
        GamePlaySystem.Instance.OnColorCollectedChanged += HandleColorCollectedChanged;
    }

    private void OnDestroy()
    {
        // Unsubscribe từ event để avoid memory leaks
        if (GamePlaySystem.Instance != null)
        {
            GamePlaySystem.Instance.OnColorCollectedChanged -= HandleColorCollectedChanged;
        }
    }
    
    private void HandleColorCollectedChanged(int newColorCollected)
    {
        UpdateProgressText(newColorCollected);
    }
    
    private void UpdateProgressText(int colorCollected)
    {
        if (text != null && shadow != null)
        {
            string progressText = $"{colorCollected}/{totalMesh}";
            text.text = progressText;
            shadow.text = progressText;
        }
    }
}
