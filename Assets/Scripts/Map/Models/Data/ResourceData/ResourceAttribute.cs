using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

public class ResourceAttribute
{
    //for serialization
    public ResourceAttribute() { }

    public ResourceAttribute(string name)
    {
        this.name = name;
    }
    
    public enum GenerateMode
    {
        None,
        Single,
        Group
    }

    public enum PickFinishedMode
    {
        None,
        Destory,
        Rest
    }

    /*
    public List<Item> Items
    {
        get { return items; }
        set { items = value; }
    }
    */

    [XmlIgnore]
    public Action<ObjData>[] OnPickeds
    {
        get { return onPickeds; }
        set { onPickeds = value; }
    }

    [XmlIgnore]
    public Action<ObjData>[] OnPickFinisheds
    {
        get { return onPickFinisheds; }
        set { onPickFinisheds = value; }
    }

    [XmlIgnore]
    public Sprite[] Sprites
    {
        get { return sprites; }
        set { sprites = value; }
    }

    public List<int> DropItems
    {
        get { return dropItems; }
        set { dropItems = value; }
    }

    public List<float> DropRate
    {
        get { return dropRate; }
        set { dropRate = value; }
    }

    public MapConstants.LandformType Landform
    {
        get { return landform; }
        set { landform = value; }
    }

    public GenerateMode Gm
    {
        get { return gm; }
        set { gm = value; }
    }

    public PickFinishedMode OnPickFinishedMode
    {
        get { return onPickFinishedMode; }
        set { onPickFinishedMode = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public float GatherTime
    {
        get { return gatherTime; }
        set { gatherTime = value; }
    }

    public float GrowthTime
    {
        get { return growthTime; }
        set { growthTime = value; }
    }

    public int Type
    {
        get { return type; }
        set { type = value; }
    }

    public int Kind
    {
        get { return kind; }
        set { kind = value; }
    }
    
    public int Max
    {
        get { return max; }
        set { max = value; }
    }

    public int StateCount
    {
        get { return stateCount; }
        set { stateCount = value; }
    }

    public int ToolId
    {
        get { return toolId; }
        set { toolId = value; }
    }

    public int Width
    {
        get { return width; }
        set { width = value; }
    }

    public int Height
    {
        get { return height; }
        set { height = value; }
    }

    public bool IsNeedTool
    {
        get { return isNeedTool; }
        set { isNeedTool = value; }
    }

    Action<ObjData>[] onPickeds = null, onPickFinisheds = null;
    List<int> dropItems = new List<int>();
    List<float> dropRate = new List<float>();
    Sprite[] sprites;

    MapConstants.LandformType landform = MapConstants.LandformType.Grassland;
    //Vector2 pivot; 
    GenerateMode gm = GenerateMode.Single;
    PickFinishedMode onPickFinishedMode = PickFinishedMode.Destory;
    string name;
    float gatherTime = 5f, growthTime = 2f;
    int type = 0, kind = 0, max = 10, stateCount = 1, toolId = 0, width = 3, height = 5;
    bool isNeedTool = false;
}
