using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Worm : MonoBehaviour {
    //이동과 기능(hp,공격등의)을 담당 

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
        Soar_Attack,    //솟아오르는 공격
        Groggy,         //그로기 상태
        Groggy_End,   //그로기 끝난 상태
        Death,            //죽음
        Ready           //준비상태 모든 상태가 되기 전 거치는 상태임
    }

    public Action action_state;
    public float hp;
    float speed;    //이동속도

    /// // Idle Set// ///
    [Tooltip("idle상태에서 캐릭터를 기준으로 얼마만큼 아래에 있을 것인지 입력 (양수)")]
    public float idle_y_pos;    
    [Tooltip("캐릭터를 기준으로 얼마만큼의 거리에 떨어져 돌 것인지 (양수)")]
    public float idle_radius;


    public float idle_speed;
    public float rush_attack_speed;
    public float rush_attack_origin_dis;
    public float min_speed;

    public Transform around_transform;
    public Transform player;
    Vector3 move_dir;   //이동용

    public Vector3 dis_standard;    //이동 완료로 보는 기준거리

    private void Start()
    {
        action_ready(Action.Idle);
        around_transform.position = new Vector3(player.position.x + idle_radius, player.position.y - idle_y_pos, player.position.z);
    }

    private void Update()
    {
            move_control();
    }

    //테스트를 위해 키 입력을 받아 움직임 조정
    void move_control()
    {
        //땅 아래에서 주인공 주위를 뱅글뱅글 돈다.
        if(action_state == Action.Idle)  action_idle();
        if (action_state == Action.Rush_Attack) action_rush_attack();

        //if (Input.GetKey(KeyCode.A))
        //{
        //    move_dir += Vector3.left;
        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    move_dir += Vector3.right;
        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    move_dir += Vector3.forward;
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    move_dir += Vector3.back;
        //}

        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    move_dir += Vector3.up;
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    move_dir += Vector3.down;
        //}

        transform.position += move_dir * speed * Time.deltaTime;
        tail_dir = move_dir;    //꼬리에 넘겨줄 움직이는 방향을 저장함.
        move_dir = Vector3.zero;

    }

    private void LateUpdate()
    {
        //꼬리 업데이트
        for (int i = 0; i < tail.Length; i++)
        {
            tail[i].move_update(tail_dir);
        }
    }

    
    //idle상태 움직임
    void action_idle()
    {
        around_transform.RotateAround(player.transform.position, Vector3.up, 1f) ;
        move_dir = (around_transform.position - this.transform.position).normalized;
    }

    bool l;
    void action_rush_attack()
    {
        move_dir = (move_target - transform.position).normalized;
        move_dir.y = Mathf.Lerp(1.0f, -1.0f, (rush_attack_origin_dis / Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z))) /10.0f);
        if(!l)StartCoroutine(C_timer());
    }

    // 다른 상태로 변환되기까지 행동을 지정.
    public void action_ready (Action _action)
    {
        switch (_action)
        {
            case Action.Idle:
                //플레이어의 발 아래 + x좌표로 반지름만큼 이동한 위치로 위치를 세팅한다.
                action_state = Action.Idle;
                speed = idle_speed;
                break;
            case Action.Dash:

                break;
            case Action.Rush_Attack:
                if(!timer)StartCoroutine(Rush_Attack_Timer());
                Debug.Log("aa");
                break;
            case Action.Soar_Attack:

                break;
            case Action.Groggy:

                break;
            case Action.Groggy_End:

                break;
            case Action.Death:

                break;

            default:
                break;
        }
    }
    bool timer;
    Vector3 move_target;
    public float rush_attack_timer;
    IEnumerator Rush_Attack_Timer()
    {
        timer = true;
        action_state = Action.Ready;
        move_target = (player.transform.position - transform.position).normalized ;
        move_target = new Vector3(transform.position.x+((idle_radius * 2) * move_target.x), transform.position.y - 4, transform.position.z+ ((idle_radius * 2)*move_target.z));
        rush_attack_origin_dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(move_target.x, move_target.z));

        yield return new WaitForSeconds(rush_attack_timer);

        Debug.Log("rush");
        action_state = Action.Rush_Attack;
        speed = rush_attack_speed;
        timer = false;
    }

    IEnumerator C_timer()
    {
        l = true;
        yield return new WaitForSeconds(2.0f);
        action_ready(Action.Idle);
        l = false;
    }

    //bool move_complete(Action _state)
    //{
    //    //if (_state == Action.Rush_Attack)
    //    //{
    //    //
    //    //    if (transform.position.y < move_target.y) return true;
    //    //
    //    //    return false;
    //    //
    //    //}
    //    //return false;
    //    ////만약 x,z 좌표상에서 범위에 들어왔는데
    //    //if (transform.position.x > move_target.x - dis_standard.x &&
    //        //transform.position.x < move_target.x + dis_standard.x &&
    //        //transform.position.z > move_target.z - dis_standard.z &&
    //        //transform.position.z < move_target.z + dis_standard.z)
    //    //{

    //        //else return true;   //올라가능 중이 아니라면 y좌표 체크는 하지 않고 바로 이동완료 체크
    //    //}
    //    //else return false;

    //}
}

