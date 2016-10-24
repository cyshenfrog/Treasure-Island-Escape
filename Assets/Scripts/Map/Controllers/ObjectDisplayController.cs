using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Collections;

public class ObjectDisplayController : MonoBehaviour
{
    void Awake()
    {

    }

    void Start()
    {
        //to get some info
        cellWidthInWC = GroundController.CellWidthInWC;
        cellHeightInWC = GroundController.CellHeightInWC;
        resourceAttributeCount = (int)ResourceType.Count;
        ObjDisplay.Init();
        landformList = GroundRandomer.Self.LandformList;
        
        //to init some info
        resourceList = new List<ObjDisplay>[resourceAttributeCount];
        for(int i = 0; i < resourceAttributeCount; ++i)
        {
            resourceList[i] = new List<ObjDisplay>();
        }
        allList = new List<ObjDisplay>[][] { resourceList };

        resourceAttributes = GetResourceAttributes();

        for (int i = 0; i < resourceAttributeCount; ++i)
        {
            int max = resourceAttributes[i].Max;
            for(int j = 0; j < max; ++j)
                GenerateResource(resourceAttributes[i]);
        }

        resourceIsGenerating = new bool[resourceAttributeCount];
        for (int i = 0; i < resourceAttributeCount; ++i)
            resourceIsGenerating[i] = false;

        allIsGenerating = new bool[][] { resourceIsGenerating };
    }

    void Update()
    {
        gameTime += Time.deltaTime;
    }

