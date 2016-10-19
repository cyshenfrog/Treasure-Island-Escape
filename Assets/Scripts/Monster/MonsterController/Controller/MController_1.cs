using UnityEngine;
using System.Collections;

public class MController_1 : MonsterController {

    //被打逃跑

    //be attack
    private Vector2 beAttackedPosition;
    private Vector2 relativePosition;

    private bool cacheRolePosition = false;
    
    private int roleDirectionX;
    private int roleDirectionY;

    protected override void Awake() {
        base.Awake();
        Data = MonsterManager.Create(id);
    }

    protected override void OnMouseDown() {
        //check pointer is not over ugui
        base.OnMouseDown();
        role.GetComponent<RoleController>().Attack(this, transform.localPosition);

    }

    public override void BeAttacked() {
        //monster die
        if (Data.Hp <= 0) {
            State = MonsterState.DEAD;
            return;
        }

        if (!cacheRolePosition) {
            cacheRolePosition = true;
            beAttackedPosition = role.transform.position;
            relativePosition = role.transform.position - transform.position;
        }

        runAway();      
    }

    protected virtual void runAway() {
        if (Vector2.Distance(beAttackedPosition, transform.position) < AnimalConstant.RunAwayDistance) {

            if (lastTimes <= 0) {
                lastTimes = Random.Range(10, 40);

                do {
                    //random run direction (excludes role direction)
                    randomUnit = Random.insideUnitCircle;

                } while (Mathf.Sign(randomUnit.x) == Mathf.Sign(relativePosition.x) || Mathf.Sign(randomUnit.y) == Mathf.Sign(relativePosition.y));
            }

            Run();
        }
        else {
            //finish run state
            State = MonsterState.IDlE;
            cacheRolePosition = false;           
        }
    }  
}
