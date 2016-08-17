using UnityEngine;

public class RoleController : MonoBehaviour {

    public float Speed;

    private GameObject RoleCamera;

    private bool triggerToMove = false;
    private Vector3 coordinateTarget = Vector3.zero;

	void Start () {
        RoleCamera = GameObject.Find("Main Camera");
    }

	void Update () {
        //Speed = Role.GetRoleData(Carceer.None).MoveSpeed;
        RoleCamera.transform.position = new Vector3(transform.position.x, transform.position.y, RoleCamera.transform.position.z);

        if (!triggerToMove) {
            move();
        }
        else {
            moveToTarget();
        }
    }
    void OnGUI() {
        
    }

    //腳色移動
    private void move() {

        Vector3 pos;
        bool w = Input.GetKey("w");        
        bool a = Input.GetKey("a");
        bool s = Input.GetKey("s");
        bool d = Input.GetKey("d");
        string key = w.GetHashCode().ToString() 
            + a.GetHashCode().ToString() 
            + s.GetHashCode().ToString() 
            + d.GetHashCode().ToString();

        switch (key) {
            case "1000":
                pos = transform.position;
                transform.position = new Vector3(pos.x, pos.y + Time.deltaTime * Speed, pos.z);
                break;
            case "0100":
                pos = transform.position;
                transform.position = new Vector3(pos.x - Time.deltaTime * Speed, pos.y, pos.z);
                break;
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

    //移動到指定座標
    public void MoveToTarget(Vector3 target) {
        triggerToMove = true;
        coordinateTarget = target;
    }

    //取消自動移動
    public void CancelTarget() {
        triggerToMove = false;
    }
}
