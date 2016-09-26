using UnityEngine;
using System.Collections;

public class Chef : Role {

    public Chef() {
        carceerName = Carceer.Chef;
        filePath = "chef";
    }

    public override Role Load() {
        Role r = new Chef();
        readFile(ref r);
        return r;
    }
}
