using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable {
    public bool OnOff;
	
	void Update () {
        base.Update();
		if(this.get_capture_area)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(OnOff)
                {
                    OnOff = false;
                }
                else
                {
                    OnOff = true;
                }
            }
        }
	}
}
