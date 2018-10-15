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

    public Vector3 movement;

    float foot_step_tick;
    bool has_ground;
    bool is_falling_out;

    [SerializeField]
    private bool check_falling;
    float fall_tick;
    public Transform spawn_point;
    bool dash_start;

    float step;
    public Material stamina;

    public CalcPinDist cpd;

    public override void Work(InputManager im)
    {
        if( !im.has_not_anything_input() && ( player.cur_ani.Equals("Run") || player.cur_ani.Equals("Player_Idle") ) )
        {
            if(cpd != null)
            {
                switch( cpd.p.direction )
                {
                    case Pin.Direction.Forward:
                        movement.x = Mathf.Lerp(movement.x , im.get_Horizontal() , moveSpeed);
                        movement.z = Mathf.Lerp(movement.z , im.get_Vertical() , moveSpeed);
                        break;
                    case Pin.Direction.Back:
                        movement.x = Mathf.Lerp(movement.x , im.get_Horizontal() * -1 , moveSpeed);
                        movement.z = Mathf.Lerp(movement.z , im.get_Vertical() * -1 , moveSpeed);
                        break;
                    case Pin.Direction.Left:
                        movement.x = Mathf.Lerp(movement.x , im.get_Vertical() * -1 , moveSpeed);
                        movement.z = Mathf.Lerp(movement.z , im.get_Horizontal() , moveSpeed);
                        break;
                    case Pin.Direction.Right:
                        movement.x = Mathf.Lerp(movement.x , im.get_Vertical() , moveSpeed);
                        movement.z = Mathf.Lerp(movement.z , im.get_Horizontal() * -1 , moveSpeed);
                        break;
                }
            }
            else
            {
                movement.x = Mathf.Lerp(movement.x , im.get_Horizontal() , moveSpeed);
                movement.z = Mathf.Lerp(movement.z , im.get_Vertical() , moveSpeed);
            }


            /*
            if( cx >= 0 && sy >= 0 )
            {
                movement.x = Mathf.Lerp(movement.x , im.get_Horizontal() , moveSpeed);
                movement.z = Mathf.Lerp(movement.z , im.get_Vertical() , moveSpeed);
            }
            else if( cx < 0 && sy >= 0 )
            {
                movement.x = Mathf.Lerp(movement.x , im.get_Vertical() , moveSpeed);
                movement.z = Mathf.Lerp(movement.z , im.get_Horizontal() * -1 , moveSpeed);
            }
            else if( cx < 0 && sy < 0 )
            {
                movement.x = Mathf.Lerp(movement.x , im.get_Horizontal() * -1 , moveSpeed);
                movement.z = Mathf.Lerp(movement.z , im.get_Vertical() * -1 , moveSpeed);
            }
            else if( cx >= 0 && sy < 0 )
            {
                movement.x = Mathf.Lerp(movement.x , im.get_Vertical() * -1 , moveSpeed);
                movement.z = Mathf.Lerp(movement.z , im.get_Horizontal() , moveSpeed);
            }
            */

            player.ani.SetFloat("Forward" , movement.magnitude);
            Vector3 t = movement * 3;
            t.y = 0;
            Quaternion q = Quaternion.LookRotation(t);
            transform.rotation = Quaternion.Slerp(transform.rotation , q , moveSpeed * 1.5f);


            if( Input.GetButton("Dash") && stamina.GetFloat("_Amount") < 0 )
            {

                if( im.get_Horizontal() != 0 && im.get_Horizontal() > 0 )
                {
                    movement = Vector3.Lerp(movement , player.ac.cam.transform.right.normalized * moveSpeed * 5f , moveSpeed);
                }
                else if( im.get_Horizontal() != 0 && im.get_Horizontal() < 0 )
                {
                    movement = Vector3.Lerp(movement , -player.ac.cam.transform.right.normalized * moveSpeed * 5f , moveSpeed);
                }

                if( im.get_Vertical() != 0 && im.get_Vertical() > 0 )
                {
                    movement = Vector3.Lerp(movement , player.ac.cam.transform.forward.normalized * moveSpeed * 5f , moveSpeed);

                }
                else if( im.get_Vertical() != 0 && im.get_Vertical() < 0 )
                {
                    movement = Vector3.Lerp(movement , -player.ac.cam.transform.forward.normalized * moveSpeed * 5f , moveSpeed);
                }
                stamina.SetFloat("_Amount" , stamina.GetFloat("_Amount") + 0.005f);

            }
            else
            {

                if( im.get_Horizontal() != 0 && im.get_Horizontal() > 0 )
                {
                    movement = Vector3.Lerp(movement , player.ac.cam.transform.right.normalized * moveSpeed , moveSpeed);
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

            if( Input.GetButton("Dash") )
            {
                if( foot_step_tick >= 0.18f )
                {
                    foot_step_tick = 0;
                    player.Foot_Step.PlayOneShot(player.Foot_Step.clip);
                }
                else
                {
                    foot_step_tick += Time.deltaTime;
                }
            }
            else
            {
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
        }

        else
        {
            movement = Vector3.Lerp(movement , Vector3.zero , moveSpeed);
            player.ani.SetFloat("Forward" , movement.magnitude);
        }
        cc.Move(movement * 0.2f);
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
        if( stamina.GetFloat("_Amount") < -1 )
        {
            stamina.SetFloat("_Amount" , -1);
        }
        else if( stamina.GetFloat("_Amount") != -1 )
        {
            stamina.SetFloat("_Amount" , stamina.GetFloat("_Amount") - 0.003f);
        }
    }

    public void Start()
    {
        foot_step_tick = 0;
        player = GetComponent<Player>();
        cc = GetComponent<CharacterController>();
        cpd = FindObjectOfType<CalcPinDist>();
        step = cc.stepOffset;
    }
}
