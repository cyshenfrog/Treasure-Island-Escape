using UnityEngine;
using System.Collections;

public class M5ExitBoom : StateMachineBehaviour {

	//OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.transform.parent.GetComponent<MonsterController>().State = MonsterState.DEAD;
	}
}
