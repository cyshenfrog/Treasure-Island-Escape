using UnityEngine;
using System.Collections;

public class World
{
    TileData[,] tiledata;
    int width, height;

    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    public World(int width = 100, int height = 100)
    {
        this.width = width;
        this.height = height;

        tiledata = new TileData[width, height];
        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                tiledata[x, y] = new Sea(x, y);
            }
        }
    }

    public TileData GetTileDataAt(int x, int y)
    {
        /*
        if (x > width || x < 0 || y > height || y < 0)
        {
            Debug.LogError("Tile (" + x + ", " + y + ") is out of range.");
            return null;
        }
        else
        {
            Debug.Log(x + " " + y);
            return tiledata[x, y];
        }*/
        Debug.Log("x = " + x + " y = " + y);
        return null;
    }

    public TileData GetTileDataAt(Vector2 v)
    {
        return GetTileDataAt((int)v.x, (int)v.y);
    }
}
