using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomObservableTorch : ObservableTorch {
    //ObservableTorch는 신호만 보내고.. 얘는 사용가능해 졌을때 보스 공격시킬라고 만듬

    public override void on_light()
    {
        //ObservableTorch의 on_light를 실행하고 보스에게 보스의 상태를 변환시켜주는 함수를 호출!
        base.on_light();
        BossRoomManager.get_instance().send_boss_state(Boss_Worm.Action.Rush_Attack);
    }

}
