using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Player player;
    PlayerMove pm;
    CharacterController cc;
    float ani_speed;
    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        pm = player.GetComponent<PlayerMove>();
        cc = player.GetComponent<CharacterController>();
        
    }

    public void FixedUpdate()
    {
        if( player.has_on_ladder )
        {
            player.transform.rotation = Quaternion.LookRotation(transform.up);
            pm.set_movement_zero();
            float h = Input.GetAxisRaw("Vertical");
            if( h != 0 )
            {
                player.ani.speed = 1;
                Vector3 up = Vector3.zero;
                up.y = h;
                up *= pm.moveSpeed * 0.35f;
                cc.Move(up);
            }
            else
            {
                player.ani.speed = 0;
            }
        }
    }

    

    private void OnTriggerEnter(Collider collision)
    {
        if( collision.gameObject.CompareTag("Player") && collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") )
        {
            var pos = player.transform.position;
            //pos.y -= 1f;
            player.transform.position = pos;
            player.has_on_ladder = true;
            player.ani.SetBool("Ladder" , true);
            pm.enabled = false;
        }
    }

}
