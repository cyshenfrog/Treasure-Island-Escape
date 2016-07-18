using UnityEngine;
using System.Collections;

//狀態相關 : buff, debuff
public interface StatusAction {

    void Death();
    void Sleep();
    void Poison();
    void Cold();
    void Hot();
    void FierceWind();
    void Overload();
    void Night();
    void Damnation();
    void Greedy();
    void Excited();
    void Exhaustion();
    void Blessing();
    void Fury();
}

public enum Status {
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
    Fury
}
