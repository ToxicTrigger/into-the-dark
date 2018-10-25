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
        state = State.Deactivated;  //초기 상태는 비활성화된 상태
    }

    void Update()
    {
    }

    public override void notify(Observable observable)
    {
        BasicSwitch torch = observable as BasicSwitch;

        if (torch.get_switch())
        {
            if(activate_torch_count <max_count ) activate_torch_count++;

            if (activate_torch_count >= max_count)
            {
                activate();
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
                deactivate();
            }
        }
    }

    public int tick;
    public float power;
    public float tick_by_tick_time;

    //활성화 시키는 함수W
    void activate()
    {
        activate_count++;
        //최초 활성화 연출
        if (activate_count == 1)
        {
            manager.play_event(BossRoomManager.EventList.AncientWeapon);
        }
        animator.SetBool("activate", true);
        manager.drop_item();

        state = State.Activated;

        manager.player.ac.Shake(tick, power, tick_by_tick_time * Time.deltaTime);

        for (int i = 0; i < 3; i++)
            manager.reloc.get_reloc((int)manager.phase).torch_set[0].foot_switch[i].ground_move_ctrl(Vector3.up);
        //이걸 애니메이션 재생이 완료 된 후 실행하자!
        //timer_start();
    }

    //활성화 타이머
    IEnumerator activate_timer()
    {
        manager.send_boss_state(Boss_State.State.Groggy, BossRoomManager.get_instance().center); //weapon_activation() : 보스 그로기상태 전환 
        manager.get_ancient_ui().switching_ui(true, time_list[(int)BossRoomManager.get_instance().phase]);
        yield return new WaitForSeconds(time_list[(int)BossRoomManager.get_instance().phase]);
        deactivate();
    }

    public void timer_start()
    {
        StopCoroutine(_timer);  //이전 코루틴 정지 _ 새로운 코루틴을 그냥 할당해 버리면 이전 코루틴을 정지시킬 수 없어짐 (아마도?)
        _timer = activate_timer();  //새로운 코루틴 할당 
        StartCoroutine(_timer);
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
        }
        move_dir = Vector3.zero;
        ready_timer = null;
        SoundManager.get_instance().stop_sound(SoundManager.SoundList.rumble,true);
    }

    //고대병기의 "활성화 유지시간이 끝나며" 일괄처리 _ 퍼즐과 보스움직임에대한 처리를 해준다.
    void deactivate()
    {
        animator.SetBool("activate", false);
        state = State.Deactivated;
        manager.send_boss_state(Boss_State.State.Soar_Attack, BossRoomManager.get_instance().center);
        manager.increase_pahse(false);
        manager.get_ancient_ui().switching_ui(false,0.0f);
        for (int i = 0; i < 3; i++)
            manager.reloc.get_reloc((int)manager.phase).torch_set[0].foot_switch[i].ground_move_ctrl(Vector3.down);
        //현재 고대병기가 비활성화되는 때는 완벽히 스위치를 초기화했을 때 이므로 여기서 임의로 카운트를0으로 만들어줌
        activate_torch_count = 0;
        StopCoroutine(_timer);

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
