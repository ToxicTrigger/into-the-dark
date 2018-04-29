using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    public float power = 1;
    public float speed =1 ;
    public int how_penetrate;
    public int max_penetrate;
    public Vector3 look;
    public Rigidbody rig;
    

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            return;
        }
        Look look = collision.gameObject.GetComponent<Look>();
        Element element = collision.gameObject.GetComponent<Element>();
        if (look != null)
        {
            look = collision.gameObject.GetComponent<Look>();
            look.Hp -= 1;
            
        }
        if (element != null)
        {

        }

        Destroy(gameObject);
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        //rig.AddForce((look * speed), ForceMode.Acceleration);
        rig.velocity = look * speed;
        transform.LookAt(transform.forward);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
