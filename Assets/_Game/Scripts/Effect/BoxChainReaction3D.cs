using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxChainReaction3D : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Parent container for all box objects")]
    private Transform boxContainer;

    [SerializeField]
    [Tooltip("Prefab used to create boxes (must be a 3D object)")]
    private GameObject boxPrefab;

    [Tooltip("Number of boxes to create at start")]
    public int InitialBoxCount = 4;

    [Header("Animation Settings")]
    [SerializeField]
    [Tooltip("Initial space between boxes (will decrease as boxes are added)")]
    private float initialSpacing = 1.5f;

    [SerializeField]
    [Tooltip("Width of each box")]
    private float boxWidth = 1f;

    [SerializeField]
    [Tooltip("Duration of fly-in animation for new box")]
    private float flyInDuration = 0.5f;

    [SerializeField]
    [Tooltip("Duration of collision/impact animation")]
    private float collisionDuration = 0.2f;

    [SerializeField]
    [Tooltip("Duration of boxes repositioning animation")]
    private float repositionDuration = 0.3f;

    [SerializeField]
    [Tooltip("How far a box moves when hit during collision")]
    private float collisionOffset = 0.5f;

    [SerializeField]
    [Tooltip("Initial position for new boxes (off-screen)")]
    private Vector3 flyInStartPosition = new Vector3(10f, 0f, 0f);

    [Header("Squash & Stretch")]
    [SerializeField]
    [Tooltip("How much to stretch horizontally during fly-in")]
    private float flyInStretch = 1.3f;

    [SerializeField]
    [Tooltip("How much to squash when landing after fly-in")]
    private float landingSquash = 0.7f;

    [SerializeField]
    [Tooltip("How much to squash horizontally when hit")]
    private float collisionSquash = 0.6f;

    [SerializeField]
    [Tooltip("How much to stretch vertically when hit")]
    private float collisionStretch = 1.2f;

    private List<Transform> boxes = new List<Transform>();
    private float initialTotalWidth;

    private Vector3 originalScale;

    //public SoundSO soundData;
    public AudioClip boosterWhoosh1Clip;
    public AudioClip boosterImpact1Clip;
    private void Start()
    {
        // Calculate the initial total width we want to maintain
        initialTotalWidth = (InitialBoxCount * boxWidth) + ((InitialBoxCount - 1) * initialSpacing);
        InitializeBoxes();
    }

    private float CalculateDynamicSpacing(int boxCount)
    {
        if (boxCount <= 1) return initialSpacing;
        
        // Calculate spacing needed to maintain initialTotalWidth
        float availableSpace = initialTotalWidth - (boxCount * boxWidth);
        var   minSpacing     = Mathf.Max(0.001f, availableSpace / (boxCount - 1));
        return minSpacing;
    }

    public void InitializeBoxes()
    {
        // Clear existing boxes
        foreach (var box in boxes)
        {
            if (box != null)
                Destroy(box.gameObject);
        }

        boxes.Clear();
        GamePlaySystem.Instance.CurrentQueueTargets.Clear();
        // Create initial boxes
        for (int i = 0; i < InitialBoxCount; i++)
        {
            CreateBox();
        }

        RepositionAllBoxes(0);
    }

    public Transform CreateBox()
    {
        var box = Instantiate(boxPrefab, boxContainer).transform;
        originalScale = box.localScale;
        boxes.Add(box);
        var queueTarget = box.GetComponent<QueueTargetControl>();
        GamePlaySystem.Instance.CurrentQueueTargets.Add(queueTarget);
        return box;
    }

    public IEnumerator AddBoxWithAnimationCoroutine()
    {
        // Create a new box that will fly in
        var newBox = CreateBox();
        if (boosterWhoosh1Clip != null) SoundManager.Instance.PlayOneShot(boosterWhoosh1Clip, 1 );
        // Calculate positions with dynamic spacing
        CalculatePositions(out List<Vector3> oldPositions, out List<Vector3> newPositions);

        // Set initial position for the new box (off-screen to the right)
        newBox.position = flyInStartPosition + new Vector3(0, 0, -0.25f);

        // Apply initial stretch for fly-in
        newBox.localScale = new Vector3(flyInStretch * originalScale.x, 1/(flyInStretch * originalScale.y), originalScale.z);

        // Step 1: Fly in animation
        Sequence flyInSequence = DOTween.Sequence();
        flyInSequence.Append(newBox.DOMove(oldPositions[^1], flyInDuration)
                             .SetEase(Ease.OutQuint));

        // Squash on landing
        flyInSequence.Join(newBox.DOScale(new Vector3(landingSquash * originalScale.x, 1/ landingSquash * originalScale.y, landingSquash * originalScale.z), flyInDuration)
                           .SetEase(Ease.InOutQuint));

        // Return to normal scale at end
        flyInSequence.Append(newBox.DOScale(originalScale, 0.15f).SetEase(Ease.OutBack));
        
        yield return flyInSequence.WaitForCompletion();

        // Start the chain reaction
        yield return StartCoroutine(StartChainReactionCoroutine(oldPositions, newPositions));
    }

    private void CalculatePositions(out List<Vector3> oldPositions, out List<Vector3> newPositions)
    {
        oldPositions = new List<Vector3>();
        newPositions = new List<Vector3>();

        int oldCount = boxes.Count - 1; // Exclude the newly added box
        int newCount = boxes.Count;

        // Get the container's position as reference point
        Vector3 centerPoint = boxContainer.position;
        
        // Calculate dynamic spacing values
        float oldSpacing = CalculateDynamicSpacing(oldCount);
        float newSpacing = CalculateDynamicSpacing(newCount);

        // Calculate old positions (before new box)
        float oldTotalWidth = (oldCount * boxWidth) + ((oldCount - 1) * oldSpacing);
        float oldStartX = centerPoint.x - (oldTotalWidth / 2);

        for (int i = 0; i < oldCount; i++)
        {
            float xPos = oldStartX + (i * (boxWidth + oldSpacing)) + (boxWidth / 2);
            oldPositions.Add(new Vector3(xPos, centerPoint.y, centerPoint.z));
        }

        // If there are existing boxes, calculate the position for the new box
        if (oldCount > 0)
        {
            oldPositions.Add(oldPositions[oldPositions.Count - 1] +
                            new Vector3(boxWidth + oldSpacing, 0, 0));
        }
        else
        {
            // If this is the first box, place it at the center
            oldPositions.Add(centerPoint);
        }

        // Calculate new positions (after adding new box) with new spacing
        float newTotalWidth = (newCount * boxWidth) + ((newCount - 1) * newSpacing);
        float newStartX = centerPoint.x - (newTotalWidth / 2);

        for (int i = 0; i < newCount; i++)
        {
            float xPos = newStartX + (i * (boxWidth + newSpacing)) + (boxWidth / 2);
            newPositions.Add(new Vector3(xPos, centerPoint.y, centerPoint.z));
        }
    }

    private void RepositionAllBoxes(float duration)
    {
        int count = boxes.Count;
        if (count == 0) return;

        // Get the container's position as reference point
        Vector3 centerPoint = boxContainer.position;
        
        // Calculate dynamic spacing for current box count
        float spacing = CalculateDynamicSpacing(count);

        float totalWidth = (count * boxWidth) + ((count - 1) * spacing);
        float startX = centerPoint.x - (totalWidth / 2);

        for (int i = 0; i < count; i++)
        {
            float xPos = startX + (i * (boxWidth + spacing)) + (boxWidth / 2);

            if (duration <= 0)
            {
                boxes[i].position   = new Vector3(xPos, centerPoint.y, centerPoint.z);
                boxes[i].localScale = originalScale;
            }
            else
            {
                boxes[i].DOMove(new Vector3(xPos, centerPoint.y, centerPoint.z), duration)
                       .SetEase(Ease.OutBack);
                boxes[i].DOScale(originalScale, duration)
                       .SetEase(Ease.OutBack);
            }
        }
    }

    private IEnumerator StartChainReactionCoroutine(List<Vector3> oldPositions, List<Vector3> newPositions)
    {
        int lastIndex = boxes.Count - 1;
        var boxToMove = boxes[lastIndex];
        
        // Handle the new box settling into position
        boxToMove.position = boxToMove.position + new Vector3(0, 0, -0.25f);
        Vector3 targetPosition = newPositions[lastIndex] + new Vector3(0, 0, -0.25f);
        Tween settleTween = boxToMove.DOMove(targetPosition, repositionDuration)
                                           .SetEase(Ease.OutBack);
        yield return settleTween.WaitForCompletion();
        
        // Start chain reaction from right to left
        for (int i = lastIndex - 1; i >= 0; i--)
        {
            if (boosterImpact1Clip != null) SoundManager.Instance.PlayOneShot(boosterImpact1Clip,1 );
            // Box gets hit - squash horizontally and stretch vertically
            Vector3 collisionPos = oldPositions[i] + new Vector3(-collisionOffset, 0, -0.25f);
            Sequence hitSequence = DOTween.Sequence();

            // Move and squash simultaneously
            hitSequence.Append(boxes[i].DOMove(collisionPos, collisionDuration)
                              .SetEase(Ease.OutQuint));
            hitSequence.Join(boxes[i].DOScale(new Vector3(collisionSquash * originalScale.x , collisionStretch * originalScale.y, collisionSquash * originalScale.z),
                                          collisionDuration/2)
                              .SetEase(Ease.OutQuint));

            yield return hitSequence.WaitForCompletion();

            // Box returns to its new position with a slight bounce effect
            Sequence returnSequence = DOTween.Sequence();
            returnSequence.Append(boxes[i].DOMove(newPositions[i] + new Vector3(0, 0, -0.25f), repositionDuration)
                                 .SetEase(Ease.OutBack));
            returnSequence.Join(boxes[i].DOScale(originalScale, repositionDuration)
                                .SetEase(Ease.OutElastic, 0.5f, 0.3f));

            yield return returnSequence.WaitForCompletion();
        }

        foreach (var box in boxes)
        {
            // Reset the scale of all boxes
            box.position = box.position + new Vector3(0, 0, 0.25f);
        }
    }

    [ContextMenu("Trigger Animation")]
    public void TriggerAnimation()
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);
        StartCoroutine(AddBoxWithAnimationCoroutine());
    }
}