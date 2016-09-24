using UnityEngine;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {

    private static Dictionary<MapConstants.LandformType, Dictionary<int, List<int>>> Group;
    //dictionary : tile-type, monster-group, monster id

    public static void Initial() {
        Group = new Dictionary<MapConstants.LandformType, Dictionary<int, List<int>>>();
    }

    public static Monster Create(int id) {
        //Monster m = Monster.Load(id);
        return Monster.Load(id);
    }

    public static Monster Data(MapConstants.LandformType tileType, int group, Monster.MonsterType monsterType = Monster.MonsterType.Mobs) {
        List<int> mGroup = Group[tileType][group];
        int randomId = Random.Range(0, mGroup.Count);
        return Monster.Load(randomId);
    }

    public static GameObject Prefab(Monster monster) {
        GameObject source = Resources.Load("Monster/Prefabs/M" + monster.Id) as GameObject;
        GameObject prefab = Instantiate(source);
        prefab.GetComponent<MonsterController>().Data = monster;
        return prefab;
    }
    
}
