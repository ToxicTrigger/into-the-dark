using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpUI : MonoBehaviour {

    //public GameObject Image;
    public GameObject red_hp;

    float max_width;

    public int max_hp;

    float current_width;
    public bool onoff;

    public float hp_percentage;

    public RectTransform rt;
    RectTransform this_rt;

    Boss_Worm boss;
    public Vector3 ui_up_pos;
    public float slide_speed;
    Vector3 ui_down_pos; 

	void Start () {
        boss = BossRoomManager.get_instance().get_boss();
        BossRoomManager.get_instance().set_hp_ui(this);
        red_hp = this.transform.GetChild(0).gameObject;
        max_hp = boss.get_max_hp();
        rt = red_hp.GetComponent<RectTransform>();
        this_rt = this.GetComponent<RectTransform>();
        max_width = rt.rect.width;

        onoff = false;
        ui_down_pos = this_rt.position;
        ui_up_pos = new Vector3(ui_down_pos.x, ui_up_pos.y, ui_down_pos.z);
    }

    void Update()
    {
        if (onoff)
        {
            hp_percentage = (float)boss.get_hp() / (float)max_hp;

            current_width = max_width * hp_percentage;

            rt.sizeDelta = new Vector2(current_width, rt.rect.height);
        }
    }

    public void switching_ui(bool _onoff)
    {
        onoff = _onoff;
        if (onoff)
        {
            StartCoroutine(slide_ui(Vector3.up));
        }
        else
        {
            StartCoroutine(slide_ui(Vector3.down));
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
