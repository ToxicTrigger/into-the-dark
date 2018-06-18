using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaInfo : MonoBehaviour {
	public Text areaTitle;
	public Image L, R;
	public Color text_color;
	public bool EventOn;
	public float speed = 0.01f;
	IEnumerator up()
	{
		EventOn = true;
		while(text_color.a<=1)
		{
			text_color.a += speed;
			yield return new WaitForSeconds(0.01f);
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
		while(text_color.a >= 0)
		{
			text_color.a -= speed;
			yield return new WaitForSeconds(0.01f);
		}
		EventOn = false;
	}
	void Start () 
	{
		text_color = areaTitle.color;
	}
	
	// Update is called once per frame
	void Update () {
		areaTitle.color = text_color;
		L.color = text_color;
		R.color = text_color;
	}
	IEnumerator UpDown()
	{
		Up();
		yield return new WaitForSeconds(2.0f);
		Down();
	}

	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Area"))
		{	
			areaTitle.text =  other.name;
			StartCoroutine(UpDown());
		}
	}
}
