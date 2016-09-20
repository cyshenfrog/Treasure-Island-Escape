using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float speed = 10.0f;
    
	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * Vector3.up * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += speed * Vector3.left * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += speed * Vector3.down * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * Vector3.right * Time.deltaTime;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Bank" && Input.GetKeyDown(KeyCode.B))
        {
            Bank bank = other.GetComponent<Bank>();
            if (bank.IsClose)
                bank.open();
            else
                bank.close();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bank")
        {
            Bank bank = other.GetComponent<Bank>();
            if (!bank.IsClose)
                bank.close();
        }
    }
}
