using UnityEngine;
using System.Collections;

public class Ellipse : Form
{
    public Ellipse(Vector2 center, float a, float b)
    {
        formType = MapConstants.FormType.Ellipse;

        this.center = center;
        this.a = a;
        this.b = b;

        a2 = a * a;
        b2 = b * b;
    }

    public override bool Inside(Vector2 position)
    {
        return b2 * position.x * position.x + a2 * position.y * position.y - a2 * b2 < 0f;
    }
    
    float a, b, a2, b2;
}

/*
//redundant
public Ellipse(Vector2 center, float a, float b, MapConstants.LandformType mt, int sid)
{
    Debug.Log("YO");
    this.center = center;
    this.a = a;
    this.b = b;
    this.mt = mt;
    this.sid = sid;

    a2 = a * a;
    b2 = b * b;
}
*/

/*
//redundant
public void FillToWorlid(World world)
{
    this.world = world;
    //Matrix4x4 ro = Matrix4x4.Scale(new Vector3(20f, 0f));
    //Vector3 v = ro * Vector3.one;

    //BFS
    Queue q = new Queue();
    TileData data = world.GetTileDataAt(center);
    //
    data.SearchID = sid;
    data.MaterialType = mt;
    q.Enqueue(data);

    while (q.Count != 0)
    {
        data = (TileData)q.Dequeue();

        for(int i = 0; i < 4; ++i)
        {
            TileData td = world.GetTileDataAt(data.Po+ MapConstants.bfs[i]);
            if(td != null)
            {
                float x = td.X - center.x, y = td.Y - center.y;
                if (td.SearchID != sid)
                {
                    if ((x * x) / a2 + (y * y) / b2 < 1)
                    {
                        //has not found yet
                        td.SearchID = sid;
                        data.MaterialType = mt;
                        q.Enqueue(td);
                    }
                    else
                    {
                        //outside

                    }
                }
            }
        }
    }//
}
*/
