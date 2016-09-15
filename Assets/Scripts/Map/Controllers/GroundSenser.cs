using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class GroundSenser : MonoBehaviour
{
    //in the GroundSenser

	void Start ()
    {
        GetComponent<BoxCollider>().size = GroundController.StaticSightWidth * Vector3.right + GroundController.StaticSightHeight * Vector3.up;
	}

    void OnTriggerEnter(Collider tile)
    {
        
    }

    void OnTriggerExit(Collider tile)
    {

    }
}
