using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaCaster : MonoBehaviour {
	public Player player;
	public bool draw_ui;
	public float speed;
	public Color alpha;
	public string layerName;

	public bool is_Totem_UI;

	public List<Image> uis;
	public List<Text> texts;

	private void Start() {
		uis = new List<Image>();
		texts = new List<Text>();

		Image[] img = FindObjectsOfType<Image>();
		Text[] tex = FindObjectsOfType<Text>();

		int ui = LayerMask.NameToLayer(layerName);

		IEnumerator iter = img.GetEnumerator();
		while(iter.MoveNext())
		{
			Image i = iter.Current as Image;
			if(i.gameObject.layer.Equals(ui))
			{
				uis.Add(i);
			}
		}

		iter = tex.GetEnumerator();
		while(iter.MoveNext())
		{
			Text i = iter.Current as Text;
			if(i.gameObject.layer.Equals(ui))
			{
				texts.Add(i);
			}
		}
	}

	bool EventOn;
	IEnumerator up()
	{
		EventOn = true;
		while(alpha.a<=1)
		{
			alpha.a += speed;
			yield return new WaitForSeconds(0.001f);
		}
		EventOn = false;
	}

	public void Up()
	{
		if(!EventOn)
		StartCoroutine(up());
	}

	public void Down()
	{
		if(!EventOn)
		StartCoroutine(down());
	}

	IEnumerator down()
	{
		EventOn = true;
		while(alpha.a >= 0)
		{
			alpha.a -= speed;
			yield return new WaitForSeconds(0.05f);
		}
		EventOn = false;
	}
	
	float tick;
	bool hasOn;
	
	void Update () 
	{
		if(hasOn)
		{
			if(tick >= 3)
			{
				tick = 0;
				hasOn = false;
				Down();
			}
		}
		if(!is_Totem_UI)
		{
			if(!player.is_target_something.Equals(0) || player.is_fighting_something)
			{
				hasOn = true;
				tick = 0;
				Up();
			}else{
				tick += Time.deltaTime;
			}
		}else{
			if(player != null)
			{
				if(player.is_build_totem)
				{
					hasOn = true;
					tick = 0;
					Up();
				}else{
					tick += Time.deltaTime;
				}
			}
		}


		IEnumerator iter = uis.GetEnumerator();
		while(iter.MoveNext())
		{
			Image img = iter.Current as Image;
			img.color = alpha;
		}
		iter = texts.GetEnumerator();
		while(iter.MoveNext())
		{
			Text img = iter.Current as Text;
			img.color = alpha;
		}
	}
}
