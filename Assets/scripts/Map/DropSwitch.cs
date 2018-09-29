using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSwitch : MonoBehaviour
{
    public Vector3 Down, origin;
    public bool onOff;
    float tick;

    public void OnTriggerEnter(Collider other)
    {
        if( other.name.Contains("Poison") )
        {
            onOff = true;
        }
    }

    public void Start()
    {
        origin = transform.position;
        Down = origin;
        Down.y -= 20;
    }

    public void Update()
    {
        if( onOff )
        {
            if( tick <= 5.0f )
            {
                tick += Time.deltaTime;
                transform.position = Down;
            }
            else
            {
                tick = 0;
                onOff = false;
                transform.position = origin;
            }
        }
    }
}
