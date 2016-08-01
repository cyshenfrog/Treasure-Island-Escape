using UnityEngine;
using System.Collections.Generic;

//狀態相關 : buff, debuff
public class Status {
    

    public enum StatusPool {
        None,
        Sleep,
        Death,
        Poison,
        Cold,
        Hot,
        FierceWind,
        Overload,
        Night,
        Damnation,
        Greedy,
        Excited,
        Exhaustion,
        Blessing,
        Fury,
    }
    public enum Objects {
        Player,
        Monster
    }


    private static Dictionary<string, Dictionary<StatusPool, float>> statusCollections = new Dictionary<string, Dictionary<StatusPool, float>>();
    public static Dictionary<string, Dictionary<StatusPool, float>> StatusCollectios {
        set { statusCollections = value; }
        get { return statusCollections; }
    }


    public static void Add(string id, StatusPool stat, float time) {

        if (!statusCollections.ContainsKey(id)) {
            statusCollections.Add(id, new Dictionary<StatusPool, float>());
            statusCollections[id].Add(stat, time);
        }
        else {
            if (!statusCollections[id].ContainsKey(stat)) {
                statusCollections[id].Add(stat, time);
            }
        }        
    }

    public static void Update(string id, StatusPool stat, float time) {
        if (statusCollections.ContainsKey(id) && statusCollections[id].ContainsKey(stat)) {
            statusCollections[id][stat] = time;
        }
    }

    public static void Remove(string id, StatusPool stat) {
        if (statusCollections.ContainsKey(id) && statusCollections[id].ContainsKey(stat)) {
            statusCollections[id].Remove(stat);
            if (statusCollections[id].Count == 0) {
                statusCollections.Remove(id);
            }
        }
    }

    public static bool ContainStatus(string id, StatusPool stat) {
        return statusCollections.ContainsKey(id) && statusCollections[id].ContainsKey(stat);       
    }

}


