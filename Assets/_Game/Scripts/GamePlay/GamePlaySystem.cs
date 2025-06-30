using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Quản lý toàn bộ logic và luồng chơi game chính.
/// Đây là một Singleton, đảm bảo chỉ có một instance duy nhất trong scene.
/// Chịu trách nhiệm xử lý tương tác người dùng, quản lý trạng thái game (thắng, thua),
/// quản lý các đối tượng trong game như các ô chứa (CubeTarget), hàng đợi (QueueTarget),

/// và điều phối các hiệu ứng animation.
/// </summary>
public partial class GamePlaySystem : Singleton<GamePlaySystem>
{
    #region <====================| Properties |====================>

    [Tooltip("Tham chiếu đến HandController để hiển thị/ẩn hoạt ảnh hướng dẫn.")]
    [SerializeField] private HandController handController;

    /// <summary>
    /// Template URL để mở trang đánh giá ứng dụng trên Google Play Store.
    /// </summary>
    private const string AndroidRatingURI = "http://play.google.com/store/apps/details?id={0}";
    private string url; // URL cuối cùng sau khi thay thế package name.

    [Tooltip("Số lượng khối len cần thu thập để kích hoạt sự kiện đặc biệt (ví dụ: đi đến store).")]
    [SerializeField] private int cubeCountClaimed = 15;

    [Tooltip("Panel UI hiển thị khi kết thúc game (thắng/thua).")]
    [SerializeField] private GameObject endGamePanel;

    /// <summary>
    /// Tham chiếu đến CameraController để điều khiển các hành vi của camera.
    /// </summary>
    public CameraController CameraController;

    /// <summary>
    /// Tham chiếu đến hệ thống quản lý các ô trong hàng đợi (queue).
    /// </summary>
    public BoxChainReaction3D BoxChainReaction3D;

    /// <summary>
    /// Danh sách các ô chứa len chính đang hoạt động trên màn hình.
    /// </summary>
    public List<CubeTargetControl> CurrentCubeTargets = new();

    /// <summary>
    /// Danh sách các ô trong hàng đợi (overflow) đang hoạt động.
    /// </summary>
    public List<QueueTargetControl> CurrentQueueTargets = new();

    /// <summary>
    /// Prefab cho đối tượng sợi len (kết nối từ cuộn len đến ô chứa).
    /// </summary>
    public GameObject YarnWoolPrefab;
    
    /// <summary>
    /// Prefab cho đối tượng cuộn len (được đặt vào trong các ô).
    /// </summary>
    public GameObject RollWoolPrefab;


    // Tham chiếu đến controller quản lý mesh của level (nơi người chơi click để lấy len).
    private GamePlayMeshController _meshController;

    // Mảng lưu trữ màu sắc của các ô chứa (CubeTarget) hiện tại.
    private Color[] _colorTargets = new Color[]
                                    {
                                        Color.black,
                                        Color.black,
                                    };

    // Danh sách các màu mục tiêu có thể được chọn để sinh ra trong CubeTarget mới.
    private List<Color> _colorTargetList = new List<Color>();

    // Pool chứa các booster "Broom" (chổi), không được sử dụng trong code hiện tại.
    private List<RollWoolAnimation> _broomBoosterPool = new List<RollWoolAnimation>();

    /// <summary>
    /// Đếm số lần người chơi đã click để lấy len thành công.
    /// </summary>
    private int _currentColorClickedCount;

    /// <summary>
    /// Đếm tổng số khối len đã được thu thập (mỗi ô chứa đầy tương đương 3 khối).
    /// </summary>
    private int _currentColorCollected;

    [Tooltip("Số lượng ô chứa (CubeTarget) mặc định khi bắt đầu level.")]
    [SerializeField] private int _cubeTargetCountDefault = 4;

    /// <summary>
    /// Đếm số lượng cuộn len hiện có trong hàng đợi (Queue).
    /// </summary>
    private int _queueCount;

