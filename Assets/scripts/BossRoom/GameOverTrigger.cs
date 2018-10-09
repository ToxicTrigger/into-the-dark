using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour {
    
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BossRoomManager.get_instance().game_over();
        }
    }

}
