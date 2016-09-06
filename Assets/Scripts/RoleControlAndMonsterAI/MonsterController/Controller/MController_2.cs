using UnityEngine;
using System.Collections;

public class MController_2 : MonsterController {

    //被打攻擊

    void OnMouseDown() {
        if (role.GetComponent<RoleController>().Attack(data, transform.localPosition)) {
            State = MonsterState.BEATTACK;
        }
    }

    public override void BeAttacked() {
        
    }
}
