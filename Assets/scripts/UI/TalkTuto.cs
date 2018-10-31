using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTuto : Switch {
    public TotuUI ui;
    public bool has_on;
    bool Use;
	
	// Update is called once per frame
	new void Update () {
        if(!Use)
        {
            base.Update();
            if (this.get_capture_area)
            {
                if (ui.state == 6)
                {
                    if (!has_on)
                    {
                        ui.StartCoroutine(ui.StartTimer(3));
                        has_on = true;
                        Use = true;
                    }
                }
            }
            else
            {
                e.Down();
            }
        }

    }
}
