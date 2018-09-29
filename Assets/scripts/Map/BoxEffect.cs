using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEffect : MonoBehaviour {
    public Mesh mesh;
    public GameObject box;

	// Use this for initialization
	void Start ()
    {
	    foreach(var i in mesh.vertices)
        {
            Instantiate(box, i + transform.position, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
