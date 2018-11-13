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
        Teleport_Pin,
        Flow,
    }

    public struct EventInfo
    {
        public State state;
        public Transform target;
        //이벤트 최소 보증시간
        public float guarantee_time;
        public float action_speed;
        public Vector3 angle;
        public string event_name;
        public Vector3 flow_dir;

        public EventInfo(State _state, Transform _tr, float _time, float _speed, Vector3 _angle, string _name, Vector3 _flow_dir)
        {
            state = _state;
            target = _tr;
            guarantee_time = _time;
            action_speed = _speed;
            angle = _angle;
            event_name = _name;
            flow_dir = _flow_dir;
        }
    }

    public Camera cam;

    [SerializeField]
    private State now_state, cur_state;


    public Transform now_target, cur_target;
    public bool has_camera_using;

    public float action_speed = 1, default_speed;
    public float guarantee_time, default_time = 0;
    public string now_event_name;
    public Vector3 flow_dir;

    //public Queue<State> command;
    public Queue<EventInfo> command;

    public float default_fov;
    public List<Transform> Pins;

    public Vector3 Angle, default_angle;
    public Vector3 Offset, default_offset;

    public bool GameStart;
    public float StartTime;

    IEnumerator Zoom(float fov, bool zoom_in, float speed)
    {
        float cam_fov = cam.fieldOfView;
        if (zoom_in)
        {
            while (cam.fieldOfView >= fov && cam.fieldOfView - 0.1f >= fov)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.time * speed);
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (cam.fieldOfView <= fov && cam.fieldOfView - 0.1f <= fov)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.time * speed);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void ZoomInOut(float fov, bool zoom_in, float speed)
    {
        StartCoroutine(Zoom(fov, zoom_in, speed));
    }

    public void SetState(State state)
    {
        cur_state = now_state;
        now_state = state;
    }

    public void SetStateTarget(Transform target, State state)
    {
        if(!has_camera_using)
        {
            SetTarget(target);
            SetState(state);
        }
        else
        {
            EventInfo info = new EventInfo(state, target, 0, 0.3f, default_angle, "Player", Vector3.zero);
            command.Enqueue(info);
        }
    }


    public void SetStateTarget(int num, State state, float _action_speed, float _guarantee_time, Vector3 angle, string _name, Vector3 _flow_dir)
    {
        if (!has_camera_using)
        {
            has_camera_using = true;
            SetTarget(Pins[num]);
            SetState(state);
            action_speed = _action_speed;
            guarantee_time = _guarantee_time;
            now_event_name = _name;
            Angle = angle;
            flow_dir = _flow_dir;
        }
        else if (!now_event_name.Equals(_name))
        {
            EventInfo info = new EventInfo(state, Pins[num], _guarantee_time, _action_speed, angle, _name, _flow_dir);
            command.Enqueue(info);
        }

    }

    public void SetStateTarget(Transform transform, State state, float _action_speed, float _guarantee_time, Vector3 angle, string _name, Vector3 _flow_dir)
    {
        if (!has_camera_using)
        {
            has_camera_using = true;
            SetTarget(transform);
            SetState(state);
            action_speed = _action_speed;
            guarantee_time = _guarantee_time;
            now_event_name = _name;
            Angle = angle;
            flow_dir = _flow_dir;
        }
        else if (!now_event_name.Equals(_name))  //같은 이벤트는 연속으로 받지 않는다.
        {
            EventInfo info = new EventInfo(state, transform, _guarantee_time, _action_speed, angle, _name, _flow_dir);
            command.Enqueue(info);
        }
    }

    public void SetStateTarget(EventInfo _event_info)
    {
        if (!has_camera_using)
        {
            has_camera_using = true;
            SetTarget(_event_info.target);
            SetState(_event_info.state);
            action_speed = _event_info.action_speed;
            guarantee_time = _event_info.guarantee_time;
            now_event_name = _event_info.event_name;
            Angle = _event_info.angle;
            flow_dir = _event_info.flow_dir;
        }
        else if (!now_event_name.Equals(_event_info.event_name))
        {
            EventInfo info = new EventInfo(_event_info.state, _event_info.target, _event_info.guarantee_time, _event_info.guarantee_time, _event_info.angle, _event_info.event_name, _event_info.flow_dir);
            command.Enqueue(info);
        }
    }

    public void SetTarget(Transform target)
    {
        if (!target.Equals(now_target))
        {
            cur_target = now_target;
            now_target = target;
        }
    }

    public void SetTarget(int number)
    {
        if (!Pins[number].Equals(now_target))
        {
            cur_target = now_target;
            now_target = this.Pins[number];
        }
    }

    int ignore;
    public bool overlap;
    public float guarantee_timer;
    public bool use_overlap=true;
    IEnumerator calc_fsm()
    {
        ignore = (1 << LayerMask.NameToLayer("Ground")) + (1 << LayerMask.NameToLayer("Ignore Raycast"));
        CalcPinDist cpd = FindObjectOfType<CalcPinDist>();
        ignore = ~ignore;
        while (true)
        {
            bool has_use_Zone = cpd == null ? false : cpd.enabled;
            if (!has_use_Zone)
            {
                if(enabled)
                {
                    switch (now_state)
                    {
                        case State.Idle:
                            has_camera_using = false;
                            break;

                        case State.Follow:
                            RaycastHit hit;
                            Vector3 pos = transform.position;
                            Vector3 tmp = now_target.position;
                            tmp.y += 1;
                            Ray ray = new Ray(tmp, ((transform.position + Offset) - tmp).normalized);
                            if (Physics.Raycast(ray, out hit, Vector3.Distance(tmp, transform.position), ignore))
                            {
                                if (!hit.collider.gameObject.CompareTag("Sword")
                                    && hit.collider.gameObject.layer != LayerMask.NameToLayer("Object")
                                    && hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast")
                                    && hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground")
                                    && hit.collider.gameObject.layer != LayerMask.NameToLayer("Totem")
                                    && use_overlap
                                    )
                                {
                                    pos = Vector3.Lerp(pos, hit.point, action_speed * 0.2f);
                                    overlap = true;
                                }
                                else
                                {
                                    pos = Vector3.Lerp(pos, now_target.position + Offset, action_speed);
                                }
                            }
                            else
                            {
                                overlap = false;
                                if (GameStart)
                                {
                                    pos = Vector3.Lerp(pos, now_target.position + Offset, action_speed * 0.1f);
                                }
                                else
                                {
                                    pos = Vector3.Lerp(pos, now_target.position + Offset, action_speed);
                                }
                            }
                            Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(tmp, transform.position + Offset), Color.blue);
                            transform.position = pos;
                            has_camera_using = false;
                            //transform.eulerAngles = Angle;
                            var ang = Quaternion.Lerp(transform.rotation, Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Angle), action_speed * 10f), action_speed);
                            transform.rotation = ang;
                            break;
                        case State.Move_Pin:
                            guarantee_timer += Time.deltaTime;

                            pos = Vector3.Lerp(transform.position, now_target.position, action_speed);
                            transform.position = pos;

                            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Angle), action_speed * 10f);
                            if (Vector3.Distance(pos, now_target.position) >= 0.2f)
                            {
                                has_camera_using = true;
                            }
                            else
                            {
                                end_state();
                            }
                            break;

                        case State.Teleport_Pin:
                            guarantee_timer += Time.deltaTime;
                            transform.position = now_target.position;
                            transform.rotation = Quaternion.Euler(Angle);
                            end_state();
                            break;

                        case State.Flow:
                            guarantee_timer += Time.deltaTime;
                            transform.position += flow_dir * action_speed * Time.deltaTime;
                            end_state();
                            break;

                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void end_state()
    {
        if (guarantee_timer >= guarantee_time)
        {
            guarantee_timer = 0;
            has_camera_using = false;
            flow_dir = Vector3.zero;
            //SetState(State.Idle);
        }
    }

    public void Update()
    {
        if (GameStart)
        {
            if (StartTime <= 0)
            {
                StartTime = 0;
                GameStart = false;
                FindObjectOfType<PlayerMove>().enabled = true;
            }
            else
            {
                StartTime -= Time.deltaTime;
            }
        }
        if (!has_camera_using)
        {
            if (command.Count != 0)
            {
                SetStateTarget((EventInfo)command.Dequeue());
            }
        }
    }

    IEnumerator ShakeCam(int tick, float power, float tick_by_tick_time)
    {
        for (int i = 0; i < tick; ++i)
        {
            float v = Random.Range(-power, power);
            float h = Random.Range(-power, power);
            Vector3 shake = new Vector3(v, h, -v);
            transform.position += shake;

            yield return new WaitForSeconds(tick_by_tick_time);
        }
    }

    public void Shake(int tick, float power, float tick_by_tick_time)
    {
        StartCoroutine(ShakeCam(tick, power, tick_by_tick_time));
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