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
        if( !Use && (ui.state == 9 || ui.state == 8)  )
        {
            base.Update();
            if( this.get_capture_area )
            {

                if( !hasTalking )
                {
                    if(!popUpEnd)
                    {
                        ui.StartCoroutine(ui.StartTimer(0.4f));
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
