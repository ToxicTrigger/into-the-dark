using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptWall : MonoBehaviour {
    //보스룸 플레이를 방해하는 벽 _ 공격에 부숴진다.

    public float hp = 3;

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sword"))
        {
            hp -= 1;

            if (hp <= 0)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (collision.gameObject.GetComponent<Element>().type == Element.Type.Light)
            {
                hp -= 3;
            }
            else
                hp -= 1;

            if (hp <= 0)
                Destroy(gameObject);

            Debug.Log("화살 속성 = " + collision.gameObject.GetComponent<Element>().type);
            Destroy(collision.gameObject);
        }
    }

}
