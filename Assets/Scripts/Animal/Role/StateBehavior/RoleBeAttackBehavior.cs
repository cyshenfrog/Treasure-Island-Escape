using UnityEngine;
using System.Collections;

public class RoleBeAttackBehavior : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.transform.parent.GetComponent<RoleController>().State = RoleState.IDLE;
    }
}
