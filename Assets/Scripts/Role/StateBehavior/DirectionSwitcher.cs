using UnityEngine;
using System.Collections;

public class DirectionSwitcher : StateMachineBehaviour {

    public enum DirectionType {
        Axis,
        Quadrant,
        XOnly,
        YOnly,
    }

    public static Vector2 Target = Vector2.right;
    public DirectionType DirectionApply;

    private Vector2 quadrant() {
        float x = Target.x >= 0 ? 1 : -1,
              y = Target.y > 0 ? 1 : -1;
        return new Vector2(x, y);
    }

    private Vector2 axis() {
        float x = Mathf.Abs(Target.x) >= Mathf.Abs(Target.y) ? Target.x / Mathf.Abs(Target.x) : 0,
              y = Mathf.Abs(Target.x) >= Mathf.Abs(Target.y) ? 0 : Target.y / Mathf.Abs(Target.y);

        return new Vector2(x, y);
    }

    private Vector2 xOnly() {
        float x = Target.x >= 0 ? 1 : -1,
              y = 0;

        return new Vector2(x, y);
    }

    private Vector2 yOnly() {
        float x = 0,
              y = Target.y >= 0 ? 1 : -1;

        return new Vector2(x, y);
    }

    private Vector2 generateParam() {
        Vector2 param;

        switch (DirectionApply) {
            case DirectionType.Axis:
                param = axis();
                break;
            case DirectionType.Quadrant:
                param = quadrant();
                break;
            case DirectionType.XOnly:
                param = xOnly();
                break;
            case DirectionType.YOnly:
                param = yOnly();
                break;
            default:
                param = Vector2.zero;
                break;
        }

        return param;
    }

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Vector2 param = generateParam();

        animator.SetFloat("TargetX", param.x);
        animator.SetFloat("TargetY", param.y);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Vector2 param = generateParam();

        animator.SetFloat("TargetX", param.x);
        animator.SetFloat("TargetY", param.y);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash) {
        Vector2 param = generateParam();

        animator.SetFloat("TargetX", param.x);
        animator.SetFloat("TargetY", param.y);
    }

}
