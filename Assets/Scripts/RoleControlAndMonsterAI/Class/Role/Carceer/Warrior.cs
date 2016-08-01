using UnityEngine;
using System.Collections;

public class Warrior : Role {

    private static Carceer carceerName = Carceer.Warrior;      //職業名稱

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

    public override Monolog LogSet
    {
        set { logSet = value; }
        get { return logSet; }
    }

    public override Carceer CarceerName
    {
        set { carceerName = value; }
        get { return carceerName; }
    }
    public override int Hp
    {
        set { hp = value; }
        get { return hp; }
    }
    public override int MaxHp
    {
        set { maxHp = value; }
        get { return maxHp; }
    }
    public override int BaseMaxHp
    {
        set { baseMaxHp = value; }
        get { return baseMaxHp; }
    }
    public override float Attack
    {
        set { attack = value; }
        get { return attack; }
    }
    public override float BaseAttack
    {
        set { baseAttack = value; }
        get { return baseAttack; }
    }
    public override float Defence
    {
        set { defence = value; }
        get { return defence; }
    }
    public override float BaseDefence
    {
        set { baseDefence = value; }
        get { return baseDefence; }
    }
    public override int Load
    {
        set { load = value; }
        get { return load; }
    }
    public override int BaseLoad
    {
        set { baseLoad = value; }
        get { return baseLoad; }
    }
    public override float AttackSpeed
    {
        set { attackSpeed = value; }
        get { return attackSpeed; }
    }
    public override float BaseAttackSpeed
    {
        set { baseAttackSpeed = value; }
        get { return baseAttackSpeed; }
    }
    public override float Hungry
    {
        set { hungry = value; }
        get { return hungry; }
    }
    public override float Tired
    {
        set { tired = value; }
        get { return tired; }
    }
    public override float Spirit
    {
        set { spirit = value; }
        get { return spirit; }
    }
    public override float MaxHungry
    {
        set { maxHungry = value; }
        get { return maxHungry; }
    }
    public override float MaxTired
    {
        set { maxTired = value; }
        get { return maxTired; }
    }
    public override float MaxSpirit
    {
        set { maxSpirit = value; }
        get { return maxSpirit; }
    }
    public override float BaseMaxHungry
    {
        set { baseMaxHungry = value; }
        get { return baseMaxHungry; }
    }
    public override float BaseMaxTired
    {
        set { baseMaxTired = value; }
        get { return baseMaxTired; }
    }
    public override float BaseMaxSpirit
    {
        set { baseMaxSpirit = value; }
        get { return baseMaxSpirit; }
    }
    public override float BeHungry
    {
        set { beHungry = value; }
        get { return beHungry; }
    }
    public override float BeTired
    {
        set { beTired = value; }
        get { return beTired; }
    }
    public override float BeFear
    {
        set { beFear = value; }
        get { return beFear; }
    }
    public override float BaseBeHungry
    {
        set { baseBeHungry = value; }
        get { return baseBeHungry; }
    }
    public override float BaseBeTired
    {
        set { baseBeTired = value; }
        get { return baseBeTired; }
    }
    public override float BaseBeFear
    {
        set { baseBeFear = value; }
        get { return baseBeFear; }
    }
    public override float BeHungryAtNight
    {
        set { beHungryAtNight = value; }
        get { return beHungryAtNight; }
    }
    public override float BeTiredAtNight
    {
        set { beTiredAtNight = value; }
        get { return beTiredAtNight; }
    }
    public override float BeFearAtNight
    {
        set { beFearAtNight = value; }
        get { return beFearAtNight; }
    }
    public override float BaseBeHungryAtNight
    {
        set { baseBeHungryAtNight = value; }
        get { return baseBeHungryAtNight; }
    }
    public override float BaseBeTiredAtNight
    {
        set { baseBeTiredAtNight = value; }
        get { return baseBeTiredAtNight; }
    }
    public override float BaseBeFearAtNight
    {
        set { baseBeFearAtNight = value; }
        get { return baseBeFearAtNight; }
    }
}
