using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeIn : MonoBehaviour {
    public ActionCamera cam;

	// Use this for initialization
	void Start () {
        cam = FindObjectOfType<ActionCamera>();
	}

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            cam.Shake(20,1,Time.deltaTime);
        }
    }
}
