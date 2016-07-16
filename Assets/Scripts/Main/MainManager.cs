using UnityEngine;
using System.Collections;

public class MainManager : MonoBehaviour {
    public GameObject Main, Load;
    public void Go()
    {
        Main.SetActive(false);
        Load.SetActive(true);
    }
    public void Back()
    {
        Main.SetActive(true);
        Load.SetActive(false);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
