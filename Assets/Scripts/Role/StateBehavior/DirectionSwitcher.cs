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

    private static Vector2 quadrant(Vector2 target) {
        float x = target.x >= 0 ? 1 : -1,
              y = target.y > 0 ? 1 : -1;
        return new Vector2(x, y);
    }

    private static Vector2 axis(Vector2 target) {
        float x = Mathf.Abs(target.x) >= Mathf.Abs(target.y) ? target.x / Mathf.Abs(target.x) : 0,
              y = Mathf.Abs(target.x) >= Mathf.Abs(target.y) ? 0 : target.y / Mathf.Abs(target.y);

        return new Vector2(x, y);
    }

    private static Vector2 xOnly(Vector2 target) {
        float x = target.x >= 0 ? 1 : -1,
              y = 0;
        return new Vector2(x, y);
    }

    private static Vector2 yOnly(Vector2 target) {
        float x = 0,
              y = target.y >= 0 ? 1 : -1;

        return new Vector2(x, y);
    }

    public static Vector2 DirectionSwitch(Vector2 vector, DirectionType dir) {
        switch (dir) {
            case DirectionType.Axis:
                return axis(vector);
            case DirectionType.Quadrant:
                return quadrant(vector);
            case DirectionType.XOnly:
                return xOnly(vector);
            case DirectionType.YOnly:
                return yOnly(vector);
            default:
                return Vector2.zero;
        }
    }

    private Vector2 generateParam() {
        Vector2 param;

        switch (DirectionApply) {
            case DirectionType.Axis:
                param = axis(Target);
                break;
            case DirectionType.Quadrant:
                param = quadrant(Target);
                break;
            case DirectionType.XOnly:
                param = xOnly(Target);
                break;
            case DirectionType.YOnly:
                param = yOnly(Target);
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
