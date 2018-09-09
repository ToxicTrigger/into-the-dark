using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
    public float detective_range;
    public Transform Player;
    public bool detected;
    public float move_speed;
    
	void Start () {
        Player = FindObjectOfType<Player>().transform;
    }

    void move()
    {
        if(detected)
        {
            Vector3 pos = transform.position;
            pos.y += Mathf.Abs(Mathf.Sin(Time.time) * 0.1f);
            transform.position = Vector3.Lerp(pos, Player.position, Time.time * 0.005f);
        }
    }
	
	void Update () {
		if(Vector3.Distance(transform.position, Player.position) <= detective_range)
        {
            detected = true;
        }else
        {
            detected = false;
        }

        move();
	}

    public abstract void do_work();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            do_work();
        }
    }
}
