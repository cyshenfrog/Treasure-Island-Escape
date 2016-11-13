using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MonsterController : MonoBehaviour {

    public int id;

    //basic
    public Monster Data { set; get; }
    public MonsterState State { set; get; }
    protected GameObject role;
    protected Animator animator;

    //idle and move parameters
    protected int lastTimes = 0;
    protected Vector2 randomUnit;
    protected int randomAction;

    protected virtual void Awake() {
        //data = Monster.Load(id);
        Data = new Monster();
        Data.MaxHp = 1000;
        Data.Hp = 1000;
        Data.Speed = 0;
        Data.AttackSpace = 1;
        Data.AttackRange = 3;
        Data.Attack = 100;

        role = GameObject.Find(AnimalConstant.RolePrefab);
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    protected virtual void Start() {
        State = MonsterState.IDlE;
        //GroundController.StaticCellWidth;
    }

    protected virtual void Update() {
        if (lastTimes <= 0 && (State == MonsterState.IDlE || State == MonsterState.MOVE)) {
            randomAction = Random.Range(0, 2);
            randomUnit = Random.insideUnitCircle.normalized;
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
            case MonsterState.DEAD:
                Die();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// moving according to randomUnit
    /// </summary>
    public virtual void Move() {
        //TileDataManager.RegisterTileData();
        
        Vector2 direction = DirectionSwitcher.DirectionSwitch(randomUnit);
        transform.Translate(direction * Data.Speed * Time.deltaTime);
        lastTimes--;
    }

    /// <summary>
    /// Running according to randomUnit
    /// </summary>
    public virtual void Run() {
        Vector2 direction = DirectionSwitcher.DirectionSwitch(randomUnit);
        transform.Translate(direction * Data.Speed * Time.deltaTime * AnimalConstant.RunMagnification);
        lastTimes--;
    }

    public virtual void Idle() {
        animator.SetTrigger("Idle");
        lastTimes--;
    }

    public virtual void Attack() {
        role.GetComponent<RoleController>().BeAttacked((int)Data.Attack, transform.position);
    }

    public virtual void BeAttacked() {
        if (Data.Hp <= 0) State = MonsterState.DEAD;
    }

    public virtual void Night() { }

    public virtual void Die() {
        //play animation
        //remove data in monster collection
        Debug.Log("monster die");
        Destroy(gameObject);
    }

    
}
