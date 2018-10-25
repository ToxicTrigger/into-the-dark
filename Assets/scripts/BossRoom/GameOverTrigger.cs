using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour {

    public Transform re_start_position;

    public void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Player"))
        {
            if (re_start_position == null)
                BossRoomManager.get_instance().game_over();
            else
                BossRoomManager.get_instance().game_over(re_start_position.position);
            Debug.Log("trigger : " + this.name);
        }
    }

}
