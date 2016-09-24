using UnityEngine;
using System.Collections;

public class MoveToObject : MonoBehaviour {

    private GameObject role;  //the role that player control

    static public Item pickUpTarget;

    void Start() {
        role = GameObject.Find("Role");
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            role.GetComponent<RoleController>().SendObjPosition(transform.localPosition);
        }
    }

    void OnMouseDown() {
        //call api to move the position of object 
        role.GetComponent<RoleController>().MoveToTarget(transform.position);
        pickUpTarget = transform.GetComponent<Item>();
    }

    /*
    void OnCollisionEnter(Collision c) {
        if (c.gameObject.name == "Role") {
            Destroy(gameObject);
        }
    }*/

    void OnDestroy()
    {
        if (role != null) {
            role.GetComponent<RoleController>().CancelMoveToTarget();
        }
    }

}
