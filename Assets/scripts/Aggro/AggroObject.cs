using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AggroObject : Observer {
    public float aggro_point = 1;
    public SphereCollider sc;

    public override void notify(Observable obj)
    {
        AggroDetector ai = obj.GetComponent<AggroDetector>();
        if (!ai.ao.observers.Contains(this))
        {
            if (ai.flag)
            {
                ai.ao.add_observer(this);
            }
            else
            {
                Debug.Log("re");
                ai.ao.remove_observer(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AggroDetector>() != null)
        {
            other.GetComponent<AggroDetector>().ao.add_observer(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AggroDetector>() != null)
        {
            
            other.GetComponent<AggroDetector>().ao.remove_observer(this);
        }
    }

    public void remove(GameObject obj)
    {
        AggroDetector ai = obj.GetComponent<AggroDetector>();
        ai.ao.remove_observer(this);
    }

    void Start () {
        sc = GetComponent<SphereCollider>();
        sc.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
        sc.radius = aggro_point;
	}
}
