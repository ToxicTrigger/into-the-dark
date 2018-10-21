using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneInit : MonoBehaviour
{
    public Animator ani;
    IEnumerator init_scene()
    {
        ani.SetBool("fade" , true);
        yield return new WaitForSeconds(3 / 2);
        FindObjectOfType<Player>().transform.position = FindObjectOfType<PlayerMove>().spawn_point.position;
        yield return new WaitForSeconds(2);
        ani.SetBool("fade" , false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine( init_scene());
        }
    }

    private void Start()
    {
        ani = FindObjectOfType<Player>().gameObject.transform.parent.GetComponent<Animator>();
    }
}
