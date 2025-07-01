using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WoolDebugWindow : EditorWindow
{
    private Vector2 _scrollPos;
    private static bool _showInactive = false;
    private static bool _showAllScenes = false;
    private bool _autoRefresh = true;
    private static float _lastRefreshTime;
    private const float REFRESH_INTERVAL = 1f;

    [MenuItem("Tools/Wool Debug Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<WoolDebugWindow>("Wool Debug", false);
        window.minSize = new Vector2(300, 400);
        window.Show();
    }

    private void OnGUI()
    {
        // Auto-refresh logic
        if (_autoRefresh && EditorApplication.timeSinceStartup - _lastRefreshTime > REFRESH_INTERVAL)
        {
            Repaint();
            _lastRefreshTime = (float)EditorApplication.timeSinceStartup;
        }

        // Header with controls
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            GUILayout.Label("Wool Debugger", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            
            // Toggle buttons
            _showInactive = GUILayout.Toggle(_showInactive, "Show Inactive", EditorStyles.toolbarButton, GUILayout.Width(100));
            _showAllScenes = GUILayout.Toggle(_showAllScenes, "All Scenes", EditorStyles.toolbarButton, GUILayout.Width(80));
            _autoRefresh = GUILayout.Toggle(_autoRefresh, "Auto-Refresh", EditorStyles.toolbarButton, GUILayout.Width(90));
            
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_TreeEditor.Refresh"), EditorStyles.toolbarButton, GUILayout.Width(30)))
            {
                Repaint();
            }
        }
        EditorGUILayout.EndHorizontal();

        // Get all WoolControl objects
        var wools = _showAllScenes 
            ? FindObjectsOfType<WoolControl>(true) 
            : FindObjectsOfType<WoolControl>();

        if (!_showInactive)
        {
            wools = wools.Where(wool => wool.gameObject.activeInHierarchy).ToArray();
        }

        // Sort by WoolOrder
        var sortedWools = wools.OrderBy(wool => wool.WoolOrder).ToArray();

        // Display wool list
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        {
            EditorGUILayout.Space(5);
            
            // Column headers
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Order", GUILayout.Width(50));
            GUILayout.Label("Name", GUILayout.Width(150));
            GUILayout.Label("Position", GUILayout.Width(200));
            GUILayout.Label("Active");
            EditorGUILayout.EndHorizontal();

            // Wool items
            foreach (var wool in sortedWools)
            {
                if (wool == null) continue;
                
                bool isCurrent = wool.WoolOrder == WoolControl.CurrentWoolInSequence;
                var bgColor = GUI.backgroundColor;
                
                if (isCurrent)
                {
                    GUI.backgroundColor = new Color(0.4f, 0.8f, 1f, 0.3f);
                }

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    // Wool order (editable)
                    int newOrder = EditorGUILayout.DelayedIntField(wool.WoolOrder, GUILayout.Width(40));
                    if (newOrder != wool.WoolOrder)
                    {
                        wool.WoolOrder = Mathf.Max(1, newOrder); // Ensure order is at least 1
                        EditorUtility.SetDirty(wool);
                    }
                    
                    // Wool name with button to select
                    if (GUILayout.Button(wool.gameObject.name, EditorStyles.label, GUILayout.Width(150)))
                    {
                        Selection.activeGameObject = wool.gameObject;
                        SceneView.FrameLastActiveSceneView();
                    }

                    // Position
                    EditorGUILayout.LabelField(wool.transform.position.ToString("F1"), GUILayout.Width(200));
                    
                    // Active toggle
                    bool isActive = wool.gameObject.activeSelf;
                    if (GUILayout.Toggle(isActive, "", GUILayout.Width(20)) != isActive)
                    {
                        wool.gameObject.SetActive(!isActive);
                    }
                    
                    // Ping button
                    if (GUILayout.Button(EditorGUIUtility.IconContent("d_ViewToolZoom"), GUILayout.Width(25)))
                    {
                        EditorGUIUtility.PingObject(wool.gameObject);
                        Selection.activeGameObject = wool.gameObject;
                        SceneView.FrameLastActiveSceneView();
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                GUI.backgroundColor = bgColor;
            }

            if (sortedWools.Length == 0)
            {
                EditorGUILayout.HelpBox("No Wool objects found in the scene.", MessageType.Info);
            }
        }
        EditorGUILayout.EndScrollView();

        // Help text
        EditorGUILayout.Space(10);
        EditorGUILayout.HelpBox("Click on the order number to edit it directly.", MessageType.Info);

        // Status bar
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        GUILayout.Label($"Found {sortedWools.Length} wool objects");
        GUILayout.FlexibleSpace();
        GUILayout.Label($"Next: {WoolControl.CurrentWoolInSequence}");
        EditorGUILayout.EndHorizontal();
    }

    private void OnHierarchyChange()
    {
        if (_autoRefresh)
        {
            Repaint();
        }
    }

    private void OnSelectionChange()
    {
        if (_autoRefresh)
        {
            Repaint();
        }
    }

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        // Không tự động mở cửa sổ khi load
        // Chỉ đăng ký sự kiện để cập nhật khi cần thiết
        //EditorApplication.hierarchyChanged += OnHierarchyOrSceneChanged;
        //EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnHierarchyOrSceneChanged()
    {
        var window = GetWindow<WoolDebugWindow>();
        if (window != null && window._autoRefresh)
        {
            window.Repaint();
        }
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode || state == PlayModeStateChange.EnteredEditMode)
        {
            var window = GetWindow<WoolDebugWindow>();
            if (window != null)
            {
                window.Repaint();
            }
        }
    }
}
