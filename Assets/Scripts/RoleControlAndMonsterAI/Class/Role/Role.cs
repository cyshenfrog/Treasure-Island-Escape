using UnityEngine;
using System.Collections.Generic;

public enum Carceer {
        None,
        Warrior,
        Chef,
        Engineer,
        Explorer
}
public class Role : Monolog {

    private static Carceer carceerName = Carceer.None;      //職業名稱

    private static int hp;              //血量(顯示現在血量)
    private static int maxHp;           //血量(顯示遊戲中血量上限)
    private static int baseMaxHp;       //血量(基礎數值)

    private static float attack;        //攻擊(遊戲中腳色攻擊力)
    private static float baseAttack;    //攻擊(基礎數值)

    private static float defence;       //防禦(遊戲中)
    private static float baseDefence;   //防禦(基礎數值)

    private static int load;            //負重(遊戲中)
    private static int baseLoad;        //負重(基礎數值)

    private static float attackSpeed;      //攻擊速度(遊戲中)
    private static float baseAttackSpeed;  //攻擊速度(基礎數值)

    private static float hungry;        //飢餓值(顯示現在飢餓值)
    private static float tired;         //疲勞值(顯示現在疲勞值)
    private static float spirit;        //精神值(顯示現在精神值)

    private static float maxHungry;     //飢餓值(遊戲中飢餓值上限)
    private static float maxTired;      //疲勞值(遊戲中疲勞值上限)
    private static float maxSpirit;     //精神值(遊戲中精神值上限)

    private static float baseMaxHungry;    //飢餓值(基礎數值)
    private static float baseMaxTired;     //疲勞值(基礎數值)
    private static float baseMaxSpirit;    //精神值(基礎數值)

    private static float moveSpeed;        //移動速度(遊戲中)
    private static float baseMoveSpeed;    //移動數值(基礎數值)

    private static float beHungry;         //飢餓值耗損速度(遊戲中)
    private static float beTired;          //疲勞值耗損速度(遊戲中)
    private static float beFear;           //精神值耗損速度(遊戲中)

    private static float baseBeHungry;     //飢餓值耗損速度(基礎數值)
    private static float baseBeTired;      //疲勞值耗損速度(基礎數值)
    private float baseBeFear;       //精神值耗損速度(基礎數值)

    private static float beHungryAtNight;       //夜間飢餓值耗損速度(遊戲中)
    private static float beTiredAtNight;        //夜間疲勞值耗損速度(遊戲中)
    private static float beFearAtNight;         //夜間精神值耗損速度(遊戲中)

    private static float baseBeHungryAtNight;   //夜間飢餓值耗損速度(基礎數據)
    private static float baseBeTiredAtNight;    //夜間疲勞值耗損速度(基礎數據)
    private static float baseBeFearAtNight;     //夜間精神值耗損速度(基礎數據)

    private static Monolog logSet;  //獨白字典

    public virtual Monolog LogSet {
        set { logSet = value; }
        get { return logSet; }
    }

    public virtual Carceer CarceerName {
        set { carceerName = value; }
        get { return carceerName; }
    }
    public virtual int Hp {
        set { hp = value; }
        get { return hp; }
    }
    public virtual int MaxHp {
        set { maxHp = value; }
        get { return maxHp; }
    }
    public virtual int BaseMaxHp {
        set { baseMaxHp = value; }
        get { return baseMaxHp; }
    }
    public virtual float Attack {
        set { attack = value; }
        get { return attack; }
    }
    public virtual float BaseAttack {
        set { baseAttack = value; }
        get { return baseAttack; }
    }
    public virtual float Defence {
        set { defence = value; }
        get { return defence; }
    }
    public virtual float BaseDefence{
        set { baseDefence = value; }
        get { return baseDefence; }
    }
    public virtual int Load {
        set { load = value; }
        get { return load; }
    }
    public virtual int BaseLoad {
        set { baseLoad = value; }
        get { return baseLoad; }
    }
    public virtual float AttackSpeed {
        set { attackSpeed = value; }
        get { return attackSpeed; }
    }
    public virtual float BaseAttackSpeed {
        set { baseAttackSpeed = value; }
        get { return baseAttackSpeed; }
    }
    public virtual float Hungry {
        set { hungry = value; }
        get { return hungry; }
    }
    public virtual float Tired {
        set { tired = value; }
        get { return tired; }
    }
    public virtual float Spirit {
        set { spirit = value; }
        get { return spirit; }
    }
    public virtual float MaxHungry {
        set { maxHungry = value; }
        get { return maxHungry; }
    }
    public virtual float MaxTired {
        set { maxTired = value; }
        get { return maxTired; }
    }
    public virtual float MaxSpirit {
        set { maxSpirit = value; }
        get { return maxSpirit; }
    }
    public virtual float BaseMaxHungry {
        set { baseMaxHungry = value; }
        get { return baseMaxHungry; }
    }
    public virtual float BaseMaxTired {
        set { baseMaxTired = value; }
        get { return baseMaxTired; }
    }
    public virtual float BaseMaxSpirit {
        set { baseMaxSpirit = value; }
        get { return baseMaxSpirit; }
    }
    public virtual float BeHungry {
        set { beHungry = value; }
        get { return beHungry; }
    }
    public virtual float BeTired {
        set { beTired = value; }
        get { return beTired; }
    }
    public virtual float BeFear {
        set { beFear = value; }
        get { return beFear; }
    }
    public virtual float BaseBeHungry {
        set { baseBeHungry = value; }
        get { return baseBeHungry; }
    }
    public virtual float BaseBeTired {
        set { baseBeTired = value; }
        get { return baseBeTired; }
    }
    public virtual float BaseBeFear {
        set { baseBeFear = value; }
        get { return baseBeFear; }
    }
    public virtual float BeHungryAtNight {
        set { beHungryAtNight = value; }
        get { return beHungryAtNight; }
    }
    public virtual float BeTiredAtNight {
        set { beTiredAtNight = value; }
        get { return beTiredAtNight; }
    }
    public virtual float BeFearAtNight {
        set { beFearAtNight = value; }
        get { return beFearAtNight; }
    }
    public virtual float BaseBeHungryAtNight {
        set { baseBeHungryAtNight = value; }
        get { return baseBeHungryAtNight; }
    }
    public virtual float BaseBeTiredAtNight {
        set { baseBeTiredAtNight = value; }
        get { return baseBeTiredAtNight; }
    }
    public virtual float BaseBeFearAtNight {
        set { baseBeFearAtNight = value; }
        get { return baseBeFearAtNight; }
    }


    
    
}
