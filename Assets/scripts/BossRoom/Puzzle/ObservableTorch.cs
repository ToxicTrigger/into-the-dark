using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableTorch : Observable {
    //고대병기와 상호작용하는 횃불

    public enum State
    {
        On =0,  //켜짐
        Off       //꺼짐
    }    

    public State torch_state;
    public bool use_enabled;   //사용 가능 상태 (퍼즐을 모두 풀면 true로 바뀜)
    public Light torch_light;

    bool on_player=false;

	void Start () {
        use_enabled = false;
        torch_state = State.Off;
        torch_light.gameObject.SetActive(false);
    }
	
	void Update () {
		if(on_player && use_enabled && Input.GetKey(KeyCode.E))
        {
            torch_state = State.On;
            notify_all();
            use_enabled = false;
            torch_light.gameObject.SetActive(true);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        //사용가능 상태에 플레이어가 접근해서 E키를 누름
        if(other.CompareTag("Player"))
        {
            on_player = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            on_player = false;
        }
    }

    void notify_all()
    {
        //내가 가지고있는 옵저버 수만큼 실행
        for(int i=0; i < this.observers.Count; i++)
        {
            Observer weapon = this.observers[i] as Observer;
            weapon.notify(this);
        }
    }

    public State get_state()
    {
        return torch_state;
    }

    //스위치에 변화가 생김, 보스가 피가 1깎임(오브젝트 재배치), 보스의 그로기 상태가 풀림 일때 다시 거점을 활성화 해야 하므로... (통상 스위치에 변화가 생겼을 때! 가 빛이 꺼지는 때 갇 ㅚㄹ듯,,,,) 
    public virtual void off_light()
    {
        use_enabled = false;
        torch_state = State.Off;
        notify_all();    //불이 꺼졌으면 꺼짐을 알리기위해 ancientweapon에게 알림            
        torch_light.gameObject.SetActive(false);
        Debug.Log(this.name + " : off_light() 호출");
    }

    public virtual void on_light()
    {
        use_enabled = true;
        //BossRoomManager.get_instance().send_boss_state(Boss_Worm.Action.Rush_Attack);
    }

}
