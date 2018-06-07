using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    //플레이어 카메라 (기본적인 팔로우캠 , 고정 캠 , 연출 캠이 있으며 해당 스크립트에서는 팔로우, 고정을 처리하고 연출은 적용만 시켜줌_외부에서 입력받을 수 있게끔 확장)

    public enum State
    {
        Follow, //플레이어 따라감
        Fixed,   //특정 구간에 고정
        Event   //연출 카메라 
    }
    public State cam_state;

    Transform tr;
    public Transform player;
    Transform fixed_target;
    Transform[] event_target;

    public Vector3 offset;
    public float move_speed;
    float speed;

	void Start () {
        tr = transform;
        cam_state = State.Follow;
        speed = move_speed;
    }
	
	void Update () {
        speed = move_speed;
        if (cam_state == State.Follow)
        {
            tr.position = Vector3.Lerp(tr.position, player.position + offset, Time.deltaTime * speed);
        }
        else if(cam_state == State.Fixed)
        {
            tr.position = Vector3.Lerp(tr.position, fixed_target.position + offset, Time.deltaTime * speed);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CamTrigger"))
        {
            fixed_target = other.transform;
        }   
        if(other.CompareTag("EventTrigger"))    //이벤트 발동은 영역에 들어가던가. 메세지를 받던가.
        {
            CameraEventSetting _tr = other.GetComponent<CameraEventSetting>();
            set_event_target(_tr.get_target_list());
        }
    }

    //이벤트 타겟의 Transform을 모두 복사해온다.
    public void set_event_target(Transform[] _tr)
    {
        for(int i =0; i < _tr.Length; i++)
        {
            event_target[i] = _tr[i];
        }
    }

    public void set_state(State _stat)
    {
        cam_state = _stat;
        if (_stat == State.Follow)
            speed = move_speed;
    }
}
