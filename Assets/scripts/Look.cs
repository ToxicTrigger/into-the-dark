using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Look : MonoBehaviour {
    public Transform Target;
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
                Destroy(gameObject);
                if(!isOnDeadEff)
                {
                    GameObject dead = GameObject.Instantiate(Dead, transform.position, Quaternion.identity, null);
                    Destroy(dead, 2.0f);
                    isOnDeadEff = true;
                }
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

    float updateTick = 2;
    void Update () {
        if(Hp <= 0)
        {
            mind = State.Dead;
        }
        if(det.is_fined)
        {
            if(na.enabled)
            {
                this.transform.LookAt(Target);
                na.SetDestination(Target.position);

                if (updateTick >= 2.0f)
                {
                    StartCoroutine(setRushPoint());
                    rush_point = (transform.forward * 2 + transform.position);
                    tmp.position = rush_point;
                    updateTick = 0;
                }
                else
                {
                    updateTick += Time.deltaTime;
                }
                
            }
        }
        else
        {
            if(na.enabled) na.SetDestination(start);
        }
	}
    public Transform tmp;
    IEnumerator setRushPoint()
    {
        while (Vector3.Distance(transform.position, rush_point) >= 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, rush_point, Time.deltaTime);
            yield return new WaitForSeconds(0.02f);
        }
    }


}
