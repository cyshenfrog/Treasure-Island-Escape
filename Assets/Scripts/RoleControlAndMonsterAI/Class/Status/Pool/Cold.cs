using UnityEngine;
using System.Collections;

public class Cold : MonoBehaviour {

    private int timer = 10;

    private Role role;
    private Monster monster;

    private string id;

    private bool isRole = true;

    public void SetStatus(string monsterId)
    {
        id = monsterId;
        isRole = false;
    }

    public void SetStatus(Carceer c)
    {
        id = c.ToString();
        role = Role.GetRoleData(c);       
        StartCoroutine(cold());
    }

    public void SetStatus(Carceer c, int time)
    {
        timer = time;
        id = c.ToString();
        role = Role.GetRoleData(c);
        StartCoroutine(cold());
    }

    void OnDestroy() {
        Status.Remove(id, Status.StatusPool.Cold);
    }

    IEnumerator cold() {

        int run = 0;
        Status.Add(id, Status.StatusPool.Cold, timer);

        //移動速度減少??
        if (isRole) {
            //role.MoveSpeed *= ??
        }
        else {
        }

        while (timer != run)
        {
            if (isRole) {
                role.Hungry -= (int)(role.MaxHungry * 0.02f);
            }
            else {
                //monster?
            }
            
            yield return new WaitForSeconds(1);
            run++;
        }
        Status.Remove(id, Status.StatusPool.Cold);
        Destroy(this);
    }

}
