using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutoCutScene : MonoBehaviour
{
    public bool has_played;
    public Animation dragon;
    public PlayerMove pm;
    public Animator player;
    public ActionCamera ac;

    IEnumerator shake()
    {
        pm.enabled = false;
        ac.Shake(100, 0.2f, Time.deltaTime);
        player.SetBool("stun", true);
        yield return new WaitForSeconds(2.2f);

        dragon["new"].speed = 1;
        dragon.Play("new");
        ac.Shake(120, 0.5f, Time.deltaTime);
        yield return new WaitForSeconds(2.8f);
        dragon["new"].speed = -1;
        //dragon.Play("new");

        ac.Shake(50, 0.15f, 0.05f);


        yield return new WaitForSeconds(2.0f);
        player.SetBool("stun", false);
        pm.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!has_played)
        {
            if (other.gameObject.name.Equals("Player"))
            {
                has_played = true;
                StartCoroutine(shake());
      
            }
        }
    }
}
