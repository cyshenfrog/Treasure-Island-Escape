using UnityEngine;
using System.Collections.Generic;

public class Edge
{
    //function: 

    Vector2 fixedpoint0, fixedpoint1;
    int m;
    public List<Vertex> p0;

    public Vector2 FixedPoint0
    {
        get { return fixedpoint0; }
        set { fixedpoint0 = value; }
    }
    
    public Vector2 FixedPoint1
    {
        get { return fixedpoint1; }
    }

    public Edge(Vector2 fixedpoint0, Vector2 fixedpoint1)
    {
        this.fixedpoint0 = fixedpoint0;
        this.fixedpoint1 = fixedpoint1;
        p0 = new List<Vertex>();
    }

    public override string ToString()
    {
        return "FixedPoint0 is (" + fixedpoint0.x + ", " + fixedpoint0.y + ") and FixedPoint1 is (" + fixedpoint1.x + ", " + fixedpoint1.y + ")";
    }
}
