using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CalcPinDist : MonoBehaviour
{
    public List<Transform> pins;
    public Transform near_pin;
    public Transform cam_pos;
    public Transform player;
    public Camera cam;
    public float Speed;
    public PlayerMove pm;
    public Pin p;

	void Awake ()
    {
        pins = new List<Transform>();
        var p = GameObject.FindGameObjectsWithTag("Pin");
        player = FindObjectOfType<Player>().transform;
        pm = player.GetComponent<PlayerMove>();
        cam = Camera.main;
        cam_pos = cam.transform;
        foreach(var i in p)
        {
            pins.Add(i.transform);
        }

	}

	
	void Update ()
    {
        float dis = float.MaxValue;
        foreach( var i in pins)
        {
            float d = Vector3.Distance(i.position , player.position);
            if(d < dis)
            {
                dis = d;
                near_pin = i.transform;
                cam_pos = near_pin.transform.parent;
                p = near_pin.transform.parent.GetComponent<Pin>();

                
            }
        }
        Vector3 lerp = Vector3.Lerp(cam.transform.position , cam_pos.position, Time.deltaTime * Speed);
        cam.transform.position = lerp;
        Quaternion rot = cam.transform.rotation;
        rot = Quaternion.Slerp(rot , Quaternion.LookRotation((near_pin.transform.position - cam.transform.position ).normalized), Time.deltaTime * Speed);
        cam.transform.rotation = rot;
	}
}
