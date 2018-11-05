using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string map_name;
    public Animator ani;
    public bool use_cutscene;

    IEnumerator ChangeScene()
    {
        ani.SetBool("load", true);
        FindObjectOfType<Player>().gameObject.GetComponent<AudioSource>().Stop();
        if(use_cutscene)
        {
            FindObjectOfType<CutScenePlay>().play_scene();
            float t = 0;
            foreach (var i in FindObjectOfType<CutScenePlay>().scene_info.keep_time)
            {
                t += i;
            }

            yield return new WaitForSeconds(t + 20);
        }
        

        yield return new WaitForSeconds(8);
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
