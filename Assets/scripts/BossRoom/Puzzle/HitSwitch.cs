using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : BasicSwitch {
    //때리면 작동 -> 메세지 전달의 단순한 작업만 실행
    

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sword"))
        {
            Destroy(other.gameObject);
            set_switch(true);
        }
        if (other.CompareTag("Arrow"))
        {
            Destroy(other.gameObject);
            set_switch(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Arrow"))
        {
            Destroy(collision.gameObject);
            set_switch(true);
        }
    }
    
}
