using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotuUI : MonoBehaviour
{
    public int state = -1;
    public Animator ani;
    public PlayerMove pm;
    public GameObject Tip;

    public float input_time;
    public bool event_run;
    public IEnumerator StartTimer(float time)
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
            case 6:
                Tip.transform.GetChild(0).GetComponent<Text>().text = "마우스 왼쪽을 눌러 대상을 겨냥 하세요";
                if (pm.transform.GetComponent<Player>().weapon.type == Weapon.Type.Bow)
                {
                    if (!event_run)
                    {
                        if (Input.GetButtonDown("Fire1"))
                        {
                            StartCoroutine(StartTimer(5));
                            //Destroy(gameObject);
                        }
                    }
                }

                
                break;
            case 7:
                Tip.transform.GetChild(0).GetComponent<Text>().text = "지정된 위치에 토템을 설치 후 활로 겨냥하세요";
                if (Input.GetKeyDown(KeyCode.E))
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
