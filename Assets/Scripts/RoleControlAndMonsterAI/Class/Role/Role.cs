using UnityEngine;
using System.Collections.Generic;

public class Role {

    protected static string filePath = "none";
    protected Carceer carceerName = Carceer.None;      //職業名稱

    protected int hp;              //血量(顯示現在血量)
    protected int maxHp;           //血量(顯示遊戲中血量上限)
    protected int baseMaxHp;       //血量(基礎數值)

    protected float attack;        //攻擊(遊戲中腳色攻擊力)
    protected float baseAttack;    //攻擊(基礎數值)

    protected float defence;       //防禦(遊戲中)
    protected float baseDefence;   //防禦(基礎數值)

    protected int loading;            //負重(遊戲中)
    protected int baseLoading;        //負重(基礎數值)

    protected float attackSpeed;      //攻擊速度(遊戲中)
    protected float baseAttackSpeed;  //攻擊速度(基礎數值)

    protected float hungry;        //飢餓值(顯示現在飢餓值)
    protected float tired;         //疲勞值(顯示現在疲勞值)
    protected float spirit;        //精神值(顯示現在精神值)

    protected float maxHungry;     //飢餓值(遊戲中飢餓值上限)
    protected float maxTired;      //疲勞值(遊戲中疲勞值上限)
    protected float maxSpirit;     //精神值(遊戲中精神值上限)

    protected float baseMaxHungry;    //飢餓值(基礎數值)
    protected float baseMaxTired;     //疲勞值(基礎數值)
    protected float baseMaxSpirit;    //精神值(基礎數值)

    protected float moveSpeed;        //移動速度(遊戲中)
    protected float baseMoveSpeed;    //移動數值(基礎數值)

    protected float beHungry;         //飢餓值耗損速度(遊戲中)
    protected float beTired;          //疲勞值耗損速度(遊戲中)
    protected float beFear;           //精神值耗損速度(遊戲中)

    protected float baseBeHungry;     //飢餓值耗損速度(基礎數值)
    protected float baseBeTired;      //疲勞值耗損速度(基礎數值)
    protected float baseBeFear;       //精神值耗損速度(基礎數值)

    protected float beHungryAtNight;       //夜間飢餓值耗損速度(遊戲中)
    protected float beTiredAtNight;        //夜間疲勞值耗損速度(遊戲中)
    protected float beFearAtNight;         //夜間精神值耗損速度(遊戲中)

    protected float baseBeHungryAtNight;   //夜間飢餓值耗損速度(基礎數據)
    protected float baseBeTiredAtNight;    //夜間疲勞值耗損速度(基礎數據)
    protected float baseBeFearAtNight;     //夜間精神值耗損速度(基礎數據)

    //protected Monolog logSet;  //獨白字典

    /*public Monolog LogSet {
        set { logSet = value; }
        get { return logSet; }
    }*/

    public Carceer CarceerName {
        set { carceerName = value; }
        get { return carceerName; }
    }
    public int Hp {
        set {
            if (hp > maxHp)
                hp = maxHp;
            else if (hp < 0)
                hp = 0;
            else
                hp = value;
        }
        get { return hp; }
    }
    public int MaxHp {
        set { maxHp = value; }
        get { return maxHp; }
    }
    public int BaseMaxHp {
        set { baseMaxHp = value; }
        get { return baseMaxHp; }
    }
    public float Attack {
        set { attack = value; }
        get { return attack; }
    }
    public float BaseAttack {
        set { baseAttack = value; }
        get { return baseAttack; }
    }
    public float Defence {
        set { defence = value; }
        get { return defence; }
    }
    public float BaseDefence{
        set { baseDefence = value; }
        get { return baseDefence; }
    }
    public int Loading {
        set { loading = value; }
        get { return loading; }
    }
    public int BaseLoading {
        set { baseLoading = value; }
        get { return baseLoading; }
    }
    public float AttackSpeed {
        set { attackSpeed = value; }
        get { return attackSpeed; }
    }
    public float BaseAttackSpeed {
        set { baseAttackSpeed = value; }
        get { return baseAttackSpeed; }
    }
    public float MoveSpeed {
        set { moveSpeed = value; }
        get { return moveSpeed; }
    }
    public float BaseMoveSpeed
    {
        set { baseMoveSpeed = value; }
        get { return baseMoveSpeed; }
    }
    public float Hungry {
        set {
            if (hungry > maxHungry)
                hungry = maxHungry;
            else if (hungry < 0)
                hungry = 0;
            else
                hungry = value;
        }
        get { return hungry; }
    }
    public float Tired {
        set {
            if (tired > maxTired)
                tired = maxTired;
            else if (tired < 0)
                tired = 0;
            else
                tired = value;
        }
        get { return tired; }
    }
    public float Spirit {
        set {
            if (spirit > maxSpirit)
                spirit = maxSpirit;
            else if (tired < 0)
                spirit = 0;
            else
                spirit = value;
        }
        get { return spirit; }
    }
    public float MaxHungry {
        set { maxHungry = value; }
        get { return maxHungry; }
    }
    public float MaxTired {
        set { maxTired = value; }
        get { return maxTired; }
    }
    public float MaxSpirit {
        set { maxSpirit = value; }
        get { return maxSpirit; }
    }
    public float BaseMaxHungry {
        set { baseMaxHungry = value; }
        get { return baseMaxHungry; }
    }
    public float BaseMaxTired {
        set { baseMaxTired = value; }
        get { return baseMaxTired; }
    }
    public float BaseMaxSpirit {
        set { baseMaxSpirit = value; }
        get { return baseMaxSpirit; }
    }
    public float BeHungry {
        set { beHungry = value; }
        get { return beHungry; }
    }
    public float BeTired {
        set { beTired = value; }
        get { return beTired; }
    }
    public float BeFear {
        set { beFear = value; }
        get { return beFear; }
    }
    public float BaseBeHungry {
        set { baseBeHungry = value; }
        get { return baseBeHungry; }
    }
    public float BaseBeTired {
        set { baseBeTired = value; }
        get { return baseBeTired; }
    }
    public float BaseBeFear {
        set { baseBeFear = value; }
        get { return baseBeFear; }
    }
    public float BeHungryAtNight {
        set { beHungryAtNight = value; }
        get { return beHungryAtNight; }
    }
    public float BeTiredAtNight {
        set { beTiredAtNight = value; }
        get { return beTiredAtNight; }
    }
    public float BeFearAtNight {
        set { beFearAtNight = value; }
        get { return beFearAtNight; }
    }
    public float BaseBeHungryAtNight {
        set { baseBeHungryAtNight = value; }
        get { return baseBeHungryAtNight; }
    }
    public float BaseBeTiredAtNight {
        set { baseBeTiredAtNight = value; }
        get { return baseBeTiredAtNight; }
    }
    public float BaseBeFearAtNight {
        set { baseBeFearAtNight = value; }
        get { return baseBeFearAtNight; }
    }

    public virtual Role Load() {
        Role r = new Role();
        readFile(ref r);
        return r;
    }

    protected void readFile(ref Role r) {
        //read file
    }

    public static Role GetRoleData(Carceer c) {

        switch (c) {
            case Carceer.Chef:
                return new Chef();
            case Carceer.Engineer:
                return new Engineer();
            case Carceer.Explorer:
                return new Explorer();
            case Carceer.Warrior:
                return new Warrior();
            default:
                return new Engineer();
        }
    }
    
    
}
