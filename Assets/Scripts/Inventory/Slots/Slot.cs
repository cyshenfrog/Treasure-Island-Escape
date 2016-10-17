﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Stack<Item> items;
    public Stack<Item> Items
    {
        get { return items; }
        set { items = value; }
    }

    private Image itemImage;
    private Image slotColor;
    private Text stackText;
    public Sprite slotEmpty;
    // Use this for initialization
    //private Canvas canvas;

    public static GameObject toolTip;
    public static Text sizeText;
    public static Text visualText;

	void Start () {
        stackText = gameObject.GetComponentInChildren<Text>();
        slotColor = transform.GetComponent<Image>();
        itemImage = transform.Find("ItemImage").GetComponent<Image>();

        items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform textRect = stackText.GetComponent<RectTransform>();

        int textScale = (int) (slotRect.sizeDelta.x * 0.6f);
        stackText.resizeTextMaxSize = textScale;
        stackText.resizeTextMinSize = textScale;
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
        textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);

        //canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (toolTip == null)
        {
            toolTip = GameObject.Find("ToolTip");
            sizeText = GameObject.Find("ToolTipSize").GetComponent<Text>();
            visualText = GameObject.Find("ToolTipText").GetComponent<Text>();
            toolTip.SetActive(false);
        }
    }
	
    public void showToolTip()
    {
        if(!isEmpty && GameObject.Find("HoverIcon(Clone)") == null)
        {
            toolTip.SetActive(true);

            //canvas will scale with screen size. Therefore, we have to scale offset with screen size too.
            float scaleFactor = 0;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            float logWidth = Mathf.Log(screenSize.x / 1920.0f, 2);
            float logHeight = Mathf.Log(screenSize.y / 1080.0f, 2);
            float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, 1);
            scaleFactor = Mathf.Pow(2, logWeightedAverage);

            float xPos = transform.position.x + 20*scaleFactor;
            float yPos = transform.position.y + 50*scaleFactor;
            float zPos = transform.position.z + 1*scaleFactor;
            toolTip.transform.position = new Vector3(xPos,yPos ,zPos);
            
            string content = currentItem.toolTipContent();
            sizeText.text = content;
            visualText.text = content;
        }
        
    }
    public void hideToolTip()
    {
        toolTip.SetActive(false);
    }
    public void addItem(Item item)
    {
        items.Push(item);
        if(items.Count > 1)
        {
            stackText.text = items.Count.ToString();
        }
        changeSprite(item.icon);
    }

    public void addItems(Stack<Item> items)
    {
        this.items = new Stack<Item>(items);
        stackText.text = items.Count > 1 ? items.Count.ToString() : null;
        changeSprite(currentItem.icon);

    }


    private void changeSprite(Sprite icon)
    {
        itemImage.sprite = icon;
    }
    public void changeSlotColorToWhite()
    {
        itemImage.color = Color.white;
        slotColor.color = Color.white;
    }
    public void changeSlotColorToGray()
    {
        itemImage.color = Color.gray;
        slotColor.color = Color.gray;
    }
    private void useItem()
    {
        if (!isEmpty)
        {
            Item temp = items.Pop();
            updateCookingSystem(temp);
            updateCraftSystem(temp);
            temp.use();
            stackText.text = items.Count > 1 ? items.Count.ToString() : null;
            if (isEmpty)
            {
                changeSprite(slotEmpty);
            }
        }
    }
    public void clearSlot()
    {
        items.Clear();
        changeSprite(slotEmpty);
        stackText.text = null;
    }
    public Stack<Item> removeItems(int amount)
    {
        Stack<Item> temp = new Stack<Item>();
        for(int i=0;i< amount; ++i)
        {
            temp.Push(items.Pop());
        }
        stackText.text = items.Count > 1 ? items.Count.ToString() : null;
        return temp;

    }
    public Item removeItem()
    {
        Item temp;
        temp = items.Pop();
        stackText.text = items.Count > 1 ? items.Count.ToString() : null;
        return temp;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("HoverIcon(Clone)") && transform.parent.transform.tag != "Bank" && transform.tag != "MaterialSlot" && itemImage.color!=Color.gray)
        {
            useItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !isEmpty && !GameObject.Find("HoverIcon(Clone)") && transform.tag != "ResultSlot")
        {
            //Vector2 position;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            InventoryManager.SelectStackSize.SetActive(true);
            //Inventory.SelectStackSize.transform.position = canvas.transform.TransformPoint(position);
            InventoryManager.setStackInfo(items.Count);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        showToolTip();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hideToolTip();
    }

    void updateCraftSystem(Item item)
    {
        GameObject temp = GameObject.Find("MakingWindow");
        if (temp)
        {
            CraftSystem craftSystem = temp.GetComponent<CraftSystem>();
            for (int i = 0; i < 5; ++i)
            {
                if (!craftSystem.AllSlots[i].isEmpty && craftSystem.AllSlots[i].currentItem.type == item.type)
                {
                    craftSystem.searchItemsInBag();
                    return;
                }
            }
        }
    }
    void updateCookingSystem(Item item)
    {
        GameObject temp = GameObject.Find("CookingWindow");
        if (temp)
        {
            CookingSystem cookingSystem = temp.GetComponent<CookingSystem>();
            for (int i = 0; i < 2; ++i)
            {
                if (!cookingSystem.NecessarySlots[i].isEmpty && cookingSystem.NecessarySlots[i].currentItem.type == item.type)
                {
                    cookingSystem.searchItemsInBag();
                    return;
                }
            }
        }
    }
    public bool isEmpty
    {
        get { return items.Count == 0; }
    }
    public Item currentItem
    {
        get { return items.Peek(); }
    }
    public bool isStackable
    {
        get { return currentItem.maxStackSize > items.Count; }
    }
}
