using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    public Damageable Player;
    public float Damage;
    public bool Players;
    public string boss_name;
    public Transform boss;
    public GameObject prefab;
    public GameObject Monster;

    public bool is_item;
    public void Start()
    {
        Player = FindObjectOfType<Player>().GetComponent<Damageable>();
        if(!is_item)
            boss = GameObject.Find(boss_name).transform;
    }

    public void OnCollisionEnter(Collision other)
    {
        if( other.gameObject.CompareTag("Enemy") )
        {
            if( Players )
            {
                other.gameObject.GetComponent<Damageable>().Hp -= 20;
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (other.CompareTag("Switch"))
            {
                Vector3 pos = boss.position;
                pos.y += 30f;
                GameObject drop = Instantiate(prefab, pos, Quaternion.identity, null);
                drop.GetComponent<Rigidbody>().useGravity = true;
                drop.GetComponent<Poison>().Players = true;
                drop.GetComponent<Collider>().isTrigger = false;
                Destroy(gameObject);
            }
            else
            {
                GameObject tmp = Instantiate(Monster, transform.position, Quaternion.identity, null);
                Destroy(gameObject);
            }
        }

        if(other.gameObject.name.Equals("Player"))
        {
            Player.Hp -= Damage;
            Player.GetComponent<Player>().ac.Shake(3 , 0.2f , Time.deltaTime);
        }
        if(!other.CompareTag("Enemy") && other.gameObject.name.Equals("Player") )
        {
            Destroy(gameObject);
        }
        if( other.CompareTag("Enemy") )
        {
            if(Players)
            {
                other.GetComponent<Damageable>().Hp -= 50;
                Destroy(gameObject);
            }
        }
    }
}
