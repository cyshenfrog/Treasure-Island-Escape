using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldRandomer : MonoBehaviour
{
    public int Width, Height;
    public float DistanceThreshold;

    void Awake()
    {
        List<Vector2> centers = RandomSites();

        //dfs generator
        


    }

    List<Vector2> RandomSites()
    {
        List<Vector2> centers = new List<Vector2>();
        bool conti = true;

        while(conti)
        {
            //to create sites randomly
            centers.Clear();
            for (int i = 0; i < 5; ++i)
            {
                centers.Add(new Vector2(Random.Range(0, Width), Random.Range(0, Height)));
            }
            
            //to check if these sites is too close
            conti = ReasonalDistance(centers);
        }

        return centers;
    }

    bool ReasonalDistance(List<Vector2> centers)
    {
        for (int i = 0; i < 5; ++i)
        {
            for (int j = i + 1; j < 5; ++j)
            {
                if (Vector2.Distance(centers[i], centers[j]) < DistanceThreshold)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
