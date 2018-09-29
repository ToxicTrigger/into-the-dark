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
    private Transform des;

    [SerializeField]
    private float Speed;

    public bool find_target;
    public bool end_cut_scene;

    public Damageable damageable;
    public GameObject Poison;
    public Detecter detecter;
    public Animator ani;

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

    IEnumerator FSM()
    {
        float attack_tick = 0;
        while( this.gameObject != null )
        {
            switch( state )
            {
                case State.Action:
                    ani.applyRootMotion = false;
                    ani.SetBool("isAction" , true);
                    yield return new WaitForSeconds(6);
                    ani.SetBool("isAction" , false);
                    state = State.Attack;
                    ani.applyRootMotion = true;
                    break;

                case State.Attack:
                    transform.LookAt(Player.transform);
                    if( attack_tick <= 1.4f )
                    {
                        attack_tick += Time.deltaTime;
                        ani.SetBool("isAttack" , false);
                    }
                    else
                    {
                        ani.SetBool("isAttack" , true);
                        //yield return new WaitForSeconds(0.3f);
                        Vector3 pos = transform.position;
                        GameObject poison = Instantiate(Poison , pos , Quaternion.identity , null);
                        poison.transform.position = transform.position;
                        poison.GetComponent<Rigidbody>().useGravity = false;
                        poison.GetComponent<Rigidbody>().velocity = ( Player.transform.position - pos ).normalized * 40;
                        attack_tick = 0;
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
            damageable.Damaged(1 , 0);
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

    bool first;
    public void Update()
    {
        if( damageable.Hp <= 0 )
        {
            ani.applyRootMotion = false;
            ani.SetBool("isGroggy" , false);
            ani.SetBool("isDead" , true);
            state = State.Dead;
            Destroy(this.gameObject , 5);
        }

        if(detecter.is_find)
        {
            if(!first)
            {
                first = true;
                state = state == State.Sleep ? State.Action : State.Idle;
                find_target = true;
            }

        }
    }


}
