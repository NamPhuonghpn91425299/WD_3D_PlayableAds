using System;
using UnityEngine;
using UnityEngine.UI;

public class ProcessUI : MonoBehaviour
{
    public Text text;
    public Text shadow;
    public int totalMesh;

    private int _lastCollected = -1;
    private string _cachedSuffix; // "/{total}"
    private GamePlaySystem _game;

    private void Start()
    {
        _game = GamePlaySystem.Instance;

        // Hiển thị theo đơn vị màu
        totalMesh = _game.TotalColor;
        _cachedSuffix = "/" + totalMesh;

        // Khởi tạo UI lần đầu
        ForceRefresh();
    }

    public void Update()
    {
        TryUpdateUI();
    }

    // Chỉ cập nhật khi thay đổi để tránh GC và tốn CPU
    private void TryUpdateUI()
    {
        int collected = _game.CurrentColorCollected; // đơn vị màu (units)
        if (collected == _lastCollected) return;

        _lastCollected = collected;
        string value = collected + _cachedSuffix;
        if (text) text.text = value;
        if (shadow) shadow.text = value;
    }

    // Dùng khi cần cưỡng bức refresh (ví dụ sau khi load level)
    public void ForceRefresh()
    {
        _lastCollected = -1;
        TryUpdateUI();
    }
}