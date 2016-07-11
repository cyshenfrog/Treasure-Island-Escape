using UnityEngine;
using System.Collections;

public class Monster {

    private int hp;
    private int baseHp;

    private float speed;
    private float baseSpeed;

    private float attack;
    private float baseAttack;

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
}
