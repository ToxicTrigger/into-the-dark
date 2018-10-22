using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

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
    public bool is_new;
    public bool is_danger_loop;
    public float add_smoothness, add_color_red;
    public float danger_speed;
    public PostProcessingProfile post;
    public VignetteModel new_setting;
    public VignetteModel default_setting;
    IEnumerator danger_timer;

    public Boss_Worm boss;
    Boss_State boss_state;
    Boss_Action boss_action;
    public Player player;
    public CharacterController p_controller;
    SoundManager sound_manager;
    Vector3 cross_point = Vector3.zero;
    public Transform start_point;
    public AncientWeapon ancient_weapon;

    public GameObject enemy;
    public DestroyCheck destroy_check;

    public GroundCheck center;

    public CutScenePlay cut_scene;

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
    public int wood_bridge_count =3;

    public bool is_game_over;

    public GameObject tuto_enemy;
    public Transform tuto_enemy_pos;

    public GameObject hp_heal;

    [System.Serializable]
    public struct ItemPos
    {
        public Transform[] item_pos;
    }
    public ItemPos[] item_pos_list;

    public Particle_Handler[] fog;


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

        player = FindObjectOfType<Player>();
        p_controller = player.GetComponent<CharacterController>();
    }
    private void Start()
    {
        default_setting.settings = post.vignette.settings;
        //post.vignette.settings = new_setting.settings;
        //danger_screen(true);
    }


    //플레이어가 보스룸에 입장하면 호출하는 함수
    public void player_enter_bossroom()
    {
        phase = Phase.one;
        Map_Initialization();
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
        player.all_collect_item();

        for (int i = 0; i < reloc.get_reloc((int)phase).torch_set[0].foot_switch.Length; i++)
        {
            reloc.get_reloc((int)phase).torch_set[0].switch_object[i].set_switch(false);
            if (_add)
            {
                Destroy(reloc.get_reloc((int)phase).torch_set[0].foot_switch[i].gameObject);
                Destroy(reloc.get_reloc((int)phase).torch_set[0].switch_object[i].gameObject);
            }
        }
        if (_add)
        {
            //페이즈 증가
            phase++;  
            Map_Initialization();
        }

    }

    public void game_over()
    {
        if (!is_game_over)
        {
            p_controller.enabled = false;
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
            p_controller.enabled = false;
            ui_black_screen.change_screen(BlackScreen.ScreenState.Fade_Out);
        }
    }

    public void game_clear()
    {
        sound_manager.clear();
        for (int i = 0; i < fog.Length; i++)
        {
            fog[i].OnOff = false;
        }
        for (int i = 0; i < enemy_list.Count; i++)
        {
            Destroy(enemy_list[i].gameObject);
        }
        enemy_list.Clear();
        p_controller.enabled = false;
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
                p_controller.enabled = true;
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
    public void send_boss_state(Boss_State.State _state, GroundCheck _gameobj, float _dis, float _height)
    {
        boss_state.set_state(_state, _gameobj, _dis, _height);
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

    public void play_cut_scene()
    {
        cut_scene.play_scene();
    }

    public void minus_wood_bridge_count()
    {
        wood_bridge_count--;
        if(wood_bridge_count <=1)
        {
            game_over();
        }
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
        wood_bridge_count = 3;

        for (int i=0; i<wood_bridge.Length; i++)
        {
            wood_bridge[i].initialize_bridge();
        }
        for (int i = 0; i < pillar_list.Length; i++)
        {
            pillar_list[i].init_floor();
        }
        Map_Initialization();
        player.all_collect_item();
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

    public void drop_item()
    {
        for(int i=0; i<item_pos_list[boss_action.groggy_cnt].item_pos.Length; i++)
        {
            GameObject _item = Instantiate(hp_heal, item_pos_list[boss_action.groggy_cnt].item_pos[i].position, Quaternion.identity);
        }        
    }

    GameObject cur_obj;
    public void danger_screen(bool _is_danger, GameObject obj)
    {
        if (_is_danger && danger_timer == null)
        {
            cur_obj = obj;
            is_danger_loop = true;
            danger_timer = danger_loop();
            StartCoroutine(danger_timer);
        }
        else if(_is_danger == false && cur_obj == obj)
        {
            is_danger_loop = false;
        }
    }


    IEnumerator danger_loop()
    {
        float smoothness = default_setting.settings.smoothness;
        Vector4 color = default_setting.settings.color;

        while (is_danger_loop)
        {
            while (true)
            {
                if (!is_new)
                {
                    smoothness += add_smoothness;
                    if (color.x >= new_setting.settings.color.r)
                        color.x = new_setting.settings.color.r;
                    else
                        color.x += add_color_red;

                    post.vignette.set_val(smoothness, color);

                    if (smoothness >= new_setting.settings.smoothness && color.x >= new_setting.settings.color.r)
                    {
                        is_new = true;
                        break;
                    }
                }
                else
                {
                    smoothness -= add_smoothness;
                    if(color.x >0)
                        color.x -= add_color_red;
                    
                    post.vignette.set_val(smoothness, color);

                    if (smoothness <= default_setting.settings.smoothness && color.x <= default_setting.settings.color.r)
                    {
                        is_new = false;
                        break;
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(danger_speed);
        }

        while (true)
        {
            smoothness -= add_smoothness;
            color.x -= add_color_red;

            post.vignette.set_val(smoothness, color);

            if (smoothness <= default_setting.settings.smoothness && color.x <= default_setting.settings.color.r)
            {
                is_new = false;
                post.vignette.settings = default_setting.settings;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        danger_timer = null;
    }
}
