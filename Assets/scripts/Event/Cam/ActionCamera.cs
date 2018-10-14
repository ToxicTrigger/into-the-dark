using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    public enum State
    {
        Idle,
        Follow,
        Move_Pin,
        Rotate_Camera,
        Move_Pin_AND_Rotate,
        Shake,
        Look,
        
    }

    public struct EventInfo
    {
        public State state;
        public Transform target;
        //이벤트 최소 보증시간
        public float guarantee_time;
        public float action_speed;

        public EventInfo(State _state, Transform _tr, float _time, float _speed)
        {
            state = _state;
            target = _tr;
            guarantee_time = _time;
            action_speed = _speed;
        }
    }

    public Camera cam;

    [SerializeField]
    private State now_state, cur_state;


    public Transform now_target, cur_target;
    public bool has_camera_using;

    public float action_speed = 1, default_speed;
    public float guarantee_time, default_time = 0;

    //public Queue<State> command;
    public Queue<EventInfo> command;

    public float default_fov;
    public List<Transform> Pins;

    public Vector3 Angle, default_angle;
    public Vector3 Offset, default_offset;

    IEnumerator Zoom(float fov , bool zoom_in , float speed)
    {
        float cam_fov = cam.fieldOfView;
        if( zoom_in )
        {
            while( cam.fieldOfView >= fov && cam.fieldOfView - 0.1f >= fov )
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView , fov , Time.time * speed);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while( cam.fieldOfView <= fov && cam.fieldOfView - 0.1f <= fov )
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView , fov , Time.time * speed);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void ZoomInOut(float fov , bool zoom_in , float speed)
    {
        StartCoroutine(Zoom(fov , zoom_in , speed));
    }

    public void SetState(State state)
    {
        cur_state = now_state;
        now_state = state;
    }

    public void SetStateTarget(int num , State state , float _action_speed)
    {
        if(!has_camera_using)
        {
            SetTarget(Pins[num]);
            SetState(state);
            action_speed = _action_speed;
        }
        else
        {
            EventInfo info = new EventInfo(state, Pins[num], 0.0f, _action_speed);
            command.Enqueue(info);
        }

    }

    public void SetStateTarget(Transform transform , State state , float _action_speed)
    {
        if(!has_camera_using)
        {
            SetTarget(transform);
            SetState(state);
            action_speed = _action_speed;
        }
        else
        {
            EventInfo info = new EventInfo(state, transform, 0.0f, _action_speed);
            command.Enqueue(info);
        }
    }

    public void SetStateTarget(EventInfo _evnet_info)
    {
        if (!has_camera_using)
        {
            SetTarget(_evnet_info.target);
            SetState(_evnet_info.state);
            action_speed = _evnet_info.action_speed;
        }
        else
        {
            EventInfo info = new EventInfo(_evnet_info.state, _evnet_info.target, 0.0f, _evnet_info.guarantee_time);
            command.Enqueue(info);
        }
    }

    public void SetTarget(Transform target)
    {
        if( !target.Equals(now_target) )
        {
            cur_target = now_target;
            now_target = target;
        }
    }

    public void SetTarget(int number)
    {
        if( !Pins[ number ].Equals(now_target) )
        {
            cur_target = now_target;
            now_target = this.Pins[ number ];
        }
    }

    float guarantee_timer;
    IEnumerator calc_fsm()
    {
        while( true )
        {
            switch ( now_state )
            {
                case State.Idle:
                    has_camera_using = false;
                    break;

                case State.Follow:
                    Vector3 pos = Vector3.Lerp(transform.position , now_target.position + Offset , action_speed);
                    transform.position = pos;
                    has_camera_using = false;
                    transform.eulerAngles = Angle;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation , Quaternion.Euler(Angle) , action_speed * 10f);
                    break;

                case State.Move_Pin:
                    guarantee_timer += Time.deltaTime;

                    pos = Vector3.Lerp(transform.position , now_target.position , action_speed);
                    transform.position = pos;

                    transform.rotation = Quaternion.RotateTowards(transform.rotation , Quaternion.Euler(Angle) , action_speed * 10f);
                    if( Vector3.Distance(pos , now_target.position) >= 0.2f )
                    {
                        has_camera_using = true;
                    }
                    else if(guarantee_timer >= guarantee_time)
                    {
                        guarantee_timer = 0;
                        has_camera_using = false;
                        SetState(State.Idle);
                    }
                    break;

            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void Update()
    {
        if( !has_camera_using )
        {
            if( command.Count != 0 )
            {
                //SetState(command.Dequeue());
                SetStateTarget((EventInfo)command.Dequeue());
            }
        }
    }

    IEnumerator ShakeCam(int tick , float power , float tick_by_tick_time)
    {
        for( int i = 0 ; i < tick ; ++i )
        {
            float v = Random.Range(-power , power);
            float h = Random.Range(-power , power);
            Vector3 shake = new Vector3(v , h, -v);
            transform.position += shake;

            yield return new WaitForSeconds(tick_by_tick_time);
        }
    }

    public void Shake(int tick , float power , float tick_by_tick_time)
    {
        StartCoroutine(ShakeCam(tick , power , tick_by_tick_time));
    }

    // Use this for initialization
    void Awake()
    {
        command = new Queue<EventInfo>();

        cam = Camera.main;

        default_angle = transform.eulerAngles;
        default_offset = Offset;
        default_fov = cam.fieldOfView;
        default_speed = action_speed;

        StartCoroutine(calc_fsm());
    }
}


//public void SetState(State state)
//{
//    if (!has_camera_using)
//    {
//        cur_state = now_state;
//        now_state = state;
//    }
//    else
//    {
//        if (state != now_state)
//        {
//            //command.Enqueue(state);
//            EventInfo info = new EventInfo();
//            info.state = state;
//            command.Enqueue(info);
//        }
//    }
//}

//public void SetStateTarget(int num, State state, float _action_speed)
//{
//    SetTarget(Pins[num]);
//    SetState(state);
//    action_speed = _action_speed;
//}

//public void SetStateTarget(Transform transform, State state, float _action_speed)
//{
//    SetTarget(transform);
//    SetState(state);
//    action_speed = _action_speed;
//}

//public void SetTarget(Transform target)
//{
//    if (!target.Equals(now_target))
//    {
//        cur_target = now_target;
//        now_target = target;
//    }
//}

//public void SetTarget(int number)
//{
//    if (!Pins[number].Equals(now_target))
//    {
//        cur_target = now_target;
//        now_target = this.Pins[number];
//    }
//}