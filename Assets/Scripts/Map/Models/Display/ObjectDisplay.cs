using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class ObjDisplay : MonoBehaviour
{
    public ObjDisplay(ResourceAttribute ra, ObjData od)
    {
        this.ra = ra;
        this.od = od;

        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();


        bc.size = ra.Width * Vector3.right + ra.Height * Vector3.up + Vector3.forward;
        sr.sprite = ra.Sprites[0];

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
}
