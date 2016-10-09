using UnityEngine;
using System.Collections;

public class WorldEventControl : MonoBehaviour {

    public bool isSandStorm;
    public bool isSnowStorm;

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

    void onEnterDesert()
    {

    }

    bool IsKeyItemCollected(bool isCollected)
    {

        return true;
    }

    void playWetherAnime()
    {

    }

    void stopWetherAnime()
    {

    }
}
