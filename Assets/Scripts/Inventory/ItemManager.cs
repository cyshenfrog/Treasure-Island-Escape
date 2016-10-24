using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

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

    static private GameObject createDropItemsParent(Vector3 position)
    {
        GameObject dropItems = new GameObject("Drop");
        dropItems.transform.position = position;
        return dropItems;
    }
}
