using UnityEngine;
using System.Collections.Generic;

public class TileData2
{
    public TileData2(Vector2 position)
    {
        this.position = position;
        center = this;
        fromtile = this;

        materialtypes = new MapConstants.MaterialType[] { MapConstants.MaterialType.None, MapConstants.MaterialType.None, MapConstants.MaterialType.None, MapConstants.MaterialType.None };
        materialdirections = new Vector2[4];

        directions = new List<Vector2>();
        directions.Add(Vector2.up);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        directions.Add(Vector2.right);
    }

    public void SwitchNext(TileData2 last, Vector2 fromdirection)
    {
        center = last.center;
        fromtile = last;

        materialtypes[0] = last.MaterialTypes[0];
        materialdirections[0] = -fromdirection;

        directions.Remove(fromdirection);
    }

    public MapConstants.MaterialType[] MaterialTypes
    {
        get { return materialtypes; }
        set { materialtypes = value; }
    }

    public Vector2[] MaterialDirections
    {
        get { return materialdirections; }
        set { materialdirections = value; }
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

    public TileData2 Center
    {
        get { return center; }
    }

    public static bool DFS(List<TileData2> selectedtds, int index, TileData2[][] world)
    {
        TileData2 td = selectedtds[index];

        while (td.Directions.Count != 0)
        {
            int random = Random.Range(0, td.Directions.Count);
            Vector2 direction = td.Directions[random], nextposition = td.Position + direction;

            //to remove the direction in td
            td.Directions.RemoveAt(random);

            //to get nexttd
            if(nextposition.x < world.Length && nextposition.x >= 0 && nextposition.y < world[0].Length && nextposition.y >= 0)
            {
                //reasonal world coordinates
                TileData2 nexttd = world[(int)nextposition.x][(int)nextposition.y];
                
                //to check if the nexttd has benn found
                if (nexttd.MaterialTypes[0] == MapConstants.MaterialType.None)
                {
                    //success
                    nexttd.SwitchNext(td, -direction);
                    selectedtds[index] = nexttd;
                    return true;
                }
                else
                {
                    //failure!! nexttd is an edge!
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
            }
            else
            {
                //non reasonal world coordinates (out of boundary)
                td.Directions.Remove(direction);
            }
        }

        //all directions in td have been found

        if(td != td.fromtile)
        {
            selectedtds[index] = selectedtds[index].fromtile;
            return DFS(selectedtds, index, world);
        }
        else
        {
            return false;
        }
    }
    

    List<Vector2> directions;
    Vector2[] materialdirections;
    MapConstants.MaterialType[] materialtypes;
    TileData2 center, fromtile;
    Vector2 position;
}
