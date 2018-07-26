using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : BasicSwitch {
    //때리면 작동 -> 메세지 전달의 단순한 작업만 실행

    bool strong_hit;

    void Start()
    {
        strong_hit = false;
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
            if (other.GetComponent<Element>().type == Element.Type.Light) strong_hit = true;
            else strong_hit = false;

            Destroy(other.gameObject);
            set_switch(true);
            Debug.Log(other.GetComponent<Element>().type);
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

    public bool get_strong_hit()
    {
        return strong_hit;
    }
}
