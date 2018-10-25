using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScenePlay : MonoBehaviour {

    public SoundManager sound_manager;

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

    Image cur_scene, now_scene;
    public bool test;

    public bool is_clear;
    public Ending ending;

    private void Start()
    {
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
        if (test && Input.GetKeyDown(KeyCode.Space))
            play_scene();
    }

    public void play_scene()
    {
        if (timer == null)
        {
            timer = play_timer();
            StartCoroutine(timer);
            if (sound_manager != null)
                sound_manager.clear();
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
            if (cur_scene == scene_info.screen[0])
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
                if(cur_scene != null)
                {
                    minus_alpha -= add_alpha;
                    cur_scene.color = new Vector4(1, 1, 1, minus_alpha);
                }
                if (now_scene != null)
                {
                    plus_alpha += add_alpha;
                    now_scene.color = new Vector4(1, 1, 1, plus_alpha);
                }
                if (now_scene.color.a >= 1 && cur_scene == null || now_scene.color.a >= 1 && cur_scene.color.a <= 0)
                    break;

                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(scene_info.keep_time[i]);
            minus_alpha = 1;
            plus_alpha = 0;
            cur_scene = now_scene;
            now_scene = null;
        }

        while (true)
        {
            minus_alpha -= add_alpha;
            if (minus_alpha <= 0)
                minus_alpha = 0;
            cur_scene.color = new Vector4(1, 1, 1, minus_alpha);

            if (cur_scene.color.a <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("minus_alpha");
        while (true)
        {
            Debug.Log("vo");
            if (cut_scene_sound.volume >= 0.002)
                cut_scene_sound.volume -= 0.002f;
            yield return new WaitForSeconds(0.01f);
            Debug.Log("vo2");
            if (cut_scene_sound.volume <= 0.002f)
            {
                break;
            }
        }
        cut_scene_sound.Stop();
        Debug.Log("sound_stop");
        if (is_clear)
        {
            yield return new WaitForSeconds(3.0f);            
            scene_info.back_screen.color = new Vector4(0, 0, 0, 0);
            ending.play_scroll();
        }
        Debug.Log("is_clear");
        while (!is_clear)
        {
            plus_alpha -= add_alpha * 0.13f;
            scene_info.back_screen.color = new Vector4(0, 0, 0, plus_alpha);
            if (plus_alpha <= 0)
            {
                plus_alpha = 0;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("end");
        timer = null;
    }
}
