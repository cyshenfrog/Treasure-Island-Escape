using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MoveToObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    private RoleController role;  //the role that player control
    private Bag bag;

    static public Item pickUpTarget;
    
    public static GameObject itemName;
    public static Text sizeText;
    public static Text visualText;

    void Start() {
        role = GameObject.Find("Role").GetComponent<RoleController>();
        bag = role.GetComponent<Bag>();
        if (itemName == null)
        {
            itemName = GameObject.Find("ItemNameTag");
            sizeText = itemName.transform.Find("ItemNameTagSize").GetComponent<Text>();
            visualText = sizeText.transform.Find("ItemNameTagText").GetComponent<Text>();
            itemName.SetActive(false);
        }
    }

    void Update() {
        if (Input.GetKeyDown("space")) {
            //role.SendObjPosition(transform.position);
            if (pickUpTarget == null)
            {
                pickUpTarget = findClosestItem();
                if(pickUpTarget != null)
                    role.SendObjPosition(pickUpTarget.transform.position);
            }
        }
        if (role.State != RoleState.PICKUP && pickUpTarget != null)
            pickUpTarget = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //call api to move the position of object 
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            role.MoveToTarget(transform.position);
            pickUpTarget = transform.GetComponent<Item>();
        }
    }
    
    Item findClosestItem()
    {
        float minDistance = 1000000;
        int index = -1, i=0;
        List<GameObject> itemList = new List<GameObject>();
        //find item with tag
        GameObject[] singleItems = GameObject.FindGameObjectsWithTag("Item");
        GameObject[] stackItems = GameObject.FindGameObjectsWithTag("Items");

        //add items to a list
        foreach (GameObject item in singleItems)
        {
            itemList.Add(item);
        }
        foreach (GameObject item in stackItems)
        {
            itemList.Add(item);
        }

        //find closest item
        foreach (GameObject item in itemList)
        {
            float distance = Vector2.Distance(item.transform.position, role.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
            ++i;
        }
        
        if (index == -1)
            return null;
        else {
            if (itemList[index].transform.tag == "Item")
                return itemList[index].GetComponent<Item>();
            else if (itemList[index].transform.tag == "Items")
                return itemList[index].GetComponentInChildren<Item>();
            else
                return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showItemName();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hideItemName();
    }
    
    void showItemName()
    {
        if (GameObject.Find("HoverIcon(Clone)") == null)
        {
            itemName.SetActive(true);

            RectTransform CanvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
            
            //lower left corner is 0,0 && upper right corner is 1,1
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(transform.position);

            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. 
            //Because of this, we need to subtract the height / width of the canvas * 0.5 to get the correct position.
            Vector2 WorldObject_ScreenPosition = new Vector2(
                                                    ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                                                    ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f))
                                                );
            
            itemName.GetComponent<RectTransform>().anchoredPosition = WorldObject_ScreenPosition + new Vector2(0, -75);

            string content = transform.GetComponent<Item>().type.ToString();
            if(transform.parent != null && transform.parent.tag == "Items")
            {
                string itemCount = "*" + transform.parent.transform.childCount.ToString(); 
                content = string.Concat(content, itemCount);
            }
            else
            {
                string itemCount = "*1";
                content = string.Concat(content, itemCount);
            }
            sizeText.text = content;
            visualText.text = content;
        }
    }

    void hideItemName()
    {
        itemName.SetActive(false);
    }

    void OnDestroy()
    {
        if (role != null)
        {
            role.CancelMoveToTarget();
        }
        if (itemName!=null && itemName.activeSelf == true)
            itemName.SetActive(false);
    }

}
