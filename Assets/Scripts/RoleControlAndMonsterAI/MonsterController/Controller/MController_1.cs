using UnityEngine;
using System.Collections;

public class MController_1 : MonsterController {

    //被打逃跑

    //be attack
    private Vector2 rolePosition;
    private bool cacheRolePosition = false;
    
    private int roleDirectionX;
    private int roleDirectionY;

    void OnMouseDown() {
        if (role.GetComponent<RoleController>().Attack(data, transform.localPosition)) {

            State = MonsterState.BEATTACK;

            //refresh data of be attacked
            lastTimes = 0;
            beAttackedState = STATE_STARTPLAYANIM;
            cacheRolePosition = false;

            //monster die
            if (data.Hp == 0) Die();
        }
    }

    public override void BeAttacked()
    {
        base.BeAttacked();
        if(beAttackedState == STATE_FINISFANIM) runAway();
    }

    protected virtual void runAway() {
        //check if trigger run action
        if (!cacheRolePosition)
        {
            //if not , record role direction data
            rolePosition = role.transform.localPosition;
            cacheRolePosition = true;
            float deltaX = rolePosition.x - transform.localPosition.x;
            float deltaY = rolePosition.y - transform.localPosition.y;

            roleDirectionX = deltaX == 0 ? AnimalConstant.Origin : deltaX > 0 ? AnimalConstant.Right : AnimalConstant.Left;
            roleDirectionY = deltaY == 0 ? AnimalConstant.Origin : deltaY > 0 ? AnimalConstant.Up : AnimalConstant.Down;
        }

        float distance = Vector2.Distance(rolePosition, transform.localPosition);

        if (distance < AnimalConstant.RunAwayDistance) {

            if (lastTimes == 0) {
                lastTimes = Random.Range(10, 40);

                do
                {
                    //random run direction (excludes role direction)
                    randomDirection = Random.Range(0, 4);
                } while (randomDirection == roleDirectionX || randomDirection == roleDirectionY);
            }
            Run();
        }
        else
        {
            //finish run state
            cacheRolePosition = false;
            State = MonsterState.IDlE;
            beAttackedState = STATE_STARTPLAYANIM;
        }

    }

    
}
