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
    [Tooltip("재배치 하는 스크립트와 총 스위치 개수를 통일할 것, 반드시 시간 스위치부터 입력할 것")]
    public BasicSwitch [] all_switch;

    //public ObservableTorch[] torch;

    void Start()
    {
        Debug.Log("start");
        set_switch_pos(); 
    }

    public void send_boss_state(Boss_Worm.Action _action)
    {
        if(boss != null)boss.action_ready(_action);
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
    
    public void send_attack_count_ui(int _time)
    {
        UIManager.get_instance().play_attack_timer(_time);
    }

    public void set_switch_pos()
    {
        Debug.Log("set_switch_pos");
        int[] random_Array = new int[all_switch.Length];
        int num = -1;

        for(int i =0; i < random_Array.Length; i++)
        {
            random_Array[i] = num;
        }

        for (int i = 0; i < all_switch.Length; i++)
        {
            if(i <= 3)
            {
                num = Random.Range(0, 4);

            }

            if(i >= 4)
            {
                num = Random.Range(4, all_switch.Length);
            }

            for (int z = 0; z < random_Array.Length ; z++)
            {
                if (random_Array[z] == num)
                {
                    --i;
                    break;
                }
                if(z == random_Array.Length - 1)
                {
                    random_Array[i] = num;
                }
            }

            all_switch[i].transform.position =
                BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].switch_position[random_Array[i]].transform.position;
            
        }
        BossRoomRelocation.get_instance().togle_set();
    }

}
