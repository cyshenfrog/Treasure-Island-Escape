using UnityEngine;
using System.Collections;

public class SmallStone : ResourceData
{
    public SmallStone(Vector3 center, int no)
    {
        this.center = center;
        this.no = no;

        //Max = 15;
        max = 15;
    }

    public static int max;
}
