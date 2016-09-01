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

public class WorldController1 : MonoBehaviour
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
        //Ellipse ep = new Ellipse(new Vector2(Random.Range(40, 60), Random.Range(40, 60)), Random.Range(15, 20), Random.Range(10, 15), MapConstants.LandformType.Forest, 1);
        //ep.FillToWorlid(world);
        

        //to create a gameobject for each of our tiles, so they show visually.
        tiles = new Tile[ww, wh];
        for (int x = 0; x < ww; ++x)
        {
            for(int y = 0; y < wh; ++y)
            {
                TileData data = world.GetTileDataAt(x, y);
                
                GameObject go = new GameObject();
                //go.name = "Tile_" + data.X + "_" + data.Y;
                //go.transform.localPosition = new Vector3(data.X * 3, data.Y * 3);
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
        //sr.color = MapConstants.materialColors[(int)data.MaterialType];
    }
}
