using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTestCamera : MonoBehaviour {
    //테스트용 카메라

    public Transform target;
    public float distance;
    public float height;

	void Start () {
		
	}
	
	void Update () {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + height, target.transform.position.z - distance);
        transform.LookAt(target);
	}
}
