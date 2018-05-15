using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalASound : MonoBehaviour {
    //보스에게 A신호를 주는 소리를 발생시키는 오브젝트이다.
    //플레이어와 몬스터 누구든지 발생시킬 수 있다.

    string signal_type = "A";

	void Start () {
		
	}
	
	void Update () {
		
	}

    //무엇이든 범위에 들어온다면 매니저에게 신호를 보낸다. other의 위치와 signal_type을 함께 보낸다.
    void OnTriggerEnter(Collider other)
    {
        BossRoomManager.get_instance().send_signal(other.transform.position, signal_type);
    }

}
