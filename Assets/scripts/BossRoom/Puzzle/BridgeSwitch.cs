using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSwitch : MonoBehaviour {

    public BridgePuzzle.Direction dir;

    public BridgePuzzle bridge;

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            Destroy(other.gameObject);
            bridge.hit_switch(this.dir);
        }
        if (other.CompareTag("Arrow"))
        {
            Destroy(other.gameObject);
            bridge.hit_switch(this.dir);
        }

    }
}
