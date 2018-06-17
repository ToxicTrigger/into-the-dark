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

    public Direction move_dir;
    public float move_speed;
    public float move_delay;
    public Vector3 move_target;     //이동완료 타겟에 대한건 좀 더 생각해볼것.
    bool move;
    Vector3 dir;

    float origin_dis;

	void Start () {
    }

    private void Update()
    {
        if (move)
        {
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
            else if(move_dir == Direction.Down)
            {
                if (transform.position.y <= -20.0f)
                {
                    move_sound.Stop();
                    transform.position = new Vector3(transform.position.x, -20, transform.position.z);
                    move = false;
                }
            }
        }
    }

    public override void notify(Observable obj)
    {
        BasicSwitch obj_switch = obj as BasicSwitch;
        if (obj_switch.get_switch())
        {         
            cnt++;
            if(cnt >= clear_cnt)
            {
                StartCoroutine(timer());
            }
        }
        else
        {
            cnt--;
            StartCoroutine(timer());
        }
    }

    public IEnumerator timer()
    {

        move_dir = move_dir == Direction.Up ? Direction.Down : Direction.Up;

        switch (move_dir)
        {
            case Direction.Up:
                dir = Vector3.up;
                break;
            case Direction.Down:
                dir = Vector3.down;
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
        move = true;
    }

}
