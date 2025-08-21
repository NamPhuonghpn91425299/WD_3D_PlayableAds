using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandleTapByPointSelection : MonoBehaviour
{
    public int selectedIndex = -1;
    public int currrentIndex = 0;
    public Interactable InputInteractable;
    public LayerMask layerMask;

    private Camera _mainCamera;
    private RaycastHit[] _woolHits = new RaycastHit[10]; // Sử dụng mảng để SphereCast

    public WoolPointData[] woolPoints;

    public HandController handScript;

    [SerializeField] private GameObject AudioSourceBG;
  
    [SerializeField] private float _tapRadius = 0.2f;
    public GameObject UIWIn;

    public AudioClip winSound;

    private void Start()
    {
        _mainCamera = CameraContainer.Instance.MainCamera;
        InputInteractable.OnTap += HandleTap;
    }

    private void Update()
    {
        if (selectedIndex != 0)
        {
            UpdateHandPosition();
        }
        
    }

    private void OnDestroy()
    {
        InputInteractable.OnTap -= HandleTap;
    }

    private void OnDisable()
    {
        InputInteractable.OnTap -= HandleTap;
        selectedIndex = -1;
    }

    private void HandleTap(Vector2 screenPos)
    {
        if (!AudioSourceBG.activeSelf)
        {
            AudioSourceBG.SetActive(true);
        }
        if (woolPoints.Length == 0) return;

        Ray ray = _mainCamera.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        WoolControl wool = FindBestWoolNearRay(ray);
        if (wool != null)
        {
            wool.WoolRotation();
            GamePlaySystem.Instance.RaiseMotion(EMotionType.Shy, Random.value);
        }
    }


    private WoolControl FindBestWoolNearRay(Ray ray)
    {
        // Bước 1: Ưu tiên Raycast trực tiếp
        if (Physics.Raycast(ray, out RaycastHit directHit, 100f, layerMask))
        {
            if (directHit.collider.gameObject.TryGetComponent<WoolControl>(out var directWool))
            {
                if (directWool.WoolOrder == selectedIndex)
                {
                    ProcessWoolSelection(directWool);
                     return directWool; // Tìm thấy, trả về ngay lập tức
                }
               
            }
        }

        // Bước 2: Tìm kiếm lân cận bằng SphereCast nếu Raycast trượt
        int hitCount = Physics.SphereCastNonAlloc(ray, _tapRadius, _woolHits, 100f, layerMask);

        if (hitCount > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                if (_woolHits[i].collider.gameObject.TryGetComponent<WoolControl>(out var wool))
                {
                    if (wool.WoolOrder == selectedIndex)
                    {
                        ProcessWoolSelection(wool);
                        return wool; // Trả về đối tượng tìm thấy
                    }
                }
            }
        }

        return null; // Không tìm thấy bất kỳ đối tượng nào
    }

    private void ProcessWoolSelection(WoolControl wool)
    {
        currrentIndex = selectedIndex;
        selectedIndex++;
        
        hasArrivedAtTarget = false;
        StartCoroutine(IEWaitAndActivateNext());
    }



    private IEnumerator IEWaitAndActivateNext()
    {
        if (selectedIndex >= woolPoints.Length )
        {
            yield return new WaitForSeconds(2f);
            SoundManager.Instance.PlayOneShot(winSound, 1);
            GamePlaySystem.Instance.WinGame();

            UIWIn.SetActive(true);
            yield break;
        }
    }



    [SerializeField] private float handMoveSpeed = 5f; // Tốc độ di chuyển của hand
    [SerializeField] private float arrivalThreshold = 0.1f; // Khoảng cách để coi như đã đến
    private bool hasArrivedAtTarget = false;

    private void UpdateHandPosition()
    {
        if (selectedIndex < 0 || selectedIndex >= woolPoints.Length) return;

        var point = woolPoints[selectedIndex];
        if (point.referenceTransform == null) return;

        Vector3 targetPos = point.referenceTransform.position + handScript.Offset;
        Vector3 currentPos = handScript.transform.position;
        
        // Giữ nguyên trục Z của hand hiện tại
        targetPos.z = currentPos.z;
        
        // Tính khoảng cách đến mục tiêu (chỉ tính trên trục X và Y)
        float distanceToTarget = Vector2.Distance(new Vector2(currentPos.x, currentPos.y), new Vector2(targetPos.x, targetPos.y));
        
        // Nếu chưa đến mục tiêu
        if (distanceToTarget > arrivalThreshold)
        {
            // Di chuyển hand về phía mục tiêu (chỉ trên trục X và Y)
            handScript.transform.position = Vector3.MoveTowards(currentPos, targetPos, handMoveSpeed * Time.deltaTime);
            hasArrivedAtTarget = false;
        }
        // Nếu đã đến mục tiêu và chưa play animation
        else if (!hasArrivedAtTarget)
        {
            // Đặt vị trí chính xác (giữ nguyên Z)
            handScript.transform.position = targetPos;
            hasArrivedAtTarget = true;
            
            // Play animation khi đến nơi
            handScript.StopAllCoroutines();
            handScript.StartCoroutine(handScript.PlayAnim());
        }
    }
}
[Serializable]
public struct WoolPointData
{
    public Transform targetTransform;       // Đối tượng tương tác (Wool)
    public WoolControl woolControl;         // Script điều khiển logic
    public Transform referenceTransform;    // Đối tượng gốc để cập nhật vị trí tay (ví dụ: Elbow/Shoulder)
}
