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
    public int cognition_step_count;
    [Tooltip("인식 후 보스의 상태 변화 시간")]
    public int [] cognition_time_list;

    void Start () {
        if (type != Type.Null)
        {
            input_manager = InputManager.get_instance();
            sound_manager = SoundManager.get_instance();
            manager = BossRoomManager.get_instance();
            clear_guard();
        }
    }

    private void Update()
    {
        if (type != Type.Null)
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
                //둘중 하나라도 입력이 되고 있다면
                if (input_manager.get_Horizontal() != 0 || input_manager.get_Vertical() != 0)
                {
                    if (step_sound.isPlaying == false && !is_play)
                    {
                        is_play = true;
                        step_sound.Play();


                        //Invoke("sound_delay_init", sound_delay);

                        if (!is_cognition)
                            step_cnt++;

                        if (step_cnt >= cognition_step_count)
                        {
                            is_cognition = true;
                            sound_manager.mute_sound(SoundManager.SoundList.heartbeat_1, false);
                        }
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

                if (is_cognition)
                {
                    cognition_time += Time.deltaTime;
                    if (!cognition_text)
                    {
                        ui_state_text.on_text_ui(UiStateText.TextName.cognation);
                        cognition_text = true;
                    }
                    if (cognition_time >= cognition_time_list[1])
                    {
                        sound_manager.mute_sound(SoundManager.SoundList.heartbeat_2, true);
                        //보스의 솟아오르기 공격 실행
                        manager.send_boss_state(Boss_State.State.Soar_Attack, this);
                        //clear_guard();
                        is_cognition = false;
                    }
                    else if (cognition_time >= cognition_time_list[0])
                    {
                        sound_manager.mute_sound(SoundManager.SoundList.heartbeat_1, true);
                        sound_manager.mute_sound(SoundManager.SoundList.heartbeat_2, false);
                        //1초마다 초침소리 재생
                        if (!attack_ready_text)
                        {
                            ui_state_text.on_text_ui(UiStateText.TextName.attack_ready);
                            attack_ready_text = true;
                        }
                        if (cognition_time > tick_count)
                        {
                            sound_manager.play_sound(SoundManager.SoundList.time_ticktock);
                            tick_count++;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            is_ground = true;
            CancelInvoke();
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //캐릭터가 땅을 벗어나고 0.5초가 지나면 카운트 리셋, 
            Invoke("clear_guard", 0.5f);
        }
    }

    void clear_guard()
    {
        is_ground = false;
        cognition_time = 0;
        is_cognition = false;
        step_cnt = 0;
        tick_count = 20;
        cognition_text = false;
        attack_ready_text = false;
        player = null;
        sound_manager.mute_sound(SoundManager.SoundList.heartbeat_1,true);
        sound_manager.mute_sound(SoundManager.SoundList.heartbeat_2, true);
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
}
