using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AWTimerUI : MonoBehaviour {
    //고대병기 타이머 ui

    public Text ui_name, time;
    public Image back_ground;

    float max_width;
    float current_width;

    public float max_time;
    public float cur_time=0;


    public bool onoff;

    void Start () {
        BossRoomManager.get_instance().set_ancient_ui(this);
        switching_ui(false,0.0f);
        
    }
	
	void Update () {

        if (onoff)
        {
            cur_time =  (float)System.Math.Truncate(cur_time * 100) / 100;
            time.text = cur_time.ToString();

            cur_time -= Time.deltaTime;

            if (cur_time <= 0)
                switching_ui(false, 0.0f);
        }
	}

    public void switching_ui(bool _onoff, float _max_time)
    {
        Debug.Log(onoff);
        onoff = _onoff;
        max_time = _max_time;
        cur_time = max_time;
        if(onoff)
        {
            back_ground.enabled = true;
            ui_name.enabled = true;
            time.enabled = true;
        }
        else
        {
            back_ground.enabled = false;
            ui_name.enabled = false;
            time.enabled = false;
        }
    }
    
}
