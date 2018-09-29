using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckTarget : Switch
{
    public string Tag;
    public bool In;

    public void OnTriggerEnter(Collider collision)
    {
        if( collision.gameObject.CompareTag(Tag) )
        {
           In = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if( other.CompareTag(Tag))
        {
           In = false;
        }
    }

    public new void Update()
    {
        this.OnOff = In;
    }
}
