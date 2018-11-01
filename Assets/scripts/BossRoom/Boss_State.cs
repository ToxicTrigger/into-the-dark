using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_State : MonoBehaviour
{
    public enum State
    {
        Idle = 0,   
        Soar_Attack,    
        Cross_Attack,  
        Groggy,        
        Death,            
    }

    State back_state;
    public State state;
    Boss_Action boss_action;
    BossRoomManager manager;
    GroundCheck soar_target;

    void Start()
    {
        manager = BossRoomManager.get_instance();
        boss_action = this.GetComponent<Boss_Action>();
    }

    public State get_state()
    {
        return state;
    }

    public void set_state(State _state, GroundCheck _soar_target)
    {
        state = _state;
        soar_target = _soar_target;

        this.GetComponent<Boss_Action>().action_phase = 1;

        if (_state == State.Groggy)
        {
            manager.get_hp_ui().switching_ui(true);
            manager.get_groggy_ui().set_boss_groggy(true,boss_action.get_groggy_point());
        }
        else if (back_state == State.Groggy)
        {
            manager.get_hp_ui().switching_ui(false);
            manager.get_groggy_ui().set_boss_groggy(false, Vector3.zero);
        }

        if(_state == State.Soar_Attack || _state == State.Cross_Attack)
        {
            boss_action.set_soar_target(soar_target);
        }

        back_state = state;
    }

    public void set_state(State _state, GroundCheck _soar_target, float _dis, float _height )
    {
        state = _state;
        soar_target = _soar_target;

        this.GetComponent<Boss_Action>().action_phase = 1;

        if (_state == State.Groggy)
        {
            manager.get_hp_ui().switching_ui(true);
            manager.get_groggy_ui().set_boss_groggy(true, boss_action.get_groggy_point());
        }
        else if (back_state == State.Groggy)
        {
            manager.get_hp_ui().switching_ui(false);
            manager.get_groggy_ui().set_boss_groggy(false, Vector3.zero);
        }

        if (_state == State.Soar_Attack || _state == State.Cross_Attack)
        {
            boss_action.set_soar_target(soar_target,_dis, _height);
        }

        back_state = state;
    }
}
