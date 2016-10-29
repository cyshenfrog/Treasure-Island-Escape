using UnityEngine;
using UnityEditor;
using System.Collections;

public class TestEditorWindow : EditorWindow {



    [MenuItem("Editor/DrawSprite")]
    static void AddWindow() {
        TestEditorWindow window = (TestEditorWindow)EditorWindow.GetWindow(typeof(TestEditorWindow), true, "Draw Test");
        window.Show();
    }

    void OnGUI() {

    }
}
