using UnityEngine;
using System.Collections;

public class OneLooseEdge
{
    //function: 

    Vector2 fixedpoint;
    Vertex loosepoint;
    int m;

    public Vector2 FixedPoint
    {
        get { return fixedpoint; }
    }

    public Vertex LoosePoint
    {
        get { return loosepoint; }
    }

    public OneLooseEdge(Vector2 fixedpoint, Vertex loosepoint)
    {
        this.fixedpoint = fixedpoint;
        this.loosepoint = loosepoint;
    }
}
