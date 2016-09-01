using UnityEngine;
using System.Collections.Generic;
using System;

public class Test : MonoBehaviour
{
    public Texture2D Image;
    public Sprite single;

	// Use this for initialization
	void Start ()
    {
        //atlas
       
        

        /*
        int tWidth = Image.width, tHeight = Image.height;
        int reWidth = 1278, reHeight = 823;
        float times = reWidth / tWidth > reHeight / tHeight ? reWidth / tWidth : reHeight / tHeight;
        */
        /*
        int newWidth = 1024, newHeight = 2048, cellWidth = 32, cellHeight = 32;
        int widthCount = newWidth / cellWidth, heightCount = newHeight / cellHeight;

        
        Debug.Log(Image.width);
        Debug.Log(Image.height);

        ResizeCanvas(Image, newWidth, newHeight);

        Debug.Log(Image.width);
        Debug.Log(Image.height);


        //Debug.Log(Image.Resize(tWidth * ((int)times + 1), tHeight * ((int)times + 1)));
        //Image.Apply();

        Transform tf = ((GameObject)Resources.Load(@"Map\Tile")).transform;
        SpriteRenderer sr;

        for(int i = 0; i < heightCount; ++i)
        {
            for(int j = 0; j < widthCount; ++j)
            {
                tf = Instantiate(tf);
                tf.name = "Tile " + j.ToString() + '_' + i.ToString();
                tf.localPosition = Vector3.right * j * cellWidth * 0.01f + Vector3.up * i * cellHeight * 0.01f;
                sr = tf.GetComponent<SpriteRenderer>();
                sr.sprite = Sprite.Create(Image, new Rect(j * cellWidth, i * cellWidth, cellWidth, cellHeight), Vector2.zero);
            }
        }
        */
        //GetComponent<SpriteRenderer>().sprite = Sprite.Create(Image, new Rect(0, 0, Image.width, Image.height), Vector2.zero);

        /*
        bool test = true;
        for (int i = 0; i < 10; ++i)
        {
            if (test = func())
            {
                Debug.Log("enter");
            }
        }*/
    }

    public static Color32[] ResizeCanvas(Texture2D texture, int width, int height)
    {
        float oldWidth = texture.width, oldHeight = texture.height;
        float newWR = width / oldWidth, newHR = height / oldHeight, times = newWR > newHR ? newWR : newHR;
        times = (int)times + 1;
        Debug.Log(times);

        var newPixels = times == 1f ? texture.GetPixels32() : ResizeCanvas(texture.GetPixels32(), (int)oldWidth, (int)oldHeight, (int)(oldWidth * times), (int)(oldHeight * times), (int)times);

        texture.Resize((int)(oldWidth * times), (int)(oldHeight * times));
        texture.SetPixels32(newPixels);
        texture.Apply();
        return newPixels;
    }

    private static Color32[] ResizeCanvas(IList<Color32> pixels, int oldWidth, int oldHeight, int width, int height, int times)
    {
        var newPixels = new Color32[(width * height)];
        Debug.Log(width * height);

        for(int i = 0; i < oldHeight; ++i)
        {
            for(int j = 0; j < oldWidth; ++j)
            {
                int newIndex = i * width * times + j * times;
                int oldIndex = i * oldWidth + j;

                Color32 c = pixels[i * oldWidth + j];

                newPixels[newIndex] = c;
                newPixels[newIndex + 1] = c;
                newPixels[newIndex + width] = c;
                newPixels[newIndex + width + 1] = c;
            }
        }


        /*
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
        */

        return newPixels;
    }

    bool func()
    {
        //td.MaterialTypes[0] = MapConstants.MaterialType.Desert;
        Debug.Log("YO");
        return false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
