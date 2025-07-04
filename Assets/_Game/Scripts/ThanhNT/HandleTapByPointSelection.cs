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

    public WoolPointData[] woolPoints;

    public HandController handScript;
    public Vector3 offset = new Vector3(0.3f, -0.3f, -1.4f);

    private void Start()
    {
        _mainCamera = CameraContainer.Instance.MainCamera;
        InputInteractable.OnTap += HandleTap;
        CameraController.Instance.OnEndGameIntro += () =>
        {
            if (handScript != null)
            {
                woolPoints[0].targetTransform.gameObject.SetActive(true);
            }
        };
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
        if (woolPoints.Length == 0) return;

        Ray ray = _mainCamera.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        WoolControl wool = TrySelectWool(ray);
        if (wool != null)
        {
            wool.WoolRotation();
            GamePlaySystem.Instance.RaiseMotion(EMotionType.Shy, Random.value);
        }
    }

    private WoolControl TrySelectWool(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            Debug.Log($"Hit: {hit.collider.gameObject.name}");
            currrentIndex = selectedIndex;

            if (hit.collider.gameObject.name == currrentIndex.ToString())
            {
                selectedIndex++;
                woolPoints[currrentIndex].targetTransform.gameObject.SetActive(false);

                if (handScript.transform.childCount > 0)
                    handScript.transform.GetChild(0).gameObject.SetActive(false);

                StartCoroutine(IEWaitAndActivateNext());
                Debug.Log($"Selected Index: {selectedIndex}");

                return woolPoints[currrentIndex].woolControl;
            }
        }

        return null;
    }

    private IEnumerator IEWaitAndActivateNext()
    {
        if (selectedIndex >= woolPoints.Length -1)
        {
            yield return new WaitForSeconds(2f);
            GamePlaySystem.Instance.WinGame();
            yield break;
        }
        yield return new WaitForSeconds(0.15f);
        ActivateNextWool();
    }

    public void ActivateNextWool()
    {
        if (!handScript.gameObject.activeSelf && selectedIndex < woolPoints.Length)
        {
            var point = woolPoints[selectedIndex];

            point.targetTransform.gameObject.SetActive(true);
            handScript.gameObject.SetActive(true);

            if (handScript.transform.childCount > 0)
                handScript.transform.GetChild(0).gameObject.SetActive(true);

            handScript.StopAllCoroutines();
            handScript.StartCoroutine(handScript.PlayAnim());

            handScript.transform.position = point.targetTransform.position + offset;
        }
    }

    private void UpdateHandPosition()
    {
        if (selectedIndex < 0 || selectedIndex >= woolPoints.Length) return;

        var point = woolPoints[selectedIndex];
        Vector3 newPos = point.targetTransform.position;

        if (point.referenceTransform != null)
        {
            newPos.x = point.referenceTransform.position.x;
            newPos.y = point.referenceTransform.position.y;
        }

        point.targetTransform.position = newPos;
        handScript.transform.position = newPos + offset;
    }
}
[Serializable]
public struct WoolPointData
{
    public Transform targetTransform;       // Đối tượng tương tác (Wool)
    public WoolControl woolControl;         // Script điều khiển logic
    public Transform referenceTransform;    // Đối tượng gốc để cập nhật vị trí tay (ví dụ: Elbow/Shoulder)
}
