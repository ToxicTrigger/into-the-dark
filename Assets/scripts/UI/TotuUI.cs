using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotuUI : MonoBehaviour
{
    public int state = -1;
    public Animator ani;
    public PlayerMove pm;
    public GameObject Tip;

    public float input_time;
    public bool event_run;
    public IEnumerator StartTimer(int time)
    {
        if( Tip != null  && state == 5) Tip.SetActive(true);
        event_run = true;
        yield return new WaitForSeconds(time);
        state++;
        event_run = false;
        if( Tip != null ) Tip.SetActive(false);
    }

    public IEnumerator StopTimer()
    {
        pm.enabled = false;
        yield return new WaitForSeconds(7.0f);
        pm.enabled = true;
        state = -1;
    }
    private void Start()
    {
        StartCoroutine(StopTimer());
    }


    void Update()
    {
        switch( state )
        {
            case -1:
                if(pm.enabled)
                {
                    if( InputManager.get_instance().get_Vertical() != 0 )
                    {
                        if(!event_run)
                        StartCoroutine(StartTimer(2));
                    }
                }
                break;
            case 1:
                if(Input.GetButtonDown("Dash"))
                {
                    if( !event_run )
                        StartCoroutine(StartTimer(3));
                }
                break;
            case 3:
                if(Input.GetButtonDown("Fire1"))
                {
                    if( !event_run )
                        StartCoroutine(StartTimer(1));
                }
                break;
            case 5:
                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    if( !event_run )
                        StartCoroutine(StartTimer(1));
                }
                break;
            case 7:
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if( !event_run )
                        StartCoroutine(StartTimer(1));
                    state = 8;
                }
                break;
        }
        ani.SetInteger("step" , state);
    }
}
