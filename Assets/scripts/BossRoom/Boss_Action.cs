using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Action : MonoBehaviour {

    Boss_State state;
    Boss_Worm boss;

    public Boss_Tail[] tail;

    public Transform player;
    //하나의 액션이 2개 이상의 단계를 가질 때 사용한다. 
    public int action_phase;

    Vector3 move_dir;
    Vector3 tail_dir;

    IEnumerator c_timer;

    public Transform around_transform;

    int soar_count;
    public Transform [] soar_target;
    public Transform groggy_point;


    //wipping attack
    Transform attack_point;
    float y_up_point;
    float attack_speed;

    public float idle_radius;
    public float idle_y_pos;

    //원 그리는 부분
    public int segments;
    public float x_radius;
    public float y_radius;
    LineRenderer line;
    

    void Start () {
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        boss = GetComponent<Boss_Worm>();
        state = GetComponent<Boss_State>();
        action_phase = 1;
        around_transform.position = new Vector3(player.position.x + idle_radius, player.position.y - idle_y_pos, player.position.z);
        
        create_point();
    }

    private void LateUpdate()
    {
        if (state.get_state() != Boss_State.State.Groggy &&
            state.get_state() != Boss_State.State.Whipping_Attack)
        {
            for (int i = 0; i < tail.Length; i++)
            {
                tail[i].move_update(tail_dir);
            }
        }
    }


    Vector3 up_point;
    void Update () {

        //테스트용
        if (Input.GetKeyDown(KeyCode.Space) && state.get_state() == Boss_State.State.Idle)
        {
            state.set_state(Boss_State.State.Rush_Attack);
        }
        if(rush_attack)
            create_point();

        around_transform.RotateAround(player.transform.position, Vector3.up, 2f);

        switch (state.get_state())
        {
            case Boss_State.State.Idle:

                    move_dir = (around_transform.position - this.transform.position).normalized;
                    transform.position = around_transform.position;

                break;
            case Boss_State.State.Rush_Attack:
                //일정거리 남으면 공격범위 표시 -> 땅에 닿으면 공격범위 표시 X

                if(action_phase == 1)
                {
                    //타이머를 1회만 실행하기 위함
                    if (c_timer == null)
                    {
                        c_timer = Rush_Attack_Timer();
                        StartCoroutine(c_timer);
                    }
                }
                else if(action_phase ==2)
                {
                    move_target = new Vector3(player.position.x, player.position.y + 5, player.position.z);
                    origin_dis = Vector2.Distance(new Vector2(rush_attack_start_pos.x, rush_attack_start_pos.z), new Vector2(move_target.x, move_target.z));

                    move_dir = (move_target - transform.position).normalized;  //이동 방향        
                    float cur_dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z));

                    move_dir.y = Mathf.Lerp(1.0f, -1.0f, (origin_dis - cur_dis) / origin_dis - 0.3f);
                    if (move_dir.y < 0) move_dir.y *= jump_power;

                    transform.position += move_dir * speed * Time.deltaTime;

                    if(cur_dis < 1.5 )//&& transform.position.y < move_target.y+5)
                    {
                        //EventManager.get_instance().camera_shake(c_shake_power_rush, c_shake_cnt_rush, c_shake_speed_rush, EventManager.Direction.Up_Down, c_shake_minus_rush);
                        //카메라 쉐이크
                        action_phase = 3;
                    }
                }
                else if(action_phase == 3)
                {
                    //Vector3 _dir = (around_transform.position - transform.position).normalized;
                    move_dir.y = -1;//_dir.y-10;
                    transform.position += move_dir * speed * Time.deltaTime;
                    if (rush_attack && Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(transform.position.x, transform.position.z)) <= x_radius)
                    {
                        attack_player(50);
                    }

                    if (rush_attack)
                    {
                        rush_attack = false;
                        line.SetVertexCount(0);
                        line.SetVertexCount(segments + 1);
                    }

                    if(tail[tail.Length-1].transform.position.y < around_transform.position.y-10)
                    {
                        action_phase = 1;
                        state.set_state(Boss_State.State.Idle);
                        c_timer = null;
                    }
                    
                }

                break;
            case Boss_State.State.Whipping_Attack:
                //if(action_phase==1)
                //{
                //    //땅 속에서 특정 위치로 이동한다.
                //    transform.position = attack_point.position;

                //    //공격 위치는 딱1회 계산할 예정이므로 이곳에서 처리를 하고 넘어간다.
                //    up_point = attack_point.position;
                //    up_point.y += y_up_point;   //up_point(attack_point)에 y만큼 올려준다.
                //    move_dir = (up_point - transform.position).normalized;

                //    action_phase = 2;

                //}
                //else if(action_phase ==2)
                //{
                //    transform.position += move_dir * speed * Time.deltaTime;

                //    if (transform.position.y > up_point.y)
                //    {
                //        move_dir = (player.transform.position - transform.position).normalized;

                //        action_phase = 3;
                //    }
                //}
                //else if(action_phase == 3)
                //{
                //    //위로 이동을 완료했다면 플레이어를 향해 이동한다.

                //    transform.position += move_dir * speed * Time.deltaTime;

                //    if(/*플레이어 이동 완료*/)
                //    {
                //       move_dir = (up_point - transform.position).normalized;    //다만 돌아가기 때문에 회전을 한다. 회전에 제한을 걸자

                //       action_phase = 4;
                //    }

                //}
                //else if(action_phase ==4)
                //{
                //    //플레이어를 향해 이동을 했다면 다시 돌아간다.
                //    transform.position += move_dir * speed * Time.deltaTime;

                //    if (/*돌아가기 완료*/)
                //    {
                //        move_dir = (attack_point.position - transform.position).normalized;

                //        action_phase = 5;
                //    }
                //}

                //else if(action_phase == 5)
                //{
                //    transform.position += move_dir * speed * Time.deltaTime;

                //    if(transform.position.y < attack_point.position.y)
                //    {
                //        action_phase = 1;
                //        state.set_state(Boss_State.State.Idle);
                //    }
                //}
                break;
            case Boss_State.State.Soar_Attack:

                if (action_phase == 1)
                {
                    if (transform.position.y > move_target.y)
                    {
                        action_phase = 2;
                        move_target = new Vector3(soar_target[soar_count].position.x, transform.position.y, soar_target[soar_count].position.z);
                        origin_dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z));
                                           
                    }
                }
                else if (action_phase == 2)
                {
                    float cur_dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z));

                    if (cur_dis < 0.5)
                    {
                        action_phase = 3;
                        move_target = new Vector3(soar_target[soar_count].position.x, around_transform.position.y, soar_target[soar_count].position.z);
                    }
                }
                else if (action_phase == 3)
                {
                    if (tail[0].transform.position.y <= move_target.y)
                    {
                        //soar_count++;
                        state.set_state(Boss_State.State.Idle);
                        action_phase = 1;
                        move_target = Vector3.zero;
                        //EventManager.get_instance().off_event();
                        //BossRoomManager.get_instance().crumbling_pillar_all();  //페이즈에 따라 기둥을 무너뜨린다. 
                        //EventManager.get_instance().camera_shake(c_shake_power, c_shake_cnt, c_shake_speed, EventManager.Direction.Up_Down, c_shake_minus_rush);
                    }
                }

                move_dir = (move_target - transform.position).normalized;

                break;
            case Boss_State.State.Groggy:
                if(action_phase == 1)
                {
                    transform.position = groggy_point.position;
                    Quaternion quat = Quaternion.identity;
                    quat = Quaternion.Euler(new Vector3(0, 140, 0));

                    //애니메이션 재생이 제대로 이루어질 장소와 회전값을 찾아 넣는다.
                    //그로기 상태로 변화하는 곳의 위치가 고정되어 있다면 그대로 사용이 가능하지만 추후 변한다면....
                    action_phase = 2;
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
    float jump_power;
    public float speed;

    bool rush_attack;

    IEnumerator Rush_Attack_Timer()
    {
        //boss_cry.Play();
        //사운드 재생

        //if (action_state == Action.Ready)
        //    action_state = Action.Up;    //이동을 하지 않기 위함 (이 자리를 공격시작 자리로 정한다.)
        //else action_state = Action.Ready;

        rush_attack_start_pos = transform.position;

        yield return new WaitForSeconds(rush_timer_cnt); // 공격 대기시간
        
        move_target = new Vector3(player.position.x, player.position.y + 2, player.position.z); //내위치 + ((반지름*2) * 이동방향) _ x,z || around_transform의 y위치 
        origin_dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z));  //거리 계산 (실시간 이동 거리에 따라 상승/하락 이동의 지정을 위함)_내 위치(x,z)와 타겟(x,z) 높이는 거리에 반영하지 않으므로!

        action_phase = 2; //다음 행동

        //action_state = Action.Rush_Attack;  //공격상태로 전환
        //speed = rush_attack_speed;          //공격 스피드로 전환
        //jump_power = rush_jump_power;

        //boss_rush_attack[Random.Range(0, 2)].Play();
        //사운드 재생
        rush_attack = true;
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

}
