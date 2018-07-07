using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStemWorm : MonoBehaviour {
    //줄기 지렁이(?) 보스

    public enum Action
    {
        Stop = 0,
        Move_Signal_A,   //땅속에서 이동상태
        Move_Signal_B,
        Move_End,   //땅속 이동 끝
        Move_Up,    //솟아오름
        Move_Attack,
        Groggy,     //그로기상태
        Groggy_End, //그로기 해제 상태
        Death
    }

    //state _ 구조체로 엮어주는게 좋을까?
    //캐릭터들의 상태를 나타내는 변수들////
    [SerializeField]
    int hp;

    [SerializeField]
    float normal_speed;
    [SerializeField]
    float min_speed;
    [SerializeField]
    int level;

    float jump;
    float speed;
    Element.Type element = Element.Type.Fire;    //현재 속성 초기속성입니다.
    //////////////////////////////////////

    [SerializeField]
    float distance;  //출발지부터 구멍(hole)까지의 거리(그리고 같은 방향의 타겟끼리의 간격)
    [SerializeField]
    float jump_height;

    //int max_level = 3; //최대 레벨 3

    //타겟들의 정보가 담겨있는 구조체
    public struct TargetHole
    {
        public Vector3 target_hole_pos;  //타겟 구멍들의 위치를 저장한다.
        public Vector3 target_hole_pos_mid; //타겟 구멍까지의 거리의 중간을 지정한다. (최고점을 이 중간으로 정함..)
        public int near_rank;  //타겟 구멍-플레이어까지의 거리에 랭크를 매긴다.
        public float player_distance;   //구멍에서 플레이어까지의 거리
    }

    //TargetHole[] hole_list; //지상에 올라온 후 내려갈 구멍의 리스트
    Vector3 move_target; //지하에서 이동할 때 갖는 타겟
//Vector3 attack_start_pos;   //지하에서 공격을 시작 할 위치
    [SerializeField]
    Vector3 dis_standard;   //이동 완료로 보는 기준거리를 설정

    int targetnum;

    public Action action_state;

    public GameObject player;

    Vector3 normal_position;    //땅속에서의 기본 위치

    ///////////////////////////////////////////////////

    private void Start()
    {
        normal_position = this.transform.position;  //초기위치로 한다.
        speed = normal_speed;
        move_target = player.transform.position;
        action_state = Action.Stop;
        //타겟 리스트를 max_level * 8 만큼 생성
        //hole_list = new TargetHole[max_level * 8];
    }


    void OnTriggerEnter(Collider other)
    {
        if (action_state == Action.Groggy)  
        {
                //other의 속성이 내 속성과 같으면 데미지를 입는다.
                if (other.CompareTag("Arrow"))  //임의로 태그를 달았음, 추후 속성으로 체크할것, 빛?
                {
                    add_damage();
                }
        }
    }

    void add_damage()
    {
        //올바른 공격을 맞았다면 무조건 1씩 닳는다.
        hp--;
        BossRoomManager.get_instance().off_switch();
        action_state = Action.Groggy_End;

        if (hp <= 0)  //만약 hp가 0이 된다면 죽음
        {
            Destroy(this.gameObject);   //현재는 바로 삭제, 
        }
    }
    float temp_jump;
    void Update()
    {
        //그로기 액션상태가 아닐 때만 이동하고 기타 행동을함,
        if (action_state != Action.Stop && action_state != Action.Groggy && action_state != Action.Groggy_End)
        {
            if (action_state == Action.Move_Signal_A || action_state == Action.Move_Signal_B) move_on_target();
            if (action_state == Action.Move_Up) move_on_up();
            if (action_state == Action.Move_Attack) move_on_attack();

            boss_move();
        }
        else if(action_state == Action.Groggy)  //그로기 상태라면
        {
            if(!groggy_timer_on) groggy_action();
        }
        else if(action_state == Action.Groggy_End)
        {
            action_state = Action.Stop;
            this.transform.position = normal_position;  //그로기 상태에서 해제한 후 위치를 기본 위치로 바꾼다.
            groggy_timer_on = false;
        }
    }
    bool groggy_timer_on;

    //move_target으로 이동 
    void boss_move()
    {
        if (action_state == Action.Move_Attack)
        {
            check_distance();
        }
        else jump = 0;

        this.transform.position += new Vector3((move_target - this.transform.position).normalized.x, jump, (move_target - this.transform.position).normalized.z) * speed * Time.deltaTime;
        
        boss_lookat();
    }

    [Space(32)]
    [Tooltip("솟아 오르는 각도")]
    public float x_angle_start = -90.0f;
    [Tooltip("들어가는 각도")]
    public float x_angle_end = 90.0f;
    float x_angle = -9999;

    [Space(16)]

    [Tooltip("최대 속도(현재 공격시에 적용되는 속도임)")]
    public float max_speed;
    [Tooltip("공격시 점프하는 힘클수록 높이 점프함")]
    public float jump_power;
    [Tooltip("이동완료 후 공격시작까지 걸리는 시간")]
    public float attack_timer = 0.5f;

    //lookat 함수 어택시 boss의 각도에 따라 보는 곳을 정해주려고 만들어줌...
    void boss_lookat()  
    {
        Quaternion Angle = Quaternion.identity;
        Vector3 v = move_target - this.transform.position;
        float angle = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        //높이/밑변을 하면 두 변이 이루는 각의 결과값이 나옴 (Matfh.Atan2(높이,밑변))
        //Mathf.Rad2Deg 는 저렇게 계산하면 나오는 값이 라디안 값인데 이걸 디그리값으로 바꿔주는 것이다.

        if (action_state == Action.Move_Attack)
        {
            x_angle = Mathf.Lerp(x_angle_start, x_angle_end, (origin_distance / Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.z), new Vector2(move_target.x, move_target.z))) / 10.0f);
            //거리는 멀수록 값이 큼 때문에 값을 반대로 넣어줌

        }
        else
            x_angle = 0;

        Angle.eulerAngles = new Vector3(x_angle, angle, 0);

        this.transform.rotation = Angle;
    }


    //신호 A와 B를 받은 후 이동한다. 
    void move_on_target()
    {
        if (action_state == Action.Move_Signal_A)
        {
            if (move_complete(action_state))
            {
                action_state = Action.Move_End;
            }
        }

        if (action_state == Action.Move_Signal_B)
        {
            if (move_complete(action_state))
            {
                action_state = Action.Move_End;
            }
        }

        //이동이 완료되었다면 신호에 따라 상승공격과 돌진공격을 실행한다. (현재는 돌진공격만 적용)
        if (action_state == Action.Move_End)
        {
           // attack_start_pos = this.transform.position; //공격 시작 위치를 현재 위치로 저장한다. (구멍에서 솟아오를 부분)
            if (!on_timer)  //코루틴 중복실행(?)방지용
                StartCoroutine(AttackTimer());  //코루틴 실행 잠깐의 시간을 갖고 플레이어를 포착할 예정이므로 타이머를 넣어줌..
        }
    }

    bool on_timer = false;
    float origin_distance;  //공격시 계산되는 gameobject와 타겟의 거리 계산값의 초기값 (?)

    IEnumerator AttackTimer()   //일정 시간이 지난 후 플레이어 위치를 타겟으로 설정
    {
        on_timer = true;
        temp_jump = jump_power; //임시로
        yield return new WaitForSeconds(attack_timer);  //지정한 시간만큼 기다림

        speed = max_speed;  //attack시 공격 속도 빠르게 해줌
        move_target = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);

        origin_distance = Vector2.Distance(new Vector2(move_target.x, move_target.z),
                                            new Vector2(this.transform.position.x, this.transform.position.z)); 
        if(action_state != Action.Groggy)action_state = Action.Move_Attack;  //지금부터 주어진 정보를 이용해 공격 시작!
        on_timer = false;   //다시 타이머 사용이 가능하게 세팅
    }

    //현재는 공격이 끝났는지 체크해주는 용도임.. 
    void move_on_attack()
    {
        if (move_complete(action_state))
        {
            //속도를 기본값으로 돌린다.
            speed = normal_speed;
            action_state = Action.Stop; //이동이 끝난 것으로 정함
            transform.position = new Vector3(transform.position.x, move_target.y, transform.position.z);
        }

    }

    //attack시 거리 체크
    void check_distance()
    {
        speed = max_speed;

        if (Vector2.Distance(new Vector2(move_target.x, move_target.z), new Vector2(this.transform.position.x, this.transform.position.z))
            < origin_distance / 2)
        {
            jump = -temp_jump;
            temp_jump += 0.05f;
        }
        else
        {
            jump = temp_jump;
            if (temp_jump >= 0) temp_jump -= 0.05f;
        }
    }

    //up할 때 나온 지점에서 빠른 속도로 올라간다. (mid x)
    void move_on_up()
    {

    }

    //신호받는 함수 
    bool receive_complete = false;
    public void signal_receive(Vector3 _sound_pos, string _signal_type)
    {
        if (action_state == Action.Move_Signal_A ||
            action_state == Action.Move_Signal_B ||
            action_state == Action.Stop)//땅 속에서 움직일 때! (A의 신호를 받았는데 또 A의 신호를 받을 수 있는가? (A도중에 다른 A로의 이동을 하는가?))
        {
            if (_signal_type == "A")    //일단 A는 땅 속에서 받는 신호에 무조건적인 우선순위를 가지고있음.
            {
                action_state = Action.Move_Signal_A;
                receive_complete = true;
            }
            else if (_signal_type == "B")
            {
                //현재 A신호를 받지 않았을 때만 Signal B의 영향을 받게한다. (A가 우선순위가 높음)
                if (action_state != Action.Move_Signal_A)
                {
                    action_state = Action.Move_Signal_B;
                    receive_complete = true;
                }
            }
            else Debug.LogError("receive signal type Error");
        }

        if (receive_complete) {

            move_target = new Vector3(_sound_pos.x, this.transform.position.y, _sound_pos.z);   //움직일 곳을 신호가 난 장소로 정해준다.
        }
        receive_complete = false;
    }




    //이동완료했는지 체크하는 함수
    bool move_complete(Action _state)
    {
        //만약 x,z 좌표상에서 범위에 들어왔는데
        if (transform.position.x > move_target.x - dis_standard.x &&
            transform.position.x < move_target.x + dis_standard.x &&
            transform.position.z > move_target.z - dis_standard.z &&
            transform.position.z < move_target.z + dis_standard.z)
        {
            if (_state == Action.Move_Up)    //올라가는 중이었다면 y좌표도 마저 체크한다.
            {
                if (transform.position.y > move_target.y - dis_standard.y &&
                    transform.position.y < move_target.y + dis_standard.y)
                {
                    return true;    //this의 y위치가 타겟의 y위치보다 높이 올라갔다면 바로 이동완료로 보고 내려간다.
                }
                else return false;  //x,z는 완료했으나 y가 아직 범위에 안들어왔다면 이동완료 x!
            }
            else if(_state == Action.Move_Attack)
            {

                if (transform.position.y < move_target.y) return true;

                else return false;

            }
            else return true;   //올라가능 중이 아니라면 y좌표 체크는 하지 않고 바로 이동완료 체크
        }
        else return false;

    }

    /// /그로기/ ////
    
    //그로기 상태일 때 액션한다. (데미지를 입거나, 모습을 보인다.)
    void groggy_action()
    {
        groggy_timer_on = true;
        this.transform.position = new Vector3(this.transform.position.x, normal_position.y + 2, this.transform.position.z);   //일단 현재 위치에서 플레이어의 높이까지 모습을 드러냄 (추후 수정)
    }

    /// /상태/ ///
    
    public void state_change(Action _state)
    {
        action_state = _state;
    }

    /// /속성/ ///
    
    //불, 물속성을 세팅함
    void set_element()
    {
        //랜덤으로

    }

    //불, 물속성에서 반대되는 속성으로 바꾸는 함수
    void reverse_element()
    {
        //불이면 물로 물이면 불로 바꾼다.
        if (element == Element.Type.Fire)
            element = Element.Type.Water;
        else if (element == Element.Type.Water)
            element = Element.Type.Fire;
    }

    //불, 물속성에서 무속성으로 해제한다.
    void clear_element()
    {
        if (element == Element.Type.Fire || element == Element.Type.Water)  //혹시 모르니 체크 불,물에서 돌아가므로 
            element = Element.Type.Void;
    }

    public Action get_action_state()
    {
        return action_state;
    }
}





