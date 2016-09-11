using UnityEngine;
using System.Collections;

public class MController_3 : MController_2 {

    protected override void Update() {
        base.Update();
        if (Vector2.Distance(role.transform.position, transform.position) < data.AttackRange)
            State = MonsterState.ATTACK;
    }

}
