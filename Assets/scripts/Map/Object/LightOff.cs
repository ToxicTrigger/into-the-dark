using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOff : MonoBehaviour
{
    public Light this_light;
    public float power;
    public void Start()
    {
        this_light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this_light.intensity > 0)
        {
            this_light.intensity -= power;
        }
        else
        {
            this_light.enabled = false;
        }
    }
}
