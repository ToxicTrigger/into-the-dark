using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AWTimerUI : MonoBehaviour {
    //고대병기 타이머 ui

    public GameObject yellow_time;

    float max_width;
    float current_width;

    public float max_time;
    public float cur_time=0;

    public float time_percentage;

    public RectTransform rt;

    public bool onoff;

    Image image; 

    void Start () {
        rt = yellow_time.GetComponent<RectTransform>();
        image = this.gameObject.GetComponent<Image>();
        BossRoomManager.get_instance().set_ancient_ui(this);
        max_width = rt.rect.width;
        switching_ui(false,0.0f);
        
    }
	
	void Update () {

        if (onoff)
        {
            cur_time += Time.deltaTime;
            
            time_percentage = (max_time - cur_time) / (float)max_time;
            current_width = max_width * time_percentage;
            rt.sizeDelta = new Vector2(current_width, rt.rect.height);
            if (cur_time > max_time)
                switching_ui(false, 0.0f);
        }
	}

    public void switching_ui(bool _onoff, float _max_time)
    {
        onoff = _onoff;
        if(onoff)
        {
            image.color = new Vector4(image.color.r, image.color.g, image.color.b, 1);
            rt.gameObject.SetActive(true);
            max_time = _max_time;
        }
        else
        {
            cur_time = 0;
            rt.sizeDelta = new Vector2(max_width, rt.rect.height);
            image.color = new Vector4(image.color.r, image.color.g, image.color.b, 0);
            rt.gameObject.SetActive(false);
        }
    }
    
}
