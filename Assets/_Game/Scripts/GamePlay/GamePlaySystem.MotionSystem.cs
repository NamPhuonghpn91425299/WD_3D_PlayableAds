using System.Collections.Generic;
using UnityEngine;

// Đánh dấu đây là một phần của class GamePlaySystem
public partial class GamePlaySystem
{
    // Một Dictionary tĩnh và chỉ đọc để ánh xạ loại cảm xúc (enum) với "Hash" của parameter trong Animator.
    public static readonly Dictionary<EMotionType, int> MainMotionParameterHash = new ()
    {
        // Sử dụng Animator.StringToHash("...") để chuyển đổi tên parameter thành một số nguyên (integer).
        // Việc này hiệu quả hơn rất nhiều so với việc dùng chuỗi ("idle") mỗi lần gọi Animator,
        // vì so sánh số nguyên nhanh hơn so sánh chuỗi.
        {EMotionType.None,      Animator.StringToHash("idle")},       // Hash của state 'idle'
        {EMotionType.Shy,       Animator.StringToHash("isShy")},      // Hash của trigger 'isShy'
        {EMotionType.Surpries,  Animator.StringToHash("isSurpries")}, // Hash của trigger 'isSurpries'
    };
    
    // Hash cho một parameter kiểu Float tên là "RandomValue".
    // Parameter này có thể dùng để điều khiển blend tree hoặc một giá trị ngẫu nhiên nào đó trong animation.
    public static int RandomValueParameterHash = Animator.StringToHash("RandomValue");

    /// <summary>
    /// Kích hoạt một animation cảm xúc trên Animator của nhân vật chính.
    /// </summary>
    /// <param name="motionType">Loại cảm xúc muốn kích hoạt (từ enum EMotionType).</param>
    /// <param name="motionBlendValue">Giá trị blend, dùng cho parameter "RandomValue". Mặc định là 1.</param>
    public void RaiseMotion(EMotionType motionType, float motionBlendValue = 1f)
    {
        // 1. Kiểm tra đầu vào: Nếu không có motion nào được yêu cầu thì thoát.
        if (motionType == EMotionType.None) return;
        
        // 2. Lấy Animator từ _meshController. Đây là Animator của nhân vật/đối tượng chính.
        var mainMotionAnimator = _meshController.MainMotionAnimator;
        if (!mainMotionAnimator) return; // Nếu không có Animator thì thoát.

        // 3. Kiểm tra trạng thái hiện tại: Chỉ cho phép kích hoạt motion mới khi đang ở trạng thái 'idle'.
        var state = mainMotionAnimator.GetCurrentAnimatorStateInfo(0); // Lấy thông tin state của layer 0.
        // So sánh hash của state hiện tại với hash của state 'idle' đã lưu.
        // Điều này ngăn việc kích hoạt một motion mới khi một motion khác đang chạy (ví dụ: đang "ngạc nhiên" thì không thể "xấu hổ" ngay lập tức).
        if (state.shortNameHash != MainMotionParameterHash[EMotionType.None]) return;
        
        // 4. Lấy hash của motion cần kích hoạt từ Dictionary.
        int motionHash = MainMotionParameterHash[motionType];
        
        // 5. Thiết lập giá trị blend.
        // Giới hạn giá trị trong khoảng [-1, 1].
        motionBlendValue = Mathf.Clamp(motionBlendValue, -1f, 1f);
        // Đặt giá trị cho parameter "RandomValue" trên Animator.
        mainMotionAnimator.SetFloat(RandomValueParameterHash, motionBlendValue);
        
        // 6. Kích hoạt Trigger.
        // Trigger trong Animator sẽ tự động reset sau khi được sử dụng.
        // Nó sẽ khiến Animator chuyển từ state 'idle' sang state tương ứng (ví dụ: 'Shy' hoặc 'Surpries').
        mainMotionAnimator.SetTrigger(motionHash);
    }
}


// Enum định nghĩa các loại cảm xúc có thể có.
// Việc dùng enum giúp code dễ đọc, dễ bảo trì và tránh lỗi gõ sai chuỗi.
public enum EMotionType
{
    None     = 0,
    Shy      = 1, // Xấu hổ
    Surpries = 2, // Ngạc nhiên
}