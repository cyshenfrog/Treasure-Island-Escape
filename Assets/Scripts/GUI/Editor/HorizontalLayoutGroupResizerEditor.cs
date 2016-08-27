using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(HorizontalLayoutGroupResizer))]
public class HorizontalLayoutGroupResizerEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Resize"))
            ((HorizontalLayoutGroupResizer)target).Resize();
    }
}
