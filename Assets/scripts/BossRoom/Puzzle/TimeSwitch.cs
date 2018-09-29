using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSwitch : BasicSwitch {
    //시간에 따라 유지되는 스위치
    //

    FootSwitch foot_switch;

    public TimeSwitch [] time_switch_list;

    public Light _light;

    public float wait_time; //켜진 후 기다리는 시간
    public bool clear_puzzle = false;


    private void Start()
    {
        _light.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //현재 활만 (근거리공격 추후 추가 어딨는지몰겠음ㅠㅜ) 모든 공격에 상호작용 하므로 속성체크안함
        if(other.CompareTag("Arrow") && !get_switch() && use_enable)
        {
            StartCoroutine(switch_on_timer());            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Arrow") && !get_switch() && use_enable)
        {
            StartCoroutine(switch_on_timer());
            Destroy(collision.gameObject);
        }
    }

    IEnumerator switch_on_timer()
    {
        _light.gameObject.SetActive(true);   //활성화 표시용 불 켜줌
        set_switch(true);

        for(int i=0; i<time_switch_list.Length; i++)
        {
            if (!time_switch_list[i].get_switch())
            {
                break;
            }

            //for문에서 빠져나가지 않고 time_switch_list.Length -1과 같다면 내가 켜짐으로서 모든 오브젝트가 켜졌단 뜻이므로 코루틴을 해제시킴
            if(i == time_switch_list.Length -1)
            {
                for (int z = 0; z < time_switch_list.Length; z++)
                {
                    time_switch_list[z].clear_puzzle = true;
                }
                clear_puzzle = true;  //다른 스위치들의 타이머를 정지시키고 자신것도 정지
            }            
        }

        yield return new WaitForSeconds(wait_time);
        if (!clear_puzzle)
        {
            _light.gameObject.SetActive(false);
            set_switch(false);
            //foot_switch.ground_move_ctrl(Vector3.down);//시간이 다하면 강제로 내려줌
        }

    }

    public override void off_switch_set()
    {
        _light.gameObject.SetActive(false);
        clear_puzzle = false;
    }

    public void set_wait_time(int _time)
    {
        wait_time = _time;
    }
    
    public void set_switch_set(int i , TimeSwitch _switch)
    {
        time_switch_list[i] = _switch;
    }

    public TimeSwitch get_switch_set(int i)
    {
        return time_switch_list[i];
    }
    
    public void new_switch_set(int i)
    {
        time_switch_list = new TimeSwitch[i];
    }

    public void set_foot_switch(FootSwitch _fs)
    {
        foot_switch = _fs;
    }

    //발판용
    public void off_switch()
    {
        _light.gameObject.SetActive(false);
        set_switch(false);
    }
}
