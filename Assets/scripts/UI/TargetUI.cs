using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUI : MonoBehaviour {

    public Transform target;
    public float up_dis;

    Vector3 pos;
    RectTransform rt;
    public bool is_switch;
	void Start () {
        rt = this.GetComponent<RectTransform>();
	}
	
	void Update () {

        if (!is_switch)
        {
            if (Camera.main.WorldToScreenPoint(target.position).x > Camera.main.pixelWidth ||
                    Camera.main.WorldToScreenPoint(target.position).x < 0.0f ||
                    Camera.main.WorldToScreenPoint(target.position).y > Camera.main.pixelHeight ||
                    Camera.main.WorldToScreenPoint(target.position).y < 0.0f)
            {
                pos = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0.0f);
                //Debug.Log(Camera.main.pixelWidth +" , " +Camera.main.pixelHeight + " =>"+_pos);
                Vector3 _dir = (Camera.main.WorldToScreenPoint(target.position) - new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0.0f)).normalized;
                while (true)
                {
                    if (pos.x + 10 > Camera.main.pixelWidth ||
                        pos.x - 10 < 0.0f ||
                        pos.y + 10 > Camera.main.pixelHeight ||
                        pos.y - 10 < 0.0f)
                    {
                        //Debug.Log("break");
                        break;
                    }
                    pos += _dir;
                    pos.y = pos.y + up_dis;
                }
                rt.transform.position = pos;
            }
            else
                rt.transform.position = Camera.main.WorldToScreenPoint(target.position + new Vector3(0, up_dis, 0));
        }
        else
            rt.transform.position = Camera.main.WorldToScreenPoint(target.position + new Vector3(0, up_dis, 0));

    }
}
