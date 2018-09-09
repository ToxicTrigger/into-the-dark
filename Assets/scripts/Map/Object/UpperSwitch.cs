using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperSwitch : Platform {
    public Transform Door;
    public Vector3 Up_Pos, Def_pos, cur_pos;
    public float speed;
    bool inTrigger;
    public Switch _switch;
    public AudioSource sound, sound2;

    public void Start()
    {
        Def_pos = Door.position;
        Up_Pos.x = Def_pos.x;
        Up_Pos.y += Def_pos.y;
        Up_Pos.z = Def_pos.z;
    }

    private void OnTriggerStay(Collider other)
    {
        if(_switch ==null)
        {
            if (other.CompareTag("Player"))
            {
                inTrigger = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_switch == null)
        {
            if (other.CompareTag("Player"))
            {
                inTrigger = false;
            }
        }
    }

    void Update () {
        if(_switch == null)
        {
            if (inTrigger)
            {
                if (Door.position.y <= Up_Pos.y)
                {
                    Door.Translate((Door.up * -1) * speed * 1.3f);
                }
                else if(Door.position.y >= Up_Pos.y)
                {

                }
            }
            else
            {
                if (Door.position.y >= Def_pos.y)
                {
                    Door.Translate(Door.up * speed * 0.5f);
                }
                else
                {
                }
            }
        }
        else
        {
            if(!_switch.OnOff)
            {
                if (Door.position.y >= Up_Pos.y)
                {
                    Door.Translate((Door.position - Up_Pos).normalized * -1 * speed );
                }
            }
            else
            {
                if (Door.position.y <= Def_pos.y)
                {
                    Door.Translate((Def_pos - Door.position).normalized * speed);
                }
            }
        }
	}
}
