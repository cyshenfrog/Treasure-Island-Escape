using UnityEngine;
using System.Collections.Generic;
using System;

public static class TileDataManager
{
    /*
    public static bool DFS(List<TileData> DFSList, ref int index, TileData[][] groundData, Form islandForm)
    {
        TileData td = DFSList[index];

        while (td.Directions.Count != 0)
        {
            int random = UnityEngine.Random.Range(0, td.Directions.Count);
            Vector2 direction = td.Directions[random], nextPosition = td.Position + direction;

            //to remove the direction in td
            td.Directions.RemoveAt(random);

            //inside or randomly outside
            if ((nextPosition.x < groundData.Length && nextPosition.x >= 0 && nextPosition.y < groundData[0].Length && nextPosition.y >= 0))
            {
                //Debug.Log(islandForm.Inside(nextPosition));

                //to check if the nextTd has benn found
                if (groundData[(int)nextPosition.x][(int)nextPosition.y] == null)
                {
                    //first found
                    DFSList[index] = CreateNext(nextPosition, direction, td, groundData, true);
                    return true;
                }
                else
                {
                    //it has been found before
                    CreateEdge(direction, td, groundData[(int)nextPosition.x][(int)nextPosition.y]);
                }
            }

            /////
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
            }////
        }

        //all directions in td have been found
        if (td != td.FromTile)
        {
            //back to fromTile
            DFSList[index] = DFSList[index].FromTile;
            return DFS(DFSList, ref index, groundData, islandForm);
        }
        else
        {
            //td is original center
            DFSList.RemoveAt(index);

            if (DFSList.Count != 0)
            {
                if (index >= DFSList.Count)
                    index = DFSList.Count - 1;

                return DFS(DFSList, ref index, groundData, islandForm);
            }
            else
                return false;
        }
    }
    */

    public static int Choice(int direction)
    {
        int p0 = direction, p1, p2, p3, p4;

        switch(direction)
        {
            case (int)MapConstants.Directions.TopLeft:
                p1 = (int)MapConstants.Directions.Top;
                p2 = (int)MapConstants.Directions.Left;
                p3 = (int)MapConstants.Directions.TopRight;
                p4 = (int)MapConstants.Directions.BottomLeft;
                break;

            case (int)MapConstants.Directions.Top:
                p1 = (int)MapConstants.Directions.TopLeft;
                p2 = (int)MapConstants.Directions.TopRight;
                p3 = p0;
                p4 = p0;
                break;

            case (int)MapConstants.Directions.TopRight:
                p1 = (int)MapConstants.Directions.Top;
                p2 = (int)MapConstants.Directions.Right;
                p3 = (int)MapConstants.Directions.TopLeft;
                p4 = (int)MapConstants.Directions.BottomRight;
                break;

            case (int)MapConstants.Directions.Left:
                p1 = (int)MapConstants.Directions.TopLeft;
                p2 = (int)MapConstants.Directions.BottomLeft;
                p3 = p0;
                p4 = p0;
                break;

            case (int)MapConstants.Directions.Right:
                p1 = (int)MapConstants.Directions.TopRight;
                p2 = (int)MapConstants.Directions.BottomRight;
                p3 = p0;
                p4 = p0;
                break;

            case (int)MapConstants.Directions.BottomLeft:
                p1 = (int)MapConstants.Directions.Bottom;
                p2 = (int)MapConstants.Directions.Left;
                p3 = (int)MapConstants.Directions.TopLeft;
                p4 = (int)MapConstants.Directions.BottomRight;
                break;

            case (int)MapConstants.Directions.Bottom:
                p1 = (int)MapConstants.Directions.BottomLeft;
                p2 = (int)MapConstants.Directions.BottomRight;
                p3 = p0;
                p4 = p0;
                break;

            case (int)MapConstants.Directions.BottomRight:
                p1 = (int)MapConstants.Directions.Bottom;
                p2 = (int)MapConstants.Directions.Right;
                p3 = (int)MapConstants.Directions.TopRight;
                p4 = (int)MapConstants.Directions.BottomLeft;
                break;

            default:
                p1 = p0;
                p2 = p0;
                p3 = p0;
                p4 = p0;
                break;
        }

        int random = UnityEngine.Random.Range(0, 9);

        switch(random)
        {
            case 0:
            case 5:
            case 8:
                return p0;

            case 1:
            case 6:
                return p1;

            case 2:
            case 7:
                return p2;

            case 3:
                return p3;

            case 4:
                return p4;

            default:
                return p0;
        }
    }

