using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class MoveToObject : MonoBehaviour, IPointerClickHandler {

    private RoleController role;  //the role that player control
    private Bag bag;

    static public Item pickUpTarget;

    void Start() {
        role = GameObject.Find("Role").GetComponent<RoleController>();
        bag = role.GetComponent<Bag>();
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            role.SendObjPosition(transform.position);
            pickUpTarget = transform.GetComponent<Item>();
        }
        if (role.State != RoleState.PICKUP && pickUpTarget != null)
            pickUpTarget = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //call api to move the position of object 
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            role.MoveToTarget(transform.position);
            pickUpTarget = transform.GetComponent<Item>();
        }
    }
    
    
    void OnDestroy()
    {
        if (role != null) {
            role.CancelMoveToTarget();
        }
    }

}
