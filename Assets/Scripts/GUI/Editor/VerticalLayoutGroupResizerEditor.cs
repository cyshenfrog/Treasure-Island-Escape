using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(VerticalLayoutGroupResizer))]
public class VerticalLayoutGroupResizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Resize"))
            ((VerticalLayoutGroupResizer)target).Resize();
    }
}
