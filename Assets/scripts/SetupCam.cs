using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupCam : MonoBehaviour {

    public CusCamera cam;
    public Vector3 offset;
    public Transform pos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(cam.lookPlayer)
        {
            transform.SetPositionAndRotation(pos.position + offset, Quaternion.Euler(26.017f,0,0));
            
        }
        else
        {
            transform.SetPositionAndRotation(cam.positions[cam.Level].transform.position, cam.positions[cam.Level].transform.rotation);
        }

	}
}
