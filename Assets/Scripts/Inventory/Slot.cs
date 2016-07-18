using UnityEngine;
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

    private Text stackText;
    private Inventory inventory;
    public Sprite slotEmpty;
    // Use this for initialization
    //private Canvas canvas;

    private static GameObject toolTip;
    private static Text sizeText;
    private static Text visualText;

	void Start () {
        inventory = gameObject.transform.parent.GetComponent<Inventory>();
        stackText = gameObject.GetComponentInChildren<Text>();
        
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
	
	// Update is called once per frame
	void Update () {
	
	}
    public void showToolTip()
    {
        if(!isEmpty && GameObject.Find("HoverIcon(Clone)") == null)
        {
            toolTip.SetActive(true);
            float xPos = transform.position.x + 1;
            float yPos = transform.position.y;
            float zPos = transform.position.z + 1;
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
        GetComponent<Image>().sprite = icon;
    }

    private void useItem()
    {
        if (!isEmpty)
        {
            items.Pop().use();
            stackText.text = items.Count > 1 ? items.Count.ToString() : null;
            if (isEmpty)
            {
                changeSprite(slotEmpty);
                inventory.EmptySlot += 1;
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
        if(eventData.button == PointerEventData.InputButton.Right && !GameObject.Find("HoverIcon(Clone)") && transform.parent.transform.tag == "Inventory")
        {
            useItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Left && Input.GetKey(KeyCode.LeftShift) && !isEmpty && !GameObject.Find("HoverIcon(Clone)"))
        {
            //Vector2 position;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            Inventory.SelectStackSize.SetActive(true);
            //Inventory.SelectStackSize.transform.position = canvas.transform.TransformPoint(position);
            inventory.setStackInfo(items.Count);
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
