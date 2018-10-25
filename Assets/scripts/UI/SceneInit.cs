using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneInit : MonoBehaviour
{
    public Animator ani;
    IEnumerator init_scene()
    {
        ani.SetBool("fade", true);
        yield return new WaitForSeconds(1);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(3);
        ani.SetBool("fade", false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Equals("Player"))
        {
            StartCoroutine( init_scene());
        }
    }

    private void Awake()
    {
        ani = FindObjectOfType<Player>().gameObject.transform.parent.GetComponent<Animator>();
    }
}
