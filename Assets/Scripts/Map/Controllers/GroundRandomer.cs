using UnityEngine;
using System.Collections.Generic;

public class GroundRandomer
{
    private GroundRandomer(int w, int h, float d)
    {
        width = w;
        height = h;
        distanceThreshold = d;

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
        for (int i = 0; i < randomLandformTypeAmount; ++i)
            for (int j = i + 1; j < randomLandformTypeAmount; ++j)
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
        Vector2[] positions = new Vector2[randomLandformTypeAmount];
        bool conti = true;

        while (conti)
        {
            Debug.Log("random...");

            //to create positions randomly
            Vector2 v;
            for (int i = 0; i < randomLandformTypeAmount; ++i)
            {
                while (!islandForm.Inside(v = new Vector2(Random.Range(1, width), Random.Range(1, height)))) ;
                positions[i] = v;
            }

            //to check if these sites is too close
            conti = UnReasonableDistance(positions);
        }

        //to ensure that position[5] has max y value and position[6] has min y value
        if (positions[4].y < positions[5].y)
            Swap(positions, 4, 5);

        int temp = randomLandformTypeAmount - 2;
        for (int i = 0; i < temp; ++i)
        {
            if (positions[i].y > positions[4].y)
                Swap(positions, i, 4);
            else if (positions[i].y < positions[5].y)
                Swap(positions, i, 5);
        }

        return positions;
    }

    static void GenerateGround()
    {
        Vector2[] positions = RandomSites();
        //List<TileData> DFSList = new List<TileData>(), volcanoBFSList = new List<TileData>(), snowfieldBFSList = new List<TileData>();
        Queue<TileData>[] qs = new Queue<TileData>[randomLandformTypeAmount];

        for (int i = 0; i < positions.Length; ++i)
            Debug.Log(positions[i]);
        
        //ground initialization
        groundData = new TileData[width][];
        int sea = (int)MapConstants.LandformType.Sea;
        landformList[sea] = new Dictionary<Vector2, TileData>();
        Vector2 v;
        for (int i = 0; i < width; ++i)
        {
            groundData[i] = new TileData[height];
            for (int j = 0; j < height; ++j)
                landformList[sea].Add(v = new Vector2(i, j), groundData[i][j] = new TileData((MapConstants.LandformType)sea, v));
        }
        

        /*
        //ground initialization
        groundData = new TileData[width][];
        for (int i = 0; i < width; ++i)
            groundData[i] = new TileData[height];
        */

        
        //landformList initialization
        TileData td;
        for (int i = 0; i < randomLandformTypeAmount; ++i)
        {
            //to create the TileData in the position of groundData
            landformList[i] = new Dictionary<Vector2, TileData>();
            qs[i] = new Queue<TileData>();
            qs[i].Enqueue(groundData[(int)positions[i].x][(int)positions[i].y]);

            //td.MaterialTypes[0] = (MapConstants.LandformType)i;
        }

        Debug.Log(islandForm.Center);

        TileDataManager.BFSearchs(qs, landformList, groundData, islandForm);
        

        /*
        //DFS and BFS generator

        //to initialize DFSList and two BFSLists

        
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
    }

    public static GroundRandomer Self
    {
        get { return self; }
    }

    public TileData[][] GroundData
    {
        get { return groundData; }
    }

    public Dictionary<Vector2, TileData>[] LandformList
    {
        get { return landformList; }
    }

    static GroundRandomer self = null;
    static TileData[][] groundData;
    static Dictionary<Vector2, TileData>[] landformList = new Dictionary<Vector2, TileData>[MapConstants.LandformTypeAmount];
    static Form islandForm;
    static float distanceThreshold;
    static int landformTypeAmount = MapConstants.LandformTypeAmount, randomLandformTypeAmount = landformTypeAmount - 1, width, height;
}
