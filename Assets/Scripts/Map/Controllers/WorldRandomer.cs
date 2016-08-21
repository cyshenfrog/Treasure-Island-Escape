using UnityEngine;
using System.Collections.Generic;

public class WorldRandomer
{
    public WorldRandomer(int width, int height, float distanceThreshold)
    {
        this.width = width;
        this.height = height;
        this.distanceThreshold = distanceThreshold;

        islandForm = new Ellipse(new Vector2(width / 2, height / 2), width / 2 - 50, height / 2 - 55);

        GenerateWorld();
    }

    void GenerateWorld()
    {
        Vector2[] positions = RandomSites();
        worldData = new TileData[width][];
        List<TileData> selected = new List<TileData>(), Volcanos = new List<TileData>(), Snowfields = new List<TileData>();

        //world initialization
        for(int i = 0; i < width; ++i)
            worldData[i] = new TileData[height];

        //DFS and BFS generator
        //first selected are positions points
        for (int i = 2; i < materialTypeAmount; ++i)
        {
            worldData[(int)positions[i].x][(int)positions[i].y] = TileData.Factory((MapConstants.LandformType)i, positions[i]);
            selected.Add(worldData[(int)positions[i].x][(int)positions[i].y]);
        }

        worldData[(int)positions[0].x][(int)positions[0].y] = TileData.Factory(0, positions[0]);
        Volcanos.Add(worldData[(int)positions[0].x][(int)positions[0].y]);
        worldData[(int)positions[1].x][(int)positions[1].y] = TileData.Factory((MapConstants.LandformType)1, positions[1]);
        Snowfields.Add(worldData[(int)positions[1].x][(int)positions[1].y]);

        int index = 0;
        while (selected.Count != 0)
        {
            if (!TileData.DFS(selected, index, worldData, islandForm))
            {
                //failure
                selected.RemoveAt(index);
            }

            if (++index >= selected.Count)
            {
                index = 0;
                TileData.BFS(Volcanos, worldData, islandForm);
                TileData.BFS(Snowfields, worldData, islandForm);
            }
        }
    }

    Vector2[] RandomSites()
    {
        Vector2[] positions = new Vector2[materialTypeAmount];
        bool conti = true;

        while(conti)
        {
            Debug.Log("random...");
            //to create sites randomly
            for (int i = 0; i < materialTypeAmount; ++i)
                positions[i] = new Vector2(Random.Range(1, width), Random.Range(1, height));
            
            //to check if these sites is too close
            conti = UnReasonableDistance(positions);
        }
        
        if(positions[0].y < positions[1].y)
            Swap(positions, 0, 1);

        for(int i = 2; i < materialTypeAmount; ++i)
        {
            if (positions[i].y > positions[0].y)
                Swap(positions, i, 0);
            else if (positions[i].y < positions[1].y)
                Swap(positions, i, 1);
        }

        return positions;
    }

    bool UnReasonableDistance(Vector2[] positions)
    {
        for (int i = 0; i < materialTypeAmount; ++i)
            for (int j = i + 1; j < materialTypeAmount; ++j)
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

    TileData[][] worldData;
    Ellipse islandForm;
    int materialTypeAmount = (int)MapConstants.LandformType.Sea, width, height;
    float distanceThreshold;
}
