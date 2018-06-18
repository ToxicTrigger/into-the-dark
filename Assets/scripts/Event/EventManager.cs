using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    //현재 실행중인 이벤트의 정보를 읽어와 실행하는 역할을 한다. 

    private static EventManager instance = null;

    public static EventManager get_instance()
    {
        if (instance == null)
        {
            //FindObjectOfType은 유니티에서 비용이 큰 함수지만 처음 한번만 호출되므로 괜찮음~
            instance = GameObject.FindObjectOfType(typeof(EventManager)) as EventManager;
            if (instance == null)
                Debug.LogError("Singleton Error");
        }

        return instance;
    }
    /// /////////////////////////////////////////////////////////////////////////////////////

    public GameObject player;
    public PlayerCamera p_camera;

    EventPlot cur_event;
	void Start () {
		
	}
	
	void Update () {
		
	}

    public GameObject get_player()
    {
        return player;
    }

    //이벤트를 실행할 준비를 한다. 모든 키입력에 대한 컨트롤을 멈춘다. (대표적으로 플레이어와 카메라)
    public void event_setting(EventPlot _event)
    {
        p_camera.set_state(PlayerCamera.State.Event);
        cur_event = _event;
        cur_event.set_play_event(true);
        cur_event.set_event(p_camera);
    }

    public void off_event()
    {
        p_camera.set_state(PlayerCamera.State.Follow);  //현재 follow상태로 바꿔주고 있으나 다음부터는 이전 상태 (원래 상태로 바꿔야함) 고정에서 이벤트로 바뀌었을 수도 있으니까!
        cur_event.set_play_event(false);
    }
    public enum Direction
    {
        Up_Down,
        Left_Right
    }
    Direction dir;
    public void camera_shake(float _power, int loop_cnt, float loop_speed, Direction _dir,float minus)
    {
        if (!p_camera.shake)
        {
            switch (_dir)
            {
                case Direction.Up_Down:
                    p_camera.up_down_move(_power, loop_cnt, loop_speed, minus);
                    break;
                case Direction.Left_Right:
                    p_camera.left_right_move(_power, loop_cnt, loop_speed);
                    break;
                default:
                    break;
            }
        }
    }
}
