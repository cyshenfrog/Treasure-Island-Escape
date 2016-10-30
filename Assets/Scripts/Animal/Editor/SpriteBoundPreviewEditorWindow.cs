using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

public class SpriteBoundPreviewEditorWindow : EditorWindow {

    [MenuItem("Editor/SpriteBound Preview")]
    static void AddWindow() {

        SpriteBoundPreviewEditorWindow window = (SpriteBoundPreviewEditorWindow)EditorWindow.GetWindow(typeof(SpriteBoundPreviewEditorWindow), true, "SpriteBound Preview");
        window.Show();
    }

    Texture2D select;
    Sprite[] cut;
    string[] spriteName = { "None" };
    int spriteIndex = 0;

    float top = 0;
    float bottom = 0;
    float right = 0;
    float left = 0;

    void OnGUI() {

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();

        select = EditorGUILayout.ObjectField("Select Image", select, typeof(Texture), true, GUILayout.Width(200))as Texture2D;
        if (GUILayout.Button("Convert", GUILayout.Width(200))) {
            if (select != null) {
                cut = cutSprite(select);
                spriteName = new string[cut.Length + 1];
                spriteName[0] = "None";
                for (int i = 1; i <= cut.Length; i++)
                    spriteName[i] = cut[i - 1].name;     
            }
        }
        spriteIndex = EditorGUILayout.Popup(spriteIndex, spriteName, GUILayout.Width(200));

        GUILayout.Label("Top");
        top = EditorGUILayout.Slider(top, 0, 1, GUILayout.Width(200));

        GUILayout.Label("Bottom");
        bottom = EditorGUILayout.Slider(bottom, 0, 1, GUILayout.Width(200));

        GUILayout.Label("Right");
        right = EditorGUILayout.Slider(right, 0, 1, GUILayout.Width(200));

        GUILayout.Label("Left");
        left = EditorGUILayout.Slider(left, 0, 1, GUILayout.Width(200));

        GUILayout.EndVertical();


        GUILayout.BeginVertical();

        if (spriteIndex != 0) {
            Texture2D show = spriteToTexture(cut[spriteIndex - 1]);
            GUILayout.Label(show);
        }

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        /*Handles.BeginGUI();
        Handles.color = Color.red;
        Handles.DrawLine(new Vector3(0, 0), new Vector3(300, 300));
        Handles.EndGUI();*/

        if (select != null && spriteIndex != 0) {
            //draw line
        }

    }


    Sprite[] cutSprite(Texture2D t) {
        string path = AssetDatabase.GetAssetPath(t);
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
        return sprites;
    }

    Texture2D spriteToTexture(Sprite s) {
        Texture2D t = new Texture2D((int)s.rect.width, (int)s.rect.height);
        Color[] pixels = s.texture.GetPixels(
            (int)s.rect.x,
            (int)s.rect.y,
            (int)s.rect.width,
            (int)s.rect.height);

        t.SetPixels(pixels);
        t.Apply();

        return t;
    }
}
