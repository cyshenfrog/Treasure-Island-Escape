using UnityEngine;
using System.Collections;

public static class MapConstants
{
    //sea = 0, which is represented to index in landformList
    public enum LandformType
    {
        Sea,
        Grassland,
        Forest,
        Desert,
        Marsh,
        Snowfield,
        Volcano,
        None
    }

    public enum FormType
    {
        Ellipse,
        None
    }

    public enum BuildingType
    {
        Vestige
    }
    
    public static Vector2[] SearchDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    public static int LandformTypeAmount = (int)LandformType.None;
}
