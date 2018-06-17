using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientWeapon : Observer
{
    //고대병기 
    enum State
    {
        Activated = 0,    //활성화된
        Deactivated     //비활성화된
    }

    //public Light weapon_light;

    BossRoomManager manager;

    public Animator animator;

    int activate_count = -1; //현재까지 활성화된 횟수를 저장
    [Tooltip("고대병기의 활성화 횟수에 따른 유지시간 지정")]
    public float[] time_list;   //엔진에서 횟수에 따른 시간을 지정해줄거임
    [Tooltip("고대병기에 할당된 스위치의 최대 개수")]
    public int max_count = 2;
    public int activate_torch_count = 0;
    float timer;    //활성화 유지 시간

    State state;
    IEnumerator _timer;

    void Start()
    {
        //weapon_light.gameObject.SetActive(false);
        state = State.Deactivated;  //초기 상태는 비활성화된 상태
    }

    public override void notify(Observable observable)
    {
        //throw new System.NotImplementedException();
        ObservableTorch torch = observable as ObservableTorch;
        Debug.Log(torch.get_state());
        //Debug.Log("AncientWeapon __ torch.name = \"" + torch.name + " || torch.get_switch() = \"" + torch.get_state());
        if (torch.get_state() == ObservableTorch.State.On)
        {
            if(activate_torch_count <max_count ) activate_torch_count++;

            if (activate_torch_count >= max_count)
            {
                activate(); //활성화!
            }
        }
        else
        {
            if (activate_torch_count > 0)
            {
                activate_torch_count--;
            }
            if (state == State.Activated)
            {
                torch_deactivate();
            }
        }
    }

    //활성화 시키는 함수
    void activate()
    {
        animator.SetBool("activate", true);
        if (_timer != null)StopCoroutine(_timer);  //이전 코루틴 정지 _ 새로운 코루틴을 그냥 할당해 버리면 이전 코루틴을 정지시킬 수 없어짐 (아마도?)
        _timer = activate_timer();  //새로운 코루틴 할당 
        //weapon_light.gameObject.SetActive(true);
        state = State.Activated;
        BossRoomManager.get_instance().send_boss_state(Boss_Worm.Action.Groggy); //weapon_activation() : 보스 그로기상태 전환 
        StartCoroutine(_timer);
    }

    //활성화 타이머
    IEnumerator activate_timer()
    {
        if (activate_count < time_list.Length - 1)
            activate_count++;   //활성화 횟수를 증가시킨다.
        yield return new WaitForSeconds(time_list[activate_count]);
        deactivate();   //일정 시간이 지나면 비활성화 시키는 함수를 호출한다.
    }

    //고대병기의 "활성화 유지시간이 끝나며" 일괄처리 _ 퍼즐과 보스움직임에대한 처리를 해준다.
    void deactivate()
    {
        Debug.Log("비활성화");
        animator.SetBool("activate", false);
        //weapon_light.gameObject.SetActive(false);
        state = State.Deactivated;
        BossRoomManager.get_instance().send_boss_state(Boss_Worm.Action.Idle);
        BossRoomManager.get_instance().off_switch();
    }

    //외부 요인으로 인하여 고대병기 비활성화 _ 고대병기에 대한 처리만 해준다.
    void torch_deactivate()
    {
        Debug.Log("비활성화");
        animator.SetBool("activate", false);
        //외부 요인으로 인해 비활성화됨
        //weapon_light.gameObject.SetActive(false);
        state = State.Deactivated;
        StopCoroutine(_timer);    //타이머가 정상적으로 종료되기 전에 외부요인으로 인해 비활성화 되었으므로 임의로 종료시킨다.
    }
}
