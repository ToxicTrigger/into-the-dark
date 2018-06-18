using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.UI;

public class Player : MonoBehaviour{
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
    
    public Text totem_cnt;
    public Damageable damageable;
    [Tooltip("검 모델을 넣으세요. 플레이어의 모델링 안에 들어있습니다.")]
    public BoxCollider Sword;

    [Tooltip("무언가에게 겨냥 당하고 있음?")]
    public int is_target_something;
    [Tooltip("무언가와 싸우는 중인가?")]
    public bool is_fighting_something;
    [Tooltip("토템을 설치하나?")]
    public bool is_build_totem;

    public GameObject sword_Effect;

    [Range(-1, 1)]
    public float step_one = 0.04f;
    [Range(-1, 1)]
    public float step_two = 0.06f;
    [Range(-1, 1)]
    public float step_three = 0.3f;
    [Range(-1, 1)]
    public float step_Dodge = 0.15f;

    float bow_time;
    [Tooltip("현재 진행중인 애니메이션 이름")]
    public string cur_ani;
    public CharacterController character;

    public AudioSource Foot_Step, Sword_Sound;



    public void Start()
    {
        character = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        totems = new List<GameObject>(3);
        damageable = GetComponent<Damageable>();
        cam = Camera.main;
    }

    public void setSwordEnable(int val)
    {
        if (val == 0)
        {
            //sword_Effect.SetActive(false);
            Sword.enabled = false;
        }
        else
        {
            //sword_Effect.SetActive(true);
            Sword.enabled = true;
            Sword_Sound.PlayOneShot(Sword_Sound.clip);
        }
    }

    void gen_arrow()
    {
        Vector3 mouse = Input.mousePosition;

        Vector3 nor = transform.forward.normalized;
        GameObject arrow = GameObject.Instantiate(weapon.arrow.gameObject, weapon.fire_point.position, Quaternion.LookRotation(click_pos), null);
        arrow.GetComponent<Arrow>().look = weapon.fire_point.forward;
        if(has_targeting_totem)
        {
            arrow.GetComponent<Arrow>().has_targeting_totem = true;
        }
        

        if (bow_time >= 3.2f & bow_time < 4f)
        {
            //강공격 여부 ㅇㅇ 
        }
        Destroy(arrow, 5.0f);
        AggroManager.get_instance().gen_aggro(transform.position, 10 + bow_time, 3);
    }

