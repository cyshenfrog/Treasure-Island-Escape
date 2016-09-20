using UnityEngine;
using System.Collections;

public class MController_6 : MController_2 {

    //茶鵝
    private int collectTimes = 0;
    
    protected override void Update() {
        //BroadcastMessage("stateMachine", SendMessageOptions.DontRequireReceiver);
        stateMachine();
    }

    protected override void OnMouseDown() {
        if (State == MonsterState.IDlE && role.GetComponent<RoleController>().Collect(transform.position)) {
            collectTimes++;
            Debug.Log("collect times: " + collectTimes);
            if (collectTimes >= 2)
            {
                State = MonsterState.ATTACK;
                collectTimes = 0;

            }
        }
        else if(State != MonsterState.IDlE) base.OnMouseDown();
        
    }
}
