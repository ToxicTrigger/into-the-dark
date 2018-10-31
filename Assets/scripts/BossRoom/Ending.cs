using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour {

    public RectTransform image_rt;
    public Image ending_image;
    public float speed;
    public float y_pos;
    public AudioSource ending_sound;

    public void play_scroll()
    {
        StartCoroutine(scroll());
    }

    IEnumerator scroll()
    {
        ending_sound.Play();
        ending_image.color = new Vector4(1, 1, 1, 1);

        while (true)
        {
            image_rt.position += Vector3.up * speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            if (image_rt.position.y > y_pos)
                break;
        }

        SceneManager.LoadScene(0);
    }

}
