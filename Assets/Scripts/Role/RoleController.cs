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
            case RoleState.BEATTACK:
                beAttacked();
                break;
            default:
                break;

        }

    }

    private void mouseHold() {
        if (Input.GetMouseButton(0)) {

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = transform.position.z;

            Vector3 degree = worldPoint - transform.position;

            float atan = Mathf.Atan2(degree.y, degree.x) * Mathf.Rad2Deg;

            if (atan > -22.5f && atan <= 22.5f)         //right
                keyAction("0001");
            else if (atan > 22.5f && atan <= 67.5f)     //right-top
                keyAction("1001");
            else if (atan > 67.5f && atan <= 112.5f)    //top
                keyAction("1000");
            else if (atan > 112.5 && atan <= 157.5f)    //left-top
                keyAction("1100");
            else if (atan > 157.5f || atan <= -157.5f)  //left
                keyAction("0100");
            else if (atan <= -22.5f && atan > -67.5f)   //right-bottom
                keyAction("0011");
            else if (atan <= -67.5 && atan > -112.5f)   //bottom
                keyAction("0010");
            else if (atan <= -112.5f && atan > -157.5f) //left-bottom
                keyAction("0110");

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

        //Vector3 pos;                             //record the position of role
        bool w = Input.GetKey("w");        
        bool a = Input.GetKey("a");
        bool s = Input.GetKey("s");
        bool d = Input.GetKey("d");
        string key = w.GetHashCode().ToString()  //change key code to a string key
            + a.GetHashCode().ToString() 
            + s.GetHashCode().ToString() 
            + d.GetHashCode().ToString();

        keyAction(key);

        
    }

    private void keyAction(string key) {

        Vector3 pos;   //record the position of role

        //according string key decide role's movement
        switch (key)
        {
            case "1000":
                wink = AnimalConstant.RoleWinkColdDown;
                gripper = AnimalConstant.RoleGripperColdDown;
                anim.SetTrigger("walkUp");
                pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y + Time.deltaTime * Speed, pos.z);
                break;
            case "0100":
                wink = AnimalConstant.RoleWinkColdDown;
                gripper = AnimalConstant.RoleGripperColdDown;
                anim.SetTrigger("walkLeft");
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed, pos.y, pos.z);
                break;
            case "0010":
                wink = AnimalConstant.RoleWinkColdDown;
                gripper = AnimalConstant.RoleGripperColdDown;
                anim.SetTrigger("walkDown");
                pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y - Time.deltaTime * Speed, pos.z);
                break;
            case "0001":
                wink = AnimalConstant.RoleWinkColdDown;
                gripper = AnimalConstant.RoleGripperColdDown;
                anim.SetTrigger("walkRight");
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed, pos.y, pos.z);
                break;
            //below deal with moving diagonally
            case "1100":
                wink = AnimalConstant.RoleWinkColdDown;
                gripper = AnimalConstant.RoleGripperColdDown;
                anim.SetTrigger("walkLeft");
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "0110":
                wink = AnimalConstant.RoleWinkColdDown;
                gripper = AnimalConstant.RoleGripperColdDown;
                anim.SetTrigger("walkLeft");
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "0011":
                wink = AnimalConstant.RoleWinkColdDown;
                gripper = AnimalConstant.RoleGripperColdDown;
                anim.SetTrigger("walkRight");
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "1001":
                wink = AnimalConstant.RoleWinkColdDown;
                gripper = AnimalConstant.RoleGripperColdDown;
                anim.SetTrigger("walkRight");
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            default:
                anim.SetTrigger("stand");
                wink -= Time.deltaTime;
                gripper -= Time.deltaTime;
                if (gripper <= 0)
                {
                    anim.SetTrigger("gripper");
                    gripper = AnimalConstant.RoleGripperColdDown;
                }
                if (wink <= 0)
                {
                    anim.SetTrigger("wink");
                    wink = AnimalConstant.RoleWinkColdDown;
                }

                break;
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

    private void beAttacked()  {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle_stand_right")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("idle_stand_left")) {
            State = RoleState.TYRANTS;
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

        //State = RoleState.ATTACK;

        if (Vector2.Distance(transform.position, target) <= AnimalConstant.RoleAttackRange) {
            m.Hp -= (int)data.Attack;
            Debug.Log("Monster Hp : " + m.Hp + "/" + m.MaxHp);

            //play animation
            //animation callback to unlock state;

            return true;
        }
        else {
            //State = RoleState.ATTACK; ???
            return false;
        }
    }

    public void BeAttacked(int attack, Vector2 pos) {

        if (State != RoleState.TYRANTS) {
            data.Hp -= attack;
            Debug.Log("Role Hp : " + data.Hp + "/" + data.MaxHp);
            State = RoleState.BEATTACK;
            if (data.Hp == 0) {
                Die();
            }
            else {

                if (pos.x > transform.position.x) anim.SetTrigger("beattacked_right");
                else anim.SetTrigger("beattacked_left");
            }

        }
    }

    public bool Collect(Vector2 pos) {
        if (Vector2.Distance(transform.position, pos) < AnimalConstant.RoleCollectRange) {
            Debug.Log("collect...");
            return true;
        }
        return false;
    }

    public void Die() {

    }
}
