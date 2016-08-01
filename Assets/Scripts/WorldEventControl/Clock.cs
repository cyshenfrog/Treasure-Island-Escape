using UnityEngine;
using System.Collections;

public class Clock : Timer {
    public static Clock instance;
    public int worldHour { get; protected set; }
    public int worldDay { get; protected set; }
    public int worldClock { get; protected set; }
    public bool isDaytime { get; protected set; }
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.LogError("why do we have two timer? ");
            Destroy(this.gameObject);
        }
    }

    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        worldHour = Mathf.FloorToInt( realSecond / 30 );
        worldDay = Mathf.FloorToInt( worldHour / 24 );
        worldClock = worldHour % 24;
    }
}
