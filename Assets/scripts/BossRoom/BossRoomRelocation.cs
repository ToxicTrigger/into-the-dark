using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomRelocation : MonoBehaviour {

    private static BossRoomRelocation instance = null;

    public static BossRoomRelocation get_instance()
    {
        if (instance == null)
        {
            //FindObjectOfType은 유니티에서 비용이 큰 함수지만 처음 한번만 호출되므로 괜찮음~
            instance = GameObject.FindObjectOfType(typeof(BossRoomRelocation)) as BossRoomRelocation;
            if (instance == null)
                Debug.LogError("Singleton Error");
        }

        return instance;
    }

    /// ///////////////////////////////////////////////////

    [System.Serializable]
    public class Torch_set 
    {
        public ObserverTorch torch_object;
        public Transform torch_position;  
        public TimeSwitch[] switch_object;
        public Transform[] switch_position;
        public int[] switch_time;
        public FootSwitch[] foot_switch;
        public Transform[] foot_switch_position;
        public DefaultBridge[] bridge;
    }
    public ObserverTorch torch;
    public TimeSwitch Time_switch;
    public FootSwitch Foot_switch;

    [System.Serializable]
    public class Relocation_set
    {
        public Torch_set[] torch_set;
        public Transform[] water_position;
        public GameObject[] water_object;
        public GroundCheck[] ground_list;
        public CrumblingPillar[] c_pillar;
    }

    [Tooltip("맵에 스폰할 적 프리팹 넣기")]
    public GameObject enemy;

    [Tooltip("reloc_set 하나에 각 페이즈마다 쓰일 배치가 들어감")]
    public Relocation_set[] reloc_set;    

    public bool []switch_fog;
    public GameObject s_fog;

    //재배치 시작!
    public void relocation(int phase)
    {
        for(int i=0; i<reloc_set[phase].torch_set.Length; i++)
        {
            for (int z = 0; z < reloc_set[phase].torch_set[i].switch_object.Length; z++)
            {
                TimeSwitch _switch = (TimeSwitch)Instantiate(Time_switch, reloc_set[phase].torch_set[i].switch_position[z].position, Quaternion.identity);
                FootSwitch _f_switch = (FootSwitch)Instantiate(Foot_switch, reloc_set[phase].torch_set[i].foot_switch_position[z].position, Quaternion.identity);

                reloc_set[phase].torch_set[i].switch_object[z] = _switch;
                reloc_set[phase].torch_set[i].foot_switch[z] = _f_switch;

                _switch.set_wait_time(reloc_set[phase].torch_set[i].switch_time[z]);
                _switch.set_use_enable(true);
                _switch.add_observer(BossRoomManager.get_instance().get_ancient_weapon());
                _switch.new_switch_set(reloc_set[phase].torch_set[i].switch_object.Length - 1);
                _switch.set_foot_switch(_f_switch);

                _f_switch.set_ground(_switch.gameObject);

                if(phase == 1 && switch_fog[z] == true)
                {
                    GameObject _fog = (GameObject)Instantiate(s_fog, _switch.gameObject.transform.position, Quaternion.identity, _switch.gameObject.transform);
                    _fog.transform.localPosition = new Vector3(0, -1, 0);
                    _fog.SetActive(true);
                }

            }


            for(int x=0; x < reloc_set[phase].torch_set[i].switch_object.Length; x++)
            {
                int cnt = 0;

                for (int v = 0; v < reloc_set[phase].torch_set[i].switch_object.Length-1; v++)
                {
                    if(cnt == x)
                    {
                        cnt++;
                        v--;
                    }
                    else
                    {
                        reloc_set[phase].torch_set[i].switch_object[x].set_switch_set(v,
                            reloc_set[phase].torch_set[i].switch_object[cnt]);
                        cnt++;
                    }
                }
            }
            //고대병기 클리어 조건을 스위치 개수로함
            BossRoomManager.get_instance().get_ancient_weapon().set_active_count(reloc_set[phase].torch_set[i].switch_object.Length);
        }

        for(int i =0; i<reloc_set[phase].ground_list.Length; i++)
        {
            for (int z = 0; z < 4; z++)
            {
                if (reloc_set[phase].ground_list[i].enemy_count >= 16)
                {
                    break;
                }

                BossRoomManager.get_instance().create_enemy(reloc_set[phase].ground_list[i].enemy_position[z].position,
                                                            reloc_set[phase].ground_list[i].gameObject.GetComponent<Observer>());
                reloc_set[phase].ground_list[i].enemy_count++;
            }
        }

        if (reloc_set[phase].c_pillar.Length > 0)
        {
            for (int i = 0; i < reloc_set[phase].c_pillar.Length; i++)
            {
                reloc_set[phase].c_pillar[i].crumbling_all();
            }
        }

    }

    public Relocation_set get_reloc(int phase)
    {
        return reloc_set[phase];
    }

}