    [Tooltip("Prefab của level hiện tại, chứa GamePlayMeshController và các mesh len.")]
    [SerializeField] private GameObject _levelPrefab;
    /// <summary>
    /// Lấy tổng số màu sắc có trong level từ MeshController.
    /// </summary>
    public int TotalColor => _meshController.TotalColor;

    // Cờ đánh dấu xem có đang chạy animation của booster Rainbow không.
    private bool _isPLayAnimUsingRainBow;

    // Hằng số giới hạn số ô chứa tối đa.
    private readonly int maxCubeTarget = 2;
    private Vector3 _scaleDefaultRollWool;


    /// <summary>
    /// Tổng số ô chứa (CubeTarget) đang được kích hoạt và hiển thị trên màn hình.
    /// </summary>
    [HideInInspector] public int TotalCubeActive;
    
    /// <summary>
    /// Đếm số ô chứa (CubeTarget) đã sẵn sàng nhận len (không bị khóa).
    /// </summary>
    [HideInInspector] public int CubeReadyCount;

    
    /// <summary>
    /// Lấy số lượng cuộn len hiện tại trong hàng đợi.
    /// </summary>
    public int QueueCount => _queueCount;

    /// <summary>
    /// Sự kiện (Action) để thông báo cho các hệ thống khác (ví dụ: UI) về việc khóa/mở khóa booster Rainbow.
    /// `true` để khóa, `false` để mở.
    /// </summary>
    public Action<bool> LockRainBowBooster;

    // Tham chiếu đến các file âm thanh.
    public AudioClip woolXoayClip;
    public AudioClip wool1Clip;
    public AudioClip loseSound;

    private int _currentOpenCubeTargetCost;
    private static int _replayCount = 0;

    #endregion <=============================================>


    #region UNITY_METHODS

    public override void Awake()
    {
        base.Awake();
#if UNITY_ANDROID || UNITY_EDITOR
        // Xây dựng URL tới Google Play Store dựa trên định danh của ứng dụng.
        url = AndroidRatingURI.Replace("{0}", Application.identifier);
#endif
        IsGoToStore = false;
    }

    private void Start()
    {
        LoadLevel();
    }

    /// <summary>
    /// Reset lại trạng thái của game khi bắt đầu lại từ đầu.
    /// </summary>
    public void Reset()
    {
        _replayCount = 0;
        CameraController.ResetCamearState();
    }

    #endregion

    #region MAIN_METHODS

    /// <summary>
    /// Kích hoạt hoặc vô hiệu hóa hoạt ảnh hướng dẫn của bàn tay.
    /// </summary>
    /// <param name="isActive">`true` để kích hoạt, `false` để tắt.</param>
    public void ActiveHandController(bool isActive)
    {
        handController.SetActiveAnim(isActive);
    }
    
    /// <summary>
    /// Đếm tổng số lần người chơi đã click vào mesh len.
    /// </summary>
    public int MeshCountClick { get; private set; } = 0;

    /// <summary>
    /// Chuyển người chơi đến trang của game trên store.
    /// Sử dụng API của Luna Playable.
    /// </summary>
    public void GoToStore()
    {
        Luna.Unity.Playable.InstallFullGame();
        IsGoToStore = true;
    }

    /// <summary>
    /// Cờ đánh dấu người chơi đã được chuyển đến store hay chưa.
    /// </summary>
    public bool IsGoToStore;

