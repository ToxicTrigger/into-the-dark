using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventState
{
	Off,
	Press_On,
	On,
    Press_Off,
}

//플레이 중 일정한 조건을 충족하였을 경우 일반 지형이 나타나는 변동
//Ex> 조건: 플레이어가 설정된 지형을 밟은 뒤 3초 후
public class Platform : MonoBehaviour 
{
	bool on_off;
	float pressing_time;
	public float MAX_PRESS_TIME = 3.0f;
	public bool only_player;
	bool is_pressing;
	public EventState state;

    private void OnCollisionStay(Collision collision)
    {
        this.OnTriggerStay(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        this.OnTriggerExit(collision.collider);
    }

    EventState cur;
    void change_state(EventState evn)
    {
            cur = state;
            state = evn;
    }

    private void OnTriggerStay(Collider other) 
	{
		if(only_player)
		{
			bool player = other.CompareTag("Player");
			if(player)
			{
				if(pressing_time <= MAX_PRESS_TIME)
				{
					pressing_time += Time.deltaTime;
					is_pressing = true;
                    if(cur == EventState.Off)
                    {
                        change_state(EventState.Press_On);
                    }
                    else if(cur == EventState.On)
                    {
                        change_state(EventState.Press_Off);
                    }
				}else{
					is_pressing = false;
					on_off = true;
                    if(state == EventState.Press_On)
                    {
                        change_state(EventState.On);
                    }
                    else if(state == EventState.Press_Off)
                    {
                        change_state(EventState.Off);
                    }
				}
			}
		}else{
			if(pressing_time <= MAX_PRESS_TIME)
			{
                pressing_time += Time.deltaTime;
                is_pressing = true;
                if (cur == EventState.Off)
                {
                    change_state(EventState.Press_On);
                }
                else if (cur == EventState.On)
                {
                    change_state(EventState.Press_Off);
                }
            }
            else
            {
                is_pressing = false;
                on_off = true;
                if (state == EventState.Press_On)
                {
                    change_state(EventState.On);
                }
                else if (state == EventState.Press_Off)
                {
                    change_state(EventState.Off);
                }
            }
		}
	}

	private void OnTriggerExit(Collider other) 
	{
		if(only_player)
		{
			bool player = other.CompareTag("Player");
			if(player)
			{
				on_off = false;
				pressing_time = 0;
				is_pressing = false;
                if(state == EventState.Press_Off || state == EventState.On)
                {
                    change_state(EventState.On);
                }
                else if(state == EventState.Press_On || state == EventState.Off)
                {
                    change_state(EventState.Off);
                }
			}
		}else{
			on_off = false;
			pressing_time = 0;
			is_pressing = false;
            if (state == EventState.Press_Off || state == EventState.On)
            {
                change_state(EventState.On);
            }
            else if (state == EventState.Press_On || state == EventState.Off)
            {
                change_state(EventState.Off);
            }
        }
	}
}
