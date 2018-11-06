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
    float dash_tick;
    Color ui_color;
    Color ui_def;

    float step;
    public Material stamina;
    public float stamina_amount;

    public CalcPinDist cpd;
    public int cus_x = 1, cus_z = 1;
    public bool reverse;

    public float stamina_tick;
    public bool stamina_zero;

    public ParticleSystem run_effect;

    public override void Work(InputManager im)
    {
        if( !im.has_not_anything_input() && ( player.cur_ani.Equals("Run") || player.cur_ani.Equals("Player_Idle") ) )
        {
            
            if(cpd != null && cpd.enabled)
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
                if(!reverse)
                {
                    movement.x = Mathf.Lerp(movement.x, im.get_Horizontal() * cus_x, moveSpeed);
                    movement.z = Mathf.Lerp(movement.z, im.get_Vertical() * cus_z, moveSpeed);
                }
                else
                {
                    movement.x = Mathf.Lerp(movement.x, im.get_Vertical() * cus_z, moveSpeed);
                    movement.z = Mathf.Lerp(movement.z, im.get_Horizontal() * cus_x, moveSpeed);
                }

            }

            player.ani.SetFloat("Forward" , movement.magnitude);
            Vector3 t = movement * 3;
            t.y = 0;
            Quaternion q = Quaternion.LookRotation(t);
            transform.rotation = Quaternion.Slerp(transform.rotation , q , moveSpeed * 1.5f);

            if( Input.GetButton("Dash") && !stamina_zero )
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

                if(stamina_amount >= 0 )
                {
                    stamina_zero = true;
                    return;
                }
                else
                {
                    stamina.SetFloat("_Amount", stamina.GetFloat("_Amount") + 0.005f);
                    dash_tick = 5;
                }

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

    void update_stamina_ui()
    {
        if(dash_tick > 0)
        {
            dash_tick -= Time.deltaTime;
            ui_color.a = 1;
        }
        else
        {
            dash_tick = 0;
            ui_color.a = 0;
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
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            run_effect.Play();
        }
        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            run_effect.Stop();
        }

        ui_def = stamina.GetColor("_Color");
        update_stamina_ui();
        float a = Mathf.Lerp(ui_def.a, ui_color.a, Time.deltaTime);
        ui_def.a = a;

        stamina.SetColor("_Color", ui_def);

        stamina_amount = stamina.GetFloat("_Amount");
        has_ground = cc.isGrounded;

        if(stamina_zero)
        {
            if(stamina_tick > 3)
            {
                stamina_zero = false;
                stamina_tick = 0;
            }
            else
            {
                stamina_tick += Time.deltaTime;
            }
        }

        if( check_falling )
        {
            update_fall();
            update_move_player_checkPoint();
        }
        if( stamina_amount < -1 )
        {
            stamina.SetFloat("_Amount" , -1);
        }
        else if(stamina_amount != -1 )
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
        ui_color = Color.white;
        run_effect = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<ParticleSystem>();
    }

    public void OnApplicationQuit()
    {
        stamina.SetColor("_Color", new Color(1, 1, 1, 0));
        stamina.SetFloat("_Amount", -1);
    }
}
