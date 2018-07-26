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

    public State state;

    public State get_state()
    {
        return state;
    }

    public void set_state(State _state)
    {
        state = _state;
    }

}
