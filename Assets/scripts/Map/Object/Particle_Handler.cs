using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Handler : MonoBehaviour
{
    public bool OnOff;
    public ParticleSystem particle;
    private void Update()
    {
        if(particle != null)
        {
            if( OnOff )
            {
                particle.Play();
            }
            else
            {
                particle.Stop();
            }
        }
    }
}
