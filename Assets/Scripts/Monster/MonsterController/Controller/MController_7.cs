using UnityEngine;
using System.Collections;

public class MController_7 : MController_3 {

    //攻城獅

    public GameObject Boss;
    public float BossDistance = 5;

    protected override void Awake() {
        base.Awake();
        Boss = GameObject.Find("MTEST");
    }

    protected override void Update() {
        if (lastTimes <= 0 && (State == MonsterState.IDlE || State == MonsterState.MOVE)) {
            randomAction = Random.Range(0, 2);
            randomDirection = Random.Range(0, 4);
            lastTimes = Random.Range(10, 40);

            State = (MonsterState)randomAction;
        }

        if (Vector2.Distance(transform.position, Boss.transform.position) > 5)
            State = MonsterState.MOVE;

        stateMachine();
    }

    public override void Move() {
        if (Vector2.Distance(transform.position, Boss.transform.position) > 5) {
            Vector2 target = Boss.transform.position - transform.position;
            float degree = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;

            if (degree < 45 && degree >= -45)
                randomDirection = AnimalConstant.Right;
            else if (degree < 135 && degree >= 45)
                randomDirection = AnimalConstant.Up;
            else if (degree >= 135 || degree < -135)
                randomDirection = AnimalConstant.Left;
            else
                randomDirection = AnimalConstant.Down;

        }

        base.Move();
    }

}
