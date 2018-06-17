using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemUIConrtoller : MonoBehaviour {
	public List<TotemInfo> totems;
	public Player player;

	private void Start() {
		player = FindObjectOfType<Player>();
	}

	
	// Update is called once per frame
	void Update ()
	{
		switch(player.cur_totems)
		{
			case 0 : totems[0].has_off = false; totems[1].has_off = false; totems[2].has_off = false; break;
			case 1 : totems[0].has_off = true; totems[1].has_off = false; totems[2].has_off = false; break;
			case 2 : totems[0].has_off = true; totems[1].has_off = true; totems[2].has_off = false; break;
			case 3 : totems[0].has_off = true; totems[1].has_off = true; totems[2].has_off = true; break;
			
		}
	}
}
