using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventState
{
	Off,
	Press,
	On,
}

//플레이 중 일정한 조건을 충족하였을 경우 일반 지형이 나타나는 변동
//Ex> 조건: 플레이어가 설정된 지형을 밟은 뒤 3초 후
public class Platform : MonoBehaviour 
{
	bool on_off;
	float pressing_time;
	public float MAX_PRESS_TIME = 3.0f;
	bool only_player;
	bool is_pressing;
	public EventState state;

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
					state = EventState.Press;
				}else{
					is_pressing = false;
					on_off = true;
					state = EventState.On;
				}
			}
		}else{
			if(pressing_time <= MAX_PRESS_TIME)
			{
				pressing_time += Time.deltaTime;
				is_pressing = true;
				state = EventState.Press;
			}else{
				is_pressing = false;
				on_off = true;
				state = EventState.On;
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
				state = EventState.Off;
			}
		}else{
			on_off = false;
			pressing_time = 0;
			is_pressing = false;
			state = EventState.Off;
		}
	}
}
