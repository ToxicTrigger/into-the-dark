using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class HpControl : MonoBehaviour {
	public Image Hp_red;
	public Image Hp_White;
	public Player player;

	public float timer = 2.0f;
	public float tick = 0;
	public bool done;
	public float hp_delta;

	private void Awake() {
		player = GameObject.FindObjectOfType<Player>();	
	}
	
	void Update () {
		if(tick >= timer)
		{
			done = true;
		}else{
			done = false;
		}

		if(player.damageable.has_hit)
		{
			tick = 0;
		}else{
			if(tick <= timer)
			{
				tick += Time.deltaTime;
			}else{
				if( done ) 
				{
					Hp_red.fillAmount = Mathf.Lerp(Hp_red.fillAmount, Hp_White.fillAmount, Time.deltaTime* 1.5f);
				}	
			}
		}
		Hp_White.fillAmount = (player.damageable.Hp / 100);
	}
}
