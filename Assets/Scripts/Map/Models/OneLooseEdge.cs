using UnityEngine;
using System.Collections;

public class OneLooseEdge
{
    //function: 

    Vector2 fixedpoint, loosepoint;
    int m;

    public Vector2 FixedPoint
    {
        get { return fixedpoint; }
    }

    public Vector2 LoosePoint
    {
        get { return loosepoint; }
    }

    public OneLooseEdge(Vector2 fixedpoint, Vector2 loosepoint)
    {
        this.fixedpoint = fixedpoint;
        this.loosepoint = loosepoint;
    }
}
