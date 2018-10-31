using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTuto : Switch
{
    public TotuUI ui;
    public bool Use;
    public bool popUpEnd;

    new void Update()
    {
        if( !Use && ui.state == 7 )
        {
            base.Update();
            if( this.get_capture_area )
            {

                if( !hasTalking )
                {
                    if(!popUpEnd)
                    {
                        ui.StartCoroutine(ui.StartTimer(3));
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
