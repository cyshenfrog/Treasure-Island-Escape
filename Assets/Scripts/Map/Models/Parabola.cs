using UnityEngine;
using System.Collections.Generic;

public class Parabola
{
    //function: (y - center.y)^2 = -4c(x - center.x)

    List<TwoLooseEdge> twolooseedges = new List<TwoLooseEdge>();
    List<Vertex> boundary = new List<Vertex>();
    List<Parabola> relatedparabolas = new List<Parabola>();
    Vertex up, down;


    Vector2 focus;
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
        get { return (int)((directrix - focus.x) / 2); }
    }

    public int CenterY
    {
        get { return (int)focus.y; }
    }

    public Vector2 Center
    {
        get { return new Vector2(CenterX, CenterY); }
    }

    public int C
    {
        get { return directrix - CenterX; }
    }

    public List<Parabola> RelatedParabolas
    {
        get { return relatedparabolas; }
        set { relatedparabolas = value; }
    }

    public List<Vertex> Boundary
    {
        get { return boundary; }
        set { boundary = value; }
    }

    //public Parabola(Vector2 focus, int siteindex, int directrix, List<Parabola> parabolas)
    public Parabola(Vector2 focus, int siteindex, List<Parabola> parabolas)
    {
        this.focus = focus;
        this.siteindex = siteindex;

        //to init the parabola
        int count = parabolas.Count, interindex = -1;
        Vector2 intersection = Vector2.zero;
        //smallest
        float x = -1f;

        for (int i = 0; i < count; ++i)
        {
            intersection = InterSection(parabolas[i], focus.y);

            if (intersection.x != -1f && x < intersection.x)
            {
                //reasonal and closer solution
                x = intersection.x;
                interindex = i;
            }
        }

        if (interindex == -1)
        {
            //沒有跟任何雙曲線相交!
            up = new Vertex(new Vector2(0f, focus.y), this);
            down = new Vertex(new Vector2(0f, focus.y), this);
        }
        else
        {
            //intersection

            Parabola p = parabolas[interindex];

            up = new Vertex(intersection, this, p);
            down = new Vertex(intersection, this, p);
            relatedparabolas.Add(p);
        }

        boundary.Add(up);
        boundary.Add(down);
    }

    
    //the distance between point and line
    public static int Distance(Vector2 p, int l)
    {
        return (int)Mathf.Abs(p.x - l);
    }
    

    bool isreasonal(Vector2 v)
    {
        
        int bcount = boundary.Count;

        /* non reasonal => drop!?
        if(bcount > 2)
        {
            //it has a two loose edge at least
            for (int i = 1; i < bcount - 1; i += 2)
            {
                if (v.y < boundary[i].CenterY && v.y > boundary[i + 1].CenterY)
                {
                    //very non reasonal
                    Debug.LogError("isreasonal error");
                    return false;
                }
            }


        }
        else if(v.y <= boundary[0].CenterY && v.y >= boundary[bcount-1].CenterY)
        {
            for(int i = 1; i < bcount - 1; i += 2)
            {
                if(v.y < boundary[i].CenterY && v.y > boundary[i+1].CenterY)
                {
                    //non reasonal
                    return false;
                }
            }
            return true;
        }
        */

        if (v.y <= boundary[0].CenterY && v.y >= boundary[bcount - 1].CenterY)
        {
            for (int i = 1; i < bcount - 1; i += 2)
            {
                if (v.y < boundary[i].CenterY && v.y > boundary[i + 1].CenterY)
                {
                    //non reasonal
                    return false;
                }
            }
            return true;
        }

        return false;
    }

    public static List<Vertex> InterSection(Parabola p0, Parabola p1)
    {
        int c0 = p0.C, c1 = p1.C, y0 = p0.CenterY, y1 = p1.CenterY, x0 = p0.CenterX, x1 = p1.CenterX;

        //coefficients of function
        int a = c1 - c0;
        int b = 2 * (y1 * c0 - y0 * c1);
        int c = c1 * y0 * y0 - c0 * y1 * y1 - 4 * c0 * c1 * (x0 - x1);

        List<float> solutions = solutionFormula(a, b, c);

        if(solutions != null)
        {
            List<Vertex> intersections = new List<Vertex>();
            int i = 0;
            while(i < solutions.Count)
            {
                float y = solutions[i];
                float x = -((y - y0) * (y - y0) - 4 * c0 * x0) / (4 * c0);

                Vector2 intersection = new Vector2(x, y);

                //is reasonal?
                if (p0.isreasonal(intersection) && p1.isreasonal(intersection))
                {
                    //reasonal
                    ++i;
                    intersections.Add(new Vertex(intersection, p0, p1));
                }
                else
                {
                    //non-reasonal
                    solutions.RemoveAt(i);
                }
            }

            if (intersections.Count > 0)
                return intersections;
        }

        return null;
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

    public void UpdateBoundary(int index, int height)
    {
        //function: (y - center.y) ^ 2 = -4c(x - center.x)
        int best = index == 0 ? height : 0;

        //y = best
        float x1 = -(best - CenterY) * (best - CenterY) / 4 * C + CenterX;
        //x = 0
        float y0 = index == 0 ? 4 * C * CenterX + CenterY : -(4 * C * CenterX) + CenterY;
        
        boundary[index].Center = x1 >= 0f ? new Vector2(x1, best) : new Vector2(0, y0);
    }
}
