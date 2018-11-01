using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTuto : Switch
{
    public TotuUI ui;
    public bool Use;
    public bool popUpEnd;
    public bool skip;

    new void Update()
    {
        if (ui.state == 5)
        {
            base.Update();
            if (this.get_capture_area)
            {
                if (!skip)
                {
                    skip = true;
                    ui.StartCoroutine(ui.StartTimer(1));
                }
            }
        }
        if( !Use && (ui.state == 6 || ui.state == 7)  )
        {
            base.Update();
            if( this.get_capture_area )
            {

                if( !hasTalking )
                {
                    if(!popUpEnd)
                    {
                        ui.StartCoroutine(ui.StartTimer(1));
                        popUpEnd = true;
                    }
                }
                else
                {
                    Use = true;
                }

            }
            else
            {
                e.Down();
            }
        }

    }
}
