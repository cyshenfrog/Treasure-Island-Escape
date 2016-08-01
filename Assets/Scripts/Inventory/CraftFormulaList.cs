using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftFormulaList : MonoBehaviour
{
    public static Dictionary<string, string> formula = new Dictionary<string, string>();
    void Start()
    {
        createFormula();
    }
    void createFormula()
    {
        formula.Add("GreenPotion","Health-1-Mana-3-Null-0-Null-0-Null-0-");
        formula.Add("Health", "Mana-1-Mana-1-Null-0-Null-0-Null-0-");
    }
}