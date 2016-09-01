using UnityEngine;
using System.Collections.Generic;

public class Parabola
{
    //function: (y - center.y)^2 = -4c(x - center.x)

    List<TwoLooseEdge> twolooseedges = new List<TwoLooseEdge>();
    //bool[] uplist = new bool[2];
    List<Parabola> relatedparabolas = new List<Parabola>();
    Parabola[] updownparabolas = new Parabola[2];
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
        get { return (int)((directrix - focus.x) / 2 + focus.x); }
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

    public Vertex Up
    {
        get { return up; }
        set { up = value; }
    }

    public Vertex Down
    {
        get { return down; }
        set { down = value; }
    }

    public List<Parabola> RelatedParabolas
    {
        get { return relatedparabolas; }
        set { relatedparabolas = value; }
    }

    public Parabola[] UpDownParabolas
    {
        get { return updownparabolas; }
        set { updownparabolas = value; }
    }

    public List<TwoLooseEdge> TwoLooseEdges
    {
        get { return twolooseedges; }
        set { twolooseedges = value; }
    }

    /*
    public bool[] UpList
    {
        get { return uplist; }
        set { uplist = value; }
    }*/

    //public Parabola(Vector2 focus, int siteindex, int directrix, List<Parabola> parabolas)
    public Parabola(Vector2 focus, int siteindex, List<Parabola> parabolas)
    {
        this.focus = focus;
        this.siteindex = siteindex;

        //to init the parabola
        int count = parabolas.Count, interindex = -1;
        Vertex intersection = null;
        //smallest
        float x = -1f;

        for (int i = 0; i < count; ++i)
        {
            List<Vertex> solution = InterSection(parabolas[i], this);
            if (solution != null)
            {
                //must ont intersection
                //intersection = InterSection(parabolas[i], this, true);
                intersection = solution[0];
                if (x < intersection.Center.x)
                {
                    //reasonal and closer solution
                    x = intersection.Center.x;
                    interindex = i;
                }
            }
        }

        if (interindex == -1)
        {
            //沒有跟任何雙曲線相交!
            up = new Vertex(new Vector2(0f, focus.y), this);
            down = new Vertex(new Vector2(0f, focus.y), this);
            updownparabolas[0] = null;
            updownparabolas[1] = null;
        }
        else
        {
            //intersection

            Parabola p = parabolas[interindex];

            up = intersection;
            down = intersection;
            updownparabolas[0] = p;
            updownparabolas[1] = p;

            /*
            relatedparabolas.Add(p);
            relatedparabolas.Add(p);
            */

            p.TwoLooseEdges.Add(new TwoLooseEdge(up, down));
        }
    }
    
    //the distance between point and line
    public static int Distance(Vector2 p, int l)
    {
        return (int)Mathf.Abs(p.x - l);
    }
    

    bool isreasonal(Vertex v)
    {
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

        int thisindex = v.RelatedParabolas[0] == this ? 0 : 1;
        
        if(thisindex == 0)
        {
            //this is up
            if(down.RelatedParabolas.Count == 1)
            {
                if(down.CenterY < v.CenterY)
                {
                    return isintwolooseedge(v);
                }
            }
            else if(down.RelatedParabolas[0] == v.RelatedParabolas[0] && down.RelatedParabolas[1] == v.RelatedParabolas[1])
            {
                //to update the intersection
                return isintwolooseedge(v);
            }
        }
        else
        {
            //this is down
            if (up.RelatedParabolas.Count == 1)
            {
                if (up.CenterY > v.CenterY)
                    return isintwolooseedge(v);
            }
            else if (up.RelatedParabolas[0] == v.RelatedParabolas[0] && up.RelatedParabolas[1] == v.RelatedParabolas[1])
            {
                //to update the intersection
                return isintwolooseedge(v);
            }
        }

        return false;

        /*
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

        return false;*/
    }

    bool isintwolooseedge(Vertex v)
    {
        for (int i = 0; i < twolooseedges.Count; ++i)
        {
            if (v.CenterY <= twolooseedges[i].LoosePoint0.CenterY && v.CenterY >= twolooseedges[i].LoosePoint1.CenterY)
            {
                //in the twolooseedge
                return false;
            }
        }
        return true;
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

                Vertex intersection = new Vertex(new Vector2(x, y), p0, p1);

                //is reasonal?
                if (p0.isreasonal(intersection) && p1.isreasonal(intersection))
                {
                    //reasonal
                    ++i;
                    intersections.Add(intersection);
                }
                else
                {
                    //non-reasonal
                    solutions.RemoveAt(i);
                }
            }

            if(intersections.Count == 2)
            {
                if(intersections[0].CenterY < intersections[1].CenterY)
                {
                    Vertex temp = intersections[0];
                    intersections[0] = intersections[1];
                    intersections[1] = temp;
                }

                return intersections;
            }
            else if(intersections.Count == 1)
            {
                return intersections;
            }
        }

        return null;
    }

    /*
    //only called when parabola created?
    //can this merge with upper function?
    public static Vector2 InterSection(Parabola p, Parabola self, bool n)
    {
        float y = self.CenterY;
        int y0 = p.CenterY, x0 = p.CenterX, c0 = p.C;
        float x = -((y - y0) * (y - y0) - 4 * c0 * x0) / (4 * c0);

        Vertex inter = new Vertex(new Vector2(x, y), self, p);

        //is reasonal?
        if (p.isreasonal(inter))
        {
            //reasonal
            return inter.Center;
        }
        else
        {
            //non-reasonal
            return new Vector2(-1f, -1f);
        }
    }
    */

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

    public void UpdateBoundary(bool isup, int height)
    {
        //function: (y - center.y) ^ 2 = -4c(x - center.x)
        int best = isup ? height : 0;

        //y = best
        float x1 = -(best - CenterY) * (best - CenterY) / 4 * C + CenterX;
        //x = 0
        float y0 = isup ? 4 * C * CenterX + CenterY : -(4 * C * CenterX) + CenterY;
        
        if(isup)
            up.Center = x1 >= 0f ? new Vector2(x1, best) : new Vector2(0, y0);
        else
            down.Center = x1 >= 0f ? new Vector2(x1, best) : new Vector2(0, y0);
    }
}
