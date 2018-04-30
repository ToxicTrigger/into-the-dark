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
    public Element type;
    public TrailRenderer tr;

    private void OnTriggerEnter(Collider collision)
    {
        bool is_player = false;
        if(collision.gameObject.tag.Equals("Player") || collision.gameObject.layer == 2)
        {
            is_player = true;
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
        
        if (!is_player)
        {
            Destroy(gameObject);
        }
        else
        {
        }
    }

    private void Awake()
    {
        type = GetComponent<Element>();
        rig = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        //rig.AddForce((look * speed), ForceMode.Acceleration);
        rig.velocity = look * speed;
        transform.LookAt(transform.forward);
    }

    void Update () {
        if (type.type == Element.Type.Fire)
        {
            tr.startColor = Color.red;
        }
        else if (type.type == Element.Type.Water)
        {
            tr.startColor = Color.cyan;
        }
    }
}
