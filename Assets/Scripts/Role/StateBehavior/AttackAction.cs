using UnityEngine;
using System.Collections;

public class AttackAction : StateMachineBehaviour {

    public bool IsDodge = false;
    public bool IsRole = true;
    public Role RObj { set; get; }
    public RoleController RController { set; get; }
    public Monster MObj { set; get; }
    public MonsterController MController { set; get; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (!IsDodge) {
            MController.EnterBeAttack();
            Debug.Log("Monster Hp : " + MObj.Hp + "/" + MObj.MaxHp);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (IsRole) {
            animator.gameObject.transform.parent.GetComponent<RoleController>().State = RoleState.IDLE;
        }
    }

    public void TriggeredController(MonsterController controller) {
        MObj = controller.Data;
        MController = controller;
    }

    public void TriggeredController(RoleController controller) {
        RObj = controller.Data;
        RController = controller;
    }
}
