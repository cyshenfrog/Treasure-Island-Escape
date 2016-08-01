using UnityEngine;
using System.Collections.Generic;

public class Vertex
{
    public Vertex(Vector2 center, Parabola p0, Parabola p1)
    {
        this.center = center;

        //always p0 is upper parabola
        relatedparabolas.Add(p0);
        relatedparabolas.Add(p1);
    }

    public Vertex(Vector2 center, Parabola p0)
    {
        this.center = center;

        relatedparabolas.Add(p0);
    }

    public Vertex(Vector2 center)
    {
        this.center = center;
    }

    public int CenterY
    {
        get { return (int)center.y; }
    }

    public Vector2 Center
    {
        get { return center; }
        set { center = value; }
    }

    public List<Parabola> RelatedParabolas
    {
        get { return relatedparabolas; }
    }

    public void SwapParabola()
    {
        Parabola t = relatedparabolas[0];
        relatedparabolas[0] = relatedparabolas[1];
        relatedparabolas[1] = t;
    }

    Vector2 center;
    List<Parabola> relatedparabolas = new List<Parabola>();
    int m, reference = 0;
    bool isloose = true;
}
