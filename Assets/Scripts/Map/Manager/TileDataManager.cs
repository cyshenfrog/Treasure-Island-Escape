using UnityEngine;
using System.Collections.Generic;

public class TileDataManager
{
    public static bool DFS(List<TileData> DFSList, ref int index, TileData[][] worldData, Ellipse islandForm)
    {
        TileData td = DFSList[index];

        while (td.Directions.Count != 0)
        {
            int random = Random.Range(0, td.Directions.Count);
            Vector2 direction = td.Directions[random], nextPosition = td.Position + direction;

            //to remove the direction in td
            td.Directions.RemoveAt(random);

            //inside or randomly outside
            if ((nextPosition.x < worldData.Length && nextPosition.x >= 0 && nextPosition.y < worldData[0].Length && nextPosition.y >= 0))
            {
                //Debug.Log(islandForm.Inside(nextPosition));

                //to check if the nextTd has benn found
                if (worldData[(int)nextPosition.x][(int)nextPosition.y] == null)
                {
                    //first found
                    DFSList[index] = CreateNext(nextPosition, direction, td, worldData, true);
                    return true;
                }
                else
                {
                    //it has been found before
                    CreateEdge(direction, td, worldData[(int)nextPosition.x][(int)nextPosition.y]);
                }
            }

            /*
            //to get nexttd
            if(islandForm.Inside(nextPosition))
            {
                
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
                        
                        TileData next = worldData[(int)nextPosition.x][(int)nextPosition.y] = Factory(td.MaterialTypes[0], nextPosition, td);
                        next.MaterialDirections[0] = direction;
                        next.Directions.Remove(-direction);
                        
                        selectedDFS[index] = CreateNext(nextPosition, direction, td, worldData, true);
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
                                if (next.MaterialTypes[i] == MapConstants.LandformType.Sea)
                                {
                                    next.MaterialTypes[i] = td.MaterialTypes[0];
                                    next.MaterialDirections[i] = direction;
                                    next.Directions.Remove(-direction);
                                    break;
                                }
                            }
                        }
                        
                        CreateEdge(direction, td, worldData[(int)nextPosition.x][(int)nextPosition.y]);
                    }
                }
            }*/
        }

        //all directions in td have been found
        if (td != td.FromTile)
        {
            //back to fromTile
            DFSList[index] = DFSList[index].FromTile;
            return DFS(DFSList, ref index, worldData, islandForm);
        }
        else
        {
            //td is original center
            DFSList.RemoveAt(index);

            if (DFSList.Count != 0)
            {
                if (index >= DFSList.Count)
                    index = DFSList.Count - 1;

                return DFS(DFSList, ref index, worldData, islandForm);
            }
            else
                return false;
        }
    }

    public static bool BFS(List<TileData> BFSList, TileData[][] worldData, Ellipse islandForm)
    {
        //int times = selected.Count > maxBFSTimes ? maxBFSTimes : selected.Count;

        //while (--times >= 0)
        //{

        TileData td = BFSList[0];

        while (td.Directions.Count != 0)
        {
            Vector2 direction = td.Directions[0], nextPosition = td.Position + direction;

            //to remove the direction in td
            td.Directions.RemoveAt(0);

            //inside or randomly outside
            //if (islandForm.Inside(nextPosition) || (UnityEngine.Random.Range(0, 9) < 5 && nextPosition.x < worldData.Length && nextPosition.x >= 0 && nextPosition.y < worldData[0].Length && nextPosition.y >= 0))
            if ((nextPosition.x < worldData.Length && nextPosition.x >= 0 && nextPosition.y < worldData[0].Length && nextPosition.y >= 0))
            {
                //to check if the nexttd has benn found
                if (worldData[(int)nextPosition.x][(int)nextPosition.y] == null)
                {
                    //first found
                    BFSList.Add(CreateNext(nextPosition, direction, td, worldData, false));
                    return true;
                }
                else
                {
                    //it has been found before
                    CreateEdge(direction, td, worldData[(int)nextPosition.x][(int)nextPosition.y]);
                }
            }

            /*
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
            */
        }

        BFSList.RemoveAt(0);

        if (BFSList.Count != 0)
            return BFS(BFSList, worldData, islandForm);
        else
            return false;
        //}
    }

    public static TileData Factory(MapConstants.LandformType mt, Vector2 position, TileData fromTile = null)
    {
        switch (mt)
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
            case MapConstants.LandformType.Grassland:
                return new Grassland(position, fromTile);
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
                if (next.MaterialTypes[i] == MapConstants.LandformType.None)
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
}
