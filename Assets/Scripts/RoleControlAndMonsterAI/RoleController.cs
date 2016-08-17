using UnityEngine;

public class RoleController : MonoBehaviour {

    /// # basic data
    public float Speed;


    private GameObject RoleCamera;       /// # main camera

    private bool triggerToMove = false;               /// # check that moveToTarget is call
    private Vector3 coordinateTarget = Vector3.zero;  /// # be used to moveToTarget, decide where role will move to 

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
            move();
        }
        else {
            moveToTarget();
        }
    }
    void OnGUI() {
        
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


    /// # let role move to target
    private void moveToTarget() {
        if (transform.position == coordinateTarget || Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) triggerToMove = false;
        else {
            if (coordinateTarget.x == transform.position.x || coordinateTarget.y == transform.position.y) {
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
