using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : Observer
{
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
    ActionCamera ac;
    Boss_Action boss_action;
    public Boss_State boss_state;
    public UiStateText ui_state_text;
    public BlackScreen ui_black_screen;
    public AudioSource step_sound;
    int step_cnt;
    float on_ground_time;
    public bool is_cognition;  //보스의 인식
    public float sound_delay;
    public float playing_time;
    bool is_play;

    bool cognition_text;
    bool attack_ready_text;

    public bool is_danger;
    public GameObject player;
    public GameObject _player;

    public Transform[] enemy_position;

    public int enemy_count;
    public int heartbeat_count;

    [Tooltip("몇걸음부터 인식할지")]
    public float cognition_step_count;
    [Tooltip("인식 후 보스의 상태 변화 시간")]
    public int[] cognition_time_list;

    public Rigidbody[] wood_bridge_piece;

    public GameObject wood_bridge;
    public GameObject active_bridge;

    public float[] sound_volume_list;

    public int shack_tick;
    public float shack_power;
    public float add_power;
    float power;
    public float shack_tick_by_time;

    public float add_force_power;

    public float fly_speed;
    public float gravity;

    IEnumerator fly_timer;

    public bool re_start;

    void Start()
    {
        if (type != Type.Null)
        {
            input_manager = InputManager.get_instance();
            sound_manager = SoundManager.get_instance();
            manager = BossRoomManager.get_instance();
            clear_guard();
            if (type == Type.Wood)
            {
                initialize_bridge();
            }
        }
        ac = FindObjectOfType<ActionCamera>();
        boss_state = FindObjectOfType<Boss_State>();
        boss_action = FindObjectOfType<Boss_Action>();
        ui_black_screen = FindObjectOfType<BlackScreen>();
    }

    public IEnumerator shoot_player()
    {
        Vector3 dir = (transform.position - player.transform.position).normalized;
        _player = player;
        while (true)
        {
            dir.y -= gravity;
            _player.transform.position += dir * fly_speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            if (re_start)
                break;
        }
        _player = null;
        fly_timer = null;
    }

    bool attack;
    private void Update()
    {
        if (type == Type.Stone || (type == Type.Wood && manager.phase == BossRoomManager.Phase.two))
        {
            if (is_danger&& player != null &&!attack)
            {
                //if (fly_timer == null)
                //{
                //    re_start = false;
                //    fly_timer = shoot_player();
                //    StartCoroutine(fly_timer);
                //}
                //manager.game_over(this);

                //플레이어 스턴 추가
                player.GetComponent<Damageable>().Damaged(100, 3.0f);

                attack_ground();
            }

            if (is_ground)
            {
                if (input_manager.get_Horizontal() != 0 || input_manager.get_Vertical() != 0)
                {
                    if (player.GetComponent<Player>().cur_ani == ("Run"))
                    {
                        if (!step_sound.isPlaying && !is_play)
                        {
                            is_play = true;
                            step_sound.Play();
                        }

                        playing_time += Time.deltaTime;

                        if (playing_time >= sound_delay)
                            sound_delay_init();
                    }
                    else if(step_sound.isPlaying)
                    {
                        is_play = false;
                        step_sound.Stop();
                    }

                }                
            }

            if (is_ground && (boss_state.get_state() == Boss_State.State.Idle || boss_state.get_state() == Boss_State.State.Move))
            {
                //if (type == Type.Stone)
                //{
                //    //둘중 하나라도 입력이 되고 있다면
                //    if (input_manager.get_Horizontal() != 0 || input_manager.get_Vertical() != 0)
                //    {
                //        if (step_sound.isPlaying == false && !is_play)
                //        {
                //            is_play = true;
                //            step_sound.Play();

                //            if (!is_cognition)
                //                step_cnt++;
                //        }
                //        playing_time += Time.deltaTime;
                //        if (playing_time >= sound_delay)
                //        {
                //            sound_delay_init();
                //        }
                //    }
                //    else
                //    {
                //        if (!is_cognition)
                //        {
                //            step_cnt = 0;
                //        }
                //    }

                //    if (step_cnt >= cognition_step_count && !is_cognition)
                //    {
                //        is_cognition = true;
                //        BossRoomManager.get_instance().send_boss_state(Boss_State.State.Cross_Attack, this, 50, 2);
                //        //boss_action.set_cross_dis(50, 2.0f, this.gameObject);
                //        sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[0];
                //    }
                //}
                //else if (type == Type.Wood)
                //{
                //    on_ground_time += Time.deltaTime;
                //    if (on_ground_time > cognition_step_count)
                //    {
                //        on_ground_time = 0;
                //        is_cognition = true;
                //        sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[0];
                //    }
                //}
                
                //입장시 바로 cross_attack실행 후 심장소리 재생, is_cognition = true
                if(!is_cognition)
                {
                    if(type == Type.Stone)
                        BossRoomManager.get_instance().send_boss_state(Boss_State.State.Cross_Attack, this, 50, 2);

                    sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[0];
                    is_cognition = true;
                    attack = false;
                }
                if (is_cognition)
                {
                    if (!sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].isPlaying && (boss_state.get_state() == Boss_State.State.Idle || boss_state.get_state() == Boss_State.State.Move))
                    {
                        sound_manager.play_sound(SoundManager.SoundList.heartbeat);
                        ac.Shake(shack_tick, power, shack_tick_by_time * Time.deltaTime);
                        heartbeat_count++;
                        if (heartbeat_count == 2 || heartbeat_count == 4)
                            power += add_power;
                    }

                    if (type == Type.Stone)
                    {
                        if (heartbeat_count == 5)
                        {
                            sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[2];
                        }

                        else if (heartbeat_count == 3)
                        {
                            sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[1];
                        }

                        if (heartbeat_count == 6)
                        {
                            sound_manager.play_sound(SoundManager.SoundList.boss_attack_ready);
                            manager.send_boss_state(Boss_State.State.Soar_Attack, this);
                            is_cognition = false;
                        }
                    }
                    else if (type == Type.Wood)
                    {
                        if (heartbeat_count == 5)
                            sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[2];

                        else if (heartbeat_count == 3)
                            sound_manager.sound_list[(int)SoundManager.SoundList.heartbeat].volume = sound_volume_list[1];

                        if (heartbeat_count == 6)
                        {
                            sound_manager.play_sound(SoundManager.SoundList.boss_attack_ready);
                            manager.send_boss_state(Boss_State.State.Cross_Attack, this,30,2.3f);
                            is_cognition = false;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            if (type == Type.Stone || (type == Type.Wood && manager.phase == BossRoomManager.Phase.two))
            {
                is_ground = true;
                CancelInvoke();
                player = other.gameObject;
            }
        }
        if (other.CompareTag("Boss"))
        {
            if (type == Type.Wood && manager.phase == BossRoomManager.Phase.two && boss_state.get_state() == Boss_State.State.Cross_Attack)
            {
                type = Type.Null;
                manager.minus_wood_bridge_count();
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
            is_ground = false;
        }
    }

    void clear_guard()
    {
        power = shack_power;
        heartbeat_count = 0;
        is_ground = false;
        is_cognition = false;
        set_danger(false);
        player = null;

        //step_cnt = 0;
        //tick_count = cognition_time_list[0];
        //sound_delay_init();
        //on_ground_time = 0;
        //cognition_time = 0;
    }

    void attack_ground()
    {
        attack = true;
        set_danger(false);
        is_cognition = false;
        power = shack_power;
        heartbeat_count = 0;
    }

    void out_ground()
    {
        set_danger(false);
        is_ground = false;
        is_cognition = false;
        power = shack_power;
        heartbeat_count = 0;
        player = null;
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
        //Debug.Log(observable.name);
        if (observable.gameObject.GetComponent<DestroyCheck>())
        {
            enemy_count--;
            //Debug.Log(this.name + "의 적 삭제 적용 단계" + enemy_count);
        }
        if (observable.gameObject.GetComponent<BlackScreen>())
        {
            re_start = true;
        }

    }

    public Transform[] get_enemy_point()
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
        for (int i = 0; i < wood_bridge_piece.Length; i++)
        {
            wood_bridge_piece[i] = active_wood_bridge.transform.Find((i + 1).ToString()).GetComponent<Rigidbody>();
        }
    }

}
