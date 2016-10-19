using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;



public class ObjectDisplayController : MonoBehaviour
{
    void Awake()
    {
        GetResourceAttributes();
        
        for(int i = 0; i < resourceAttributeCount; ++i)
        {
            int max = resourceAttributes[i].Max;
            RandomlyGenerateResource(resourceAttributes[i], 0, max);
        }


    }

    void Start()
    {
        landformList = GroundRandomer.Self.LandformList;
    }

    void Update()
    {

    }

    void RandomlyGenerateResource(ResourceAttribute ra, int mode, int randomTimes)
    {
        int max = ra.Max, landform = (int)ra.Landform;
        Vector2[] keys = landformList[landform].Keys.ToArray();
        int length = keys.Length;

        switch (mode)
        {
            case 0:
                //simply random
                for(int i = randomTimes; i > 0; --i)
                {
                    //to get a tiledata randomly
                    TileData td = null;
                    int random = 0;
                    bool con = true;

                    while (con)
                    {
                        random = Random.Range(0, length);
                        td = landformList[landform][keys[random]];
                        con = td.FixedObj != null;
                    }

                    ObjData od = ObjData.Create(ra, nextOID++, td.Position);
                    ObjDisplay display = new ObjDisplay(ra, od);
                    td.FixedObj = display;

                    //to refresh
                    //GroundController.GetMapPoolByTileData(td).GetComponent<TileEnableAction>().OnEnable();

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
        string resourcePath = DataConstant.ResourceAttributePath;

        resourceAttributeCount = (int)ResourceType.Count;

        resourceAttributes = new ResourceAttribute[resourceAttributeCount];

        for(int i = 0; i < resourceAttributeCount; ++i)
        {
            string file = resourceAttributePath + ((ResourceType)i).ToString() + ".xml";

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
                Debug.LogError("The file name " + ((ResourceType)i).ToString() + " is not existed");
#endif
                break;
            }
        }
    }

    Dictionary<Vector2, TileData>[] landformList;
    ResourceAttribute[] resourceAttributes;
    ///ResourceDisplay[]

    string resourceAttributePath = DataConstant.ResourceAttributePath;
    //integer overflow???
    int nextOID = 0, resourceAttributeCount;
}
