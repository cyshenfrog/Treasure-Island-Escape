using UnityEngine;
using System.Collections;

public class Explorer : Role {

    public Explorer() {
        carceerName = Carceer.Explorer;
        filePath = "chef";
    }

    public override Role Load()
    {
        Role r = new Explorer();
        readFile(ref r);
        return r;
    }
}
