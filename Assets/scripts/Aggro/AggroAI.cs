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
    public bool has_FoundTarget, has_RangedTarget, has_Dead, has_Hit, has_patrol;
    public float attack_range = 1.0f;
    public Attackable attack;
    public Damageable damage;
    public Player player;
    public Rigidbody rig;
    public GameObject DeadParticle;
    public AudioSource HitSound;

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
                //na.isStopped = true;
                na.enabled = false;
            }
            else
            {
                has_RangedTarget = false;
                // na.isStopped = false;
                na.enabled = true;
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
    bool has_registered;
    void update_move()
    {
        if(target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) >= na.radius & !has_carry_on)
            {
                if(na.enabled)
                na.SetDestination(target.transform.position);
                has_carry_on = true;
                has_FoundTarget = true;
                if(!has_registered)
                {
                    if(player != null) player.is_target_something += 1;
                    has_registered = true;
                }
            }
            else
            {
                has_FoundTarget = false;
                target = null;
                has_carry_on = false;
                high_aggro = 0;
                if(has_registered)
                {
                    if(player != null) player.is_target_something -= 1;
                    has_registered = false;
                }
            }
        }
        else
        {
            if(na.enabled & !has_patrol)
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
            //Debug.DrawLine(transform.position, obj.transform.position, Color.blue);
        }
    }

    void update_state()
    {
        //아래 4가지 변수들은 기본적으로 FSM 애니메이션에 포함되어야 합니다.
        ani.SetBool("hasFoundPlayer", has_FoundTarget);
        ani.SetBool("hasRangedPlayer", has_RangedTarget);
        ani.SetBool("hasDead", has_Dead);
        ani.SetBool("hasHit", has_Hit);
        ani.SetBool("hasPatrol", has_patrol);
    }

    private void Awake()
    {
        na = GetComponent<NavMeshAgent>();
        origin_pos = transform.position;
        ani = GetComponent<Animator>();
        attack = GetComponent<Attackable>();
        damage = GetComponent<Damageable>();
        player = FindObjectOfType<Player>();
    }
    float rally_tick, patrol_done;
    void Update_rally_point()
    {   
        if(has_patrol)
        {
            if(patrol_done >= 2.0f)
            {
                patrol_done = 0;
                has_patrol =false;
            }else{
                patrol_done += Time.deltaTime;
            }
        }

        if(!has_FoundTarget & !has_RangedTarget)
        {   
            if(rally_tick >= 5.0f){
                has_patrol = true;
                float x = Random.Range(-4, 4);
                float z = Random.Range(-4, 4);
                Vector3 pos = transform.position;
                pos.x += x;
                pos.z += z;
                na.SetDestination(pos);
                //Debug.Log("work" + na.pathPending);
                rally_tick = 0;
            }else{
                //has_patrol = false;
                rally_tick += Time.deltaTime;
            }
        }else{
            has_patrol = false;
        }
    }

    void Make_DeadEffect()
    {
        //yield return new WaitForSeconds(0.5f);
                if(DeadParticle != null)
            {
                GameObject tmp = Instantiate(DeadParticle, transform.position, Quaternion.identity, null);
                Destroy(tmp, 4.0f);
            }
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
            if(rig != null)
            {
                rig.isKinematic = false;
                rig.transform.parent = null;
                rig.AddForce((transform.position - player.transform.position).normalized * 12, ForceMode.Impulse);
                rig.AddTorque((transform.position - player.transform.position).normalized * 200, ForceMode.Impulse);
                Destroy(rig.gameObject, 2.0f);
            }


            na.enabled = false;
            //StartCoroutine(Make_DeadEffect());
            Make_DeadEffect();
            Destroy(gameObject);
        }

        update_attackable_range();
        update_state();
        Update_rally_point();

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

    private void OnTriggerEnter(Collider other)
    {   
        if(other.CompareTag("Sword") || other.CompareTag("Arrow"))
        {
            GameObject sound = Instantiate(HitSound.gameObject, transform.position, Quaternion.identity, null);
            sound.GetComponent<AudioSource>().PlayOneShot(sound.GetComponent<AudioSource>().clip);
            //HitSound.PlayOneShot(HitSound.clip);
            Destroy(sound, 1.0f);

            Debug.Log( other.name + " " + other.tag);
            damage.Damaged(1, 0.2f);
            //has_Hit = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sword") || other.CompareTag("Arrow"))
        {
            has_Hit = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {   
        if(collision.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("Hit by Arrow");
            damage.Damaged(collision.gameObject.GetComponent<Arrow>().power, 0.1f);
            Destroy(collision.gameObject);
        }else if(collision.gameObject.CompareTag("Sword"))
        {
             has_Hit = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.CompareTag("Sword"))
        {
             has_Hit = false;
        }
    }

    private void OnDestroy() {
        if(has_registered) 
        if(player != null) player.is_target_something -= 1;
    }
}
