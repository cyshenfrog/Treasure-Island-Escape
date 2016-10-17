using UnityEngine;
using System.Collections.Generic;

public class RoleController : MonoBehaviour {

    //basic data
    public float Speed;
    public Animator anim;

    private Role data;                   //role data

    private Vector3 coordinateTarget = Vector3.zero;  //be used to moveToTarget, decide where role will move to 
    private List<Vector2> objPosition = new List<Vector2>();

    private float wink = AnimalConstant.RoleWinkColdDown;
    private float gripper = AnimalConstant.RoleGripperColdDown;
    private float tyrants = AnimalConstant.RoleTyrants;

    public RoleState State { set; get; }

    void Awake() {
        data = new Chef();
        data.Attack = 10;
        data.MaxHp = 1000;
        data.Hp = 1000;
    }
	void Start () {
        State = RoleState.IDLE;
    }

	void Update () { 
        //Speed = Role.GetRoleData(Carceer.None).MoveSpeed;
        switch (State) {
            case RoleState.IDLE:
            case RoleState.TYRANTS:
                idle();
                break;
            case RoleState.MOUSEHOLD:
                mouseHold();
                break;
            case RoleState.PICKUP:
                moveToTarget();
                break;
            default:
                break;
        }
        
    }

    private void playAnimation(string triggerName, Vector2 targetWorldPosition) {
        Vector2 direction = targetWorldPosition - (Vector2)transform.position;
        DirectionSwitcher.Target = direction;
        anim.SetTrigger(triggerName);
    }

    private void mouseHold() {
        if (Input.GetMouseButton(0)) {

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = transform.position.z;

            Vector3 degree = worldPoint - transform.position;
            
            float atan = Mathf.Atan2(degree.y, degree.x) * Mathf.Rad2Deg;

            if (atan > -22.5f && atan <= 22.5f)         //right
                control(1, 0);
            else if (atan > 22.5f && atan <= 67.5f)     //right-top
                control(1, 1);
            else if (atan > 67.5f && atan <= 112.5f)    //top
                control(0, 1);
            else if (atan > 112.5 && atan <= 157.5f)    //left-top
                control(-1, 1);
            else if (atan > 157.5f || atan <= -157.5f)  //left
                control(-1, 0);
            else if (atan <= -22.5f && atan > -67.5f)   //right-bottom
                control(1, -1);
            else if (atan <= -67.5 && atan > -112.5f)   //bottom
                control(0, -1);
            else if (atan <= -112.5f && atan > -157.5f) //left-bottom
                control(-1, -1);
                
        }
        else State = RoleState.IDLE;
    }

