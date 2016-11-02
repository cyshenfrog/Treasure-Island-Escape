using UnityEngine;
using System.Collections.Generic;
using System;

public class TileData
{
    //public components
    //public int SearchID = 0;

    public TileData(MapConstants.LandformType lm, Vector2 position, TileData fromTile = null)
    {
        this.position = position;
        this.fromTile = fromTile != null ? fromTile : this;
        center = this;
        materialTypes[0] = lm;

        looseObj = new List<object>();
        rdList = new List<ResourceAttribute>();
    }

    public MapConstants.LandformType[] MaterialTypes
    {
        get { return materialTypes; }
        set
        {
            if(mtChanged == null)
            {
                materialTypes = value;
            }
            else
            {
                MapConstants.LandformType[] oldmt = materialTypes;
                materialTypes = value;

                //?????
                for(int i = 0; i < 4; ++i)
                {
                    if (oldmt[i] != materialTypes[i])
                        mtChanged(this);
                }
            }
        }
    }

    public MapConstants.BuildingType BuildingType
    {
        get { return buildingType; }
    }

    public Vector2[] MaterialDirections
    {
        get { return materialDirections; }
        set { materialDirections = value; }
    }

    /*
    public List<Vector2> Directions
    {
        get { return directions; }
        set { directions = value; }
    }
    */

    public Vector2[] Directions
    {
        get { return directions; }
        set { directions = value; }
    }

    public List<ResourceAttribute> RdList
    {
        get { return rdList; }
        set { rdList = value; }
    }

    public Vector2 Position
    {
        get { return position; }
    }

    public Vector2 BuildingPosition
    {
        get { return buildingPosition; }
    }

    public TileData Center
    {
        get { return center; }
    }

    public TileData FromTile
    {
        get { return fromTile; }
    }

    public TileData BuildingCenter
    {
        get { return buildingCenter; }
    }

    public List<object> LooseObj
    {
        get { return looseObj; }
        set { looseObj = value; }
    }

    public ObjBehaviour FixedObj
    {
        get { return fixedObj; }
        set { fixedObj = value; }
    }

    public bool IsRunable
    {
        get
        {
            switch (materialTypes[0])
            {
                case MapConstants.LandformType.Sea:
                    return false;
                default:
                    return true;
            }
        }
    }

    public bool IsConstructable
    {
        get
        {
            switch (materialTypes[0])
            {
                case MapConstants.LandformType.Sea:
                    return false;
                default:
                    return true;
            }
        }
    }

    public bool IsConstructed
    {
        get
        {
            return false;
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

    //DFS directions
    //protected List<Vector2> directions = new List<Vector2>() { Vector2.left + Vector2.up, Vector2.up, Vector2.right + Vector2.up, Vector2.left, Vector2.right, Vector2.left + Vector2.down, Vector2.down, Vector2.right + Vector2.down };
    protected Vector2[] directions = new Vector2[] { Vector2.left + Vector2.up, Vector2.up, Vector2.right + Vector2.up, Vector2.left, Vector2.right, Vector2.left + Vector2.down, Vector2.down, Vector2.right + Vector2.down };
    //used for blending images
    protected Vector2[] materialDirections = new Vector2[4];
    protected List<ResourceAttribute> rdList;
    protected MapConstants.LandformType[] materialTypes = new MapConstants.LandformType[] { MapConstants.LandformType.None, MapConstants.LandformType.None, MapConstants.LandformType.None, MapConstants.LandformType.None };
    //unknown
    protected MapConstants.BuildingType buildingType;
    protected TileData center, fromTile, buildingCenter;
    protected List<object> looseObj;
    protected Vector2 position, buildingPosition;
    protected Action<TileData> mtChanged;
    protected ObjBehaviour fixedObj = null;
    protected bool isRunable, isConstructable, isConstructed;


    //constructedObj
    //looseObj
    //TileData center????
}