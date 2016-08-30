using UnityEngine;
using System.Collections.Generic;

public class WorldRandomer
{
    public WorldRandomer(int width, int height, float distanceThreshold)
    {
        this.width = width;
        this.height = height;
        this.distanceThreshold = distanceThreshold;

        //islandForm = new Ellipse(new Vector2(width / 2, height / 2), width / 2 - 50, height / 2 - 55);

        GenerateWorld();
    }

    void GenerateWorld()
    {
        Vector2[] positions = RandomSites();
        List<TileData> DFSList = new List<TileData>(), volcanoBFSList = new List<TileData>(), snowfieldBFSList = new List<TileData>();

        //world initialization
        worldData = new TileData[height][];
        for (int i = 0; i < height; ++i)
            worldData[i] = new TileData[width];

        //landformList initialization
        for (int i = 0; i < landformTypeAmount; ++i)
            landformList[i] = new List<TileData>();

        //DFS and BFS generator
        //to initialize DFSList and two BFSLists
        //temp is the count of landformType except for snowfield and volcano
        int temp = landformTypeAmount - 2;
        for (int i = 0; i < temp; ++i)
        {
            worldData[(int)positions[i].y][(int)positions[i].x] = TileDataManager.Factory((MapConstants.LandformType)i, positions[i]);
            DFSList.Add(worldData[(int)positions[i].y][(int)positions[i].x]); 
        }

        worldData[(int)positions[5].y][(int)positions[5].x] = TileDataManager.Factory((MapConstants.LandformType)5, positions[5]);
        snowfieldBFSList.Add(worldData[(int)positions[5].y][(int)positions[5].x]);
        worldData[(int)positions[6].y][(int)positions[6].x] = TileDataManager.Factory((MapConstants.LandformType)6, positions[6]);
        volcanoBFSList.Add(worldData[(int)positions[6].y][(int)positions[6].x]);

        //to generate map
        int index = 0;
        bool isDFSContinued = true, isVolcanoBFSContinued = true, isSnowfieldContinued = true;
        while (isDFSContinued || isVolcanoBFSContinued || isSnowfieldContinued)
        {
            //DFS will not execute when isDFSContinued is false
            if(isDFSContinued && (isDFSContinued = TileDataManager.DFS(DFSList, ref index, worldData, islandForm)))
                landformList[index].Add(DFSList[index]);
                
            if (++index >= DFSList.Count)
            {
                index = 0;

                if (isVolcanoBFSContinued && (isVolcanoBFSContinued = TileDataManager.BFS(volcanoBFSList, worldData, islandForm)))
                    landformList[5].Add(volcanoBFSList[volcanoBFSList.Count - 1]);

                if (isSnowfieldContinued && (isSnowfieldContinued = TileDataManager.BFS(snowfieldBFSList, worldData, islandForm)))
                    landformList[6].Add(snowfieldBFSList[snowfieldBFSList.Count - 1]);
            }
        }
    }

    Vector2[] RandomSites()
    {
        Vector2[] positions = new Vector2[landformTypeAmount];
        bool conti = true;

        while(conti)
        {
            Debug.Log("random...");

            //to create positions randomly
            for (int i = 0; i < landformTypeAmount; ++i)
                positions[i] = new Vector2(Random.Range(1, width), Random.Range(1, height));
            
            //to check if these sites is too close
            conti = UnReasonableDistance(positions);
        }
        
        //to ensure that position[5] has max y value and position[6] has min y value
        if(positions[5].y < positions[6].y)
            Swap(positions, 5, 6);

        int temp = landformTypeAmount - 2;
        for(int i = 0; i < temp; ++i)
        {
            if (positions[i].y > positions[5].y)
                Swap(positions, i, 5);
            else if (positions[i].y < positions[6].y)
                Swap(positions, i, 6);
        }

        return positions;
    }

    bool UnReasonableDistance(Vector2[] positions)
    {
        for (int i = 0; i < landformTypeAmount; ++i)
            for (int j = i + 1; j < landformTypeAmount; ++j)
                if (Vector2.Distance(positions[i], positions[j]) < distanceThreshold)
                    return true;

        return false;
    }

    void Swap<T>(T[] positions, int i, int j)
    {
        T temp = positions[i];
        positions[i] = positions[j];
        positions[j] = temp;
    }

    public TileData[][] WorldData
    {
        get { return worldData; }
    }

    public List<TileData>[] LandformList
    {
        get { return landformList; }
        set { landformList = value; }
    }

    // height width 
    TileData[][] worldData;
    List<TileData>[] landformList = new List<TileData>[MapConstants.LandformTypeAmount];
    Ellipse islandForm;
    int landformTypeAmount = MapConstants.LandformTypeAmount, width, height;
    float distanceThreshold;
}
