using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public string map_name;

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadSceneAsync(map_name);
        }
    }

}
