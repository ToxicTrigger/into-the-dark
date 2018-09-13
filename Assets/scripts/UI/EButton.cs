﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EButton : MonoBehaviour {
	public SpriteRenderer text;
	public Color color;
	public float speed;
	public bool EventOn;

	public void Start ()
    {
		text = GetComponent<SpriteRenderer>();
		color = text.color;
	}

	IEnumerator up()
	{
		EventOn = true;
		while(color.a<=1)
		{
			color.a += speed;
			yield return new WaitForSeconds(0.01f);
		}
		EventOn = false;
	}

	public void Up()
	{
		if(!EventOn)
        {
            StartCoroutine(up());
        }
	}

	public void Down()
	{
		if(!EventOn)
        {
            StartCoroutine(down());
        }
	}

	IEnumerator down()
	{
		EventOn = true;
		while(color.a >= 0)
		{
			color.a -= speed;
			yield return new WaitForSeconds(0.01f);
		}
		EventOn = false;
	}
	
	public void Update ()
    {
		text.color = color;
		transform.LookAt(Camera.main.transform);
	}
}
