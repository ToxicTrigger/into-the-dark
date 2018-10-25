using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour {

    public Image ending_image;
    public RectTransform image_rt;
    public float speed;
    public float y_pos;
    public AudioSource ending_sound;
    IEnumerator timer;

    private void Start()
    {
    }

    public void play_scroll()
    {
        Debug.Log("play_scroll");
        if (timer == null)
        {
            timer = scroll();
            StopCoroutine(timer);
            StartCoroutine(timer);
        }
    }

    public float time = 0;
    IEnumerator scroll()
    {
        time = 0;
        Debug.Log("scroll");
        ending_image.color = new Vector4(1, 1, 1, 1);
        ending_sound.Play();
        while (true)
        {
            image_rt.position += Vector3.up * speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            //time += 0.01f;
            if (image_rt.position.y >= y_pos)
                break;
        }
        //초기 씬으로
    }
}
