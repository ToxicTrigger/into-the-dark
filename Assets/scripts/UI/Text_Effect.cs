using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Text_Effect : MonoBehaviour {
	public string text;
	public string sub_text;
	public float time = 0.2f;
	public bool reset;
	public TextMesh textMesh;
	public bool EventDone;

	private void Start() {
		textMesh = GetComponent<TextMesh>();	
		text = textMesh.text;
	}
	
	IEnumerator start() 
	{
		if(!EventDone)
		{
			StringBuilder sb = new StringBuilder(sub_text);
			char[] tt = text.ToCharArray();
			//System.CharEnumerator iter = text.GetEnumerator();
			foreach(char c in tt)
			{
				Debug.Log(c);
				sb.Append(c);
				sub_text = sb.ToString();
				yield return new WaitForSeconds(time);
			}
			sub_text = text;
			EventDone = true;
		}
	}

	public void StartEffect()
	{
		StartCoroutine(start());
	}

	void Update () {
		if( reset & sub_text.Equals(text))
		{
			sub_text = "";
			reset = false;
			EventDone = false;
		}
		textMesh.text = sub_text;
	}

	private void OnDisable() {
			sub_text = "";
			reset = false;
			EventDone = false;
	}
}
