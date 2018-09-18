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

    public Camera cam;

    [SerializeField]
    private State now_state, cur_state;

    public Transform now_target, cur_target;
    public bool has_camera_using;

    public float action_speed = 1;

    public Queue<State> command;

    public List<Vector3> Angles;
    public List<Transform> Pins;
    public List<Vector3> Offsets;

    public Vector3 Angle;
    public Vector3 Offset;

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
        if(!has_camera_using)
        {
            cur_state = now_state;
            now_state = state;
        }
        else
        {
            command.Enqueue(state);
        }
    }

    public void SetStateTarget(int num , State state)
    {
        SetTarget(Pins[ num ]);
        SetState(state);
    }

    public void SetStateTarget(Transform transform , State state)
    {
        SetTarget(transform);
        SetState(state);
    }

    public void SetTarget(Transform target)
    {
        cur_target = now_target;
        now_target = target;
    }

    public void SetTarget(int number)
    {
        cur_target = now_target;
        now_target = this.Pins[ number ];
    }

    IEnumerator calc_fsm()
    {
        while( true )
        {
            switch( now_state )
            {
                case State.Idle:
                    has_camera_using = false;
                    break;

                case State.Follow:
                    Vector3 pos = Vector3.Lerp(transform.position , now_target.position + Offset, Time.time * action_speed);
                    transform.position = pos;
                    has_camera_using = false;
                    break;

                case State.Move_Pin:
                    pos = Vector3.Lerp(transform.position , now_target.position , Time.time * action_speed);
                    transform.position = pos;

                    if(Vector3.Distance(pos, now_target.position) >= 0.2f)
                    {
                        has_camera_using = true;
                    }
                    else
                    {
                        has_camera_using = false;
                    }
                    break;
                    
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void Update()
    {
        if(!has_camera_using)
        {
            if(command.Count != 0)
            {
                SetState(command.Dequeue());
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        command = new Queue<State>();
        cam = Camera.main;
        StartCoroutine(calc_fsm());
    }
}
