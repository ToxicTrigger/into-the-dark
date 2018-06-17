using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlot : MonoBehaviour
{
    //실질적인 이벤트의 정보가 들어가는 스크립트이다.

    public enum CameraEffect
    {
        Null,
        Fade_In,
        Fade_Out,
        Shake
    }

    public enum Finish_Condition
    {
        Null,
        time_out,
        move_finish
    }

    public enum Camera_Type
    {
        Fixed,
        Follow,
        Move
    }

    //하나의 씬에 들어가는 정보의 목록
    [System.Serializable]
    public class Scene_Data
    {
        public Transform camera_position;
        public bool target_ctrl;
        public Camera_Type camera_type;
        public Transform cam_pos_target;
        public float over_dis;
        public float cam_speed;
        public float rot_speed;
        public CameraEffect[] c_effect;
        public GameObject enemy;
        public Transform enemy_move_target;
        public Finish_Condition scene_change_condition;
        public float maintain_time;
        public bool fixed_x;
        public bool fixed_y;
        public bool fixed_z;
    }

    public Scene_Data[] scene;
    PlayerCamera p_camera;
    GameObject target;

    int scene_turn; //현재 씬의 순서 

    bool play_event = false;

    void Start()
    {

    }

    void Update()
    {
        if (play_event)
        {
            play_scene();
        }
    }

    Vector3 fixed_vector = Vector3.zero;
    float origin_distance;
    void play_scene()
    {
        //*타겟 처리*
        if (scene[scene_turn].target_ctrl)
        {
            scene[scene_turn].enemy.transform.position = scene[scene_turn].enemy_move_target.position;
        }

        //*카메라 처리*
        switch (scene[scene_turn].camera_type)
        {
            case Camera_Type.Fixed:
                p_camera.transform.position = scene[scene_turn].cam_pos_target.position;    //바로 target pos로 위치 고정
                break;
            case Camera_Type.Follow:
                break;
            case Camera_Type.Move:

                if (scene[scene_turn].fixed_x) fixed_vector.x = p_camera.transform.position.x;
                if (scene[scene_turn].fixed_y) fixed_vector.y = p_camera.transform.position.y;
                if (scene[scene_turn].fixed_z) fixed_vector.z = p_camera.transform.position.z;
                float cur_dis = Vector2.Distance(new Vector2(fixed_vector.x, fixed_vector.z), new Vector2(p_camera.transform.position.x, p_camera.transform.position.z));

                if ((origin_distance - cur_dis) / origin_distance < 0.9)
                {
                    p_camera.transform.position += (fixed_vector - p_camera.transform.position).normalized * scene[scene_turn].cam_speed * Time.deltaTime;
                }
                break;
        }

        //이벤트 종료으 ㅣ경우 시간, 이동의 완료, 외부에서의 입력 총 세가지로 이루어진다.
    }

    public void set_event(PlayerCamera _cam)
    {
        p_camera = _cam;
        //player = _player;

        p_camera.transform.position = scene[scene_turn].camera_position.position;

        Vector3 dir = ((scene[scene_turn].cam_pos_target.position - p_camera.transform.position).normalized * scene[scene_turn].over_dis);
        dir.x = 0;

        fixed_vector = scene[scene_turn].cam_pos_target.position + dir;
        
        origin_distance = Vector2.Distance(new Vector2(fixed_vector.x, fixed_vector.z), new Vector2(p_camera.transform.position.x, p_camera.transform.position.z));
    }

    public void set_play_event(bool _set)
    {
        play_event = _set;
    }

    void send_event()
    {
        EventManager.get_instance().event_setting(this);
    }
}
