using UnityEngine;
using System.Collections.Generic;

public static class VibrationPatterns
{
    /// <summary>
    /// Các kiểu rung được định nghĩa sẵn
    /// </summary>
    public enum PatternType
    {
        Continuous,     // Rung liên tục
        Pulse,          // Rung giật cục đều đặn
        DoubleTap,      // Rung 2 nhịp ngắn
        TripleTap,      // Rung 3 nhịp ngắn
        Heartbeat,      // Rung như nhịp tim
        BuildUp,        // Rung tăng dần
        FadeOut,        // Rung giảm dần
        Wave,           // Rung sóng (mạnh-yếu-mạnh)
        SOS,            // Pattern SOS (...---...)
        Success,        // Rung khi thành công
        Warning         // Rung cảnh báo
    }

    /// <summary>
    /// Gọi vibration với pattern được chọn
    /// </summary>
    public static void Vibrate(PatternType pattern, int duration = 0)
    {
#if UNITY_EDITOR
        Debug.Log($"[VIBRATION] Would vibrate with pattern: {pattern} for {duration}ms");
        return;
#endif

        string jsPattern = GetPatternString(pattern, duration);
        string jsCode = $@"
            try {{
                if (typeof navigator !== 'undefined' && navigator.vibrate) {{
                    navigator.vibrate({jsPattern});
                    console.log('[VIBRATION] Pattern: {pattern} = ' + {jsPattern});
                }} else {{
                    console.log('[VIBRATION] API not supported');
                }}
            }} catch(e) {{
                console.log('[VIBRATION] Error: ' + e);
            }}
        ";
        
        Application.ExternalEval(jsCode);
    }

    /// <summary>
    /// Lấy string pattern cho JavaScript
    /// </summary>
    private static string GetPatternString(PatternType pattern, int duration)
    {
        switch (pattern)
        {
            case PatternType.Continuous:
                return duration > 0 ? duration.ToString() : "200";
                
            case PatternType.Pulse:
                return CreatePulsePattern(duration > 0 ? duration : 1000, 50, 50);
                
            case PatternType.DoubleTap:
                return "[100, 50, 100]";
                
            case PatternType.TripleTap:
                return "[100, 50, 100, 50, 100]";
                
            case PatternType.Heartbeat:
                return "[100, 100, 200, 200, 100, 100, 200, 500]";
                
            case PatternType.BuildUp:
                return "[50, 100, 100, 100, 150, 100, 200, 100, 300]";
                
            case PatternType.FadeOut:
                return "[300, 100, 200, 100, 150, 100, 100, 100, 50]";
                
            case PatternType.Wave:
                return "[200, 100, 100, 100, 200, 100, 100, 100, 200]";
                
            case PatternType.SOS:
                // S(...) O(---) S(...)
                return "[100,50,100,50,100, 200, 300,50,300,50,300, 200, 100,50,100,50,100]";
                
            case PatternType.Success:
                return "[50, 50, 100, 50, 200]";
                
            case PatternType.Warning:
                return "[500, 100, 500]";
                
            default:
                return "200";
        }
    }

    /// <summary>
    /// Tạo pattern rung giật cục với thời gian tùy chỉnh
    /// </summary>
    private static string CreatePulsePattern(int totalDuration, int vibrateTime, int pauseTime)
    {
        List<int> pattern = new List<int>();
        int currentDuration = 0;
        bool isVibrating = true;
        
        while (currentDuration < totalDuration)
        {
            if (isVibrating)
            {
                int duration = Mathf.Min(vibrateTime, totalDuration - currentDuration);
                pattern.Add(duration);
                currentDuration += duration;
            }
            else
            {
                int duration = Mathf.Min(pauseTime, totalDuration - currentDuration);
                if (currentDuration + duration < totalDuration)
                {
                    pattern.Add(duration);
                    currentDuration += duration;
                }
                else
                {
                    break;
                }
            }
            isVibrating = !isVibrating;
        }
        
        return "[" + string.Join(", ", pattern) + "]";
    }
    
    /// <summary>
    /// Tạo custom pattern từ mảng thời gian
    /// </summary>
    public static void VibrateCustom(int[] pattern)
    {
#if UNITY_EDITOR
        Debug.Log($"[VIBRATION] Custom pattern: [{string.Join(", ", pattern)}]");
        return;
#endif

        string jsPattern = "[" + string.Join(", ", pattern) + "]";
        string jsCode = $@"
            if (navigator.vibrate) {{
                navigator.vibrate({jsPattern});
                console.log('[VIBRATION] Custom: ' + {jsPattern});
            }}
        ";
        
        Application.ExternalEval(jsCode);
    }
    
    /// <summary>
    /// Dừng vibration đang chạy
    /// </summary>
    public static void Stop()
    {
#if !UNITY_EDITOR
        Application.ExternalEval("if(navigator.vibrate) navigator.vibrate(0);");
#endif
    }
}
