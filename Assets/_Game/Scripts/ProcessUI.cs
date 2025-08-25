
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ProcessUI : MonoBehaviour
{
    public Text text;
    public Text shadow;
    public int totalMesh;

    public void Update()
    {
        text.text = $"{GamePlaySystem.Instance.CurrentColorCollected}/{totalMesh}";
        shadow.text = $"{GamePlaySystem.Instance.CurrentColorCollected}/{totalMesh}";
    }
}
