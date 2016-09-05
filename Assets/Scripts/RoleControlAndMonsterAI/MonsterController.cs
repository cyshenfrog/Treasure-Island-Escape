using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

    public int id;
    public string RolePrefabName;
    public float Speed;
    public float Distance;

    private GameObject role;
    private Monster data;

    private MonsterState enemyState;

    int lastTimes = 0;
    int randomDirection;
    int randomAction;

    void Awake() {
        //data = Monster.Load(id);
        /*data.MaxHp = 1000;
        data.Hp = 1000;*/
    }

	void Start () {
        role = GameObject.Find(RolePrefabName);
        enemyState = MonsterState.IDlE;
	}
	
	void Update () {
        if (lastTimes == 0) {
            randomAction = Random.Range(0, 2);
            randomDirection = Random.Range(0, 4);
            lastTimes = Random.Range(10, 40);

            enemyState = (MonsterState)randomAction;
        }

        switch (enemyState) {
            case MonsterState.IDlE:
                Idle();
                break;
            case MonsterState.MOVE:
                Move();
                break;
            case MonsterState.NIGHT:
                break;
            case MonsterState.BEATTACK:
                break;
            default:
                break;
        }


    }

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


    void OnMouseDown() {
        
        if (role.GetComponent<RoleController>().Attack(data, transform.localPosition)) {
            //play animation
            if (data.Hp == 0) {
                //play animation
                //remove data in monster collection
                Debug.Log("monster die");
                Destroy(gameObject);
            }
        }

        
    }
}
