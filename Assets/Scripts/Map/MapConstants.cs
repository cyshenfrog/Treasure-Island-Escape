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
        Grassland,
        Sea,
        None
    }

    public enum BuildingType
    {
        Vestige
    }
    
    public static Vector2[] SearchDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    public static int LandformTypeAmount = (int)LandformType.None;
}
