using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class TileData
{
    //public components
    //public int SearchID = 0;

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

    public List<Vector2> Directions
    {
        get { return directions; }
        set { directions = value; }
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

    public bool IsRunable
    {
        get { return isRunable; }
    }

    public bool IsConstructable
    {
        get { return isConstructable; }
        set { isConstructable = value; }
    }

    public bool IsConstructed
    {
        get { return isConstructed; }
        set { isConstructed = value; }
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
    protected List<Vector2> directions = new List<Vector2>() { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    //used for blending images
    protected Vector2[] materialDirections = new Vector2[4];
    protected MapConstants.LandformType[] materialTypes = new MapConstants.LandformType[] { MapConstants.LandformType.None, MapConstants.LandformType.None, MapConstants.LandformType.None, MapConstants.LandformType.None };
    //unknown
    protected MapConstants.BuildingType buildingType;
    protected TileData center, fromTile, buildingCenter;
    protected Vector2 position, buildingPosition;
    protected Action<TileData> mtChanged;
    protected bool isRunable, isConstructable, isConstructed = false;

    //constructedObj
    //looseObj
    //TileData center????
}