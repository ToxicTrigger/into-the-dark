using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    public Animator fade;

    public string Scene_name;

    IEnumerator move(Collider other, float time)
    {
        fade.SetBool("fade", true);
        yield return new WaitForSeconds(time);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadSceneAsync(Scene_name);
        yield return new WaitForSeconds(time);
        fade.SetBool("fade", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player"))
        {
            StartCoroutine(move(other, 1.0f));
        }
    }
}
