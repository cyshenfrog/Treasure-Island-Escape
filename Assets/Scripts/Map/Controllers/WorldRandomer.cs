using UnityEngine;
using System.Collections.Generic;

public class WorldRandomer
{
    public WorldRandomer(int width, int height, float distanceThreshold)
    {
        this.width = width;
        this.height = height;
        this.distanceThreshold = distanceThreshold;

        GenerateWorld();
    }

    void GenerateWorld()
    {
        Vector2[] positions = RandomSites();
        world = new TileData[width][];
        List<TileData> selected = new List<TileData>();

        //world initialization
        for(int i = 0; i < width; ++i)
        {
            world[i] = new TileData[height];

            /*
            //to reduce ?
            for(int j = 0; j < heightcount; ++j)
            {
                //world[i][j] = new TileData(new Vector2(i, j));
            }
            */
        }

        //dfs generator
        //first selected are positions points
        for (int i = 0; i < materialTypeAmount; ++i)
        {
            world[(int)positions[i].x][(int)positions[i].y] = TileData.Factory((MapConstants.MaterialType)i, positions[i]);
            selected.Add(world[(int)positions[i].x][(int)positions[i].y]);
        }

        int index = 0;
        while (selected.Count != 0)
        {
            if (TileData.DFS(selected, index, world))
            {
                //success
                //out of range
                if (++index >= selected.Count)
                    index = 0;
            }
            else
            {
                //failure
                //out of range
                selected.RemoveAt(index);
                if(index >= selected.Count)
                    index = 0;
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

    public TileData[][] World
    {
        get { return world; }
    }

    TileData[][] world;
    int materialTypeAmount = (int)MapConstants.MaterialType.None, width, height;
    float distanceThreshold;
}
