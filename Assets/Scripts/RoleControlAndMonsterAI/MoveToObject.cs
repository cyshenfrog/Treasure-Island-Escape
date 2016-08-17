using UnityEngine;
using System.Collections;

public class MoveToObject : MonoBehaviour {

    private GameObject role;

    void Start() {
        role = GameObject.Find("Role");
    }

    void OnMouseDown() {
        role.GetComponent<RoleController>().MoveToTarget(transform.position);
    }

    void OnDestroy() {
        if (role != null)
            role.GetComponent<RoleController>().CancelTarget();
    }
}
