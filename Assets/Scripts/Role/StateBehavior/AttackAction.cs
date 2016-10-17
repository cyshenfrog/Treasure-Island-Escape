using UnityEngine;
using System.Collections;

public class AttackAction : StateMachineBehaviour {

    public bool IsDodge = false;
    public bool IsRole = true;
    public Role RObj { set; get; }
    public Monster MObj { set; get; }
    public int Attack { set; get; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!IsDodge) MObj.Hp -= Attack;
        Debug.Log("Monster Hp : " + MObj.Hp + "/" + MObj.MaxHp);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(IsRole) animator.gameObject.transform.parent.GetComponent<RoleController>().State = RoleState.IDLE;
    }

    public void SetObject(Monster obj, int attack) {
        MObj = obj;
        Attack = attack;
    }

    public void SetObject(Role obj, int attack) {
        RObj = obj;
        Attack = attack;
    }
}
