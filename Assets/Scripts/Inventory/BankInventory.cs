using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BankInventory : InventoryManager
{

    private List<GameObject> allSlots;
    
    private Button btn;
    
    private EventSystem eventSystem;
    private GameObject dropItem;
    // Use this for initialization
    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        
        CreateBankLayout();
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
               
            }
            else if (!eventSystem.IsPointerOverGameObject(-1) && !movingSlot.isEmpty)
            {
                dropItem = movingSlot.currentItem.dropItem;
                float angle = Random.Range(0.0f, Mathf.PI * 2);
                Vector3 v = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
                foreach (Item item in movingSlot.Items)
                {
                    Instantiate(dropItem, player.transform.position - 3 * v, Quaternion.identity);
                }
                movingSlot.clearSlot();
                Destroy(hoverObj);
            }
        }
        if (hoverObj)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
            hoverObj.transform.position = canvas.transform.TransformPoint(position);
        }
    }
    /*
    public void putItemBack()
    {
        if(from != null && from.transform.parent.tag=="Bank")
            moveItem(click);
        else if (!movingSlot.isEmpty)
        {
            foreach(Item item in movingSlot.items)
            {
                addItem(item);
            }
            movingSlot.clearSlot();
            Destroy(hoverObj);
        }
        selectStackSize.SetActive(false);
    }*/
    private void CreateBankLayout()
    {
        allSlots = new List<GameObject>();
        foreach (Transform child in transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null)
            {
                btn = slot.GetComponent<Button>();
                GameObject temp = child.gameObject;
                btn.onClick.AddListener(delegate { moveItem(temp); });
                allSlots.Add(child.gameObject);
            }
        }
    }
    /*
    public bool addItem(Item item)
    {
        if (item.maxStackSize == 1)
        {
            placeEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot temp = slot.GetComponent<Slot>();
                if ((!temp.isEmpty) && (temp.currentItem.type == item.type) && (temp.isStackable))
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

        foreach (GameObject slot in allSlots)
        {
            Slot temp = slot.GetComponent<Slot>();
            if (temp.isEmpty)
            {
                temp.addItem(item);
                return true;
            }
        }
        return false;

    }*/
}
