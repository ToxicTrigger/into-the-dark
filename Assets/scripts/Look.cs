using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Look : MonoBehaviour {
    public enum State
    {
        Dead,
        Idle,
        Hit,
        Stun,
        HitBack,
        Hovering,
        Attack,
        Rush
    };

    public Detecter det;
    public State mind;
    public NavMeshAgent na;
    public Vector3 start;
    public float Hp = 2;

    public Vector3 rush_point;
    public bool is_end_attack;

    public GameObject Hit, Dead;
    public bool isOnDeadEff;

    void Start () {
        start = transform.position;
        na = GetComponent<NavMeshAgent>();
	}
	
    void UpdateState()
    {
        switch (mind)
        {
            case State.Dead:
                na.enabled = false;
                if(!isOnDeadEff)
                {
                    GameObject dead = GameObject.Instantiate(Dead, transform.position, Quaternion.identity, null);
                    Destroy(dead, 2.0f);
                    isOnDeadEff = true;
                }
                Destroy(det.gameObject);
                Destroy(gameObject);
                break;
            case State.Hit:
                //gameObject.GetComponent<Rigidbody>().AddForce((transform.forward * 10) * -1, ForceMode.Impulse);
                break;
        }
    }

    private void LateUpdate()
    {
        UpdateState();
    }

    //공격자가 호출합니다.
    public void onAttack(float damage)
    {
        GameObject eff = GameObject.Instantiate(Hit, transform.position, Quaternion.identity, null);
        Destroy(eff, 1.0f);
        if (Hp > 0)
        {
            Hp -= damage;
        }
        else
        {
            mind = State.Dead;
        }
    }

    void Update () {
        if(Hp <= 0)
        {
            mind = State.Dead;
        }
        if(det.is_find)
        {
            if(na.enabled)
            {
                transform.LookAt(det.target.transform);
                if(is_end_attack)
                {
                    na.SetDestination(det.target.transform.position);
                }
               
                if (updateRushTick >= 2.0f)
                {
                    rush_point = (transform.forward * 2 + transform.position);
                    StartCoroutine(setRushPoint());
                    updateRushTick = 0;
                }
                else
                {
                    updateRushTick += Time.deltaTime;
                    is_end_attack = true;
                }
                
            }
        }
        else
        {
            if(na.enabled) na.SetDestination(start);
        }
	}


    float updateRushTick = 2;
    IEnumerator setRushPoint()
    {
        is_end_attack = false;
        while (Vector3.Distance(transform.position, rush_point) >= 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, rush_point, Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        }

    }


}
