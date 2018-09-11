using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOff : MonoBehaviour {
    public new Light light;
    public float power;
    public void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update () {
		if(light.intensity > 0)
        {
            light.intensity -= power;
        }
        else
        {
            light.enabled = false;
        }
	}
}
