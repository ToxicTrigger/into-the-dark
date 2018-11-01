using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class Boss_Action : MonoBehaviour {

    Boss_State state;
    Boss_Worm boss;

    Animator animator;

    public AudioSource rush_attack_sound;

    public Boss_Tail[] tail;

    public Transform player;

    public int action_phase;

    Vector3 move_dir;
    Vector3 tail_dir;

    //idle
    public Transform rotate_center;

    public Transform around_transform;

    //soar
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

    //cross
    public Transform water_pos;
    public Poison poison;
    public Transform drop_point;
    Vector3 cross_point;
    public float cross_distance;
    public Transform cross_center;
    Vector3 cross_start;
    Vector3 cross_end;
    public float cross_height;

    ActionCamera ac;

    void Start () {
        animator = this.GetComponent<Animator>();

        boss = GetComponent<Boss_Worm>();
        state = GetComponent<Boss_State>();
        action_phase = 1;

        ac = FindObjectOfType<ActionCamera>();
    }

    private void LateUpdate()
    {
        if (state.get_state() != Boss_State.State.Groggy 
            )
        {
            for (int i = 0; i < tail.Length; i++)
            {
                tail[i].move_update(tail_dir);
            }
        } 
    }

    void Update () {

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

            case Boss_State.State.Cross_Attack:
                
                if(action_phase ==1)
                {

                    cross_point = soar_target.transform.position;

                    Vector3 dir = (cross_center.position - cross_point).normalized;

                    dir.y = 0;

                    cross_start = cross_point + (-dir * cross_distance);
                    cross_end = cross_center.position;

                    transform.parent.position = new Vector3(cross_start.x, around_transform.position.y, cross_start.z);
                    action_phase = 2;

                    move_target = new Vector3(cross_end.x, cross_end.y, cross_end.z);
                    origin_dis = Vector2.Distance(new Vector2(cross_start.x, cross_start.z), new Vector2(cross_end.x, cross_end.z));

                    jump_power = 1;
                    StartCoroutine(drop_poison());
                }
                else if(action_phase ==2)
                {
                    move_dir = (move_target - transform.parent.position).normalized;  //이동 방향        
                    float cur_dis = Vector2.Distance(new Vector2(transform.parent.position.x, transform.parent.position.z), new Vector2(move_target.x, move_target.z));
                    move_dir.y = Mathf.Lerp(cross_height, -2.0f, (origin_dis - cur_dis) / origin_dis - 0.3f);
                    move_dir.y *= jump_power;

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
                                        

                    if (cur_dis < 2 && tail[tail.Length - 1].transform.position.y < around_transform.position.y+10)
                    {
                        action_phase = 1;

                        state.set_state(Boss_State.State.Idle, null);
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
                        state.set_state(Boss_State.State.Idle, null);
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

                    transform.parent.position = groggy_point[groggy_cnt].position;


                    animator.SetBool("groggy", true);

                    SoundManager.get_instance().play_sound(SoundManager.SoundList.boss_groggy);

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
            case Boss_State.State.Death:               

                break;
            default:
                break;
        }

    }

    public float[] drop_time;

    IEnumerator drop_poison()
    {
        //Rigidbody rig;
        for (int i = 0; i < drop_time.Length; i++)
        {
            yield return new WaitForSeconds(drop_time[i]);
            Poison _poison = Instantiate(poison, drop_point.position, Quaternion.identity);
        }
    }

    Vector3 move_target;
    float origin_dis;
    public float jump_power;
    public float cross_speed;

    void attack_player(int _damage)
    {
        player.gameObject.GetComponent<Damageable>().Damaged(_damage, 1.0f, player.transform);
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
}
