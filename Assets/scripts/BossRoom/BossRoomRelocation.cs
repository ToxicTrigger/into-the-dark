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
    ///
    //그대로 스위치가 들어갈지 모르나 일단!
    //hit 스위치에 대한것도 추가해야함

    //횃불과 스위치는 세트로 정해준다.
    [System.Serializable]
    public class Torch_set  //횃불 세트 하나에는 1횃불의 위치와 스위치 위치가 들어간다.
    {
        //public ObservableTorch torch_object;
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
        //public Transform[] switch_position;
        //public BasicSwitch[] switch_object;
        public Torch_set[] torch_set;
        public Transform[] water_position;
        public GameObject[] water_object;
        public GroundCheck[] ground_list;
        //public Transform[] enemy_position;
        public CrumblingPillar[] c_pillar;
    }

    [Tooltip("맵에 스폰할 적 프리팹 넣기")]
    public GameObject enemy;

    [Tooltip("reloc_set 하나에 각 페이즈마다 쓰일 배치가 들어감")]
    public Relocation_set[] reloc_set;

    public BasicSwitch[] hit_switch;

    ArrayList water_list = new ArrayList();

    int time =2;

    //재배치 시작!
    public void relocation(int phase)
    {
        //페이즈가 2 이상이라면 기존 스위치와 횃불을 모두 삭제한다. (phase-1)
        if(phase >= 1) //페이즈2이상
        {
            for (int i = 0; i < reloc_set[phase - 1].torch_set.Length; i++)
            {
                //Destroy(reloc_set[phase - 1].torch_set[i].torch_object.gameObject);

                for(int z= 0; z<reloc_set[phase-1].torch_set[i].switch_object.Length; z++)
                {
                    Destroy(reloc_set[phase - 1].torch_set[i].switch_object[z].gameObject);
                    Destroy(reloc_set[phase - 1].torch_set[i].foot_switch[z].gameObject);
                }
            }
        }

        //페이즈 정보에 따라 새로운 스위치와 횃불을 동적으로 생성한다.
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

                _f_switch.set_ground(_switch);
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
                if (reloc_set[phase].ground_list[i].enemy_count > 8)
                    break;

                BossRoomManager.get_instance().create_enemy(reloc_set[phase].ground_list[i].enemy_position[z].position,
                                                            reloc_set[phase].ground_list[i].gameObject.GetComponent<Observer>());
                reloc_set[phase].ground_list[i].enemy_count++;
            }
        }


        for (int i = 0; i < water_list.Count; i++)
        {
            Destroy((GameObject)water_list[i]);
        }

        water_list.Clear();

        for (int i = 0; i < reloc_set[phase].water_position.Length; i++)
        {
            GameObject _water = (GameObject)Instantiate(reloc_set[phase].water_object[i],
                                                        reloc_set[phase].water_position[i].position, Quaternion.identity);

            water_list.Add(_water);
        }

        if (reloc_set[phase].c_pillar.Length > 0)
        {
            for (int i = 0; i < reloc_set[phase ].c_pillar.Length; i++)
                reloc_set[phase ].c_pillar[i].crumbling_all();
        }

    }

    public Relocation_set get_reloc(int phase)
    {
        return reloc_set[phase];
    }

}
