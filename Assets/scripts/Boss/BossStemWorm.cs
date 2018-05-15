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
        Move_Down, //타겟을향해 내려감
        Death
    }

    //state _ 구조체로 엮어주는게 좋을까?
    //캐릭터들의 상태를 나타내는 변수들////
    [SerializeField]
    int hp;
    [SerializeField]
    float max_speed;
    [SerializeField]
    float normal_speed;
    [SerializeField]
    float min_speed;
    [SerializeField]
    int level;

    float speed;
    Element.Type element = Element.Type.Fire;    //현재 속성 초기속성입니다.
    //////////////////////////////////////

    [SerializeField]
    float distance;  //출발지부터 구멍(hole)까지의 거리(그리고 같은 방향의 타겟끼리의 간격)
    [SerializeField]
    float jump_height;

    int max_level = 3; //최대 레벨 3

    //타겟들의 정보가 담겨있는 구조체
    public struct TargetHole
    {
        public Vector3 target_hole_pos;  //타겟 구멍들의 위치를 저장한다.
        public Vector3 target_hole_pos_mid; //타겟 구멍까지의 거리의 중간을 지정한다. (최고점을 이 중간으로 정함..)
        public int near_rank;  //타겟 구멍-플레이어까지의 거리에 랭크를 매긴다.
        public float player_distance;   //구멍에서 플레이어까지의 거리
    }

    TargetHole[] hole_list; //지상에 올라온 후 내려갈 구멍의 리스트
    Vector3 move_target; //지하에서 이동할 때 갖는 타겟
    Vector3 attack_start_pos;   //지하에서 공격을 시작 할 위치
    [SerializeField]
    Vector3 dis_standard;   //이동 완료로 보는 기준거리를 설정

    int targetnum;

    public Action action_state;

    public NavMeshAgent nav;
    public GameObject player;

    ///////////////////////////////////////////////////

    private void Start()
    {
        speed = normal_speed;
        move_target = player.transform.position;
        action_state = Action.Stop;
        //타겟 리스트를 max_level * 8 만큼 생성
        hole_list = new TargetHole[max_level * 8];
    }

    void Update()
    {
        for (int i = 0; i < 24; i++)
            Debug.DrawRay(hole_list[i].target_hole_pos, Vector3.up * 1000, Color.red);

        if (action_state != Action.Stop)
        {
            if (action_state == Action.Move_Signal_A || action_state == Action.Move_Signal_B) move_on_target();
            if (action_state == Action.Move_Up) move_on_mid();
            if (action_state == Action.Move_Down) move_on_attack();

            boss_move();
        }
        else //정지해있으면 땅 속에서 타겟을 포착한다. (타겟에 따라 A타입과 B타입이 존재함)
        {
            //조건에 따라 타겟을 할당해주고 Action옵션을 부여해줌 (현재는 Action.Move 뿐이지만 필요에 따라 Move종류를 줄 수 있다.)

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (action_state == Action.Move_Up)  //올라갈때만 공격받는다.
        {
            Element other_element = other.GetComponent<Element>();
            if (other_element != null)
            {
                //other의 속성이 내 속성과 같으면 데미지를 입는다.
                if (other_element.type == element)
                {
                    add_damage();
                }
            }
        }
    }

    void add_damage()
    {
        //올바른 공격을 맞았다면 무조건 1씩 닳는다.
        hp--;

        if (hp <= 0)  //만약 hp가 0이 된다면 죽음
        {
            Destroy(this.gameObject);   //현재는 바로 삭제, 
        }
    }

    //move_target의 값으로 이동만 담당하는 함수이다.
    void boss_move()
    {
        this.transform.position += ((move_target - this.transform.position).normalized * speed) * Time.deltaTime;
        this.transform.LookAt(move_target);
    }

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

    //타겟의 위치를 초기화하고 그중 하나의 타겟을 선택하고 "그 타겟으로 이동(공격)을 시작하는 일"이다.
    void move_on_attack()
    {
        if (move_complete(action_state))
        {
            speed = normal_speed;
            action_state = Action.Stop; //이동이 끝난 것으로 정함
        }

    }

    //up할 때 나온 지점에서 빠른 속도로 올라간다. (mid x)
    void move_on_mid()
    {
        move_target = attack_start_pos + new Vector3(0, jump_height, 0);
        speed = max_speed;

        if (move_complete(action_state))
        {
            speed = min_speed;
            StartCoroutine(timer());
            action_state = Action.Move_Down; //이동이 끝난 것으로 정함
            //올라간 순간 내려갈 곳을 선택 (계속 플레이어 움직임을 체크하고 있다가 이 때 정한다.)
            set_player_hole_distance();

        }

    }
    public float time;
    IEnumerator timer()
    {
        yield return new WaitForSeconds(time);

        speed = max_speed;
    }


    //땅 속에서 지상의 타겟을 향해 이동한다 하는 상태일 때 
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

        //이동이 완료되었다면
        if (action_state == Action.Move_End)
        {
            action_state = Action.Move_Up;   //바로 Up상태로 전환

            attack_start_pos = this.transform.position; //시작 위치를 현재 위치로 정한다.
            set_hole_pos(); //그리고 시작 위치를 중심으로 타겟 구멍들의 위치를 초기화하고
        }
    }

 

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

            move_target = _sound_pos;   //움직일 곳을 신호가난 장소로 정해준다.
        }
        receive_complete = false;
    }
    IEnumerator Timer(Vector3 _pos)
    {
        yield return new WaitForSeconds(0.5f);

    }
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
            else if (_state == Action.Move_Down)
            {
                if (transform.position.y < move_target.y) return true;
                else return false;
            }
            else return true;   //올라가능 중이 아니라면 y좌표 체크는 하지 않고 바로 이동완료 체크
        }
        else return false;

    }

}