    //role move by keyboard
    private void idle() {

        if (State == RoleState.TYRANTS) {

            tyrants -= Time.deltaTime;
            if (tyrants <= 0) {
                State = RoleState.IDLE;
                tyrants = AnimalConstant.RoleTyrants;
            }
        }

        control(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


    }

    /// <summary>
    /// move to target(x, y) and play animation
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void control(float x, float y) {
        if (x != 0 || y != 0) {
            //build unit vector
            Vector3 unit = new Vector3(x, y, 0) / Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
            
            //reset idle action constant
            wink = AnimalConstant.RoleWinkColdDown;
            gripper = AnimalConstant.RoleGripperColdDown;
            
            //play animation "Walk"
            DirectionSwitcher.Target = unit;
            anim.SetTrigger("Walk");

            //move
            transform.Translate(unit * Time.deltaTime * Speed);
        }
        else {
            //play animation "idle"
            anim.SetTrigger("Idle");

            wink -= Time.deltaTime;
            gripper -= Time.deltaTime;

            //play animation "idle action" if time is coming
            if (gripper <= 0) {
                anim.SetTrigger("IdleAction2");
                gripper = AnimalConstant.RoleGripperColdDown;
            }
            if (wink <= 0) {
                anim.SetTrigger("IdleAction1");
                wink = AnimalConstant.RoleWinkColdDown;
            }
        }
    }

    //let role move to target
    private void moveToTarget() {
        if (transform.position == coordinateTarget || Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
            State = RoleState.IDLE; //triggerToMove = false;
        else
        {
            if (coordinateTarget.x == transform.position.x || coordinateTarget.y == transform.position.y) {

                if (coordinateTarget.x > transform.position.x) anim.SetTrigger("walkRight");
                else if (coordinateTarget.x < transform.position.x) anim.SetTrigger("walkLeft");
                else if (coordinateTarget.y > transform.position.y) anim.SetTrigger("walkUp");
                else if (coordinateTarget.y < transform.position.y) anim.SetTrigger("walkDown");
                transform.position = Vector3.MoveTowards(transform.position, coordinateTarget, Time.deltaTime * Speed);
            }
            else {
                float x = coordinateTarget.x - transform.position.x;
                float y = coordinateTarget.y - transform.position.y;
                float deltaX = Mathf.Abs(x);
                float deltaY = Mathf.Abs(y);

                Vector3 relayStation = deltaX > deltaY ?
                    x > 0 ? new Vector3(transform.position.x + deltaY, coordinateTarget.y, 0) : new Vector3(transform.position.x - deltaY, coordinateTarget.y, 0) :
                    y > 0 ? new Vector3(coordinateTarget.x, transform.position.y + deltaX, 0) : new Vector3(coordinateTarget.x, transform.position.y - deltaX, 0);

                if (coordinateTarget.x > transform.position.x) anim.SetTrigger("walkRight");
                else anim.SetTrigger("walkLeft");

                transform.position = Vector3.MoveTowards(transform.position, relayStation, Time.deltaTime * Speed);

            }
        }
        
    }

    /// <summary>
    /// let role move to target
    /// </summary>
    /// <param name="target"></param>
    public void MoveToTarget(Vector2 target) {
        State = RoleState.PICKUP;
        coordinateTarget = target;
    }

    public void SendObjPosition(Vector2 pos) {
        objPosition.Add(pos);
        float minDistance = 1000000;
        int index = -1;
        for (int i = 0;i < objPosition.Count;i++) {
            if (Vector2.Distance(objPosition[i], transform.position) < minDistance && Vector2.Distance(objPosition[i], transform.position) <= AnimalConstant.RolePickUpRange) {
                minDistance = Vector2.Distance(objPosition[i], transform.position);
                index = i;
            }
        }

        if(index != -1)
            MoveToTarget(objPosition[index]);
           //test manual merge
    }

    public void CancelMoveToTarget() {
        State = RoleState.IDLE;
        objPosition = new List<Vector2>();
    }

    public bool Attack(Monster m, Vector3 target) {

        State = RoleState.ATTACK;

        if (Vector2.Distance(transform.position, target) <= AnimalConstant.RoleAttackRange) {
            anim.GetBehaviour<AttackAction>().SetObject(m, (int)data.Attack);
            playAnimation("Attack", target);
            //animation callback to unlock state;

            return true;
        }
        else {
            State = RoleState.IDLE;
            return false;
        }
    }

    public void BeAttacked(int attack, Vector2 pos) {

        if (State != RoleState.TYRANTS) {
            data.Hp -= attack;
            Debug.Log("Role Hp : " + data.Hp + "/" + data.MaxHp);
            State = RoleState.BEATTACK;
            if (data.Hp <= 0) {
                Die();
            }
            else {
                playAnimation("BeAttack", pos);
            }

        }
    }

    public bool Collect(Vector2 pos) {
        if (Vector2.Distance(transform.position, pos) < AnimalConstant.RoleCollectRange) {
            Debug.Log("collect...");

            playAnimation("Jump", pos);

            return true;
        }
        return false;
    }

    public void Die() {
        State = RoleState.DEAD;
        anim.SetTrigger("Dead");
    }
}
