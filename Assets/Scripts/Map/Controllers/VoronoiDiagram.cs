using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class VoronoiDiagram : MonoBehaviour
{
    public int PolygonNumber, WorldWidth, WorldHeight;

	// Use this for initialization
	void Start ()
    {
        List<Site> sites = RandomSites();

        /*
        Texture2D tt = new Texture2D(512, 512);

        for (int x = 0; x < WorldWidth; ++x)
            for (int y = 0; y < WorldHeight; ++y)
                tt.SetPixel(x, y, Color.red);

        tt.Apply();

        Sprite s = Sprite.Create(tt, new Rect(0, 0, WorldWidth, WorldHeight), new Vector2(.5f, .5f), 40f);

        GetComponent<SpriteRenderer>().sprite = s;
        */
	}

    List<Site> RandomSites()
    {
        List<Site> sites = new List<Site>();

        for(int i = 0; i < PolygonNumber; ++i)
        {
            sites.Add(new Site(new Vector2(Random.Range(0, WorldWidth), Random.Range(0, WorldHeight))));
        }


        return sites;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
