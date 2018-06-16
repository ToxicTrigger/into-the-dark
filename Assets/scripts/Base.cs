using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour {
    public enum State
    {
        Idle,
        Ready,
        On
    }
    public List<ParticleSystem> fogs;
    public List<Base> our_base;
    public State state;
    public bool TotemIn;
    public bool has_clear;

	// Use this for initialization
	void Start () {
        state = State.Idle;
	}

    void UpdateParticles()
    {
        switch (state)
        {
            //===================================
            case State.Idle:
                foreach (var tmp in fogs)
                {
                    tmp.Play(true);
                }
                break;
            //===================================
            case State.Ready:
                foreach (var tmp in fogs)
                {
                    tmp.Play(true);
                }
                break;
            //===================================
            case State.On:
                foreach (var tmp in fogs)
                {
                    tmp.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
                break;
            //===================================
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            Arrow tmp = other.gameObject.GetComponent<Arrow>();
            if (tmp.has_targeting_totem & TotemIn)
            {
                bool on = true;

                foreach (var item in our_base)
                {
                    if (item.state != State.Ready)
                    {
                        on = false;
                    }
                }
                if (on)
                {
                    foreach (var item in our_base)
                    {
                        item.state = State.On;
                    }
                }
            }
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("TotemAggro"))
        {
            TotemIn = true;
            state = State.Ready;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("In" + other.name);
        if (other.gameObject.CompareTag("Arrow"))
        {
            Arrow tmp = other.gameObject.GetComponent<Arrow>();
            if (tmp.has_targeting_totem & TotemIn)
            {
                bool on = true;

                foreach (var item in our_base)
                {
                    if (item.state != State.Ready)
                    {
                        on = false;
                    }   
                }
                if( on )
                {
                    foreach (var item in our_base)
                    {
                        item.state = State.On;
                    }
                }
            }
        }
        if (other.CompareTag("TotemAggro"))
        {
            TotemIn = true;
            state = State.Ready;
        }
    }

    private void OnTriggerExit(Collider other)
    {
         Debug.Log("Out" + other.name);
        if (other.CompareTag("TotemAggro"))
        {
           
            TotemIn = false;
            if (!has_clear) state = State.Idle;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateParticles();
    }
}
