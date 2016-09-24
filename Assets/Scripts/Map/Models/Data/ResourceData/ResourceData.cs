using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ResourceData : ObjData
{
    public ResourceData()
    {
    }

    public enum GrowthMode
    {
        None,
        Growth,
        Recovery
    }
    /*
    public List<Item> Items
    {
        get { return items; }
        set { items = value; }
    }
    */
    public List<string> Items
    {
        get { return items; }
        set { items = value; }
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

    public GrowthMode Gm
    {
        get { return gm; }
        set { gm = value; }
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
    
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
    
    public int Max
    {
        get { return max; }
        set { max = value; }
    }
    
    public int ToolId
    {
        get { return toolId; }
        set { toolId = value; }
    }

    public int OnPickFinishedMode
    {
        get { return onPickFinishedMode; }
        set { onPickFinishedMode = value; }
    }

    public bool IsNeedTool
    {
        get { return isNeedTool; }
        set { isNeedTool = value; }
    }

    Action pickAction = null, onPickFinishedAction = null;
    //List<Item> items = new List<Item>();
    List<string> items = new List<string>();
    List<float> dropRate = new List<float>();

    MapConstants.LandformType landform = MapConstants.LandformType.Grassland;
    GrowthMode gm = GrowthMode.Growth;
    string name = "";
    float gatherTime = 5f, growthTime = 2f;
    int id = 0, max = 10, toolId = 0, onPickFinishedMode = 0;
    bool isNeedTool = false;
}
