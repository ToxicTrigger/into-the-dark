using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class AIAction : MonoBehaviour
{
    public enum State
    {
        Idle,
        Action,
        Attack,
        Hit,
        Groggy,
        Dead,
        Skill,
        Move,
        Sleep
    }

    public AIAction.State state;

    private Player Player;

    private Vector3 origin;

    [SerializeField]
    private float Speed;

    public bool find_target;
    public bool end_cut_scene;

    public Damageable damageable;
    public GameObject Poison;
    public Detecter detecter;
    public Animator ani;

    public GameObject DeadEffect;

    public void Start()
    {
        state = State.Sleep;
        origin = transform.position;
        damageable = GetComponent<Damageable>();
        Player = FindObjectOfType<Player>();
        StartCoroutine(FSM());
        detecter = transform.GetChild(0).GetComponent<Detecter>();
        ani = GetComponent<Animator>();
    }

    public float cool_down, attack_speed;
    public float cooldown = 0;
    public float attack_tick = 0;
    public float cooltick = 0;
    public int count = 0;

    public bool attack_end;

    /*  중간 보스 공격
     *  해당 보스의 공격 패턴은 다음과 같다.
     *  1. 플레이어 거리 확인
     *  2. 사거리 내에 있을 때 공격신호 확인
     *  3. 총 세번의 공격을 하는데 해당 공격이 끝나면 쿨타임을 가진 후 다시 3 반복
     *  4. 중간에 만약 플레이어가 벗어난다면 공격 카운트 초기화 후 1 로 회귀
     *  
     *  가능하다면 보스의 쿨타임을 조절할 수 있도록 하는 것이 좋을 것 같음.
     *  
     *  공격을 이루는 요소는 공격과 공격 사이의 시간, 모든 공격이 끝나고 쿨타임이 있다.
     */
    IEnumerator FSM()
    {

        while( this.gameObject != null )
        {
            switch( state )
            {
                case State.Action:
                    FindObjectOfType<MidHp>().start = true;
                    ani.applyRootMotion = false;
                    ani.SetBool("isAction" , true);
                    yield return new WaitForSeconds(6);
                    ani.SetBool("isAction" , false);
                    state = State.Attack;
                    ani.applyRootMotion = true;
                    break;

                case State.Attack:
                    transform.LookAt(Player.transform);
                    if(cooldown <= cool_down)
                    {
                        cooldown += Time.deltaTime;
                    }
                    else
                    {
                        if(count < 3)
                        {
                            if(cooltick <= 1)
                            {
                                if (attack_tick <= attack_speed)
                                {
                                    attack_tick += Time.deltaTime;
                                    ani.SetBool("isAttack", false);
                                }
                                else
                                {
                                    ani.SetBool("isAttack", true);
                                    //yield return new WaitForSeconds(0.3f);
                                    Vector3 pos = transform.position;
                                    GameObject poison = Instantiate(Poison, pos, Quaternion.identity, null);
                                    poison.transform.position = transform.position;
                                    poison.GetComponent<Rigidbody>().useGravity = true;
                                    poison.GetComponent<Rigidbody>().velocity = (Player.transform.position - pos).normalized * Vector3.Distance(transform.position, Player.transform.position) / 2;
                                    poison.GetComponent<Rigidbody>().AddForce(Vector3.up * 10, ForceMode.Impulse);
                                    Player.ac.Shake(4, 0.4f, Time.deltaTime);
                                    attack_tick = 0;
                                    count += 1;
                                    cooltick = 0;
                                }

                            }
                            else
                            {
                                cooltick += Time.deltaTime;
                            }
                        }
                        else
                        {
                            attack_end = true;
                        }

                    }
                    if (attack_end)
                    {
                        ani.SetBool("isAttack", false);
                        count = 0;
                        cooldown = 0;
                        attack_end = false;
                    }
                    break;

                case State.Groggy:
                    ani.applyRootMotion = false;
                    ani.SetBool("isGroggy" , true);
                    yield return new WaitForSeconds(5);
                    state = State.Action;
                    ani.applyRootMotion = true;
                    ani.SetBool("isGroggy" , false);
                    break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void switchState(State state)
    {
        this.state = state;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Arrow"))
        {
            damageable.Damaged(1 , 0, collision.transform);
        }
        else
        {
            state = State.Groggy;
        }
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player") )
        {
            find_target = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if( other.CompareTag("Player") )
        {
            find_target = false;
        }
    }
    bool dead;
    bool first;
    public void Update()
    {
        if( damageable.Hp <= 0 )
        {
            ani.applyRootMotion = false;
            ani.SetBool("isGroggy" , false);
            ani.SetBool("isDead" , true);
            state = State.Dead;

            if (!dead)
            {
                GameObject eff = Instantiate(DeadEffect, transform.position, Quaternion.identity, null);
                //eff.transform.localScale *= 30;
                Destroy(eff, 10);
                dead = true;
            }
            Destroy(this.gameObject , 3);
        }


        if(detecter.is_find)
        {
            if(!first)
            {
                RenderSettings.fogEndDistance = 70;
                first = true;
                state = state == State.Sleep ? State.Action : State.Idle;
                find_target = true;
            }
        }
    }

    private void OnDestroy()
    {
        FindObjectOfType<MidHp>().start = false;
    }

}
