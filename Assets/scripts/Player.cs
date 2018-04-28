using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool is_attack;
    public Animator ani;
    public Vector3 click_pos;
    public Element.Type cur_attack_type;
    public UnityEngine.Camera cam;

    public float bow_time;

    private void Start()
    {
        //cam = UnityEngine.Camera.main;
    }

    private void FixedUpdate()
    {
        if(!is_attack)
        {
            ani.SetBool("Attack", false);
        }
    }

    // Update is called once per frame
    void Update () {
        Vector3 tmp = transform.position;
        tmp.y = 10f;
        //cam.transform.position = tmp;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetBool("Attack", true);
        }
        if (Input.GetButton("Fire2"))
        {
            Debug.Log("Fire");
            bow_time += Time.deltaTime;
            RaycastHit hit;
            Vector3 mouse = Input.mousePosition;
            mouse.y = transform.position.y;
            if(Physics.Raycast(cam.ScreenPointToRay(mouse), out hit, 100))
            {
                click_pos = hit.point;
                click_pos.y = transform.position.y;
                transform.LookAt(click_pos);
                Debug.DrawRay(cam.transform.position, hit.point);
            }
            
        }
        if (Input.GetButtonUp("Fire2"))
        {
            bow_time = 0f;
        }

        ani.SetFloat("Bow_Fire", bow_time);
    }
}
