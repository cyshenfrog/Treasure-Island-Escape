using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class ObjDisplay : MonoBehaviour
{
    public static void Init()
    {
        cellWidthInWC = GroundController.CellWidthInWC;
        cellHeightInWC = GroundController.CellHeightInWC;
    }

    public void Init(ResourceAttribute ra, ObjData od)
    {
        this.ra = ra;
        this.od = od;

        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();

        float width = ra.Width * cellWidthInWC, height = ra.Height * cellHeightInWC;
        bc.size = width * Vector3.right + height * Vector3.up + Vector3.forward;
        bc.center = height * .5f * Vector3.up;
        sr.sprite = ra.Sprites[0];

        cellWidthInWC = GroundController.CellWidthInWC;
        cellHeightInWC = GroundController.CellHeightInWC;

        transform.localPosition = od.Position.x * cellWidthInWC * Vector3.right + od.Position.y * cellHeightInWC * Vector3.up;

        //to set action....
    }


    public void Idle(int mode) { }
    public void Click() { }
    public void Collision(Transform collision) { }
    public void Enable() { }
    public void Updated() { }
    
    protected ObjData od;
    protected ResourceAttribute ra;
    protected SpriteRenderer sr;
    protected BoxCollider bc;

    protected Action<Transform> CollisionAction;
    protected Action<int> IdleAction;
    protected Action ClickAction, EnableAction, UpdatedAction;

    static float cellWidthInWC, cellHeightInWC;
}
