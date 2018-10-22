﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class Boss_Action : MonoBehaviour {

    Boss_State state;
    Boss_Worm boss;

    Animator animator;

    public AudioSource rush_attack_sound;

    enum Rotate_Cnt
    {
        one = 0,
        two,
        three,
        four
    }
    Rotate_Cnt rot_cnt;

    public Boss_Tail[] tail;

    public Transform player;
    //하나의 액션이 2개 이상의 단계를 가질 때 사용한다. 
    public int action_phase;

    Vector3 move_dir;
    Vector3 tail_dir;

    IEnumerator c_timer;

    //idle
    public Transform rotate_center;
    public float map_width;
    public float map_height;
    public Vector3[] rotate_pos;
    public float idle_speed;

    public Transform around_transform;

    public float recognize_distance;    //인식 거리
    public float proximity_distance;    //가까운 거리 (가까운 것으로 인정하는 거리)
    //
    //soar
    public int soar_count;
    public GroundCheck soar_target;
    public float y_dis;
    public float soar_speed;
    public float soar_attack_check;
    public int soar_shake_tick;
    public float soar_shake_power;
    public float soar_tick_by_time;
    //groggy
    public Transform [] groggy_point;
    public Vector3 g_point;
    public int groggy_cnt;

    public int shake_tick;
    public float shake_power;

    //wipping attack
    Transform attack_point;
    public float y_up_point;
    public float attack_speed;

    public float idle_radius;
    public float idle_y_pos;

    //원 그리는 부분
    public int segments;
    public float x_radius;
    public float y_radius;
    LineRenderer line;

    //move
    public Transform move_pos;
    public float move_speed;

    //cross
    public Transform water_pos;

    ActionCamera ac;

    void Start () {
        animator = this.GetComponent<Animator>();
        rot_cnt = Rotate_Cnt.one;
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        boss = GetComponent<Boss_Worm>();
        state = GetComponent<Boss_State>();
        action_phase = 1;
        //around_transform.position = new Vector3(rotate_center.position.x - map_width, player.position.y - idle_y_pos, player.position.z);

        ac = FindObjectOfType<ActionCamera>();

        //rotate_pos = new Vector3[4];
        //rotate_pos[0] = new Vector3 (rotate_center.position.x - map_width, rotate_center.position.y - idle_y_pos, rotate_center.position.z);
        //rotate_pos[1] = new Vector3(rotate_center.position.x , rotate_center.position.y - idle_y_pos, rotate_center.position.z + map_height);
        //rotate_pos[2] = new Vector3(rotate_center.position.x + map_width, rotate_center.position.y - idle_y_pos, rotate_center.position.z );
        //rotate_pos[3] = new Vector3(rotate_center.position.x, rotate_center.position.y - idle_y_pos, rotate_center.position.z - map_height);
    }

    private void LateUpdate()
    {
        if (state.get_state() != Boss_State.State.Groggy 
            //&& state.get_state() != Boss_State.State.Whipping_Attack
            )
        {
            for (int i = 0; i < tail.Length; i++)
            {
                tail[i].move_update(tail_dir);
            }
        } 
    }

    Vector3 cross_point;
    public float cross_distance;
    public Transform cross_center;
    Vector3 cross_start;
    Vector3 cross_end;
    public float cross_height;
    Vector3 up_point;

    bool idle_state;

    void Update () {

        if(rush_attack)
            create_point();

        switch (state.get_state())
        {
            case Boss_State.State.Idle:
                if (action_phase == 1)
                {
                    animator.SetBool("groggy", false);
                    action_phase = 2;
                }
                else if (action_phase == 2)
                {
                    transform.parent.position = around_transform.position;
                }
                break;

            case Boss_State.State.Move:

                if(action_phase ==1)
                {

                    move_dir = (around_transform.position - transform.parent.position).normalized;

                    action_phase = 2;
                    
                }
                else if(action_phase == 2)
                {
                    transform.parent.position += move_dir * move_speed * Time.deltaTime;
                    //c_timer = null;
                    if(Vector3.Distance(around_transform.position, transform.parent.position) < 1.0f)
                    {
                        state.set_state(Boss_State.State.Idle,null);
                    }
                }
                break;

            case Boss_State.State.Rush_Attack:
                //일정거리 남으면 공격범위 표시 -> 땅에 닿으면 공격범위 표시 X

                if(action_phase == 1)
                {
                    //타이머를 1회만 실행하기 위함
                    if (c_timer == null)
                    {
                        //jump_power = 15;
                        c_timer = Rush_Attack_Timer();
                        StartCoroutine(c_timer);
                    }
                    
                }
                else if(action_phase ==2)
                {
                    move_target = new Vector3(player.position.x, player.position.y + 5, player.position.z);
                    origin_dis = Vector2.Distance(new Vector2(rush_attack_start_pos.x, rush_attack_start_pos.z), new Vector2(move_target.x, move_target.z));

                    move_dir = (move_target - transform.position).normalized;  //이동 방향        
                    float cur_dis = Vector2.Distance(new Vector2(transform.parent.position.x, transform.parent.position.z), new Vector2(move_target.x, move_target.z));

                    move_dir.y = Mathf.Lerp(2.0f, -1.0f, (origin_dis - cur_dis) / origin_dis - 0.2f);
                    if (move_dir.y < 0) move_dir.y *= jump_power;

                    transform.parent.position += move_dir * rush_speed * Time.deltaTime;

                    if(cur_dis < 2.0f )//&& transform.parent.position.y < move_target.y+5)
                    {
                        //EventManager.get_instance().camera_shake(c_shake_power_rush, c_shake_cnt_rush, c_shake_speed_rush, EventManager.Direction.Up_Down, c_shake_minus_rush);
                        //카메라 쉐이크
                        action_phase = 3;
                    }
                }
                else if(action_phase == 3)
                {
                    Debug.Log( "3  -- " + action_phase);
                    move_dir.y = -1;//_dir.y-10;
                    transform.parent.position += move_dir * rush_speed * Time.deltaTime;
                    if (rush_attack && Vector2.Distance(new Vector2(player.transform.parent.position.x, player.transform.parent.position.z), new Vector2(transform.parent.position.x, transform.parent.position.z)) <= x_radius)
                    {
                        Debug.Log(" !맞았음! 플레이어와의 거리 >> "+Vector2.Distance(new Vector2(player.transform.parent.position.x, player.transform.parent.position.z), new Vector2(transform.parent.position.x, transform.parent.position.z)));
                        attack_player(10);
                    }
                    else if (rush_attack)
                    {                        
                        rush_attack = false;
                        line.SetVertexCount(0);
                        line.SetVertexCount(segments + 1);
                    }

                    if(tail[tail.Length-1].transform.parent.position.y < around_transform.position.y-10)
                    {
                        Debug.Log("rush_end");        
                        //action_phase = 1;
                        state.set_state(Boss_State.State.Idle, null);
                        c_timer = null;
                    }
                    
                }

                break;

            case Boss_State.State.Cross_Attack:
                
                if(action_phase ==1)
                {
                    //cross_point = BossRoomManager.get_instance().get_cross_point();

                    cross_point = soar_target.transform.position;

                    //시작위치에 세팅

                    Vector3 dir = (cross_center.position - cross_point).normalized;

                    dir.y = 0;

                    cross_start = cross_point + (-dir * cross_distance);
                    cross_end = cross_center.position;//cross_point + (dir * cross_distance);

                    transform.parent.position = new Vector3(cross_start.x, around_transform.position.y, cross_start.z);
                    action_phase = 2;

                    move_target = new Vector3(cross_end.x, cross_end.y, cross_end.z);
                    origin_dis = Vector2.Distance(new Vector2(cross_start.x, cross_start.z), new Vector2(cross_end.x, cross_end.z));

                    jump_power = 1; test_cnt = 0;
                }
                else if(action_phase ==2)
                {
                    move_dir = (move_target - transform.parent.position).normalized;  //이동 방향        
                    float cur_dis = Vector2.Distance(new Vector2(transform.parent.position.x, transform.parent.position.z), new Vector2(move_target.x, move_target.z));
                    move_dir.y = Mathf.Lerp(cross_height, -2.0f, (origin_dis - cur_dis) / origin_dis - 0.3f);
                    move_dir.y *= jump_power;
                    test_cnt++;
                    //Debug.Log("["+test_cnt+"] cross_height = "+move_dir.y);
                    //if (move_dir.y < 0) move_dir.y *= jump_power;

                    transform.parent.position += move_dir * cross_speed * Time.deltaTime;

                    if(tail[0].transform.parent.position.y > water_pos.position.y && SoundManager.get_instance().sound_list[(int)SoundManager.SoundList.boss_attack_up].mute == true)
                    {
                        SoundManager.get_instance().mute_sound(SoundManager.SoundList.boss_attack_up, false);
                        SoundManager.get_instance().play_sound(SoundManager.SoundList.boss_attack_up);
                    }
                    else if(tail[0].transform.parent.position.y < water_pos.position.y && SoundManager.get_instance().sound_list[(int)SoundManager.SoundList.boss_attack_up].mute == false)
                    {
                        SoundManager.get_instance().mute_sound(SoundManager.SoundList.boss_attack_up, true);
                        SoundManager.get_instance().play_sound(SoundManager.SoundList.boss_attack_down);
                    }
                    

                    if (cur_dis < 2 && tail[tail.Length - 1].transform.position.y < around_transform.position.y+10)//&& transform.position.y < move_target.y+5)
                    {
                        //EventManager.get_instance().camera_shake(c_shake_power_rush, c_shake_cnt_rush, c_shake_speed_rush, EventManager.Direction.Up_Down, c_shake_minus_rush);
                        //카메라 쉐이크
                        action_phase = 1;

                        state.set_state(Boss_State.State.Move, null);
                    }
                }
                else if(action_phase ==3)
                {
                    //돌아가기
                    move_dir = Vector3.zero;
                    move_dir.y = -1;

                    transform.parent.position += move_dir * cross_speed * Time.deltaTime;
                    
                    if (tail[tail.Length - 1].transform.parent.position.y < around_transform.position.y - 10)
                    {
                        action_phase = 1;
                        state.set_state(Boss_State.State.Idle,null);
                        cross_point = Vector3.zero;
                        cross_start = Vector3.zero;
                        cross_end = Vector3.zero;
                        BossRoomManager.get_instance().set_cross_point(Vector3.zero);
                    }
                }


                break;

            case Boss_State.State.Soar_Attack:
                if(action_phase == 1)
                {
                    action_phase = 2;
                    for (int i = 0; i < 5; i++)
                    {
                        tail[i].GetComponent<Collider>().isTrigger = true;
                    }
                    animator.SetBool("groggy", false);
                    move_target = transform.parent.position + new Vector3(0, y_dis, 0);
                    move_dir = (move_target - transform.parent.position).normalized;
                }
                else if (action_phase == 2)
                {
                    transform.parent.position += move_dir * soar_speed * Time.deltaTime;
                    if (transform.parent.position.y > move_target.y)
                    {
                        action_phase = 3;
                        move_target = new Vector3(soar_target.transform.position.x, transform.parent.position.y, soar_target.transform.position.z);
                        origin_dis = Vector2.Distance(new Vector2(transform.parent.position.x, transform.parent.position.z), new Vector2(move_target.x, move_target.z));
                        move_dir = (move_target - transform.parent.position).normalized;
                    }
                }
                else if (action_phase == 3)
                {
                    transform.parent.position += move_dir * soar_speed * Time.deltaTime;
                    if (Vector3.Distance(transform.parent.position, move_target) < 2.0f)
                    {                        
                        action_phase = 4;
                        move_target = new Vector3(soar_target.transform.position.x, around_transform.position.y, soar_target.transform.position.z);
                        move_dir = (move_target - transform.parent.position).normalized;

                    }
                }
                else if(action_phase == 4)
                {
                    transform.parent.position += move_dir * soar_speed * Time.deltaTime;

                    if(Vector3.Distance(transform.parent.position, move_target) < soar_attack_check)
                    {
                        ac.Shake(soar_shake_tick, soar_shake_power, soar_tick_by_time * Time.deltaTime);
                        soar_target.set_danger(true);
                    }

                    if (tail[tail.Length-1].transform.parent.position.y <= move_target.y)
                    {              
                        for(int i =0; i<soar_target.get_enemy_point().Length; i++)
                        {
                            if (soar_target.enemy_count >= 8)
                                break;
                            BossRoomManager.get_instance().create_enemy(soar_target.get_enemy_point()[i].position, soar_target);

                            soar_target.enemy_count++;
                        }
                        action_phase = 1;
                        move_target = Vector3.zero;
                        soar_target.set_danger(false);
                        state.set_state(Boss_State.State.Move, null);
                    }
                }

                break;
            case Boss_State.State.Groggy:
                if(action_phase == 1)
                {
                    for (int i = 0; i < 5; i++)
                        tail[i].GetComponent<Collider>().isTrigger = false;

                    Vector3 temp = Vector3.zero;
                    if (groggy_point[groggy_cnt].position.x < rotate_center.position.x)
                        temp.x = -1;
                    else temp.x = 1;

                    if (groggy_point[groggy_cnt].position.z < rotate_center.position.z)
                        temp.z = -1;
                    else temp.z = 1;

                    temp = new Vector3(g_point.x , - g_point.y, g_point.z * temp.z);

                    Quaternion quat = Quaternion.identity;
                    quat = Quaternion.LookRotation((rotate_center.position - groggy_point[groggy_cnt].position).normalized);
                    quat.x = 0;
                    quat.z = 0;
                    transform.parent.rotation = quat;

                    transform.parent.position = groggy_point[groggy_cnt].position;// + temp;
                    //quat = Quaternion.Euler(new Vector3(0, 140, 0));


                    animator.SetBool("groggy", true);

                    SoundManager.get_instance().play_sound(SoundManager.SoundList.boss_groggy);

                    //애니메이션 재생이 제대로 이루어질 장소와 회전값을 찾아 넣는다.
                    //그로기 상태로 변화하는 곳의 위치가 고정되어 있다면 그대로 사용이 가능하지만 추후 변한다면....
                    action_phase = 2;
                    if (groggy_cnt == 2)
                        groggy_cnt = 0;
                    else
                        groggy_cnt++;
                }
                else if(action_phase ==2)
                {
                    //대기
                }

                break;
            case Boss_State.State.Groggy_End:

                break;
            case Boss_State.State.Death:               
                //애니메이션/소리 재생등을 완료한 후 삭제까지 처리!
                break;
            case Boss_State.State.Ready:

                break;
            case Boss_State.State.Up:

                break;
            default:
                break;
        }

    }



    Vector3 rush_attack_start_pos;
    Vector3 move_target;
    float rush_timer_cnt =3.0f;
    float origin_dis;
    public float jump_power;
    public float rush_speed;
    public float cross_speed;

    bool rush_attack;

    IEnumerator Rush_Attack_Timer()
    {
        //boss_cry.Play();
        //사운드 재생

        //if (action_state == Action.Ready)
        //    action_state = Action.Up;    //이동을 하지 않기 위함 (이 자리를 공격시작 자리로 정한다.)
        //else action_state = Action.Ready;

        rush_attack_start_pos = transform.parent.position;
        yield return new WaitForSeconds(rush_timer_cnt); // 공격 대기시간
        
        move_target = new Vector3(player.position.x, player.position.y + 2, player.position.z); //내위치 + ((반지름*2) * 이동방향) _ x,z || around_transform의 y위치 
        origin_dis = Vector2.Distance(new Vector2(transform.parent.position.x, transform.parent.position.z), new Vector2(move_target.x, move_target.z));  //거리 계산 (실시간 이동 거리에 따라 상승/하락 이동의 지정을 위함)_내 위치(x,z)와 타겟(x,z) 높이는 거리에 반영하지 않으므로!

        action_phase = 2; //다음 행동

        //action_state = Action.Rush_Attack;  //공격상태로 전환
        //speed = rush_attack_speed;          //공격 스피드로 전환
        //jump_power = rush_jump_power;

        //boss_rush_attack[Random.Range(0, 2)].Play();
        //사운드 재생
        rush_attack = true;
    }

    IEnumerator Whipping_Timer()
    {
        transform.parent.position += move_dir;
        yield return new WaitForSeconds(0.5f);
        action_phase = 3;
        move_dir = (player.transform.parent.position - transform.parent.position).normalized;
    }

    IEnumerator Normal_Timer(float _time, Boss_State.State _state)
    {
        yield return new WaitForSeconds(_time);
        state.set_state(_state,null);
        action_phase = 2;
    }

    void create_point()
    {
        
        float x, z;
        float y = 1;

        float angle = 20;

        for(int i=0; i<(segments+1); i++)
        {
            x = Mathf.Cos(Mathf.Deg2Rad * angle) * x_radius;
            z = Mathf.Sin(Mathf.Deg2Rad * angle) * y_radius;

            Vector3 pos = player.transform.position + new Vector3(x, y, z);

            line.SetPosition(i, transform.InverseTransformPoint(pos));

            angle += (360f / segments); 
        }

    }

    void attack_player(int _damage)
    {
        player.gameObject.GetComponent<Damageable>().Damaged(_damage, 1.0f);
    }

    public void set_idle_state(bool _state)
    {
        idle_state = _state;
    }

    public Vector3 get_groggy_point()
    {
        return groggy_point[groggy_cnt].position;
    }

    public void set_soar_target(GroundCheck _gameobj)
    {
        soar_target = _gameobj;
    }

    public void set_soar_target(GroundCheck _gameobj, float _dis, float _height)
    {
        soar_target = _gameobj;
        cross_distance = _dis;
        cross_height = _height;
    }

    public void groggy_shake_cam()
    {
        Debug.Log("shake");
        ac.Shake(shake_tick, shake_power, Time.deltaTime);
    }

    public void set_cross_dis(float _dis, float _height, GameObject obj)
    {
        if (obj == soar_target.gameObject)
        {
            cross_distance = _dis;
            cross_height = _height;
        }
    }
    int test_cnt;
}