    /// <summary>
    /// Xử lý logic khi người chơi click vào một mesh len.
    /// </summary>
    /// <param name="startPoint">Vị trí bắt đầu của sợi len (trên mesh).</param>
    /// <param name="spiralPath">Đường đi của sợi len khi được kéo ra.</param>
    /// <param name="colorClick">Màu của mesh len đã được click.</param>
    /// <returns>Trả về `true` nếu click hợp lệ và len được đặt vào một ô, ngược lại `false`.</returns>
    public bool OnClickMesh(Transform startPoint, List<Vector3> spiralPath, Color colorClick)
    {
        MeshCountClick++;
        if (woolXoayClip != null) SoundManager.Instance.PlayOneShot(woolXoayClip, 1);

        // 1. Ưu tiên kiểm tra các ô chứa chính (CubeTarget)
        for (int i = 0; i < CurrentCubeTargets.Count; i++)
        {
            if (!CurrentCubeTargets[i].IsActive && !CurrentCubeTargets[i].gameObject.activeSelf) continue; // Bỏ qua nếu ô không hoạt động
            if (CurrentCubeTargets[i].CheckColor(colorClick))
            {
                if (!CurrentCubeTargets[i].IsReady) continue; // Bỏ qua nếu ô chưa sẵn sàng
                
                // Thêm một cuộn len vào ô
                CurrentCubeTargets[i].AddChild(i, out var headtrans);
                if (headtrans == null) break;
                
                // Tạo và thiết lập animation cho cuộn len (RollWool) và sợi len (YarnWool)
                var rollWool = Instantiate(RollWoolPrefab);
                rollWool.GetComponent<RollWoolAnimation>()
                   .ResetMesh()
                   .SetParent(headtrans)
                   .SetColor(colorClick)
                   .PlayAnim(RollWoolAnimationExtensions.ParentType.CubeTarget);

                ChoseYarnWool(rollWool.transform, startPoint, spiralPath, colorClick);
                _currentColorClickedCount++;
                return true; // Click thành công
            }
        }

        // 2. Nếu không có ô chứa chính nào phù hợp, kiểm tra hàng đợi (QueueTarget)
        for (int i = 0; i < CurrentQueueTargets.Count; i++)
        {
            if (CurrentQueueTargets[i].AddChild(colorClick))
            {
                // Tạo và thiết lập animation tương tự cho hàng đợi
                var rollWool = Instantiate(RollWoolPrefab);
                rollWool.GetComponent<RollWoolAnimation>()
                   .ResetMesh()
                   .SetParent(CurrentQueueTargets[i].transform)
                   .SetColor(colorClick)
                   .PlayAnim(RollWoolAnimationExtensions.ParentType.CubeQueue);

                ChoseYarnWool(rollWool.transform, startPoint, spiralPath, colorClick);
                _currentColorClickedCount++;
                _queueCount++;

                // Kiểm tra điều kiện thua game
                if (_queueCount >= CurrentQueueTargets.Count && CubeReadyCount == TotalCubeActive)
                {
                    StartCoroutine(OnEndGameAction(false)); // Thua
                    Luna.Unity.LifeCycle.GameEnded();
                }
                return true; // Click thành công
            }
        }
        return false; // Click không hợp lệ (không có chỗ chứa)
    }
    
    /// <summary>
    /// Gửi dữ liệu tracking khi kết thúc game.
    /// </summary>
    /// <param name="isQuit">Người chơi có thoát game không.</param>
    /// <param name="isWin">Người chơi thắng hay thua.</param>
    public void TrackingEndGame(bool isQuit, bool isWin)
    {
        if (isWin) PlayerPrefs.SetInt(REPLAY_COUNT, 0);
        var state = isWin ? "win" : "lose";
        var totalColor = Math.Max(1, TotalColor);
        var timePlay = (int)(DateTime.Now - _timeStart).TotalSeconds;
        // Logic gửi tracking (đã bị comment out trong code gốc)
    }

    private bool _isUseBroomBooster;
    private Color _nextColor;
    private bool _hasCube;
    
    /// <summary>
    /// Kiểm tra xem có còn khối len nào có thể được tạo ra từ mesh không.
    /// </summary>
    public bool HasCube => _hasCube;

