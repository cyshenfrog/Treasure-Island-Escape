using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CookingFormulaList : MonoBehaviour {

    public static Dictionary<string, string> formula = new Dictionary<string, string>();

    [SerializeField]
    private GameObject[] targetButton;

    private TargetBar targetBar;
    private CookingSystem CookingWindow;
    void Start()
    {
        targetBar = GetComponent<TargetBar>();
        CookingWindow = GameObject.Find("CookingWindow").GetComponent<CookingSystem>();
        createAllFormula();
    }
    void createAllFormula()
    {
        string name;
        string[] split;
        GameObject temp;

        //create corresponding craft button for each object
        foreach (GameObject obj in targetButton)
        {
            temp = Instantiate(obj);
            name = temp.name;
            split = name.Split('_');
            createFormula(split[0], temp.GetComponentInChildren<Text>().text, temp.transform);
        }
    }
    void createFormula(string name, string fml, Transform button)
    {
        //load item to get tool tip
        Item item = (Resources.Load("Inventory/Items/" + name, typeof(GameObject)) as GameObject).GetComponent<Item>();

        //initialize slot and push item into slot
        TargetBarSlot slot = button.GetComponent<TargetBarSlot>();
        slot.initialize();
        slot.addItem(item);

        //set button callback
        button.GetComponent<Button>().onClick.AddListener(delegate { CookingWindow.ShowCookingFormula(name, item.toolTipContent()); });

        //create craft formula
        formula.Add(name, fml);

        //add button to target bar
        targetBar.AddElement(button, true);
    }
}
