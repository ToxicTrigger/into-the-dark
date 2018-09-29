using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reversor : MonoBehaviour
{
    public Switch target;
    public bool state;

	void Update () {
		if(target.OnOff)
        {
            state = false;
        }
        else
        {
            state = true;
        }
	}
}
