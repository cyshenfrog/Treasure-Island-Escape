using UnityEngine;
using System.Collections;

public class Bag : MonoBehaviour {

    public Inventory inventory1, inventory2, inventory3;
    private int affordableWeight = 500;
    private int currentWeight;
    private RoleController roleController;

    void Start()
    {
        roleController = GameObject.Find("Role").GetComponent<RoleController>();
    }
    
    void OnTriggerStay(Collider other)
    {
        if (roleController.State == RoleState.PICKUP)
        {
            if (other.transform.parent == null && other.tag == "Item" && MoveToObject.pickUpTarget != null)
            {
                Item item = other.GetComponent<Item>();
                if (item.type == MoveToObject.pickUpTarget.type && other.transform.position == MoveToObject.pickUpTarget.transform.position)
                {
                    if (pickUpItem(other.GetComponent<Item>()))
                    {
                        Destroy(other.gameObject);
                    }
                    roleController.State = RoleState.IDLE;
                }
            }
            else if (other.transform.tag == "Items" && MoveToObject.pickUpTarget != null)
            {
                int childDeleted = 0;
                Transform parent = other.transform.parent;
                foreach (Transform child in parent)
                {
                    Item item = child.transform.GetComponent<Item>();
                    if (other.transform.parent != null && item.type == MoveToObject.pickUpTarget.type && item.transform.position == MoveToObject.pickUpTarget.transform.position)
                    {
                        if (pickUpItem(child.GetComponent<Item>()))
                        {
                            Destroy(child.gameObject);
                            ++childDeleted;
                        }
                    }
                    else //if item type != pickUpTarget type. There is no need to continue the foreach loop.
                    {
                        break;
                    }
                }
                if (parent.transform.childCount == childDeleted)
                    Destroy(parent.gameObject);
                roleController.State = RoleState.IDLE;
            }
            else if (other.transform.parent!=null && other.transform.tag == "Item" && MoveToObject.pickUpTarget != null) {
                Item item = other.GetComponent<Item>();
                if (item.type == MoveToObject.pickUpTarget.type && other.transform.position == MoveToObject.pickUpTarget.transform.position)
                {
                    GameObject parent = other.transform.parent.gameObject;
                    if (pickUpItem(other.GetComponent<Item>()))
                    {
                        Destroy(other.gameObject);
                        Destroy(parent);
                    }
                    roleController.State = RoleState.IDLE;
                }
            }
        }
    }

    public bool pickUpItem(Item item)
    {

        if ((affordableWeight - currentWeight) < item.weight)   //make sure player can afford the weight
            return false;

        if (item.maxStackSize == 1)
        {
            if (inventory1.placeEmpty(item))
            {
                updateCraftInventory(item);
                updateCookingInventory(item);
                return true;
            }
            else if (inventory2.placeEmpty(item))
            {
                updateCraftInventory(item);
                updateCookingInventory(item);
                return true;
            }
            else if (inventory3.placeEmpty(item))
            {
                updateCraftInventory(item);
                updateCookingInventory(item);
                return true;
            }
        }
        else
        {
            if (inventory1.addItem(item))
            {
                updateCraftInventory(item);
                updateCookingInventory(item);
                return true;
            }
            else if (inventory2.addItem(item))
            {
                updateCraftInventory(item);
                updateCookingInventory(item);
                return true;
            }
            else if (inventory3.addItem(item))
            {
                updateCraftInventory(item);
                updateCookingInventory(item);
                return true;
            }
        }
        return false;
    }
    void updateCraftInventory(Item item)
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
    void updateCookingInventory(Item item)
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
