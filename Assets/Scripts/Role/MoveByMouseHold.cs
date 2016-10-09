using UnityEngine;
using System.Collections;

public class MoveByMouseHold : MonoBehaviour {

    void OnMouseDown() {
        GameObject.Find("Role").GetComponent<RoleController>().State = RoleState.MOUSEHOLD;
    }
}
