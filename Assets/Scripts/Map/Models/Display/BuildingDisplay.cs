using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class BuildingDisplay : ObjDisplay
{
    public BuildingDisplay(ResourceAttribute ra, ObjData od): base (ra, od)
    {

    }
}
