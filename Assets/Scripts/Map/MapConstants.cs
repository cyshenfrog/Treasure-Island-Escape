using UnityEngine;
using System.Collections;

public static class MapConstants
{
    public enum LandformType
    {
        Volcano,
        Snowfield,
        Marsh,
        Desert,
        Forest,
        Grasslands,
        Sea
    }

    public enum BuildingType
    {
        Vestige
    }
    
    public static Vector2[] bfs = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
}
