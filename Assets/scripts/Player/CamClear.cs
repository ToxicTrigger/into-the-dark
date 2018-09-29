using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamClear : MonoBehaviour
{
    public Transform Target;
    public Collider cur;
    public bool same;

    void Update()
    {
        if( Target != null )
        {
            RaycastHit ray;
            Ray r = new Ray(transform.position , ( Target.position - transform.position ).normalized);
            Debug.DrawRay(r.origin , r.direction * 100 , Color.red);
            Physics.Raycast(r.origin , r.direction , out ray , Vector3.Distance(transform.position , Target.position));
            if( ray.collider != null )
            {
                cur = ray.collider;
                if( !same )
                {
                    if( cur.gameObject.layer != LayerMask.NameToLayer("Ground") & cur.Equals(ray.collider) )
                    {
                        same = true;
                        Debug.Log("가리는 놈 : " + ray.collider.name);
                        cur.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                else if( !cur.Equals(ray.collider) )
                {
                    same = false;
                    cur.GetComponent<MeshRenderer>().enabled = true;
                }
            }
            else
            {
                Debug.Log("벗어남");
                if( cur != null )
                {
                    if( cur.GetComponent<MeshRenderer>() != null )
                    {
                        cur.GetComponent<MeshRenderer>().enabled = true;
                    }
                }

                same = false;
                cur = null;
            }
        }
    }
}
