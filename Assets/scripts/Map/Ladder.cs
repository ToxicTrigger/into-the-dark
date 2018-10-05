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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            player.has_on_ladder = true;
            pm.enabled = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (player.has_on_ladder)
        {
            pm.set_movement_zero();
            float h = Input.GetAxisRaw("Vertical");
            Vector3 up = Vector3.zero;
            up.y = h;
            up *= pm.moveSpeed * 0.5f;
            cc.Move(up);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.layer != LayerMask.NameToLayer("Ground") && collision.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") )
        {
            player.has_on_ladder = false;
            pm.enabled = true;
        }
    }
}
