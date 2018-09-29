using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiGroggyPoint : MonoBehaviour {

    public Sprite img;
    Image img_ui;

    public bool boss_groggy;
    public Transform groggy_point;

    Transform s;

    float height;
    float width;


    void Start () {
        img_ui = this.gameObject.GetComponent<Image>();
        img_ui.color = new Vector4(1,1,1,0);
        boss_groggy = false;
        BossRoomManager.get_instance().set_groggy_ui(this);

        height = Camera.main.orthographicSize;
        width = (height*2) * Camera.main.aspect/2;

    }
    Vector3 _pos;
    void Update () {

        if (boss_groggy)
        {
            if(Camera.main.WorldToScreenPoint(groggy_point.position).x > Camera.main.pixelWidth ||
                Camera.main.WorldToScreenPoint(groggy_point.position).x < 0.0f ||
                Camera.main.WorldToScreenPoint(groggy_point.position).y > Camera.main.pixelHeight ||
                Camera.main.WorldToScreenPoint(groggy_point.position).y < 0.0f)
            {                
                _pos = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0.0f);
                Debug.Log(Camera.main.pixelWidth +" , " +Camera.main.pixelHeight + " =>"+_pos);
                Vector3 _dir = (Camera.main.WorldToScreenPoint(groggy_point.position) - new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0.0f)).normalized;
                while(true)
                {
                    if (_pos.x +10 > Camera.main.pixelWidth ||
                        _pos.x -10 < 0.0f ||
                        _pos.y +10 > Camera.main.pixelHeight ||
                        _pos.y -10 < 0.0f)
                    {
                        Debug.Log("break");
                        break;
                    }
                        _pos += _dir;
                }
                img_ui.transform.position = _pos;
            }
            else
                img_ui.transform.position = Camera.main.WorldToScreenPoint(groggy_point.position);


        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            boss_groggy = boss_groggy == true ? false : true;
        }
	}

    public void set_boss_groggy(bool _groggy, Vector3 _point)
    {
        boss_groggy = _groggy;
        groggy_point.position = _point;

        if(boss_groggy == true)
            img_ui.color = new Vector4(1, 1, 1, 1);
        else
            img_ui.color = new Vector4(1, 1, 1, 0);

    }





}
