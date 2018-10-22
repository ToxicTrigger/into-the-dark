using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormTail : MonoBehaviour {
    //꼬리
    //신축성(?)있음

    public float stretch_limit; //늘어나는 한도
    public float origin_dis;
    public bool max_stretch;    //끝까지 늘어졌는가
    public bool head;

    //바로 상위 꼬리/ 하위 꼬리/ 머리를 저장
    WormTail parent_tail;
    WormTail child_tail;
    public WormTail head_tail;

    //////////////////////////////

    Vector3 origin_pos;
    Vector3 ago_pos;
    Quaternion ago_rot;

    Vector3 move_dir;
    Vector3 this_forward;
    Vector3 rot_dir;
    public float speed;
    public float follow_speed;
    public float rot_speed;

	void Start () {

        ago_pos = transform.position;

        if (transform.parent.GetComponent<WormTail>() == null)
        {
            head = true;
        }
        else
        {
            head = false;
            parent_tail = transform.parent.GetComponent<WormTail>();
            origin_dis = Vector3.Distance(transform.position, parent_tail.transform.position);
        }
        if(transform.childCount > 0)
            child_tail = transform.GetChild(0).GetComponent<WormTail>();

        origin_pos = transform.localPosition;
        speed = 6;

        
    }
	
	void Update () {
		if(head)
        {
            if (Input.GetKey(KeyCode.A))
                move_dir += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                move_dir += Vector3.right;
            if (Input.GetKey(KeyCode.W))
                move_dir += Vector3.forward;
            if (Input.GetKey(KeyCode.S))
                move_dir += Vector3.back;
            if (!Input.anyKey)
            {
                move_dir = Vector3.zero;
            }
            else
            {
                rot_dir = move_dir;
            }

            move_dir = move_dir.normalized;

            if(move_dir != Vector3.zero)
            {
                //child_tail.set_stretch(true);
            }
            //child_tail.set_move_dir(move_dir);
            transform.position += move_dir * speed * Time.deltaTime;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rot_dir), rot_speed * Time.deltaTime);

        }
        else
        {
            stretch_tail();
        }
	}

    //꼬리 늘이기
    void stretch_tail()
    {

        if (Vector3.Distance(parent_tail.transform.position, transform.position) > stretch_limit)
        {
            set_stretch(true);        
        }
        else if (parent_tail.get_stretch() || parent_tail.head && !max_stretch && head_tail.get_move_dir() != Vector3.zero)
        {
            transform.position = ago_pos;
            ago_pos = transform.position;
        }
        else
        {
            ago_pos = transform.position;
        }

        transform.rotation = ago_rot;

        if (max_stretch)
        {
            if (head_tail.get_move_dir() == Vector3.zero && !parent_tail.get_stretch() && Vector3.Distance(parent_tail.transform.position, transform.position) != origin_dis)
            {
                move_dir = (origin_pos - transform.localPosition).normalized;
                transform.localPosition += move_dir * speed * Time.deltaTime;

                ago_rot = Quaternion.LookRotation(parent_tail.transform.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, ago_rot, rot_speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, parent_tail.transform.position) - origin_dis < 0.01f)
                {
                    transform.localPosition = origin_pos;
                    max_stretch = false;
                    move_dir = Vector3.zero;
                    ago_pos = origin_pos;
                    transform.rotation = ago_rot;
                }
            }
            else
            {
                if (Vector3.Distance((parent_tail.transform.position + (parent_tail.transform.TransformVector(Vector3.back).normalized * stretch_limit)), transform.position) > 0.1)
                {
                    move_dir = ((parent_tail.transform.position + (parent_tail.transform.TransformVector(Vector3.back).normalized * stretch_limit)) - transform.position).normalized;                    

                    transform.position += move_dir * follow_speed * Time.deltaTime;
                }
                else
                {
                    ago_rot = Quaternion.LookRotation(parent_tail.transform.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, ago_rot, rot_speed * Time.deltaTime);
                }
            }
        }

        ago_rot = transform.rotation;

        if (child_tail)
            child_tail.set_move_dir(move_dir);
    }

    public bool get_stretch()
    {
        return max_stretch;
    }

    public void set_stretch(bool _stretch)
    {
        max_stretch = _stretch;
    }

    public void set_move_dir(Vector3 _dir)
    {
        move_dir = _dir;
    }

    public Vector3 get_move_dir()
    {
        return move_dir;
    }


}
