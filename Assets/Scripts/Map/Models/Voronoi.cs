using UnityEngine;
using System.Collections.Generic;

public class Voronoi
{
    List<Site> sites;
    List<Edge> edges = new List<Edge>();
    List<OneLooseEdge> onelooseedges = new List<OneLooseEdge>();
    List<Vertex> vertexes = new List<Vertex>();
    List<Parabola> parabolas = new List<Parabola>();
    int width, height;
    Site[] sitearr;

    public Voronoi(List<Site> sites, int width, int height)
    {
        this.sites = sites;
        this.width = width;
        this.height = height;

        sitesSort();

        //to calculate

    }

    void sitesSort()
    {
        int count = sites.Count;

        //to pick up all sites from sites list into sites array
        sitearr = new Site[count];
        for (int i = 0; i < count; ++i)
            sitearr[i] = sites[i];

        //to use quick sort
        quicksort(0, count - 1);

        for (int i = 0; i < count; ++i)
            sites[i] = sitearr[i];

        //for debug
        for (int i = 0; i < count; ++i)
            Debug.Log(sites[i].ToString());
    }

    void quicksort(int left, int right)
    {
        if(right > left)
        {
            //do quick sort
            int pivotindex = sitearr.Length / 2;
            Debug.Log(pivotindex);

            pivotindex = partition(left, right, pivotindex);

            quicksort(left, pivotindex - 1);
            quicksort(pivotindex + 1, right);
        }
    }

    int partition(int left, int right, int pivotindex)
    {
        Site pivot = sitearr[pivotindex];
        sitesSwap(pivotindex, right);

        int storeindex = left, last = right - 1;


        for(int i = 1; i < last; ++i)
        {
            if(sitearr[i].X < pivot.X)
            {
                sitesSwap(i, storeindex);
                ++storeindex;
            }
        }

        //final
        sitesSwap(right, storeindex);
        return storeindex;
    }
    
    void sitesSwap(int a, int b)
    {
        Site temp = sitearr[a];
        sitearr[a] = sitearr[b];
        sitearr[b] = temp;
    }

