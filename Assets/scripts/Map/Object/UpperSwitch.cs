using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperSwitch : Platform
{
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
        if (_switch == null)
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

    bool has_played;

    void Update()
    {
        if (_switch == null)
        {
            if (inTrigger)
            {
                if (Door.position.y < Up_Pos.y)
                {
                    Vector3 tmp = Vector3.up * -1 * speed * 2.5f;
                    Door.position = tmp + Door.position;
                    if (!sound.isPlaying)
                    {
                        sound.Play();
                    }
                }
                else if (Door.position.y > Up_Pos.y)
                {
                    Door.position = Up_Pos;
                    sound2.Play();
                    if (sound.isPlaying)
                        sound.Stop();
                }
            }
            else
            {
                if (Door.position.y > Def_pos.y)
                {
                    Vector3 tmp = Vector3.up * speed * 0.5f;
                    Door.position = tmp + Door.position;

                    if (!sound.isPlaying)
                    {
                        sound.Play();
                    }
                }
                else if (Door.position.y < Def_pos.y)
                {
                    Door.position = Def_pos;
                    if (sound.isPlaying)
                        sound.Stop();
                }
            }
        }
        else
        {
            if (!_switch.OnOff)
            {
                if (Door.position.y >= Up_Pos.y)
                {
                    Vector3 tmp = Vector3.up * -1 * speed * 0.5f;
                    Door.position = tmp + Door.position;
                }
            }
            else
            {
                if (Door.position.y <= Def_pos.y)
                {
                    Vector3 tmp = Vector3.up * speed * 0.5f;
                    Door.position = tmp + Door.position;
                }
            }
        }
    }
}