    void calc_click_pos(bool sword)
    {
        has_targeting_totem = false;
        RaycastHit hit, coll;
        Vector3 mouse = Input.mousePosition;
        if (Physics.Raycast(cam.ScreenPointToRay(mouse), out hit, 10000))
        {
            click_pos = hit.point;
            click_pos.y = transform.position.y + 0.1f;

            if( !sword )
            line.gameObject.SetActive(true);

            Vector3 pos = transform.position;
            pos.y += 0.5f;

            if (!sword)
            {
                line.SetPosition(0, pos);
            }
                
            if (Physics.Linecast(pos, click_pos, out coll))
            {
                if(coll.collider.CompareTag("TotemAggro"))
                {
                    Debug.DrawLine(pos, click_pos, Color.red);
                    click_pos = coll.collider.gameObject.transform.position;
                    has_targeting_totem = true;
                    if(sword)
                    {
                        line.SetPosition(0, pos);
                        line.SetPosition(1, click_pos);
                        line.startColor = Color.blue;
                        line.endColor = Color.yellow;
                    }else{
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
            Debug.DrawLine(pos, click_pos, Color.white);

            if (!sword)
            {
                line.SetPosition(1, click_pos);
            }
            
            transform.LookAt(click_pos);
        }
    }
    int totem_size;
    void gen_totem()
    {
        if(totems != null)
        {
            cur_totems = totems.Count;
            installable_totems = 3 - cur_totems;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                is_build_totem = true;

                IEnumerator iter = totems.GetEnumerator();
                int i = 0;
                while(iter.MoveNext())
                {
                    Debug.Log("Totem:" + i);
                    GameObject tmp = iter.Current as GameObject;
                    if (Vector3.Distance(tmp.transform.position, transform.position) <= 1.4f)
                    {   
                        Destroy(totems[i].gameObject);
                        totems.RemoveAt(i);
                        Debug.Log("Totem:" + i + " is Over");
                    }
                    i+=1;
                }

                if (totems.Count > 2)
                {
                    Destroy(totems[0].gameObject);
                    totems.RemoveAt(0);
                }

                if( totem_size > 2)
                {
                    totem_size = 0;
                }

                GameObject t = Instantiate(totem, transform.position, Quaternion.identity, null);
                totems.Insert(totems.Count , t);
                ParticleSystem[] fogs = FindObjectsOfType<ParticleSystem>();
                List<ParticleSystem> tmpFog = new List<ParticleSystem>();
                foreach(var tmp in fogs)
                {
                    if(tmp.CompareTag("Fog"))
                    {
                        tmpFog.Add(tmp);
                    }
                }

                IEnumerator it = tmpFog.GetEnumerator();
                while(it.MoveNext())
                {
                    ParticleSystem trigger = it.Current as ParticleSystem;
                    trigger.trigger.SetCollider(1 + totems.Count + 1, t.transform.GetChild(0));
                }

                //ParticleCollider.instance.ps.trigger.SetCollider(1 + totems.Count + 1, t.transform.GetChild(0));
            }
            if(Input.GetKeyUp(KeyCode.Q))
            {
                is_build_totem = false;
            }
        }
    }
    public void FixedUpdate()
    {
                if(Input.GetButton("Dodge"))
        {
            ani.SetFloat("Forward", 0.0f);
            ani.SetFloat("Turn", 0.0f);
            ani.SetBool("Dodge", true);
            //Input.ResetInputAxes();
        }    
        if (!is_attack)
        {
            ani.SetBool("Attack", false);
        }
    }
    //넌 조만간 죽는다.
    void step_ani()
    {
        switch (cur_ani)
        {
            case "Swing_0":
                character.Move(transform.forward.normalized * step_one);
                //transform.position += transform.forward.normalized * step_one;
                break;
            case "Swing_1":
                character.Move(transform.forward.normalized * step_two);
                //transform.position += (transform.forward.normalized * step_two);
                break;
            case "Jump":
                character.Move(transform.forward.normalized * step_three);
                //transform.position += (transform.forward.normalized * step_three);
                break;
            case "wakeUp":
                ani.SetBool("Dodge", false);
                break;
            case "Dodge":
                character.Move(transform.forward.normalized * step_Dodge);
                //transform.position += (transform.forward.normalized * step_Dodge);
                break;
        }
    
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        
    }

    void Update_Y_pos()
    {
        if(!character.isGrounded)
        {
            character.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);
            Debug.Log("Fly");
        }
    }

    public bool attack_click;
    public float click_tick;

    void Click_Attack()
    {
        if(attack_click)
        {
            ani.SetBool("Attack", true);
            weapon.type = Weapon.Type.Sword;
            is_fighting_something = true;
        }

        if(click_tick >= 0.3f)
        {   
            click_tick = 0;
            attack_click = false;
            is_fighting_something = false;
            weapon.type = Weapon.Type.Idle;
            is_attack = false;
            line.gameObject.SetActive(false);
        }else{
            click_tick += Time.deltaTime;
        }
    }

    void Update () {
        Update_Y_pos();

        Vector3 tmp = transform.position;
        tmp.y = 10f;
        gen_totem();
        cur_ani = ani.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        step_ani();


        if (Input.GetButtonDown("Fire1"))
        {
            attack_click = true;
            click_tick =0;
        }
        
        Click_Attack();

        if (Input.GetButton("Fire2"))
        {
            weapon.type = Weapon.Type.Bow;
            bow_time += Time.deltaTime;
            if(!bow_fullback.isPlaying)
            {
                bow_fullback.Play();
            }
            is_fighting_something = true;
            //tpc.m_MoveSpeedMultiplier = 0;
            calc_click_pos(false);
        }
        ani.SetFloat("Bow_Fire", bow_time);

        if (Input.GetMouseButtonUp(1))
        {
            is_fighting_something = false;
            if (weapon.isUsing)
            {
                if (bow_time >= 0.5f)
                {
                    calc_click_pos(false);
                    gen_arrow();

                    bow_time = 0f;
                    weapon.type = Weapon.Type.Idle;
                    line.gameObject.SetActive(false);
                    if (!bow_release.isPlaying)
                    {
                        bow_fullback.Stop();
                        bow_release.Play();
                    }
                }
                else
                {
                    bow_time = 0f;
                    weapon.type = Weapon.Type.Idle;
                    line.gameObject.SetActive(false);
                    if (!bow_release.isPlaying)
                    {
                        bow_fullback.Stop();
                    }
                }
            }
        }
    }
}
