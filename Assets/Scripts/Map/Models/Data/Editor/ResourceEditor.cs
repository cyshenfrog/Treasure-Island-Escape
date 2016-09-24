using UnityEngine;
using UnityEditor;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;

public class ResourceEditor : EditorWindow
{
    [MenuItem("Editor/ResourceEditor")]
    static void CallEditorWindow()
    {
        GetWindow(typeof(ResourceEditor));
    }

    void Load()
    {
        string file = filePath + fileName + ".xml";

        if(File.Exists(file))
        {
            //to read the file
            var serializer = new XmlSerializer(typeof(ResourceData));

            using (var stream = new FileStream(file, FileMode.Open))
            {
                rd = (ResourceData)serializer.Deserialize(stream);
            }

            isNeedTool = rd.IsNeedTool ? 1 : 0;
            gm = (int)rd.Gm;
            landform = (int)rd.Landform;

            EditorUtility.DisplayDialog("迷之音", "讀入" + fileName + "了!", "好");
        }
        else
        {
            //first loading
            try
            {
                //to detect if the filepath is legal => to create the filepath
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                rd = new ResourceData();
                Save();

                EditorUtility.DisplayDialog("迷之音", "查無此檔!\n自動產生新檔案!", "好");
            }
            catch(Exception e)
            {
                Debug.LogError("The creation of directoryPath is fail: " + e.ToString());
            }
        }
    }

    void Draw()
    {
        EditorGUILayout.BeginHorizontal();
        rd.Id = EditorGUILayout.IntField("編號", rd.Id, GUILayout.Width(200f));
        rd.Name = EditorGUILayout.TextField("名稱", rd.Name);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        landform = EditorGUILayout.Popup("生長地區", landform, landformName, GUILayout.Width(200f));
        rd.GatherTime = EditorGUILayout.FloatField("採集時間", rd.GatherTime, GUILayout.Width(200f));
        rd.Max = EditorGUILayout.IntField("數量上限", rd.Max, GUILayout.Width(200f));
        rd.GrowthTime = EditorGUILayout.FloatField("重生頻率", rd.GrowthTime, GUILayout.Width(200f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if ((isNeedTool = EditorGUILayout.Popup("是否需要採集工具", isNeedTool, YesNoMode, GUILayout.Width(200f))) == 1)
        {
            rd.ToolId = EditorGUILayout.IntField("採集工具編號", rd.ToolId, GUILayout.Width(200f));
        }

        rd.OnPickFinishedMode = EditorGUILayout.Popup("採集後動作", rd.OnPickFinishedMode, DisappearChangeMode, GUILayout.Width(200f));
        EditorGUILayout.EndHorizontal();

        int count = rd.DropRate.Count;
        for(int i = 0; i < count; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            items[i] = EditorGUILayout.IntField("道具", items[i], GUILayout.Width(200f));
            rd.DropRate[i] = EditorGUILayout.FloatField("掉落率", rd.DropRate[i], GUILayout.Width(200f));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("New", GUILayout.Width(150f)))
        {
            items.Add(0);
            rd.DropRate.Add(1f);
        }
        if (GUILayout.Button("Delete", GUILayout.Width(150f)))
        {
            int last = rd.DropRate.Count - 1;
            if (last >= 0)
            {
                items.RemoveAt(last);
                rd.DropRate.RemoveAt(last);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void Save()
    {
        string file = filePath + fileName + ".xml";

        //to detect if the filePath is legal
        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        //to put some info into rd
        int count = items.Count;
        for (int i = 0; i < count; ++i)
            if (rd.Items.Count == i)
            {
                /*
                Item item = new Item();
                item.type = (itemType)items[i];
                rd.Items.Add(item);
                */
                rd.Items.Add(items[i]);
            }

        rd.IsNeedTool = isNeedTool == 1 ? true : false;
        rd.Gm = (ResourceData.GrowthMode)gm;
        rd.Landform = (MapConstants.LandformType)landform;

        var serializer = new XmlSerializer(typeof(ResourceData));

        //to ensure the encoding
        using (var stream = new StreamWriter(file, false, Encoding.UTF8))
        {
            serializer.Serialize(stream, rd);
        }

        EditorUtility.DisplayDialog("迷之音", "存入" + fileName + "了!", "好");
    }
    
    void OnGUI()
    {
        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        //to load file
        EditorGUILayout.BeginHorizontal();
        fileName = EditorGUILayout.TextField("Loaded Filename: ", fileName, GUILayout.Width(300f));

        if (GUILayout.Button("Load", GUILayout.Width(150f)))
            if (EditorUtility.DisplayDialog("迷之音", "確定要讀入" + fileName + "嘛?", "是", "再考慮"))
                Load();
        EditorGUILayout.EndHorizontal();

        //to draw the buttons
        if(rd != null)
            Draw();

        //to save
        EditorGUILayout.BeginHorizontal();
        fileName = EditorGUILayout.TextField("Saved Filename: ", fileName, GUILayout.Width(300f));

        if (GUILayout.Button("Save", GUILayout.Width(150f)))
            if (EditorUtility.DisplayDialog("迷之音", "確定要存入" + fileName + "嘛?", "是", "否"))
                Save();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    static string fileName = "Resource", filePath = Application.dataPath + @"\Resources\Resource\";

    ResourceData rd = null;
    Vector2 scrollView = Vector2.zero;
    
    string[] YesNoMode = new string[] { "否", "是" }, DisappearChangeMode = new string[] { "消失", "改變" };
    string[] landformName = MapConstants.LandformName;

    //for data
    List<int> items = new List<int>();
    int isNeedTool = 0, gm = 1, landform = 0;
}