using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable
{
    public bool OnOff;
    public bool handlr;
    public Particle_Handler ph;
    public AudioSource sound;

    private new void Awake()
    {
        base.Awake();

        if( ph != null )
        {
            handlr = true;
        }
    }

    public new void Update()
    {
        base.Update();
        if( this.get_capture_area )
        {
            if( Input.GetButtonDown("Submit") )
            {
                if( OnOff )
                {
                    if( sound != null )
                    {
                        sound.PlayOneShot(sound.clip);
                    }
                    if( handlr )
                    {
                        ph.OnOff = false;
                    }
                    OnOff = false;
                }
                else
                {
                    if( sound != null )
                    {
                        sound.PlayOneShot(sound.clip);
                    }
                    if( handlr )
                    {
                        ph.OnOff = true;
                    }
                    OnOff = true;
                }
            }
        }
    }
}
