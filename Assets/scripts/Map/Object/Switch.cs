using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable
{
    public bool OnOff;
    public bool handlr;
    public Particle_Handler[] ph;
    public AudioSource sound;

    private new void Awake()
    {
        base.Awake();

        ph = transform.GetComponents<Particle_Handler>();

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
                        foreach(var a in ph)
                        {
                            a.OnOff = false;
                        }
                        
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
                        foreach (var a in ph)
                        {
                            a.OnOff = true;
                        }
                    }
                    OnOff = true;
                }
            }
        }
    }
}
