using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour {

    private static BossRoomManager instance = null;

    public static BossRoomManager get_instance()
    {
        if (instance == null)
        {
            //FindObjectOfType은 유니티에서 비용이 큰 함수지만 처음 한번만 호출되므로 괜찮음~
            instance = GameObject.FindObjectOfType(typeof(BossRoomManager)) as BossRoomManager;
            if (instance == null)
                Debug.LogError("Singleton Error");
        }

        return instance;
    }

    /// ///////////////////////////////////////////////////
    public Boss_Worm boss;
    public Player player;
    public BasicSwitch [] all_switch;

    //public ObservableTorch[] torch;


    public void send_boss_state(Boss_Worm.Action _action)
    {
        boss.action_ready(_action);
    }

    public void off_switch()
    {
        for(int i =0; i<all_switch.Length; i++)
        {
            all_switch[i].set_switch(false);
            all_switch[i].off_switch_set();
        }
        //모든 스위치를 꺼줌
    }
}
