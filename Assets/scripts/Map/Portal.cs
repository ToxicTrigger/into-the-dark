using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform des;
    public Animator fade;

    IEnumerator move(Collider other, float time)
    {
        fade.SetBool("fade" , true);
        yield return new WaitForSeconds(time);
        fade.SetBool("fade" , false);
        other.transform.position = des.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Equals("Player"))
        {
            StartCoroutine(move(other , 1.0f));
        }
    }
}
