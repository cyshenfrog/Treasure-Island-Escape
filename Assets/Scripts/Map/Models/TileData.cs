using UnityEngine;
using System.Collections;
using System;
/*
public abstract class TileData
{
    
    public enum TileType
    {
        Floor,
        Building,
        BuildingCenter,
        Border
    }
    

    //based on Sea
    //protected TileType type = TileType.Floor;
    protected MapConstants.MaterialType materialType = MapConstants.MaterialType.Sea;
    protected MapConstants.BuildingType buildingType;
    protected bool isRunable = false, isConstructable = false, isConstructed = false;
    protected TileData buildingCenter = null;
    protected int x, y, buildingX, buildingY;
    protected Vector2 po;
    //constructedObj
    //looseObj

    Action<TileData> mtChanged;

    public int SearchID = 0;

    public int X
    {
        get { return x; }
    }

    public int Y
    {
        get { return y; }
    }

    public Vector2 Po
    {
        get { return po; }
    }

    public MapConstants.MaterialType MaterialType
    {
        get { return materialType; }
        set
        {
            MapConstants.MaterialType oldmt = materialType;
            materialType = value;

            //to call the callback and let things know we've changed.
            if (mtChanged != null && oldmt != materialType)
                mtChanged(this);
        }
    }

    public void AddMtChanged(Action<TileData> a)
    {
        mtChanged += a;
    }

    public void RemoveMtChanged(Action<TileData> a)
    {
        mtChanged -= a;
    }
}

//default
public class Sea : TileData
{
    public Sea(int x, int y)
    {
        this.x = x;
        this.y = y;
        po = new Vector2(x, y);
    }
}

public class Floor : TileData
{
    public Floor(int x, int y, MapConstants.MaterialType mt)
    {
        this.x = x;
        this.y = y;
        po = new Vector2(x, y);
        materialType = mt;
        
        isRunable = true;
        isConstructable = true;
    }
}

public class Building : TileData
{
    public Building(int x, int y, MapConstants.MaterialType mt, MapConstants.BuildingType bt, int bx, int by, TileData center)
    {
        
    }
}
*/