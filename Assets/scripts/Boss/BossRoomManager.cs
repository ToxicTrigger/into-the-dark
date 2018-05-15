using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour {

    private static BossRoomManager instance = null;

    public static BossRoomManager get_instance()
    {
        if (instance == null)
        {
            //FindObjectOfType은 유니티에서 비용이 큰 함수지만 처음 한번만 호출되므로 괜찮음~
            instance = GameObject.FindObjectOfType(typeof(BossRoomManager)) as BossRoomManager;
            if (instance == null)
                Debug.LogError("Singleton Error");
        }

        return instance;
    }

    /// ///////////////////////////////////////////////////
    public BossStemWorm boss;
    public Player player;

	void Start () {

	}
	
	void Update () {
		if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D))    //키입력이 들어오면 캐릭터 이동중이어서 소리가 난다고 가정한다.
        {
            send_signal(player.transform.position, "B");
        }
	}

    //신호a를 
    public void send_signal(Vector3 _sound_pos , string _signal_type)
    {
        boss.signal_receive(_sound_pos, _signal_type);
    }

}
