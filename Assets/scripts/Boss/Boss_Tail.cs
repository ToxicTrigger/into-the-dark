using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Tail : MonoBehaviour {
    //보스 꼬리 _ 애니메이션(?) 부분만 담당하는 스크립트임

    bool boss_head;
    Quaternion parent_tail_rot; //부모 tail의 Rotation정보를 받아올 변수
    Quaternion this_tail_rot;     //자신 tail의 Rotation정보를 저장할 변수 (목표값)

    public float rot_speed;


    //꼬리의 움직임을 업데이트함
    public void move_update(Vector3 _move_dir)
    {
        if (transform.parent.GetComponent<Boss_Tail>() == null) boss_head = true;
        else
        {
            boss_head = false;
            parent_tail_rot = transform.parent.rotation; //부모의 회전 정보를 받아옴
        }


        //현재 내가 머리라면
        if (boss_head)
        {
            //움직임이 있을때만 회전한다.
            if (_move_dir != Vector3.zero)
            {
                this_tail_rot = Quaternion.identity;
                float y_angle = Mathf.Atan2(_move_dir.x, _move_dir.z) * Mathf.Rad2Deg;
                float x_angle = 0;
                if (_move_dir.y != 0)
                    x_angle = Mathf.Atan2(-_move_dir.y, _move_dir.z) * Mathf.Rad2Deg;
                
                this_tail_rot.eulerAngles = new Vector3 (x_angle, y_angle, 0);
                Debug.Log("this_tail_rot = " + this_tail_rot);

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this_tail_rot, rot_speed * Time.deltaTime);
            }
        }
        else //머리가 아니라면 앞 꼬리의 정보를 받아와 계산한다.
        {
            //부모가 회전한 만큼을 빼줘 원상복구 시켜준 후 다시 부모의 회전값을 기준으로 계산한다.
            //this.transform.rotation = Quaternion.Euler(-parent_tail_rot.x,-parent_tail_rot.y, -parent_tail_rot.z);
            
        }

    }

}