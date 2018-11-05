
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
    public List<Rigidbody> particles;
    public Switch target;
    public Material mat;
    bool event_on;

    void Start()
    {
        particles = new List<Rigidbody>();
        for(int i = 0 ; i < transform.childCount ; ++i )
        {
            particles.Add(transform.GetChild(i).GetComponent<Rigidbody>());
        }
    }

    public void Update()
    {
        if(target.OnOff)
        {
            float level = mat.GetFloat("_Level");
            if(level < 1)
            {
                level += Time.deltaTime * 0.25f;
                mat.SetFloat("_Level" , level);
            }
            else
            {
                level = 0;
                mat.SetFloat("_Level" , level);
                Destroy(gameObject);
            }
        }
        else
        {
            float level = mat.GetFloat("_Level");
            if( level > 0 )
            {
                level -= Time.deltaTime * 0.25f;
                mat.SetFloat("_Level" , level);
            }
            else
            {
                level = 0;
                mat.SetFloat("_Level" , level);
            }
        }
    }
}
