using UnityEngine;
using System.Collections.Generic;

public class ObjData
{
    public static ObjData Create(int type0 = 0, int type1 = 0, int type2 = 0, int type3 = 0)
    {
        switch(type0)
        {
            //...
            default: return null;
        }
    }

    public static ObjData Create(ResourceAttribute ra, int oID, Vector2 position)
    {
        ObjData od = new ObjData();
        od.oID = oID;
        od.state = 0;
        od.position = position;
        return od;
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

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }

    public int State
    {
        get { return state; }
        set { state = value; }
    }

    public int OID
    {
        get { return oID; }
        set { oID = value; }
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

    /*
    public int Number
    {
        get { return number; }
        set { number = value; }
    }
    */

    //position and center are world positions
    protected Vector3 position, center;
    protected int oID, number, state, type, kind, type2, type3;
}
