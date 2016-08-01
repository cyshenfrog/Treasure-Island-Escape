using UnityEngine;
using System.Collections.Generic;


//獨白相關 
public class Monolog {

    private List<string> log = new List<string>();
    public List<string> Log {
        set { log = value; }
        get { return log; }
    }

    public void Add(string str) {
        log.Add(str);
    }

    public string GetLog(Log monolog) {
        return Log[(int)monolog];
    }
}

public enum Log {
    
}
