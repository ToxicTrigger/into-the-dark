using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XORPlatform : Platform
{
    public Particle_Handler[] triggers;

    private void Update()
    {
        if (this.state == EventState.On)
        {
            triggers[0].OnOff = false;
            triggers[1].OnOff = true;
        }
        else if (this.state == EventState.Off)
        {
            triggers[0].OnOff = true;
            triggers[1].OnOff = false;
        }
    }
}
