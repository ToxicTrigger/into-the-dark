using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : InputHandler
{
    public Player player;
    public CharacterController cc;

    [SerializeField]
    private float _moveSpeed = 0.12f;
    public float moveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    [SerializeField]
    private Vector3 movement;

    float foot_step_tick;
    bool has_ground;
    bool is_falling_out;

    [SerializeField]
    private bool check_falling;
    float fall_tick;
    public Transform spawn_point;
    bool dash_start;
    public float y;

    public override void Work(InputManager im)
    {
        if( !im.has_not_anything_input() & ( player.cur_ani.Equals("Run") || player.cur_ani.Equals("Player_Idle") ) )
        {
            movement.x = Mathf.Lerp(movement.x , im.get_Horizontal(), moveSpeed);
            movement.z = Mathf.Lerp(movement.z , im.get_Vertical(), moveSpeed);
            player.ani.SetFloat("Forward" , movement.magnitude);

            y = Mathf.Sin(player.ac.cam.transform.eulerAngles.y);
            
            if( y == 0 )
            {
                //movement = movement.normalized;
            }
            else if( y > 0 )
            {
                float xz = movement.x * -1;
                float zx = movement.z;

                movement.x = zx;
                movement.z = xz;
                //movement = movement.normalized;
            }
            else if( y < 0 )
            {
                float xz = movement.x;
                float zx = movement.z * -1;

                movement.x = zx;
                movement.z = xz;
                //movement = movement.normalized;
            }

            Quaternion q = Quaternion.LookRotation(movement * 3);
            transform.rotation = Quaternion.Slerp(transform.rotation , q , moveSpeed * 1.5f);

            if( Input.GetButton("Dash") )
            {
                if( im.get_Horizontal() != 0 && im.get_Horizontal() > 0 )
                {
                    movement = Vector3.Lerp(movement, player.ac.cam.transform.right.normalized * moveSpeed * 5f, moveSpeed);
                }
                else if( im.get_Horizontal() != 0 && im.get_Horizontal() < 0 )
                {
                    movement = Vector3.Lerp(movement , -player.ac.cam.transform.right.normalized * moveSpeed * 5f , moveSpeed);
                }

                if( im.get_Vertical() != 0 && im.get_Vertical() > 0 )
                {
                    movement = Vector3.Lerp(movement , player.ac.cam.transform.forward.normalized * moveSpeed *5f , moveSpeed);
                    
                }
                else if( im.get_Vertical() != 0 && im.get_Vertical() < 0 )
                {
                    movement = Vector3.Lerp(movement , -player.ac.cam.transform.forward.normalized * moveSpeed * 5f , moveSpeed);

                }
            }
            else
            {
                if( im.get_Horizontal() != 0 && im.get_Horizontal() > 0 )
                {
                    movement = Vector3.Lerp(movement , player.ac.cam.transform.right.normalized * moveSpeed, moveSpeed);
                }
                else if( im.get_Horizontal() != 0 && im.get_Horizontal() < 0 )
                {
                    movement = Vector3.Lerp(movement , -player.ac.cam.transform.right.normalized * moveSpeed , moveSpeed);
                }

                if( im.get_Vertical() != 0 && im.get_Vertical() > 0 )
                {
                    movement = Vector3.Lerp(movement , player.ac.cam.transform.forward.normalized * moveSpeed , moveSpeed);
                }
                else if( im.get_Vertical() != 0 && im.get_Vertical() < 0 )
                {
                    movement = Vector3.Lerp(movement , -player.ac.cam.transform.forward.normalized * moveSpeed , moveSpeed);
                }
            }


            if( foot_step_tick >= 0.25f )
            {
                foot_step_tick = 0;
                player.Foot_Step.PlayOneShot(player.Foot_Step.clip);
            }
            else
            {
                foot_step_tick += Time.deltaTime;
            }
        }
        else if( !im.has_not_anything_input() & ( player.cur_ani.Contains("Stand") || player.cur_ani.Contains("Jump") ) )
        {
            //movement.x = im.get_Horizontal();
            //movement.z = im.get_Vertical();
            //movement = movement.normalized * moveSpeed;

            Quaternion q = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation , q , moveSpeed);
        }
        else
        {
            movement = Vector3.Lerp(movement , Vector3.zero , moveSpeed);
            player.ani.SetFloat("Forward" , movement.magnitude);
        }
        cc.Move(movement * 0.35f);
    }

    public void set_movement_zero()
    {
        movement = Vector3.zero;
    }

    void update_fall()
    {
        if( !has_ground )
        {
            //추락 확정
            if( fall_tick >= 1.5f )
            {
                fall_tick = 0;
                is_falling_out = true;
            }
            else
            {
                fall_tick += Time.deltaTime;
            }
        }
        else
        {
            fall_tick = 0;
        }
    }


    void update_move_player_checkPoint()
    {
        if( is_falling_out )
        {
            transform.position = spawn_point.position;
            is_falling_out = false;
        }
    }

    public void FixedUpdate()
    {
        has_ground = cc.isGrounded;
        if( check_falling )
        {
            update_fall();
            update_move_player_checkPoint();
        }
    }

    public void Start()
    {
        foot_step_tick = 0;
        player = GetComponent<Player>();
        cc = GetComponent<CharacterController>();
    }
}
