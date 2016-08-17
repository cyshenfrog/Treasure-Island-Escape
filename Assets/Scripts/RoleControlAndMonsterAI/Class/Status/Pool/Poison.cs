using UnityEngine;
using System.Collections;

public class Poison : MonoBehaviour {

    private int timer = 10;

    private Role role;
    private Monster monster;

    private string id;

    private bool isRole = true;
	
    public void SetStatus(string monsterId) {
        id = monsterId;
        isRole = false;
        //Monster = Monster.GetMonsterData(id);
        StartCoroutine(poison());
    }

    public void SetStatus(Carceer c) {
        id = c.ToString();
        role = Role.GetRoleData(c);
        
        StartCoroutine(poison());
    }

    public void SetStatus(Carceer c, int time)
    {
        timer = time;
        id = c.ToString();
        role = Role.GetRoleData(c);
        StartCoroutine(poison());
    }

    void OnDestroy() {
        Status.Remove(id, Status.StatusPool.Poison);
    }

    IEnumerator poison() {
        int run = 0;
        Status.Add(id, Status.StatusPool.Poison, timer);

        while (timer != run) {

            if (isRole) {
                role.Hp -= (int)(role.MaxHp * 0.02f);
            }
            else {
                //怪物扣血////
            }
            yield return new WaitForSeconds(1);
            run++;
        }
        Status.Remove(id, Status.StatusPool.Poison);
        Destroy(this);
    }

    

}
