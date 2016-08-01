using UnityEngine;
using System.Collections;

public class TwoLooseEdge
{
    //function: 

    Vector2 loosepoint0, loosepoint1;
    int m;

    public Vector2 LoosePoint0
    {
        get { return loosepoint0; }
    }

    public Vector2 LoosePoint1
    {
        get { return loosepoint1; }
    }

    public TwoLooseEdge(Vector2 loosepoint0, Vector2 loosepoint1)
    {
        this.loosepoint0 = loosepoint0;
        this.loosepoint1 = loosepoint1;
    }
}
