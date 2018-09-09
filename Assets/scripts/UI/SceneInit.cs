using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneInit : MonoBehaviour {


    public void init_scene()
    {
        SceneManager.UnloadSceneAsync(0);
        SceneManager.LoadSceneAsync(0);
    }
}