    /// <summary>
    /// Sinh ra một màu mới cho một ô chứa (CubeTarget) đã trống.
    /// </summary>
    /// <param name="indexCube">Index của CubeTarget cần sinh màu mới.</param>
    public void GenNewCube(int indexCube)
    {
        if (indexCube == -1) return;
        
        // Tính toán độ ưu tiên của các màu còn lại
        _meshController.ColorPriorityCalculator();

        if (_meshController.CubeCount.Count > 1)
        {
            // Lấy danh sách màu có thể chọn, loại trừ các màu đã có trên các CubeTarget khác
            _colorTargetList = RemoveColorInList(_meshController._colorPriority, _colorTargets);
            _nextColor = GetColorByPriority(0); // Chọn màu có độ ưu tiên cao nhất
        }
        else
        {
            _nextColor = _meshController.CubeCount.FirstOrDefault().Key;
        }

        _hasCube = _meshController.CubeCount.TryGetValue(_nextColor, out var cubeCount);

        if (_hasCube)
        {
            // Giảm số lượng màu đó trong kho
            _meshController.CubeCount[_nextColor]--;
            if (_meshController.CubeCount[_nextColor] == 0)
            {
                _meshController.CubeCount.Remove(_nextColor);
            }
        }
        
        // Gán màu mới cho CubeTarget
        CurrentCubeTargets[indexCube].SetColor(_nextColor);
        _colorTargets[indexCube] = _nextColor;
        CheckLockRainBowBooster(); // Kiểm tra xem có nên khóa booster Rainbow không

        // Nếu hết sạch màu trong kho, khóa chức năng mở thêm ô chứa
        if (_meshController.CubeCount.Count == 0)
            LockOpenCube();
    }


    /// <summary>
    /// Xử lý khi một CubeTarget bị tắt (ví dụ: bị khóa).
    /// </summary>
    /// <param name="indexCube">Index của CubeTarget.</param>
    /// <param name="active">Trạng thái mới (true: bật, false: tắt).</param>
    public void CheckTurnOffCube(int indexCube, bool active)
    {
        if (!active)
        {
            CurrentCubeTargets[indexCube].gameObject.SetActive(false);
            TotalCubeActive--;
            CubeReadyCount--;
            SmoothRepositioner(); // Sắp xếp lại vị trí các ô còn lại cho đẹp
            Debug.Log("CubeReadyCount: " + CubeReadyCount + " TotalCubeActive: " + TotalCubeActive);
        }
    }
    
    /// <summary>
    /// Được gọi khi một CubeTarget được lấp đầy.
    /// </summary>
    public void FinishedCollectingCube()
    {
        _currentColorCollected += 3;
        if (_currentColorCollected / 3 == cubeCountClaimed)
        {
            // Khi đạt đủ số lượng, kích hoạt sự kiện đặc biệt
            Luna.Unity.LifeCycle.GameEnded();
            GoToStore();
            Debug.Log(" CurrentColorCollected: " + _currentColorCollected + " End Game with cubeCountClaimed: " + cubeCountClaimed);
        }
    }

