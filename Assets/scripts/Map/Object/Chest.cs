using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Interactable
{
    public Animator ani;
    public ParticleSystem ps;
    public bool open;
    public GameObject Item;
    public float time, speed;

    IEnumerator Gen()
    {
        var t = Instantiate(Item, transform.position, Quaternion.identity, null);
        t.GetComponent<Item>().enabled = false;
        var pos = t.transform.position;
        
        float tick = 0;
        while(tick < time)
        {
            tick += Time.deltaTime;
            pos.y += Time.deltaTime * speed;
            t.transform.position = pos;
            yield return new WaitForFixedUpdate();
        }
        t.GetComponent<Item>().enabled = true;
        ps.Stop();
    }

    private new void Update()
    {
        if(!open)
        {
            base.Update();
            if (hasTalking)
            {
                StartCoroutine(Gen());
                ps.Play();
                ani.SetBool("open", true);
                open = true;
            }
        }

    }
}
