using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class TileData
{
    /*
    public TileData(Vector2 position)
    {
        this.position = position;
        center = this;
        fromtile = this;

        materialtypes = new MapConstants.MaterialType[] { MapConstants.MaterialType.None, MapConstants.MaterialType.None, MapConstants.MaterialType.None, MapConstants.MaterialType.None };
        for (int i = 0; i < 4; ++i)
            materialtypes[i] = MapConstants.MaterialType.None;

        materialdirections = new Vector2[4];

        directions = new List<Vector2>();
        directions.Add(Vector2.up);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        directions.Add(Vector2.right);
    }
    */

    public void SwitchNext(TileData last, Vector2 fromdirection)
    {
        center = last.center;
        fromTile = last;

        materialTypes[0] = last.MaterialTypes[0];
        materialDirections[0] = -fromdirection;

        directions.Remove(fromdirection);
    }

    public static bool DFS(List<TileData> selectedTds, int index, TileData[][] worldData)
    {
        TileData td = selectedTds[index];

        while (td.Directions.Count != 0)
        {
            int random = UnityEngine.Random.Range(0, td.Directions.Count);
            Vector2 direction = td.Directions[random], nextposition = td.Position + direction;

            //to remove the direction in td
            td.Directions.RemoveAt(random);

            //to get nexttd
            if(nextposition.x < worldData.Length && nextposition.x >= 0 && nextposition.y < worldData[0].Length && nextposition.y >= 0)
            {
                //reasonal world coordinates
                TileData nexttd = worldData[(int)nextposition.x][(int)nextposition.y];
                
                //to check if the nexttd has benn found
                if (nexttd.MaterialTypes[0] == MapConstants.MaterialType.None)
                {
                    //success
                    nexttd.SwitchNext(td, -direction);
                    selectedTds[index] = nexttd;
                    return true;
                }
                else
                {
                    //failure!!
                    if(nexttd.MaterialTypes[0] != td.MaterialTypes[0])
                    {
                        //nexttd is an edge!!
                        for (int i = 1; i < 4; ++i)
                        {
                            if (nexttd.MaterialTypes[i] == MapConstants.MaterialType.None)
                            {
                                nexttd.MaterialTypes[i] = td.MaterialTypes[0];
                                nexttd.MaterialDirections[i] = direction;
                                nexttd.Directions.Remove(-direction);
                                break;
                            }
                        }
                    }
                    else
                    {
                        //nexttd is found and is the same as td
                        return DFS(selectedTds, index, worldData);
                    }
                }
            }
        }

        //all directions in td have been found
        if(td != td.fromTile)
        {
            selectedTds[index] = selectedTds[index].fromTile;
            return DFS(selectedTds, index, worldData);
        }
        else
        {
            //td is original center
            return false;
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
    
    protected List<Vector2> directions;
    protected Vector2[] materialDirections;
    protected MapConstants.MaterialType[] materialTypes;
    protected MapConstants.BuildingType buildingType;
    protected TileData center, fromTile, buildingCenter;
    protected Vector2 position, buildingPosition;
    protected Action<TileData> mtChanged;
    protected bool isRunable, isConstructable, isConstructed;

    //constructedObj
    //looseObj
}