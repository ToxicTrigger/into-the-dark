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
        if( Tip != null && state == 5 ) Tip.SetActive(true);
        event_run = true;
        yield return new WaitForSeconds(time);
        state++;
        event_run = false;
        if( Tip != null ) Tip.SetActive(false);
    }

    public IEnumerator StopTimer()
    {
        pm.enabled = false;
        yield return new WaitForSeconds(6.7f);
        pm.enabled = true;
        state = -1;
    }
    private void Start()
    {
        StartCoroutine(StopTimer());
    }

    bool temp;
    void Update()
    {
        switch( state )
        {
            case -1:
                if( pm.enabled )
                {
                    if( !InputManager.get_instance().has_not_anything_input() )
                    {
                        if( !event_run )
                            StartCoroutine(StartTimer(2));
                    }
                }
                break;
            case 1:
                if( Input.GetButtonDown("Dash") )
                {
                    if( !event_run )
                        StartCoroutine(StartTimer(3));
                }
                break;
            case 3:
                if( Input.GetButtonDown("Fire1") )
                {
                    if( !event_run )
                        StartCoroutine(StartTimer(1));
                }
                break;
            case 5:
                if( Input.GetKeyDown(KeyCode.Tab) )
                {
                    if( !event_run )
                        StartCoroutine(StartTimer(1));
                }
                Tip.transform.GetChild(0).GetComponent<Text>().text = "마우스 왼쪽을 꾹 눌러 대상을 겨냥 하세요";
                if( pm.transform.GetComponent<Player>().weapon.type == Weapon.Type.Bow )
                {
                    if( !event_run )
                    {
                        if( Input.GetButtonDown("Fire1") )
                        {
                            StartCoroutine(StartTimer(3));
                            //Destroy(gameObject);
                        }
                    }
                }
                break;
            case 6:
                if( !temp )
                {
                    state = 7;
                    temp = true;
                }
                break;

            case 9:
                if( Input.GetKeyDown(KeyCode.E) )
                {
                    temp = false;
                    if( !event_run )
                        StartCoroutine(StartTimer(1));
                }
                break;
            case 11:
                Tip.transform.GetChild(0).GetComponent<Text>().text = "안개가 있는 곳은 화살 공격이 무효화됩니다";

                if( !event_run )
                    StartCoroutine(StartTimer(3));
                break;
            case 12:
                if( !event_run )
                    StartCoroutine(StartTimer(1));
                break;

            case 13:
                Tip.transform.GetChild(0).GetComponent<Text>().text = "밝게 빛나는 곳에 토템을 설치하세요";
                if( !event_run && Input.GetKeyDown(KeyCode.R) )
                {
                    StartCoroutine(StartTimer(1));
                    
                }
                break;
            case 14:
                if( !temp )
                {
                    StartCoroutine(StartTimer(1));
                    temp = true;
                }
                break;
            case 15:
                temp = false;
                Tip.transform.GetChild(0).GetComponent<Text>().text = "토템을 통과한 화살은 안개를 무시합니다";
                if( !event_run )
                {
                    StartCoroutine(StartTimer(3));
                }
                break;
            case 16:
                Tip.transform.GetChild(0).GetComponent<Text>().text = "토템을 조준하여 활을 거점으로 발사 하세요";
                if( !event_run  )
                {
                    StartCoroutine(StartTimer(6));
                }
                break;
            case 17:
                Tip.transform.GetChild(0).GetComponent<Text>().text = "토템으로 가서 토템을 회수할 수 있습니다";
                if( !event_run && Input.GetKeyDown(KeyCode.R) )
                {
                    StartCoroutine(StartTimer(0.3f));

                }
                break;
        }
        ani.SetInteger("step" , state);
    }
}
