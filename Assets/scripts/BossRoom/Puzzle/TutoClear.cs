using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoClear : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Equals("Player"))
        {
            BossRoomManager.get_instance().tuto_clear = true;
            Destroy(this.gameObject);
        }
    }
}
