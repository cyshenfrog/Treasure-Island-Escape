﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test3 : MonoBehaviour
{
    void Start()
    {
        t.Func();
        Destroy(this);
    }

    public Test2 t;
}