    public static bool BFS(Queue<TileData> q, List<TileData> landformList, TileData[][] groundData, Form islandForm, ref int stack, int last)
    {
        int count = q.Count;
        List<TileData> success = new List<TileData>();

        while (count-- > 0)
        {
            TileData td = q.Dequeue();

            for (int i = 0; i < 8; ++i)
            {
                Vector2 direction = td.Directions[i], nextPosition = td.Position + direction;

                //if (islandForm.Inside(nextPosition) || (UnityEngine.Random.Range(0, 9) < 5 && nextPosition.x < worldData.Length && nextPosition.x >= 0 && nextPosition.y < worldData[0].Length && nextPosition.y >= 0))
                //if (nextPosition.x < groundData.Length && nextPosition.x >= 0 && nextPosition.y < groundData[0].Length && nextPosition.y >= 0)
                try
                {
                    if (groundData[(int)nextPosition.x][(int)nextPosition.y] == null)
                    {
                        //first found
                        TileData next = CreateNext(nextPosition, direction, td, groundData, false);

                        landformList.Add(next);
                        success.Add(next);
                    }
                    else
                    {
                        //it has been found before
                        CreateEdge(direction, td, groundData[(int)nextPosition.x][(int)nextPosition.y]);
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.Log("BFS Error: " + e);
                }
            }
        }

        /*
        //mode 2: random pick many point
        int random;
        int c = success.Count >= 3 ? 3 : success.Count;

        for (int i = c; i >= 0; --i)
        {
            random = UnityEngine.Random.Range(0, i);
            q.Enqueue(success[random]);
            success.RemoveAt(random);
        }
        */

        //mode 3: 
        int random;
        int c = success.Count >= 3 ? 3 : success.Count;

        for (int i = c; i >= 0; --i)
        {
            random = UnityEngine.Random.Range(0, i);
            q.Enqueue(success[random]);
            success.RemoveAt(random);
        }

        return !(++stack >= maxStack || q.Count == 0);
    }
    
    //nowbfs
    public static bool BFS(int index, Queue<TileData> q, Dictionary<Vector2, TileData> dLandformList, TileData[][] groundData, ref Form islandForm, ref int stack)
    {
        int count = q.Count;
        //Debug.Log(count);
        //List<TileData> success = new List<TileData>();

        while (count-- > 0)
        {
            TileData td = q.Dequeue();

            if (td.MaterialTypes[0] == MapConstants.LandformType.Sea)
            {
                td.MaterialTypes[0] = (MapConstants.LandformType)index;
                //Debug.Log("index = " + index);
                dLandformList.Add(td.Position, td);
                dSeaList.Remove(td.Position);

                for (int i = 0; i < 8; ++i)
                {
                    Vector2 direction = td.Directions[i], sidePosition = td.Position + direction, nextPosition = sidePosition + direction;

                    //if (nextPosition.x < groundData.Length && nextPosition.x >= 0 && nextPosition.y < groundData[0].Length && nextPosition.y >= 0)
                    //if (islandForm.Inside(sidePosition) || (UnityEngine.Random.Range(0, 9) < 5))
                    if (islandForm.Inside(sidePosition))
                    {
                        try
                        {
                            if (groundData[(int)sidePosition.x][(int)sidePosition.y].MaterialTypes[0] == MapConstants.LandformType.Sea)
                            {
                                //first found
                                //to ignore that the nextPosition could be out of range
                                TileData side = groundData[(int)sidePosition.x][(int)sidePosition.y];
                                side.MaterialTypes[0] = (MapConstants.LandformType)index;
                                side.MaterialDirections[0] = direction;
                                dLandformList.Add(side.Position, side);
                                dSeaList.Remove(side.Position);
                                //CreateSide(sidePosition, direction, td, groundData, false);
                            }
                            else
                            {
                                //it has been found before
                                CreateEdge(direction, td, groundData[(int)sidePosition.x][(int)sidePosition.y]);
                            }

                            if (groundData[(int)nextPosition.x][(int)nextPosition.y].MaterialTypes[0] == MapConstants.LandformType.Sea && islandForm.Inside(nextPosition))
                            {
                                TileData next = groundData[(int)nextPosition.x][(int)nextPosition.y];

                                //mode 0: normal
                                q.Enqueue(next);

                                //success.Add(next);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log("BFS Error: " + e);
                        }
                    }
                }
            }
        }



        //mode 1: random pick one point
        //if(success.Count != 0)
        //q.Enqueue(success[UnityEngine.Random.Range(0, success.Count)]);


        //mode 2: random pick many point
        /*
        if (success.Count >= 2)
        {
            int random = UnityEngine.Random.Range(0, success.Count);
            q.Enqueue(success[random]);
            success.RemoveAt(random);
            random = UnityEngine.Random.Range(0, success.Count);
            q.Enqueue(success[random]);
            success.RemoveAt(random);
        }
        */

        //mode 3: 

        return ++stack < maxStack && q.Count != 0;
    }

    public static void BFSearchs(Queue<TileData>[] qs, Dictionary<Vector2, TileData>[] dLandformsList, TileData[][] groundData, Form islandForm)
    {
        int randomLandformTypeAmount = MapConstants.LandformTypeAmount - 1;
        int[] stacks = new int[randomLandformTypeAmount];
        bool[] flags = new bool[randomLandformTypeAmount];
        int flagsCount = 0;
        maxStack = MapConstants.MaxStack;

        dSeaList = dLandformsList[randomLandformTypeAmount];

        for (int i = 0; i < randomLandformTypeAmount; ++i)
        {
            stacks[i] = 1;
            flags[i] = false;
        }


        int index = 0;
        while (true)
        {
            //to do bfs
            if (!BFS(index, qs[index], dLandformsList[index], groundData, ref islandForm, ref stacks[index]))
            {
                flags[index] = true;

                if (++flagsCount >= randomLandformTypeAmount)
                {
                    //finished
                    return;
                }
            }

            //Debug.Log(flags[0]);

            //next turn
            while (true)
            {
                index = (++index) % randomLandformTypeAmount;
                //Debug.Log(index);
                if (!flags[index])
                    break;
            }
        }
    }

    /*
    public static bool BFSs(List<TileData> BFSList, TileData[][] groundData, Form islandForm)
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
            if (nextPosition.x < groundData.Length && nextPosition.x >= 0 && nextPosition.y < groundData[0].Length && nextPosition.y >= 0)
            {
                //to check if the nexttd has benn found
                if (groundData[(int)nextPosition.x][(int)nextPosition.y] == null)
                {
                    //first found
                    BFSList.Add(CreateNext(nextPosition, direction, td, groundData, false));
                    return true;
                }
                else
                {
                    //it has been found before
                    CreateEdge(direction, td, groundData[(int)nextPosition.x][(int)nextPosition.y]);
                }
            }

            /////
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
            /////
        }

        BFSList.RemoveAt(0);

        return false;
        ///
        if (BFSList.Count != 0)
            return BFS(BFSList, groundData, islandForm);
        else
            return false;
            ////
        //}
    }
    */

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
                    //next.Directions.Remove(-direction);
                    return;
                }
    }

    static TileData CreateSide(Vector2 sidePosition, Vector2 direction, TileData td, TileData[][] groundData, bool DFS)
    {
        //TileData next = groundData[(int)nextPosition.x][(int)nextPosition.y] = Factory(td.MaterialTypes[0], nextPosition, DFS ? td : null);
        TileData side = groundData[(int)sidePosition.x][(int)sidePosition.y];
        side.MaterialTypes[0] = td.MaterialTypes[0];
        side.MaterialDirections[0] = direction;
        //next.Directions.Remove(-direction);
        return side;
    }

    static TileData CreateNext(Vector2 nextPosition, Vector2 direction, TileData td, TileData[][] groundData, bool DFS)
    {
        //TileData next = groundData[(int)nextPosition.x][(int)nextPosition.y] = Factory(td.MaterialTypes[0], nextPosition, DFS ? td : null);
        TileData next = groundData[(int)nextPosition.x][(int)nextPosition.y];
        next.MaterialTypes[0] = td.MaterialTypes[0];
        next.MaterialDirections[0] = direction;
        //next.Directions.Remove(-direction);
        return next;
    }

    static Dictionary<Vector2, TileData> dSeaList;
    static List<TileData> seaList;
    static int maxStack;
}
