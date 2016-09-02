using UnityEngine;
using System.Collections;

public class MoveToObject : MonoBehaviour {

    private GameObject role;  //the role that player control

    void Start() {
        role = GameObject.Find("Role");
    }

    void OnMouseDown() {
        //call api to move the position of object 
        role.GetComponent<RoleController>().MoveToTarget(transform.position);
    }

    void OnCollisionEnter() {
        if (role != null)
            role.GetComponent<RoleController>().State = RoleState.IDLE;
    }

    void OnDestroy()
    {
        if (role != null)
            role.GetComponent<RoleController>().State = RoleState.IDLE;
    }

}
