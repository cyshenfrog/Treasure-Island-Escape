using UnityEngine;
using System.Collections.Generic;
using System;

public class MonsterManager : MonoBehaviour {

    private static List<Monster> Data = new List<Monster>();
    //dictionary : tile-type, sub-type, boss or normal, id 
    //id by count

    public static void Initial() {
        
    }


    public static void Create(List<Monster> l, List<GameObject> g, Action a, int id) {
        //data , gameobject
    }
    public static Monster Create(int id) {
        return Monster.Load(id);
    }
    
}
