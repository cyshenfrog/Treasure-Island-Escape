using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TargetBarSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public bool init = false;

    void Start()
    {
        if (!init)
        {
            items = new Stack<Item>();

            if (toolTip == null && Slot.toolTip != null)
            {
                toolTip = Slot.toolTip;
                sizeText = Slot.sizeText;
                visualText = Slot.visualText;
            }
            else if (toolTip == null)
            {
                toolTip = GameObject.Find("ToolTip");
                sizeText = GameObject.Find("ToolTipSize").GetComponent<Text>();
                visualText = GameObject.Find("ToolTipText").GetComponent<Text>();
            }
            init = true;
        }
    }

    public void initialize()
    {
        items = new Stack<Item>();

        if (toolTip == null && Slot.toolTip != null)
        {
            toolTip = Slot.toolTip;
            sizeText = Slot.sizeText;
            visualText = Slot.visualText;
        }
        else if (toolTip == null)
        {
            toolTip = GameObject.Find("ToolTip");
            sizeText = GameObject.Find("ToolTipSize").GetComponent<Text>();
            visualText = GameObject.Find("ToolTipText").GetComponent<Text>();
        }
        init = true;
    }

    public void showToolTip()
    {
        if (!isEmpty && GameObject.Find("HoverIcon(Clone)") == null)
        {
            toolTip.SetActive(true);

            //canvas will scale with screen size. Therefore, we have to scale offset with screen size too.
            float scaleFactor;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            float logWidth = Mathf.Log(screenSize.x / 1920.0f, 2);
            float logHeight = Mathf.Log(screenSize.y / 1080.0f, 2);
            float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, 1);
            scaleFactor = Mathf.Pow(2, logWeightedAverage);
            
            float xPos = transform.position.x + 150*scaleFactor;
            float yPos = transform.position.y - 125*scaleFactor;
            float zPos = transform.position.z + 1*scaleFactor;
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
