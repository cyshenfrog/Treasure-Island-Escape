using UnityEngine;
using System.Collections.Generic;

public class Vertex
{
    Vector2 point;
    int m, reference = 0;
    bool isloose = true;

    public Vertex(Vector2 point, int i)
    {
        this.point = point;
        reference += i;
    }
}
