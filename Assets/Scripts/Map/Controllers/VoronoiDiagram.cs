using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class VoronoiDiagram : MonoBehaviour {

    public int PolygonNumber, WorldWidth, WorldHeight;

	// Use this for initialization
	void Start ()
    {
        Texture2D tt = new Texture2D(512, 512);

        for (int x = 0; x < WorldWidth; ++x)
            for (int y = 0; y < WorldHeight; ++y)
                tt.SetPixel(x, y, Color.red);

        tt.Apply();

        Sprite s = Sprite.Create(tt, new Rect(0, 0, WorldWidth, WorldHeight), new Vector2(.5f, .5f), 40f);

        GetComponent<SpriteRenderer>().sprite = s;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
