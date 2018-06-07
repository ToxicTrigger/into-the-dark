using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Worm : MonoBehaviour
{
    //이동과 기능(hp,공격등의)을 담당 

    public AudioSource boss_cry;

    //꼬리 움직임
    public Boss_Tail[] tail;
    Vector3 tail_dir;       //꼬리 회전계산용
                            /// ///////////////////////////////////////////////////////
    // 기본 정보 (hp, speed 등)

    public enum Action
    {
        Idle = 0,   //대기상태 (플레이어 주위를 뱅글뱅글 돈다.)
        Dash,       //돌진 (소리가 난 곳으로 돌진한다.)
        Rush_Attack,    //포물선 공격
        Rush_Attack_End,
        Soar_Attack,    //솟아오르는 공격
        Groggy,         //그로기 상태
        Groggy_End,   //그로기 끝난 상태
        Death,            //죽음
        Ready           //준비상태 모든 상태가 되기 전 거치는 상태임
    }

    //각 Action상태에 대한 단계를 표시 (rush는 두가지고 soar도 세가지 상태로 나누려 했는데 차라리 이렇게 하는건?)
    public enum Phase
    {
        one,
        two,
        three
    }

    [Space(16)]
    [Header("*Boss State*")]
    public Action action_state;
    public Phase action_phase;
    public float hp;
    public float speed;    //이동속도

    /// // Idle Set// ///
    [Space(16)]
    [Header("*IdleSetting*")]
    [Tooltip("idle상태에서의 속도")]
    public float idle_speed;
    [Tooltip("idle상태에서 캐릭터를 기준으로 얼마만큼 아래에 있을 것인지 입력 (양수)")]
    public float idle_y_pos;
    [Tooltip("캐릭터를 기준으로 얼마만큼의 거리에 떨어져 돌 것인지 (양수)")]
    public float idle_radius;

    [Space(16)]
    [Header("*RushAttackSetting*")]
    [Tooltip("RushAttack상태에서의 속도")]
    public float rush_attack_speed;
    [Tooltip("RushAttack시 강하게 올라오는 정도에 대한 수치 (값이 크면 높이 올라감)")]
    public float rush_jump_power;
    public float rush_jump_top;
    //RushAttack(SoarAttack)시 목표 까지의 거리
    float origin_dis;
    [Space(16)]
    [Tooltip("솟아오를 위치 (높이)")]
    public float soar_height;
    [Tooltip("해당 스테이지의 중간지점, 배치 후 넣어줄 예정")]
    public Transform stage_center;
    public float soar_attack_speed;

    [Tooltip("Idle상태에서 보스가 around_transform을 기준으로 이동")]
    public Transform around_transform;
    public Transform player;
    Vector3 move_dir;   //이동용


    /// //타이머
    public int rush_attack_timer;

    public Vector3 dis_standard;    //이동 완료로 보는 기준거리
    Vector3 move_target;    //Rush_Attack시 타겟으로 삼는 벡터 (솟아오르는 곳에서 캐릭터의 방향 + 반지름 *2 around_transform.y 를 기준으로 한다.)

    private void Start()
    {
        StartCoroutine(start_timer());
        around_transform.position = new Vector3(player.position.x + idle_radius, player.position.y - idle_y_pos, player.position.z);
    }
    IEnumerator start_timer()
    {
        yield return new WaitForSeconds(0.5f);
        action_ready(Action.Idle);
    }
    private void Update()
    {
        around_transform.RotateAround(player.transform.position, Vector3.up, 2f);

        if (Input.GetKey(KeyCode.Space))
            action_ready(Action.Rush_Attack);
        move_control();
        if (action_state == Action.Idle)
            speed = idle_speed;
        if (action_state == Action.Rush_Attack)
            speed = rush_attack_speed;

    }

    private void LateUpdate()
    {
        //꼬리 업데이트
        for (int i = 0; i < tail.Length; i++)
        {
            tail[i].move_update(tail_dir);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //그로기 상태에서만 충돌체크 (현재 충돌체크를 이용하는게 공격받는것 밖에 없음)
        if (action_state == Action.Groggy)
        {
            if (collision.transform.CompareTag("Arrow"))
            {
                add_damage();
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //그로기 상태에서만 충돌체크 (현재 충돌체크를 이용하는게 공격받는것 밖에 없음)
        if (action_state == Action.Groggy)
        {
            if (other.CompareTag("Arrow"))
            {
                add_damage();
                Destroy(other.gameObject);
            }
        }
        if (action_state == Action.Rush_Attack)
        {
            if(other.CompareTag("Player"))
            {
                action_ready(Action.Rush_Attack_End);
            }
        }
    }

    //데미지입는 함수 hp가 0이되면 Destroy
    void add_damage()
    {
        hp -= 1;

        if (hp <= 0)
        {
            action_ready(Action.Death);
            //매니저에게 죽음 상태를 전달
            Destroy(this.gameObject);
        }
        //매니저에게 데미지입음 전달 (데미지 입으면 어떤행동을 할까?)
        action_ready(Action.Soar_Attack);  //현재 데미지를 입으면 Idle 상태로 전환한다.
        BossRoomManager.get_instance().off_switch();
        BossRoomManager.get_instance().set_switch_pos();
    }

    //테스트를 위해 키 입력을 받아 움직임 조정
    void move_control()
    {
        //땅 아래에서 주인공 주위를 뱅글뱅글 돈다.
        if (action_state == Action.Idle) action_idle();
        if (action_state == Action.Rush_Attack) action_rush_attack();
        if (action_state == Action.Rush_Attack_End) action_rush_attack_end();
        if (action_state == Action.Soar_Attack) action_soar_attack();

        if (action_state != Action.Idle && action_state != Action.Rush_Attack)
            transform.position += move_dir * speed * Time.deltaTime;
        tail_dir = move_dir;    //꼬리에 넘겨줄 움직이는 방향을 저장함.
        move_dir = Vector3.zero;
    }

    //idle상태 움직임
    void action_idle()
    {
        //around_transform.RotateAround(player.transform.position, Vector3.up, 2f) ;
        move_dir = (around_transform.position - this.transform.position).normalized;
        transform.position = around_transform.position;
    }

    public float jump_power;
    void action_rush_attack()
    {
        move_target = new Vector3(player.position.x, player.position.y + 10, player.position.z);
        origin_dis = Vector2.Distance(new Vector2(rush_attack_start_pos.x, rush_attack_start_pos.z), new Vector2(move_target.x, move_target.z));

        move_dir = (move_target - transform.position).normalized;  //이동 방향        
        float cur_dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z)); //

        //-> 일정높이까지 진행하는 코드
        //float y_pos = Mathf.Lerp(1.0f, -1.0f, Mathf.Lerp(1,0,rush_attack_origin_dis / cur_dis));
        //float y_pos = Mathf.Lerp(1.0f, -1.0f, (rush_attack_origin_dis - cur_dis) / rush_attack_origin_dis);
        ////Debug.Log(rush_attack_origin_dis);

        //if (y_pos > 0)
        //{
        //    y_pos = Mathf.Lerp(rush_attack_start_pos.y + rush_jump_top, rush_attack_start_pos.y, y_pos);
        //    mid_y_pos = transform.position.y;
        //}
        //else
        //{
        //    if (y_pos < 0)
        //    {
        //        y_pos *= -1;    // 0이하의 값을 가지므로 -1을 곱해주기
        //    }
        //    if (y_pos > 0.95f) rush_attack_end = true;
        //    y_pos = Mathf.Lerp(mid_y_pos, around_transform.position.y, y_pos);
        //}
        //move_dir.y = y_pos - transform.position.y;
        //move_dir.y *= jump_power;
        //-> 일정높이까지 진행하는 코드

        move_dir.y = Mathf.Lerp(1.0f,-1.0f, (origin_dis - cur_dis) / origin_dis - 0.3f);
        if(move_dir.y < 0) move_dir.y*= jump_power;

        transform.position += move_dir  *speed * Time.deltaTime;

        //move_dir = new Vector3(move_dir.x, y_pos, move_dir.z).normalized;

        if (move_complete(action_state))
            action_ready(Action.Rush_Attack_End);  //이동 완료했다면 Idle상태로 변환
    }
    IEnumerator timer;  //**************이렇게 쓰는거 공부해!*************

    Vector3 rush_attack_start_pos;
    IEnumerator Rush_Attack_Timer()
    {
        boss_cry.Play();
        action_state = Action.Ready;    //이동을 하지 않기 위함 (이 자리를 공격시작 자리로 정한다.)
        BossRoomManager.get_instance().send_attack_count_ui(rush_attack_timer);
        rush_attack_start_pos = transform.position;

        yield return new WaitForSeconds(rush_attack_timer); // 공격 대기시간

        //rush_move_target = (player.transform.position - transform.position).normalized;    //이동 방향
        move_target = new Vector3(player.position.x, player.position.y+2, player.position.z); //내위치 + ((반지름*2) * 이동방향) _ x,z || around_transform의 y위치 
        origin_dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z));  //거리 계산 (실시간 이동 거리에 따라 상승/하락 이동의 지정을 위함)_내 위치(x,z)와 타겟(x,z) 높이는 거리에 반영하지 않으므로!

        action_state = Action.Rush_Attack;  //공격상태로 전환
        speed = rush_attack_speed;          //공격 스피드로 전환
        jump_power = rush_jump_power;
    }

    void action_rush_attack_end()
    {
        move_dir = (around_transform.position - transform.position).normalized;

        if (move_complete(action_state))
            action_ready(Action.Idle);
    }

    void action_soar_attack()
    {
        move_dir = (move_target - transform.position).normalized;

        switch (action_phase)
        {
            case Phase.one:
                break;
            case Phase.two:
                break;
            case Phase.three:
                break;
            default:
                break;
        }

        if(move_complete(Action.Soar_Attack))
        {
            action_ready(Action.Idle); 
        }
    }

    // 다른 상태로 변환되기까지 행동을 지정.
    public void action_ready(Action _action)
    {
        switch (_action)
        {
            case Action.Idle:
                //around_transform을 플레이어의 발 아래 + x좌표로 반지름만큼 이동한 위치로 위치를 세팅한다.
                action_state = Action.Idle;
                around_transform.position = new Vector3(player.position.x + idle_radius, player.position.y - idle_y_pos, player.position.z);
                //transform.position = new Vector3(transform.position.x, around_transform.position.y, transform.position.z);

                speed = idle_speed;

                break;
            case Action.Dash:

                break;
            case Action.Rush_Attack:
                // 어그로 신호가 들어올 때 !한번! 해당 함수가 실행되지만. 코루틴 중첩(?)방지를 위해 Stop해준다. 
                if (action_state == Action.Idle) //현재 Idle상태에서만 Rush_Attack을 실행한다.
                {
                    timer = Rush_Attack_Timer();
                    StopCoroutine(timer);
                    StartCoroutine(timer);
                }

                break;
            case Action.Rush_Attack_End:
                speed = rush_attack_speed * 5;
                break;

            case Action.Soar_Attack:
                action_state = Action.Soar_Attack;
                move_target = transform.position + (Vector3.up * soar_height) ;
                speed = soar_attack_speed;
                break;
            case Action.Groggy:
                //공격가능 상태   
                //(임시) 그로기 상태가 되면 현재위치 + 플레이어 y위치로 이동
                transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                action_state = Action.Groggy;
                StopCoroutine(timer); //그로기 상태가 되면 공격을 취소함

                break;
            case Action.Groggy_End:
                //그로기 끝난 상태

                break;
            case Action.Death:

                break;

            default:
                break;
        }
    }

    //이동 완료 함수
    bool move_complete(Action _state)
    {
        switch (_state)
        {
            case Action.Idle:
                break;
            case Action.Dash:
                break;
            case Action.Rush_Attack:
                //Rush_Attack상태에서의 이동 완료 체크.

                //if(around_transform.position.y -5 > tail[tail.Length-1].transform.position.y) 꼬리기준 체크 아래는 머리기준 체크
                if(around_transform.position.y -5 > tail[0].transform.position.y)
                {
                    action_state = Action.Rush_Attack_End;
                    return true;
                }

                break;
            case Action.Rush_Attack_End:
                if (3.0f > Vector3.Distance(around_transform.position, transform.position))
                {
                    action_ready(Action.Idle);
                    return true;
                }
                break;
            case Action.Soar_Attack:

                switch (action_phase)
                {
                    case Phase.one: //솟아오르는 중 y좌표로 목표보다 올라가있으면 이동 완료로 본다.
                        if (transform.position.y > move_target.y)
                        {
                            action_phase = Phase.two;
                            move_target = new Vector3(stage_center.position.x, transform.position.y, stage_center.position.z);
                            //origin_dis = Vector3.Distance(move_target, transform.position);
                            return false;
                        }
                        break;
                    case Phase.two:
                        float cur_dis = Vector3.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z));
                        if (cur_dis < 0.5)
                        {
                            action_phase = Phase.three;
                            move_target = new Vector3(stage_center.position.x, around_transform.position.y, stage_center.position.z);
                            return false;
                        }
                        break;
                    case Phase.three://내려가는 중 y좌표로 목표보다 아래라면 이동 완료로 본다.
                        if (transform.position.y < move_target.y)
                        {
                            action_phase = Phase.one;
                            move_target = Vector3.zero;
                            return true;
                        }
                        break;
                    default:
                        break;
                }

                break;
            case Action.Groggy:
                break;
            case Action.Groggy_End:
                break;
            case Action.Death:
                break;
            case Action.Ready:
                break;
            default:
                break;
        }

        return false;
    }
}

