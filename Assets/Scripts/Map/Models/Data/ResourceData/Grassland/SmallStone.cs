using UnityEngine;
using System.Collections;

public class SmallStone : ResourceData
{
    public SmallStone(Vector3 center, int oID)
    {
        this.center = center;
        this.oID = oID;

        //Max = 15;
        max = 15;
    }

    public static int max;
}
