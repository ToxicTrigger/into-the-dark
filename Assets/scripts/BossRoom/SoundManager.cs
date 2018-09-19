﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    private static SoundManager instance = null;

    public static SoundManager get_instance()
    {
        if (instance == null)
        {
            //FindObjectOfType은 유니티에서 비용이 큰 함수지만 처음 한번만 호출되므로 괜찮음~
            instance = GameObject.FindObjectOfType(typeof(SoundManager)) as SoundManager;
            if (instance == null)
                Debug.LogError("Singleton Error");
        }

        return instance;
    }

    /// ////////////////////////////////////////////////////////////////////////////////////

    public enum SoundList
    {
        bossroom_idle,
        heartbeat_1,
        heartbeat_2,
        step_stone,
        step_wood,
        time_ticktock,
        rumble,
        boss_ready_real,
        boss_attack_up,
        boss_attack_down,
        hit_boss_one,
        hit_boss_two
    };
    public SoundList sound_number;

    public AudioSource[] sound_list;


    private void Start()
    {
        //sound_list[(int)SoundList.bossroom_idle].Play();
        sound_list[(int)SoundList.heartbeat_1].Play();
        sound_list[(int)SoundList.heartbeat_2].Play();
    }

    public void play_sound(SoundList _sound_number)
    {
        sound_list[(int)_sound_number].Play();
    }

    public void stop_sound(SoundList _sound_number, bool is_immediately)
    {

        if(!is_immediately)
            StartCoroutine(volume_ctrl(_sound_number));
        else
        {
            sound_list[(int)_sound_number].Stop();
        }
    }

    public void mute_sound(SoundList _sound_number, bool _is_mute)
    {
        sound_list[(int)_sound_number].mute = _is_mute;
    }

    IEnumerator volume_ctrl(SoundList _sound_number)
    {
        while (true)
        {
            sound_list[(int)_sound_number].volume -= 0.05f;

            yield return new WaitForSeconds(0.01f);

            if (sound_list[(int)_sound_number].volume <= 0)
            {
                sound_list[(int)_sound_number].Stop();
                sound_list[(int)_sound_number].volume = 1;
                break;
            }
        }

    }
}