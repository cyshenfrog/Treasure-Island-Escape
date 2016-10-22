using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System;

public class ObjectDisplayController : MonoBehaviour
{
    void Awake()
    {

    }

    void Start()
    {
        cellWidthInWC = GroundController.CellWidthInWC;
        cellHeightInWC = GroundController.CellHeightInWC;
        ObjDisplay.Init();

        landformList = GroundRandomer.Self.LandformList;

        resourceList = new List<ObjDisplay>[] { bushList };

        allList = new List<ObjDisplay>[][] { resourceList };
        
        GetResourceAttributes();
        InitResourceAction();

        for (int i = 0; i < resourceAttributeCount; ++i)
        {
            int max = resourceAttributes[i].Max;
            RandomlyGenerateResource(resourceAttributes[i], 0, max);
        }
    }

    void Update()
    {

    }

    public static Color32[] ResizeCanvas(Texture2D texture, int width, int height)
    {
        var newPixels = ResizeCanvas(texture.GetPixels32(), texture.width, texture.height, width, height);
        texture.Resize(width, height);
        texture.SetPixels32(newPixels);
        texture.Apply();
        return newPixels;
    }

    private static Color32[] ResizeCanvas(IList<Color32> pixels, int oldWidth, int oldHeight, int width, int height)
    {
        var newPixels = new Color32[(width * height)];
        var wBorder = (width - oldWidth) / 2;
        var hBorder = (height - oldHeight) / 2;

        for (int r = 0; r < height; r++)
        {
            var oldR = r - hBorder;
            if (oldR < 0) { continue; }
            if (oldR >= oldHeight) { break; }

            for (int c = 0; c < width; c++)
            {
                var oldC = c - wBorder;
                if (oldC < 0) { continue; }
                if (oldC >= oldWidth) { break; }

                var oldI = oldR * oldWidth + oldC;
                var i = r * width + c;
                newPixels[i] = pixels[oldI];
            }
        }

        return newPixels;
    }

    void GetResourceAttributes()
    {
        string resourcePath = DataConstant.ResourceAttributePath;

        resourceAttributeCount = (int)ResourceType.Count;

        resourceAttributes = new ResourceAttribute[resourceAttributeCount];

        for (int i = 0; i < resourceAttributeCount; ++i)
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

                //to get their sprites
                resourceAttributes[i].Sprites = Resources.LoadAll<Sprite>(loadResourceAttributeImagePath + ((ResourceType)i).ToString());

                //to revise those sprites
                int length = resourceAttributes[i].Sprites.Length;
                Debug.Log(length);
                int width = (int)(resourceAttributes[i].Width * cellWidthInWC * 100) + 1, height = (int)(resourceAttributes[i].Height * cellHeightInWC * 100) + 1;

                for (int j = 0; j < length; ++j)
                {
                    Texture2D t = resourceAttributes[i].Sprites[j].texture;
                    ResizeCanvas(t, width, height);

                    /*
                     * 等比例??
                    float oldWidth = t.width, oldHeight = t.height;
                    float widthRatio = oldWidth / width, heightRatio = oldHeight / height;
                    Debug.Log(oldWidth + " " + oldHeight + " " + widthRatio + " " + heightRatio);
                    
                    Color32[] colors = t.GetPixels32();
                    Color32[] newColors = new Color32[width * height];
                    
                    for (int k = 0; k < height; ++k)
                    {
                        for(int l = 0; l < width; ++l)
                        {
                            try
                            {
                                newColors[k * width + l] = colors[(int)(k * heightRatio * oldWidth) + (int)(l * widthRatio)];
                            }
                            catch
                            {
                                Debug.LogError("k = " + k + " l = " + l);
                                break;
                            }
                        }
                    }
                    
                    t.Resize(width, height);
                    t.SetPixels32(newColors);
                    t.Apply();
                    */

                    resourceAttributes[i].Sprites[j] = Sprite.Create(t, new Rect(0f, 0f, width, height), pivot);
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

    //purely handmade??
    void InitResourceAction()
    {
        resourcesOnPicked = new Action<ObjData>[resourceAttributeCount];
        resourcesOnPickFinished = new Action<ObjData>[resourceAttributeCount];

        resourcesOnPicked[(int)ResourceType.Bush] = (od) =>
        {
            //to play animation??


        };

        resourcesOnPickFinished[(int)ResourceType.Bush] = (od) =>
        {
            //to drop items???

            //to destory
            allList[od.Type][od.Kind].RemoveAt(od.OID);
            //Destroy(gameObject);
        };
    }

    void RandomlyGenerateResource(ResourceAttribute ra, int mode, int randomTimes)
    {
        int max = ra.Max, landform = (int)ra.Landform;
        Vector2[] keys = landformList[landform].Keys.ToArray();
        int length = keys.Length;

        switch (mode)
        {
            case 0:
                //simply random one
                for(int i = randomTimes; i > 0; --i)
                {
                    //to get a tiledata randomly
                    TileData td = null;
                    int random = 0;
                    bool con = true;

                    while (con)
                    {
                        random = UnityEngine.Random.Range(0, length);
                        td = landformList[landform][keys[random]];
                        con = td.FixedObj != null;
                    }

                    ObjData od = ObjData.Create(ra, nextOID, td.Position);
                    GameObject newGo = Instantiate(objGameObject);
                    newGo.GetComponent<ObjDisplay>().Init(ra, od);
                    newGo.name = ra.Name + nextOID++.ToString();
                    td.FixedObj = newGo;

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

   

    public GameObject objGameObject;
    
    enum DisplayType
    {
        Resource,
        Count
    }

    Dictionary<Vector2, TileData>[] landformList;
    ResourceAttribute[] resourceAttributes;
    Action<ObjData>[] resourcesOnPicked, resourcesOnPickFinished;
    ///ResourceDisplay[]
    Vector2 pivot = .5f * Vector2.right;

    //to manager things
    List<ObjDisplay>[][] allList;
    List<ObjDisplay>[] resourceList;
    List<ObjDisplay> bushList = new List<ObjDisplay>();

    string resourceAttributePath = DataConstant.ResourceAttributePath, loadResourceAttributeImagePath = DataConstant.LoadResourceAttributeImagePath;
    float cellWidthInWC, cellHeightInWC;
    //integer overflow???
    int nextOID = 0, resourceAttributeCount, displayTypeCount = (int)DisplayType.Count;
}
