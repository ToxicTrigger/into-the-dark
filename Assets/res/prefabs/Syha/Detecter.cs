using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detecter : MonoBehaviour {
    public bool is_fined;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Heejin"))
        {
            is_fined = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name.Equals("Heejin"))
        {
            is_fined = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Heejin"))
        {
            is_fined = false;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
