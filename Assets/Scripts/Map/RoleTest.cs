﻿using UnityEngine;
using System.Collections;

public class RoleTest : MonoBehaviour
{
    //in the Role

    void Update()
    {
        transform.localPosition += Vector3.right * Input.GetAxis("Horizontal");
        transform.localPosition += Vector3.up * Input.GetAxis("Vertical");
    }
}
