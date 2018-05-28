using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbSwitch : BasicSwitch {
    //방해 스위치 (유도 스위치) : 다른 스위치의 동작을 방해하므로 방해 스위치가 더 기능을 떠올리기 쉬울것 같아서 스크립트이름은 일케지음

    public DisturbModel this_model;    //해당 스위치의 모델링(부숴졌을때 모델링에만 변화를 줌)

    public bool destroy_switch; //해당 스위치가 파괴되었는가



    private void Start()
    {
        destroy_switch = false; //파괴되지 않음
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Boss"))
        {
            BossStemWorm boss = other.gameObject.GetComponent<BossStemWorm>();

            //솟아오르기 공격에만 해당되나 테스트를 위해 RushAttack에도 적용함
            if((boss.get_action_state() == BossStemWorm.Action.Move_Up 
                || boss.get_action_state() == BossStemWorm.Action.Move_Attack)&&!destroy_switch)
            {
                Debug.Log("충돌! = " + this.name);
                this_model.switch_destroy();
                destroy_switch = true;
                set_switch(true);
            }
        }
    }

    //새로운 게임 (플레이어가 처음부터 공략해야하는 상태) 초기화
    public override void off_switch_set()
    {
        //모델을 켜준다.
        this_model.gameObject.SetActive(true);
        destroy_switch = false; //안부숴짐 
    }

}
