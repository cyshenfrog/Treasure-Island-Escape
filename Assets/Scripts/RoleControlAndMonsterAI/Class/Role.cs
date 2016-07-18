using UnityEngine;
using System.Collections.Generic;


public class Role : Monolog {

    public enum Carceer {
        None,
        Warrior,
        Chef,
        Engineer,
        Explorer
    }

    public static Carceer carceerName = Carceer.None;      //職業名稱

    private int hp;              //血量(顯示現在血量)
    private int maxHp;           //血量(顯示遊戲中血量上限)
    private int baseMaxHp;       //血量(基礎數值)

    private float attack;        //攻擊(遊戲中腳色攻擊力)
    private float baseAttack;    //攻擊(基礎數值)

    private float defence;       //防禦(遊戲中)
    private float baseDefence;   //防禦(基礎數值)

    private int load;            //負重(遊戲中)
    private int baseLoad;        //負重(基礎數值)

    private float attackSpeed;      //攻擊速度(遊戲中)
    private float baseAttackSpeed;  //攻擊速度(基礎數值)

    private float hungry;        //飢餓值(顯示現在飢餓值)
    private float tired;         //疲勞值(顯示現在疲勞值)
    private float spirit;        //精神值(顯示現在精神值)

    private float maxHungry;     //飢餓值(遊戲中飢餓值上限)
    private float maxTired;      //疲勞值(遊戲中疲勞值上限)
    private float maxSpirit;     //精神值(遊戲中精神值上限)

    private float baseMaxHungry;    //飢餓值(基礎數值)
    private float baseMaxTired;     //疲勞值(基礎數值)
    private float baseMaxSpirit;    //精神值(基礎數值)

    private float moveSpeed;        //移動速度(遊戲中)
    private float baseMoveSpeed;    //移動數值(基礎數值)

    private float beHungry;         //飢餓值耗損速度(遊戲中)
    private float beTired;          //疲勞值耗損速度(遊戲中)
    private float beFear;           //精神值耗損速度(遊戲中)

    private float baseBeHungry;     //飢餓值耗損速度(基礎數值)
    private float baseBeTired;      //疲勞值耗損速度(基礎數值)
    private float baseBeFear;       //精神值耗損速度(基礎數值)

    private float beHungryAtNight;       //夜間飢餓值耗損速度(遊戲中)
    private float beTiredAtNight;        //夜間疲勞值耗損速度(遊戲中)
    private float beFearAtNight;         //夜間精神值耗損速度(遊戲中)

    private float baseBeHungryAtNight;   //夜間飢餓值耗損速度(基礎數據)
    private float baseBeTiredAtNight;    //夜間疲勞值耗損速度(基礎數據)
    private float baseBeFearAtNight;     //夜間精神值耗損速度(基礎數據)

    private Dictionary<Log, string> logSet;  //獨白字典

    public Dictionary<Log, string> LogSet {
        set { logSet = value; }
        get { return logSet; }
    }

    public static Carceer CarceerName {
        set { carceerName = value; }
        get { return carceerName; }
    }
    public int Hp {
        set { hp = value; }
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
    public int Load {
        set { load = value; }
        get { return load; }
    }
    public int BaseLoad {
        set { baseLoad = value; }
        get { return baseLoad; }
    }
    public float AttackSpeed {
        set { attackSpeed = value; }
        get { return attackSpeed; }
    }
    public float BaseAttackSpeed {
        set { baseAttackSpeed = value; }
        get { return baseAttackSpeed; }
    }
    public float Hungry {
        set { hungry = value; }
        get { return hungry; }
    }
    public float Tired {
        set { tired = value; }
        get { return tired; }
    }
    public float Spirit {
        set { spirit = value; }
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


    //查詢獨白字典
    public string LogString(Log log) {
        foreach (var item in logSet) {
            if (item.Key == log) return item.Value;
        }
        return null;
    }
    
}
