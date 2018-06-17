using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : InputHandler {
	public Player player;
	public CharacterController cc;
	public float moveSpeed = 0.01f;
	public Vector3 movement;

    float foot_step_tick;

    public override void Work(InputManager im)
    {
        if(!im.has_not_anyting_input() & (player.cur_ani.Equals("Run") || player.cur_ani.Equals("Player_Idle")))
		{
			movement.x = im.get_Horizontal();
			movement.z = im.get_Vertical();	
			player.ani.SetFloat("Forward", movement.normalized.magnitude );
			movement = movement.normalized * moveSpeed;


			Quaternion q = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Slerp(transform.rotation, q, moveSpeed);
            //transform.LookAt(transform.position + movement);

            if (foot_step_tick >= 0.25f)
            {
                foot_step_tick = 0;
                player.Foot_Step.PlayOneShot(player.Foot_Step.clip);
            }
            else
            {
                foot_step_tick += Time.deltaTime;
            }
           

			cc.Move(movement);
		}else if(!im.has_not_anyting_input() & (player.cur_ani.Contains("Stand") || player.cur_ani.Contains("Jump"))){
			movement.x = im.get_Horizontal();
			movement.z = im.get_Vertical();	
			movement = movement.normalized * moveSpeed;

			Quaternion q = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Slerp(transform.rotation, q, moveSpeed);
		}else{
			movement = Vector3.zero;
			player.ani.SetFloat("Forward", movement.z);
		}
    }

    // Use this for initialization
    void Start () {
        foot_step_tick = 0;
        player = GetComponent<Player>();
		cc = GetComponent<CharacterController>();
	}
}
