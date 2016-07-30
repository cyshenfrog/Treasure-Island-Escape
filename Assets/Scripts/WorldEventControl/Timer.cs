using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    
    public float realSecond { get; protected set; }
    void Update ()
    {
        realSecond = Time.timeSinceLevelLoad;
	}

}
