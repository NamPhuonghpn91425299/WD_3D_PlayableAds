#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrientationElement))]
public class OrientationElementEditor : Editor
{
    private OrientationElement element;
    private SerializedProperty adjustPositionProp;
    private SerializedProperty adjustScaleProp;
    private SerializedProperty adjustAnchoredPositionProp;
    private SerializedProperty adjustSizeDeltaProp;
    private SerializedProperty adjustLocalScaleProp;

    private SerializedProperty portraitPositionProp;
    private SerializedProperty landscapePositionProp;
    private SerializedProperty portraitScaleProp;
    private SerializedProperty landscapeScaleProp;
    private SerializedProperty portraitAnchoredPositionProp;
    private SerializedProperty landscapeAnchoredPositionProp;
    private SerializedProperty portraitSizeDeltaProp;
    private SerializedProperty landscapeSizeDeltaProp;
    private SerializedProperty portraitLocalScaleProp;
    private SerializedProperty landscapeLocalScaleProp;

    private void OnEnable()
    {
        element = (OrientationElement)target;

        adjustPositionProp = serializedObject.FindProperty("adjustPosition");
        adjustScaleProp = serializedObject.FindProperty("adjustScale");
        adjustAnchoredPositionProp = serializedObject.FindProperty("adjustAnchoredPosition");
        adjustSizeDeltaProp = serializedObject.FindProperty("adjustSizeDelta");
        adjustLocalScaleProp = serializedObject.FindProperty("adjustLocalScale");

        portraitPositionProp = serializedObject.FindProperty("portraitPosition");
        landscapePositionProp = serializedObject.FindProperty("landscapePosition");
        portraitScaleProp = serializedObject.FindProperty("portraitScale");
        landscapeScaleProp = serializedObject.FindProperty("landscapeScale");
        portraitAnchoredPositionProp = serializedObject.FindProperty("portraitAnchoredPosition");
        landscapeAnchoredPositionProp = serializedObject.FindProperty("landscapeAnchoredPosition");
        portraitSizeDeltaProp = serializedObject.FindProperty("portraitSizeDelta");
        landscapeSizeDeltaProp = serializedObject.FindProperty("landscapeSizeDelta");
        portraitLocalScaleProp = serializedObject.FindProperty("portraitLocalScale");
        landscapeLocalScaleProp = serializedObject.FindProperty("landscapeLocalScale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Transform Settings", EditorStyles.boldLabel);

        // Position Settings
        EditorGUILayout.PropertyField(adjustPositionProp, new GUIContent("Adjust Position"));
        if (adjustPositionProp.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Portrait Position", EditorStyles.miniLabel);
            EditorGUILayout.PropertyField(portraitPositionProp);
            EditorGUILayout.LabelField("Landscape Position", EditorStyles.miniLabel);
            EditorGUILayout.PropertyField(landscapePositionProp);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();

        // Scale Settings
        EditorGUILayout.PropertyField(adjustScaleProp, new GUIContent("Adjust Scale"));
        if (adjustScaleProp.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Portrait Scale", EditorStyles.miniLabel);
            EditorGUILayout.PropertyField(portraitScaleProp);
            EditorGUILayout.LabelField("Landscape Scale", EditorStyles.miniLabel);
            EditorGUILayout.PropertyField(landscapeScaleProp);
            EditorGUI.indentLevel--;
        }

        // Only show RectTransform settings if the object has RectTransform
        if (element.GetComponent<RectTransform>() != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("RectTransform Settings", EditorStyles.boldLabel);

            // Anchored Position Settings
            EditorGUILayout.PropertyField(adjustAnchoredPositionProp, new GUIContent("Adjust Anchored Position"));
            if (adjustAnchoredPositionProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Portrait Anchored Position", EditorStyles.miniLabel);
                EditorGUILayout.PropertyField(portraitAnchoredPositionProp);
                EditorGUILayout.LabelField("Landscape Anchored Position", EditorStyles.miniLabel);
                EditorGUILayout.PropertyField(landscapeAnchoredPositionProp);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            // Size Delta Settings
            EditorGUILayout.PropertyField(adjustSizeDeltaProp, new GUIContent("Adjust Size Delta"));
            if (adjustSizeDeltaProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Portrait Size Delta", EditorStyles.miniLabel);
                EditorGUILayout.PropertyField(portraitSizeDeltaProp);
                EditorGUILayout.LabelField("Landscape Size Delta", EditorStyles.miniLabel);
                EditorGUILayout.PropertyField(landscapeSizeDeltaProp);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();

            // Local Scale Settings for RectTransform
            EditorGUILayout.PropertyField(adjustLocalScaleProp, new GUIContent("Adjust Local Scale"));
            if (adjustLocalScaleProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Portrait Local Scale", EditorStyles.miniLabel);
                EditorGUILayout.PropertyField(portraitLocalScaleProp);
                EditorGUILayout.LabelField("Landscape Local Scale", EditorStyles.miniLabel);
                EditorGUILayout.PropertyField(landscapeLocalScaleProp);
                EditorGUI.indentLevel--;
            }
        }


        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif