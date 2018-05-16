using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour {
    bool is_attack;
    public Animator ani;
    Vector3 click_pos;
    public Weapon weapon;
    public LineRenderer line;
    ThirdPersonCharacter tpc;
    float origin_move_speed;

    public Element cur_attack_type;
    public Camera cam;
    public GameObject fire_effect, ice_effect;
    public AudioSource bow_fullback, bow_release;
    public TrailRenderer red, blue;
    public Queue<GameObject> totems;
    public int cur_totems, installable_totems;
    public GameObject totem;

    public float bow_time;

    public void Start()
    {
        weapon = GetComponent<Weapon>();
        cur_attack_type = GetComponent<Element>();
        tpc = GetComponent<ThirdPersonCharacter>();
        origin_move_speed = tpc.m_MoveSpeedMultiplier;
        totems = new Queue<GameObject>(0);
    }

    void gen_arrow()
    {
        RaycastHit hit;
        Vector3 mouse = Input.mousePosition;
        if (Physics.Raycast(cam.ScreenPointToRay(mouse), out hit, 10000))
        {

        }

        Vector3 nor = transform.forward.normalized;
        GameObject arrow = GameObject.Instantiate(weapon.arrow.gameObject, weapon.fire_point.position, Quaternion.LookRotation(click_pos), null);
        arrow.GetComponent<Arrow>().look = weapon.fire_point.forward;
        //arrow.transform.LookAt(weapon.fire_point.forward);


        if (bow_time >= 3.2f & bow_time < 4f)
        {
            arrow.GetComponent<Arrow>().type.type = cur_attack_type.type;
            GameObject tmp =
            cur_attack_type.type == Element.Type.Fire ?
                Instantiate(fire_effect, transform.position, Quaternion.identity, null) :
                Instantiate(ice_effect, transform.position, Quaternion.identity, null);
            Destroy(tmp, 2.0f);
        }
        Destroy(arrow, 5.0f);
    }
    void swap_element_type()
    {
        cur_attack_type.type = cur_attack_type.type == Element.Type.Fire ? Element.Type.Water : Element.Type.Fire;
        GameObject tmp =
            cur_attack_type.type == Element.Type.Fire ?
            Instantiate(fire_effect, transform.position, Quaternion.identity, null) :
            Instantiate(ice_effect, transform.position, Quaternion.identity, null);
        Destroy(tmp, 2.0f);
        t = cur_attack_type.type;
    }
    void calc_click_pos()
    {
        RaycastHit hit;
        Vector3 mouse = Input.mousePosition;
        if (Physics.Raycast(cam.ScreenPointToRay(mouse), out hit, 10000))
        {
            click_pos = hit.point;
            click_pos.y = transform.position.y;
            transform.LookAt(click_pos);
            line.gameObject.SetActive(true);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, click_pos);

        }
    }

    public Element.Type t;
    public void editElementTypeInAni(int input)
    {
        switch (input)
        {
            // None
            case 0: cur_attack_type.type = Element.Type.None; break;
            case 1: cur_attack_type.type = t; break;
        }

    }

    void gen_totem()
    {
        cur_totems = totems.Count;
        installable_totems = 3 - cur_totems;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (totems.Count > 2)
            {
                Destroy(totems.Dequeue());
            }

            GameObject t = Instantiate(totem, transform.position, Quaternion.identity, null);
            totems.Enqueue(t);
        }
    }

    public void FixedUpdate()
    {
        if (!is_attack)
        {
            ani.SetBool("Attack", false);
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (weapon.isUsing)
            {
                if(bow_time >= 1.0f)
                {
                    gen_arrow();

                    bow_time = 0f;
                    weapon.type = Weapon.Type.Idle;
                    line.gameObject.SetActive(false);
                    tpc.m_MoveSpeedMultiplier = origin_move_speed;
                    if (!bow_release.isPlaying)
                    {
                        bow_fullback.Stop();
                        bow_release.Play();
                    }
                }
                else
                {
                    bow_time = 0f;
                    weapon.type = Weapon.Type.Idle;
                    line.gameObject.SetActive(false);
                    tpc.m_MoveSpeedMultiplier = origin_move_speed;
                    if (!bow_release.isPlaying)
                    {
                        bow_fullback.Stop();
                        //bow_release.Play();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
                //TODO :: melee attack here
            weapon.type = Weapon.Type.Idle;
            red.time = 0;
            blue.time = 0;

        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            swap_element_type();
        }
    }

    void Update () {
        Vector3 tmp = transform.position;
        tmp.y = 10f;
        gen_totem();

        if (Input.GetButton("Fire1"))
        {
            ani.SetBool("Attack", true);
            weapon.type = Weapon.Type.Sword;
            if(cur_attack_type.type == Element.Type.Fire)
            {
                red.time = 0.63f;
            }else if(cur_attack_type.type == Element.Type.Water)
            {
                blue.time = 0.63f;
            }
            
            calc_click_pos();
        }
        else if (Input.GetButton("Fire2"))
        {
            weapon.type = Weapon.Type.Bow;
            bow_time += Time.deltaTime;
            if(!bow_fullback.isPlaying)
            {
                bow_fullback.Play();
            }
            tpc.m_MoveSpeedMultiplier = 0;
            calc_click_pos();
        }
        ani.SetFloat("Bow_Fire", bow_time);
    }
}
