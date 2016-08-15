using UnityEngine;
using System.Collections.Generic;

public class TileData2
{
    public TileData2(Vector2 position)
    {
        this.position = position;
        center = this;

        directions.Add(Vector2.up);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        directions.Add(Vector2.right);
    }

    public TileData2(Vector2 position, TileData2 center, Vector2 fromdirection)
    {
        this.position = position;
        this.center = center;

        directions.Add(Vector2.up);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        directions.Add(Vector2.right);

        directions.Remove(fromdirection);
    }

    public MapConstants.MaterialType MaterialType0
    {
        get { return materialtype0; }
        set { materialtype0 = value; }
    }

    public bool DFS()
    {



        return true;
    }

    List<Vector2> directions;
    MapConstants.MaterialType materialtype0 = MapConstants.MaterialType.None, materialtype1 = MapConstants.MaterialType.None;
    TileData2 center;
    Vector2 position;
}
