﻿using UnityEngine;
using System.Collections.Generic;

public class GroundRandomer
{
    private GroundRandomer(int w, int h, float d)
    {
        width = w;
        height = h;
        distanceThreshold = d;

        //islandForm = new Ellipse(new Vector2(width / 2, height / 2), width / 2 - 50, height / 2 - 55);

        islandForm = FormManager.Factory((MapConstants.FormType)Random.Range(0, (int)MapConstants.FormType.None), width, height);

        GenerateGround();
    }

    public static GroundRandomer Create(int width, int height, float distanceThreshold)
    {
        if (self == null)
            self = new GroundRandomer(width, height, distanceThreshold);
        else
            Debug.Log("GroundRandomer Create Error: Attempt to create more than two Groundrandomer");

        return self;
    }

    static bool UnReasonableDistance(Vector2[] positions)
    {
        for (int i = 0; i < landformTypeAmount; ++i)
            for (int j = i + 1; j < landformTypeAmount; ++j)
                if (Vector2.Distance(positions[i], positions[j]) < distanceThreshold)
                    return true;

        return false;
    }
    
    static void Swap<T>(T[] positions, int i, int j)
    {
        T temp = positions[i];
        positions[i] = positions[j];
        positions[j] = temp;
    }

    static Vector2[] RandomSites()
    {
        Vector2[] positions = new Vector2[landformTypeAmount];
        bool conti = true;

        while (conti)
        {
            Debug.Log("random...");

            //to create positions randomly
            for (int i = 0; i < landformTypeAmount; ++i)
                positions[i] = new Vector2(Random.Range(1, width), Random.Range(1, height));

            //to check if these sites is too close
            conti = UnReasonableDistance(positions);
        }

        //to ensure that position[5] has max y value and position[6] has min y value
        if (positions[5].y < positions[6].y)
            Swap(positions, 5, 6);

        int temp = landformTypeAmount - 2;
        for (int i = 0; i < temp; ++i)
        {
            if (positions[i].y > positions[5].y)
                Swap(positions, i, 5);
            else if (positions[i].y < positions[6].y)
                Swap(positions, i, 6);
        }

        return positions;
    }

    static void GenerateGround()
    {
        Vector2[] positions = RandomSites();
        //List<TileData> DFSList = new List<TileData>(), volcanoBFSList = new List<TileData>(), snowfieldBFSList = new List<TileData>();
        Queue<TileData>[] qs = new Queue<TileData>[landformTypeAmount];


        //ground initialization
        groundData = new TileData[width][];
        for (int i = 0; i < width; ++i)
            groundData[i] = new TileData[height];
        
        //landformList initialization
        for (int i = 0; i < landformTypeAmount; ++i)
        {
            //to create the TileData in the position of groundData
            landformList[i] = new List<TileData>();
            landformList[i].Add(groundData[(int)positions[i].x][(int)positions[i].y] = TileDataManager.Factory((MapConstants.LandformType)i, positions[i]));
            qs[i] = new Queue<TileData>();
            qs[i].Enqueue(groundData[(int)positions[i].x][(int)positions[i].y]);
        }

        //DFS and BFS generator

        //to initialize DFSList and two BFSLists

        /*
        //temp is the count of landformType except for snowfield and volcano
        int temp = landformTypeAmount - 2;
        for (int i = 0; i < temp; ++i)
            DFSList.Add(groundData[(int)positions[i].x][(int)positions[i].y]);

        snowfieldBFSList.Add(groundData[(int)positions[5].x][(int)positions[5].y]);
        volcanoBFSList.Add(groundData[(int)positions[6].x][(int)positions[6].y]);
        
        
        //to generate map
        int index = 0;
        bool isDFSContinued = true, isVolcanoBFSContinued = true, isSnowfieldContinued = true;
        while (isDFSContinued || isVolcanoBFSContinued || isSnowfieldContinued)
        {
            //to do DFS

            //DFS will not execute when isDFSContinued is false
            if(isDFSContinued && (isDFSContinued = TileDataManager.DFS(DFSList, ref index, groundData, islandForm)))
                landformList[index].Add(DFSList[index]);
                
            if (++index >= DFSList.Count)
            {
                index = 0;
                
                //to do BFS
                if (isVolcanoBFSContinued && (isVolcanoBFSContinued = TileDataManager.BFS(volcanoBFSList, groundData, islandForm)))
                    landformList[5].Add(volcanoBFSList[volcanoBFSList.Count - 1]);

                if (isSnowfieldContinued && (isSnowfieldContinued = TileDataManager.BFS(snowfieldBFSList, groundData, islandForm)))
                    landformList[6].Add(snowfieldBFSList[snowfieldBFSList.Count - 1]);
            }
        }
        */

        TileDataManager.BFSearchs(qs, landformList, groundData, islandForm);
    }

    public static GroundRandomer Self
    {
        get { return self; }
    }

    public TileData[][] GroundData
    {
        get { return groundData; }
    }

    public List<TileData>[] LandformList
    {
        get { return landformList; }
    }

    static GroundRandomer self = null;
    static TileData[][] groundData;
    static List<TileData>[] landformList = new List<TileData>[MapConstants.LandformTypeAmount];
    static Form islandForm;
    static float distanceThreshold;
    static int landformTypeAmount = MapConstants.LandformTypeAmount, width, height;
}
