using UnityEngine;
using System.Collections;

public class Bank : MonoBehaviour {
    private GameObject inventoryPrefab;
    private GameObject bankInventory;
    private GameObject canvas;
    private bool isClose;
    public bool IsClose
    {
        get { return isClose; }
    }
	// Use this for initialization
	void Start () {
        inventoryPrefab = Resources.Load("Inventory/BankInventory", typeof(GameObject)) as GameObject;
        canvas = GameObject.Find("Canvas");
        createBankInventory();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void createBankInventory()
    {
        bankInventory = Instantiate(inventoryPrefab);
        RectTransform rectTransform = bankInventory.GetComponent<RectTransform>();
        rectTransform.SetParent(canvas.transform, false);
        rectTransform.SetAsFirstSibling();
        bankInventory.SetActive(false);
        isClose = true;
    }
    public void open()
    {
        bankInventory.SetActive(true);
        isClose = false;
    }
    public void close()
    {
        //bankInventory.GetComponent<BankInventory>().putItemBack();
        bankInventory.SetActive(false);
        isClose = true;
    }
}
