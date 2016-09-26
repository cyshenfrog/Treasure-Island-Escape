using UnityEngine;
using System.Collections;

public class Hot : MonoBehaviour {

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
        StartCoroutine(hot());
    }

    public void SetStatus(Carceer c, int time)
    {
        timer = time;
        id = c.ToString();
        role = Role.GetRoleData(c);
        
        StartCoroutine(hot());
    }

    void OnDestroy()
    {
        //Status.Remove(id, Status.StatusPool.Cold);
    }

    IEnumerator hot()
    {
        int run = 0;
        //Status.Add(id, Status.StatusPool.Hot, timer);

        //移動速度減少??
        if (isRole)
        {
            //role.MoveSpeed *= ??
        }
        else
        {
        }

        while (timer != run)
        {
            if (isRole)
            {
                role.Hp -= (int)(role.MaxHp * 0.02f);
            }
            else
            {
                //monster?
            }

            yield return new WaitForSeconds(1);
            run++;
        }
        //Status.Remove(id, Status.StatusPool.Hot);
        Destroy(this);
    }
}
