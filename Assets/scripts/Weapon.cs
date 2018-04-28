using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float Damage = 1;
    public Player player;
    public bool is_hited;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Enemy") & player.is_attack)
        {
            if(other != null)
            {
                Look look = other.gameObject.GetComponent<Look>();
                if (look.mind != Look.State.Dead)
                {
                    is_hited = true;
                    Debug.Log("Hit");
                    StartCoroutine(operHp(Damage, look));
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy") & player.is_attack)
        {
            if (other != null)
            {
                Look look = other.gameObject.GetComponent<Look>();
                if (look.mind != Look.State.Dead)
                {
                    is_hited = false;
                    Debug.Log("Hit Out");
                }
            }
        }
    }

    public void setHitOn()
    {
        player.is_attack = true;
    }

    public void setHitOff()
    {
        player.is_attack = false;
    }

    IEnumerator operHp(float Damage, Look target)
    {
        target.onAttack(Damage);
        target.na.enabled = false;
        yield return new WaitForSeconds(0.2f);
        target.na.enabled = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
