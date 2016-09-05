using UnityEngine;
using System.Collections.Generic;

public class RoleController : MonoBehaviour {

    //basic data
    public float Speed;

    private GameObject RoleCamera;       //main camera
    private Role data;                   //role data

    private Vector3 coordinateTarget = Vector3.zero;  //be used to moveToTarget, decide where role will move to 
    private List<Vector2> objPosition = new List<Vector2>();

    public RoleState State { set; get; }

    void Awake() {
        data = new Role();
        data.Attack = 100;
    }
	void Start () {
        RoleCamera = GameObject.Find("Main Camera");
        State = RoleState.IDLE;
    }

    /*void FixedUpdate() {
        //let camera follow role  
        RoleCamera.transform.position = transform.localPosition + new Vector3(0, 0, -10);
    }*/

	void Update () {
        //Speed = Role.GetRoleData(Carceer.None).MoveSpeed;

        switch (State) {
            case RoleState.IDLE:
                move();
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

    private void mouseHold() {
        if (Input.GetMouseButton(0))
        {

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = transform.position.z;

            Vector3 degree = worldPoint - transform.position;

            float atan = Mathf.Atan2(degree.y, degree.x) * Mathf.Rad2Deg;

            if (atan > -22.5f && atan <= 22.5f)         //right
                move("0001");
            else if (atan > 22.5f && atan <= 67.5f)     //right-top
                move("1001");
            else if (atan > 67.5f && atan <= 112.5f)    //top
                move("1000");
            else if (atan > 112.5 && atan <= 157.5f)    //left-top
                move("1100");
            else if (atan > 157.5f || atan <= -157.5f)  //left
                move("0100");
            else if (atan <= -22.5f && atan > -67.5f)   //right-bottom
                move("0011");
            else if (atan <= -67.5 && atan > -112.5f)   //bottom
                move("0010");
            else if (atan <= -112.5f && atan > -157.5f) //left-bottom
                move("0110");

        }
        else
            State = RoleState.IDLE;
    }

    //role move by keyboard
    private void move() {

        Vector3 pos;                             //record the position of role
        bool w = Input.GetKey("w");        
        bool a = Input.GetKey("a");
        bool s = Input.GetKey("s");
        bool d = Input.GetKey("d");
        string key = w.GetHashCode().ToString()  //change key code to a string key
            + a.GetHashCode().ToString() 
            + s.GetHashCode().ToString() 
            + d.GetHashCode().ToString();

        //according string key decide role's movement
        switch (key) {
            case "1000":
                pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y + Time.deltaTime * Speed, pos.z);
                break;
            case "0100":
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed, pos.y, pos.z);
                break;
            //below deal with moving diagonally
            case "0010":
                pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y - Time.deltaTime * Speed, pos.z);
                break;
            case "0001":
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed, pos.y, pos.z);
                break;
            case "1100":
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "0110":
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "0011":
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "1001":
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
        }
    }

    private void move(string key)
    {

        Vector3 pos;   //record the position of role

        //according string key decide role's movement
        switch (key) {
            case "1000":
                pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y + Time.deltaTime * Speed, pos.z);
                break;
            case "0100":
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed, pos.y, pos.z);
                break;
            /// # below deal with moving diagonally
            case "0010":
                pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y - Time.deltaTime * Speed, pos.z);
                break;
            case "0001":
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed, pos.y, pos.z);
                break;
            case "1100":
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "0110":
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "0011":
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y - Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
            case "1001":
                pos = transform.position;
                transform.position = new Vector3(pos.x + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.y + Time.deltaTime * Speed * (Mathf.Sqrt(2) / 2), pos.z);
                break;
        }
    }


    //let role move to target
    private void moveToTarget() {
        if (transform.position == coordinateTarget || Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
            State = RoleState.IDLE; //triggerToMove = false;
        else
        {
            if (coordinateTarget.x == transform.position.x || coordinateTarget.y == transform.position.y)
                transform.position = Vector3.MoveTowards(transform.position, coordinateTarget, Time.deltaTime * Speed);
            else
            {
                float x = coordinateTarget.x - transform.position.x;
                float y = coordinateTarget.y - transform.position.y;
                float deltaX = Mathf.Abs(x);
                float deltaY = Mathf.Abs(y);

                Vector3 relayStation = deltaX > deltaY ?
                    x > 0 ? new Vector3(transform.position.x + deltaY, coordinateTarget.y, 0) : new Vector3(transform.position.x - deltaY, coordinateTarget.y, 0) :
                    y > 0 ? new Vector3(coordinateTarget.x, transform.position.y + deltaX, 0) : new Vector3(coordinateTarget.x, transform.position.y - deltaX, 0);

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
            if (Vector2.Distance(objPosition[i], transform.localPosition) < minDistance && Vector2.Distance(objPosition[i], transform.localPosition) <= AnimalConstant.RolePickUpRange) {
                minDistance = Vector2.Distance(objPosition[i], transform.localPosition);
                index = i;
            }
        }

        if(index != -1)
            MoveToTarget(objPosition[index]);

    }

    public void CancelMoveToTarget() {
        State = RoleState.IDLE;
        objPosition = new List<Vector2>();
    }

    public bool Attack(Monster m, Vector3 target) {

        //State = RoleState.ATTACK;

        if (Vector2.Distance(transform.localPosition, target) <= AnimalConstant.RoleAttackRange) {
            m.Hp -= (int)data.Attack;
            Debug.Log(m.Hp + "/" + m.MaxHp);

            //play animation
            //animation callback to unlock state;

            return true;
        }
        else {
            //State = RoleState.ATTACK; ???
            return false;
        }
    }
}
