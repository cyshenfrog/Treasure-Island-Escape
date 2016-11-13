using UnityEngine;
using System.Collections;

public class MController_5 : MController_3 {

    //詭異的蘑菇

    private float bombCountBackwards = 2;
    private bool oneshotAttack = true;

    public override void Attack() {
        if (Vector2.Distance(role.transform.position, transform.position) > Data.AttackRange)
            State = MonsterState.IDlE;
        else {
            bombCountBackwards -= Time.deltaTime;
            animator.SetTrigger("Blink");
            if (bombCountBackwards <= 0 && oneshotAttack) {
                animator.SetTrigger("Boom");
                role.GetComponent<RoleController>().BeAttacked((int)Data.Attack, transform.position);
                oneshotAttack = false;
            }
        }
    }

    public override void Idle() {
        base.Idle();
        bombCountBackwards = 2;
    }
}
