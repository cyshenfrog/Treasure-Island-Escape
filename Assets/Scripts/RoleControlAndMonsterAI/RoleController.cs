using UnityEngine;
using System.Collections;

public class RoleController : MonoBehaviour {

    public float Speed;
    private GameObject RoleCamera;

	// Use this for initialization
	void Start () {
        RoleCamera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
        RoleCamera.transform.position = new Vector3(transform.position.x, transform.position.y, RoleCamera.transform.position.z);

        Vector3 pos;

        if (Input.GetKey("w")) {
            pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y + Time.deltaTime * Speed, pos.z);
        }
        if (Input.GetKey("s")) {
            pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y - Time.deltaTime * Speed, pos.z);
        }
        if (Input.GetKey("a"))
        {
            pos = transform.position;
            transform.position = new Vector3(pos.x - Time.deltaTime * Speed, pos.y, pos.z);
        }
        if (Input.GetKey("d"))
        {
            pos = transform.position;
            transform.position = new Vector3(pos.x + Time.deltaTime * Speed, pos.y, pos.z);
        }
    }
}
