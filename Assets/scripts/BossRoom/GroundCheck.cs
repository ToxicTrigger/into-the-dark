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
    public int heartbeat_count;

    [Tooltip("몇걸음부터 인식할지")]
    public float cognition_step_count;
    [Tooltip("인식 후 보스의 상태 변화 시간")]
    public int [] cognition_time_list;

    public Rigidbody []wood_bridge_piece;

    public GameObject wood_bridge;
    public GameObject active_bridge;

    public float []sound_volume_list;

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
                    manager.game_over();
                    player = null;
                    //is_danger = false;
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
                        is_cognition = true;
                        sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[0];
                        //sound_manager.play_sound(SoundManager.SoundList.boss_ready_real);
                    }
                }
                else if (type == Type.Wood)
                {
                    on_ground_time += Time.deltaTime;
                    if (on_ground_time > cognition_step_count)
                    {
                        on_ground_time = 0;
                        is_cognition = true;
                        sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[0];
                    }
                }

                if (is_cognition)
                {
                    if (!sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].isPlaying)
                    {
                            sound_manager.play_sound(SoundManager.SoundList.heartbeat);
                            heartbeat_count++;
                    }

                    if(type == Type.Stone)
                    {
                        if(heartbeat_count == 5)
                            sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[2];

                        else if (heartbeat_count == 3)
                            sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[1];

                        if(heartbeat_count ==6)
                        {
                            StartCoroutine(attack_timer(Boss_State.State.Soar_Attack));
                            is_cognition = false;
                        }
                    }
                    else if (type == Type.Wood)
                    {
                        if (heartbeat_count == 3)
                            sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[2];

                        else if (heartbeat_count == 2)
                            sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[1];

                        if(heartbeat_count == 3)
                        {
                            StartCoroutine(attack_timer(Boss_State.State.Cross_Attack));
                            is_cognition = false;
                        }
                    }
                    //cognition_time += Time.deltaTime;

                    //if (!cognition_text && type == Type.Wood)
                    //{
                    //    ui_state_text.on_text_ui(UiStateText.TextName.wood_cognition);
                    //    cognition_text = true;
                    //}

                    //if (cognition_time >= cognition_time_list[1])
                    //{
                    //    if (type == Type.Stone)
                    //    {
                    //        //보스의 솟아오르기 공격 실행
                    //        manager.send_boss_state(Boss_State.State.Soar_Attack, this);
                    //    }
                    //    else if(type == Type.Wood)
                    //    {
                    //        manager.send_boss_state(Boss_State.State.Cross_Attack, this);
                    //    }

                    //    is_cognition = false;
                    //}
                    //else if (cognition_time >= cognition_time_list[0])
                    //{
                    //    if (type == Type.Stone)//돌땅에서는 10초지나면 바로 공격
                    //    {
                    //        //보스의 솟아오르기 공격 실행
                    //        manager.send_boss_state(Boss_State.State.Soar_Attack, this);

                    //        is_cognition = false;
                    //    }
                    //}
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
            if (type == Type.Wood && manager.phase == BossRoomManager.Phase.two)
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
        heartbeat_count = 0;
        on_ground_time = 0;
        is_ground = false;
        cognition_time = 0;
        is_cognition = false;
        step_cnt = 0;
        tick_count = cognition_time_list[0];
        cognition_text = false;
        attack_ready_text = false;
        player = null;
        sound_delay_init();
        set_danger(false);
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
        if (observable.gameObject.GetComponent<DestroyCheck>())
        {
            enemy_count--;
            //Debug.Log(this.name + "의 적 삭제 적용 단계" + enemy_count);
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

    IEnumerator attack_timer(Boss_State.State _state)
    {
        sound_manager.play_sound(SoundManager.SoundList.boss_attack_ready);
        manager.send_boss_state(_state, this);
        yield return new WaitForSeconds(3.0f);
    }
}
