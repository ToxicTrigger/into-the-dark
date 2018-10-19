﻿using System.Collections;
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

    public float default_fov;
    public List<Transform> Pins;

    public Vector3 Angle, default_angle;
    public Vector3 Offset, default_offset;

    public bool GameStart;
    public float StartTime;

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
        if( !has_camera_using )
        {
            cur_state = now_state;
            now_state = state;
        }
        else
        {
            if( state != now_state )
            {
                command.Enqueue(state);
            }
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
    IEnumerator calc_fsm()
    {
        CalcPinDist cpd = FindObjectOfType<CalcPinDist>();
        bool has_use_Zone = cpd == null ? false : cpd.enabled ;
        while( true )
        {
            if(!has_use_Zone)
            {
                switch( now_state )
                {
                    case State.Idle:
                        has_camera_using = false;
                        break;

                    case State.Follow:

                        RaycastHit hit;
                        Vector3 pos = transform.position;
                        Vector3 tmp = now_target.position;
                        tmp.y += 1;
                        Ray ray = new Ray(tmp , ( ( transform.position + Offset ) - tmp ).normalized);
                        if( Physics.Raycast(ray , out hit , Vector3.Distance(tmp , transform.position) , ~( 1 << LayerMask.NameToLayer("Ground") ) ))
                        {
                            if(!hit.collider.gameObject.CompareTag("Sword") && hit.collider.gameObject.layer != LayerMask.NameToLayer("Object"))
                            pos = Vector3.Lerp(pos , hit.point , action_speed);
                        }
                        else
                        {
                            if(GameStart)
                            {
                                pos = Vector3.Lerp(pos , now_target.position + Offset , action_speed * 0.1f);
                            }
                            else
                            {
                                pos = Vector3.Lerp(pos , now_target.position + Offset , action_speed);
                            }
                        }
                        Debug.DrawRay(ray.origin , ray.direction * Vector3.Distance(tmp , transform.position + Offset) , Color.blue);
                        transform.position = pos;
                        has_camera_using = false;
                        //transform.eulerAngles = Angle;
                        var ang = Quaternion.Lerp(transform.rotation , Quaternion.RotateTowards(transform.rotation , Quaternion.Euler(Angle) , action_speed * 10f), action_speed);
                        transform.rotation = ang;
                        break;

                    case State.Move_Pin:
                        pos = Vector3.Lerp(transform.position , now_target.position , action_speed);
                        transform.position = pos;

                        transform.rotation = Quaternion.RotateTowards(transform.rotation , Quaternion.Euler(Angle) , action_speed * 10f);
                        if( Vector3.Distance(pos , now_target.position) >= 0.2f )
                        {
                            has_camera_using = true;
                        }
                        else
                        {
                            has_camera_using = false;
                        }
                        break;
                }
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

    public void Update()
    {
        if(GameStart)
        {
            if(StartTime <= 0)
            {
                StartTime = 0;
                GameStart = false;
            }
            else
            {
                StartTime -= Time.deltaTime;
            }
        }
        if( !has_camera_using )
        {
            if( command.Count != 0 )
            {
                SetState(command.Dequeue());
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
        command = new Queue<State>();

        cam = Camera.main;

        default_angle = transform.eulerAngles;
        default_offset = Offset;
        default_fov = cam.fieldOfView;

        StartCoroutine(calc_fsm());
    }
}
