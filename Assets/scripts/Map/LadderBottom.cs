using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBottom : MonoBehaviour {
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
        if( collision.gameObject.CompareTag("Player") && 
            collision.gameObject.layer != LayerMask.NameToLayer("Ground") && 
            collision.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") 
            )
        {
            var pos = player.transform.position;
            player.transform.position = pos;
            player.has_on_ladder = false;
            player.ani.SetBool("Ladder" , false);
            pm.enabled = true;
        }
    }

}
