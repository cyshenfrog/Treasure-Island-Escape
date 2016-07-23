using UnityEngine;
using System.Collections.Generic;

public class Parabola
{
    //function: (y - center.y)^2 = -4c(x - center.x)

    List<Vertex> vertexes;
    Vector2 focus, center;
    int directrix, siteindex;

    public Vector2 Focus
    {
        get { return focus; }
    }

    public int Directrix
    {
        get { return directrix; }
        set { directrix = value; }
    }

    public int CenterX
    {
        get { return (int)center.x; }
    }

    public int CenterY
    {
        get { return (int)center.y; }
    }

    public int C
    {
        get
        {
            return (int)Mathf.Abs((center.x - directrix) / 2);
        }
    }

    public Parabola(Vector2 focus, int siteindex, int directrix, List<Parabola> parabolas)
    {
        this.focus = focus;
        center = focus;
        this.siteindex = siteindex;
        this.directrix = directrix;

        int count = parabolas.Count, interindex = -1;
        //biggest
        float x = 10000f, y = center.y;
        for(int i = 0; i < count; ++i)
        {
            Vector2 inter = InterSection(parabolas[i], y);

            if(inter.x != -1f)
            {
                x = x > inter.x ? inter.x : x;
                interindex = i;
            }
        }

        if(interindex != -1)
        {
            //is intersected
            //vertexes.Add(new Vertex());
        }


        vertexes = new List<Vertex>();


       // vertexes.Add(new Vertex())
    }

    //the distance between point and line
    int Distance(Vector2 p, int l)
    {
        return (int)Mathf.Abs(p.x - l);
    }

    bool isreasonal(Vector2 v)
    {
        if(v.x < 0f || v.y < 0f)
        {
            return false;
        }

        return true;
    }

    public static Vector2 InterSection(Parabola p0, Parabola p1)
    {
        int c0 = p0.C, c1 = p1.C, y0 = p0.CenterY, y1 = p1.CenterY, x0 = p0.CenterX, x1 = p1.CenterX;

        //coefficients of function
        int a = c1 - c0;
        int b = 2 * (y1 * c0 - y0 * c1);
        int c = c1 * y0 * y0 - c0 * y1 * y1 - 4 * c0 * c1 * (x0 - x1);

        List<float> solutions = solutionFormula(a, b, c);

        if(solutions != null)
        {
            int i = 0;
            while(i < solutions.Count)
            {
                float y = solutions[i];
                float x = -((y - y0) * (y - y0) - 4 * c0 * x0) / (4 * c0);

                Vector2 inter = new Vector2(x, y);

                //is reasonal?
                if (p0.isreasonal(inter) && p1.isreasonal(inter))
                {
                    //reasonal
                    ++i;
                }
                else
                {
                    //non-reasonal
                    solutions.RemoveAt(i);
                }
            }
        }

        return Vector2.zero;
    }

    //only called when parabola created?
    public static Vector2 InterSection(Parabola p, float y)
    {
        int y0 = p.CenterY, x0 = p.CenterX, c0 = p.C;
        float x = -((y - y0) * (y - y0) - 4 * c0 * x0) / (4 * c0);

        Vector2 inter = new Vector2(x, y);

        //is reasonal?
        if (p.isreasonal(inter))
        {
            //reasonal
            return inter;
        }
        else
        {
            //non-reasonal
            return new Vector2(-1f, -1f);
        }
    }

    public static List<float> solutionFormula(int a, int b, int c)
    {
        int discriminant = b * b - 4 * a * c;

        //solution formula
        if (discriminant < 0f)
        {
            //No solution
            return null;
        }
        else if (discriminant == 0f)
        {
            //one solution
            List<float> solutions = new List<float>();
            solutions.Add((-b) / (2 * a));
            return solutions;
        }
        else
        {
            //two solutions
            List<float> solutions = new List<float>();
            solutions.Add((-b) + Mathf.Sqrt(discriminant) / (2 * a));
            solutions.Add((-b) - Mathf.Sqrt(discriminant) / (2 * a));
            return solutions;
        }
    }

}
