﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScenePlay : MonoBehaviour {

    public SoundManager sound_manager;
    public Player player;
    public Damageable p_damageable;

    [Tooltip("알파값 증감 정도")]
    public float add_alpha=0.05f;    
    float plus_alpha, minus_alpha=1;

    public AudioSource cut_scene_sound;

    [System.Serializable]
    public struct CutSceneInfo
    {
        public Image[] screen;
        public Image back_screen;
        public Sprite[] scene_image;
        public float[] keep_time;
    }
    public CutSceneInfo scene_info;
      
    RectTransform[] rt = new RectTransform[2];
    RectTransform back_rt;
    RectTransform this_rt;
    IEnumerator timer;

    Image pre_scene, now_scene;
    public bool test;

    public bool is_clear;
    public Ending ending;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        p_damageable = player.GetComponent<Damageable>();

        for (int i = 0; i < scene_info.screen.Length; i++)
        {
            scene_info.screen[i] = transform.GetChild(i+1).GetComponent<Image>();
            rt[i] = scene_info.screen[i].GetComponent<RectTransform>();
            rt[i].sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
            scene_info.screen[i].color = new Vector4(1, 1, 1, 0);
        }
        scene_info.back_screen = transform.GetChild(0).GetComponent<Image>();
        back_rt = scene_info.back_screen.GetComponent<RectTransform>();
        back_rt.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        scene_info.back_screen.color = new Vector4(0, 0, 0, 0);

        this_rt = GetComponent<RectTransform>();
        this_rt. sizeDelta= new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && test)
        {
            BossRoomManager.get_instance().game_clear();
        }
    }

    public void play_scene()
    {
        if (is_clear)
        {
            p_damageable.Hp = p_damageable.Max_Hp;
            p_damageable.is_invincibility = true;
        }

        var all_sound = FindObjectsOfType<AudioSource>();
        foreach (var s in all_sound)
        {
            if(!s.gameObject.name.Equals("CutScene"))
            {
                s.Stop();
            }
        }
        if (timer == null)
        {
            if (sound_manager != null)
                sound_manager.clear();

            timer = play_timer();
            StartCoroutine(timer);
        }
    }

    IEnumerator play_timer()
    {
        float sound_timer =0;

        //페이드 아웃
        while (true)
        {
            plus_alpha += add_alpha*0.13f;
            scene_info.back_screen.color = new Vector4(0, 0, 0, plus_alpha);
            if (plus_alpha >= 1)
            {
                plus_alpha = 0;
                break;
            }
            yield return new WaitForSeconds(0.01f);

        }

        //사운드 재생
        while(true)
        {
            sound_timer += Time.deltaTime;
            if(sound_timer >= 2 && !cut_scene_sound.isPlaying)
            {
                cut_scene_sound.Play();//컷씬 브금 재생
            }
            else if(sound_timer >= 4)
            {
                sound_timer = 0;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }        

        for (int i =0; i< scene_info.scene_image.Length; i++)
        {
            if (pre_scene == scene_info.screen[0])
            {
                now_scene = scene_info.screen[1];
                scene_info.screen[1].sprite = scene_info.scene_image[i];
            }
            else
            {
                now_scene = scene_info.screen[0];
                scene_info.screen[0].sprite = scene_info.scene_image[i];
            }

            while(true)
            {
                if(pre_scene != null)
                {
                    minus_alpha -= add_alpha;
                    if (minus_alpha <= 0) minus_alpha = 0;
                    pre_scene.color = new Vector4(1, 1, 1, minus_alpha);
                }
                if (now_scene != null)
                {
                    plus_alpha += add_alpha;
                    if (plus_alpha >= 1) plus_alpha = 1;
                    now_scene.color = new Vector4(1, 1, 1, plus_alpha);
                }
                if (now_scene.color.a >= 1 && pre_scene == null || now_scene.color.a >= 1 && pre_scene.color.a <= 0)
                    break;

                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(scene_info.keep_time[i]);
            minus_alpha = 1;
            plus_alpha = 0;
            pre_scene = now_scene;
            now_scene = null;
        }

        while (true)
        {
            if (pre_scene != null)
            {
                minus_alpha -= add_alpha;
                if (minus_alpha <= 0) minus_alpha = 0;
                pre_scene.color = new Vector4(1, 1, 1, minus_alpha);
            }

            if (pre_scene.color.a <= 0)
                break;

            yield return new WaitForSeconds(0.05f);
        }

        while(true)
        {
            if(cut_scene_sound.volume >= 0.001f)
                cut_scene_sound.volume -= 0.001f;
            yield return new WaitForSeconds(0.01f);
            if (cut_scene_sound.volume <= 0.001f)
                break;
        }

        timer = null;
        cut_scene_sound.Stop();
        plus_alpha = 1;

        if (is_clear)
        {
            yield return new WaitForSeconds(3.0f);
            scene_info.back_screen.color = new Vector4(0, 0, 0, 0);
            //ending.gameObject.SetActive(true);
            ending.play_scroll();
        }

        while (!is_clear)
        {
            plus_alpha -= add_alpha * 0.13f;
            if (plus_alpha <= 0) plus_alpha = 0;
            scene_info.back_screen.color = new Vector4(0, 0, 0, plus_alpha);
            if (plus_alpha <= 0)
            {
                plus_alpha = 0;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
