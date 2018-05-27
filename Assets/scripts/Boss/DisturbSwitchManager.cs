using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisturbSwitchManager : BasicSwitch {

    //스위치를 받아옴
    public DisturbSwitch[] switch_list;

    void Start () {
		
	}
	
	void Update () {
		
	}

    void check_destroy_switch()
    {
        for(int i =0; i<switch_list.Length; i++)
        {
            if (switch_list[i].destroy_switch == false) break;

            //if(i == switch_list.Length-1)

        }
    }

}
