using UnityEngine;
using System.Collections.Generic;

public abstract class ObjData
{
    public static ObjData Create(int type0 = 0, int type1 = 0, int type2 = 0, int type3 = 0)
    {
        switch(type0)
        {
            //...
            default: return null;
        }
    }

    //this is used to create sub obj under a main(center) obj
    public static ObjData SubCreate(int type0 = 0, int type1 = 0, int type2 = 0, int type3 = 0)
    {
        switch (type0)
        {
            //...
            default: return null;
        }
    }

    //bodies are localPosition
    public List<ObjData> Bodies
    {
        get;
        private set;
    }

    public Vector3 Center
    {
        get { return center; }
        set { center = value; }
    }

    public int State
    {
        get { return state; }
        set { state = value; }
    }

    public int No
    {
        get { return no; }
        set { no = value; }
    }

    //position and center are world positions
    protected Vector3 position, center;
    protected Sprite sprite;
    protected int no, state, type0, type1, type2, type3;
}
