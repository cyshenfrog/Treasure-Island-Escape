using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;



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

    void GetResourceAttributes()
    {
        string resourcePath = DataConstant.ResourcePath;

        int count = (int)ResourceType.Count;

        resourceAttributes = new ResourceAttribute[count];

        for(int i = 0; i < count; ++i)
        {
            string file = resourceAttributePath + "ResourceAttribute_" + ((ResourceType)i).ToString() + ".xml";

            if (File.Exists(file))
            {
                //to read the file
                var serializer = new XmlSerializer(typeof(ResourceAttribute));

                using (var stream = new FileStream(file, FileMode.Open))
                {
                    resourceAttributes[i] = (ResourceAttribute)serializer.Deserialize(stream);
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("");
#endif
            }
        }
    }

    Dictionary<Vector2, TileData>[] landformList;
    ResourceAttribute[] resourceAttributes;

    string resourceAttributePath = DataConstant.ResourcePath;
}
