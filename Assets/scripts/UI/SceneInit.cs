using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneInit : MonoBehaviour
{
    public GameObject player, savepos;
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void init_scene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
