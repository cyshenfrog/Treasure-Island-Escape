using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class setChildIndexOnClick : MonoBehaviour {

    private Button btn;
	// Use this for initialization
	void Start () {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener( () => setIndex() );
    }

    // Update is called once per frame
    void setIndex()
    {
        gameObject.transform.parent.SetAsFirstSibling();
    }
}
