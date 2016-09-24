using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

    public string Role;
    public float Speed;
    public float Distance;
    private GameObject RolePrefab;


    void Awake() {

    }

	void Start () {
        RolePrefab = GameObject.Find(Role);
	}
	
	void Update () {
        if (Vector3.Distance(RolePrefab.transform.localPosition, transform.localPosition) <= Distance) {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, RolePrefab.transform.localPosition, Speed * Time.deltaTime);
        }
        else {
            Move();
        }
	}

    int moveTimes = 0;
    int randomDirection;

    public virtual void Move() {
        if (moveTimes == 0) {
            randomDirection = Random.Range(0, 4);
            moveTimes = Random.Range(10, 40);
        }
        switch (randomDirection)
        {
            case 0:
                transform.localPosition = new Vector3(transform.localPosition.x - Speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
                break;
            case 1:
                transform.localPosition = new Vector3(transform.localPosition.x + Speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
                break;
            case 2:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - Speed * Time.deltaTime, transform.localPosition.z);
                break;
            case 3:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Speed * Time.deltaTime, transform.localPosition.z);
                break;
            default:
                break;
        }
        moveTimes--;
    }

    public virtual void Attack() {

    }

    public virtual void Skill() {

    }
}
