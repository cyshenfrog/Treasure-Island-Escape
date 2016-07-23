using UnityEngine;
using System.Collections.Generic;

public class Site
{
    Vector2 center;
    List<Edge> edges;
    //Parabola parabola;
    bool start = false;

    public bool isStart
    {
        get { return start; }
    }

    public int X
    {
        get { return (int)center.x; }
    }

    public Vector2 Center
    {
        get { return center; }
    }

    public Site(Vector2 center)
    {
        this.center = center;
        edges = new List<Edge>();
        //parabola = new Parabola(center);
        start = true;
    }

    public override string ToString()
    {
        return "Site at (" + center.x + ", " + center.y + ")";
    }
}
