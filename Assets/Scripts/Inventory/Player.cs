using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float speed = 10.0f;
    
    public Inventory inventory1,inventory2, inventory3;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            if (pickUpItem(other.GetComponent<Item>()))
            {
                Destroy(other.gameObject);
            }
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
    public bool pickUpItem(Item item)
    {
        if (item.maxStackSize == 1)
        {
            if (inventory1.placeEmpty(item))
            {
                return true;
            }
            else if (inventory2.placeEmpty(item))
            {
                return true;
            }
            else if (inventory3.placeEmpty(item))
            {
                return true;
            }
        }
        else
        {
            if (inventory1.addItem(item))
            {
                return true;
            }
            else if (inventory2.addItem(item))
            {
                return true;
            }
            else if (inventory3.addItem(item))
            {
                return true;
            }
        }
        return false;
    }
}
