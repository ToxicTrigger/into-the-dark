using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlatform : Platform 
{
	public Material color;
	public Color Off;
	public Color On;
	public Color Press;

	private void Awake()
	{
		color = this.GetComponent<MeshRenderer>().materials[0];
		Off = color.color;
	}

	private void Update() 
	{
		switch(this.state)
		{
			case EventState.Off:
				this.GetComponent<MeshRenderer>().materials[0].color = Off;
				break;
			case EventState.On:
				this.GetComponent<MeshRenderer>().materials[0].color = On;
				break;
			case EventState.Press:
				this.GetComponent<MeshRenderer>().materials[0].color = Press;
				break;
		}
	}
}
