using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Observer {
    private int on_switch_number;
    public bool open = false;
    private Vector3 origin, down;
    public int switch_number;

    private void Start()
    {
        origin = transform.position;
        down = origin + (Vector3.down * 10);
    }

    public override void notify(Observable observable)
    {
        Switch tmp = observable as Switch;
        if(tmp.switch_on)
        {
            on_switch_number+=1;
        }
        else
        {
            if(on_switch_number > 0)
            on_switch_number-=1;
        }
    }

    private void FixedUpdate()
    {
        if (on_switch_number == switch_number)
        {
            open = true;
        }
        else
        {
            open = false;
        }
    }

    void LateUpdate () {
        //TODO :: using Coroutine
        if(open)
        {
            transform.position = down;
        }
        else
        {
            transform.position = origin;
        }
	}
}
