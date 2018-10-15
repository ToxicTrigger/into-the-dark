using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTop : MonoBehaviour {
    public Player player;
    PlayerMove pm;
    CharacterController cc;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        pm = player.GetComponent<PlayerMove>();
        cc = player.GetComponent<CharacterController>();
    }



}
