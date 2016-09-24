using UnityEngine;
using System.Collections;

public class Test2 : MonoBehaviour
{
    public void Func()
    {
        StartCoroutine("Log");
    }

    IEnumerator Log()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("YOOOOO");
        yield break;
    }
}
