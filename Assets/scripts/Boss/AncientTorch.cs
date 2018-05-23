using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientTorch : Observable {
    //고대병기와 상호작용하는 횃불

    enum State
    {
        On =0,  //켜짐
        Off       //꺼짐
    }

    State torch_state;

	void Start () {
        torch_state = State.Off;
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //E키로 상호작용
        if(Input.GetKey(KeyCode.E))
        {
            torch_state = State.On;
           
        }
    }


}
