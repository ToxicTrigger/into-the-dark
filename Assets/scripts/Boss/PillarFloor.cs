﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFloor : MonoBehaviour {
    //기둥의 한 층! 
    //데미지를 입을 때 마다 흔들린다면? 

    [Tooltip("기둥 체력")]
    public int hp;
    [Tooltip("기둥의 힘 (플레이어에게 몇의 데미지를 줄 것인가?)")]
    public int power;
    bool crack;        //균열이 있는지?
    bool crumbling; //무너지는 중?
    Vector3 target_position;
    float speed;

    float origin_dis;

	void Update () {
        if (crumbling)
        {
            transform.position += (target_position - transform.position).normalized * speed * Time.deltaTime;
            //회전 추가

            if(move_complete())
            {
                crumbling = false;
            }
        }
	}

    public void set_state(Vector3 _pos, bool _crack, float _speed)
    {
        speed = _speed;
        target_position = _pos;
        crack = _crack;

        crumbling = true;

        origin_dis = Vector3.Distance(target_position, transform.position);
    }

    bool move_complete()
    {
        float cur_dis = Vector3.Distance(target_position, transform.position);
        if ((origin_dis - cur_dis) / origin_dis > 0.9)
        {
            return true;
        }
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (crack)
        {
            if (other.CompareTag("Sword"))
            {
                hp -= 1;

                if (hp <= 0)
                    Destroy(gameObject);
            }
        }

        if (crumbling) { 
}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (crack)
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
        //무너지는 도중 플레이어와 충돌이 일어나면?
        if(crumbling)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //플레이어에게 데미지 입힘!
            }
        }
    }



}
