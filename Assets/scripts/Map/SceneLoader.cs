using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string map_name;
    public Animator ani;

    IEnumerator ChangeScene()
    {
        ani.SetBool("load" , true);
        yield return new WaitForSeconds(4);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync(map_name);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChangeScene());
        }
    }

}
