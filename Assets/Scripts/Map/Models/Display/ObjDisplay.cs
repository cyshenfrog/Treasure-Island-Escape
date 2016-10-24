using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class ObjDisplay : MonoBehaviour, IPointerClickHandler
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

    public void Enable() { }

    public void Updated() { }

    public void OnPointerClick(PointerEventData e)
    {
        if (e.button == PointerEventData.InputButton.Left)
        {
            //to let role come here
            Role.MoveToTarget(transform.position);
            OnPick();
        }
    }

    public float OnPick()
    {
        ra.OnPickeds[od.State](od);
        
        Invoke("OnPickFinished", ra.GatherTime);

        //should return something??
        return ra.GatherTime;
    }

    void OnPickFinished()
    {
        ra.OnPickFinisheds[od.State](od);
    }


    public RoleController Role;

    protected ObjData od;
    protected SpriteRenderer sr;
    protected BoxCollider bc;

    protected Action<Transform> CollisionAction;
    protected Action<int> IdleAction;
    protected Action ClickAction, EnableAction, UpdatedAction;

    protected bool isUsed = false;

    static float cellWidthInWC, cellHeightInWC;

    ResourceAttribute ra;
}
