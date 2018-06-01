using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Tail : MonoBehaviour {
    //보스 꼬리 _ 애니메이션(?) 부분만 담당하는 스크립트임

    public bool boss_head;
    Quaternion parent_tail_rot; //부모 tail의 Rotation정보를 받아올 변수
    Quaternion this_tail_rot_temp;    //보관용
    Quaternion this_tail_rot;     //자신 tail의 Rotation정보를 저장할 변수 (목표값)

    Transform parent_tr;
    Vector3 target_pos;
    float bone_distance;

    Boss_Tail head_tail;

    public float rot_speed_min;    //꼬리의 회전지연
    public float rot_speed;
    public float rot_delay;

    public float speed;
    public float delay;

    public bool move_on;

    private void Start()
    {
        parent_tail_rot = Quaternion.identity;
        this_tail_rot_temp = this.transform.rotation;
        head_tail = head_seach(this.transform);
        bone_distance = Vector3.Distance(transform.parent.position, transform.position);
        parent_tr = transform.parent.transform;
    }

    Boss_Tail head_seach(Transform tail)
    {
        if(tail.parent.GetComponent<Boss_Tail>() == null)
        {
            return tail.GetComponent<Boss_Tail>();
        }
        else
        {
            return head_seach(tail.parent);
        }
    }

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

                //Quaternion this_tail_rot_x = Quaternion.identity;
                //float y_angle = Mathf.Atan2(_move_dir.x, _move_dir.z) * Mathf.Rad2Deg;
                //float x_angle = 0;

                //if (_move_dir.y != 0)
                //{
                //    x_angle = Mathf.Atan2(-_move_dir.y, _move_dir.z) * Mathf.Rad2Deg;
                //}

                //this_tail_rot.eulerAngles = new Vector3(x_angle, y_angle, 0);

                this_tail_rot.SetLookRotation(_move_dir);   //ㅅㅂ

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this_tail_rot , rot_speed * Time.deltaTime);
                move_on = true;
            }
            else move_on = false;
        }
        else //머리가 아니라면 앞 꼬리의 정보를 받아와 계산한다.
        {
            if (head_tail.move_on)
            {
                this.transform.rotation = this_tail_rot_temp;
                speed = rot_speed;
            }
            else
            {
                speed = rot_speed;
            }


            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, parent_tail_rot, speed * Time.deltaTime);

            this_tail_rot_temp = this.transform.rotation;


            // //부모의 바로 뒤, 이동 방향의 반대에 위치한 곳으로 하지만 이동 방향을 바라보는 

            ////target_pos = (transform.parent.position) * -bone_distance ;
            ////target_pos = (parent_tr.position - target_pos).normalized;
            // target_pos = (-parent_tr.forward * bone_distance);
            // //target_pos = new Vector3(parent_tr.position.x, parent_tr.position.y, parent_tr.position.z );

            // Vector3 originAxis = transform.TransformDirection(transform.forward);
            // Quaternion targetDir = Quaternion.FromToRotation(originAxis , target_pos);

            // transform.rotation *= targetDir;

            // this_tail_rot_temp = this.transform.rotation;

        }
    }        

}