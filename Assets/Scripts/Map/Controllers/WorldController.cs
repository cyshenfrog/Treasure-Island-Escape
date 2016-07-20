using UnityEngine;
using System.Collections;

public class Tile
{
    TileData data;
    GameObject go;

    public Tile(TileData data, GameObject go)
    {
        this.data = data;
        this.go = go;
    }
}

public class 

public class Ellipse
{
    Vector2 center;
    int sid;
    float a, b, a2, b2;

    Vector3 rotate;
    World world;
    MapConstants.MaterialType mt;

    public Ellipse(Vector2 center, float a, float b, MapConstants.MaterialType mt, int sid)
    {
        Debug.Log("YO");
        this.center = center;
        this.a = a;
        this.b = b;
        this.mt = mt;
        this.sid = sid;
        
        a2 = a * a;
        b2 = b * b;
    }

    public void FillToWorlid(World world)
    {
        this.world = world;
        //Matrix4x4 ro = Matrix4x4.Scale(new Vector3(20f, 0f));
        //Vector3 v = ro * Vector3.one;

        //BFS
        Queue q = new Queue();
        TileData data = world.GetTileDataAt(center);
        data.SearchID = sid;
        data.MaterialType = mt;
        q.Enqueue(data);

        while (q.Count != 0)
        {
            data = (TileData)q.Dequeue();
            
            for(int i = 0; i < 4; ++i)
            {
                TileData td = world.GetTileDataAt(data.Po+ MapConstants.bfs[i]);
                if(td != null)
                {
                    float x = td.X - center.x, y = td.Y - center.y;
                    if (td.SearchID != sid)
                    {
                        if ((x * x) / a2 + (y * y) / b2 < 1)
                        {
                            //has not found yet
                            td.SearchID = sid;
                            data.MaterialType = mt;
                            q.Enqueue(td);
                        }
                        else
                        {
                            //outside
                            
                        }
                    }
                }
            }
        }
    }
}

public class WorldController : MonoBehaviour
{
    World world;
    Tile[,] tiles;

    public Sprite BaseSprite, BuildingSprite;
    
	void Start ()
    {
        //to create a world with all sea tiledata
        world = new World();
        int ww = world.Width, wh = world.Height;

        //to change the tiledata
        Ellipse ep = new Ellipse(new Vector2(Random.Range(40, 60), Random.Range(40, 60)), Random.Range(15, 20), Random.Range(10, 15), MapConstants.MaterialType.Forest, 1);
        ep.FillToWorlid(world);
        

        //to create a gameobject for each of our tiles, so they show visually.
        tiles = new Tile[ww, wh];
        for (int x = 0; x < ww; ++x)
        {
            for(int y = 0; y < wh; ++y)
            {
                TileData data = world.GetTileDataAt(x, y);
                
                GameObject go = new GameObject();
                go.name = "Tile_" + data.X + "_" + data.Y;
                go.transform.localPosition = new Vector3(data.X * 3, data.Y * 3);
                go.AddComponent<SpriteRenderer>();

                //first use
                OnMtChanged(data, go);

                //to add action
                data.AddMtChanged((tile) => { OnMtChanged(tile, go); });

                tiles[x, y] = new Tile(data, go);
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    //????
    public void OnMtChanged(TileData data, GameObject go)
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();

        sr.sprite = BaseSprite;
        sr.color = MapConstants.materialColors[(int)data.MaterialType];
    }
}
