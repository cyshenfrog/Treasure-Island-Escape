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

    

    public static Color[] GrasslandColor = new Color[] { new Color(.5176471f, .8f, .4705882f), new Color(.3803922f, .6235294f, .3411765f), new Color(.854902f, .8941177f, .8039216f), new Color(.6470588f, .7176471f, .5882353f), new Color(.4666667f, .7176471f, .4745098f), new Color(.7529412f, .7333333f, .7333333f) };
    
    public static int LandformTypeAmount = (int)LandformType.None;
    public static string LandformPath = @"Map\Grassland";
}
