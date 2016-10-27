using UnityEngine;
using System.Collections;

public class MakingPillar : MonoBehaviour {

    public GameObject makingWindow;
    public GameObject makingWindowTargetBar;
    public GameObject cookingWindow;
    public GameObject cookingWindowTargetBar;
    // Use this for initialization
    void Start () {
	    if(makingWindow == null)
        {
            makingWindow = GameObject.Find("MakingWindow");
        }
        if(makingWindowTargetBar == null)
        {
            makingWindowTargetBar = GameObject.Find("MakingWindowTargetBar");
        }
        if(cookingWindow == null)
        {
            cookingWindow = GameObject.Find("CookingWindow");
        }
        if(cookingWindowTargetBar == null)
        {
            cookingWindowTargetBar = GameObject.Find("CookingWindowTargetBar");
        }

        hideAllWindow();

	}
	
	void hideAllWindow()
    {
        makingWindow.SetActive(false);
        makingWindowTargetBar.SetActive(false);
        cookingWindow.SetActive(false);
        cookingWindowTargetBar.SetActive(false);
    }

    public void toggleMakingWindow()
    {
        makingWindow.SetActive(!makingWindow.activeSelf);
        makingWindowTargetBar.SetActive(!makingWindowTargetBar.activeSelf);
    }

    public void toggleCookingWindow()
    {
        cookingWindow.SetActive(!cookingWindow.activeSelf);
        cookingWindowTargetBar.SetActive(!cookingWindowTargetBar.activeSelf);
    }
}
