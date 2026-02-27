using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PrefabMeshSetupTransferTool : EditorWindow
{
    [Serializable]
    private class PrefabMeshSetupData
    {
        public int version = 1;
        public string prefabName;
        public string prefabAssetPath;
        public string exportedAtUtc;
        public List<NodeSetup> nodes = new List<NodeSetup>();
    }

    [Serializable]
    private class NodeSetup
    {
        public string hierarchyPath;
        public Vector3 localPosition;
        public Vector3 localEulerAngles;
        public Vector3 localScale;
        public MeshReference meshFilterMesh;
        public MeshReference skinnedMesh;
        public MeshColliderSetup meshCollider;
    }

    [Serializable]
    private class MeshReference
    {
        public string meshName;
        public string assetPath;
        public string assetGuid;
    }

    [Serializable]
    private class MeshColliderSetup
    {
        public bool enabled;
        public bool convex;
        public bool isTrigger;
        public MeshReference sharedMesh;
    }

    private GameObject _prefabAsset;
    private Vector2 _scroll;

    [MenuItem("Tools/Prefab Mesh Setup Transfer")]
    public static void ShowWindow()
    {
        GetWindow<PrefabMeshSetupTransferTool>("Prefab Mesh Setup");
    }

    private void OnGUI()
    {
        _scroll = EditorGUILayout.BeginScrollView(_scroll);
        GUILayout.Label("Prefab Mesh Setup Transfer", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "Export local transform + mesh assignments (MeshFilter, SkinnedMeshRenderer, MeshCollider) from a prefab to JSON, then import in another project.",
            MessageType.Info);

        _prefabAsset = (GameObject)EditorGUILayout.ObjectField(
            "Prefab Asset",
            _prefabAsset,
            typeof(GameObject),
            false);

        EditorGUILayout.Space();

        GUI.enabled = _prefabAsset != null;
        if (GUILayout.Button("Export JSON"))
        {
            ExportPrefabSetup();
        }

        if (GUILayout.Button("Import JSON To Prefab"))
        {
            ImportPrefabSetup();
        }
        GUI.enabled = true;

        EditorGUILayout.EndScrollView();
    }

    private void ExportPrefabSetup()
    {
        if (!TryGetPrefabPath(_prefabAsset, out var prefabPath))
        {
            EditorUtility.DisplayDialog("Error", "Please assign a valid prefab asset.", "OK");
            return;
        }

        var savePath = EditorUtility.SaveFilePanel(
            "Export Prefab Mesh Setup",
            Application.dataPath,
            $"{_prefabAsset.name}_MeshSetup",
            "json");

        if (string.IsNullOrEmpty(savePath))
        {
            return;
        }

        var prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
        if (prefabRoot == null)
        {
            EditorUtility.DisplayDialog("Error", $"Cannot load prefab: {prefabPath}", "OK");
            return;
        }

        try
        {
            var data = new PrefabMeshSetupData
            {
                prefabName = _prefabAsset.name,
                prefabAssetPath = prefabPath,
                exportedAtUtc = DateTime.UtcNow.ToString("O")
            };

            var transforms = prefabRoot.GetComponentsInChildren<Transform>(true);
            foreach (var tr in transforms)
            {
                var node = new NodeSetup
                {
                    hierarchyPath = GetHierarchyPath(prefabRoot.transform, tr),
                    localPosition = tr.localPosition,
                    localEulerAngles = tr.localEulerAngles,
                    localScale = tr.localScale
                };

                var meshFilter = tr.GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    node.meshFilterMesh = CreateMeshReference(meshFilter.sharedMesh);
                }

                var skinnedMeshRenderer = tr.GetComponent<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer != null)
                {
                    node.skinnedMesh = CreateMeshReference(skinnedMeshRenderer.sharedMesh);
                }

                var meshCollider = tr.GetComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    node.meshCollider = new MeshColliderSetup
                    {
                        enabled = meshCollider.enabled,
                        convex = meshCollider.convex,
                        isTrigger = meshCollider.isTrigger,
                        sharedMesh = CreateMeshReference(meshCollider.sharedMesh)
                    };
                }

                data.nodes.Add(node);
            }

            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);

            Debug.Log($"Exported mesh setup: {savePath}");
            EditorUtility.RevealInFinder(savePath);
            EditorUtility.DisplayDialog("Success", $"Exported {data.nodes.Count} nodes.", "OK");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
    }

    private void ImportPrefabSetup()
    {
        if (!TryGetPrefabPath(_prefabAsset, out var prefabPath))
        {
            EditorUtility.DisplayDialog("Error", "Please assign a valid prefab asset.", "OK");
            return;
        }

        var jsonPath = EditorUtility.OpenFilePanel("Import Prefab Mesh Setup", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath))
        {
            return;
        }

        if (!File.Exists(jsonPath))
        {
            EditorUtility.DisplayDialog("Error", $"File not found: {jsonPath}", "OK");
            return;
        }

        var json = File.ReadAllText(jsonPath);
        var data = JsonUtility.FromJson<PrefabMeshSetupData>(json);
        if (data == null || data.nodes == null || data.nodes.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "Invalid or empty JSON data.", "OK");
            return;
        }

        var prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);
        if (prefabRoot == null)
        {
            EditorUtility.DisplayDialog("Error", $"Cannot load prefab: {prefabPath}", "OK");
            return;
        }

        int appliedCount = 0;
        int missingPathCount = 0;
        int missingMeshCount = 0;

        try
        {
            foreach (var node in data.nodes)
            {
                var tr = FindByHierarchyPath(prefabRoot.transform, node.hierarchyPath);
                if (tr == null)
                {
                    missingPathCount++;
                    continue;
                }

                tr.localPosition = node.localPosition;
                tr.localEulerAngles = node.localEulerAngles;
                tr.localScale = node.localScale;

                if (node.meshFilterMesh != null)
                {
                    var meshFilter = tr.GetComponent<MeshFilter>();
                    if (meshFilter != null)
                    {
                        var mesh = ResolveMesh(node.meshFilterMesh);
                        if (mesh != null)
                        {
                            meshFilter.sharedMesh = mesh;
                        }
                        else
                        {
                            missingMeshCount++;
                        }
                    }
                }

                if (node.skinnedMesh != null)
                {
                    var skinnedMeshRenderer = tr.GetComponent<SkinnedMeshRenderer>();
                    if (skinnedMeshRenderer != null)
                    {
                        var mesh = ResolveMesh(node.skinnedMesh);
                        if (mesh != null)
                        {
                            skinnedMeshRenderer.sharedMesh = mesh;
                        }
                        else
                        {
                            missingMeshCount++;
                        }
                    }
                }

                if (node.meshCollider != null)
                {
                    var meshCollider = tr.GetComponent<MeshCollider>();
                    if (meshCollider != null)
                    {
                        meshCollider.enabled = node.meshCollider.enabled;
                        meshCollider.convex = node.meshCollider.convex;
                        meshCollider.isTrigger = node.meshCollider.isTrigger;

                        var mesh = ResolveMesh(node.meshCollider.sharedMesh);
                        if (mesh != null || node.meshCollider.sharedMesh == null)
                        {
                            meshCollider.sharedMesh = mesh;
                        }
                        else
                        {
                            missingMeshCount++;
                        }
                    }
                }

                appliedCount++;
            }

            PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                $"Imported mesh setup to {_prefabAsset.name}. Applied: {appliedCount}, Missing Paths: {missingPathCount}, Missing Meshes: {missingMeshCount}");
            EditorUtility.DisplayDialog(
                "Import Complete",
                $"Applied: {appliedCount}\nMissing Paths: {missingPathCount}\nMissing Meshes: {missingMeshCount}",
                "OK");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
    }

    private static bool TryGetPrefabPath(GameObject prefabAsset, out string prefabPath)
    {
        prefabPath = string.Empty;
        if (prefabAsset == null)
        {
            return false;
        }

        prefabPath = AssetDatabase.GetAssetPath(prefabAsset);
        if (string.IsNullOrEmpty(prefabPath))
        {
            return false;
        }

        return PrefabUtility.GetPrefabAssetType(prefabAsset) != PrefabAssetType.NotAPrefab;
    }

    private static string GetHierarchyPath(Transform root, Transform current)
    {
        if (current == root)
        {
            return string.Empty;
        }

        var names = new List<string>();
        var cursor = current;
        while (cursor != null && cursor != root)
        {
            names.Add(BuildSegment(cursor));
            cursor = cursor.parent;
        }

        names.Reverse();
        return string.Join("/", names);
    }

    private static Transform FindByHierarchyPath(Transform root, string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return root;
        }

        var parts = path.Split('/');
        var cursor = root;

        foreach (var part in parts)
        {
            ParseSegment(part, out var targetName, out var targetIndex);
            int matchIndex = -1;
            Transform found = null;

            for (int i = 0; i < cursor.childCount; i++)
            {
                var child = cursor.GetChild(i);
                if (!string.Equals(child.name, targetName, StringComparison.Ordinal))
                {
                    continue;
                }

                matchIndex++;
                if (matchIndex == targetIndex)
                {
                    found = child;
                    break;
                }
            }

            if (found == null)
            {
                return null;
            }

            cursor = found;
        }

        return cursor;
    }

    private static string BuildSegment(Transform tr)
    {
        if (tr.parent == null)
        {
            return $"{tr.name}#0";
        }

        int sameNameIndex = 0;
        for (int i = 0; i < tr.parent.childCount; i++)
        {
            var sibling = tr.parent.GetChild(i);
            if (!string.Equals(sibling.name, tr.name, StringComparison.Ordinal))
            {
                continue;
            }

            if (sibling == tr)
            {
                return $"{tr.name}#{sameNameIndex}";
            }

            sameNameIndex++;
        }

        return $"{tr.name}#0";
    }

    private static void ParseSegment(string segment, out string name, out int index)
    {
        name = segment;
        index = 0;

        int hash = segment.LastIndexOf('#');
        if (hash <= 0 || hash >= segment.Length - 1)
        {
            return;
        }

        var possibleIndex = segment.Substring(hash + 1);
        if (!int.TryParse(possibleIndex, out var parsed))
        {
            return;
        }

        name = segment.Substring(0, hash);
        index = Mathf.Max(0, parsed);
    }

    private static MeshReference CreateMeshReference(Mesh mesh)
    {
        if (mesh == null)
        {
            return null;
        }

        var meshPath = AssetDatabase.GetAssetPath(mesh);
        return new MeshReference
        {
            meshName = mesh.name,
            assetPath = meshPath,
            assetGuid = string.IsNullOrEmpty(meshPath) ? string.Empty : AssetDatabase.AssetPathToGUID(meshPath)
        };
    }

    private static Mesh ResolveMesh(MeshReference meshRef)
    {
        if (meshRef == null)
        {
            return null;
        }

        if (!string.IsNullOrEmpty(meshRef.assetGuid))
        {
            var pathFromGuid = AssetDatabase.GUIDToAssetPath(meshRef.assetGuid);
            var byGuid = LoadMeshAtPath(pathFromGuid, meshRef.meshName);
            if (byGuid != null)
            {
                return byGuid;
            }
        }

        var byPath = LoadMeshAtPath(meshRef.assetPath, meshRef.meshName);
        if (byPath != null)
        {
            return byPath;
        }

        if (!string.IsNullOrEmpty(meshRef.meshName))
        {
            var ids = AssetDatabase.FindAssets($"t:Mesh {meshRef.meshName}");
            foreach (var id in ids)
            {
                var path = AssetDatabase.GUIDToAssetPath(id);
                var mesh = LoadMeshAtPath(path, meshRef.meshName);
                if (mesh != null)
                {
                    return mesh;
                }
            }
        }

        return null;
    }

    private static Mesh LoadMeshAtPath(string path, string targetName)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        var assets = AssetDatabase.LoadAllAssetsAtPath(path);
        Mesh firstMesh = null;
        foreach (var asset in assets)
        {
            var mesh = asset as Mesh;
            if (mesh == null)
            {
                continue;
            }

            if (firstMesh == null)
            {
                firstMesh = mesh;
            }

            if (!string.IsNullOrEmpty(targetName) && string.Equals(mesh.name, targetName, StringComparison.Ordinal))
            {
                return mesh;
            }
        }

        return firstMesh;
    }
}
