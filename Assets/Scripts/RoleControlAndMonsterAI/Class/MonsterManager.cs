using UnityEngine;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {

    private static List<Monster> Data = new List<Monster>();

    public static Monster Create(int i) {
        return Monster.Load(i);
    }
    
}
