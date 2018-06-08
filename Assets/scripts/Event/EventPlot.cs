using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlot : MonoBehaviour {
    //실질적인 이벤트의 정보가 들어가는 스크립트이다.

    public enum CameraEffect
    {
        Fade_In,
        Fade_Out,
        Shake
    }

    public enum Finish_Condition
    {
        time_out,
        move_finish
    }

    //하나의 씬에 들어가는 정보의 목록
    [System.Serializable]
    public class Scene_Data
    {
        public Transform camera_position;
        public bool follow;
        public float cam_speed;
        public CameraEffect[] c_effect;
        public Transform player_move_target;
        public float player_speed;
        public Finish_Condition scene_change_condition;
        public float maintain_time;
    }

    public Scene_Data[] scene;
    PlayerCamera p_camera;
    GameObject player;
    int scene_turn; //현재 씬의 순서 

	void Start () {
		
	}
	
	void Update () {
		
	}

    void play_scene()
    {
        //*플레이어 처리*
        

        //*카메라 처리*플레이어가 이동하는 만큼 본인도 이동한다.
        if (scene[scene_turn].follow)
        {

        }
    }

    void set_event(PlayerCamera _cam, GameObject _player)
    {
        p_camera = _cam;
        player = _player;

        p_camera.transform.position = scene[scene_turn].camera_position.position;

    }

    void send_event()
    {
        EventManager.get_instance().event_setting();
    }
}
