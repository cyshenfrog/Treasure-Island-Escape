using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    public TextAsset Image;
    public Texture2D Image2;

	// Use this for initialization
	void Start ()
    {
        Vector2 p0 = new Vector2(0f, 0f), p1 = new Vector2(1f, 1f), p2 = new Vector2(2f, 2f), p3 = new Vector2(3f, 3f);

        Edge e0 = new Edge(p0, p1), e1 = new Edge(p2, p3), e2 = new Edge(p1, p2);
        /*
        TileData2[][] world = new TileData2[2][];

        //world initialization
        for (int i = 0; i < 2; ++i)
        {
            world[i] = new TileData2[2];
            for (int j = 0; j < 2; ++j)
            {
                world[i][j] = new TileData2(new Vector2(i, j));
            }
        }

        TileData2 td = world[0][0];
        TileData2 td2 = new TileData2(new Vector2(20, 20));
        td = td2;

        //func(world[0][0]);

        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
                Debug.Log(world[i][j].Position);
            }
        }
        */
        /*
        List<TileData2> world = new List<TileData2>();

        //world initialization
        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
                world.Add(new TileData2(new Vector2(i, j)));
            }
        }

        for (int i = 0; i < 4; ++i)
        {
            Debug.Log(world[i].Position);
        }

        TileData2 td = world[0];
        TileData2 td2 = new TileData2(new Vector2(20, 20));
        world[0] = td2;

        for (int i = 0; i < 4; ++i)
        {
            Debug.Log(world[i].Position);
        }
        */
        /*
        Vertex v = new Vertex(p0, p1, p2);

        e0.p0.Add(v);
        e1.p0.Add(v);

        Debug.Log(e0.p0[0].points[0]);
        Debug.Log(e1.p0[0].points[0]);

        v.points[0] = p3;

        Debug.Log(e0.p0[0].points[0]);
        Debug.Log(e1.p0[0].points[0]);
        */
        
        Sprite s = Sprite.Create(Image2, new Rect(0, 0, 128, 128), new Vector2(.5f, .5f), 40f);
        GetComponent<SpriteRenderer>().sprite = s;
    }

    void func(TileData2 td)
    {
        td.MaterialTypes[0] = MapConstants.MaterialType.Desert;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
