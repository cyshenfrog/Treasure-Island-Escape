using UnityEngine;
using System.Collections;

public class RoleController : MonoBehaviour {

    public float Speed;
    private GameObject RoleCamera;

	void Start () {
        RoleCamera = GameObject.Find("Main Camera");
        
        
    }

	void Update () {
        RoleCamera.transform.position = new Vector3(transform.position.x, transform.position.y, RoleCamera.transform.position.z);
        Move();
        
    }
    void OnGUI() {
        
    }

    //腳色移動
    public void Move() {

        Vector3 pos;

        if (Input.GetKey("w"))
        {
            pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y + Time.deltaTime * Speed, pos.z);
        }
        else if (Input.GetKey("s"))
        {
            pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y - Time.deltaTime * Speed, pos.z);
        }
        else if (Input.GetKey("a"))
        {
            pos = transform.position;
            transform.position = new Vector3(pos.x - Time.deltaTime * Speed, pos.y, pos.z);
        }
        else if (Input.GetKey("d"))
        {
            pos = transform.position;
            transform.position = new Vector3(pos.x + Time.deltaTime * Speed, pos.y, pos.z);
        }
    }
}