/* 보스 공격 방식이 변경되며 필요 없어진 코드...
 * 보스의 정해진 영역에 타겟의 위치를 쫙 세팅해주고 플레이어 위치에 제일 가까운 타겟을 선택해주는 함수임...
 * 
    //보스가 나온 후 들어갈 "타겟들의 위치를" 설정한다. 
    void set_hole_pos()
    {
        int level_up_num = 0;
        int dis_level = 1;

        for (int i = 1; i < 4; i++)
        {
            hole_list[0 + level_up_num].target_hole_pos = new Vector3(attack_start_pos.x - distance * dis_level, attack_start_pos.y, attack_start_pos.z + distance * dis_level);
            hole_list[1 + level_up_num].target_hole_pos = new Vector3(attack_start_pos.x, attack_start_pos.y, attack_start_pos.z + distance * dis_level);
            hole_list[2 + level_up_num].target_hole_pos = new Vector3(attack_start_pos.x + distance * dis_level, attack_start_pos.y, attack_start_pos.z + distance * dis_level);
            hole_list[3 + level_up_num].target_hole_pos = new Vector3(attack_start_pos.x + distance * dis_level, attack_start_pos.y, attack_start_pos.z);
            hole_list[4 + level_up_num].target_hole_pos = new Vector3(attack_start_pos.x + distance * dis_level, attack_start_pos.y, attack_start_pos.z - distance * dis_level);
            hole_list[5 + level_up_num].target_hole_pos = new Vector3(attack_start_pos.x, attack_start_pos.y, attack_start_pos.z - distance * dis_level);
            hole_list[6 + level_up_num].target_hole_pos = new Vector3(attack_start_pos.x - distance * dis_level, attack_start_pos.y, attack_start_pos.z - distance * dis_level);
            hole_list[7 + level_up_num].target_hole_pos = new Vector3(attack_start_pos.x - distance * dis_level, attack_start_pos.y, attack_start_pos.z);
            if (i == 1)
            {
                if (level >= 2) { dis_level = 2; level_up_num = 8; }
                else
                {
                    break;
                }
            }

            if (i == 2)
            {
                if (level >= 3) { dis_level = 3; level_up_num = 16; }
                else
                {
                    break;
                }
            }
        }
    }

    int[] num;
    int cnt;

    //타겟 리스트의 near_rank의 상위 N개 정도를 뽑아서 랜덤으로 돌려 1개를 최종으로 선택한다. 
    void select_hole()
    {
        cnt = 0;
        num = new int[5];
        for (int i = 0; i < hole_list.Length; i++)
        {
            if (hole_list[i].near_rank < 6 && cnt < 5)
            {
                num[cnt] = i;
                cnt++;
            }
        }

        int ran = Random.Range(0, 5);
        //targetnum = num[ran];
        targetnum = num[0];

    }

        bool on = false;
    bool set_hole = false;
    //플레이어랑 구멍사이의 거리 세팅
    void set_player_hole_distance()
    {
        for(int i =0; i< 8*level; i++)  //레벨에 8을 곱한 만큼이 최대값 
        {
            hole_list[i].player_distance = Vector3.Distance(player.transform.position, hole_list[i].target_hole_pos);   //하나씩 거리값을 넣어줌
        }

        int temp = -1;
        for (int i = 0; i < 8 * level; i++)
        {
            if (temp == -1) temp = i;
            else
            {
                temp = hole_list[i].player_distance < hole_list[temp].player_distance ? i : temp;
            }
        }
        targetnum = temp;
        move_target = hole_list[temp].target_hole_pos;
    }


 */
