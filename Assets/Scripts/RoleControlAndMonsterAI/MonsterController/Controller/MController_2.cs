using UnityEngine;
using System.Collections;

public class MController_2 : MonsterController {

    //被打攻擊
    

    void OnMouseDown() {
        if (role.GetComponent<RoleController>().Attack(data, transform.localPosition)) {
            State = MonsterState.BEATTACK;

            //monster die
            if (data.Hp == 0) Die();
        }
    }

    public override void BeAttacked() {
        base.BeAttacked();
        if (beAttackedState == STATE_FINISFANIM) {
            State = MonsterState.ATTACK;
        }      
    }

    public override void Attack() {
        base.Attack();

    }
}
