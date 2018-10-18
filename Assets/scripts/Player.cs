using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    bool is_attack;
    public Animator ani;
    Vector3 click_pos;
    public Weapon weapon;
    public LineRenderer line;
    float origin_move_speed;
    Camera cam;
    public AudioSource bow_fullback, bow_release;

    public List<GameObject> totems;
    //Queue<GameObject> totems;
    [Tooltip("설치된 토템의 갯수")]
    public int cur_totems;
    [Tooltip("설치 가능한 토템의 수")]
    public int installable_totems;
    [Tooltip("토템 프리팹을 넣으세요")]
    public GameObject totem;
    [Tooltip("플레이어가 토템을 겨냥하고 있나?")]
    public bool has_targeting_totem;

    public Damageable damageable;
    [Tooltip("검 모델을 넣으세요. 플레이어의 모델링 안에 들어있습니다.")]
    public GameObject Sword;

    [Tooltip("무언가에게 겨냥 당하고 있음?")]
    public int is_target_something;
    [Tooltip("무언가와 싸우는 중인가?")]
    public bool is_fighting_something;
    [Tooltip("토템을 설치하나?")]
    public bool is_build_totem;

    public GameObject sword_Effect;

    [Range(-1 , 1)]
    public float step_one = 0.04f;
    [Range(-1 , 1)]
    public float step_two = 0.06f;
    [Range(-1 , 1)]
    public float step_three = 0.3f;
    [Range(-1 , 1)]
    public float step_Dodge = 0.15f;
    public ActionCamera ac;
    public PlayerMove move;

    float bow_time;
    [Tooltip("현재 진행중인 애니메이션 이름")]
    public string cur_ani;
    public CharacterController character;
    public AudioSource Foot_Step, Sword_Sound;
    public GameObject Fail_UI;
    
    public void Start()
    {
        character = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        totems = new List<GameObject>();
        damageable = GetComponent<Damageable>();
        cam = Camera.main;
        move = GetComponent<PlayerMove>();

        ac = FindObjectOfType<ActionCamera>();
        ac.SetStateTarget(this.transform , ActionCamera.State.Follow);
    }

    public void setSwordEnable()
    {
        if( !attack_click )
        {
            sword_Effect.GetComponent<TrailRenderer>().Clear();
            sword_Effect.SetActive(false);
            Sword.GetComponent<Collider>().enabled = false;
        }
        else
        {
            Sword.GetComponent<Collider>().enabled = true;
            sword_Effect.SetActive(true);
            sword_Effect.GetComponent<TrailRenderer>().Clear();
            
        }
    }

    void gen_arrow()
    {
        Vector3 mouse = Input.mousePosition;

        GameObject arrow = GameObject.Instantiate(weapon.arrow.gameObject , weapon.fire_point.position , Quaternion.LookRotation(click_pos) , null);
        arrow.GetComponent<Arrow>().look = weapon.fire_point.forward;

        if( has_targeting_totem )
        {
            arrow.GetComponent<Arrow>().has_targeting_totem = true;
        }

        if( bow_time >= 3.2f & bow_time < 4f )
        {
            //강공격 여부 ㅇㅇ 
            arrow.GetComponent<Element>().type = Element.Type.Light;
        }
        Destroy(arrow , 5.0f);
        AggroManager.get_instance().gen_aggro(transform.position , 10 + bow_time , 3);
    }

    void calc_click_pos(bool sword)
    {
        move.set_movement_zero();
        has_targeting_totem = false;
        RaycastHit hit, coll;
        Vector3 mouse = Input.mousePosition;
        if( Physics.Raycast(cam.ScreenPointToRay(mouse) , out hit , 10000) )
        {
            //Debug.Log(hit.transform.gameObject.name);
            click_pos = hit.point;
            click_pos.y = transform.position.y + 0.1f;

            if( !sword )
                line.gameObject.SetActive(true);

            Vector3 pos = transform.position;
            pos.y += 0.5f;

            if( !sword )
            {
                line.SetPosition(0 , pos);
            }

            if( Physics.Linecast(pos , click_pos , out coll) )
            {
                if( coll.collider.CompareTag("TotemAggro") )
                {
                    Debug.DrawLine(pos , click_pos , Color.red);
                    click_pos = coll.collider.gameObject.transform.position;
                    has_targeting_totem = true;
                    if( sword )
                    {
                        line.SetPosition(0 , pos);
                        line.SetPosition(1 , click_pos);
                        line.startColor = Color.blue;
                        line.endColor = Color.yellow;
                    }
                    else
                    {
                        line.startColor = Color.yellow;
                        line.endColor = Color.yellow;
                    }
                }
            }
            else
            {
                line.startColor = Color.white;
                line.endColor = Color.white;
            }
            Debug.DrawLine(pos , click_pos , Color.white);

            if( !sword )
            {
                line.SetPosition(1 , click_pos);
            }

            transform.LookAt(click_pos);
        }
    }

    const int MAX_TOTEM = 5;
    void gen_totem()
    {
        if( totems != null )
        {
            if( Input.GetKeyDown(KeyCode.R) )
            {
                // 먼저 제거할 것을 찾고 있다면 삭제 후 함수를 끝냄
                foreach( var tt in totems )
                {
                    if( Vector3.Distance(tt.transform.position , transform.position) <= 1.4f )
                    {
                        GameObject del = tt;
                        totems.Remove(del);
                        Destroy(del);
                        ++installable_totems;
                        return;
                    }
                }
            }

            if( totems.Count < MAX_TOTEM && installable_totems > 0 )
            {
                if( Input.GetKeyDown(KeyCode.R) )
                {
                    is_build_totem = true;
                    // 토템 생성
                    GameObject t = Instantiate(totem , transform.position , Quaternion.identity , null);
                    totems.Add(t);
                    --installable_totems;

                    // 해당 씬에서 모든 파티클들의 ParticleSystem 을 가져옴
                    ParticleSystem[] fogs = FindObjectsOfType<ParticleSystem>();
                    // 가져온 파티클들 중 Fog 인 것을 골라 담음
                    List<ParticleSystem> tmpFog = new List<ParticleSystem>();
                    foreach( var tmp in fogs )
                    {
                        if( tmp.CompareTag("Fog") )
                        {
                            tmpFog.Add(tmp);
                        }
                    }

                    // 가져온 안개들의 Trigger 에 해당 토템을 추가함
                    foreach( var i in tmpFog )
                    {
                        ParticleSystem trigger = i;
                        trigger.trigger.SetCollider(1 + totems.Count + 1 , t.transform.GetChild(1));
                    }
                }
                if( Input.GetKeyUp(KeyCode.R) )
                {
                    is_build_totem = false;
                }
            }
        }
    }

    void step_ani()
    {
        switch( cur_ani )
        {
            case "Swing_0":
                move.set_movement_zero();
                character.Move(transform.forward.normalized * step_one);
                break;
            case "Swing_1":
                move.set_movement_zero();
                character.Move(transform.forward.normalized * step_two);
                break;
            case "Jump":
                character.Move(transform.forward.normalized * step_three);
                break;
            case "wakeUp":
                character.Move(transform.forward.normalized * step_Dodge * 1.6f);
                ani.SetBool("Dodge" , false);
                break;
            case "Ready":
                character.Move(transform.forward.normalized * step_Dodge * 1.6f);
                break;
            case "Dodge":
                character.Move(transform.forward.normalized * step_Dodge * 2f);
                break;
            case "Dodge1":
                character.Move(transform.forward.normalized * step_Dodge * 1f);
                break;
        }

    }
    Vector3 g;
    void Update_Y_pos()
    {
        if( !character.isGrounded )
        {
            g += Vector3.up * ( Physics.gravity.y * 0.1f ) * Time.deltaTime;
            character.Move(g);
        }
        else
        {
            g = Vector3.zero;
        }
    }

    public bool attack_click;
    public float click_tick;

    void Click_Attack()
    {
        if( attack_click )
        {
            ani.SetBool("Attack" , true);
            weapon.type = Weapon.Type.Sword;
            is_fighting_something = true;
        }

        if( click_tick >= 0.5f )
        {
            click_tick = 0;
            attack_click = false;
            is_fighting_something = false;
            is_attack = false;
            line.gameObject.SetActive(false);
        }
        else
        {
            click_tick += Time.deltaTime;
        }
    }

    void change_weapon_type()
    {
        if( Input.GetKeyDown(KeyCode.Tab) )
        {
            FindObjectOfType<WeaponUI>().changed = false;
            weapon.type = weapon.type != Weapon.Type.Bow ? Weapon.Type.Bow : Weapon.Type.Sword;
        }
    }
    public float num;

    public float dodge_tick;
    bool dodged;
    public void FixedUpdate()
    {
        if( !damageable.Dead )
        {
            setSwordEnable();
            if( !dodged )
            {
                if( Input.GetButton("Dodge") )
                {
                    //TODO :: 회피 코드 수정하기
                    //        현재 바라보는 방향이 아닌 입력되고 있는 방향으로의 회피
                    ani.SetBool("Dodge" , true);
                    dodged = true;
                }
            }
            else
            {
                if(dodge_tick >= 0.05f)
                {
                    dodged = false;
                    dodge_tick = 0;
                }
                else
                {
                    dodge_tick += Time.deltaTime;
                }
            }


            if( !is_attack )
            {
                ani.SetBool("Attack" , false);
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Cancel()
    {
        esc_push = false;
        UI_Ani.SetBool("esc" , esc_push);
    }

    public bool has_on_ladder;
    float end_tick;
    public Animator UI_Ani;
    public bool esc_push;
    public void Update()
    {
        if( Input.GetKeyDown(KeyCode.Escape) )
        {
            if(esc_push)
            {
                esc_push = false;
            }
            else
            {
                esc_push = true;
            }
            UI_Ani.SetBool("esc" , esc_push);
        }
        
        if( damageable.Dead )
        {
            Fail_UI.SetActive(true);
        }
        else
        {
            if( end_tick <= 1.0f )
            {
                end_tick += Time.deltaTime;
            }
            else
            {
                damageable.Damaged(0.5f , 0);
                end_tick = 0;
            }

            if( !has_on_ladder )
                Update_Y_pos();

            Vector3 tmp = transform.position;
            tmp.y = 10f;
            gen_totem();
            cur_ani = ani.GetCurrentAnimatorClipInfo(0)[ 0 ].clip.name;
            step_ani();
            ani.SetFloat("Bow_Fire" , bow_time);
            change_weapon_type();
            Click_Attack();

            if( Input.GetButtonDown("Fire1") )
            {
                if( weapon.type.Equals(Weapon.Type.Sword) )
                {
                    attack_click = true;
                    click_tick = 0;
                }
            }
            else if( Input.GetButton("Fire1") )
            {
                if( !weapon.type.Equals(Weapon.Type.Sword) )
                {
                    bow_time += Time.deltaTime;
                    if( !bow_fullback.isPlaying ) bow_fullback.Play();
                    is_fighting_something = true;
                    calc_click_pos(false);
                }
            }

            if( Input.GetMouseButtonUp(0) )
            {
                is_fighting_something = false;
                if( weapon.isUsing )
                {
                    if( bow_time >= 0.5f )
                    {
                        calc_click_pos(false);
                        gen_arrow();

                        bow_time = 0f;
                        line.gameObject.SetActive(false);
                        if( !bow_release.isPlaying )
                        {
                            bow_fullback.Stop();
                            bow_release.Play();
                        }
                    }
                    else
                    {
                        bow_time = 0f;
                        line.gameObject.SetActive(false);
                        if( !bow_release.isPlaying )
                        {
                            bow_fullback.Stop();
                        }
                    }
                }
            }
        }
    }
}
