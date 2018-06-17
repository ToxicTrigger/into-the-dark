using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFloor : MonoBehaviour {
    //기둥의 한 층! 
    //데미지를 입을 때 마다 흔들린다면? 

    [Tooltip("기둥 체력")]
    public int hp;
    [Tooltip("기둥의 힘 (플레이어에게 몇의 데미지를 줄 것인가?)")]
    public int power;

    public GameObject piece;

    bool crack;        //균열이 있는지?
    bool crumbling; //무너지는 중?
    Vector3 target_position;
    float speed;
    float rot_speed;

    float origin_dis;
    Quaternion target_rot = Quaternion.identity;

    public AudioSource [] drop_sound;

    void Update () {
        if (crumbling)
        {
            Vector3 dir = (target_position - transform.position).normalized;

            transform.position += dir * speed * Time.deltaTime;
            //회전 추가
            dir.y = 0;
            target_rot.SetFromToRotation(Vector3.up, dir);
            transform.rotation = Quaternion.Slerp(transform.rotation,target_rot, rot_speed * Time.deltaTime);

            if(move_complete())
            {
                drop_sound[Random.Range(0,2)].Play();
                   //EventManager.get_instance().camera_shake(power);
                   crumbling = false;
            }
        }
	}

    public void set_state(Vector3 _pos, bool _crack, float _speed, float _rot_speed)
    {
        speed = _speed;
        rot_speed = _rot_speed;
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
                {
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject c_piece = (GameObject)Instantiate(piece, transform.position, Quaternion.identity);
                    }

                    Destroy(gameObject);
                }
            }
        }

        if (crumbling)
        {
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
