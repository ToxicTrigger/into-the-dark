﻿using System.Collections;
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
    public AudioSource arrow_hit_rub, arrow_hit_stone,arrow_fire_start,arrow_water_start;
    public bool has_targeting_totem;

    private void OnTriggerEnter(Collider collision)
    {
        bool is_player = false;
        if(collision.gameObject.tag.Equals("Player") || collision.gameObject.layer == 2 || collision.gameObject.layer == 9) is_player = true;

        Look look = collision.gameObject.GetComponent<Look>();
        Element element = collision.gameObject.GetComponent<Element>();

        if (look != null)
        {
            look = collision.gameObject.GetComponent<Look>();
            look.Hp -= 1;
        }
        if (element != null)
        {
            //TODO :: 만약 몬스터가 속성에 대응할 여부가 있을 때
        }

        //TODO :: ObjectPool refactoring here
        if (!is_player)
        {
            AudioSource AS = null;
            if(element != null)
            {
                switch (element.type)
                {
                    case Element.Type.None: AS = arrow_hit_rub; break;
                    case Element.Type.Fire: AS = arrow_hit_stone; break;
                    case Element.Type.Water: AS = arrow_hit_stone; break;
                    default: break;
                }
            }
            else
            {
                AS = arrow_hit_rub;
            }
            Debug.Log(collision.gameObject.name);
            GameObject tmp = Instantiate(AS.gameObject, transform.position, Quaternion.identity, null);
            tmp.GetComponent<AudioSource>().Play();
            Destroy(tmp, 2.0f);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Fog"))
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {       
        AudioSource AS = null;
        if (type.type == Element.Type.Fire)
        {
            AS = arrow_fire_start;
            GameObject tmp = Instantiate(AS.gameObject, transform.position, Quaternion.identity, null);
            tmp.GetComponent<AudioSource>().Play();
            Destroy(tmp, 2.0f);
        }
        
    }

    private void Awake()
    {
        type = GetComponent<Element>();
        rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rig.velocity = look * speed;
    }

    void Update () {
        if (type.type == Element.Type.Fire)tr.startColor = Color.red;

        else if (type.type == Element.Type.Water) tr.startColor = Color.cyan;
        
    }
}
