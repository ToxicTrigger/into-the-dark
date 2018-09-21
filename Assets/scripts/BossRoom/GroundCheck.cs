using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : Observer {
    //플레이어가 땅에 있는지에 대한 체크

    public enum Type
    {
        Stone,
        Wood,
        Null
    };
    public Type type;
    public float cognition_time;
    public float tick_count;
    public bool is_ground;
    InputManager input_manager;
    SoundManager sound_manager;
    BossRoomManager manager;
    public Boss_State boss_state;
    public UiStateText ui_state_text;
    public BlackScreen ui_black_screen;
    public AudioSource step_sound;
    int step_cnt;
    float on_ground_time;
    bool is_cognition;  //보스의 인식
    public float sound_delay;
    public float playing_time;
    bool is_play;

    bool cognition_text;
    bool attack_ready_text;

    public bool is_danger;
    public GameObject player;

    public Transform [] enemy_position;

    public int enemy_count;

    [Tooltip("몇걸음부터 인식할지")]
    public float cognition_step_count;
    [Tooltip("인식 후 보스의 상태 변화 시간")]
    public int [] cognition_time_list;

    public Rigidbody []wood_bridge_piece;

    public GameObject wood_bridge;
    public GameObject active_bridge;

    void Start () {
        if (type != Type.Null)
        {
            input_manager = InputManager.get_instance();
            sound_manager = SoundManager.get_instance();
            manager = BossRoomManager.get_instance();
            clear_guard();
            if(type == Type.Wood)
            {
                initialize_bridge();
            }
        }

    }

    private void Update()
    {
        if (type == Type.Stone || (type == Type.Wood && manager.phase == BossRoomManager.Phase.two))
        {
            if (is_danger)
            {
                if (player != null)
                {
                    ui_black_screen.add_observer(this);
                    ui_black_screen.change_screen(BlackScreen.ScreenState.Fade_Out);
                    player = null;
                }
            }

            if (is_ground && (boss_state.get_state() == Boss_State.State.Idle || boss_state.get_state() == Boss_State.State.Move))
            {
                if (type == Type.Stone)
                {
                    //둘중 하나라도 입력이 되고 있다면
                    if (input_manager.get_Horizontal() != 0 || input_manager.get_Vertical() != 0)
                    {
                        if (step_sound.isPlaying == false && !is_play)
                        {
                            is_play = true;
                            step_sound.Play();

                            if (!is_cognition)
                                step_cnt++;
                        }
                        playing_time += Time.deltaTime;
                        if (playing_time >= sound_delay)
                        {
                            sound_delay_init();
                        }
                    }
                    else
                    {
                        if (!is_cognition)
                            step_cnt = 0;
                    }

                    if (step_cnt >= cognition_step_count && !is_cognition)
                    {
                        //Debug.Log("사운드플레이");
                        is_cognition = true;
                        //sound_manager.mute_sound(SoundManager.SoundList.heartbeat_1, false);
                        sound_manager.play_sound(SoundManager.SoundList.boss_ready_real);
                    }
                }
                else if (type == Type.Wood)
                {
                    on_ground_time += Time.deltaTime;
                    if (on_ground_time > cognition_step_count)
                    {
                        on_ground_time = 0;
                        is_cognition = true;
                        //sound_manager.mute_sound(SoundManager.SoundList.heartbeat_1, false);
                    }
                }

                if (is_cognition)
                {
                    cognition_time += Time.deltaTime;

                    //if (!cognition_text && type == Type.Stone)
                    //{
                    //    ui_state_text.on_text_ui(UiStateText.TextName.cognation);
                    //    cognition_text = true;
                    //}
                    if (!cognition_text && type == Type.Wood)
                    {
                        ui_state_text.on_text_ui(UiStateText.TextName.wood_cognition);
                        cognition_text = true;
                    }

                    if (cognition_time >= cognition_time_list[1])
                    {
                        //sound_manager.mute_sound(SoundManager.SoundList.heartbeat_2, true);
                        if (type == Type.Stone)
                        {
                            //보스의 솟아오르기 공격 실행
                            manager.send_boss_state(Boss_State.State.Soar_Attack, this);
                        }
                        else if(type == Type.Wood)
                        {
                            manager.send_boss_state(Boss_State.State.Cross_Attack, this);
                        }

                        is_cognition = false;
                    }
                    else if (cognition_time >= cognition_time_list[0])
                    {
                        if (type == Type.Stone)//돌땅에서는 10초지나면 바로 공격
                        {
                            //보스의 솟아오르기 공격 실행
                            manager.send_boss_state(Boss_State.State.Soar_Attack, this);

                            is_cognition = false;
                        }

                        //sound_manager.mute_sound(SoundManager.SoundList.heartbeat_1, true);
                        //sound_manager.mute_sound(SoundManager.SoundList.heartbeat_2, false);
                        //if (!attack_ready_text && type == Type.Stone)
                        //{
                        //    ui_state_text.on_text_ui(UiStateText.TextName.attack_ready);
                        //    attack_ready_text = true;
                        //}
                        //if (cognition_time > tick_count)
                        //{
                        //    sound_manager.play_sound(SoundManager.SoundList.time_ticktock);
                        //    tick_count++;
                        //} // 초침소리 뺌
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            is_ground = true;
            CancelInvoke();
            player = other.gameObject;
        }
        if (other.CompareTag("Boss"))
        {
            //Debug.Log("충돌!" + this.name + " || " + other.name);
            if (type == Type.Wood)
            {
                type = Type.Null;
                for (int i = 0; i < wood_bridge_piece.Length; i++)
                {
                    wood_bridge_piece[i].isKinematic = false;
                }
                clear_guard();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && type != Type.Null)
        {
            //캐릭터가 땅을 벗어나고 0.5초가 지나면 카운트 리셋, 
            Invoke("clear_guard", 0.5f);
        }
    }

    void clear_guard()
    {
        //Debug.Log("땅 초기화" + this.name);
        on_ground_time = 0;
        is_ground = false;
        cognition_time = 0;
        is_cognition = false;
        step_cnt = 0;
        tick_count = cognition_time_list[0];
        cognition_text = false;
        attack_ready_text = false;
        player = null;
        if (sound_manager.sound_list[(int)SoundManager.SoundList.boss_ready_real].isPlaying && type == Type.Stone)
        {
            //Debug.Log("사운드 정지");
            sound_manager.stop_sound(SoundManager.SoundList.boss_ready_real,false);
        }
        sound_delay_init();
    }

    void sound_delay_init()
    {
        playing_time = 0;
        is_play = false;
    }

    public void set_danger(bool _danger)
    {
        is_danger = _danger;
    }

    public override void notify(Observable observable)
    {
        if (observable.gameObject.GetComponent<BlackScreen>())
        {
            BlackScreen torch = observable as BlackScreen;     
            
            //ObservableTorch torch = observable as ObservableTorch;
            if (torch.get_screen_state() == BlackScreen.ScreenState.Fade_Out)
            //if(torch.get_state() == ObservableTorch.State.On)
            {
                Debug.Log("플레이어 사망");
                //플ㄹ레이어 사망, 맵 재시작
                manager.init_bossroom();
                ui_black_screen.change_screen(BlackScreen.ScreenState.Fade_In);
                ui_black_screen.remove_observer(this);
                sound_manager.stop_sound(SoundManager.SoundList.boss_ready_real,false);
            }
        }
        else if(observable.gameObject.GetComponent<DestroyCheck>())
        {
            enemy_count--;
        }

    }

    public Transform [] get_enemy_point()
    {
        return enemy_position;
    }

    public void initialize_bridge()
    {
        //Debug.Log("다리 재생성");
        //이전 다리의 삭제와 새로운 다리의 생성,
        if (active_bridge != null)
        {
            Destroy(active_bridge.gameObject);
        }
        GameObject active_wood_bridge = (GameObject)Instantiate(wood_bridge, wood_bridge.transform.position, wood_bridge.transform.rotation, this.transform.parent);
        active_wood_bridge.transform.localScale = new Vector3(1, 1, 1);
        //active_wood_bridge.transform.SetParent(this.transform.parent);
        active_bridge = active_wood_bridge;

        active_bridge.gameObject.SetActive(true);

        type = Type.Wood;
        for(int i=0; i<wood_bridge_piece.Length; i++)
        {
            wood_bridge_piece[i] = active_wood_bridge.transform.Find((i+1).ToString()).GetComponent<Rigidbody>();
        }
    }

}
