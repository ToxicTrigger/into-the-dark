using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toxic;

/// 게임 중 벌어지는 모든 입력 상황에 대하여 처리 합니다
public class InputManager : MonoBehaviour{
	private static InputManager me;
	protected float horizontal, vertical;
	protected float dash, dodge;
	protected bool no_input;
	float dash_tick;
	bool in_dash_one, in_dash_two;
	List<InputHandler> listeners;

	public static InputManager get_instance()
	{
		if(me != null)
		{
			return me;
		}
		me = FindObjectOfType<InputManager>();
		return me;
	}

	private void Awake() {
		listeners = new List<InputHandler>();
		InputHandler[] list = FindObjectsOfType<InputHandler>();
		for(int i = 0; i < list.Length ; i++)
		{
			listeners.Add(list[i]);
			Debug.Log("input Handler : " + listeners[i].name);
		}
		Debug.Log("Input Handler : " + listeners.Count);
	}

	public float get_Dash(){ return dash; }
	public float get_Dodge(){ return dodge; }
	public float get_Horizontal(){return horizontal;}
	public float get_Vertical(){return vertical;}
	public bool has_not_anyting_input() { return no_input;} 

	IEnumerator update_dash()
	{
		float time = 0;
		while(time <= 0.2f)
		{
			time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
	}

	void update_handlers()
	{
		for(int i = 0; i < listeners.Count; i++)
		{
			listeners[i].Work(this);
		}
	}

	void update_raw_input()
	{
		this.horizontal = Input.GetAxisRaw("Horizontal");
		this.vertical = Input.GetAxisRaw("Vertical");
		this.dodge = Input.GetAxisRaw("Dodge");

		if(horizontal != 0 || vertical != 0)
		{
			in_dash_one = true;
			
		}

		if(horizontal == 0 & vertical == 0 & dodge == 0 & dash == 0)
		{ no_input = true; } else { no_input = false; }

	}

	void FixedUpdate()
	{
		update_raw_input();
		update_handlers();
	}
}
