using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool is_attack;
    public Animator ani;
    public Vector3 click_pos;
    public Weapon weapon;
    public LineRenderer line;

    public Element.Type cur_attack_type;
    public UnityEngine.Camera cam;

    public float bow_time;

    public void Start()
    {
        weapon = GetComponent<Weapon>();
    }
    
    void Update () {
        Vector3 tmp = transform.position;
        tmp.y = 10f;

        if (Input.GetButton("Fire1"))
        {
            ani.SetBool("Attack", true);
            weapon.type = Weapon.Type.Sword;
        }
        else if (Input.GetButton("Fire2"))
        {
            bow_time += Time.deltaTime;
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

                
                weapon.type = Weapon.Type.Bow;
            }
        }
        ani.SetFloat("Bow_Fire", bow_time);
    }

    private void FixedUpdate()
    {
        if (!is_attack)
        {
            ani.SetBool("Attack", false);
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (weapon.isUsing)
            {
                Vector3 nor = transform.forward.normalized;
                GameObject arrow = GameObject.Instantiate(weapon.arrow.gameObject, weapon.fire_point.position, Quaternion.LookRotation(click_pos), null);
                arrow.GetComponent<Arrow>().look = weapon.fire_point.forward;
                arrow.transform.LookAt(weapon.fire_point.forward);
                Destroy(arrow, 5.0f);

                bow_time = 0f;
                weapon.type = Weapon.Type.Idle;
                line.gameObject.SetActive(false);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (weapon.isUsing)
            {
                weapon.type = Weapon.Type.Idle;
            }
        }
    }
}
