using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMove : Observer
{
    //현재 올라오는 돌다리에만 사용하게끔 만들어져 있으므로 추후 수정해야함.

    int cnt;
    public int clear_cnt;
    public bool clear;

    public AudioSource move_sound;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum Rot_Pos
    {
        None = -1,
        Forward =0,
        Right,
        Back,
        Left
    }

    public Direction move_dir;
    public Rot_Pos rot_pos;
    public float move_speed;
    public float move_delay;
    public Vector3 move_target;     //이동완료 타겟에 대한건 좀 더 생각해볼것.
    bool move;
    bool rot_move;
    Vector3 dir;

    public Transform obj_center;
    public float obj_dis;   //중심으로 부터의 거리
    Vector3 target_pos;

    public Transform[] child_obj;

	void Start () {
        target_pos = Vector3.zero;

        rot_move = false;
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,
                                                  (int)rot_pos * 90,
                                                  transform.rotation.z));
        switch (rot_pos)
        {
            case Rot_Pos.Forward:
                target_pos = obj_center.position + (Vector3.forward*obj_dis);
                rot_pos = Rot_Pos.Right;
                break;
            case Rot_Pos.Right:
                target_pos = obj_center.position + (Vector3.right * obj_dis);
                rot_pos = Rot_Pos.Back;
                break;
            case Rot_Pos.Back:
                target_pos = obj_center.position + (Vector3.back * obj_dis);
                rot_pos = Rot_Pos.Left;
                break;
            case Rot_Pos.Left:
                target_pos = obj_center.position + (Vector3.left * obj_dis);
                rot_pos = Rot_Pos.Forward;
                break;
            default:
                break;
        }

        if (move_dir == Direction.Down)
            target_pos.y = -20;
        else if (move_dir == Direction.Up)
            target_pos.y = move_target.y;

        transform.position = target_pos;

    }

    private void Update()
    {
        if (move)
        {
            //for (int i = 0; i < child_obj.Length; i++)
            //{
            //    child_obj[i].position += dir * move_speed * Time.deltaTime;

            //    if (move_dir == Direction.Up)
            //    {
            //        if (child_obj[i].position.y >= move_target.y)
            //        {
            //            move_sound.Stop();
            //            child_obj[i].position = new Vector3(child_obj[i].position.x, move_target.y, child_obj[i].position.z);
            //            move = false;
            //        }
            //    }
            //    else if (move_dir == Direction.Down)
            //    {
            //        if (child_obj[i].position.y <= -20.0f)
            //        {
            //            move_sound.Stop();
            //            child_obj[i].position = new Vector3(child_obj[i].position.x, -20, child_obj[i].position.z);
            //            move = false;
            //        }
            //    }
            //}

            transform.position += dir * move_speed * Time.deltaTime;

            if (move_dir == Direction.Up)
            {
                if (transform.position.y >= move_target.y)
                {
                    move_sound.Stop();
                    transform.position = new Vector3(transform.position.x, move_target.y, transform.position.z);
                    move = false;
                }
            }
            else if (move_dir == Direction.Down)
            {
                if (transform.position.y <= -20.0f)
                {
                    move_sound.Stop();
                    transform.position = new Vector3(transform.position.x, -20, transform.position.z);
                    move = false;
                }
            }
        }
        else if (rot_move)
        {
            move_to_rot_pos();
        }

    }

    public override void notify(Observable obj)
    {
        HitSwitch obj_switch = obj as HitSwitch;
        if (obj_switch.get_switch())
        {         
            cnt++;
            if(cnt >= clear_cnt)
            {
                //StartCoroutine(timer(obj_switch.get_strong_hit()));
            }
        }
        else
        {            
            cnt--;
            //if(move_dir == Direction.Up)
                //StartCoroutine(timer(obj_switch.get_strong_hit()));
        }
    }

    float origin_dis;
    public IEnumerator timer(bool _strong_hit)
    {     

        switch (move_dir)
        {
            case Direction.Up:
                if(_strong_hit == true)
                {
                    rot_move = true;
                    target_pos = Vector3.zero;
                    switch (rot_pos)
                    {
                        case Rot_Pos.Forward:
                            target_pos = obj_center.position + (Vector3.forward * obj_dis);
                            break;
                        case Rot_Pos.Right:
                            target_pos = obj_center.position + (Vector3.right * obj_dis);
                            Debug.Log("center_pos = "+ obj_center.position + " + back_dis = " + (Vector3.back * obj_dis) + " = " + target_pos);
                            break;
                        case Rot_Pos.Back:
                            target_pos = obj_center.position + (Vector3.back * obj_dis);
                            break;
                        case Rot_Pos.Left:
                            target_pos = obj_center.position + (Vector3.left * obj_dis);
                            break;
                        default:
                            break;
                    }

                    target_pos.y = transform.position.y;

                    //transform.position = target_pos;
                    dir = (target_pos - transform.position).normalized;
                }
                else
                {
                    dir = Vector3.down;
                    move_dir = Direction.Down;
                }               
                break;
            case Direction.Down:

                dir = Vector3.up;
                move_dir = Direction.Up;

                break;
            case Direction.Left:
                dir = Vector3.left;
                break;
            case Direction.Right:
                dir = Vector3.right;
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(move_delay);

        move_sound.Play();
        if(!rot_move)
            move = true;
    }
    float angle;
    public float rot_speed;
    void move_to_rot_pos()
    {
        //transform.position += dir * (rot_speed*8) * Time.deltaTime;
        //transform.rotation = Quaternion.Lerp(transform.rotation,
        //                                        Quaternion.Euler(new Vector3(transform.rotation.x, (int)rot_pos * 90, transform.rotation.z)),
        //                                         rot_speed * Time.deltaTime);

        transform.RotateAround(obj_center.position, Vector3.up, rot_speed * Time.deltaTime);

        //if(Vector3.Distance(transform.position, target_pos) < 0.2f)

        Debug.Log(transform.rotation.eulerAngles.y + ",," + (int)rot_pos * 90);

        if (transform.rotation.eulerAngles.y >= (int)rot_pos * 90)
        {
            if ((int)rot_pos == 0)
            {
                if (transform.rotation.eulerAngles.y > 1)
                {
                    return;
                }
            }

            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, (int)rot_pos * 90, transform.rotation.z));
            rot_move = false;

            if ((int)rot_pos < 3)
            {
                rot_pos++;
            }
            else if ((int)rot_pos >= 3)
            {
                rot_pos = 0;
            }

            switch (rot_pos)
            {
                case Rot_Pos.Forward:
                    break;
                case Rot_Pos.Right:
                    break;
                case Rot_Pos.Back:
                    break;
                case Rot_Pos.Left:
                    break;
                default:
                    break;
            }

        }
    }

}
