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

    public override void EnterBeAttack() {
        State = MonsterState.BEATTACK;
        //Debug.Log("??");
        //monster die
        if (Data.Hp == 0) Die();
    }

    protected virtual void chase() {
        randomUnit = role.transform.position - transform.position;
        Run();
    }

    public override void BeAttacked() {
        base.BeAttacked();
        if (beAttackedState == STATE_FINISFANIM) {
            State = MonsterState.ATTACK;
            beAttackedState = STATE_STARTPLAYANIM;
        }      
    }

    public override void Attack() {
        if (attackSpace <= 0 && Vector2.Distance(transform.position, role.transform.position) <= Data.AttackRange) {
            base.Attack();
            attackSpace = Data.AttackSpace;
        }
        else if (attackSpace > 0 && Vector2.Distance(transform.position, role.transform.position) <= Data.AttackRange) {
            attackSpace -= Time.deltaTime;
        }
        else if (Vector2.Distance(transform.position, role.transform.position) > Data.AttackRange && Vector2.Distance(transform.position, role.transform.position) <= AnimalConstant.ChaseDistance) {
            chase();
        }
        else if (Vector2.Distance(transform.position, role.transform.position) > AnimalConstant.ChaseDistance) {
            State = MonsterState.IDlE;
            lastTimes = 40;
        }

        
        
    }
}
