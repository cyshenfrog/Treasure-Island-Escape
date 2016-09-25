using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CookingSystem : InventoryManager
{
    public Inventory[] inventory = new Inventory[3];

    private Text title;
    private List<CraftSystemSlot> necessarySlots;
    public List<CraftSystemSlot> NecessarySlots
    {
        get { return necessarySlots; }
    }
    private List<Slot> optionalSlots;
    public List<Slot> OptionalSlots
    {
        get { return optionalSlots; }
    }
    private Text tooltip;

    private Button btn; //tempary variable

    private EventSystem eventSystem;

    private string itemCrafting;
    // Use this for initialization
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        CreateCookingLayout();
    }
    private void CreateCookingLayout()
    {
        //get title
        title = transform.Find("Name").GetComponent<Text>();

        //get necessary slots
        necessarySlots = new List<CraftSystemSlot>();
        Transform necessaryItemLayout = transform.Find("NecessaryItemLayout");
        foreach (Transform child in necessaryItemLayout)
        {
            CraftSystemSlot slot = child.GetComponent<CraftSystemSlot>();
            if (slot != null)
            {
                necessarySlots.Add(slot);
            }
        }

        //get optional slots
        optionalSlots = new List<Slot>();
        Transform optionalItemLayout;
        optionalItemLayout = transform.Find("ItemLayout");
        foreach (Transform child in optionalItemLayout)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null)
            {
                optionalSlots.Add(slot);
                btn = slot.GetComponent<Button>();
                GameObject temp = child.gameObject;
                btn.onClick.AddListener(delegate { moveItem(temp); });
            }
        }

        //get tooltip
        tooltip = transform.Find("TextBackground").Find("Text").GetComponent<Text>();
    }

    public void ShowCookingFormula(string itemToCraft, string itemToolTip)
    {
        if (CookingFormulaList.formula.ContainsKey(itemToCraft))
        {
            itemCrafting = itemToCraft;
            putBackMaterial();

            //change craft inventory title
            title.text = itemToCraft;

            //show necessary items
            string[] split = CookingFormulaList.formula[itemToCraft].Split('-');
            Item material;
            string path;
            for (int i = 0; i < 2; ++i)
            {
                if (split[i * 2] != "Null")
                {
                    path = string.Concat("Inventory/Items/", split[i * 2]);
                    material = (Resources.Load(path, typeof(GameObject)) as GameObject).GetComponent<Item>();
                    if (!necessarySlots[i].isEmpty)
                    {
                        necessarySlots[i].clearSlot();
                    }
                    necessarySlots[i].addItem(material);
                    necessarySlots[i].itemImage.color = Color.gray;
                    necessarySlots[i].owned_need.text = string.Concat("0/", split[i * 2 + 1]);
                }
                else
                {
                    necessarySlots[i].owned_need.text = "0/0";
                }
            }

            //check if user has enough material for necessary items
            searchItemsInBag();
            
            /*
            path = string.Concat("Inventory/Items", itemToCraft);
            material = (Resources.Load(path, typeof(GameObject)) as GameObject).GetComponent<Item>();
            
            if (!allSlots[5].isEmpty)
            {
                allSlots[5].clearSlot();
            }
            allSlots[5].addItem(material);
            allSlots[5].GetComponent<Image>().color = Color.gray;
            */

            //show tool tip
            tooltip.text = itemToolTip;
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
                    clicked.GetComponent<Image>().color = Color.gray;
                    createHoverIcon();
                    hoverText.text = from.Items.Count > 1 ? from.Items.Count.ToString() : string.Empty;
                }
            }
        }
    }
    public void craft()
    {
        int[] need = new int[2];
        if (itemCrafting != null)
        {
            int i;
            for (i = 0; i < 2; i++)
            {
                Text temp = necessarySlots[i].transform.GetChild(1).GetComponent<Text>();
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
                    for (int l = 0; l < 2; ++l)
                    {
                        if (!necessarySlots[l].isEmpty)
                        {
                            Slot slot = inventory[j].AllSlots[k].GetComponent<Slot>();
                            if (!slot.isEmpty && necessarySlots[l].currentItem.type == slot.currentItem.type && need[l] > 0)
                            {
                                if (need[l] >= slot.items.Count)
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
            //allSlots[5].GetComponent<Image>().color = Color.white;
        }

    }

    void putBackMaterial()
    {/*
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
        }*/
    }

    public void searchItemsInBag()
    {
        int[] count = new int[2];
        for (int i = 0; i < 2; ++i)
        {
            count[i] = 0;
        }

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                for (int k = 0; k < 2; k++)
                {
                    if (!necessarySlots[k].isEmpty)
                    {
                        Slot slot = inventory[i].AllSlots[j].GetComponent<Slot>();
                        if (!slot.isEmpty && necessarySlots[k].currentItem.type == slot.currentItem.type)
                        {
                            count[k] += slot.items.Count;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < 2; ++i)
        {
            if (!necessarySlots[i].isEmpty)
            {
                Text temp = necessarySlots[i].owned_need;
                String[] split = temp.text.Split('/');
                if (count[i] >= int.Parse(split[1]))
                    necessarySlots[i].itemImage.color = Color.white;
                else
                    necessarySlots[i].itemImage.color = Color.gray;
                necessarySlots[i].owned_need.text = string.Concat(count[i].ToString(), "/");
                temp.text = string.Concat(temp.text, split[1]);
            }
        }
    }

    private bool BagIsAvailiable(Item item, int count)
    {
        if (item.maxStackSize == 1)
        {
            for (int i = 0; i < 3; ++i)
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

    /*
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
    }*/
}