    public static Color32[] ResizeCanvas(Texture2D texture, int width, int height)
    {
        var newPixels = ResizeCanvas(texture.GetPixels32(), texture.width, texture.height, width, height);
        texture.Resize(width, height);
        texture.SetPixels32(newPixels);
        texture.Apply();
        return newPixels;
        
        /*等比例??
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

    Sprite[] GetResourceSprites(ResourceAttribute ra)
    {
        Vector2 pivot = .5f * Vector2.right;

        //to get their sprites
        Sprite[] sprites = Resources.LoadAll<Sprite>(loadResourceAttributeImagePath + ((ResourceType)ra.Kind).ToString());

        //to revise those sprites
        int length = sprites.Length;

        //to plus 1 to ensure that the value is correct
        int width = (int)(ra.Width * cellWidthInWC * 100) + 1, height = (int)(ra.Height * cellHeightInWC * 100) + 1;

        for (int i = 0; i < length; ++i)
        {
            Texture2D t = sprites[i].texture;
            ResizeCanvas(t, width, height);
            sprites[i] = Sprite.Create(t, new Rect(0f, 0f, width, height), pivot);
        }

        return sprites;
    }

    //for resource OnPicked
    void DestroyPicked(ObjData od)
    {
        //to play animation
        od.Odis.GetComponent<SpriteRenderer>().color = Color.red;
    }

    //for resource OnPickFinished
    void DestroyPickFinished(ObjData od)
    {
        //to drop items??
        
        foreach(int i in od.RA.DropItems)
        {
            //ItemManager.dropItem(((itemType)i).ToString(), 1, od.Position);
        }
        
        //to destory
        allList[od.RA.Type][od.RA.Kind].RemoveAt(od.OID);
        Destroy(od.Odis.gameObject);
        Destroy(od.Odis);

        //to delete the od??

        //to tell the deletion info to controller
        if (allIsGenerating[od.RA.Type][od.RA.Kind])
        {
            //during generating
        }
        else
        {
            //to start generating
            allIsGenerating[od.RA.Type][od.RA.Kind] = true;
            Debug.Log("start regenerate");
            StartCoroutine("ReGenerateResource", od);
        }
    }

    void RestPicked(ObjData od)
    {
        //to play animation
        od.Odis.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    void InRestPicked(ObjData od)
    {
        //to do something???
        od.Odis.GetComponent<SpriteRenderer>().color = Color.black;
    }

    void RestPickFinished(ObjData od)
    {
        //to drop items??

        //to change the state
        od.ChangeState(1);

        //to controller
        if (allIsGenerating[od.RA.Type][od.RA.Kind])
        {
            //during generating
        }
        else
        {
            //to start generating
            allIsGenerating[od.RA.Type][od.RA.Kind] = true;
            StartCoroutine("ReGenerateResource", od);
        }
    }

    void Nothing(ObjData od)
    {
    }

    //purely handmade??
    void InitResourceAction(ResourceAttribute ra)
    {
        //resourcesOnPickedList = new List<Action<ObjData>>[resourceAttributeCount];
        //resourcesOnPickFinishedList = new List<Action<ObjData>>[resourceAttributeCount];

        switch (ra.OnPickFinishedMode)
        {
            case ResourceAttribute.PickFinishedMode.Destory:
                ra.OnPickeds = new Action<ObjData>[] { DestroyPicked };
                ra.OnPickFinisheds = new Action<ObjData>[] { DestroyPickFinished };
                break;

            case ResourceAttribute.PickFinishedMode.Rest:
                ra.OnPickeds = new Action<ObjData>[] { RestPicked, InRestPicked };
                ra.OnPickFinisheds = new Action<ObjData>[] { RestPickFinished, Nothing };
                break;

            default:
#if UNITY_EDITOR
                Debug.LogError("ResourceAttribute Action Error");
#endif
                break;
        }
    }

    ResourceAttribute[] GetResourceAttributes()
    {
        string resourcePath = DataConstant.ResourceAttributePath;

        ResourceAttribute[] ras = new ResourceAttribute[resourceAttributeCount];

        //to get the resource attribute file
        for (int i = 0; i < resourceAttributeCount; ++i)
        {
            string file = resourceAttributePath + ((ResourceType)i).ToString() + ".xml";

            if (File.Exists(file))
            {
                //to read the file
                var serializer = new XmlSerializer(typeof(ResourceAttribute));

                using (var stream = new FileStream(file, FileMode.Open))
                {
                    ras[i] = (ResourceAttribute)serializer.Deserialize(stream);
                }

                ras[i].Sprites = GetResourceSprites(ras[i]);
                InitResourceAction(ras[i]);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("The file name " + ((ResourceType)i).ToString() + " is not existed");
#endif
                break;
            }
        }

        return ras;
    }

    IEnumerator ReGenerateResource(ObjData od)
    {
        ResourceAttribute ra = od.RA;
        Debug.Log("wait growthtime");
        yield return new WaitForSeconds(ra.GrowthTime);

        switch(ra.OnPickFinishedMode)
        {
            case ResourceAttribute.PickFinishedMode.Destory:
                Debug.Log(ra.Kind.ToString() + " " + allList[ra.Type][ra.Kind].Count + " " + ra.Max);
                while (allList[ra.Type][ra.Kind].Count < ra.Max)
                {
                    GenerateResource(ra);
                    Debug.Log("re is completed");
                    yield return ra.GrowthTime;
                }
                Debug.Log("regenerate end");
                break;

            case ResourceAttribute.PickFinishedMode.Rest:
                od.ChangeState(0);
                break;

            default:
#if UNITY_EDITOR
                Debug.LogError("ReGenerateResource OnPickFinishedMode unexpected");
#endif
                break;
        }

        //to stop ReGenerateResource
        allIsGenerating[ra.Type][ra.Kind] = false;
    }

    void GenerateResource(ResourceAttribute ra)
    {
        int max = ra.Max, landform = (int)ra.Landform;
        ResourceAttribute.GenerateMode gm = ra.Gm;
        Vector2[] keys = landformList[landform].Keys.ToArray();
        int length = keys.Length;

        switch (gm)
        {
            case ResourceAttribute.GenerateMode.Single:

                //to get a tiledata
                TileData td = null;
                int random = 0;
                bool con = true;

                while (con)
                {
                    random = UnityEngine.Random.Range(0, length);
                    td = landformList[landform][keys[random]];
                    con = td.FixedObj != null;
                }

                int id = resourceList[ra.Kind].Count;
                ObjData od = ObjData.Create(ra, id, td.Position);
                ObjDisplay odis = Instantiate(objGameObject).GetComponent<ObjDisplay>();
                odis.Init(od);
                odis.Role = Role;
                odis.transform.name = ra.Name + id.ToString();
                resourceList[ra.Kind].Add(odis);
                td.FixedObj = odis;

                //to refresh
                //GroundController.GetMapPoolByTileData(td).GetComponent<TileEnableAction>().OnEnable();

                //if many rd once??
                
                break;

            default:
                Debug.LogError("RandomlyGenerate Error: Unexpected mode");
                break;
        }
    }

   

    public GameObject objGameObject;
    public RoleController Role;
    
    enum DisplayType
    {
        Resource,
        Count
    }

    Dictionary<Vector2, TileData>[] landformList;
    ResourceAttribute[] resourceAttributes;
    //List<Action<ObjData>>[] resourcesOnPickedList, resourcesOnPickFinishedList;
    ///ResourceDisplay[]

    //to manager things
    List<ObjDisplay>[][] allList;
    List<ObjDisplay>[] resourceList;
    //List<ObjDisplay> bushList = new List<ObjDisplay>();


    bool[][] allIsGenerating;
    bool[] resourceIsGenerating;
    string resourceAttributePath = DataConstant.ResourceAttributePath, loadResourceAttributeImagePath = DataConstant.LoadResourceAttributeImagePath;
    double gameTime = 0d;
    float cellWidthInWC, cellHeightInWC;
    //integer overflow???
    int nextOID = 0, resourceAttributeCount, displayTypeCount = (int)DisplayType.Count;
}
