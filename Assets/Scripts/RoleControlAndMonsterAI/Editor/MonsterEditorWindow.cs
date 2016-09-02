using UnityEngine;
using UnityEditor;
using System.Collections;

public class MonsterEditorWindow : EditorWindow {

    [MenuItem("Editor/Monster")]
    static void AddWindow() {
        MonsterEditorWindow window = (MonsterEditorWindow)EditorWindow.GetWindow(typeof(MonsterEditorWindow), true, "怪物資料");
        window.Show();
    }


    void OnGUI() {
        GUILayout.BeginVertical();
        GUILayout.EndVertical();       
        
    }
}
