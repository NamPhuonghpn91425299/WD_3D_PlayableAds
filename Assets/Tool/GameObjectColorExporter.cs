using System;
        using UnityEngine;
        using UnityEditor;
        using System.Collections.Generic;
        
        [Serializable]
        public class GameObjectColorData
        {
            public string objectName;
            public string colorHex;
        
            public GameObjectColorData(string name, Color color)
            {
                objectName = name;
                colorHex = "#" + ColorUtility.ToHtmlStringRGBA(color);
            }
        }
        
        [Serializable]
        public class GameObjectColorDatabase
        {
            public List<GameObjectColorData> colorDataList = new List<GameObjectColorData>();
        }
        
        public class GameObjectColorExporter : EditorWindow
        {
            private List<GameObject> gameObjects = new List<GameObject>();
            private Vector2 scroll;
            private string exportedData = "";
        
            [MenuItem("Tools/GameObject Color Exporter")]
            public static void ShowWindow()
            {
                GetWindow<GameObjectColorExporter>("GameObject Color Exporter");
            }
        
            private void OnGUI()
            {
                EditorGUILayout.LabelField("GameObject Color Exporter", EditorStyles.boldLabel);
                EditorGUILayout.Space();
        
                // Drop area for GameObjects
                Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
                GUI.Box(dropArea, "Drag GameObjects here", EditorStyles.helpBox);
                HandleDragAndDrop(dropArea);
        
                EditorGUILayout.Space();
        
                // Display list of GameObjects
                scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
                for (int i = gameObjects.Count - 1; i >= 0; i--)
                {
                    EditorGUILayout.BeginHorizontal();
                    gameObjects[i] = (GameObject)EditorGUILayout.ObjectField(gameObjects[i], typeof(GameObject), true);
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        gameObjects.RemoveAt(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
        
                EditorGUILayout.Space();
        
                // Buttons
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Export Colors to JSON"))
                {
                    ExportColorsToJson();
                }
                if (GUILayout.Button("Remove All"))
                {
                    gameObjects.Clear();
                }
                EditorGUILayout.EndHorizontal();
        
                // Display exported data
                if (!string.IsNullOrEmpty(exportedData))
                {
                    EditorGUILayout.LabelField("Exported Data:");
                    EditorGUILayout.TextArea(exportedData, GUILayout.Height(150));
                }
            }
        
            private void HandleDragAndDrop(Rect dropArea)
            {
                Event currentEvent = Event.current;
        
                if (dropArea.Contains(currentEvent.mousePosition))
                {
                    if (currentEvent.type == EventType.DragUpdated || currentEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        
                        if (currentEvent.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();
        
                            foreach (var draggedObject in DragAndDrop.objectReferences)
                            {
                                if (draggedObject is GameObject gameObject && !gameObjects.Contains(gameObject))
                                {
                                    gameObjects.Add(gameObject);
                                }
                            }
                        }
        
                        currentEvent.Use();
                    }
                }
            }
        
            private void ExportColorsToJson()
            {
                var database = new GameObjectColorDatabase();
        
                foreach (var gameObject in gameObjects)
                {
                    if (gameObject != null)
                    {
                        var renderer = gameObject.GetComponent<Renderer>();
                        if (renderer != null && renderer.sharedMaterial != null)
                        {
                            if (renderer.sharedMaterial.HasProperty("_ColorTint"))
                            {
                                Color colorTint = renderer.sharedMaterial.GetColor("_ColorTint");
                                database.colorDataList.Add(new GameObjectColorData(gameObject.name, colorTint));
                            }
                            else
                            {
                                Debug.LogWarning($"GameObject '{gameObject.name}' does not have a '_ColorTint' property.");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"GameObject '{gameObject.name}' does not have a valid Renderer or Material.");
                        }
                    }
                }
        
                if (database.colorDataList.Count == 0)
                {
                    Debug.LogWarning("No valid colors found. Exporting an empty colorDataList.");
                }
        
                exportedData = JsonUtility.ToJson(database, true);
        
                // Copy JSON to clipboard
                EditorGUIUtility.systemCopyBuffer = exportedData;
                Debug.Log("Exported color data copied to clipboard!");
            }
        }