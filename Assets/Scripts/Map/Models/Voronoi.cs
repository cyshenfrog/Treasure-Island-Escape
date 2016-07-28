using UnityEngine;
using System.Collections.Generic;

public class Voronoi
{
    List<Site> sites;
    List<Edge> edges;
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
            //parabolas.Add(new Parabola(sites[i]));

            int pcount = parabolas.Count;

            for(int j = 0; j < pcount; ++j)
            {
                parabolas[j].Directrix = directrix;
            }
            
        }
    }
}
