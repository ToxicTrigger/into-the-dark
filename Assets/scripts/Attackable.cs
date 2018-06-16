using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//PosionFOg 덜어내야 함
public class Attackable : MonoBehaviour {
    public float attackTick = 0.3f;
    public float Damage = 10;
    public bool has_out = true;
    public Damageable player;
    float tick = 0;
    public PosionFogTrigger trigger;

    private void Awake()
    {
        player = FindObjectOfType<Player>().GetComponent<Damageable>();
        tick = attackTick;
    }

    private void Update()
    {
        if(trigger != null)
        {
            if (!trigger.on)
            {
                if (!has_out)
                {
                    if (tick <= attackTick)
                    {
                        tick += Time.deltaTime;
                    }
                    else
                    {
                        player.Damaged(Damage, attackTick);
                        tick = 0;
                    }
                }
                else
                {
                    tick = 0;
                }
            }
            else
            {
                GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Destroy(gameObject, 10);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") & !gameObject.CompareTag("Enemy"))
        {
            Debug.Log("dd" + other.tag + " : " + gameObject.tag);
            if(!gameObject.CompareTag("TotemAggro"))
            {
                Damageable dam = other.GetComponent<Damageable>();
                dam.Hp -= Damage;
           }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") | other.CompareTag("Totem")) has_out = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") | other.CompareTag("Totem")) has_out = true;
    }
}
