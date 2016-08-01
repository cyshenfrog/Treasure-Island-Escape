using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        Vector2 p0 = new Vector2(0f, 0f), p1 = new Vector2(1f, 1f), p2 = new Vector2(2f, 2f), p3 = new Vector2(3f, 3f);

        Edge e0 = new Edge(p0, p1), e1 = new Edge(p2, p3), e2 = new Edge(p1, p2);
        

        /*
        Vertex v = new Vertex(p0, p1, p2);

        e0.p0.Add(v);
        e1.p0.Add(v);

        Debug.Log(e0.p0[0].points[0]);
        Debug.Log(e1.p0[0].points[0]);

        v.points[0] = p3;

        Debug.Log(e0.p0[0].points[0]);
        Debug.Log(e1.p0[0].points[0]);
        */
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
