using UnityEngine;
using System.Collections;

public class MController_2 : MonsterController {

    //被打攻擊

    private float attackSpace = 0;
    private float chaseDistance = 10;

    void Start() {
    }

    void OnMouseDown() {
        if (role.GetComponent<RoleController>().Attack(data, transform.localPosition)) {
            State = MonsterState.BEATTACK;

            //monster die
            if (data.Hp == 0) Die();
        }
    }



    protected void chase() {
        Vector2 target = role.transform.position - transform.position;
        float degree = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        if (degree < 45 && degree >= -45) randomDirection = AnimalConstant.Right;
        else if (degree < 135 && degree >= 45) randomDirection = AnimalConstant.Up;
        else if (degree >= 135 || degree < -135) randomDirection = AnimalConstant.Left;
        else randomDirection = AnimalConstant.Down;
        Move();
    }

    public override void BeAttacked() {
        base.BeAttacked();
        if (beAttackedState == STATE_FINISFANIM) {
            State = MonsterState.ATTACK;
        }      
    }

    public override void Attack() {
        if (attackSpace <= 0 && Vector2.Distance(transform.position, role.transform.position) <= data.AttackRange)
        {
            base.Attack();
            attackSpace = data.AttackSpace;
        }
        else if (attackSpace > 0 && Vector2.Distance(transform.position, role.transform.position) <= data.AttackRange) {
            attackSpace -= Time.deltaTime;
        }
        else if (Vector2.Distance(transform.position, role.transform.position) > data.AttackRange && Vector2.Distance(transform.position, role.transform.position) <= chaseDistance)
        {
            chase();
        }
        else if (Vector2.Distance(transform.position, role.transform.position) > chaseDistance)
        {
            State = MonsterState.IDlE;
            lastTimes = 40;
        }

        
        
    }
}
