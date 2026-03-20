using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LevelConfigSO))]
public class LevelConfigSOEditor : Editor
{
    private SerializedProperty _levelStartLoopProp;
    private SerializedProperty _levelListProp;
    private string _searchLevelName = "";
    private int _lastFoundIndex = -1;
    private bool _logToConsole = true;

    private void OnEnable()
    {
        _levelStartLoopProp = serializedObject.FindProperty("levelStartLoop");
        _levelListProp = serializedObject.FindProperty("levelConfigDataList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawSearchUI();
        EditorGUILayout.Space(6);

        if (_levelStartLoopProp != null)
        {
            EditorGUILayout.PropertyField(_levelStartLoopProp);
            EditorGUILayout.Space(4);
        }

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSearchUI()
    {
        EditorGUILayout.LabelField("Find Level By Name", EditorStyles.boldLabel);

        using (new EditorGUILayout.HorizontalScope())
        {
            _searchLevelName = EditorGUILayout.TextField("LevelName", _searchLevelName);

            using (new EditorGUI.DisabledScope(string.IsNullOrWhiteSpace(_searchLevelName)))
            {
                if (GUILayout.Button("Ping", GUILayout.Width(60)))
                {
                    PingFirstMatch();
                }

                if (GUILayout.Button("Next", GUILayout.Width(60)))
                {
                    PingNextMatch();
                }
            }
        }

        _logToConsole = EditorGUILayout.ToggleLeft("Log result to Console", _logToConsole);

        if (_lastFoundIndex >= 0)
        {
            EditorGUILayout.HelpBox($"Found at index: {_lastFoundIndex}", MessageType.Info);
        }
    }

    private void PingFirstMatch()
    {
        _lastFoundIndex = FindMatchIndex(_searchLevelName, 0);
        ReportResult("Ping");
    }

    private void PingNextMatch()
    {
        var start = _lastFoundIndex >= 0 ? _lastFoundIndex + 1 : 0;
        var idx = FindMatchIndex(_searchLevelName, start);
        if (idx < 0 && start > 0)
        {
            idx = FindMatchIndex(_searchLevelName, 0);
        }

        _lastFoundIndex = idx;
        ReportResult("Next");
    }

    private int FindMatchIndex(string query, int startIndex)
    {
        if (string.IsNullOrWhiteSpace(query) || _levelListProp == null)
            return -1;

        var q = query.Trim();
        var count = _levelListProp.arraySize;

        // First pass: exact match (case-insensitive)
        for (int i = startIndex; i < count; i++)
        {
            if (IsMatchAtIndex(i, q, exact: true))
                return i;
        }

        // Second pass: contains (case-insensitive)
        for (int i = startIndex; i < count; i++)
        {
            if (IsMatchAtIndex(i, q, exact: false))
                return i;
        }

        return -1;
    }

    private bool IsMatchAtIndex(int index, string query, bool exact)
    {
        var element = _levelListProp.GetArrayElementAtIndex(index);
        var levelNameProp = element.FindPropertyRelative("LevelName");
        var levelName = levelNameProp?.stringValue ?? string.Empty;

        if (exact)
            return string.Equals(levelName, query, System.StringComparison.OrdinalIgnoreCase);

        return levelName.IndexOf(query, System.StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void ReportResult(string action)
    {
        if (_lastFoundIndex < 0)
        {
            EditorUtility.DisplayDialog("Not Found", "No matching LevelName found.", "OK");
            return;
        }

        var element = _levelListProp.GetArrayElementAtIndex(_lastFoundIndex);
        var levelNameProp = element.FindPropertyRelative("LevelName");
        var levelName = levelNameProp != null ? levelNameProp.stringValue : string.Empty;

        if (_logToConsole)
        {
            Debug.Log($"[LevelConfigSO] {action} found LevelName '{levelName}' at index {_lastFoundIndex}.", target);
        }
    }
}
