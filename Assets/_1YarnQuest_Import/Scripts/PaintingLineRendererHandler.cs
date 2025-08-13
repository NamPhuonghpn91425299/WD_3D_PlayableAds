using DG.Tweening;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PaintingLineRendererHandler : MonoBehaviour
{
    private bool initialized = false;
    public RectTransform rawImageRect;
    public LineRenderer lineRenderer;
    public Material LineMaterial;
    private Material instanceLineMaterial;
    private Vector3 targetPosition;
    private Vector3 currentHeadPosition;
    private Vector3 tailPosition;

    public bool SlerpMoving = false;
    public float LineConnectSpeed = 1f;

    void Update()
    {
        if (SlerpMoving)
        {
            if (lineRenderer.positionCount >= 2)
            {
                currentHeadPosition = Vector3.Lerp(currentHeadPosition, targetPosition, Time.deltaTime * LineConnectSpeed);
                lineRenderer.SetPosition(0, tailPosition);
                lineRenderer.SetPosition(1, currentHeadPosition);
            }
        }
    }

    public void ConnectLine(Vector3 origin, Vector3 target, Color color, bool firstCell = false)
    {
        if (!initialized)
        {
            initialized = true;
            lineRenderer.material = new Material(LineMaterial);
            instanceLineMaterial = lineRenderer.material;
        }
        instanceLineMaterial.SetColor("_Color", color);

        if (SlerpMoving)
        {
            if (firstCell)
            {
                currentHeadPosition = origin;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, origin);
                lineRenderer.SetPosition(1, origin);
            }
        }
        else
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, target);
        }

        tailPosition = origin;
        targetPosition = target;
    }

    public void TailFollow(Vector3 pos)
    {
        if (lineRenderer.positionCount <= 0) return;
        lineRenderer.SetPosition(0, pos);
    }

    public void ClearLine()
    {
        lineRenderer.positionCount = 0;
        currentHeadPosition = targetPosition;
    }

    public void ClearLineLinearFollowUp(float duration = 0.5f)
    {
        DOTween.To(() => tailPosition, x => tailPosition = x, targetPosition, duration)
            .OnUpdate(() =>
            {
                if (!SlerpMoving)
                {
                    if (lineRenderer.positionCount < 2) return; 
                    lineRenderer.SetPosition(0, tailPosition);
                    lineRenderer.SetPosition(1, targetPosition);
                }
            })
            .OnComplete(() =>
            {
                ClearLine();
            });
    }

    public bool Available() => lineRenderer.positionCount <= 1;
}
