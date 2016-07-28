using UnityEngine;
using System.Collections.Generic;

public class Vertex
{
    Vector2 center;
    public List<Vector2> points = new List<Vector2>();
    int m, reference = 0;
    bool isloose = true;

    public Vertex(Vector2 center, Vector2 p0, Vector2 p1)
    {
        this.center = center;

        points.Add(p0);
        points.Add(p1);
    }
}
