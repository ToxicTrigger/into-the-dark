using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Worm : MonoBehaviour {
    //이동과 기능(hp,공격등의)을 담당 

    public Boss_Tail[] tail;
    


    public float speed;
    Vector3 move_dir;
    Vector3 tail_dir;

    private void Update()
    {
            move_control();
    }

    //테스트를 위해 키 입력을 받아 움직임 조정
    void move_control()
    {
        if(Input.GetKey(KeyCode.A))
        {
            move_dir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.W))
        {
            move_dir += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move_dir += Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move_dir += Vector3.back;
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            move_dir += Vector3.up;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            move_dir += Vector3.down;
        }

        transform.position += move_dir * speed * Time.deltaTime;
        tail_dir = move_dir;    //꼬리에 넘겨줄 움직이는 방향을 저장함.
        move_dir = Vector3.zero;

    }

    private void LateUpdate()
    {
        //꼬리 업데이트
        for(int i=0; i<tail.Length; i++)
        {
            tail[i].move_update(tail_dir);
        }
    }

}
