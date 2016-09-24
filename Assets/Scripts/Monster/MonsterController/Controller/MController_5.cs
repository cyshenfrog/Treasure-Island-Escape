using UnityEngine;
using System.Collections;

public class MController_5 : MController_3 {

    //詭異的蘑菇

    private float bombCountBackwards = 3;

    public override void Attack() {
        if (Vector2.Distance(role.transform.position, transform.position) > data.AttackRange) {
            State = MonsterState.IDlE;
            bombCountBackwards = 3;
        }
        else {
            bombCountBackwards -= Time.deltaTime;
            //play count backwards anim
            if (bombCountBackwards <= 0) {
                //play bomb anim
                role.GetComponent<RoleController>().BeAttacked((int)data.Attack, transform.position);
                Destroy(gameObject);
            }
        }
    }
}
