using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Player player;
    PlayerMove pm;
    CharacterController cc;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        pm = player.GetComponent<PlayerMove>();
        cc = player.GetComponent<CharacterController>();
    }

    private void OnTriggerExit(Collider collision)
    {
        if( collision.gameObject.CompareTag("Player") && collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") )
        {
            var pos = player.transform.position;
            pos += transform.up * 1.3f ;
            pos.y += 0.2f;
            player.transform.position = pos;
            player.has_on_ladder = false;
            player.ani.SetBool("Ladder" , false);
            pm.enabled = true;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (player.has_on_ladder)
        {
            player.transform.rotation = Quaternion.LookRotation(transform.up);
            pm.set_movement_zero();
            float h = Input.GetAxisRaw("Vertical");
            if( h != 0)
            {
                Vector3 up = Vector3.zero;
                up.y = h;
                up *= pm.moveSpeed * 0.5f;
                cc.Move(up);
            }
            else
            {

            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if( collision.gameObject.CompareTag("Player") && collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") )
        {
            var pos = player.transform.position;
            pos.y -= 1f;
            player.transform.position = pos;
            player.has_on_ladder = true;
            player.ani.SetBool("Ladder" , true);
            pm.enabled = false;
        }
    }

}
