using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotemPointUI : MonoBehaviour {

    public Image image_rt;
    public float y_dis;
    public Player player;
    float alpha=0;
    void Start () {
        //player = FindObjectOfType<Player>();
        image_rt.color = new Vector4(1, 1, 1, alpha);
    }

    void Update()
    {
        if (player != null && player.enabled == true)
        {
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= 15)
            {
                alpha += 0.05f;
                if (alpha >= 1.0f) alpha = 1;
                image_rt.color = new Vector4(1, 1, 1, alpha);
                image_rt.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0.0f, y_dis, 0.0f));
            }
            else
            {
                if (alpha != 0)
                {
                    alpha -= 0.05f;
                    if (alpha <= 0.0f) alpha = 0;
                    image_rt.color = new Vector4(1, 1, 1, alpha);
                    image_rt.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + new Vector3(0.0f, y_dis, 0.0f));
                }
            }
        }
    }
}
