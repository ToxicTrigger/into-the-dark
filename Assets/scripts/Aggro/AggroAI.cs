using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AggroAI : Observable {
    public AggroDetector aggro_detector;
    public float aggro_rad = 5.0f;
    public NavMeshAgent na;
    public AggroObject target;
    public float high_aggro;
    public bool has_carry_on;
    public Vector3 origin_pos;

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

    void update_move()
    {
        if(target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) >= na.radius & !has_carry_on)
            {
                na.SetDestination(target.transform.position);
                has_carry_on = true;
            }
            else
            {
                target = null;
                has_carry_on = false;
                high_aggro = 0;
            }
        }
        else
        {
            na.SetDestination(origin_pos);
        }
    }

    private void OnDrawGizmos()
    {
        IEnumerator iter = observers.GetEnumerator();
        while(iter.MoveNext())
        {
            Observer obj = iter.Current as Observer;
            Debug.DrawLine(transform.position, obj.transform.position, Color.red);
        }
    }

    private void Awake()
    {
        na = GetComponent<NavMeshAgent>();
        origin_pos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        aggro_detector.sc.radius = aggro_rad;
        Update_Target();
        update_move();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < observers.Count; i++)
        {
            Observer o = observers[i];
            if (o == null) observers.RemoveAt(i);
        }
    }
}
