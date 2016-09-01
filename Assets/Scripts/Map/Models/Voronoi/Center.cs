using UnityEngine;
using System.Collections.Generic;

public class Center
{
    public Center(Vector2 position)
    {
        this.position = position;

        directions.Add(Vector2.up);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        directions.Add(Vector2.right);
    }

    public MapConstants.LandformType MaterialType
    {
        get { return materialtype; }
        set { materialtype = value; }
    }

    public bool DFS()
    {
        


        return true;
    }

    List<Vector2> directions = new List<Vector2>();
    MapConstants.LandformType materialtype;
    Vector2 position;
}
