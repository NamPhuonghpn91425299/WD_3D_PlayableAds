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
        // Error handling cho GamePlayManager.Instance
        if (GamePlayManager.Instance == null)
        {
            Debug.LogError("GamePlayManager.Instance is null!");
            return;
        }

        // Error handling cho Text components
        if (text == null || shadow == null)
        {
            Debug.LogError("Text components are not assigned!");
            return;
        }

        totalMesh = GamePlayManager.Instance.TotalColor;

        // Cập nhật lần đầu
        UpdateProgressText(GamePlayManager.Instance.MeshCountClick);

        // Subscribe to event từ GamePlayManager

        //GamePlayManager.Instance.OnMeshCountClickChange += HandleColorCollectedChanged;
    }

    private void OnDestroy()
    {
        // Unsubscribe từ event để avoid memory leaks
        if (GamePlayManager.Instance != null)
        {
            //GamePlayManager.Instance.OnMeshCountClickChange -= HandleColorCollectedChanged;
        }
    }

    private void HandleColorCollectedChanged(int newMeshCount)
    {
        UpdateProgressText(newMeshCount);
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
