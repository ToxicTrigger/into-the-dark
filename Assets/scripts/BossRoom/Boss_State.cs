using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_State : MonoBehaviour
{
    public enum State
    {
        Idle = 0,   //대기상태 (플레이어 주위를 뱅글뱅글 돈다.)
        Sleep,       //지상 이동 전 둥지에 잠들어있는 상태
        Move,       //지상 이동
        Rush_Attack,    //포물선 공격
        Whipping_Attack,//찌르기
        Soar_Attack,    //솟아오르는 공격
        Cross_Attack,   //가로지르는 공격 (웅덩이공격)
        Groggy,         //그로기 상태
        Groggy_End,   //그로기 끝난 상태
        Death,            //죽음
        Ready,           //준비상태 모든 상태가 되기 전 거치는 상태임
        Up
    }

    State back_state;
    public State state;
    Boss_Action boss_action;
    BossRoomManager manager;

    void Start()
    {
        manager = BossRoomManager.get_instance();
        boss_action = this.GetComponent<Boss_Action>();
    }

    public State get_state()
    {
        return state;
    }

    public void set_state(State _state)
    {
        state = _state;
        this.GetComponent<Boss_Action>().action_phase = 1;

        if (_state == State.Idle)
            boss_action.set_idle_state(true);
        else boss_action.set_idle_state(false);

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

        if (_state != State.Move && _state != State.Rush_Attack)
            manager.set_field_info(SendCollisionMessage.Field.NULL);

        back_state = state;
        //상태가 바뀔 때 항상 action_phase를 1로 만W들어준다.
    }

}