    /// <summary>
    /// Sử dụng một cuộn len từ hàng đợi (Queue) để lấp vào một ô chứa (CubeTarget).
    /// </summary>
    /// <param name="nextColor">Màu cần tìm trong Queue.</param>
    /// <param name="indexCube">Index của CubeTarget sẽ nhận cuộn len.</param>
    public void UseQueueTarget(Color nextColor, int indexCube)
    {
        try
        {
            foreach (QueueTargetControl t in CurrentQueueTargets)
            {
                if (t.CheckCurrentColor(nextColor))
                {
                    if (!t.IsAtive()) continue;

                    var rollWoolChild = t.transform.GetChild(1);
                    if (rollWoolChild != null)
                    {
                        var anim = rollWoolChild.GetComponent<RollWoolAnimation>();
                        anim.SetColor(nextColor);

                        // Lấy vị trí bắt đầu và kết thúc cho animation
                        Vector3 start = t.transform.position;
                        CurrentCubeTargets[indexCube].AddChild(indexCube, out var headTrans);
                        if (headTrans == null) return;
                        
                        // Thực hiện animation di chuyển cuộn len từ Queue lên CubeTarget
                        rollWoolChild.SetParent(headTrans);
                        Vector3 mid = (start + headTrans.position) * 0.5f + Vector3.up * 0.5f;
                        rollWoolChild.DOKill();
                        rollWoolChild.DOPath(new[] { start, mid, headTrans.position }, 0.6f, PathType.CatmullRom)
                           .SetEase(Ease.InOutCubic)
                           .OnComplete(() =>
                           {
                               anim.SnapToHole();
                               if (wool1Clip != null) SoundManager.Instance.PlayOneShot(wool1Clip, 1);
                           });
                    }
                    
                    // Reset lại ô trong Queue và giảm số lượng
                    t.ResetDefault();
                    _queueCount--;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in UseQueueTarget: {e}");
        }
    }

    private bool _isWinGame;

    /// <summary>
    /// Kiểm tra các điều kiện thắng/thua của game.
    /// </summary>
    public void CheckEndGame()
    {
        if (_isPLayAnimUsingRainBow) return;

        // Điều kiện thắng: Đã thu thập hết tất cả các màu
        if (_currentColorCollected == _meshController.TotalColor && TotalCubeActive == CubeReadyCount)
        {
            StartCoroutine(OnEndGameAction(true));
            Luna.Unity.LifeCycle.GameEnded();
        }
        
        // Điều kiện thua: Hàng đợi bị đầy
        if (_queueCount >= CurrentQueueTargets.Count && TotalCubeActive == CubeReadyCount)
        {
            StartCoroutine(OnEndGameAction(false));
            Luna.Unity.LifeCycle.GameEnded();
        }
    }

    /// <summary>
    /// Coroutine xử lý các hành động cuối game (hiển thị UI, phát âm thanh).
    /// </summary>
    /// <param name="isWin">`true` nếu thắng, `false` nếu thua.</param>
    private IEnumerator OnEndGameAction(bool isWin)
    {
        if (_isUseBroomBooster) yield break;
        if (!isWin)
            yield return new WaitForSeconds(1.2f); // Chờ một chút trước khi hiện panel thua
        
        if (isWin)
        {
            if (!_isWinGame)
            {
                _isWinGame = true;
                TrackingEndGame(false, true);
                endGamePanel.SetActive(true);
            }
        }
        else
        {
            if (loseSound != null) SoundManager.Instance.PlayOneShot(loseSound, 1f);
            //endGamePanel.SetActive(true);
        }
    }

    /// <summary>
    /// Kiểm tra và quyết định có nên khóa booster Rainbow hay không.
    /// Booster này thường được khóa khi không có cặp màu nào giống nhau trên các mesh.
    /// </summary>
    private void CheckLockRainBowBooster()
    {
        var onLockRainBowBooster = true;
        foreach (var cube in _meshController.CubeCount)
        {
            if (cube.Value > 1) onLockRainBowBooster = false;
        }
        LockRainBowBooster?.Invoke(onLockRainBowBooster);
        
    }
    
    /// <summary>
    /// Mở khóa các ô chứa (CubeTarget) dựa trên số lượng mới.
    /// </summary>
    /// <param name="newCubeCount">Tổng số ô chứa sẽ được hoạt động.</param>
    private void UnLockCubeTarget(int newCubeCount)
    {
        TotalCubeActive = 0;
        CubeReadyCount = 0;
        for (int i = 0; i < CurrentCubeTargets.Count; i++)
        {
            CurrentCubeTargets[i].gameObject.SetActive(true);
            // Kích hoạt ô nếu index của nó nhỏ hơn số lượng mới
            CurrentCubeTargets[i].SetActiveCubeTarget(i, i + 1 <= newCubeCount);
            // Hiện nút "mở khóa" cho các ô vượt quá số lượng mới
            CurrentCubeTargets[i].ActiveOpenCube(i + 1 > newCubeCount);
            Debug.Log("UnLockCubeTarget" + i );

        }
        _cubeTargetCountDefault = newCubeCount;
        SmoothRepositioner(); // Sắp xếp lại vị trí các ô
    }

    /// <summary>
    /// Khóa chức năng mở thêm ô chứa mới. Thường được gọi khi hết màu để sinh.
    /// </summary>
    public void LockOpenCube()
    {
        foreach (var cube in CurrentCubeTargets)
        {
            if (cube.IsActive) continue;
            cube.ActiveOpenCube(false);
            Debug.Log("LockOpenCube" );
        }
    }

    /// <summary>
    /// Tạo ra đối tượng sợi len (YarnWool) và chạy animation của nó.
    /// </summary>
    /// <param name="head">Transform của cuộn len (đầu sợi len).</param>
    /// <param name="tail">Transform của điểm bắt đầu trên mesh (đuôi sợi len).</param>
    /// <param name="spiralPath">Đường đi cho animation của sợi len.</param>
    /// <param name="color">Màu của sợi len.</param>
    private void ChoseYarnWool(Transform head, Transform tail, List<Vector3> spiralPath, Color color)
    {
        var yarnWool = Instantiate(YarnWoolPrefab);
        var YarnWoolScript = yarnWool.GetComponent<YarnWoolAnimation>();
        YarnWoolScript.SetColor(color);
        YarnWoolScript.SetPoints(spiralPath);
        YarnWoolScript.SetParent(head, tail);
    }

    #endregion

    #region HELPER_METHODS
    
    /// <summary>
    /// Lọc và trả về danh sách các màu có thể được sinh ra.
    /// </summary>
    /// <param name="listRemove">Dictionary chứa các màu và độ ưu tiên của chúng.</param>
    /// <param name="listReference">Mảng các màu hiện đang có trên các CubeTarget.</param>
    /// <param name="alowSameColor">Có cho phép sinh ra màu đã có trên CubeTarget khác không.</param>
    /// <returns>Danh sách các màu hợp lệ.</returns>
    private List<Color> RemoveColorInList(Dictionary<Color, float> listRemove, Color[] listReference, bool alowSameColor = false)
    {
        // ... (Logic phức tạp để chọn màu, đảm bảo không trùng lặp nếu cần)
        List<Color> result       = new List<Color>();
        var         copyListMove = listRemove;
        if (!alowSameColor)
        {
            for (int i = 0; i < listReference.Length && i < CurrentCubeTargets.Count; i++)
            {
                if (listReference[i] == Color.black) continue;
                copyListMove.Remove(listReference[i]);
            }
        }
        if (copyListMove.Count == 0 && _meshController.CubeCount.Count >= 1 && _meshController.CubeCount.First()
           .Value >= 1)
        {
            foreach (var color in listRemove)
            {
                if (color.Value == 0) continue;
                result.Add(color.Key);
            }
        }
        else
        {
            foreach (var color in listRemove)
            {
                result.Add(color.Key);
            }
        }


        return result;
    }

    /// <summary>
    /// Lấy một màu dựa trên độ ưu tiên đã được tính toán.
    /// </summary>
    private Color GetColorByPriority(float priority)
    {
        // ... (Logic chọn màu dựa trên một ngưỡng ưu tiên)
        if (_colorTargetList.Count == 1)
            return _meshController?._colorPriority?.FirstOrDefault()
               .Key ?? Color.black;
        Color result = Color.black;

        try
        {
            foreach (var colorPriority in _meshController._colorPriority)
            {
                result = colorPriority.Key;
                if (priority <= colorPriority.Value) break;
            }
        } catch (Exception e)
        {
            Debug.LogError($"Error in GetColorByPriority: {e}");
        }

        return result;
    }


    /// <summary>
    /// Tải và thiết lập một level mới. Đây là điểm khởi đầu của một màn chơi.
    /// </summary>
    private void LoadLevel()
    {
        Luna.Unity.LifeCycle.GameUpdated();
        _meshController = _levelPrefab.GetComponent<GamePlayMeshController>();
        CameraController.ResetCamearState();
        UnLockCubeTarget(_cubeTargetCountDefault); // Mở số ô chứa mặc định
        ResetCubeTarget();
        ResetQueueTarget();
        CameraController.SpawnPoint.rotation = Quaternion.identity;
        _isWinGame = false;
        _meshController?.LoadcolorForMesh(); // Tải màu cho các mesh len
        CameraController.Setup(_levelPrefab);
        CameraController.Instance?.BlockRotate(false);
        
        // Reset các biến đếm
        _currentColorClickedCount = 0;
        _currentColorCollected = 0;
        _queueCount = 0;
        MeshCountClick = 0;

        // Sinh màu ban đầu cho các ô chứa
        for (int i = 0; i < _cubeTargetCountDefault; i++)
        {
            GenNewCube(i);
            CurrentCubeTargets[i].ChangeColor();
        }
        CameraController.HoldClickTime = 0;
        CameraController.StartIntro(); // Bắt đầu animation intro của camera
    }

    private static string REPLAY_COUNT = "REPLAY_COUNT";
    public static string Play_ID { get; private set; }
    private static DateTime _timeStart;

    /// <summary>
    /// Reset tất cả các ô chứa (CubeTarget) về trạng thái mặc định.
    /// </summary>
    private void ResetCubeTarget()
    {
        foreach (var cube in CurrentCubeTargets)
        {
            cube.SetDefault();
        }
        _isPLayAnimUsingRainBow = false;
    }

    /// <summary>
    /// Reset hàng đợi (Queue) về trạng thái mặc định.
    /// </summary>
    private void ResetQueueTarget()
    {
        _queueCount = 0;
        if (CurrentQueueTargets.Count != BoxChainReaction3D.InitialBoxCount)
        {
            BoxChainReaction3D.InitializeBoxes();
            return;
        }
        foreach (var queue in CurrentQueueTargets)
        {
            queue.ResetDefault();
        }
    }

    private List<CubeTargetControl> _activeObjects = new();
    private Vector3 offset = new(1.2f, 0, 0);
    
    /// <summary>
    /// Chạy animation sắp xếp lại vị trí các CubeTarget để chúng luôn ở giữa màn hình.
    /// </summary>
    private void SmoothRepositioner()
    {
        CacheActiveObjects();
        Reposition();
    }
    
    /// <summary>
    /// Lưu lại danh sách các CubeTarget đang hoạt động vào cache.
    /// </summary>
    private void CacheActiveObjects()
    {
        _activeObjects.Clear();
        foreach (var obj in CurrentCubeTargets)
        {
            if (obj != null && obj.gameObject.activeSelf)
                _activeObjects.Add(obj);
        }
    }

    [SerializeField] float spacingCubeTarget = 1f;

    /// <summary>
    /// Tính toán và thực hiện animation di chuyển các CubeTarget đến vị trí mới.
    /// </summary>
    private void Reposition()
    {
        int activeCount = _activeObjects.Count;
        if (activeCount == 0) return;

        float totalWidth = (activeCount - 1) * spacingCubeTarget;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < activeCount; i++)
        {
            var obj = _activeObjects[i];
            Vector3 currentPos = obj.transform.position;
            Vector3 targetPos = new Vector3(startX + i * spacingCubeTarget, currentPos.y, currentPos.z);
            
            // Dùng DOTween để di chuyển mượt mà
            obj.transform
               .DOMoveX(targetPos.x, 0.3f)
               .SetEase(Ease.OutQuad)
               .OnComplete(() =>
               {
                   obj.BakeAnimPosition(); // Lưu lại vị trí mới sau khi animation kết thúc
               });
        }
    }

    #endregion
}