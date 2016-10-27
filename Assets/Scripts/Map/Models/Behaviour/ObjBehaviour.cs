using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class ObjBehaviour : MonoBehaviour
{
    public static void Init()
    {
        cellWidthInWC = GroundController.CellWidthInWC;
        cellHeightInWC = GroundController.CellHeightInWC;
    }

    public void Init(ObjData od)
    {
        this.od = od;
        od.Odis = this;
        ra = od.RA;

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
    
    public void Collision(Transform collision) { }

    public void OnEnable()
    {
        //to do something
    }

    public void Updated() { }

    public void OnMouseDown()
    {
        //to check whether there is something over this
        //something over this => return
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //to let role come here
        Role.MoveToTarget(transform.position);

        //to set gather time
        //Role.stay(gather time)

        //to pass this to role
        //, gameobject


        OnPick();
    }

    public void Interrupt()
    {
        //to end the animation
    }

    public void OnPick()
    {
        ra.OnPickeds[od.State](od);
        

        /*
        Invoke("OnPickFinished", ra.GatherTime);

        //should return something??
        return ra.GatherTime;
        */
    }

    public void OnPickFinished()
    {
        ra.OnPickFinisheds[od.State](od);
    }

    public RoleController Role;

    static float cellWidthInWC, cellHeightInWC;

    ObjData od;
    SpriteRenderer sr;
    BoxCollider bc;
    ResourceAttribute ra;

    Action<Transform> CollisionAction;
    Action<int> IdleAction;
    Action ClickAction, EnableAction, UpdatedAction;

    bool isUsed = false;
}
