using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using DG.Tweening;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class HandController : MonoBehaviour
{
    [SerializeField] private List<Sprite>   handSprites = new ();
    [SerializeField] private SpriteRenderer handSpriteRenderer;
    [SerializeField] private Vector3        positionShow = new Vector3(0.2f, -0.2f, -1.5f);
    [SerializeField] private Vector3        positionHide = new Vector3(0.2f, 10, 0);
    [SerializeField] private Vector3        offset = new Vector3(0.3f, + -0.3f, -1.4f);
    [SerializeField] private float delayTime = 0.5f;
    public List<WoolControl> WoolControls;

    private Coroutine _playAnimCoroutine;
    private int _lastIndex = -1;
    private bool _lastIsActive = false;

    private void Start()
    {
        transform.position = positionHide;
    }

    private void OnDisable()
    {
        if (_playAnimCoroutine != null) StopCoroutine(_playAnimCoroutine);
    }
    

    public void SetActiveAnim(bool isActive)
    {
        if (_playAnimCoroutine != null) StopCoroutine(_playAnimCoroutine);
        if (isActive)
        {
            transform.DOMove(positionShow, delayTime).SetEase(Ease.OutQuad);    
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }
        if (gameObject.activeSelf == false) return;
        _playAnimCoroutine = StartCoroutine(PlayAnim());
    }

    public void MoveHandToPosition(int index, bool isActive)
    {
        if (index == _lastIndex && isActive == _lastIsActive)
        {
            Debug.Log($"[HandController] Đã ở index {index} & isActive={isActive}, bỏ qua.", gameObject);
            return;
        }

        _lastIndex = index;
        _lastIsActive = isActive;
        
        if (_playAnimCoroutine != null)
            StopCoroutine(_playAnimCoroutine);
        
        bool foundTarget = false;

        foreach (var wools in WoolControls)
        {
            // Skip nếu không trùng index hoặc thiếu transform
            if (wools.WoolOrder != index || wools.woolTransform == null)
                continue;

            Debug.Log($"[HandController] Found match: WoolOrder = {wools.WoolOrder}, isActive = {isActive}", gameObject);

            // Nếu isActive = true → Di chuyển tới vị trí đó
            if (isActive)
            {
                gameObject.SetActive(true);

                Vector3 endPos = wools.woolTransform.position + offset;

                transform.DOMove(endPos, delayTime).SetEase(Ease.OutQuad);
            }

            foundTarget = true;
            break; // chỉ xử lý 1 target là đủ
        }

        // Chỉ play anim nếu đang active và có target hợp lệ
        if (gameObject.activeSelf && foundTarget)
        {
            _playAnimCoroutine = StartCoroutine(PlayAnim());
        }
    }

    // public int woolOrder;
    // public bool isActive;
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         MoveHandToPosition(woolOrder,isActive);
    //     }
    // }

    IEnumerator PlayAnim()
    {
        var index = 0;
        var cound = 2;
        while (true)
        {
            handSpriteRenderer.sprite = handSprites[index];
            yield return new WaitForSeconds(delayTime - 0.2f);
            index++;
            if (index >= cound)
            {
                index = 0;
            }
            yield return null;
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(HandController))]
public class HandControllerEditor : Editor
{
    private int woolOrder = 0;
    private bool isActive;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Test Hand Movement To Wool Order Positions", EditorStyles.boldLabel);

        woolOrder = EditorGUILayout.IntField("Wool Order", woolOrder);
        isActive = EditorGUILayout.Toggle("Is Active", isActive);

        if (GUILayout.Button("Move To Next Position"))
        {
            var handController = (HandController)target;
            handController.MoveHandToPosition(woolOrder, isActive);
        }
    }
}
#endif


