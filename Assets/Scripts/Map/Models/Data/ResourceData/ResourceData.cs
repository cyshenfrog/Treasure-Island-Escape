using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ResourceData : ObjData
{
    public ResourceData()
    {

    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    Action pickAction = null, onPickFinishedAction = null;
    //List<Item> items = new List<Item>();
    List<int> items = new List<int>();
    List<float> dropRate = new List<float>();

    MapConstants.LandformType landform = MapConstants.LandformType.Grassland;
    Vector2 pivot; 
    //GrowthMode gm = GrowthMode.Growth;
    string name = "";
    float gatherTime = 5f, growthTime = 2f;
    int id = 0, max = 10, toolId = 0, onPickFinishedMode = 0;
    bool isNeedTool = false;
}
