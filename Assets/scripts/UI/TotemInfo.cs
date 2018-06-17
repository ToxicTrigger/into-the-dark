using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotemInfo : MonoBehaviour {
	public Sprite On, Off;
	public Image me;
	public bool has_off;

	void Start()
	{
		me = GetComponent<Image>();
	}
	
	void Update () {
		if(has_off)
		{
			me.sprite = Off;
		}else{
			me.sprite = On;
		}
	}
}
