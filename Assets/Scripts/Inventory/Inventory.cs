using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {
    private RectTransform inventoryRect;
    private GameObject manager;
    private GameObject ArrowUp;

    private float inventoryHeight, inventoryWidth;
    
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;
    public int slots, rows;

    private GameObject slotPrefab;

    private List<GameObject> allSlots;
    private static Slot from, to;
    private static GameObject click;
    private int emptySlot;
    public int EmptySlot
    {
        set { emptySlot = value; }
        get { return emptySlot; }
    }

    private Button btn;

    private GameObject hoverPrefab;
    private static GameObject hoverObj;
    private Text hoverText;
    private Canvas canvas;

    private EventSystem eventSystem;

    private static GameObject selectStackSize;
    public static GameObject SelectStackSize {
        get { return selectStackSize; }
    }
    private static Text movingItemSize;
    private static int splitAmount;
    private static int maxStackCount;
    private static Slot movingSlot;

    private GameObject dropItem;
    private static GameObject player;
    //-------------------------------field for bank -------------------------------//
    private int emptyBankSlot;
    public int EmptyBankSlot
    {
        set { emptyBankSlot = value; }
        get { return emptyBankSlot; }
    }
    // Use this for initialization
    void Start () {
        slotPrefab = Resources.Load("Inventory/slot", typeof(GameObject)) as GameObject;
        ArrowUp = Resources.Load("Inventory/arrowUp", typeof(GameObject)) as GameObject;
        manager = GameObject.Find("InventoryManager");
        hoverPrefab = Resources.Load("Inventory/HoverIcon", typeof(GameObject)) as GameObject;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        if (selectStackSize == null)
        {
            selectStackSize = GameObject.Find("MoveItemStackSize");
            movingItemSize = GameObject.Find("MovingItemSize").GetComponent<Text>();
            movingSlot = GameObject.Find("MovingSlot").GetComponent<Slot>();
            //Debug.Log(movingItemSize);
            selectStackSize.SetActive(false);
        }
        if (transform.gameObject.tag == "Inventory")
        {
            CreateInventoryLayout();
        }else if(transform.gameObject.tag == "Bank")
        {
            CreateBankLayout();
        }
        
        foreach(GameObject slot in allSlots)
        {
            btn = slot.GetComponent<Button>();
            GameObject temp = slot;
            btn.onClick.AddListener(delegate { moveItem(temp); });
        }
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null)
            {               
                from.GetComponent<Image>().color = Color.white;
                
                dropItem = from.currentItem.dropItem;
                
                if (dropItem != null)
                {
                    float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                    Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f);
                    foreach (Item item in from.Items)
                    {
                        Instantiate(dropItem, player.transform.position - 3 * v, Quaternion.identity);
                    }
                }
                from.clearSlot();
                Destroy(hoverObj);
                to = null;
                from = null;
                if (transform.tag == "Inventory")
                    transform.parent.GetChild(2).GetComponent<Inventory>().emptySlot++; //效能有待加強
                else
                    emptyBankSlot++;
            }
            else if(!eventSystem.IsPointerOverGameObject(-1) && !movingSlot.isEmpty)
            {
                dropItem = movingSlot.currentItem.dropItem;
                float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                Vector3 v = new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
                foreach (Item item in movingSlot.Items)
                {
                    Instantiate(dropItem, player.transform.position - 3 * v, Quaternion.identity);
                }
                movingSlot.clearSlot();
                Destroy(hoverObj);
            }
        }
        if(hoverObj)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition,canvas.worldCamera, out position);
            hoverObj.transform.position = canvas.transform.TransformPoint(position);
        }
    }
	// Update is called once per frame
    private void CreateInventoryLayout()
    {
        allSlots = new List<GameObject>();
        emptySlot += slots;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;

        inventoryRect = GetComponent<RectTransform>();
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth +40);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int cols = slots / rows;
        Vector3 inventoryPos = inventoryRect.localPosition - new Vector3(inventoryWidth/2, -inventoryHeight);
        //-----------------------setting slot------------------//
        for(int y = 0; y < rows; y++)
        {
            for(int x=0; x< cols; x++)
            {
                GameObject newSlot = Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "slot";

                newSlot.transform.SetParent(this.transform.parent, false);

                float slotX = slotPaddingLeft * (x + 1) + ( slotSize * x ) - 5;
                float slotY = -slotPaddingTop * (y + 1) - ( slotSize * y );
                slotRect.localPosition = (inventoryPos) + new Vector3(slotX, slotY);

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                allSlots.Add(newSlot);
                newSlot.transform.SetParent(this.transform);
            }
        }
        //---------setting arrow----------------------//
        GameObject arrowUp = (GameObject)Instantiate(ArrowUp);
        RectTransform arrowUpRect = arrowUp.GetComponent<RectTransform>();

        arrowUpRect.transform.SetParent(this.transform.parent, false);
        float X = slotPaddingLeft * 10 + (slotSize * 10) + 5;
        float Y = -slotPaddingTop - slotSize + 13;

        arrowUpRect.localPosition = (inventoryPos) + new Vector3(X, Y);

        arrowUpRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize/2);
        arrowUpRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize/2);
        
        arrowUpRect.transform.SetParent(this.transform);
        //---------set parent to manager-----------------//
        inventoryRect.transform.SetParent(manager.transform);
        inventoryRect.SetAsLastSibling();
    }

    private void CreateBankLayout()
    {
        allSlots = new List<GameObject>();
        emptyBankSlot += slots;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;
        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;

        inventoryRect = GetComponent<RectTransform>();
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth * 5);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int cols = slots / rows;
        Vector3 inventoryPos = inventoryRect.localPosition - new Vector3(inventoryWidth / 2, -inventoryHeight);
        //-----------------------setting slot------------------//
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                GameObject newSlot = Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "slot";

                newSlot.transform.SetParent(this.transform.parent, false);

                float slotX = slotPaddingLeft * (x + 1) + (slotSize * x);
                float slotY = -slotPaddingTop * (y + 1) - (slotSize * y);
                slotRect.localPosition = (inventoryPos) + new Vector3(slotX, slotY);

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                allSlots.Add(newSlot);
                newSlot.transform.SetParent(this.transform);
            }
        }
    }

    public void setStackInfo(int maxStackCount)
    {
        selectStackSize.SetActive(true);
        splitAmount = 0;
        Inventory.maxStackCount = maxStackCount;
        movingItemSize.text = splitAmount.ToString();
    }
    public void splitStack()
    {
        selectStackSize.SetActive(false);
        Slot temp = click.GetComponent<Slot>();
        if(splitAmount == maxStackCount)
        {
            movingSlot.Items = temp.removeItems(splitAmount);
            createHoverIcon();
            hoverText.text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString() : null;
            if (temp.isEmpty)
                temp.clearSlot();
        }
        else if (splitAmount > 0 )
        {
            movingSlot.Items = temp.removeItems(splitAmount);
            createHoverIcon();
            hoverText.text = movingSlot.Items.Count > 1 ? movingSlot.Items.Count.ToString():null;
        }
    }
    public void mergeStack(Slot source, Slot dest)
    {
        int DestStackableSize = dest.currentItem.maxStackSize - dest.Items.Count;
        int count = source.Items.Count < DestStackableSize ? source.Items.Count : DestStackableSize;

        for(int i =0; i< count; ++i)
        {
            //Debug.Log("XDs");
            dest.addItem(source.removeItem());
            hoverText.text = source.Items.Count>1?source.Items.Count.ToString():null;
        }
        if(source.Items.Count == 0)
        {
            source.clearSlot();
            Destroy(hoverObj);
        }
    }
    public bool addItem(Item item)
    {
        if(item.maxStackSize == 1)
        {
            placeEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot temp = slot.GetComponent<Slot>();
                if ( (!temp.isEmpty) && (temp.currentItem.type == item.type) && (temp.isStackable) )
                {
                    temp.addItem(item);
                    return true;
                }
            }
            
            return placeEmpty(item);
        }
        //return false;
    }

    public void moveItem(GameObject clicked)
    {
        if (!movingSlot.isEmpty)
        {
            Slot tmp = clicked.GetComponent<Slot>();
            
            if (!tmp.isEmpty && tmp.currentItem.itemName == movingSlot.currentItem.itemName && tmp.isStackable)
            {
                mergeStack(movingSlot, tmp);
            }else if (!tmp.isEmpty && tmp.currentItem.itemName != movingSlot.currentItem.itemName)
            {
                //Debug.Log(tmp.currentItem + " " + movingSlot.currentItem);
                Stack<Item> tempTo = new Stack<Item>(tmp.Items);
                tmp.addItems(movingSlot.Items);
                if(tempTo.Count == 0)
                {
                    movingSlot.clearSlot();
                    Destroy(hoverObj);
                }
                else
                {
                    movingSlot.addItems(tempTo);
                    hoverObj.GetComponent<Image>().sprite = tempTo.Peek().icon;
                    hoverText.text = tempTo.Count>1?tempTo.Count.ToString():null;
                }
            }else if (tmp.isEmpty)
            {
                tmp.addItems(movingSlot.Items);
                movingSlot.clearSlot();
                Destroy(hoverObj);
            }
            return;
        }
        Slot temp = clicked.GetComponent<Slot>() ;
        if (movingSlot.isEmpty){
            if(!GameObject.Find("MoveItemStackSize"))
                click = clicked;
            if (from == null && !temp.isEmpty && !Input.GetKey(KeyCode.LeftShift))
            {
                from = temp;
                from.GetComponent<Image>().color = Color.gray;

                createHoverIcon();
                hoverText.text = from.Items.Count > 1 ? from.Items.Count.ToString() : null;

            }
            else if(to == null && from != null)
            {
                to = temp;
            }

            if(to!=null && from != null)
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
        }
    }

    public bool placeEmpty(Item item)
    {
        if(emptySlot > 0)
        {
            foreach(GameObject slot in allSlots)
            {
                Slot temp = slot.GetComponent<Slot>();
                if (temp.isEmpty)
                {
                    temp.addItem(item);
                    emptySlot -= 1;
                    return true;
                }
            }          
        }
        
        return false;
        
    }
    private void createHoverIcon()
    {
        hoverObj = Instantiate(hoverPrefab);

        hoverObj.GetComponent<Image>().sprite = click.GetComponent<Image>().sprite;

        RectTransform hoverRect = hoverObj.GetComponent<RectTransform>();
        
        hoverRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
        hoverRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

        hoverRect.SetParent(canvas.transform, false);
        hoverText = hoverObj.transform.GetChild(0).GetComponent<Text>();
        RectTransform hoverTextTransform = hoverText.GetComponent<RectTransform>();
        hoverTextTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
        hoverTextTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);
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
    public bool isFull
    {
        get { return emptySlot == 0; }
    }
        
}
