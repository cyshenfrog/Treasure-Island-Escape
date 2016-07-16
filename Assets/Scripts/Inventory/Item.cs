﻿using UnityEngine;
using System.Collections;

public enum itemType {MANA, HEALTH};
public enum Quality {NORMAL, RARE, EPIC, LEGENDARY};
public class Item : MonoBehaviour {

    public itemType type;
    public Quality quality;
    public Sprite icon;
    public int maxStackSize;

    public float str, stamine;
    public string itemName;
    public string description;

    public GameObject dropItem;
	// Use this for initialization
	
    public void use()
    {
        switch (type)
        {
            case itemType.MANA:
                Debug.Log("MANA");
                break;
            case itemType.HEALTH:
                Debug.Log("HEALTH");
                break;
            default:
                break;
        }
    }
    public string toolTipContent()
    {
        string abilityDescription = null;
        string color = "white";
        
        if(str != 0)
        {
            abilityDescription += "\n str " + str.ToString();
        }
        if (stamine != 0)
        {
            abilityDescription += "\n stamine " + stamine.ToString();
        }
        
        return string.Format("<color=" + color +"><size=16>{0}</size></color><size=14><i><color=lime>\n{1}</color></i>{2}</size>", itemName, description, abilityDescription);
    }
    public void setStatus(Item item)
    {
        type = item.type;
        quality = item.quality;
        icon = item.icon;
        maxStackSize = item.maxStackSize;

        str = item.str;
        stamine = item.stamine;
        itemName = item.itemName;
        description = item.description;

        dropItem = item.dropItem;
}
}
