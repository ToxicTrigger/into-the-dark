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
    public Boss_Worm tboss;
    public Player player;
    public BasicSwitch [] all_switch;
    //public ObservableTorch[] torch;

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

    public void send_boss_state(Boss_Worm.Action _action)
    {
        tboss.action_ready(_action);
    }

    //고대병기 활성화시 호출하게될 함수로 보스의 상태를 그로기상태로 바꿔준다.
    public void boss_groggy()
    {
        boss.state_change(BossStemWorm.Action.Groggy);  //보스의 상태를 그로기상태로 바꿔줌
    }

    public void boss_groggy_end()
    {
        
        boss.state_change(BossStemWorm.Action.Groggy_End);  //보스를 그로기 끝 상태로 바꿔줌
    }

    public void off_switch()
    {
        for(int i =0; i<all_switch.Length; i++)
        {
            all_switch[i].set_switch(false);
            all_switch[i].off_switch_set();
        }
        //모든 스위치를 꺼줌
    }

   ////추후변경 촛불도 또 하나의 옵저버 이므로 지금은 매니저에서 일괄로 처리해주지만...!
   //public void torch_deactivation()
   //{
   //    for(int i =0; i< torch.Length; i++)
   //    {
   //        torch[i].off_light();
   //    }
   //}
}
