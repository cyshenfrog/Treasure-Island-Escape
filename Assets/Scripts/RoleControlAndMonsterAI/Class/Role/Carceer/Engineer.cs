using UnityEngine;
using System.Collections;

public class Engineer : Role {

    public Engineer() {
        carceerName = Carceer.Engineer;
        filePath = "chef";
    }

    public override Role Load() {
        Role r = new Engineer();
        readFile(ref r);
        return r;
    }
}
