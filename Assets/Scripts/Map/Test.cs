using UnityEngine;
using System.Collections.Generic;

public class Test : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        List<int> i = new List<int>();

        if (i == null)
            Debug.Log("null");
        else
            Debug.Log("not null");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
