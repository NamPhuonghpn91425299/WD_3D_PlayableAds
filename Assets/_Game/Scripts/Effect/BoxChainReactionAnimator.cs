using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.UI;
        using DG.Tweening;
        
        public class BoxChainReactionAnimator : MonoBehaviour
        {
            [SerializeField] 
            [Tooltip("Parent container for all box objects")]
            private RectTransform boxContainer;
            
            [SerializeField] 
            [Tooltip("Prefab used to create boxes (must have RectTransform)")]
            private RectTransform boxPrefab;
            
            [SerializeField] 
            [Tooltip("Number of boxes to create at start")]
            private int initialBoxCount = 4;
        
            [Header("Animation Settings")] 
            [SerializeField]
            [Tooltip("Horizontal space between boxes")]
            private float spacing = 10f;
        
            [SerializeField] 
            [Tooltip("Width of each box")]
            private float boxWidth = 50f;
            
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
            private float collisionOffset = 15f;
            
            [SerializeField] 
            [Tooltip("Initial position for new boxes (off-screen)")]
            private Vector2 flyInStartPosition = new Vector2(300, 0);
        
            private List<RectTransform> boxes = new List<RectTransform>();
        
            private void Start()
            {
                InitializeBoxes();
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
        
                // Create initial boxes
                for (int i = 0; i < initialBoxCount; i++)
                {
                    CreateBox();
                }
        
                RepositionAllBoxes(0);
            }
        
            public RectTransform CreateBox()
            {
                var box = Instantiate(boxPrefab, boxContainer);
                boxes.Add(box);
                return box;
            }
        
            public IEnumerator AddBoxWithAnimationCoroutine()
            {
                // Create a new box that will fly in
                var newBox = CreateBox();
        
                // Calculate positions
                CalculatePositions(out List<Vector2> oldPositions, out List<Vector2> newPositions);
        
                // Set initial position for the new box (off-screen to the right)
                newBox.anchoredPosition = flyInStartPosition;
        
                // Step 1: Fly in animation
                Tween flyInTween = newBox.DOAnchorPos(oldPositions[oldPositions.Count - 1], flyInDuration)
                                         .SetEase(Ease.OutQuint);
                yield return flyInTween.WaitForCompletion();
        
                // Start the chain reaction
                yield return StartCoroutine(StartChainReactionCoroutine(oldPositions, newPositions));
            }
        
            private void CalculatePositions(out List<Vector2> oldPositions, out List<Vector2> newPositions)
            {
                oldPositions = new List<Vector2>();
                newPositions = new List<Vector2>();
        
                int oldCount = boxes.Count - 1; // Exclude the newly added box
                int newCount = boxes.Count;
        
                // Calculate old positions (before new box)
                float oldTotalWidth = (oldCount * boxWidth) + ((oldCount - 1) * spacing);
                float oldStartX     = -oldTotalWidth / 2;
        
                for (int i = 0; i < oldCount; i++)
                {
                    float xPos = oldStartX + (i * (boxWidth + spacing)) + (boxWidth / 2);
                    oldPositions.Add(new Vector2(xPos, 0));
                }
        
                oldPositions.Add(oldPositions[oldPositions.Count - 1] +
                                 new Vector2(boxWidth            + spacing, 0)); // Position for new box
        
                // Calculate new positions (after adding new box)
                float newTotalWidth = (newCount * boxWidth) + ((newCount - 1) * spacing);
                float newStartX     = -newTotalWidth / 2;
        
                for (int i = 0; i < newCount; i++)
                {
                    float xPos = newStartX + (i * (boxWidth + spacing)) + (boxWidth / 2);
                    newPositions.Add(new Vector2(xPos, 0));
                }
            }
        
            private void RepositionAllBoxes(float duration)
            {
                int   count      = boxes.Count;
                float totalWidth = (count * boxWidth) + ((count - 1) * spacing);
                float startX     = -totalWidth / 2;
        
                for (int i = 0; i < count; i++)
                {
                    float xPos = startX + (i * (boxWidth + spacing)) + (boxWidth / 2);
                    if (duration <= 0)
                        boxes[i].anchoredPosition = new Vector2(xPos, 0);
                    else
                        boxes[i].DOAnchorPos(new Vector2(xPos, 0), duration).SetEase(Ease.OutBack);
                }
            }
        
            private IEnumerator StartChainReactionCoroutine(List<Vector2> oldPositions, List<Vector2> newPositions)
            {
                int lastIndex = boxes.Count - 1;
        
                // Handle the new box settling into position
                Tween settleTween = boxes[lastIndex].DOAnchorPos(newPositions[lastIndex], repositionDuration)
                                                    .SetEase(Ease.OutBack);
                yield return settleTween.WaitForCompletion();
        
                // Start chain reaction from right to left
                for (int i = lastIndex - 1; i >= 0; i--)
                {
                    // Box gets hit and moves left
                    Vector2 collisionPos = oldPositions[i] + new Vector2(-collisionOffset, 0);
                    Tween collisionTween = boxes[i].DOAnchorPos(collisionPos, collisionDuration)
                                                   .SetEase(Ease.OutQuint);
                    yield return collisionTween.WaitForCompletion();
        
                    // Box returns to its new position
                    Tween returnTween = boxes[i].DOAnchorPos(newPositions[i], repositionDuration)
                                                .SetEase(Ease.OutBack);
                    yield return returnTween.WaitForCompletion();
                }
            }
        
            [ContextMenu("Trigger Animation")]
            public void TriggerAnimation()
            {
                StartCoroutine(AddBoxWithAnimationCoroutine());
            }
        }