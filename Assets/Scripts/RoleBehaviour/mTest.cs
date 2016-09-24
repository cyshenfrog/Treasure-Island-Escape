using UnityEngine;
using System.Collections;

public class mTest : MonoBehaviour {

    public GameObject monster;

	// Use this for initialization
	void Start () {
        Monster m = MonsterManager.Create(0);
        Debug.Log(m.Attack);
        GameObject g = Instantiate(monster);        
        g.GetComponent<MonsterController>().data = m;
        m.Attack = 999;
        Debug.Log(m.Attack + ":" + g.GetComponent<MonsterController>().data.Attack);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
