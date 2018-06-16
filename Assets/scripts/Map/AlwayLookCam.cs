using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwayLookCam : MonoBehaviour {
	public enum Type
	{
		Normal,
		LookAt
	}
	public bool Updown;
	public float up_scale;
	public float min_dist = 4.0f;
	public Transform Target;
	public float a = 0.0f;
	public Color color, text_color;
	float origin_y;
	public float per;
	public float dis;
	public TextMesh text;
	public Text_Effect effect;
	public Type type;

	public SpriteRenderer img;

	private void Start() {
		img = GetComponent<SpriteRenderer>();	
		color = img.color;
		origin_y = transform.position.y;
		per = min_dist * 0.01f;

		effect = transform.GetChild(0).GetComponent<Text_Effect>();
		text = transform.GetChild(0).GetComponent<TextMesh>();
		
		text_color = text.color;
	}
	bool textevent;
	void Update_alpha()
	{
		if( Vector3.Distance(Target.position, transform.position) <= min_dist)
		{
				dis = Vector3.Distance(Target.position, transform.position);
				a = Mathf.Abs(1.0f - dis * per);
				if(effect != null)
				if(!textevent)
				{
					effect.StartEffect();
					textevent = true;
				}
				
				//a = Mathf.Clamp(a , 0, 1);
		}else{
			a = 0;
			if(effect != null)
			effect.reset = true;
			textevent = false;
		}
	}

	void Update () {
		Update_alpha();

		color.a = a;
		img.color = color;
		if(text_color != null & text != null)
		{
			text_color.a = a;
			text.color = text_color;
		}

		switch(type)
		{
			case Type.Normal:
				transform.LookAt(Camera.main.transform);
				Vector3 eur = transform.eulerAngles;
				eur.x = 0;
				transform.eulerAngles = eur;
				break;
			case Type.LookAt:
				transform.LookAt(Camera.main.transform);
				Vector3 e = transform.eulerAngles;
				transform.eulerAngles = e;
				break;
		}


		if(Updown)
		{
			Vector3 vec = transform.position;
			float y = Mathf.Sin(Time.realtimeSinceStartup * up_scale) + origin_y;
			vec.y = y;
			transform.position = vec;
		}
	}

	private void OnDisable() {
				a = 0;
			if(effect != null)
			effect.reset = true;
			textevent = false;	
	}
}
