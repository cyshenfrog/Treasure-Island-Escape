using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class TileData
{
    public static bool DFS(List<TileData> selected, int index, TileData[][] worldData)
    {
        TileData td = selected[index];

        while (td.Directions.Count != 0)
        {
            int random = UnityEngine.Random.Range(0, td.Directions.Count);
            Vector2 direction = td.Directions[random], nextPosition = td.Position + direction;

            //to remove the direction in td
            td.Directions.RemoveAt(random);

            //to get nexttd
            if(nextPosition.x < worldData.Length && nextPosition.x >= 0 && nextPosition.y < worldData[0].Length && nextPosition.y >= 0)
            {
                //reasonable world coordinates
                
                //to check if the nexttd has benn found
                if (worldData[(int)nextPosition.x][(int)nextPosition.y] == null)
                {
                    //first found
                    TileData next = worldData[(int)nextPosition.x][(int)nextPosition.y] = Factory(td.MaterialTypes[0], nextPosition, td);
                    
                    next.MaterialDirections[0] = direction;
                    next.Directions.Remove(-direction);
                    selected[index] = next;
                    return true;
                }
                else
                {
                    //it has been found before
                    TileData next = worldData[(int)nextPosition.x][(int)nextPosition.y];
                    if (next.MaterialTypes[0] != td.MaterialTypes[0])
                    {
                        //nexttd is an edge!!
                        for (int i = 1; i < 4; ++i)
                        {
                            if (next.MaterialTypes[i] == MapConstants.MaterialType.None)
                            {
                                next.MaterialTypes[i] = td.MaterialTypes[0];
                                next.MaterialDirections[i] = direction;
                                next.Directions.Remove(-direction);
                                break;
                            }
                        }
                    }
                }
            }
        }

        //all directions in td have been found
        if(td != td.fromTile)
        {
            //back to fromTile
            selected[index] = selected[index].fromTile;
            return DFS(selected, index, worldData);
        }
        else
        {
            //td is original center
            return false;
        }
    }

    public static TileData Factory(MapConstants.MaterialType mt, Vector2 position, TileData fromTile = null)
    {
        switch(mt)
        {
            case MapConstants.MaterialType.Sea:
                return new Sea(position, fromTile);
            case MapConstants.MaterialType.Vestige:
                return new Vestige(position, fromTile);
            case MapConstants.MaterialType.Forest:
                return new Forest(position, fromTile);
            case MapConstants.MaterialType.Grasslands:
                return new Grasslands(position, fromTile);
            case MapConstants.MaterialType.Marsh:
                return new Marsh(position, fromTile);
            case MapConstants.MaterialType.Desert:
                return new Desert(position, fromTile);
            case MapConstants.MaterialType.Volcano:
                return new Volcano(position, fromTile);
            case MapConstants.MaterialType.None:
                Debug.LogError("TileData Factory Error: MaterialType is None");
                return null;
            default:
                Debug.LogError("TileData Factory Error: Unreasonable MaterialType");
                return null;
        }
    }

    //public components
    //public int SearchID = 0;

    public MapConstants.MaterialType[] MaterialTypes
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
                MapConstants.MaterialType[] oldmt = materialTypes;
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
    protected MapConstants.MaterialType[] materialTypes = new MapConstants.MaterialType[] { MapConstants.MaterialType.None, MapConstants.MaterialType.None, MapConstants.MaterialType.None, MapConstants.MaterialType.None};
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