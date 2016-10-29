using UnityEngine;
using System.Collections;

public class MController_7 : MController_3 {

    //攻城獅

    public GameObject Boss;
    public float BossDistance = 5;

    protected override void Awake() {
        base.Awake();
        //Boss = GameObject.Find("MTEST");
    }

    protected override void Update() {
        if (lastTimes <= 0 && (State == MonsterState.IDlE || State == MonsterState.MOVE)) {
            randomAction = Random.Range(0, 2);
            randomUnit = Random.insideUnitCircle.normalized;
            lastTimes = Random.Range(10, 40);

            State = (MonsterState)randomAction;
        }

        if (Vector2.Distance(transform.position, Boss.transform.position) > BossDistance)
            State = MonsterState.MOVE;

        stateMachine();
    }

    public override void Move() {
        if (Vector2.Distance(transform.position, Boss.transform.position) > 5) {
            Vector2 direction = DirectionSwitcher.DirectionSwitch(Boss.transform.position - transform.position);
            transform.Translate(direction * Data.Speed * Time.deltaTime);
            return;
        }

        base.Move();
    }

}
