﻿using UnityEngine;
using System;

public abstract class ObjDisplay : MonoBehaviour
{
    public abstract void Idle(int mode);
    public abstract void Click();
    public abstract void Collision(Transform collision);
    public abstract void Enable();
    public abstract void Updated();

    protected ObjData data;
    protected Action<Transform> CollisionAction;
    protected Action<int> IdleAction;
    protected Action ClickAction, EnableAction, UpdatedAction;
}
