using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : InputHandler {
	public Player player;
	public CharacterController cc;
	public float moveSpeed = 0.01f;
	public Vector3 movement;
    float foot_step_tick;
	public bool has_ground;
	public bool is_falling_out;
    public bool check_falling;
	public float fall_tick;
	public Transform spawn_point;

	public float dash_timer;
	public bool is_dash, dash_start;
	
    public override void Work(InputManager im)
    {
		if(!im.has_not_anything_input() & (player.cur_ani.Equals("Run") || player.cur_ani.Equals("Player_Idle")))
        {
			movement.x = im.get_Horizontal();
			movement.z = im.get_Vertical();	
			player.ani.SetFloat("Forward", movement.normalized.magnitude );
			movement = movement.normalized * moveSpeed;

			Quaternion q = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Slerp(transform.rotation, q, moveSpeed);

			if(Input.GetButton("Dash"))
			{
				cc.Move(movement * 1.5f);
                player.ActionCam.set_state(PlayerCamera.State.Dash);
			}else{
				cc.Move(movement);
                player.ActionCam.set_state(PlayerCamera.State.Follow);
			}

			if (foot_step_tick >= 0.25f)
            {
                foot_step_tick = 0;
                player.Foot_Step.PlayOneShot(player.Foot_Step.clip);
            }
            else
            {
                foot_step_tick += Time.deltaTime;
            }
		}else if(!im.has_not_anything_input() & (player.cur_ani.Contains("Stand") || player.cur_ani.Contains("Jump"))){
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
	
	void update_fall()
	{	
		if(!has_ground)
		{
			//추락 확정
			if(fall_tick >= 1.5f)
			{
				fall_tick = 0;
				is_falling_out = true;
			}else{
				fall_tick += Time.deltaTime;
			}
		}else{
			fall_tick = 0;
		}
	}

    void update_ray()
    {
        Ray down = new Ray(transform.position, transform.forward);
    }

	void update_move_player_checkPoint()
	{
		if(is_falling_out)
		{
			transform.position = spawn_point.position;
			is_falling_out = false;
		}
	}

	private void FixedUpdate() 
	{
		has_ground = cc.isGrounded;
        if(check_falling)
        {
            update_fall();
            update_move_player_checkPoint();
        }

	}
    
    void Start () 
	{
        foot_step_tick = 0;
        player = GetComponent<Player>();
		cc = GetComponent<CharacterController>();
	}
}
