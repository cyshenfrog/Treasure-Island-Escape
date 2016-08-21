using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class TileData
{
    public static bool DFS(List<TileData> selectedDFS, int index, TileData[][] worldData, Ellipse islandForm)
    {
        TileData td = selectedDFS[index];

        while (td.Directions.Count != 0)
        {
            int random = UnityEngine.Random.Range(0, td.Directions.Count);
            Vector2 direction = td.Directions[random], nextPosition = td.Position + direction;

            //to remove the direction in td
            td.Directions.RemoveAt(random);

            //to get nexttd
            if(islandForm.Inside(nextPosition))
            {
                //inside
                //to check if the nexttd has benn found
                if (worldData[(int)nextPosition.x][(int)nextPosition.y] == null)
                {
                    //first found
                    selectedDFS[index] = CreateNext(nextPosition, direction, td, worldData, true);
                    return true;
                }
                else
                {
                    //it has been found before
                    CreateEdge(direction, td, worldData[(int)nextPosition.x][(int)nextPosition.y]);
                }
            }
            else
            {
                //randomly outside
                if (UnityEngine.Random.Range(0, 9) < 5 && nextPosition.x < worldData.Length && nextPosition.x >= 0 && nextPosition.y < worldData[0].Length && nextPosition.y >= 0)
                {
                    //reasonable world coordinates

                    //to check if the nexttd has benn found
                    if (worldData[(int)nextPosition.x][(int)nextPosition.y] == null)
                    {
                        //first found
                        /*
                        TileData next = worldData[(int)nextPosition.x][(int)nextPosition.y] = Factory(td.MaterialTypes[0], nextPosition, td);
                        next.MaterialDirections[0] = direction;
                        next.Directions.Remove(-direction);
                        */
                        selectedDFS[index] = CreateNext(nextPosition, direction, td, worldData, true);
                        return true;
                    }
                    else
                    {
                        //it has been found before
                        /*
                        TileData next = worldData[(int)nextPosition.x][(int)nextPosition.y];
                        if (next.MaterialTypes[0] != td.MaterialTypes[0])
                        {
                            //nexttd is an edge!!
                            for (int i = 1; i < 4; ++i)
                            {
                                if (next.MaterialTypes[i] == MapConstants.LandformType.Sea)
                                {
                                    next.MaterialTypes[i] = td.MaterialTypes[0];
                                    next.MaterialDirections[i] = direction;
                                    next.Directions.Remove(-direction);
                                    break;
                                }
                            }
                        }
                        */
                        CreateEdge(direction, td, worldData[(int)nextPosition.x][(int)nextPosition.y]);
                    }
                }
            }
        }

        //all directions in td have been found
        if(td != td.fromTile)
        {
            //back to fromTile
            selectedDFS[index] = selectedDFS[index].fromTile;
            return DFS(selectedDFS, index, worldData, islandForm);
        }
        else
        {
            //td is original center
            return false;
        }
    }

    public static void BFS(List<TileData> selected, TileData[][] worldData)
    {
        int times = selected.Count > maxBFSTimes ? maxBFSTimes : selected.Count;

        while(--times >= 0)
        {
            TileData td = selected[0];

            while (td.Directions.Count != 0)
            {
                Vector2 direction = td.Directions[0], nextPosition = td.Position + direction;

                //to remove the direction in td
                td.Directions.RemoveAt(0);

                //to get nexttd
                if (nextPosition.x < worldData.Length && nextPosition.x >= 0 && nextPosition.y < worldData[0].Length && nextPosition.y >= 0)
                {
                    //reasonable world coordinates

                    //to check if the nexttd has benn found
                    if (worldData[(int)nextPosition.x][(int)nextPosition.y] == null)
                    {
                        //first found
                        //fromTile are not needed
                        TileData next = worldData[(int)nextPosition.x][(int)nextPosition.y] = Factory(td.MaterialTypes[0], nextPosition, td);

                        next.MaterialDirections[0] = direction;
                        next.Directions.Remove(-direction);
                        selected.Add(next);
                    }
                    else
                    {
                        //it has been found before
                        CreateEdge(direction, td, worldData[(int)nextPosition.x][(int)nextPosition.y]);
                    }
                }
            }
            
            selected.RemoveAt(0);
        }
    }

    public static TileData Factory(MapConstants.LandformType mt, Vector2 position, TileData fromTile = null)
    {
        switch(mt)
        {
            case MapConstants.LandformType.Volcano:
                return new Volcano(position, fromTile);
            case MapConstants.LandformType.Snowfield:
                return new Snowfield(position, fromTile);
            case MapConstants.LandformType.Marsh:
                return new Marsh(position, fromTile);
            case MapConstants.LandformType.Desert:
                return new Desert(position, fromTile);
            case MapConstants.LandformType.Forest:
                return new Forest(position, fromTile);
            case MapConstants.LandformType.Grasslands:
                return new Grasslands(position, fromTile);
            case MapConstants.LandformType.Sea:
                return new Sea(position, fromTile);
            default:
                Debug.LogError("TileData Factory Error: Unreasonable MaterialType");
                return null;
        }
    }

    static void CreateEdge(Vector2 direction, TileData td, TileData next)
    {
        if (next.MaterialTypes[0] != td.MaterialTypes[0])
            for (int i = 1; i < 4; ++i)
                if (next.MaterialTypes[i] == MapConstants.LandformType.Sea)
                {
                    next.MaterialTypes[i] = td.MaterialTypes[0];
                    next.MaterialDirections[i] = direction;
                    next.Directions.Remove(-direction);
                    return;
                }
    }

    static TileData CreateNext(Vector2 nextPosition, Vector2 direction, TileData td, TileData[][] worldData, bool DFS)
    {
        TileData next = worldData[(int)nextPosition.x][(int)nextPosition.y] = Factory(td.MaterialTypes[0], nextPosition, DFS ? td : null);
        next.MaterialDirections[0] = direction;
        next.Directions.Remove(-direction);
        return next;
    }

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
    protected MapConstants.LandformType[] materialTypes = new MapConstants.LandformType[] { MapConstants.LandformType.Sea, MapConstants.LandformType.Sea, MapConstants.LandformType.Sea, MapConstants.LandformType.Sea };
    //unknown
    protected MapConstants.BuildingType buildingType;
    protected TileData center, fromTile, buildingCenter;
    protected Vector2 position, buildingPosition;
    protected Action<TileData> mtChanged;
    protected bool isRunable, isConstructable, isConstructed = false;

    static int maxBFSTimes = (int)MapConstants.LandformType.Sea - 5;

    //constructedObj
    //looseObj
    //TileData center????
}