using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using static WoolControl;
// Đánh dấu đây là một phần của class GamePlaySystem
public partial class GamePlaySystem
{
    // Một Dictionary tĩnh và chỉ đọc để ánh xạ loại cảm xúc (enum) với "Hash" của parameter trong Animator.
    public static readonly Dictionary<EMotionType, int> MainMotionParameterHash = new Dictionary<EMotionType, int>()
    {
        // Sử dụng Animator.StringToHash("...") để chuyển đổi tên parameter thành một số nguyên (integer).
        // Việc này hiệu quả hơn rất nhiều so với việc dùng chuỗi ("idle") mỗi lần gọi Animator,
        // vì so sánh số nguyên nhanh hơn so sánh chuỗi.
        {EMotionType.None,      Animator.StringToHash("idle")},       // Hash của state 'idle'
        {EMotionType.Shy,       Animator.StringToHash("isShy")},      // Hash của trigger 'isShy'
        {EMotionType.Surpries,  Animator.StringToHash("isSurpries")}, // Hash của trigger 'isSurpries'
    };
    private bool _isEmotionPlaying = false;
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

        if (motionType == EMotionType.None)
        {
            Debug.Log("[RaiseMotion] motionType == None → Không thực hiện gì.");
            return;
        }
        if (_isEmotionPlaying)
        {
            Debug.Log("Đang trong một motion khác, bỏ qua...");
            return;
        }
        // 2. Lấy Animator từ _meshController. Đây là Animator của nhân vật/đối tượng chính.
        var mainMotionAnimator = _meshController.MainMotionAnimator;
        if (!mainMotionAnimator) return; // Nếu không có Animator thì thoát.

        // 3. Kiểm tra trạng thái hiện tại: Chỉ cho phép kích hoạt motion mới khi đang ở trạng thái 'idle'.
        // Điều này ngăn việc kích hoạt một motion mới khi một motion khác đang chạy (ví dụ: đang "ngạc nhiên" thì không thể "xấu hổ" ngay lập tức).
        _isEmotionPlaying = true;

        //var state = mainMotionAnimator.GetCurrentAnimatorStateInfo(0); // Lấy thông tin state của layer 0.
        // 4. Lấy hash của motion cần kích hoạt từ Dictionary.
        int motionHash = MainMotionParameterHash[motionType];
        
        // 5. Thiết lập giá trị blend.
        // Giới hạn giá trị trong khoảng [-1, 1].
        motionBlendValue = Mathf.Clamp(motionBlendValue, -1f, 1f);
        // // Đặt giá trị cho parameter "RandomValue" trên Animator.
        // mainMotionAnimator.SetFloat(RandomValueParameterHash, motionBlendValue);
        //
        // mainMotionAnimator.SetTrigger(motionHash);
        
        if (motionBlendValue > 0.5f)
        {
            mainMotionAnimator.Play("surpries");
            StartCoroutine(ResetEmotionLock(8.5f));
        }
        else
        {
            mainMotionAnimator.Play("shy");
            StartCoroutine(ResetEmotionLock(4f));
        }
    }
    private IEnumerator ResetEmotionLock(float delay)
    {
        yield return new WaitForSeconds(delay);
        //MoveHandController(CurrentWoolInSequence, true);
        _isEmotionPlaying = false;
        Debug.Log("Motion kết thúc, cho phép trigger tiếp theo.");
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