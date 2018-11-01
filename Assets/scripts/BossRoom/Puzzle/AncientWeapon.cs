using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientWeapon : Observer
{
    //고대병기 
    public enum State
    {
        Activated = 0,   
        Deactivated     
    }

    BossRoomManager manager;
    public Boss_State boss_state;

    public Animator animator;

    int activate_count = 0; 
    [Tooltip("고대병기의 활성화 횟수에 따른 유지시간 지정")]
    public float[] time_list;  
    [Tooltip("고대병기에 할당된 스위치의 최대 개수")]
    public int max_count;
    public int activate_torch_count = 0;

    public State state;
    IEnumerator _timer;
 
    

    void Start()
    {
        manager = BossRoomManager.get_instance();

        _timer = activate_timer();
        state = State.Deactivated;  
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

    void activate()
    {
        activate_count++;
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
    }

    IEnumerator activate_timer()
    {
        manager.send_boss_state(Boss_State.State.Groggy, BossRoomManager.get_instance().center);
        manager.get_ancient_ui().switching_ui(true, time_list[(int)BossRoomManager.get_instance().phase]);
        yield return new WaitForSeconds(time_list[(int)BossRoomManager.get_instance().phase]);
        deactivate();
    }

    public void timer_start()
    {
        StopCoroutine(_timer); 
        _timer = activate_timer();   
        StartCoroutine(_timer);
    }    

    void deactivate()
    {
        animator.SetBool("activate", false);
        state = State.Deactivated;
        manager.send_boss_state(Boss_State.State.Soar_Attack, BossRoomManager.get_instance().center);
        manager.increase_pahse(false);
        manager.get_ancient_ui().switching_ui(false,0.0f);
        for (int i = 0; i < 3; i++)
            manager.reloc.get_reloc((int)manager.phase).torch_set[0].foot_switch[i].ground_move_ctrl(Vector3.down);
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
