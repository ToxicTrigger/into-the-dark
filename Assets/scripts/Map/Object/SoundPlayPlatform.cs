using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayPlatform : MonoBehaviour
{
    public AudioSource sound;
    // Use this for initialization
    void Start()
    {
        sound = this.GetComponent<AudioSource>();
    }
    bool has_played;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!has_played)
            {
                Debug.Log("played");
                sound.Play();
                has_played = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (has_played)
            {
                Debug.Log("stoped");
                sound.Stop();
                has_played = false;
            }

        }
    }
}
