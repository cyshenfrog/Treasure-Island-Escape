using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour {

    static private Dictionary<string, GameObject> itemPrefab = new Dictionary<string, GameObject>();

    static public void dropItem(GameObject item, int count, Vector3 position) {
        GameObject dropItems = createDropItemsParent(position);

        for (int i = 0; i < count; ++i)
        {
            GameObject temp = Instantiate(item);
            temp.transform.position = position;
            temp.transform.parent = dropItems.transform;
            if (count > 1)
                temp.tag = "Items";
            else if (count == 1)
                temp.tag = "Item";
        }
    }

    static public void dropItem(List<itemType> itemToDropList, List<int> amountList, List<Vector3> positionList) {
        int i = 0;
        foreach (itemType itemName in itemToDropList )
        {
            if (itemPrefab.ContainsKey(itemName.ToString()))
            {
                GameObject dropItems = createDropItemsParent(positionList[i]);
                for (int amount = 0; amount < amountList[i]; ++amount)
                {
                    GameObject temp = Instantiate(itemPrefab[itemName.ToString()]);
                    temp.transform.position = positionList[i];
                    temp.transform.parent = dropItems.transform;
                    if (amountList[i] > 1)
                        temp.tag = "Items";
                    else if (amountList[i] == 1)
                        temp.tag = "Item";
                }
            }
            else {
                string path = string.Concat("Inventory/Items/", itemName.ToString());
                itemPrefab.Add(itemName.ToString(), (Resources.Load(path, typeof(GameObject)) as GameObject));

                GameObject dropItems = createDropItemsParent(positionList[i]);
                for (int amount = 0; amount < amountList[i]; ++amount)
                {
                    GameObject temp = Instantiate(itemPrefab[itemName.ToString()]);
                    temp.transform.position = positionList[i];
                    temp.transform.parent = dropItems.transform;
                    if (amountList[i] > 1)
                        temp.tag = "Items";
                    else if (amountList[i] == 1)
                        temp.tag = "Item";
                }
            }
            ++i;
        }
    }

    static private GameObject createDropItemsParent(Vector3 position)
    {
        GameObject dropItems = new GameObject("Drop");
        dropItems.transform.position = position;
        return dropItems;
    }
}
