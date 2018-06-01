using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RushAttackCount : MonoBehaviour
{

    public Text count_text;
    public GameObject player;
    public Camera view_camera;
    public float y_index;

    private void Start()
    {
        count_text.text = " ";
    }

    private void Update()
    {
        count_text.transform.position = view_camera.WorldToScreenPoint(player.transform.position + new Vector3(0, y_index, 0));
    }

    //시간에 따라 UI를 갱신해준다.
    public void renew_time(int _time)
    {
        if (_time == 0)
            count_text.text = " ";
        else
            count_text.text = _time.ToString(); //넘어온 숫자를 문자로 변환해 넣어줌 
    }

}
