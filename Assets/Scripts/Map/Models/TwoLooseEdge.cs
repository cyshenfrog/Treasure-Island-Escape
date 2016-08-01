using UnityEngine;
using System.Collections;

public class TwoLooseEdge
{
    //function: 

    Vertex loosepoint0, loosepoint1;
    int m;

    public Vertex LoosePoint0
    {
        get { return loosepoint0; }
    }

    public Vertex LoosePoint1
    {
        get { return loosepoint1; }
    }

    public TwoLooseEdge(Vertex loosepoint0, Vertex loosepoint1)
    {
        this.loosepoint0 = loosepoint0;
        this.loosepoint1 = loosepoint1;
    }
}
