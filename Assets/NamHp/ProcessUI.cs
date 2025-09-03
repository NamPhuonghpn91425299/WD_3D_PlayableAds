using System;
using UnityEngine;
using UnityEngine.UI;

public class ProcessUI : MonoBehaviour
{
    public Text text;
    public Text shadow;
    public int totalMesh;

    private void Start()
    {
        //totalMesh = GamePlaySystem.Instance.TotalColor;
    }

    public void Update()
    {
        text.text = $"{GamePlaySystem.Instance.CurrentColorCollected}/{totalMesh}";
        shadow.text = $"{GamePlaySystem.Instance.CurrentColorCollected}/{totalMesh}";
    }
}