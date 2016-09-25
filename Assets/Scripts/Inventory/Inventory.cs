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

                    GameObject dropItems = createDropItemsParent(player.transform.position - 3 * v);
                    
                    foreach (Item item in from.Items)
                    {
                        GameObject temp = Instantiate(dropItem);
                        temp.transform.position = player.transform.position - 3 * v;
                        temp.transform.parent = dropItems.transform;
                        temp.tag = "Untagged";
                    }
                }
                //from.transform.parent.GetComponent<Inventory>().emptySlot++;
                from.clearSlot();
                Destroy(hoverObj);
                to = null;
                from = null;

                Item itemToSearch = dropItem.GetComponent<Item>();
                updateCookingSystem(itemToSearch);
                updateCraftSystem(itemToSearch);
                
                //transform.parent.GetChild(2).GetComponent<Inventory>().emptySlot++; //效能有待加強
            }
            else if(!eventSystem.IsPointerOverGameObject(-1) && !movingSlot.isEmpty)
            {
                dropItem = movingSlot.currentItem.dropItem;
                float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f );

                GameObject dropItems = createDropItemsParent(player.transform.position - 3 * v);

                foreach (Item item in movingSlot.Items)
                {
                    GameObject temp = Instantiate(dropItem);
                    temp.transform.position = player.transform.position - 3 * v;
                    temp.transform.parent = dropItems.transform;
                    temp.tag = "Untagged";
                }
                
                movingSlot.clearSlot();
                Destroy(hoverObj);

                Item itemToSearch = dropItem.GetComponent<Item>();
                updateCookingSystem(itemToSearch);
                updateCraftSystem(itemToSearch);
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
    
    GameObject createDropItemsParent(Vector3 position)
    {
        GameObject dropItems = new GameObject("Drop");
        dropItems.tag = "Items";
        dropItems.AddComponent<BoxCollider>().isTrigger = true;
        dropItems.transform.position = position;
        return dropItems;
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
}
