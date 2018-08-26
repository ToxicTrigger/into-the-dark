using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSelector : MonoBehaviour {

    public TimeSwitch[] all_switch_list;
    public TimeSwitch[] active_switch_list;

    public int[] active_switch_cnt;

    BossRoomManager manager;
    public AncientWeapon ancient_weapon;

    void Start()
    {
        manager = BossRoomManager.get_instance();
        select_switch();
    }

    public void select_switch()
    {
        //해당 페이즈에 사용 할 스위치의 수만큼 num배열 생성
        int [] num = new int [active_switch_cnt[(int)manager.phase]];
        //난수를 임시저장할 변수
        int temp = -1;
        int time = 2;

        for(int a = 0; a < active_switch_list.Length; a++)
        {
            active_switch_list[a].set_use_enable(false);
            active_switch_list[a].set_wait_time(0);
            active_switch_list[a].new_switch_set(0);
        }

        active_switch_list = new TimeSwitch[active_switch_cnt[(int)manager.phase]];

        for (int z = 0; z < active_switch_cnt[(int)manager.phase]; z++)
        {
            num[z] = -1;
        }
        
        for (int i = 0; i < active_switch_cnt[(int)manager.phase]; i++)
        {
            temp = Random.Range(0, all_switch_list.Length);

            for (int z = 0; z < active_switch_cnt[(int)manager.phase]; z++)
            {
                if (num[z] == temp)
                    break;

                if(z == active_switch_cnt[(int)manager.phase]-1)
                    num[i] = temp;
            }

            //만약 i번째에 num에 새로운 수가 할당되지 않았다면 i번째를 다시 실행하기 위해 i를 하나 뺌
            if (num[i] == -1) --i;
            else
            {
                Debug.Log(num[i]);
                active_switch_list[i] = all_switch_list[num[i]];
                active_switch_list[i].set_use_enable(true);
                active_switch_list[i].set_wait_time(time);
                time++;//2,3,4,5,6으로 1씩 증가하기 때문에  
                active_switch_list[i].new_switch_set(active_switch_cnt[(int)manager.phase]-1);
            }
        }

        for(int i =0; i< active_switch_list.Length; i++)
        {
            int cnt = 0;

            for (int z = 0; z < active_switch_cnt[(int)manager.phase] - 1; z++)
            {
                if (cnt == i)
                {
                    cnt++;
                    z--;
                }
                else
                {
                    active_switch_list[i].set_switch_set(z, active_switch_list[cnt]);
                    cnt++;
                }
            }
        }

        ancient_weapon.set_active_count(active_switch_cnt[(int)manager.phase]);

    }

    public TimeSwitch get_active_switch_list(int _switch_num)
    {
        return active_switch_list[_switch_num];
    }

    public int get_active_switch_cnt(int cnt)
    {
        return active_switch_cnt[cnt];
    }

}
