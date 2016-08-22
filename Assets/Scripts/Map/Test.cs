using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    public Texture2D Image;

	// Use this for initialization
	void Start ()
    {
        int tWidth = Image.width, tHeight = Image.height;
        int reWidth = 1278, reHeight = 823;
        float times = reWidth / tWidth > reHeight / tHeight ? reWidth / tWidth : reHeight / tHeight;

        Debug.Log(Image.Resize(tWidth * ((int)times + 1), tHeight * ((int)times + 1)));
        Image.Apply();

        GetComponent<SpriteRenderer>().sprite = Sprite.Create(Image, new Rect(0, 0, 128, 128), Vector2.one / 2);
    }

    void func(TileData td)
    {
        //td.MaterialTypes[0] = MapConstants.MaterialType.Desert;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
