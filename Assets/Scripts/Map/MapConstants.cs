using UnityEngine;
using System.Collections;

public static class MapConstants
{
    public enum MaterialType
    {
        Sea,
        Vestige,
        Forest,
        Grasslands,
        Marsh,
        Desert,
        Volcano
    }

    public enum BuildingType
    {

    }

    //                                                    Sea       Vestige         Forest      Grasslands      Marsh       Desert      Volcano
    public static Color[] materialColors = new Color[] { Color.blue, Color.gray, Color.green, Color.cyan, Color.magenta, Color.yellow, Color.red };
    public static Vector2[] bfs = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
}