    void Fortune()
    {
        int scount = sites.Count, directrix;

        for(int i = 0; i < scount; ++i)
        {
            directrix = sites[i].X;

            //to new the parabola
            parabolas.Add(new Parabola(sites[i].Center, i, parabolas));

            int pcount = parabolas.Count;

            //from right to left
            //to update every directrix of parabolas
            for(int j = pcount; j >= 0; --j)
            {
                Parabola p = parabolas[j];

                p.Directrix = directrix;

                int count = p.Boundary.Count;
                //update function
                if (p.Up.RelatedParabolas.Count == 1)
                {
                    p.UpdateBoundary(0, height);
                }

                if (p.Down.RelatedParabolas.Count == 1)
                {
                    p.UpdateBoundary(count - 1, height);
                }
            }

            //to calculate intersections
            for(int j = pcount; j >= 0; --j)
            {
                for(int k = j - 1; k >= 0; --k)
                {
                    Parabola p0 = parabolas[j], p1 = parabolas[k], p2, p3;
                    List<Vertex> intersections = Parabola.InterSection(p0, p1);
                    int icount = intersections.Count;

                    switch(icount)
                    {
                        case 1:
                            if(p0.UpDownParabolas[0] == null && p0.UpDownParabolas[1] == null)
                            {
                                //only happen when first stage
                                p0.RelatedParabolas.Add(p1);
                                //must!?
                                p1.RelatedParabolas.Add(p0);

                                //to change up or down
                                //to reduce the code?
                                if (Mathf.Abs(intersections[0].CenterY - p0.Up.CenterY) < Mathf.Abs(intersections[0].CenterY - p0.Down.CenterY))
                                {
                                    //p0 up need update
                                    //p0 is down p1 is up
                                    p0.Up = intersections[0];
                                    p0.UpDownParabolas[0] = p1;
                                    p1.Down = intersections[0];
                                    p1.UpDownParabolas[1] = p0;
                                }
                                else
                                {
                                    //p0 down need update
                                    p0.Down = intersections[0];
                                    p0.UpDownParabolas[1] = p1;
                                    p1.Up = intersections[0];
                                    p1.UpDownParabolas[0] = p0;
                                }

                                //to add one loose edge
                                onelooseedges.Add(new OneLooseEdge(FixedinOneLooseEdge(p0.Center, p1.Center, directrix), intersections[0]));

                            }
                            else if(p0.UpDownParabolas[0] != null && p1.UpDownParabolas[1] != null)
                            {
                                p2 = p0.UpDownParabolas[0];
                                p3 = p0.UpDownParabolas[1];

                                if (p2 == p3)
                                {
                                    if (p1 == p2)
                                    {
                                        //p0 <=> p2(p1) = 2  =>  p0 <=> p1(p2) = 1
                                        //to update the intersection
                                        if (p0.CenterY > p1.CenterY)
                                        {
                                            //p0 is up
                                            p0.Down.Center = intersections[0].Center;
                                            //p1.Up.Center = intersections[0].Center;
                                        }
                                        else
                                        {
                                            //p1 is up
                                            p0.Up.Center = intersections[0].Center;
                                            //p1.Down.Center = intersections[0].Center;
                                        }
                                    }
                                    else
                                    {
                                        //p0 <=> p1 = 1(new) p0 <=> p2 = 1(2=>1)

                                        //new vertex in the center of p0, p0related, and p1
                                        Vertex center = new Vertex(CenterinThreePoints(p0.Focus, p1.Focus, p2.Focus));
                                        vertexes.Add(center);

                                        //to decide new up and down
                                        if (p1.CenterY > p2.CenterY)
                                        {
                                            //p1 is upper parabola
                                            //it must have only one intersection?
                                            p1.Down = Parabola.InterSection(p1, p0)[0];
                                            p1.UpDownParabolas[1] = p0;
                                            p2.Up = Parabola.InterSection(p2, p0)[0];
                                            p2.UpDownParabolas[0] = p0;
                                        }
                                        else
                                        {
                                            //p2 is upper parabola
                                            p2.Down = Parabola.InterSection(p2, p0)[0];
                                            p2.UpDownParabolas[1] = p0;
                                            p1.Up = Parabola.InterSection(p1, p0)[0];
                                            p1.UpDownParabolas[0] = p0;
                                        }


                                        //to drop the p2's two loose edge

                                        //to add edges
                                        //edges.Add(new Edge(, center.Center));
                                    }
                                }
                                else
                                {
                                    //p2 != p3
                                    if(p1 == p2 || p1 == p3)
                                    {
                                        //to update the intersection
                                        if(p0.CenterY > p1.CenterY)
                                        {
                                            //p0 is up
                                            p0.Down.Center = intersections[0].Center;
                                            //p1.Up.Center = intersections[0].Center
                                        }
                                        else
                                        {
                                            //p0 is down
                                            p0.Up.Center = intersections[0].Center;
                                            //p1.Down.Center = intersections[0].Center
                                        }
                                    }
                                    //else is not needed??
                                }
                            }
                            else
                            {
                                int p0isup = p0.UpDownParabolas[0] == null ? 1 : 0;
                                p2 = p0.UpDownParabolas[p0isup];

                                if (p2 == p1)
                                {
                                    //to update the intersection
                                    if(p0isup == 1)
                                    {
                                        p0.Down.Center = intersections[0].Center;
                                    }
                                    else
                                    {
                                        p0.Up.Center = intersections[0].Center;
                                    }
                                }
                                else
                                {
                                    Parabola[] orderbyY = new Parabola[3];
                                    orderbyY[0] = p0;
                                    orderbyY[1] = p1;
                                    orderbyY[2] = p2;
                                    
                                    //to order by centery
                                    for(int m = 1; m < 3; ++m)
                                    {
                                        for(int n = m; n > 0; --n)
                                        {
                                            if (orderbyY[n].CenterY > orderbyY[n - 1].CenterY)
                                            {
                                                Parabola temp = orderbyY[n];
                                                orderbyY[n] = orderbyY[n - 1];
                                                orderbyY[n - 1] = temp;
                                            }
                                        }
                                    }

                                    if(orderbyY[1] == p0)
                                    {
                                        //p1 <=> p2 = 0
                                        //p2 != p1
                                        if (p0isup == 1)
                                        {
                                            p0.Up = intersections[0];
                                            p0.UpDownParabolas[0] = p1;
                                            p1.Down = intersections[0];
                                            p1.UpDownParabolas[1] = p0;
                                        }
                                        else
                                        {
                                            p0.Down = intersections[0];
                                            p0.UpDownParabolas[1] = p1;
                                            p1.Up = intersections[0];
                                            p1.UpDownParabolas[0] = p0;
                                        }
                                    }
                                    else
                                    {
                                        //p1 <=> p2 = 1
                                        //new vertex
                                        Vertex center = new Vertex(CenterinThreePoints(p0.Focus, p1.Focus, p2.Focus));
                                        vertexes.Add(center);
                                        
                                        //center is destroyed
                                        if(orderbyY[1] == p1)
                                        {
                                            //must one intersection?
                                            Vertex intersection = Parabola.InterSection(p2, p0)[0];
                                            orderbyY[0].Down = intersection;
                                            orderbyY[0].UpDownParabolas[1] = orderbyY[2];
                                            orderbyY[2].Up = intersection;
                                            orderbyY[2].UpDownParabolas[0] = orderbyY[0];
                                        }
                                    }
                                    /*
                                    Parabola pt0 = p1.CenterX > p2.CenterX ? p1 : p2;
                                    Parabola pt1 = pt0 == p1 ? p2 : p1;
                                    int ptrcount = pt0.RelatedParabolas.Count;

                                    for (int m = 0; m < ptrcount; ++m)
                                    {
                                        if (pt0.RelatedParabolas[m] == pt1)
                                        {
                                            //p0 <=> p2 = 1 p2 <=> p1 = 1 p0 <=> p1 = 0 => p0 <=> p2 = 0 p2 <=> p1 = 0? p0 <=> p1 = 1
                                            //p1 p2 are related

                                            //p0 p1 p2 circle center
                                            Vertex center = new Vertex(CenterinThreePoints(p0.Focus, p1.Focus, p2.Focus));
                                            vertexes.Add(center);

                                            p0.RelatedParabolas[0] = p1;
                                            
                                            if (pt0.UpList[pt0.RelatedParabolas.IndexOf(pt1)])
                                            {
                                                if (pt0 == p1)
                                                {
                                                    if (p1.CenterY > p0.CenterY)
                                                    {
                                                        p1.Down = intersections[0];
                                                        p0.Up = intersections[0];
                                                    }
                                                }
                                                else
                                                {
                                                    pt1.Down = intersections[0];
                                                    p0.Up = intersections[0];
                                                }
                                            }
                                            else
                                            {

                                            }

                                            //to remove p2
                                            parabolas.Remove(p2);
                                            p0.RelatedParabolas[0] = p1;

                                            p1.RelatedParabolas.IndexOf(p2)
                                            }
                                    }
                                    

                                    //p0 <=> p1 = 1(new) p0 <=> p2 = 1 p1 <=> p2 = 0

                                    //the same as 1 - 0
                                    p0.RelatedParabolas.Add(p1);
                                    p1.RelatedParabolas.Add(p0);

                                    //to change up or down
                                    //to reduce the code?
                                    if (Mathf.Abs(intersections[0].CenterY - p0.Up.CenterY) < Mathf.Abs(intersections[0].CenterY - p0.Down.CenterY))
                                    {
                                        //p0 up need update
                                        //p0 is down p1 is up
                                        p0.Up = intersections[0];
                                        p0.UpList.Add(false);
                                        p1.Down = intersections[0];
                                        p1.UpList.Add(true);
                                    }
                                    else
                                    {
                                        //p0 down need update
                                        p0.Down = intersections[0];
                                        p0.UpList.Add(true);
                                        p1.Up = intersections[0];
                                        p1.UpList.Add(false);
                                    }

                                    //to add one loose edge
                                    onelooseedges.Add(new OneLooseEdge(FixedinOneLooseEdge(p0.Center, p1.Center, directrix), intersections[0]));
                                    */
                                }

                            }





                            /*
                            int rcount = p0.RelatedParabolas.Count;
                            switch(rcount)
                            {
                                case 0:
                                    //only happen when first stage
                                    p0.RelatedParabolas.Add(p1);
                                    //must!?
                                    p1.RelatedParabolas.Add(p0);

                                    //to change up or down
                                    //to reduce the code?
                                    if (Mathf.Abs(intersections[0].CenterY - p0.Up.CenterY) < Mathf.Abs(intersections[0].CenterY - p0.Down.CenterY))
                                    {
                                        //p0 up need update
                                        //p0 is down p1 is up
                                        p0.Up = intersections[0];
                                        p1.Down = intersections[0];
                                    }
                                    else
                                    {
                                        //p0 down need update
                                        p0.Down = intersections[0];
                                        p1.Up = intersections[0];
                                    }

                                    //to add one loose edge
                                    onelooseedges.Add(new OneLooseEdge(FixedinOneLooseEdge(p0.Center, p1.Center, directrix), intersections[0]));

                                    break;
                                //can reduce ??
                                case 1:
                                    p2 = p0.RelatedParabolas[0];
                                    if (p2 == p1)
                                    {
                                        //to update the intersection
                                        if (p0.CenterY > p1.CenterY)
                                        {
                                            //p0 is up
                                            p0.Up.Center = intersections[0].Center;
                                        }
                                        else
                                        {
                                            //p0 is down
                                            p1.Up.Center = intersections[0].Center;
                                        }
                                    }
                                    else
                                    {
                                        Parabola pt0 = p1.CenterX > p2.CenterX ? p1 : p2;
                                        Parabola pt1 = pt0 == p1 ? p2 : p1;
                                        int ptrcount = pt0.RelatedParabolas.Count;

                                        for(int m = 0; m < ptrcount; ++m)
                                        {
                                            if(pt0.RelatedParabolas[m] == pt1)
                                            {
                                                //p0 <=> p2 = 1 p2 <=> p1 = 1 p0 <=> p1 = 0 => p0 <=> p2 = 0 p2 <=> p1 = 1 p0 <=> p1 = 1
                                                //p1 p2 are related

                                                //p0 p1 p2 circle center
                                                Vertex center = new Vertex(CenterinThreePoints(p0.Focus, p1.Focus, p2.Focus));
                                                vertexes.Add(center);

                                                p0.RelatedParabolas[0] = p1;

                                                if (pt0.UpList[pt0.RelatedParabolas.IndexOf(pt1)])
                                                {
                                                    if(pt0 == p1)
                                                    {
                                                        if(p1.CenterY > p0.CenterY)
                                                        {
                                                            p1.Down = intersections[0];
                                                            p0.Up = intersections[0];
                                                        }
                                                    }
                                                    else
                                                    {
                                                        pt1.Down = intersections[0];
                                                        p0.Up = intersections[0];
                                                    }
                                                }
                                                else
                                                {

                                                }

                                                //to remove p2
                                                parabolas.Remove(p2);
                                                p0.RelatedParabolas[0] = p1;
                                                
                                                p1.RelatedParabolas.IndexOf(p2)
                                            }
                                        }


                                        //p0 <=> p1 = 1(new) p0 <=> p2 = 1 p1 <=> p2 = 0

                                        //the same as 1 - 0
                                        p0.RelatedParabolas.Add(p1);
                                        p1.RelatedParabolas.Add(p0);

                                        //to change up or down
                                        //to reduce the code?
                                        if (Mathf.Abs(intersections[0].CenterY - p0.Up.CenterY) < Mathf.Abs(intersections[0].CenterY - p0.Down.CenterY))
                                        {
                                            //p0 up need update
                                            //p0 is down p1 is up
                                            p0.Up = intersections[0];
                                            p0.UpList.Add(false);
                                            p1.Down = intersections[0];
                                            p1.UpList.Add(true);
                                        }
                                        else
                                        {
                                            //p0 down need update
                                            p0.Down = intersections[0];
                                            p0.UpList.Add(true);
                                            p1.Up = intersections[0];
                                            p1.UpList.Add(false);
                                        }

                                        //to add one loose edge
                                        onelooseedges.Add(new OneLooseEdge(FixedinOneLooseEdge(p0.Center, p1.Center, directrix), intersections[0]));
                                    }

                                    break;
                                case 2:
                                    //p0.r[0] == p0.r[1]
                                    p2 = p0.RelatedParabolas[0];
                                    p3 = p0.RelatedParabolas[1];

                                    if(p2 == p3)
                                    {
                                        if (p1 == p2)
                                        {
                                            //p0 <=> p2(p1) = 2  =>  p0 <=> p1(p2) = 1
                                            //to update the intersection
                                            if (p0.CenterY > p1.CenterY)
                                            {
                                                //p0 is up
                                                p0.Down.Center = intersections[0].Center;
                                                //p1.Up.Center = intersections[0].Center;
                                            }
                                            else
                                            {
                                                //p1 is up
                                                p0.Up.Center = intersections[0].Center;
                                                //p1.Down.Center = intersections[0].Center;
                                            }
                                        }
                                        else
                                        {
                                            //p0 <=> p1 = 1(new) p0 <=> p2 = 1(2=>1)

                                            //new vertex in the center of p0, p0related, and p1
                                            Vertex center = new Vertex(CenterinThreePoints(p0.Focus, p1.Focus, p2.Focus));
                                            vertexes.Add(center);




                                            //to add edges
                                            //edges.Add(new Edge(, center.Center));

                                            if (p1.CenterY > p2.CenterY)
                                            {
                                                //p1 is upper parabola
                                                //it must have only one intersection?
                                                p1.Boundary[p1.Boundary.Count - 1] = Parabola.InterSection(p1, p0)[0];
                                                p2.Boundary[0] = Parabola.InterSection(p2, p0)[0];
                                            }
                                            else
                                            {
                                                //p2 is upper parabola
                                                p2.Boundary[p2.Boundary.Count - 1] = Parabola.InterSection(p2, p0)[0];
                                                p1.Boundary[0] = Parabola.InterSection(p1, p0)[0];
                                            }

                                            //to drop the p2's two loose edge



                                            //intersections update
                                            if (p0.CenterY > p1.CenterY)
                                            {
                                                //p0 is upper parabola
                                            }
                                        }
                                    }
                                    break;
                            }*/

                            break;
                        case 2:


                            break;
                        default:
                            Debug.LogError("intersections count error");
                            return;
                    }
                }
            }
            
        }
    }

