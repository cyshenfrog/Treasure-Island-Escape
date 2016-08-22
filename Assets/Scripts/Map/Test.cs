using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    public Texture2D Image;

	// Use this for initialization
	void Start ()
    {
        /*
        int tWidth = Image.width, tHeight = Image.height;
        int reWidth = 1278, reHeight = 823;
        float times = reWidth / tWidth > reHeight / tHeight ? reWidth / tWidth : reHeight / tHeight;
        */
        Debug.Log(Image.width);
        Debug.Log(Image.height);

        ResizeCanvas(Image, 123, 650);

        //Debug.Log(Image.Resize(tWidth * ((int)times + 1), tHeight * ((int)times + 1)));
        //Image.Apply();

        GetComponent<SpriteRenderer>().sprite = Sprite.Create(Image, new Rect(0, 0, 128, 128), Vector2.one / 2);
    }

    public static Color32[] ResizeCanvas(Texture2D texture, int width, int height)
    {
        int tWidth = texture.width, tHeight = texture.height;
        float times = width / tWidth > height / tHeight ? width / tWidth : height / tHeight;

        var newPixels = ResizeCanvas(texture.GetPixels32(), tWidth, tHeight, tWidth * ((int)times + 1), tHeight * ((int)times + 1));

        texture.Resize(tWidth * ((int)times + 1), tHeight * ((int)times + 1));
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

    void func(TileData td)
    {
        //td.MaterialTypes[0] = MapConstants.MaterialType.Desert;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
