using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MonsterController : MonoBehaviour {

    public int id;
    public float Speed;

    //basic
    public Monster Data { set; get; }
    public MonsterState State { set; get; }
    protected GameObject role; 
    
    //idle and move parameters
    protected int lastTimes = 0;
    protected int randomDirection;
    protected int randomAction;

    protected int facing = -1;

    //be attacked parameters
    protected int beAttackedState = 0;
    protected const int STATE_STARTPLAYANIM = 0;
    protected const int STATE_ANIM = 1;
    protected const int STATE_FINISFANIM = 2;

    protected virtual void Awake() {
        //data = Monster.Load(id);
        Data = new Monster();
        Data.MaxHp = 1000;
        Data.Hp = 1000;
        Data.AttackSpace = 1;
        Data.AttackRange = 5;
        Data.Attack = 100;

        role = GameObject.Find(AnimalConstant.RolePrefab);
    }

    protected virtual void Start() {
        State = MonsterState.IDlE;
        //GroundController.StaticCellWidth;
    }

    protected virtual void Update() {
        if (lastTimes <= 0 && (State == MonsterState.IDlE || State == MonsterState.MOVE)) {
            randomAction = Random.Range(0, 2);
            randomDirection = Random.Range(0, 4);
            lastTimes = Random.Range(10, 40);

            State = (MonsterState)randomAction;
        }

        stateMachine();
    }

    protected virtual void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject())
            return;       
    }

    protected void stateMachine() {
        switch (State) {
            case MonsterState.IDlE:
                Idle();
                break;
            case MonsterState.MOVE:
                Move();
                break;
            case MonsterState.NIGHT:
                Night();
                break;
            case MonsterState.BEATTACK:
                BeAttacked();
                break;
            case MonsterState.ATTACK:
                Attack();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// moving according to randomDirection and lastTimes
    /// </summary>
    public virtual void Move() {

        switch (randomDirection)
        {
            case AnimalConstant.Left:
                transform.position = new Vector3(transform.position.x - Speed * Time.deltaTime, transform.position.y, transform.position.z);
                //transform.Translate(Vector2.left * Speed * Time.deltaTime);
                break;
            case AnimalConstant.Right:
                transform.position = new Vector3(transform.position.x + Speed * Time.deltaTime, transform.position.y, transform.position.z);
                break;
            case AnimalConstant.Down:
                transform.position = new Vector3(transform.position.x, transform.position.y - Speed * Time.deltaTime, transform.position.z);
                break;
            case AnimalConstant.Up:
                transform.position = new Vector3(transform.position.x, transform.position.y + Speed * Time.deltaTime, transform.position.z);
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
                transform.position = new Vector3(transform.position.x - Speed * Time.deltaTime * AnimalConstant.RunMagnification, transform.position.y, transform.position.z);
                break;
            case AnimalConstant.Right:
                transform.position = new Vector3(transform.position.x + Speed * Time.deltaTime * AnimalConstant.RunMagnification, transform.position.y, transform.position.z);
                break;
            case AnimalConstant.Down:
                transform.position = new Vector3(transform.position.x, transform.position.y - Speed * Time.deltaTime * AnimalConstant.RunMagnification, transform.position.z);
                break;
            case AnimalConstant.Up:
                transform.position = new Vector3(transform.position.x, transform.position.y + Speed * Time.deltaTime * AnimalConstant.RunMagnification, transform.position.z);
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
        role.GetComponent<RoleController>().BeAttacked((int)Data.Attack, transform.position);
    }

    public virtual void EnterBeAttack() {
        Debug.Log("enter be attack state");
    }

    public virtual void BeAttacked() {
        switch (beAttackedState) {
            case STATE_STARTPLAYANIM:
                //playanimation
                beAttackedState = STATE_ANIM;
                break;
            case STATE_ANIM:
                //callback 
                beAttackedState = STATE_FINISFANIM;
                break;
            default:
                break;
        }
    }

    public virtual void Night() { }

    public virtual void Die() {
        //play animation
        //remove data in monster collection
        Debug.Log("monster die");
        Destroy(gameObject);
    }

    
}
