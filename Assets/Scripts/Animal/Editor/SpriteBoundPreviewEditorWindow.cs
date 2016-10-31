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
    Texture2D show;
    Sprite[] cut;
    string[] spriteName = { "None" };
    int spriteIndex = 0;

    float top, bottom, left, right;
    Vector3 q1, q2, q3, q4;

    void OnGUI() {

        GUILayout.BeginVertical();

        if (GUILayout.Button("README", GUILayout.Width(200))) {
            EditorUtility.DisplayDialog("README", "Make sure the mode of the image you want to preview is \"advanced\", and \"Read/Write Enabled\" is clicked.", "OK");
        }

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
       
        GUILayout.Label("Left");
        left = EditorGUILayout.Slider(left, 0, 1, GUILayout.Width(200));

        GUILayout.Label("Right");
        right = EditorGUILayout.Slider(right, 0, 1, GUILayout.Width(200));

        GUILayout.EndVertical();

        if (spriteIndex != 0) {
            show = spriteToTexture(cut[spriteIndex - 1]);
            GUI.Label(new Rect(250, 50, show.width, show.height), show);            
        }

        if (select != null && spriteIndex != 0) {

            //handle vector
            float extentWidth = cut[spriteIndex - 1].bounds.extents.x * 100;
            float extentHeight = cut[spriteIndex - 1].bounds.extents.y * 100;
            Vector3 center = new Vector2(250, 50) + cut[spriteIndex - 1].rect.center - cut[spriteIndex - 1].rect.position;

            q1 = center + new Vector3(extentWidth * (0.5f - right), -extentHeight * (0.5f - top)) * 2;
            q2 = center + new Vector3(-extentWidth * (0.5f - left), -extentHeight * (0.5f - top)) * 2;
            q3 = center + new Vector3(-extentWidth * (0.5f - left), extentHeight * (0.5f - bottom)) * 2;
            q4 = center + new Vector3(extentWidth * (0.5f - right), extentHeight * (0.5f - bottom)) * 2;

            //draw line
            Handles.BeginGUI();
            Handles.color = Color.red;
            Handles.DrawLine(q1, q2);
            Handles.DrawLine(q1, q4);
            Handles.DrawLine(q3, q2);
            Handles.DrawLine(q3, q4);
            Handles.EndGUI();
        }

    }


    Sprite[] cutSprite(Texture2D t) {
        string path = AssetDatabase.GetAssetPath(t);
        return AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
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
