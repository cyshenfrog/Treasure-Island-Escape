using UnityEngine;
using System.Collections;

public class WorldEventControl : MonoBehaviour {

    RoleController player;
    Item[] Trees;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindObjectOfType<RoleController>();
        Trees = GameObject.FindObjectsOfType<Item>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Clock.instance.worldDay == 7)
        {

        }
	}

    void CreatBoss()
    {

    }

    bool IsKeyItemCollected(bool isCollected)
    {

        return true;
    }
}
