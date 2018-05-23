using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientWeapon : Observer {
    //고대병기 
    enum State
    {
        Activated=0,    //활성화된
        Deactivated     //비활성화된
    }

    int activate_count = 0; //현재까지 활성화된 횟수를 저장
    public float[] time_list;   //엔진에서 횟수에 따른 시간을 지정해줄거임
    float timer;    //활성화 유지 시간

    State state;

	void Start () {
        state = State.Deactivated;  //초기 상태는 비활성화된 상태
	}
	
	void Update () {
		
	}

    public override void notify(Observable observable)
    {
        throw new System.NotImplementedException();
    }

    //활성화 시키는 함수
    void activate()
    {
        state = State.Activated;
    }
    
    //비활성화 시키는 함수
    void deactivate()
    {
        state = State.Deactivated;
    }
}
