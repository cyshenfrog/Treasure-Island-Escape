using UnityEngine;
using System.Collections;

public class Warrior : Role {

    public Warrior() {
        carceerName = Carceer.Warrior;
        filePath = "chef";
    }

    public override Role Load() {
        Role r = new Warrior();
        readFile(ref r);
        return r;
    }
}
