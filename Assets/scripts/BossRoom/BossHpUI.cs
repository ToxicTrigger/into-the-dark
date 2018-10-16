﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpUI : MonoBehaviour {

    //public GameObject Image;
    public GameObject yellow_hp;

    float max_width;

    public int max_hp;

    float current_width;
    public bool onoff;

    public float hp_percentage;

    public RectTransform rt_red, rt_yellow;
    RectTransform this_rt;

    Boss_Worm boss;
    public Vector3 ui_up_pos;
    public float slide_speed;
    Vector3 ui_down_pos;

    IEnumerator corutine;

    public float red_speed;

	void Start () {
        boss = BossRoomManager.get_instance().get_boss();
        BossRoomManager.get_instance().set_hp_ui(this);
        yellow_hp = this.transform.GetChild(1).gameObject;
        max_hp = boss.get_max_hp();
        rt_yellow = yellow_hp.GetComponent<RectTransform>();
        this_rt = this.GetComponent<RectTransform>();
        max_width = rt_yellow.rect.width;

        onoff = false;
        ui_down_pos = this_rt.position;
        ui_up_pos = new Vector3(ui_down_pos.x, ui_up_pos.y, ui_down_pos.z);
        corutine = slide_ui(Vector3.up);
    }

    void Update()
    {
        if (onoff)
        {
            hp_percentage = (float)boss.get_hp() / (float)max_hp;

            current_width = max_width * hp_percentage;

            rt_yellow.sizeDelta = new Vector2(current_width, rt_yellow.rect.height);
        }
        if (rt_red.sizeDelta.x > rt_yellow.sizeDelta.x)
        {
            rt_red.sizeDelta = new Vector2(rt_red.sizeDelta.x -red_speed, rt_red.rect.height);
        }
    }

    public void switching_ui(bool _onoff)
    {
        onoff = _onoff;
        if (onoff)
        {
            this_rt.position = new Vector3(this_rt.position.x, ui_up_pos.y, this_rt.position.z);
            //StopCoroutine(corutine);
            //corutine = slide_ui(Vector3.up);
            //StartCoroutine(corutine);
        }
        else
        {
            this_rt.position = new Vector3(this_rt.position.x, -200 , this_rt.position.z);
            //StopCoroutine(corutine);
            //corutine = slide_ui(Vector3.down);
            //StartCoroutine(corutine);
        }
    }

    IEnumerator slide_ui(Vector3 _move_dir)
    {
        while (true)
        {
            this_rt.position += _move_dir * slide_speed * Time.deltaTime;

            yield return new WaitForSeconds(0.01f);

            if(_move_dir == Vector3.up)
            {
                if(this_rt.position.y >= ui_up_pos.y)
                {
                    //Debug.Log("올라옴 완료");
                    this_rt.position = ui_up_pos;
                    break;
                }
            }
            else if(_move_dir == Vector3.down)
            {
                if(this_rt.position.y <= ui_down_pos.y)
                {
                    //Debug.Log("내려감 완료");
                    this_rt.position = ui_down_pos;
                    break;
                }
            }
        }
    }

}
