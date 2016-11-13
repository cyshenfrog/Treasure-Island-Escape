using UnityEngine;
using System.Collections;

public class MController_2 : MonsterController {

    //被打攻擊

    private float attackSpace = 0;

    protected override void OnMouseDown() {
        //check pointer is not over ugui
        base.OnMouseDown();
        role.GetComponent<RoleController>().Attack(this, transform.position);
    }

    public override void BeAttacked() {
        if (Data.Hp == 0) {
            State = MonsterState.DEAD;
            return;
        }

        State = MonsterState.ATTACK;       
    }

    protected virtual void chase() {
        randomUnit = role.transform.position - transform.position;
        Run();
    }

    public override void Attack() {
        base.Attack();
        if (Vector2.Distance(transform.position, role.transform.position) > Data.AttackRange
            && Vector2.Distance(transform.position, role.transform.position) <= AnimalConstant.ChaseDistance)
            chase();
        else if (Vector2.Distance(transform.position, role.transform.position) > AnimalConstant.ChaseDistance) {
            State = MonsterState.IDlE;
            lastTimes = 40;
        }    
    }
}
