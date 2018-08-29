using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //플레이어 카메라 (기본적인 팔로우캠 , 고정 캠 , 연출 캠이 있으며 해당 스크립트에서는 팔로우, 고정을 처리하고 연출은 적용만 시켜줌_외부에서 입력받을 수 있게끔 확장)

    public enum State
    {
        Follow, //플레이어 따라감
        Fixed,   //특정 구간에 고정
        Event,   //연출 카메라 
        Active,
        Dodge,
        Sword_Attack,
        Dash
    }
    public State cam_state;

    Transform tr;
    public Transform player;
    Transform fixed_target;
    Transform[] event_target;

    public Vector3 offset;
    Vector3 _offset;
    public float move_speed;
    float speed;
    public bool shake;
    IEnumerator timer;
    Transform origin;
    public Vector2 default_fov, dash_fov, cur_fov;

    void Start()
    {
        tr = transform;
        cam_state = State.Follow;
        speed = move_speed;
        _offset = offset;
        default_fov = new Vector2(Camera.main.fieldOfView, 0);
        dash_fov = new Vector2(55.0f, 0);
        cur_fov = default_fov;
    }

    void Update()
    {
        speed = move_speed;
        if (cam_state == State.Follow)
        {
            tr.position = Vector3.Lerp(tr.position, player.position + _offset, Time.deltaTime * speed);
        }
        else if (cam_state == State.Fixed)
        {
            tr.position = Vector3.Lerp(tr.position, fixed_target.position + _offset, Time.deltaTime * speed);
        }
        else if (cam_state == State.Active)
        {
            tr.position = player.position + _offset;
        }
        else if (cam_state == State.Event)
        {
            if (origin != null)
            {
                tr.position = Vector3.Lerp(tr.position, origin.position + _offset, Time.deltaTime * speed);
                Debug.Log(_offset);
            }
        }
        else if (cam_state == State.Dodge)
        {
            tr.position = Vector3.Lerp(tr.position, player.position + _offset * 0.95f, Time.deltaTime * speed * 0.7f);
        }
        else if(cam_state == State.Sword_Attack)
        {
            if(player.GetComponent<Player>().Sword.GetComponent<Attackable>().has_out)
            {
                tr.position = Vector3.Lerp(tr.position, player.position + _offset * 0.95f, Time.deltaTime * speed * 1.5f);
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, player.position + _offset * 1.7f, Time.deltaTime * speed * 2.0f);
            }
        }
        if (cam_state == State.Dash)
        {
            cur_fov = Vector2.Lerp(cur_fov, dash_fov, Time.deltaTime);
            tr.position = Vector3.Lerp(tr.position, player.position + _offset, Time.deltaTime * speed);
        }
        else
        {
            cur_fov = Vector2.Lerp(cur_fov, default_fov, Time.deltaTime);
            tr.position = Vector3.Lerp(tr.position, player.position + _offset, Time.deltaTime * speed);
        }

        Camera.main.fieldOfView = cur_fov.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CamTrigger"))
        {
            fixed_target = other.transform;
        }
        if (other.CompareTag("EventTrigger"))    //이벤트 발동은 영역에 들어가던가. 메세지를 받던가.
        {
            CameraEventSetting _tr = other.GetComponent<CameraEventSetting>();
            set_event_target(_tr.get_target_list());
        }
    }

    public void up_down_move(float _power, int loop_cnt, float loop_speed, float minus)
    {
        if (timer != null)
        {
            _offset.y = offset.y;
            tr.position = player.position + _offset;
            cam_state = State.Follow;
            StopCoroutine(timer);
        }
        timer = up_down_timer(_power, loop_cnt, loop_speed , minus);
        StartCoroutine(timer);
    }

    public void left_right_move(float _power, int loop_cnt, float loop_speed)
    {

        origin = tr;
        if (timer != null)
        {
            _offset.x = offset.x;
            tr.position = player.position + _offset;
            cam_state = State.Follow;
            StopCoroutine(timer);
        }
        _offset.y = 0;
        _offset.z = 0;
        timer = left_right_timer(_power, loop_cnt, loop_speed);
        StartCoroutine(timer);
    }

    IEnumerator up_down_timer(float _power, int loop_cnt, float loop_speed, float minus)
    {
        shake = true;
        for (int i = 0; i < loop_cnt; i++)
        {
            if (_power > 0) _power -= minus;

            _offset.y -= _power;
            yield return new WaitForSeconds(loop_speed);
            _offset.y = offset.y;
            _offset.y += _power;
            yield return new WaitForSeconds(loop_speed);
            _offset.y = offset.y;
        }
        cam_state = State.Follow;
        _offset = offset;
        shake = false;
    }

    IEnumerator left_right_timer(float _power, int loop_cnt, float loop_speed )
    {
        shake = true;
        for (int i = 0; i < loop_cnt; i++)
        {
            if (_power > 0) _power -= 0.7f;

            _offset.x -= _power;
            yield return new WaitForSeconds(loop_speed);
            _offset.x = offset.x;
            _offset.x += _power;
            yield return new WaitForSeconds(loop_speed);
            _offset.x = offset.x;
        }
        cam_state = State.Follow;
        _offset = offset;
        shake = false;
        origin = null;
    }

    //이벤트 타겟의 Transform을 모두 복사해온다.
    public void set_event_target(Transform[] _tr)
    {
        for (int i = 0; i < _tr.Length; i++)
        {
            event_target[i] = _tr[i];
        }
    }

    public void set_state(State _stat)
    {
        cam_state = _stat;
        if (_stat == State.Follow)
        {
            speed = move_speed;
            _offset = offset;
        }
        if (_stat == State.Event)
            _offset = Vector3.zero;
    }
}
