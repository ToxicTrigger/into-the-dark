using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : Observer {  

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

    //BossRoomRelocation reloc;
    public Boss_Worm boss;
    Boss_State boss_state;
    Boss_Action boss_action;
    public Player player;
    SoundManager sound_manager;
    Vector3 cross_point = Vector3.zero;
    public Transform start_point;
    public AncientWeapon ancient_weapon;

    public GameObject enemy;
    public DestroyCheck destroy_check;

    public GroundCheck center;

    public enum Phase
    {
        one,
        two,
        three
    }

    public Phase phase;
    public SendCollisionMessage.Field field;
    public GameObject player_coll;
    public BossRoomRelocation reloc;
    public BossHpUI boss_hp_ui;
    public UiGroggyPoint boss_groggy_ui;
    public AWTimerUI ancient_timer_ui;
    public BlackScreen ui_black_screen;

    public TimeSelector time_selector;

    public AudioSource sound;
    public AudioSource idle_sound;
    public AudioSource[] back_sound;

    public List<GameObject> enemy_list;

    [System.Serializable]
    public struct InitialValue
    {
        public int boss_hp;
        public Phase phase;
    };
    public InitialValue[] init_val;

    public GroundCheck []wood_bridge;
    public CrumblingPillar[] pillar_list;
    public bool is_entrance;
    public bool is_puzzle_clear;
    public bool is_stage_clear;

    public bool is_game_over;

    public GameObject tuto_enemy;
    public Transform tuto_enemy_pos;

    public enum EventList
    {
        Entrance,       // 입장
        AncientWeapon,  // 고대병기 활성화
        Clear,          // 클리어
    }
    public BossRoomEvent[] event_slot;

    void Awake()
    {
        init_val[(int)phase].phase = Phase.one;
        phase = Phase.one;
        init_val[(int)phase].boss_hp = get_boss().get_max_hp();

        field = SendCollisionMessage.Field.NULL;
        boss_state = boss.gameObject.GetComponent<Boss_State>();
        boss_action = boss.gameObject.GetComponent<Boss_Action>();

        player_enter_bossroom();
        sound_manager = SoundManager.get_instance();

        GameObject _tuto_enemy = (GameObject)Instantiate(enemy, tuto_enemy_pos.position, Quaternion.identity, this.transform);
        DestroyCheck _destroy_check = (DestroyCheck)Instantiate(destroy_check,
                                        Vector3.zero , Quaternion.identity, _tuto_enemy.transform);
        _destroy_check.add_observer(this);

    }

    //플레이어가 보스룸에 입장하면 호출하는 함수
    public void player_enter_bossroom()
    {
        phase = Phase.one;
        Map_Initialization();
        if(!is_entrance)
        {
            //입장 연출 추가
        }
    }

    //페이즈 정보에 따라 맵을 초기화한다.
    public void Map_Initialization()
    {
        //스위치, 기둥, 다리등의 초기화 리로케이션 클래스에서 함수를 실행
        BossRoomRelocation.get_instance().relocation((int)phase);

    }

    //페이즈 증가 함수 (페이즈 증가 -> 새로운 페이즈 시작)
    public void increase_pahse(bool _add)
    {
        //페이즈 증가에 따른 스위치 끄기
        for (int i = 0; i < reloc.get_reloc((int)phase).torch_set[0].foot_switch.Length; i++)
        {
            reloc.get_reloc((int)phase).torch_set[0].switch_object[i].set_switch(false);
            Destroy(reloc.get_reloc((int)phase).torch_set[0].foot_switch[i].gameObject);
            Destroy(reloc.get_reloc((int)phase).torch_set[0].switch_object[i].gameObject);
        }
        if (_add)   //페이즈가 넘어가지 않고 스위치만 초기화되는 경우가 있으므로...
        {
            //페이즈 증가
            phase++;  
            Map_Initialization();
            //time_selector.select_switch();
        }

    }

    public void game_over()
    {
        if (!is_game_over)
        {
            is_game_over = true;
            ui_black_screen.add_observer(this);
            ui_black_screen.change_screen(BlackScreen.ScreenState.Fade_Out);
        }
    }

    public void game_over(GroundCheck _this)
    {
        if (!is_game_over)
        {
            is_game_over = true;
            ui_black_screen.add_observer(this);
            ui_black_screen.add_observer(_this);
            ui_black_screen.change_screen(BlackScreen.ScreenState.Fade_Out);
        }
    }

    public override void notify(Observable observable)
    {
        if (observable.gameObject.GetComponent<BlackScreen>())
        {
            BlackScreen torch = observable as BlackScreen;
            
            if (torch.get_screen_state() == BlackScreen.ScreenState.Fade_Out)
            {
                //플ㄹ레이어 사망, 맵 재시작
                init_bossroom();
                ui_black_screen.change_screen(BlackScreen.ScreenState.Fade_In);
                is_game_over = false;
            }
        }
        if(observable.gameObject.GetComponent<DestroyCheck>())
        {
            GameObject _tuto_enemy = (GameObject)Instantiate(enemy, tuto_enemy_pos.position, Quaternion.identity);
            DestroyCheck _destroy_check = (DestroyCheck)Instantiate(destroy_check,
                                            Vector3.zero, Quaternion.identity, _tuto_enemy.transform);
            _destroy_check.add_observer(this);
        }
    }

    public void send_boss_state(Boss_State.State _state, GroundCheck _gameobj)
    {
        boss_state.set_state(_state, _gameobj);
    }

    public void set_cross_point(Vector3 _pos)
    {
        cross_point = _pos;
    }

    public Vector3 get_cross_point()
    {
        return cross_point;
    }

    public Boss_Worm get_boss()
    {
        return boss;
    }

    public Boss_State.State get_boss_state()
    {
        return boss_state.get_state();
    }

    public void set_hp_ui(BossHpUI hp_ui)
    {
        boss_hp_ui = hp_ui;
    }

    public BossHpUI get_hp_ui()
    {
        return boss_hp_ui;
    }

    public void set_groggy_ui(UiGroggyPoint _ui)
    {
        boss_groggy_ui = _ui;
    }

    public UiGroggyPoint get_groggy_ui()
    {
        return boss_groggy_ui;
    }

    public Vector3 get_groggy_point()
    {
        return boss_action.get_groggy_point();
    }

    public AWTimerUI get_ancient_ui()
    {
        return ancient_timer_ui;
    }

    public void set_ancient_ui(AWTimerUI _ui)
    {
        ancient_timer_ui = _ui;
    }

    public AncientWeapon get_ancient_weapon()
    {
        return ancient_weapon;
    }

    public bool get_is_puzzle_clear()
    {
        return is_puzzle_clear;
    }

    public void set_is_puzzle_clear(bool _is_clear)
    {
        is_puzzle_clear = _is_clear;
    }

    public void init_bossroom()
    {
        for (int i = 0; i < reloc.get_reloc((int)phase).torch_set[0].foot_switch.Length; i++)
        {
            reloc.get_reloc((int)phase).torch_set[0].switch_object[i].set_switch(false);
            Destroy(reloc.get_reloc((int)phase).torch_set[0].foot_switch[i].gameObject);
            Destroy(reloc.get_reloc((int)phase).torch_set[0].switch_object[i].gameObject);
        }

        for(int i=0; i<enemy_list.Count;  i++)
        {
            Destroy(enemy_list[i]);
        }
        enemy_list.Clear();
        get_boss().set_hp(init_val[(int)phase].boss_hp);
        //boss_state.set_state(Boss_State.State.Idle,null);
        //phase = init_val[(int)phase].phase;        

        for(int i=0; i<wood_bridge.Length; i++)
        {
            wood_bridge[i].initialize_bridge();
        }
        for (int i = 0; i < pillar_list.Length; i++)
        {
            pillar_list[i].init_floor();
        }
        Map_Initialization();
        player.transform.position = start_point.position;
        player.gameObject.GetComponent<Damageable>().Hp = player.gameObject.GetComponent<Damageable>().Max_Hp;
    }

    public void create_enemy(Vector3 _pos, Observer _observer)
    {
        GameObject _enemy = (GameObject)Instantiate(enemy,
                                                _pos, Quaternion.identity);
        DestroyCheck _destroy_check = (DestroyCheck)Instantiate(destroy_check, 
                                                _pos, Quaternion.identity);
        _destroy_check.transform.SetParent(_enemy.transform);
        _destroy_check.add_observer(_observer);

        enemy_list.Add(_enemy);
    }

    public void play_event(EventList list)
    {
        switch (list)
        {
            case EventList.Entrance:
                event_slot[(int)EventList.Entrance].play_event();
                break;
            case EventList.AncientWeapon:
                event_slot[(int)EventList.AncientWeapon].play_event();
                break;
            case EventList.Clear:
                event_slot[(int)EventList.Clear].play_event();
                break;
            default:
                break;
        }
    }

    //public void set_field_info(SendCollisionMessage.Field _field)
    //{
    //    field = _field;

    //    for(int i =0; i< back_sound.Length; i++)
    //    {
    //        if (i == (int)field)
    //            back_sound[i].mute = false;
    //        else
    //            back_sound[i].mute = true;
    //    }
    //}

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
