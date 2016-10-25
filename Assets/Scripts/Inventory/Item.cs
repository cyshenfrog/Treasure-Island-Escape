using UnityEngine;
using System.Collections;

public enum itemType {Mana, Health, GreenPotion};
public enum Quality {NORMAL, RARE, EPIC, LEGENDARY};
public class Item : MonoBehaviour {

    public itemType type;
    public Quality quality;
    public int weight;
    public Sprite icon;
    public int maxStackSize;

    public float str, stamine;
    public string description;

    public GameObject dropItem;
	// Use this for initialization
	
    public void use()
    {
        switch (type)
        {
            case itemType.Mana:
                Debug.Log("MANA");
                break;
            case itemType.Health:
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
        
        return string.Format("<color=" + color +"><size=50>{0}</size></color><size=48><i><color=lime>\n{1}</color></i>{2}</size>", type, description, abilityDescription);
    }
    public void setStatus(Item item)
    {
        type = item.type;
        quality = item.quality;
        weight = item.weight;
        icon = item.icon;
        maxStackSize = item.maxStackSize;

        str = item.str;
        stamine = item.stamine;
        description = item.description;

        dropItem = item.dropItem;
}
}
