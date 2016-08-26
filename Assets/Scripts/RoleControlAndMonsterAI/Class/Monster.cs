﻿using UnityEngine;
using System.Collections.Generic;

public class Monster : Monolog{

    private int id = 0;

    private string name;

    private int hp;
    private int baseHp;

    private float speed;
    private float baseSpeed;

    private float attack;
    private float baseAttack;

    private float defence;       
    private float baseDefence;

    private float hungry;    

    private float maxHungry;
    private float baseMaxHungry;

    private float beHungry;
    private float baseBeHungry;

    private Dictionary<Log, string> logSet = new Dictionary<Log, string>();                       //獨白
    private Dictionary<string, float> drop = new Dictionary<string, float>();                     //掉落物
    private Dictionary<Status.StatusPool, int> status = new Dictionary<Status.StatusPool, int>(); //狀態

    public Dictionary<Log, string> LogSet {
        set { logSet = value; }
        get { return logSet; }
    }   
    public Dictionary<string, float> Drop {
        set { drop = value; }
        get { return drop; }
    }  
    public Dictionary<Status.StatusPool, int> Status {
        set { status = value; }
        get{ return status; }
    } 

    public int Id {
        get { return id; }
    }
    public string Name {
        get { return name; }
    }
    public int Hp {
        set { hp = value; }
        get { return hp; }
    }
    public int BaseHp {
        set { baseHp = value; }
        get { return baseHp; }
    }

    public float Speed {
        set { speed = value; }
        get { return speed; }
    }
    public float BaseSpeed {
        set { baseSpeed = value; }
        get { return baseSpeed; }
    }

    public float Attack {
        set { attack = value; }
        get { return attack; }
    }
    public float BaseAttack {
        set { baseAttack = value; }
        get { return baseAttack; }
    }
    public float Defence
    {
        set { defence = value; }
        get { return defence; }
    }
    public float BaseDefence
    {
        set { baseDefence = value; }
        get { return baseDefence; }
    }
    public float Hungry
    {
        set { hungry = value; }
        get { return hungry; }
    }
    public float MaxHungry
    {
        set { maxHungry = value; }
        get { return maxHungry; }
    }
    public float BaseMaxHungry
    {
        set { baseMaxHungry = value; }
        get { return baseMaxHungry; }
    }
    public float BeHungry
    {
        set { beHungry = value; }
        get { return beHungry; }
    }
    public float BaseBeHungry
    {
        set { baseBeHungry = value; }
        get { return baseBeHungry; }
    }

    //查詢獨白字典
    public string LogString(Log log)
    {
        foreach (var item in logSet)
        {
            if (item.Key == log) return item.Value;
        }
        return null;
    }
}
