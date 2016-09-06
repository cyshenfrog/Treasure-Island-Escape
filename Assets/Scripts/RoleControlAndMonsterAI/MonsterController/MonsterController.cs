using UnityEngine;
using System.Collections;

public class MonsterController : MonoBehaviour {

    public int id;
    public float Speed;
    public float Distance;

    //basic
    protected GameObject role;
    protected Monster data;
    protected MonsterState State;

    //idle and move parameters
    protected int lastTimes = 0;
    protected int randomDirection;
    protected int randomAction;

    //be attacked parameters
    protected int beAttackedState = 0;
    protected const int STATE_STARTPLAYANIM = 0;
    protected const int STATE_ANIM = 1;
    protected const int STATE_FINISFANIM = 2;

    void Awake() {
        //data = Monster.Load(id);
        data = new Monster();
        data.MaxHp = 1000;
        data.Hp = 1000;
    }

	void Start () {
        role = GameObject.Find(AnimalConstant.RolePrefab);
        State = MonsterState.IDlE;
	}
	
	void Update () {
        if (lastTimes == 0 && (State == MonsterState.IDlE || State == MonsterState.MOVE)) {
            randomAction = Random.Range(0, 2);
            randomDirection = Random.Range(0, 4);
            lastTimes = Random.Range(10, 40);

            State = (MonsterState)randomAction;
        }

        switch (State) {
            case MonsterState.IDlE:
                Idle();
                break;
            case MonsterState.MOVE:
                Move();
                break;
            case MonsterState.NIGHT:
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
        Debug.Log("attack");
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
    
}
