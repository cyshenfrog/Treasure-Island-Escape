using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectDisplayController : MonoBehaviour
{
    void Awake()
    {

    }

    void Start()
    {
        landformList = GroundRandomer.Self.LandformList;
    }

    void Update()
    {

    }

    void RandomlyGenerate(int mode, int randomTimes, ResourceAttribute rd)
    {
        int max = rd.Max, landform = (int)rd.Landform;
        Vector2[] keys = landformList[landform].Keys.ToArray();
        int length = keys.Length;

        switch (mode)
        {
            case 0:
                //simply random
                for(int i = randomTimes; i > 0; --i)
                {
                    //to get a tiledata randomly
                    int random = Random.Range(0, length);
                    TileData td = landformList[landform][keys[random]];

                    td.RdList.Add(rd);

                    //to refresh
                    GroundController.GetMapPoolByTileData(td).GetComponent<TileEnableAction>().OnEnable();

                    //if many rd once??
                }
                
                break;
            default:
                Debug.LogError("RandomlyGenerate Error: Unexpected mode");
                break;
        }
    }

    Dictionary<Vector2, TileData>[] landformList;
}
