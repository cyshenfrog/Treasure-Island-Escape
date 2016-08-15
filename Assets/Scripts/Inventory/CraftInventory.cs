using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CraftInventory : InventoryManager
{
    public Inventory[] inventory = new Inventory[3];
    private List<Slot> allSlots;
    public List<Slot> AllSlots
    {
        get { return allSlots; }
    }

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
                    allSlots[i].transform.GetComponent<Image>().color = Color.white;
                    allSlots[i].transform.GetChild(1).GetComponent<Text>().text = string.Concat("0/", split[i * 2 + 1]);
                }
                else
                {
                    allSlots[i].transform.GetChild(1).GetComponent<Text>().text = "0/0";
                }
            }
            searchItemsInBag(); //check if user has enough material for itemToCraft

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
        }
    }
    public void craft()
    {
        int[] need = new int[5];
        if (itemCrafting != null)
        {
            int i;
            for (i = 0; i < 5; i++)
            {
                Text temp = allSlots[i].transform.GetChild(1).GetComponent<Text>();
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
                            if (!slot.isEmpty && allSlots[l].currentItem.itemName == slot.currentItem.itemName && need[l] > 0)
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

            searchItemsInBag();
            allSlots[5].GetComponent<Image>().color = Color.white;
        }
        
    }
    void putBackMaterial()
    {
        if (allSlots[5].GetComponent<Image>().color == Color.white && !allSlots[5].isEmpty)
        {
            if ( BagIsAvailiable( allSlots[5].currentItem, allSlots[5].items.Count ) )
            {
                foreach (Item item in allSlots[5].Items)
                {
                    playerScript.pickUpItem(allSlots[5].currentItem);
                }
                allSlots[5].clearSlot();
            }
            else
            {
                float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
                foreach (Item item in allSlots[5].Items)
                {
                    Instantiate(allSlots[5].currentItem.dropItem, player.transform.position - 3 * v, Quaternion.identity);
                }
                allSlots[5].clearSlot();
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
                        if (!slot.isEmpty && allSlots[k].currentItem.itemName == slot.currentItem.itemName)
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
                Text temp = allSlots[i].transform.GetChild(1).GetComponent<Text>();
                String[] split = temp.text.Split('/');
                temp.text = string.Concat(count[i].ToString(), "/");
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
                    if (!temp.isEmpty && temp.currentItem.itemName == item.itemName && temp.isStackable)
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
        if (!allSlots[5].isEmpty && allSlots[5].isStackable)
        {
            for(int i =0; i< 5; ++i)
            {
                Text temp = allSlots[i].transform.GetChild(1).GetComponent<Text>();
                String[] split = temp.text.Split('/');
                int need = int.Parse(split[1]);
                need += need / allSlots[5].items.Count;
                temp.text = string.Concat(split[0], "/");
                temp.text = string.Concat(temp.text, need);
            }
            allSlots[5].addItem(allSlots[5].currentItem);
        }
    }
    public void LeftArrowClicked()
    {
        if (allSlots[5].items.Count > 1)
        {
            for (int i = 0; i < 5; ++i)
            {
                Text temp = allSlots[i].transform.GetChild(1).GetComponent<Text>();
                String[] split = temp.text.Split('/');
                int need = int.Parse(split[1]);
                need -= need / allSlots[5].items.Count;
                temp.text = string.Concat(split[0], "/");
                temp.text = string.Concat(temp.text, need);
            }
            allSlots[5].removeItem();
        }
    }
}
