using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitTuTo : ArrowSwitch {
    public TotuUI ui;
    public bool has_on;

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if( this.OnOff )
        {
            if( ui.state == 9 )
            {
                if( !has_on )
                {
                    ui.StartCoroutine(ui.StartTimer(1));
                    has_on = true;
                }

            }
        }
    }
}
