using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : InventoryManager {
    
    private List<GameObject> allSlots;

    public List<GameObject> AllSlots
    {
        get{ return allSlots;}
    }
    /*
    private int emptySlot;
    public int EmptySlot
    {
        set { emptySlot = value; }
        get { return emptySlot; }
    }
    */
    private Button btn;
    
    private EventSystem eventSystem;
    private GameObject dropItem;
    
    // Use this for initialization
    void Start () {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        CreateInventoryLayout();
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
                    float angle = Random.Range(0.0f, Mathf.PI * 2);
                    Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
                    foreach (Item item in from.Items)
                    {
                        Instantiate(dropItem, player.transform.position - 3 * v, Quaternion.identity);
                    }
                }
                //from.transform.parent.GetComponent<Inventory>().emptySlot++;
                from.clearSlot();
                Destroy(hoverObj);
                to = null;
                from = null;
                
                //transform.parent.GetChild(2).GetComponent<Inventory>().emptySlot++; //效能有待加強
                
            }
            else if(!eventSystem.IsPointerOverGameObject(-1) && !movingSlot.isEmpty)
            {
                dropItem = movingSlot.currentItem.dropItem;
                float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f );
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
	
    private void CreateInventoryLayout()
    {
        allSlots = new List<GameObject>();
        Transform slotLayout = transform.Find("SlotLayout");
        foreach (Transform child in slotLayout)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null)
            {
                allSlots.Add(child.gameObject);
                btn = slot.GetComponent<Button>();
                GameObject temp = child.gameObject;
                btn.onClick.AddListener(delegate { moveItem(temp); });
            }
        }
        Button arrowUp = transform.Find("arrowUp").GetComponent<Button>();
        arrowUp.onClick.AddListener(() => gameObject.transform.SetAsFirstSibling());
        Button arrowDown = transform.Find("arrowDown").GetComponent<Button>();
        arrowDown.onClick.AddListener(() => gameObject.transform.parent.GetChild(0).transform.SetAsLastSibling());
    }
    
    public bool addItem(Item item)
    {
        if (item.maxStackSize == 1)
        {
            return placeEmpty(item);
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
    }
    
    public bool placeEmpty(Item item)
    {
        
        foreach(GameObject slot in allSlots)
        {
            Slot temp = slot.GetComponent<Slot>();
            if (temp.isEmpty)
            {
                temp.addItem(item);
                return true;
            }
        }               
        return false;
        
    }
}
