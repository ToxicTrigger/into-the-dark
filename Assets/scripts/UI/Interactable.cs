using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	[Tooltip("대상과 상호 작용 가능한 거리")]
	public float talk_distance = 1.0f;
	[Tooltip("상호작용할 대상")]
	public Transform target;
	[Tooltip("대상과 상호작용 상태 인가?")]
	public bool hasTalking;

	[Tooltip("상호작용 상태 일 때 활성화 되어야 하는 것들")]
	public List<GameObject> events;
	public EButton e;

	// 대상이 거리안에 있나?
	bool get_capture_area;

	void Talk()
	{
		if(get_capture_area)
		{
			if(e != null & !hasTalking) e.Up();
			if(Input.GetKeyDown(KeyCode.E))
			{
				if(e != null) e.Down(); 
				hasTalking = true;
			}
		}else{
			hasTalking = false;
		}
	}

	void update_()
	{
		if(Vector3.Distance(target.position, transform.position) <= talk_distance)
		{
			
			get_capture_area = true;
			Talk();
		}else{
			get_capture_area = false;
			hasTalking = false;
			if(e != null) e.Down(); 
		}
	}

	void update_event()
	{
		if(hasTalking)
		{
			IEnumerator iter = events.GetEnumerator();
			while(iter.MoveNext())
			{
				GameObject tmp = iter.Current as GameObject;
				tmp.SetActive(true);
			}
		}else{
			IEnumerator iter = events.GetEnumerator();
			while(iter.MoveNext())
			{
				GameObject tmp = iter.Current as GameObject;
				tmp.SetActive(false);
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
		update_();
		update_event();
	}
}
