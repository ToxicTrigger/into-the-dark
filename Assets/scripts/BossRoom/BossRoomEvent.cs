using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEvent : MonoBehaviour {
    //trigger_event는 트리거에만 동작하는 것을 조건으로 함.

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
    public struct Event_Info
    {
        [Tooltip("플레이어 팔로우캠은 player_follow")]
        public string event_name;
        [Tooltip("카메라의 핀정보와 매칭시킬 핀 숫자입력 // 플레이어 팔로우캠은 상관x")]
        public int pin_num;
        [Tooltip("카메라 상태에 대한 설정")]
        public ActionCamera.State cam_state;
        [Tooltip("이벤트의 최소 유지 시간")]
        public float stand_by_time;
        public Zoom_Info zoom_info;
        public float action_speed;
        public Vector3 flow_dir;
        public bool is_player_ctrl;
        public float start_delay;
    };

    public Event_Info[] event_scene;

    public bool trigger_event;
    [Tooltip("<trigger_event용> 일회용의 경우 유지시간을 반드시 적어주세요.")]
    public bool is_disposable;

    BossRoomManager manager;
    ActionCamera ac;
    Player player;
    public CharacterController p_ctrl;

    private void Start()
    {
        manager = BossRoomManager.get_instance();
        ac = FindObjectOfType<ActionCamera>();
        player = FindObjectOfType<Player>();
        p_ctrl = player.GetComponent<CharacterController>();
    }

    public void play_event()
    {
        //for (int i = 0; i < event_scene.Length; i++)
        //{
        //    if(event_scene[i].cam_state != ActionCamera.State.Follow)
        //        ac.SetStateTarget(event_scene[i].pin_num, event_scene[i].cam_state, event_scene[i].action_speed, event_scene[i].stand_by_time, ac.Pins[event_scene[i].pin_num].eulerAngles, event_scene[i].event_name,event_scene[i].flow_dir);
        //    else
        //        ac.SetStateTarget(player.transform, event_scene[i].cam_state, event_scene[i].action_speed, event_scene[i].stand_by_time, ac.default_angle, event_scene[i].event_name, event_scene[i].flow_dir);
        //}
        StartCoroutine(event_timer());
    }

    IEnumerator event_timer()
    {
        for (int i = 0; i < event_scene.Length; i++)
        {
            yield return new WaitForSeconds(event_scene[i].start_delay);

            if (!event_scene[i].is_player_ctrl)
            {
                p_ctrl.enabled = false;
            }
            else
            {
                if(!manager.is_game_over)
                    p_ctrl.enabled = true;
            }

            if (event_scene[i].cam_state != ActionCamera.State.Follow)
                ac.SetStateTarget(event_scene[i].pin_num, event_scene[i].cam_state, event_scene[i].action_speed, event_scene[i].stand_by_time, ac.Pins[event_scene[i].pin_num].eulerAngles, event_scene[i].event_name, event_scene[i].flow_dir);
            else
                ac.SetStateTarget(player.transform, event_scene[i].cam_state, event_scene[i].action_speed, event_scene[i].stand_by_time, ac.default_angle, event_scene[i].event_name, event_scene[i].flow_dir);
            
        }
    }

    //일단 트리거 이벤트는 무조건 하나라는 가정하에 진행
    private void OnTriggerEnter(Collider other)
    {
        if (trigger_event)
        {
            if (other.gameObject.name.Equals("Player"))
            {
                if (event_scene[0].zoom_info.is_use)
                {
                    ac.ZoomInOut(event_scene[0].zoom_info.fov, event_scene[0].zoom_info.is_zoom, event_scene[0].zoom_info.zoom_speed);
                }

                for (int i = 0; i < event_scene.Length; i++)
                {
                    if (event_scene[i].cam_state != ActionCamera.State.Follow)
                        ac.SetStateTarget(event_scene[i].pin_num, event_scene[i].cam_state, event_scene[i].action_speed, event_scene[i].stand_by_time, ac.Pins[event_scene[i].pin_num].eulerAngles, event_scene[i].event_name, event_scene[i].flow_dir);
                    else
                        ac.SetStateTarget(player.transform, event_scene[i].cam_state, event_scene[i].action_speed, event_scene[i].stand_by_time, ac.default_angle, event_scene[i].event_name, event_scene[i].flow_dir);
                }

                if (is_disposable)
                {
                    trigger_event = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (trigger_event)
        {
            if (other.gameObject.name.Equals("Player"))
            {
                if (event_scene[0].zoom_info.is_use)
                {
                    if (event_scene[0].zoom_info.fov >= ac.default_fov)
                    {
                        ac.ZoomInOut(ac.default_fov, true, event_scene[0].zoom_info.zoom_speed);
                    }
                    else
                    {
                        ac.ZoomInOut(ac.default_fov, false, event_scene[0].zoom_info.zoom_speed);
                    }
                }

                ac.SetStateTarget(player.transform, ActionCamera.State.Follow, ac.default_speed, 0.0f, ac.default_angle,"player_follow", Vector3.zero);
            }
        }
    }

}
