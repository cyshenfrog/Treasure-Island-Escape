using UnityEngine;
using System.Collections;

public class Poison : MonoBehaviour {

    private int timer = 10;    /// # preset durtion to 10 sec

    /// # the object 
    private Role role;
    private Monster monster;
  
    private string id;          /// # the id of object 
    private bool isRole = true; /// # chect the object is role

    /// # api that start a status to a monster
    public void SetStatus(string monsterId) {
        id = monsterId;
        isRole = false;
        //
        /// # Monster = Monster.GetMonsterData(id);
        //
        StartCoroutine(poison());
    }

    /// # api that start a status to the role
    public void SetStatus(Carceer c) {
        id = c.ToString();
        role = Role.GetRoleData(c);
        
        StartCoroutine(poison());
    }

    /// # start a status to the role with a custom time
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
        int run = 0; /// # record the duration of status 

        /// # record the status and the object
        Status.Add(id, Status.StatusPool.Poison, timer);

        while (timer != run) {

            if (isRole) {

                /// # reduce hp by seconds
                role.Hp -= (int)(role.MaxHp * 0.02f);
            }
            else {
                //怪物扣血////
            }
            yield return new WaitForSeconds(1);
            run++;
        }

        /// # status finish 
        Status.Remove(id, Status.StatusPool.Poison);
        Destroy(this);
    }

    

}
