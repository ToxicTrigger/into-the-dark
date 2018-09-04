using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBridge : Observer
{
    //기본적으로 클리어 조건에 따라 올라오는 다리
    //그로기 해제 후 맵을 초기화할 시 퍼즐과 횃불 상태만 초기화되므로 다리는 최초 1회 올리기만 한다.

    [Tooltip("다리가 올라온 상태인지 표시")]
    public bool is_up;
    public float move_speed;
    public float move_distance;
    public float start_y_pos;

	void Start () {
        is_up = false;
        start_y_pos = transform.position.y;
        up_bridge(Vector3.down);
	}

    //신호가 들어오면 무조건 올라온다.
    public override void notify(Observable observable)
    {
        ObservableTorch torch = observable as ObservableTorch;
        Debug.Log(torch.torch_state);
        if (torch.torch_state == ObservableTorch.State.On && !is_up)
            up_bridge(Vector3.up);
    }

    void up_bridge(Vector3 _dir)
    {
            StartCoroutine(move_bridge(_dir));
    }

    IEnumerator move_bridge(Vector3 _move_dir)
    {
        while (true)
        {
            transform.position += _move_dir * move_speed * Time.deltaTime;

            if((_move_dir == Vector3.up && transform.position.y > start_y_pos) ||
                (_move_dir == Vector3.down && transform.position.y < start_y_pos - move_distance))
            {
                if (_move_dir == Vector3.up) is_up = true;
                break;
            }

            yield return new WaitForSeconds(0.01f);
        }        
    }

}
