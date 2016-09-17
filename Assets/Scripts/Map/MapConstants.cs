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

    public enum Directions
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }
    
    public static Vector2[] SearchDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    public static Color[] SeaColor = new Color[] { Color.black, Color.blue, Color.black, Color.blue, Color.black, Color.blue };
    public static Color[] GrasslandColor = new Color[] { new Color(.5176471f, .8f, .4705882f), new Color(.3803922f, .6235294f, .3411765f), new Color(.854902f, .8941177f, .8039216f), new Color(.6470588f, .7176471f, .5882353f), new Color(.4666667f, .7176471f, .4745098f), new Color(.7529412f, .7333333f, .7333333f) };
    public static Color[] ForestColor = new Color[] { new Color(.392f, .51f, .4f), new Color(.753f, .714f, .729f), new Color(.69f, .741f, .643f), new Color(.49f, .624f, .624f), new Color(.843f, .87f, .85f), new Color(.7f, .794f, .811f) };
    public static Color[] DesertColor = new Color[] { new Color(.718f, .663f, .39f), new Color(.89f, .84f, .616f), new Color(.792f, .753f, .412f), new Color(.68f, .624f, .376f), new Color(.86f, .8f, .56f), new Color(.72f, .68f, .31f) };
    public static Color[] MarshColor = new Color[] { Color.cyan, Color.yellow, Color.cyan, Color.yellow, Color.cyan, Color.yellow };
    public static Color[] SnowfieldColor = new Color[] { new Color(.96f, .97f, .98f), new Color(.91f, .92f, .97f), new Color(.898f, .91f, .93f), new Color(.87f, .91f, .88f), new Color(.9f, .93f, .91f), Color.white };
    public static Color[] VolcanoColor = new Color[] { Color.gray, Color.red, Color.gray, Color.red, Color.gray, Color.red };
    public static Color[][] LandformColor = new Color[][] { SeaColor, GrasslandColor, ForestColor, DesertColor, MarshColor, SnowfieldColor, VolcanoColor };
    //new Color(,,), new Color(,,), new Color(,,), new Color(,,), new Color(,,), new Color(,,)

    //public static 

    public static string LandformPath = @"Map\Grassland";
    public static int LandformTypeAmount = (int)LandformType.None, MaxStack = 30;
}
