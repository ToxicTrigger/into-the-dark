using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable
{
    public bool OnOff;

    public new void Update()
    {
        base.Update();
        if (this.get_capture_area)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (OnOff)
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
