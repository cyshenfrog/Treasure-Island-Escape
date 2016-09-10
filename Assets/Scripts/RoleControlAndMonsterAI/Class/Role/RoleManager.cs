using UnityEngine;
using System.Collections;

public static class RoleManager {

    private static Chef chef = new Chef();
    private static Engineer engineer = new Engineer();
    private static Explorer explorer = new Explorer();
    private static Warrior warrior = new Warrior();

    public static void Initial() {
        chef.Load();
        engineer.Load();
        explorer.Load();
        warrior.Load();
    }

    public static Role Read<T>() where T : Role {
        if (typeof(T) == typeof(Chef)) return chef;
        else if (typeof(T) == typeof(Engineer)) return engineer;
        else if (typeof(T) == typeof(Explorer)) return explorer;
        else if (typeof(T) == typeof(Warrior)) return warrior;
        else return new Role();
    }

    public static void Write() {

    }
	
}
