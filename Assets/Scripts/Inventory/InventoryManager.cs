using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
    public static Slot from, to;
    public static GameObject click;
    public static Canvas canvas;
    public static GameObject hoverPrefab;
    public static GameObject hoverObj;
    public static Text hoverText;
    public static GameObject player;
    public static Bag playerBackpack;

    public static GameObject selectStackSize;
    public static GameObject SelectStackSize {
        get { return selectStackSize; }
    }
    public static Text movingItemSize;
    public static int splitAmount;
    public static int maxStackCount;
    public static Slot movingSlot;
    
    // Use this for initialization
    void Start () {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        hoverPrefab = Resources.Load("Inventory/UIs/HoverIcon", typeof(GameObject)) as GameObject;
        if (selectStackSize == null)
        {
            selectStackSize = GameObject.Find("MoveItemStackSize");
            movingItemSize = GameObject.Find("MovingItemSize").GetComponent<Text>();
            movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
            selectStackSize.SetActive(false);
        }
        player = GameObject.Find("Role");
        playerBackpack = player.GetComponent<Bag>();
    }
    
    public static void setStackInfo(int maxStackCount)
    {
        selectStackSize.SetActive(true);
        splitAmount = 0;
        InventoryManager.maxStackCount = maxStackCount;
        movingItemSize.text = splitAmount.ToString();
    }
    public void moveItem(GameObject clicked)
    {
        if (!movingSlot.isEmpty)
        {
            Slot tmp = clicked.GetComponent<Slot>();

            if (!tmp.isEmpty && tmp.currentItem.itemName == movingSlot.currentItem.itemName && tmp.isStackable)
            {
                mergeStack(movingSlot, tmp);
            }
            else if (!tmp.isEmpty && tmp.currentItem.itemName != movingSlot.currentItem.itemName)
            {
                //Debug.Log(tmp.currentItem + " " + movingSlot.currentItem);
                Stack<Item> tempTo = new Stack<Item>(tmp.Items);
                tmp.addItems(movingSlot.Items);
                if (tempTo.Count == 0)
                {
                    movingSlot.clearSlot();
                    Destroy(hoverObj);
                }
                else
                {
                    movingSlot.addItems(tempTo);
                    hoverObj.GetComponent<Image>().sprite = tempTo.Peek().icon;
                    hoverText.text = tempTo.Count > 1 ? tempTo.Count.ToString() : string.Empty;
                }
            }
            else if (tmp.isEmpty)
            {
                tmp.addItems(movingSlot.Items);
                movingSlot.clearSlot();
                Destroy(hoverObj);
            }
            return;
        }
        Slot temp = clicked.GetComponent<Slot>();
        if (movingSlot.isEmpty)
        {
            if (!GameObject.Find("MoveItemStackSize"))
                click = clicked;
            if (from == null && !temp.isEmpty && !Input.GetKey(KeyCode.LeftShift))
            {
                from = temp;
                from.GetComponent<Image>().color = Color.gray;
                createHoverIcon();
                hoverText.text = from.Items.Count > 1 ? from.Items.Count.ToString() : string.Empty;
            }
            else if (to == null && from != null )
            {
                to = temp;
            }

            if (to != null && from != null)
            {
                if (from.tag != "ResultSlot")
                {
                    if (!to.isEmpty && to.currentItem.type == from.currentItem.type)
                    {
                        mergeStack(from, to);
                        from.GetComponent<Image>().color = Color.white;
                        from = null;
                        to = null;
                        Destroy(hoverObj);
                    }
                    else {
                        Stack<Item> tempTo = new Stack<Item>(to.Items);
                        to.addItems(from.Items);
                        if (tempTo.Count == 0)
                        {
                            from.clearSlot();
                        }
                        else
                        {
                            from.addItems(tempTo);
                        }
                        from.GetComponent<Image>().color = Color.white;
                        from = null;
                        to = null;
                        Destroy(hoverObj);
                    }
                }
                else
                {
                    if (!to.isEmpty && to.currentItem.type == from.currentItem.type && to.isStackable)
                    {
                        int DestStackableSize = to.currentItem.maxStackSize - to.Items.Count;
                        int count = from.Items.Count < DestStackableSize ? from.Items.Count : DestStackableSize;
                        if (!hoverText)
                            hoverText = hoverObj.transform.GetChild(0).GetComponent<Text>();
                        for (int i = 0; i < count; ++i)
                        {
                            to.addItem(from.items.Peek());
                            hoverText.text = (from.Items.Count - i).ToString();
                        }
                        if (hoverText.text == "0")
                        {
                            from = null;
                            Destroy(hoverObj);
                        }
                        to = null;
                    }else if (!to.isEmpty && to.currentItem.type == from.currentItem.type && !to.isStackable)
                    {
                        Stack<Item> tempTo = new Stack<Item>(from.Items);
                        while (tempTo.Count > 0)
                        {
                            playerBackpack.pickUpItem(tempTo.Pop());
                        }
                        from = null;
                        to = null;
                        Destroy(hoverObj);
                    }
                    else if (!to.isEmpty && to.currentItem.type != from.currentItem.type)
                    {
                        Stack<Item> tempTo = new Stack<Item>(to.Items);
                        to.addItems(from.Items);
                        while (tempTo.Count > 0)
                        {
                            playerBackpack.pickUpItem(tempTo.Pop());
                        }
                        Destroy(hoverObj);
                        from = null;
                        to = null;
                    }
                    else
                    {
                        to.addItems(from.items);
                        Destroy(hoverObj);
                        from = null;
                        to = null;
                    }
                }
            }
        }
    }
    public void mergeStack(Slot source, Slot dest)
    {
        int DestStackableSize = dest.currentItem.maxStackSize - dest.Items.Count;
        int count = source.Items.Count < DestStackableSize ? source.Items.Count : DestStackableSize;
        if (!hoverText)
            hoverText = hoverObj.transform.GetChild(0).GetComponent<Text>();
        for (int i = 0; i < count; ++i)
        {
            dest.addItem(source.removeItem());
            hoverText.text = source.Items.Count > 1 ? source.Items.Count.ToString() : string.Empty;
        }
        if (source.Items.Count == 0)
        {
            source.clearSlot();
            Destroy(hoverObj);
        }
    }
    public void splitStack()
    {
        selectStackSize.SetActive(false);
        Slot temp = click.GetComponent<Slot>();
        if (splitAmount == maxStackCount)
        {
            movingSlot.Items = temp.removeItems(splitAmount);
            createHoverIcon();
            hoverText.text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : string.Empty;
            if (temp.isEmpty)
                temp.clearSlot();
        }
        else if (splitAmount > 0)
        {
            movingSlot.Items = temp.removeItems(splitAmount);
            createHoverIcon();
            hoverText.text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : string.Empty;
        }
    }

    public void createHoverIcon()
    {
        hoverObj = Instantiate(hoverPrefab);
        
        hoverObj.GetComponent<Image>().sprite = click.transform.Find("ItemImage").GetComponent<Image>().sprite;

        RectTransform hoverRect = hoverObj.GetComponent<RectTransform>();

        //hoverRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25);
        //hoverRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25);

        hoverRect.SetParent(canvas.transform, false);
        hoverText = hoverObj.transform.GetChild(0).GetComponent<Text>();
        //RectTransform hoverTextTransform = hoverText.GetComponent<RectTransform>();
        //hoverTextTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25);
        //hoverTextTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25);
    }

    public void changeText(int i)
    {
        splitAmount += i;
        if (splitAmount < 0)
            splitAmount = 0;
        if (splitAmount > maxStackCount)
            splitAmount = maxStackCount;
        movingItemSize.text = splitAmount.ToString();
    }
    public void cancelSplitStack()
    {
        splitAmount = 0;
        splitStack();
    }
}
