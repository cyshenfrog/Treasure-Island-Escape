using UnityEngine;
using System.Collections;

public class ClockGUI : MonoBehaviour {
    public GameObject niddle;
    public float from, to;
	public float Time
    {
        set
        {
            if (value > 1 || value < 0)
            {
                Debug.LogError("The Clock time value is out of 0 ~ 1. The value will be ignored.");
                return;
            }
            Vector3 angle = new Vector3(0, 0, Mathf.Lerp(from, to, value));
            niddle.transform.localEulerAngles = angle;
        }
    }
    public void Reset()
    {
        Time = 0f;
    }
}
