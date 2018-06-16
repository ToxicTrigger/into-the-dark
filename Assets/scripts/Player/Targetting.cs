using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting : MonoBehaviour {
	public LineRenderer line;
	public float t, up_scale, delta;

	// Use this for initialization
	void Start () {
		line = transform.parent.GetComponent<LineRenderer>();;
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime * up_scale;
		Vector3 pos = line.GetPosition(1);
		pos.y += 2.0f + Mathf.Sin(t) * delta;
		transform.position = pos;
	}
}
