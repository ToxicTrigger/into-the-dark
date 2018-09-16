using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRelease : Interactable
{

    public void FixedUpdate()
    {
        if(hasTalking)
        {
            this.events.Clear();
        }
    }
}
