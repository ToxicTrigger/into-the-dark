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
    /// 

    BossRoomRelocation reloc;
    public Boss_Worm boss;

    public enum Phase
    {
        one,
        two,
        three
    }

    public Phase phase;

    void Start()
    {
        reloc = GetComponent<BossRoomRelocation>();
    }

    //플레이어가 보스룸에 입장하면 호출하는 함수
    public void player_enter_bossroom()
    {
        //아무것도 하지 않고있던 보스를 Idle상태로 변화시킨다.
        //페이즈를 1로 변화시킨다. 
        //페이즈1에 해당하는 정보로 맵을 초기화시킨다.
        phase = Phase.one;
        Map_Initialization();
        
    }

    //페이즈 정보에 따라 맵을 초기화한다.
    public void Map_Initialization()
    {
        //스위치, 기둥, 다리등의 초기화 리로케이션 클래스에서 함수를 실행
        reloc.relocation((int)phase);
    }

    //페이즈 증가 함수 (페이즈 증가 -> 새로운 페이즈 시작)
    public void increase_pahse()
    {
        //페이즈 증가
        phase = (Phase)((int)phase++);  //이런식으로 쓰는게 옳은가?

        //페이즈 증가에 따른 스위치 끄기

        for (int i = 0; i < reloc.get_reloc((int)phase).switch_object.Length; i++)
        {
            reloc.get_reloc((int)phase).switch_object[i].set_switch(false);
            reloc.get_reloc((int)phase).switch_object[i].off_switch_set();
        }

        //hit스위치 끄기 (다리 내리기) 추가

        for (int i = 0; i < reloc.get_reloc((int)phase).switch_object.Length; i++)
        {
            reloc.hit_switch[i].set_switch(false);
            reloc.hit_switch[i].off_switch_set();
        }

    }








    //public Boss_Worm boss;
    //public Player player;
    //[Tooltip("재배치 하는 스크립트와 총 스위치 개수를 통일할 것, 반드시 시간 스위치부터 입력할 것")]
    //public BasicSwitch [] all_switch;
    //public BasicSwitch[] hit_switch;
    //public GameObject enemy;

    //public MeshRenderer[] alpha_ctrl_obj;

    //[System.Serializable]
    //public class PhasePillarList
    //{
    //    public CrumblingPillar[] c_pillar;
    //}
    //public PhasePillarList []phase_pillar_list;

    //int boss_phase =1;

    //ArrayList water_list = new ArrayList();

    ////public ObservableTorch[] torch;

    //void Start()
    //{
    //    all_switch[3].set_switch(true);
    //    set_switch_pos(); 
    //}

    //public void send_boss_state(Boss_Worm.Action _action)
    //{
    //    if (boss != null)
    //    {
    //        if (_action == Boss_Worm.Action.Attack)
    //        {
    //            if (boss.get_edge_attack())
    //                boss.action_ready(Boss_Worm.Action.Whipping_Attack);
    //            else
    //                boss.action_ready(Boss_Worm.Action.Rush_Attack);
    //        }
    //        else
    //        {
    //            boss.action_ready(_action);
    //        }
    //    }
    //}

    //public void off_switch()
    //{
    //    for(int i =0; i<all_switch.Length; i++)
    //    {
    //        all_switch[i].set_switch(false);
    //        all_switch[i].off_switch_set();
    //    }
    //    for (int i = 0; i < hit_switch.Length; i++)
    //    {
    //        hit_switch[i].set_switch(false);
    //        hit_switch[i].off_switch_set();
    //    }
    //    //모든 스위치를 꺼줌
    //}
    
    ////등록된 모든 기둥을 무너뜨린다.
    //public void crumbling_pillar_all()
    //{
    //    if (phase_pillar_list.Length > 0)
    //    {
    //        for (int i = 0; i < phase_pillar_list[boss_phase - 1].c_pillar.Length; i++)
    //            phase_pillar_list[boss_phase - 1].c_pillar[i].crumbling_all();
    //    }
    //}

    //public void set_switch_pos()
    //{
    //    int[] random_Array = new int[BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].switch_position.Length];
    //    int num = -1;

    //    for(int i =0; i < random_Array.Length; i++)
    //    {
    //        random_Array[i] = num;
    //    }

    //    for (int i = 0; i < random_Array.Length; i++)
    //    {
    //        //if(i <= 3)
    //        //{
    //        //    num = Random.Range(0, 3);   //2번째 

    //        //}

    //        //if(i >= 4)
    //        //{
    //        //    num = Random.Range(4, all_switch.Length);
    //        //}

    //        num = Random.Range(0, BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].switch_position.Length);

    //        for (int z = 0; z < random_Array.Length ; z++)
    //        {
    //            if (random_Array[z] == num)
    //            {
    //                --i;
    //                break;
    //            }
    //            if(z == random_Array.Length - 1)
    //            {
    //                random_Array[i] = num;
    //            }
    //        }

    //        all_switch[i].transform.position =
    //            BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].switch_position[random_Array[i]].transform.position;
            
    //    }


    //    for (int i =0; i< BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].enemy_position.Length; i++)
    //    {
    //        GameObject _enemy = (GameObject)Instantiate(enemy, BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].enemy_position[i].position, Quaternion.identity);
    //    }

    //    for(int i=0; i<water_list.Count; i++)
    //    {
    //        Destroy((GameObject)water_list[i]);
    //    }

    //    water_list.Clear();

    //    for(int i = 0; i< BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].water_position.Length; i++)
    //    {
    //        GameObject _water = (GameObject)Instantiate(BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].water_object[i], 
    //                                                    BossRoomRelocation.get_instance().reloc_set[(int)BossRoomRelocation.get_instance().current_turn].water_position[i].position, Quaternion.identity);

    //        water_list.Add(_water);
    //    }

    //    BossRoomRelocation.get_instance().togle_set();

    //    //for(int i =0; i<move_obj.Length; i++)
    //    //{
    //    //    StartCoroutine(move_obj[i].timer());
    //    //}
    //}

    //public void add_phase()
    //{
    //    boss_phase++;
    //}

    //private void Update()
    //{
    //    if (player_pos_check)
    //    {
    //        //일단 매니저에서 등록된 오브젝트의 위치와 비교
    //        for (int i = 0; i < alpha_ctrl_obj.Length; i++)
    //        {
    //            alpha_ctrl_obj[i].material.color =
    //                player.transform.position.z >= alpha_ctrl_obj[i].gameObject.transform.position.z ? new Color(alpha_ctrl_obj[i].material.color.r, alpha_ctrl_obj[i].material.color.g, alpha_ctrl_obj[i].material.color.b, 0.5f) :
    //                                                           new Color(alpha_ctrl_obj[i].material.color.r, alpha_ctrl_obj[i].material.color.g, alpha_ctrl_obj[i].material.color.b, 1.0f);

    //        }
    //    }
    //}

    //public bool player_pos_check;
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        player_pos_check = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        player_pos_check = false;
    //    }
    //}
}
