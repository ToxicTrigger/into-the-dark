using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSwitch : Switch
{
    [SerializeField]
    private float timer = 5;
    private float now;

    public new void Update()
    {
        if( this.OnOff )
        {
            if( now <= timer )
            {
                now += Time.deltaTime;
            }
            else
            {
                now = 0;
                this.OnOff = false;
            }
        }
        else
        {
            now = 0;
        }
    }
}
