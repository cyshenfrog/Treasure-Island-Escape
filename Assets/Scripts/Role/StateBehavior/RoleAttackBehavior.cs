using UnityEngine;
using System.Collections;

public class RoleAttackBehavior : StateMachineBehaviour {

    public Monster MObj { set; get; }
    public MonsterController MController { set; get; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        MController.EnterBeAttack();
        Debug.Log("Monster Hp : " + MObj.Hp + "/" + MObj.MaxHp);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            animator.gameObject.transform.parent.GetComponent<RoleController>().State = RoleState.IDLE;
    }

    public void Controlled(MonsterController controller) {
        MObj = controller.Data;
        MController = controller;
    }
}
