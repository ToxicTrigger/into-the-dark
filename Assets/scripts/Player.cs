using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public bool is_attack;
    public Animator ani;
    public Vector3 click_pos;

    private void FixedUpdate()
    {
        if(!is_attack)
        {
            ani.SetBool("Attack", false);
        }
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetBool("Attack", true);
        }
	}
}
