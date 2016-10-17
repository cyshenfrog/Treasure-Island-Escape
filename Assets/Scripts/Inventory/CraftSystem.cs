﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CraftSystem : InventoryManager
{
    public Bag bag;
    public Inventory[] inventory = new Inventory[3];

    private Text title;
    private List<CraftSystemSlot> allSlots;
    public List<CraftSystemSlot> AllSlots
    {
        get { return allSlots; }
    }
    private Slot resultSlot;
    public Slot ResultSlot
    {
        get { return resultSlot; }
    }
    private Text tooltip;

    private Button btn;

    private EventSystem eventSystem;

    private string itemCrafting;
    // Use this for initialization
    void Start()
    {
        bag = GameObject.Find("Role").GetComponent<Bag>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        CreateCraftLayout();
    }
    private void CreateCraftLayout()
    {
        title = transform.Find("Name").GetComponent<Text>();

        allSlots = new List<CraftSystemSlot>();
        Transform itemLayout = transform.Find("ItemLayout");
        foreach (Transform child in itemLayout)
        {
            CraftSystemSlot slot = child.GetComponent<CraftSystemSlot>();
            if (slot != null)
            {
                allSlots.Add(slot);
            }
        }
        
        resultSlot = transform.Find("Result").Find("slot").GetComponent<Slot>();
        btn = resultSlot.GetComponent<Button>();
        btn.onClick.AddListener(delegate { moveResultItem(resultSlot.gameObject); });

        tooltip = transform.Find("TextBackground").Find("Text").GetComponent<Text>();
    }
    public void ShowCraftFormula(string itemToCraft, string itemToolTip)
    {   
        if (CraftFormulaList.formula.ContainsKey(itemToCraft))
        {
            itemCrafting = itemToCraft;
            putBackMaterial();

            //change craft inventory title
            title.text = itemToCraft;

            //show formula
            string[] split = CraftFormulaList.formula[itemToCraft].Split('-');
            Item material;
            string path;
            for (int i=0; i<5; ++i)
            {
                if (split[i*2] != "Null")
                {
                    path = string.Concat("Inventory/Items/", split[i*2]);
                    material = (Resources.Load(path, typeof(GameObject)) as GameObject).GetComponent<Item>();
                    if (!allSlots[i].isEmpty)
                    {
                        allSlots[i].clearSlot();
                    }
                    allSlots[i].addItem(material);
                    allSlots[i].itemImage.color = Color.gray;
                    allSlots[i].owned_need.text = string.Concat("0/", split[i * 2 + 1]);
                }
                else
                {
                    allSlots[i].owned_need.text = "0/0";
                }
            }

            //check if user has enough material for itemToCraft
            searchItemsInBag();

            path = string.Concat("Inventory/Items/", itemToCraft);
            material = (Resources.Load(path, typeof(GameObject)) as GameObject).GetComponent<Item>();

            if (!resultSlot.isEmpty)
            {
                resultSlot.clearSlot();
            }
            resultSlot.addItem(material);
            resultSlot.changeSlotColorToGray();

            //show tool tip
            tooltip.text = itemToolTip;
        }
    }
    public new void moveItem(GameObject clicked)
    {
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
                    clicked.GetComponent<Slot>().changeSlotColorToGray();
                    createHoverIcon();
                    hoverText.text = from.Items.Count > 1 ? from.Items.Count.ToString() : string.Empty;
                }
            }
        }
    }

    public void moveResultItem(GameObject clicked)
    {
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
                    temp.changeSlotColorToGray();
                    createHoverIcon();
                    hoverText.text = from.Items.Count > 1 ? from.Items.Count.ToString() : string.Empty;
                }
            }
        }
    }
    public void craft()
    {
        int[] need = new int[5];
        if (!resultSlot.isEmpty && resultSlot.GetComponent<Image>().color == Color.gray)
        {
            int i;
            for (i = 0; i < 5; i++)
            {
                Text temp = allSlots[i].owned_need;
                String[] split = temp.text.Split('/');  // owened / needed
                need[i] = int.Parse(split[1]);
                if (int.Parse(split[1]) > int.Parse(split[0]))  // if need > owned
                    return;
            }
            
            //delete used material
            for (int j = 0; j < 3; ++j)
            {
                for (int k = 0; k < 10; ++k)
                {
                    for (int l = 0; l < 5; ++l)
                    {
                        if (!allSlots[l].isEmpty)
                        {
                            Slot slot = inventory[j].AllSlots[k].GetComponent<Slot>();
                            if (!slot.isEmpty && allSlots[l].currentItem.type == slot.currentItem.type && need[l] > 0)
                            {
                                if(need[l] >= slot.items.Count)
                                {
                                    need[l] -= slot.items.Count;
                                    slot.clearSlot();
                                }
                                else
                                {
                                    slot.removeItems(need[l]);
                                    need[l] = 0;
                                }
                            }
                        }
                    }
                }
            }

            resultSlot.changeSlotColorToWhite();
            searchItemsInBag();
            //allSlots[5].GetComponent<Image>().color = Color.white;
        }
        
    }

    void putBackMaterial()
    {
        if (resultSlot.GetComponent<Image>().color == Color.white && !resultSlot.isEmpty)
        {
            if (BagIsAvailiable(resultSlot.currentItem, resultSlot.items.Count))
            {
                foreach (Item item in resultSlot.Items)
                {
                    bag.pickUpItem(resultSlot.currentItem);
                }
                resultSlot.clearSlot();
            }
            else
            {/*
                float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
                foreach (Item item in resultSlot.Items)
                {
                    Instantiate(resultSlot.currentItem.dropItem, player.transform.position - 3 * v, Quaternion.identity);
                }
                resultSlot.clearSlot();*/
            }
        }
    }
    public void searchItemsInBag()
    {
        int[] count = new int [5];
        for (int i=0; i < 5; ++i)
        {
            count[i] = 0;
        }

        for(int i = 0; i < 3; ++i)
        {
            for(int j = 0; j < 10; ++j)
            {
                for(int k = 0; k < 5; k++)
                {
                    if (!allSlots[k].isEmpty)
                    {
                        Slot slot = inventory[i].AllSlots[j].GetComponent<Slot>();
                        if (!slot.isEmpty && allSlots[k].currentItem.type == slot.currentItem.type)
                        {
                            count[k] += slot.items.Count;
                        }
                    }
                }
            }
        }

        for(int i = 0; i < 5; ++i)
        {
            if (!allSlots[i].isEmpty)
            {
                Text temp = allSlots[i].owned_need;
                String[] split = temp.text.Split('/');
                if (count[i] >= int.Parse(split[1]))
                    allSlots[i].itemImage.color = Color.white;
                else
                    allSlots[i].itemImage.color = Color.gray;
                allSlots[i].owned_need.text = string.Concat(count[i].ToString(), "/");
                temp.text = string.Concat(temp.text, split[1] );
            }
        }
    }

    private bool BagIsAvailiable(Item item, int count)
    {
        if(item.maxStackSize == 1)
        {
            for(int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (inventory[i].AllSlots[j].GetComponent<Slot>().isEmpty)
                        return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    Slot temp = inventory[i].AllSlots[j].GetComponent<Slot>();
                    if (temp.isEmpty)
                        return true;
                    if (!temp.isEmpty && temp.currentItem.type == item.type && temp.isStackable)
                    {
                        count -= (item.maxStackSize - temp.items.Count);
                        if (count <= 0)
                            return true;
                    }
                }
            }
        }
        return false;
    }

    public void RightArrowClicked()
    {
        if (!resultSlot.isEmpty && resultSlot.isStackable)
        {
            for (int i = 0; i < 2; ++i)
            {
                Text temp = allSlots[i].owned_need;
                String[] split = temp.text.Split('/');
                int need = int.Parse(split[1]);
                need += need / resultSlot.items.Count;
                temp.text = string.Concat(split[0], "/");
                temp.text = string.Concat(temp.text, need);
            }
            searchItemsInBag();
            resultSlot.addItem(resultSlot.currentItem);
        }
    }
    public void LeftArrowClicked()
    {
        if (resultSlot.items.Count > 1)
        {
            for (int i = 0; i < 2; ++i)
            {
                Text temp = allSlots[i].owned_need;
                String[] split = temp.text.Split('/');
                int need = int.Parse(split[1]);
                need -= need / resultSlot.items.Count;
                temp.text = string.Concat(split[0], "/");
                temp.text = string.Concat(temp.text, need);
            }
            searchItemsInBag();
            resultSlot.removeItem();
        }
    }
}
