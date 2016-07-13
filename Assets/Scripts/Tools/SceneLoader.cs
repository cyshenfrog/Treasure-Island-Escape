using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader {
    public static void Load(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
