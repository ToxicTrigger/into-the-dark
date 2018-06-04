using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(Damageable))]

public abstract class AggroAI : Observable {
    public AggroDetector aggro_detector;
    public float aggro_rad = 5.0f;
    public NavMeshAgent na;
    public AggroObject target;
    public float high_aggro;
    public bool has_carry_on;
    public Vector3 origin_pos;
    public Animator ani;
    public string cur_ani;
    public bool has_FoundTarget, has_RangedTarget, has_Dead, has_Hit;
    public float attack_range = 1.0f;
    public float Attack_Power = 1.0f;
    public Attackable attack;
    public Damageable damage;

    //구현은 HunterAI 참조
    public abstract void FSM(AggroAI ai);

    void Update_Target()
    {
        if (observers.Count == 0) high_aggro = 0;
        IEnumerator iter = observers.GetEnumerator();
        while (iter.MoveNext())
        {
            Observer obj = iter.Current as Observer;
            if(obj.GetComponent<AggroObject>().aggro_point >= high_aggro)
            {
                high_aggro = obj.GetComponent<AggroObject>().aggro_point;
                target = obj.GetComponent<AggroObject>();
                Debug.DrawLine(transform.position, obj.transform.position, Color.red);
            }
        }
    }

    void update_attackable_range()
    {
        if(target != null)
        {
            if(Vector3.Distance(target.transform.position, transform.position) <= attack_range)
            {
                has_RangedTarget = true;
                na.isStopped = true;
            }
            else
            {
                has_RangedTarget = false;
                na.isStopped = false;
            }
        }
    }
    //Todo
    void update_target_is_Damageable()
    {
        if(target != null)
        {

        }
    }

    void update_move()
    {
        if(target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) >= na.radius & !has_carry_on)
            {
                na.SetDestination(target.transform.position);
                has_carry_on = true;
                has_FoundTarget = true;
            }
            else
            {
                has_FoundTarget = false;
                target = null;
                has_carry_on = false;
                high_aggro = 0;
            }
        }
        else
        {
            na.SetDestination(origin_pos);
            has_FoundTarget = false;
        }
    }

    private void OnDrawGizmos()
    {
        IEnumerator iter = observers.GetEnumerator();
        while(iter.MoveNext())
        {
            Observer obj = iter.Current as Observer;
            Debug.DrawLine(transform.position, obj.transform.position, Color.blue);
        }
    }

    void update_state()
    {
        //아래 4가지 변수들은 기본적으로 FSM 애니메이션에 포함되어야 합니다.
        ani.SetBool("hasFoundPlayer", has_FoundTarget);
        ani.SetBool("hasRangedPlayer", has_RangedTarget);
        ani.SetBool("hasDead", has_Dead);
        ani.SetBool("hasHit", has_Hit);
    }

    private void Awake()
    {
        na = GetComponent<NavMeshAgent>();
        origin_pos = transform.position;
        ani = GetComponent<Animator>();
        attack = GetComponent<Attackable>();
        damage = GetComponent<Damageable>();
    }
    
    void Update () {
        //현재 FSM 이 가리키는 노드 이름
        cur_ani = ani.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        aggro_detector.sc.radius = aggro_rad;
        Update_Target();
        update_move();

        if(damage.Hp <= 0)
        {
            has_Dead = true;
            //ani.applyRootMotion = false;
            enabled = false;
            na.enabled = false;
            Destroy(gameObject, 1.0f);
        }

        update_attackable_range();
        update_state();

        FSM(this);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            Observer o = observers[i];
            if (o == null) observers.RemoveAt(i);
        }
    }

    IEnumerator update_hit(float damage)
    {
        has_Hit = true;

        this.damage.Hp -= damage;

        yield return new WaitForSeconds(0.5f);
        has_Hit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sword") | other.CompareTag("Arrow"))
        {
            has_Hit = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sword") | other.CompareTag("Arrow"))
        {
            has_Hit = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Arrow"))
        {
            StartCoroutine(update_hit(collision.gameObject.GetComponent<Arrow>().power));
            Destroy(collision.gameObject);
        }
    }
}
