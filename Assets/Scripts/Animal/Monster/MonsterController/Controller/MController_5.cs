using UnityEngine;
using System.Collections;

public class MController_5 : MController_3 {

    //詭異的蘑菇
    private bool oneshotAttack = true;

    public override void Attack() {
        if (Vector2.Distance(role.transform.position, transform.position) > Data.AttackRange) {
            State = MonsterState.IDlE;
            Data.AttackSpace = Data.BaseAttackSpace;
        }
        else {
            animator.SetTrigger("Blink");
            base.Attack();
        }
    }

}
