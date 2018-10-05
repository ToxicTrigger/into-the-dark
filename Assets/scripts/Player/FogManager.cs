using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    public List<ParticleSystem> fogs;
    public float View_range;

	void Start () {
        fogs = new List<ParticleSystem>();

        var tmp = GameObject.FindGameObjectsWithTag("Fog");
        foreach(var t in tmp)
        {
            fogs.Add(t.GetComponent<ParticleSystem>());
        }
	}
	
	void Update ()
    {
		foreach(var t in fogs)
        {
            float dis = Vector3.Distance(transform.position , t.transform.position);
            if(dis >= View_range)
            {
                if(!t.isStopped)
                {
                    t.Stop();
                }
                else
                {
                    Debug.Log("Stop");
                }
                
            }
            else
            {
                if(!t.isPlaying)
                {
                    t.Play();
                }
                
            }
        }
	}
}