    Vector2 CenterinThreePoints(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        //x^2 + y^2 + dx + ey + f = 0
        //=>f + ey + dx = - ( x^2 + y^2 )
        List<float> e0 = new List<float>(), e1 = new List<float>(), e2 = new List<float>();

        e0.Add(1f);
        e0.Add(p0.y);
        e0.Add(p0.x);
        e0.Add(-(p0.x * p0.x + p0.y + p0.y));

        e1.Add(1f);
        e1.Add(p1.y);
        e1.Add(p1.x);
        e1.Add(-(p1.x * p1.x + p1.y + p1.y));

        e2.Add(1f);
        e2.Add(p2.y);
        e2.Add(p2.x);
        e2.Add(-(p2.x * p2.x + p2.y + p2.y));

        //f is 0
        List<float> e01 = EquationSubtraction(e0, e1);
        List<float> e12 = EquationSubtraction(e1, e2);

        //f and e are 0
        List<float> e012 = EquationSubtraction(e01, e12);

        float x = e012[3] / e012[2];
        float y = (e01[3] - (e01[2] * x)) / e01[1];

        return new Vector2(x, y);
    }

    List<float> EquationSubtraction(List<float> e0, List<float> e1)
    {
        List<float> ne = new List<float>();

        for(int i = 0; i < 3; ++i)
        {
            if(e0[i] != e1[i])
            {
                float n0 = e0[i], n1 = e1[i];
                //doing
                for(int j = 0; j < 3; ++j)
                {
                    e0[j] *= n0;
                    e1[j] *= n1;
                    ne.Add(e0[j] - e1[j]);
                }

                return ne;
            }
            else if (e0[i] != 0f)
            {
                //directly substraction
                for (int j = 0; j < 3; ++j)
                {
                    ne.Add(e0[j] - e1[j]);
                }

                return ne;
            }
        }

        //all 0f
        return e0;
    }

    Vector2 FixedinOneLooseEdge(Vector2 p0, Vector2 p1, int directrix)
    {
        float m = -((p1.x - p0.x) / (p1.y - p0.y));
        int best = m > 0f ? 0 : height;
        Vector2 center = new Vector2((p0.x + p1.x) / 2, (p0.y + p1.y) / 2);

        //function: y - center.y = m (x - center.x)
        
        //x = 0
        float y0 = m * -center.x + center.y;
        //y = best
        float x1 = (best - center.y) / m + center.x;

        if(y0 >= 0f && y0 <= height)
        {
            return new Vector2(0f, y0);
        }
        else
        {
            //=> x1 > 0f && x1 < directrix
            return new Vector2(x1, best);
        }
    }
}
