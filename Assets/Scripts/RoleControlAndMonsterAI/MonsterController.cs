using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

    public string Role;
    public float Speed;
    public float Distance;


    private GameObject RolePrefab;
    private Monster data;

    public const int STATE_IDlE = 0;
    public const int STATE_MOVE = 1;
    public const int STATE_ATTACK = 2;
    public const int STATE_NIGHT = 3;

    private int enemyState;

    void Awake() {
        
    }

	void Start () {
        RolePrefab = GameObject.Find(Role);
        enemyState = STATE_IDlE;
	}
	
	void Update () {
        /*if (Vector3.Distance(RolePrefab.transform.localPosition, transform.localPosition) <= Distance) {
            //transform.localPosition = Vector3.MoveTowards(transform.localPosition, RolePrefab.transform.localPosition, Speed * Time.deltaTime);
            enemyState = STATE_MOVE;
            
        }
        else {
            Move();
        }*/

        if (lastTimes == 0) {
            randomAction = Random.Range(0, 2);
            randomDirection = Random.Range(0, 4);
            lastTimes = Random.Range(10, 40);

            enemyState = randomAction;
        }

        switch (enemyState) {
            case STATE_IDlE:
                Idle();
                break;
            case STATE_MOVE:
                Move();
                break;
            case STATE_NIGHT:
                break;
            default:
                break;
        }


    }

    int lastTimes = 0;
    int randomDirection;
    int randomAction;
    

    public virtual void Move() {
        
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
        lastTimes--;
    }

    public virtual void Idle() {

        switch (randomDirection) {
            case 0:
                //play animation
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
        lastTimes--;
    }

    public virtual void Attack() {

    }

    public virtual void Skill() {

    }
}
