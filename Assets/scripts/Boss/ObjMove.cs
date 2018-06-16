using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMove : Observer
{
    //현재 올라오는 돌다리에만 사용하게끔 만들어져 있으므로 추후 수정해야함.

    int cnt;
    public int clear_cnt;
    public bool clear;

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
    }

    private void Update()
    {
        if (move)
        {
            transform.position += dir * move_speed * Time.deltaTime;
            Debug.Log("d위치" + transform.position.y);
            if(transform.position.y >= -12.0f)
            {
                transform.position = new Vector3(transform.position.x, -12, transform.position.z);
                move = false;
                Debug.Log("move_end");
            }
        }
    }

    public override void notify(Observable obj)
    {
        if (!clear)
        {
            cnt++;
            if(cnt >= clear_cnt)
            {
                clear = true;
                StartCoroutine(timer());
            }
        }
    }

    IEnumerator timer()
    {

        yield return new WaitForSeconds(move_delay);
        
        move = true;
    }

}
