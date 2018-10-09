using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEvent : MonoBehaviour {

    public Player player;

    [System.Serializable]
    public struct Zoom_Info
    {
        [Tooltip("true = 사용함 || false = 사용안함")]
        public bool is_use;
        public float fov;
        [Tooltip("true = zoomIn || false = zoomOut")]
        public bool is_zoom;
        public float zoom_speed;
    }

    [System.Serializable]
    public struct Evnet_Info
    {
        [Tooltip("카메라의 핀정보와 매칭시킬 핀 숫자입력")]
        public int pin_num;
        [Tooltip("카메라 상태에 대한 설정")]
        public ActionCamera.State cam_state;
        [Tooltip("이벤트의 최소 유지 시간")]
        public float stand_by_time;
        public Zoom_Info zoom_info;

    };

    public Evnet_Info[] event_scene;
    public string event_name;

    public bool trigger_event;
    public float angle;
    public Vector3 offset;


    BossRoomManager manager;
    ActionCamera ac;
    int count;

    IEnumerator timer;

    public bool play_event()
    {
        if(timer == null)
        {
            timer = event_send();
            StartCoroutine(timer);
            return true;
        }
        else
        {
            Debug.Log("Error : 이벤트 동작중 >> " + event_name);
            return false;
        }
    }

    IEnumerator event_send()
    {
        for(int i =0; i<event_scene.Length; i++)
        {
            ac.Angle = ac.Pins[event_scene[count].pin_num].eulerAngles;
            ac.SetStateTarget(ac.Pins[event_scene[count].pin_num], ActionCamera.State.Move_Pin);

            if(event_scene[i].zoom_info.is_use)
            {
                ac.ZoomInOut(event_scene[i].zoom_info.fov, event_scene[i].zoom_info.is_zoom, event_scene[i].zoom_info.zoom_speed);
            }

            yield return new WaitForSeconds(event_scene[i].stand_by_time);
        }
        timer = null;
    }

    public string get_event_name()
    {
        return event_name;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (trigger_event && other.name == "Player")
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(trigger_event && other.name == "Player")
        {
            ac.Angle = ac.default_angle;
            ac.SetStateTarget(player.transform ,ActionCamera.State.Follow);
        }
    }

}
