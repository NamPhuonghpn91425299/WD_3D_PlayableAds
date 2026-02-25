using UnityEngine;

[CreateAssetMenu(fileName = "AppMetricaTrackingConfigSO", menuName = "ScriptableObjects/AppMetricaTrackingConfigSO", order = 1)]
public class AppMetricaTrackingConfig : ScriptableObject
{
    public string ApiKey
    {
        get
        {
#if UNITY_ANDROID
            return androidApiKey;
#elif UNITY_IOS
                return iosApiKey;
#else
                return string.Empty;
#endif
        }
    }

    [SerializeField]
    private string androidApiKey;

    [SerializeField]
    private string iosApiKey;
}