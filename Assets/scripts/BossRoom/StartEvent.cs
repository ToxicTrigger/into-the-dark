using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEvent : MonoBehaviour {
    public GameObject _event;
    bool play = false;
    
	void Update () {
		if(Input.GetKeyDown(KeyCode.O) && !play)
        {
            play = true;
            _event.SetActive(true);
        }
	}
}
