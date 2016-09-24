using UnityEngine;
using System.Collections;

public class mTest : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Monster m = MonsterManager.Create(0);
        Debug.Log(m.Attack);
        GameObject g = MonsterManager.Prefab(m);
        m.Attack = 999;
        Debug.Log(m.Attack + ":" + g.GetComponent<MonsterController>().Data.Attack);
        g.GetComponent<MonsterController>().Data.Attack = 200;
        Debug.Log(m.Attack + ":" + g.GetComponent<MonsterController>().Data.Attack);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
