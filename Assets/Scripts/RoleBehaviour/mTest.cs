using UnityEngine;
using System.Collections;

public class mTest : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Monster m = MonsterManager.Create(0);
        Debug.Log(m.Attack);
        GameObject g = MonsterManager.Prefab(m, Vector2.zero);
        m.Attack = 999;
        Debug.Log(m.Attack + ":" + g.GetComponent<MonsterController>().Data.Attack);
        g.GetComponent<MonsterController>().Data.Attack = 200;
        Debug.Log(m.Attack + ":" + g.GetComponent<MonsterController>().Data.Attack);

        /*
        Debug.Log(GroundController.StaticWorldHeightInWC);
        Debug.Log(GroundController.StaticWorldWidthInWC);
        Debug.Log(GroundController.StaticWorldHeight);
        Debug.Log(GroundController.StaticWorldWidth);
        */
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
