﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CraftSystemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image itemImage;
    public Text owned_need;

    public Stack<Item> items;
    public Stack<Item> Items
    {
        get { return items; }
        set { items = value; }
    }
    
    public Sprite slotEmpty;
    // Use this for initialization
    //private Canvas canvas;

    [SerializeField]
    private static GameObject toolTip;
    [SerializeField]
    private static Text sizeText;
    [SerializeField]
    private static Text visualText;

    void Start()
    {
        items = new Stack<Item>();

        Transform background = transform.Find("Background");
        itemImage = background.Find("Icon").GetComponent<Image>();
        owned_need = background.Find("Number").GetComponent<Text>();

        if (toolTip == null && Slot.toolTip!=null)
        {
            toolTip = Slot.toolTip;
            sizeText = Slot.sizeText;
            visualText = Slot.visualText;
        }else if (toolTip == null)
        {
            toolTip = GameObject.Find("ToolTip");
            sizeText = GameObject.Find("ToolTipSize").GetComponent<Text>();
            visualText = GameObject.Find("ToolTipText").GetComponent<Text>();
        }

    }

    public void showToolTip()
    {
        if (!isEmpty && GameObject.Find("HoverIcon(Clone)") == null)
        {
            toolTip.SetActive(true);
            float xPos = transform.position.x + 1;
            float yPos = transform.position.y + 30;
            float zPos = transform.position.z + 1;
            toolTip.transform.position = new Vector3(xPos, yPos, zPos);
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
        changeSprite(item.icon);
    }

    private void changeSprite(Sprite icon)
    {
        itemImage.sprite = icon;
    }
    
    public void clearSlot()
    {
        items.Clear();
        changeSprite(slotEmpty);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        showToolTip();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hideToolTip();
    }
    public bool isEmpty
    {
        get { return items.Count == 0; }
    }
    public Item currentItem
    {
        get { return items.Peek(); }
    }
}