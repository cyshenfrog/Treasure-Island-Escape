using UnityEngine;

public class RoleController : MonoBehaviour {

    /// # basic data
    public float Speed;


    private GameObject RoleCamera;       /// # main camera

    private bool triggerToMove = false;
    private Vector3 coordinateTarget = Vector3.zero;  /// # be used to moveToTarget, decide where role will move to 

    private bool isMouseHold = false;

	void Start () {
        RoleCamera = GameObject.Find("Main Camera");
    }

	void Update () {
        //Speed = Role.GetRoleData(Carceer.None).MoveSpeed;

        /// # let camera follow role  
        RoleCamera.transform.position = new Vector3(transform.position.x, transform.position.y, RoleCamera.transform.position.z);

        /// # two ways to move
        /// # moveToTarget when trigger is true



        if (!triggerToMove) {
            if (Input.GetMouseButton(0))
                mouseHold();
            else
                move();
        }
        else {
            moveToTarget();
        }
        
    }
    void OnGUI() {
        //Event e = Event.current;
        //Debug.Log(e.mousePosition);
    }

    private void mouseHold() {
        if (Input.GetMouseButton(0)) {

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
    }

    //role move by keyboard
    private void move() {

        Vector3 pos;                             /// # record the position of role
        bool w = Input.GetKey("w");        
        bool a = Input.GetKey("a");
        bool s = Input.GetKey("s");
        bool d = Input.GetKey("d");
        string key = w.GetHashCode().ToString()  /// # change key code to a string key
            + a.GetHashCode().ToString() 
            + s.GetHashCode().ToString() 
            + d.GetHashCode().ToString();

        /// # according string key decide role's movement
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

    private void move(string key)
    {

        Vector3 pos;   /// # record the position of role

        /// # according string key decide role's movement
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


    /// # let role move to target
    private void moveToTarget() {
        if (transform.position == coordinateTarget || Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
            triggerToMove = false;
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

    /// # api that let role move to target
    public void MoveToTarget(Vector3 target) {
        triggerToMove = true;
        coordinateTarget = target;
    }

    /// # api that cancel to let role move to target
    public void CancelTarget() {
        triggerToMove = false;
    }
}
