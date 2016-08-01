using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CraftInventory : InventoryManager
{

    private List<Slot> allSlots;

    private Button btn;

    private EventSystem eventSystem;

    private string itemCrafting;
    // Use this for initialization
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        CreateCraftLayout();
    }
    private void CreateCraftLayout()
    {
        allSlots = new List<Slot>();
        int i = 0;
        foreach (Transform child in transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null)
            {
                btn = slot.GetComponent<Button>();
                GameObject temp = child.gameObject;
                btn.onClick.AddListener(delegate { moveItem(temp); });
                allSlots.Add(child.GetComponent<Slot>());
                ++i;
            }
        }
    }
    public void ShowCraftFormula(string itemToCraft)
    {
        if (CraftFormulaList.formula.ContainsKey(itemToCraft))
        {
            itemCrafting = itemToCraft;
            putBackMaterial();
            string[] split = CraftFormulaList.formula[itemToCraft].Split('-');
            //int i = 0;
            Item material;
            string path;
            for (int i=0; i<5; ++i)
            {
                if (split[i*2] != "Null")
                {
                    path = string.Concat("Inventory/", split[i*2]);
                    material = (Resources.Load(path, typeof(GameObject)) as GameObject).GetComponent<Item>();
                    if (!allSlots[i].isEmpty)
                    {
                        allSlots[i].clearSlot();
                    }
                    allSlots[i].addItem(material);
                    allSlots[i].transform.GetComponent<Image>().color = Color.gray;
                    allSlots[i].transform.GetChild(1).GetComponent<Text>().text = string.Concat("0/", split[i*2 + 1]);
                }
                else
                {
                    allSlots[i].transform.GetChild(1).GetComponent<Text>().text = string.Concat("0/", split[i*2 + 1]);
                }
            }
            path = string.Concat("Inventory/", itemToCraft);
            material = (Resources.Load(path, typeof(GameObject)) as GameObject).GetComponent<Item>();
            
            if (!allSlots[5].isEmpty)
            {
                allSlots[5].clearSlot();
            }
            allSlots[5].addItem(material);
            allSlots[5].GetComponent<Image>().color = Color.gray;
        }
    }
    public new void moveItem(GameObject clicked)
    {
        if (!movingSlot.isEmpty)
        {
            Slot tmp = clicked.GetComponent<Slot>();

            if (!tmp.isEmpty && tmp.tag!="ResultSlot" && tmp.currentItem.itemName == movingSlot.currentItem.itemName)
            {
                Text amount = tmp.transform.GetChild(1).GetComponent<Text>();
                string[] split = amount.text.Split('/');
                
                Image toImg = clicked.GetComponent<Image>();
                if (split[0] == "0")
                {
                    movingSlot.removeItem();
                }

                if (!hoverText)
                    hoverText = hoverObj.transform.GetChild(0).GetComponent<Text>();
                int acquireAmount = int.Parse(split[1]) - tmp.items.Count;
                for (int i = 0; i < acquireAmount; i++)
                {
                    if (movingSlot.isEmpty)
                        break;
                    tmp.addItem(movingSlot.removeItem());
                    hoverText.text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : string.Empty;
                }
                if (movingSlot.Items.Count == 0)
                {
                    movingSlot.clearSlot();
                    Destroy(hoverObj);
                }

                split[0] = tmp.items.Count.ToString();
                amount.text = split[0] + "/" + split[1];
                if (int.Parse(split[0]) >= int.Parse(split[1]))
                {
                    toImg.color = Color.white;
                }
            }
        }
        if (movingSlot.isEmpty)
        {
            if (!GameObject.Find("MoveItemStackSize"))
                click = clicked;
            Slot temp = clicked.GetComponent<Slot>();
            if (from == null && !temp.isEmpty && temp.tag != "MaterialSlot")
            {
                if (clicked.GetComponent<Image>().color != Color.gray)
                {
                    from = temp;
                    clicked.GetComponent<Image>().color = Color.gray;
                    createHoverIcon();
                    hoverText.text = from.Items.Count > 1 ? from.Items.Count.ToString() : string.Empty;
                }
            }
            else if (to == null && from != null && !temp.isEmpty && temp.tag!="ResultSlot" &&temp.currentItem.type == from.currentItem.type)
            {
                to = temp;
            }

            if (to != null && from != null)
            {
                if (!to.isEmpty && to.currentItem.type == from.currentItem.type)
                {
                    Text amount = to.transform.GetChild(1).GetComponent<Text>();
                    string[] split = amount.text.Split('/');

                    Image toImg = clicked.GetComponent<Image>();
                    if (split[0] == "0")
                    {
                        from.removeItem();
                    }

                    if (!hoverText)
                        hoverText = hoverObj.transform.GetChild(0).GetComponent<Text>();
                    int acquireAmount = int.Parse(split[1]) - to.items.Count;
                    for (int i = 0; i < acquireAmount; i++)
                    {
                        if (from.isEmpty)
                            break;
                        to.addItem(from.removeItem());
                        hoverText.text = from.Items.Count > 1 ? from.Items.Count.ToString() : string.Empty;
                    }
                    if (from.Items.Count == 0)
                    {
                        from.clearSlot();
                        Destroy(hoverObj);
                    }

                    split[0] = to.items.Count.ToString();
                    amount.text = split[0] + "/" + split[1];
                    if (int.Parse(split[0]) >= int.Parse(split[1]))
                    {
                        toImg.color = Color.white;
                    }
                    from.GetComponent<Image>().color = Color.white;
                    from = null;
                    to = null;
                    Destroy(hoverObj);
                }
            }
        }
    }
    public void craft()
    {
        if (itemCrafting != null)
        {
            int i;
            for (i = 0; i < 5; i++)
            {
                if (allSlots[i].GetComponent<Image>().color == Color.gray)
                {
                    break;
                }
            }
            if (i == 5)
            {
                for (i = 0; i < 5; i++)
                {
                    allSlots[i].clearSlot();
                }
                ShowCraftFormula(itemCrafting);
                allSlots[5].GetComponent<Image>().color = Color.white;
            }
        }
    }
    void putBackMaterial()
    {
        for (int i =0; i<5; ++i)
        {
            if(allSlots[i].items.Count > 1)
            {
                while (!allSlots[i].isEmpty)
                {
                    playerScript.pickUpItem(allSlots[i].removeItem());
                }
            }else if (allSlots[i].GetComponent<Image>().color == Color.white && !allSlots[i].isEmpty)
            {
                playerScript.pickUpItem(allSlots[i].items.Pop());
            }
        }

        if (allSlots[5].GetComponent<Image>().color == Color.white && !allSlots[5].isEmpty)
        {
            while (!allSlots[5].isEmpty)
                playerScript.pickUpItem(allSlots[5].items.Pop());
        }
    }
}
