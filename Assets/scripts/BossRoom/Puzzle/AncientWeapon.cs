using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientWeapon : Observer
{
    //고대병기 
    public enum State
    {
        Activated = 0,    //활성화된
        Deactivated     //비활성화된
    }

    //public Light weapon_light;

    BossRoomManager manager;
    public Boss_State boss_state;

    public Animator animator;

    int activate_count = 0; //현재까지 활성화된 횟수를 저장
    [Tooltip("고대병기의 활성화 횟수에 따른 유지시간 지정")]
    public float[] time_list;   //엔진에서 횟수에 따른 시간을 지정해줄거임
    [Tooltip("고대병기에 할당된 스위치의 최대 개수")]
    public int max_count;
    public int activate_torch_count = 0;
    float timer;    //활성화 유지 시간

    public State state;
    IEnumerator _timer;
    public IEnumerator ready_timer;
 

    void Start()
    {
        manager = BossRoomManager.get_instance();

        _timer = activate_timer();
        state = State.Activated;  //초기 상태는 비활성화된 상태
        if (ready_timer == null)
        {
            ready_timer = ready_action(true);
            StartCoroutine(ready_timer);
        }
    }

    void Update()
    {
    }

    public override void notify(Observable observable)
    {
        BasicSwitch torch = observable as BasicSwitch;
        //ObservableTorch torch = observable as ObservableTorch;
        if (torch.get_switch())
        //if(torch.get_state() == ObservableTorch.State.On)
        {
            if(activate_torch_count <max_count ) activate_torch_count++;

            if (activate_torch_count >= max_count)
            {
                if(ready_timer == null)
                {
                    ready_timer = ready_action(false);
                    StartCoroutine(ready_timer);
                }
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
                if (ready_timer == null)
                {
                    ready_timer = ready_action(false);
                    StartCoroutine(ready_timer);
                }
                //deactivate();
            }
        }
    }

    //활성화 시키는 함수
    void activate()
    {
        activate_count++;
        //최초 활성화 연출
        if (activate_count == 1)
        {
            manager.play_event(BossRoomManager.EventList.AncientWeapon);
        }

        animator.SetBool("activate", true);
        //weapon_light.gameObject.SetActive(true);
        state = State.Activated;

        //StopCoroutine(_timer);  //이전 코루틴 정지 _ 새로운 코루틴을 그냥 할당해 버리면 이전 코루틴을 정지시킬 수 없어짐 (아마도?)
        //_timer = activate_timer();  //새로운 코루틴 할당 
        //StartCoroutine(_timer);
    }

    //활성화 타이머
    IEnumerator activate_timer()
    {
        BossRoomManager.get_instance().get_ancient_ui().switching_ui(true, time_list[(int)BossRoomManager.get_instance().phase]);
        yield return new WaitForSeconds(time_list[(int)BossRoomManager.get_instance().phase]);

        if (ready_timer == null)
        {
            ready_timer = ready_action(false);
            StartCoroutine(ready_timer);
        }
    }


    public float move_y;
    public int ready_time;
    //활성화, 비활성화가 되기 전 준비하는 코루틴 (올라가거나 내려감)
    IEnumerator ready_action(bool init)
    {
        Vector3 move_dir;
        if(state == State.Activated)
        {
            move_dir = Vector3.down;
            //내려가야함 
            if (!init)
            {
                deactivate();
            }
            else
            {
                state = State.Deactivated;
            }
        }
        else
        {
            activate();
            move_dir = Vector3.up;
            if(!manager.get_is_puzzle_clear())
            {
                //퍼즐 클리어 연출 추가

                manager.set_is_puzzle_clear(true);
            }
        }

        move_dir.y *= move_y / ready_time;

        for (int i = 0; i < ready_time; i++)
        {
            if (!SoundManager.get_instance().sound_list[(int)SoundManager.SoundList.rumble].isPlaying)
                SoundManager.get_instance().play_sound(SoundManager.SoundList.rumble);
            transform.position += move_dir;
            yield return new WaitForSeconds(0.01f);
        }
        if (state == State.Activated)
        {
            BossRoomManager.get_instance().send_boss_state(Boss_State.State.Groggy, BossRoomManager.get_instance().center); //weapon_activation() : 보스 그로기상태 전환 
            StopCoroutine(_timer);  //이전 코루틴 정지 _ 새로운 코루틴을 그냥 할당해 버리면 이전 코루틴을 정지시킬 수 없어짐 (아마도?)
            _timer = activate_timer();  //새로운 코루틴 할당 
            StartCoroutine(_timer);
        }
        move_dir = Vector3.zero;
        ready_timer = null;
        SoundManager.get_instance().stop_sound(SoundManager.SoundList.rumble,true);
    }

    //고대병기의 "활성화 유지시간이 끝나며" 일괄처리 _ 퍼즐과 보스움직임에대한 처리를 해준다.
    void deactivate()
    {
        animator.SetBool("activate", false);
        //weapon_light.gameObject.SetActive(false);
        state = State.Deactivated;
        BossRoomManager.get_instance().send_boss_state(Boss_State.State.Soar_Attack, BossRoomManager.get_instance().center);
        BossRoomManager.get_instance().increase_pahse(false);
        BossRoomManager.get_instance().get_ancient_ui().switching_ui(false,0.0f);
        //현재 고대병기가 비활성화되는 때는 완벽히 스위치를 초기화했을 때 이므로 여기서 임의로 카운트를0으로 만들어줌
        activate_torch_count = 0;
        StopCoroutine(_timer);

    }

    //외부 요인으로 인하여 고대병기 비활성화 _ 고대병기에 대한 처리만 해준다.
    void torch_deactivate()
    {
        animator.SetBool("activate", false);
        //외부 요인으로 인해 비활성화됨
        //weapon_light.gameObject.SetActive(false);
        state = State.Deactivated;
        StopCoroutine(_timer);    //타이머가 정상적으로 종료되기 전에 외부요인으로 인해 비활성화 되었으므로 임의로 종료시킨다.
        BossRoomManager.get_instance().get_ancient_ui().switching_ui(false, 0.0f);
    }

    public float get_activate_time()
    {
        return time_list[activate_count];
    }

    public void set_active_count(int _count)
    {
        max_count = _count;
    }

    public State get_state()
    {
        return state;
    }
}
