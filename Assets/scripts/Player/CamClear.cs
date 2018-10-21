using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamClear : MonoBehaviour
{
    public Transform Target;
    public Collider cur, now;
    public bool same;

    void Update()
    {
        if( Target != null )
        {
            RaycastHit ray;
            Ray r = new Ray(transform.position , ( Target.position - transform.position ).normalized);
            Debug.DrawRay(r.origin , r.direction * 100 , Color.red);
            Physics.Raycast(r.origin , r.direction , out ray , Vector3.Distance(transform.position , Target.position), LayerMask.NameToLayer("Player"));
            

            if(ray.collider != null)
            {
                Debug.Log(ray.collider.name);
                if(!ray.collider.CompareTag("Player") && !ray.collider.CompareTag("TotemAggro"))
                {
                    cur = now;
                    now = ray.collider;
                    if(now.GetComponent<Renderer>() != null)
                    now.GetComponent<Renderer>().enabled = false;
                }
            }
            else
            {
                if(cur != null)
                {
                    cur.GetComponent<Renderer>().enabled = true;
                    cur = null;
                }
            }
        }
    }
}
