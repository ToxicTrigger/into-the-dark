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

	void Start () {
		
	}
	
	void Update () {
		
	}

    public GameObject get_player()
    {
        return player;
    }

    //이벤트를 실행할 준비를 한다. 모든 키입력에 대한 컨트롤을 멈춘다. (대표적으로 플레이어와 카메라)
    public void event_setting()
    {
        p_camera.set_state(PlayerCamera.State.Event);
    }
}
