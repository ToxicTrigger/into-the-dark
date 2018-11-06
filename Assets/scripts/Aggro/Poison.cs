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
    public GameObject effect;
    public GameObject sound;

    public bool is_item;
    public void Start()
    {
        Player = FindObjectOfType<Player>().GetComponent<Damageable>();
        if(!is_item)
            boss = GameObject.Find(boss_name).transform;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && !other.gameObject.name.Contains("pot"))
        {
            if (other.CompareTag("Switch"))
            {
                if(FindObjectsOfType<Bomber>().Length < 10)
                {
                    var t = Instantiate(sound, transform.position, Quaternion.identity, null);
                    Destroy(t, 3.0f);

                    Vector3 pos = boss.position;
                    pos.y += 30f;
                    GameObject drop = Instantiate(prefab, pos, Quaternion.identity, null);
                    drop.transform.localScale *= 1.4f;
                    GameObject tmp = Instantiate(effect, transform.position, Quaternion.identity, null);
                    Destroy(tmp, 3.0f);
                    Destroy(gameObject);
                }
            }
            else
            {
                var tt = Instantiate(sound, transform.position, Quaternion.identity, null);
                Destroy(tt, 3.0f);
                if(FindObjectsOfType<AggroAI>().Length < 10)
                {
                    if (Monster != null)
                    {
                        GameObject tmp = Instantiate(Monster, transform.position, Quaternion.identity, null);
                    }
                    GameObject t = Instantiate(effect, transform.position, Quaternion.identity, null);
                    Destroy(t, 3.0f);
                    Destroy(gameObject);
                }
                Player.GetComponent<Player>().ac.Shake(3, 0.2f, Time.deltaTime);
            }
        }

        if(other.gameObject.name.Equals("Player"))
        {
            var t = Instantiate(sound, transform.position, Quaternion.identity, null);
            Destroy(t, 3.0f);
            Player.Hp -= Damage;
            Player.GetComponent<Player>().ac.Shake(3 , 0.2f , Time.deltaTime);
            GameObject tmp = Instantiate(effect, transform.position, Quaternion.identity, null);
            Destroy(tmp, 3.0f);
        }
        if(!other.CompareTag("Enemy") && other.gameObject.name.Equals("Player") )
        {
            Destroy(gameObject);
        }
        //if( other.CompareTag("Enemy") )
        //{
        //    if(Players)
        //    {
        //        Player.GetComponent<Player>().ac.Shake(3, 0.2f, Time.deltaTime);
        //        other.GetComponent<Damageable>().Damaged(10,1,other.transform);
        //        GameObject tmp = Instantiate(effect, transform.position, Quaternion.identity, null);
        //        Destroy(tmp, 3.0f);
        //        Destroy(gameObject);
        //    }
        //}
    }
}
