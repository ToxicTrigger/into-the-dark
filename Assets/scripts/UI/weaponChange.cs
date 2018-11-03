using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponChange : CheckAI
{
    public TotuUI ui;
    
    new void Update ()
    {
        base.Update();
		if(this.OnOff)
        {
            if(ui.state == 4 || ui.state == 7)
            {
                ui.StartCoroutine(ui.StartTimer(0.3f));
                Destroy(gameObject);
            }
        }
	}
}
