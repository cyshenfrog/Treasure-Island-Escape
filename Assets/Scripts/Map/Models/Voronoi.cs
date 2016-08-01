using UnityEngine;
using System.Collections.Generic;

public class Voronoi
{
    List<Site> sites;
    List<Edge> edges;
    List<Vertex> vertexes = new List<Vertex>();
    List<Parabola> parabolas;
    int width, height;
    Site[] sitearr;

    public Voronoi(List<Site> sites, int width, int height)
    {
        this.sites = sites;
        this.width = width;
        this.height = height;

        edges = new List<Edge>();
        parabolas = new List<Parabola>();

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
                if (p.Boundary[0].RelatedParabolas.Count == 1)
                {
                    p.UpdateBoundary(0, height);
                }

                if (p.Boundary[count - 1].RelatedParabolas.Count == 1)
                {
                    p.UpdateBoundary(count - 1, height);
                }
            }

            //to calculate intersections
            for(int j = pcount; j >= 0; --j)
            {
                for(int k = j - 1; k >= 0; --k)
                {
                    Parabola p0 = parabolas[j], p1 = parabolas[k];
                    List<Vertex> intersections = Parabola.InterSection(p0, p1);
                    int icount = intersections.Count;

                    switch(icount)
                    {
                        case 1:
                            int rcount = p0.RelatedParabolas.Count;

                            if (rcount == 0)
                            {
                                p0.RelatedParabolas.Add(p1);

                                //to change up or down
                                if (Mathf.Abs(intersections[0].CenterY - p0.Boundary[0].CenterY) < Mathf.Abs(intersections[0].CenterY - p0.Boundary[p0.Boundary.Count - 1].CenterY))
                                {
                                    //p0 up need update
                                    //p0 is down p1 is up?
                                    intersections[0].SwapParabola();
                                    p0.Boundary[0] = intersections[0];
                                    p1.Boundary[p1.Boundary.Count - 1] = intersections[0];

                                    //to add one loose edge
                                }
                                else
                                {
                                    //p0 down need update
                                    p0.Boundary[p0.Boundary.Count - 1] = intersections[0];
                                    p1.Boundary[0] = intersections[0];
                                }
                            }
                            else
                            {
                                //rcount is 1
                                if(p0.RelatedParabolas[0] != p1)
                                {
                                    //new vertex in the center of p0, p0related, and p1

                                    Parabola p2 = p0.RelatedParabolas[0];
                                    
                                    vertexes.Add(new Vertex(CenterinThreePoints(p0.Focus, p1.Focus, p2.Focus)));
                                    //to add edges

                                    if(p1.CenterY > p2.CenterY)
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

                                }
                                else
                                {
                                    //intersections update
                                    if(p0.CenterY > p1.CenterY)
                                    {
                                        //p0 is upper parabola
                                    }
                                }
                            }

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
}
