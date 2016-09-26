using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Collections;

public static class Explorer {
    [MenuItem("Explorer/Persistent Data Path")]
    public static void OpenPersistentDataPath()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
    [MenuItem("Explorer/Data Path")]
    public static void OpenDataPath()
    {
        EditorUtility.RevealInFinder(Application.dataPath);
    }
}
