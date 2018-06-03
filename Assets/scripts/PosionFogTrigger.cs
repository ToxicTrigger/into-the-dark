using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosionFogTrigger : MonoBehaviour {
    public float Hp = 3;
    public bool on;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Arrow") )
        {
            Hp -= other.gameObject.GetComponent<Arrow>().power;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Sword"))
        {
            Hp -= 1;
        }
    }

    private void Update()
    {
        if(Hp <= 0)
        {
            on = true;
            gameObject.SetActive(false);
        }
        else
        {
            on = false;
        }
    }
}
