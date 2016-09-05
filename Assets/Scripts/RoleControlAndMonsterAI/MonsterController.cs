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

    private int lastTimes = 0;
    private int randomDirection;
    private int randomAction;

    //be attack
    private Vector2 rolePosition;
    private bool cacheRolePosition = false;
    private int beAttackState = 0;
    private const int STATE_STARTPLAYANIM = 0;
    private const int STATE_ANIM = 1;
    private const int STATE_RUNAWAY = 2;
    private int roleDirectionX;
    private int roleDirectionY;

    void Awake() {
        //data = Monster.Load(id);
        data = new Monster();
        data.MaxHp = 1000;
        data.Hp = 1000;
    }

	void Start () {
        role = GameObject.Find(RolePrefabName);
        enemyState = MonsterState.IDlE;
	}
	
	void Update () {
        if (lastTimes == 0 && (enemyState == MonsterState.IDlE || enemyState == MonsterState.MOVE)) {
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
                BeAttack();
                break;
            default:
                break;
        }


    }

    public virtual void Move() {
        
        switch (randomDirection)
        {
            case AnimalConstant.Left:
                transform.localPosition = new Vector3(transform.localPosition.x - Speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
                break;
            case AnimalConstant.Right:
                transform.localPosition = new Vector3(transform.localPosition.x + Speed * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);
                break;
            case AnimalConstant.Down:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - Speed * Time.deltaTime, transform.localPosition.z);
                break;
            case AnimalConstant.Up:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Speed * Time.deltaTime, transform.localPosition.z);
                break;
            default:
                break;
        }
        lastTimes--;
    }

    public virtual void Run()
    {

        switch (randomDirection)
        {
            case AnimalConstant.Left:
                transform.localPosition = new Vector3(transform.localPosition.x - Speed * Time.deltaTime * AnimalConstant.RunMagnification, transform.localPosition.y, transform.localPosition.z);
                break;
            case AnimalConstant.Right:
                transform.localPosition = new Vector3(transform.localPosition.x + Speed * Time.deltaTime * AnimalConstant.RunMagnification, transform.localPosition.y, transform.localPosition.z);
                break;
            case AnimalConstant.Down:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - Speed * Time.deltaTime * AnimalConstant.RunMagnification, transform.localPosition.z);
                break;
            case AnimalConstant.Up:
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Speed * Time.deltaTime * AnimalConstant.RunMagnification, transform.localPosition.z);
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

    public virtual void BeAttack() {
        
        switch (beAttackState) {
            case STATE_STARTPLAYANIM:
                //playanimation
                beAttackState = STATE_ANIM;
                break;
            case STATE_ANIM:
                //callback 
                beAttackState = STATE_RUNAWAY;
                break;
            case STATE_RUNAWAY:
                runAway();
                break;
            default:
                break;
        }

    }

    private void runAway() {
        
        //check if trigger run action
        if (!cacheRolePosition) {
            //if not , record role direction data
            rolePosition = role.transform.localPosition;
            cacheRolePosition = true;
            float deltaX = rolePosition.x - transform.localPosition.x;
            float deltaY = rolePosition.y - transform.localPosition.y;

            roleDirectionX = deltaX == 0 ? AnimalConstant.Origin : deltaX > 0 ? AnimalConstant.Right : AnimalConstant.Left;
            roleDirectionY = deltaY == 0 ? AnimalConstant.Origin : deltaY > 0 ? AnimalConstant.Up : AnimalConstant.Down;
        }

        float distance = Vector2.Distance(rolePosition, transform.localPosition);

        if (distance < AnimalConstant.RunAwayDistance) {
            
            if (lastTimes == 0) {
                lastTimes = Random.Range(10, 40);
                
                do
                {
                    //random run direction (excludes role direction)
                    randomDirection = Random.Range(0, 4);
                } while (randomDirection == roleDirectionX || randomDirection == roleDirectionY);
            }
            Run();
        }
        else {
            //finish run state
            cacheRolePosition = false;
            enemyState = MonsterState.IDlE;
            beAttackState = STATE_STARTPLAYANIM;
        }


    }


    void OnMouseDown() {
        
        if (role.GetComponent<RoleController>().Attack(data, transform.localPosition)) {

            enemyState = MonsterState.BEATTACK;

            lastTimes = 0;
            beAttackState = STATE_STARTPLAYANIM;
            cacheRolePosition = false;

            if (data.Hp == 0) {
                //play animation
                //remove data in monster collection
                Debug.Log("monster die");
                Destroy(gameObject);
            }
        }

        
    }
}
