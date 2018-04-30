using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Observer {
    public int on_switch_number;
    public bool open = false;
    public Vector3 origin, down;

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
            if(on_switch_number != 0)
            on_switch_number-=1;
        }
    }
	
	void Update () {
        if(open)
        {
            transform.position = down;
        }
        else
        {
            transform.position = origin;
        }

		if(on_switch_number == 2)
        {
            open = true;
        }
        else
        {
            open = false;
        }
	}
}
