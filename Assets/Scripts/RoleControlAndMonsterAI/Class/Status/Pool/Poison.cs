using UnityEngine;
using System.Collections;

public class Poison : MonoBehaviour {


    public Status.Objects Objects = Status.Objects.Player;
    public float Timer = 10;

    private Role r;

	void Start () {
        /*bool checkStat = Status.CheckStat(Status.StatusPool.Poison);
        if (checkStat) {
            Destroy(this);
        }
        else {
            Status.Use(Status.StatusPool.Poison);

            if(Objects == Status.Objects.Player) StartCoroutine(poison(r));
        }*/
	}
	
    public void posion(string monsterId) {

    }

    void OnDestroy() {
        Debug.Log("destroy");
    }

    void Update() {
        if (Timer < 5) Destroy(this);

        Timer -= Time.deltaTime;
    }

    /*IEnumerator poison(Role r) {
        while (Timer > 0) {
            r.Hp -= (int)(r.MaxHp * 0.02f);
            yield return new WaitForSeconds(1);
            Timer--;
        }
        Status.Remove(Status.StatusPool.Poison);
        Destroy(this);
    }*/

    

}
