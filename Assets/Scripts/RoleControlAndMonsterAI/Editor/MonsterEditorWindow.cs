using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

public class MonsterEditorWindow : EditorWindow {

    string filePath = Application.dataPath + "/Resources/RoleControlAndMonsterAITest/Data/Monster";

    [MenuItem("Editor/Monster")]
    static void AddWindow() {

        readId();

        MonsterEditorWindow window = (MonsterEditorWindow)EditorWindow.GetWindow(typeof(MonsterEditorWindow), true, "怪物資料");
        window.Show();
    }

    GUIStyle style = new GUIStyle();
    static int id;
    bool isOld = false;
    string[] fieldName = {"名稱", "血量","攻擊範圍","攻擊速度","移動速度","攻擊"};
    string[] field = { "", "", "", "", "", ""};

    void OnGUI() {
        
        style.fontSize = 12;

        GUILayout.BeginVertical();

        if (GUILayout.Button("開啟檔案", GUILayout.Width(100))) {
            string file = "";
            file = EditorUtility.OpenFilePanel("選擇檔案", filePath, "xml");
            if (file != "") {
                isOld = true;
                file = file.Replace(filePath + "/", "");
                file = file.Replace(".xml", "");
                Monster m = Monster.Load(Int32.Parse(file));
                field[0] = m.Name;
                field[1] = m.BaseMaxHp.ToString();
                field[2] = m.BaseAttackRange.ToString();
                field[3] = m.BaseAttackSpace.ToString();
                field[4] = m.BaseSpeed.ToString();
                field[5] = m.BaseAttack.ToString();
                id = m.Id;
            }
        }

        for (int i = 0; i < fieldName.Length; i++) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(fieldName[i], style, GUILayout.Width(50));
            field[i] = GUILayout.TextField(field[i], GUILayout.Width(100));            
            GUILayout.EndHorizontal();
            GUILayout.Label("", GUILayout.Height(5));
        }

        if (GUILayout.Button("儲存", GUILayout.Width(100))) {

            bool check = true;
            foreach (string str in field) {
                if (str == "") {
                    check = false;
                    break;
                }
            }

            if (check) {
                Monster m = new Monster();
                m.Id = id;
                m.Name = field[0];
                m.BaseMaxHp = Int32.Parse(field[1]);
                m.BaseAttackRange = Int32.Parse(field[2]);
                m.BaseAttackSpace = Int32.Parse(field[3]);
                m.BaseSpeed = Int32.Parse(field[4]);
                m.BaseAttack = Int32.Parse(field[5]);

                m.Save();

                if (!isOld) {
                    using (var stream = new StreamWriter(filePath + "/id.txt", false, Encoding.UTF8)) {
                        stream.WriteLine((id + 1).ToString());
                    }
                }

                for (int i = 0; i < field.Length; i++) field[i] = "";
                EditorUtility.DisplayDialog("提示", "儲存完畢", "確認");

            }
            else {
                EditorUtility.DisplayDialog("提示", "資料不齊全", "確認");
            }
        }

        GUILayout.EndVertical();       
        
    }

    static void readId() {
        string str = Resources.Load<TextAsset>("RoleControlAndMonsterAITest/Data/Monster/id").ToString();
        id = Int32.Parse(str);
    }
}
