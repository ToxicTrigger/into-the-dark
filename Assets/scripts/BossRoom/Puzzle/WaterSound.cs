using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSound : MonoBehaviour
{
    //물을 밟을 때 발동하는 스크립트 소리와 보스 공격 실행이 목적이다.


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BossRoomManager.get_instance().send_boss_state(Boss_Worm.Action.Attack);
        }
    }
}
