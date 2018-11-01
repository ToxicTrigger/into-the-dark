using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedTrap : MonoBehaviour {
    

	void Start () {
		red = new Vector4(1, 0, 0, 1);
        active = false;
    }

    public bool active;
    public Material water_color;
    Color red;
    public ActivatedTrap [] trap_list;

    void OnTriggerEnter(Collider other)
    {
        active = true;
        for (int i = 0; i < trap_list.Length; i++)
        {
            if (trap_list[i].active == true)
            {
                return;
            }
        }

        if (other.CompareTag("Player"))
        {
            water_color.color = new Vector4(1, 0, 0, 1);
            if (BossRoomManager.get_instance().get_boss_state() == Boss_State.State.Idle)
            {
                //BossRoomManager.get_instance().set_field_info(SendCollisionMessage.Field.B);
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            for(int i =0; i<trap_list.Length; i++)
            {
                if (trap_list[i].active == true)
                {
                    return;
                }               
            }            
            water_color.color = new Vector4(0, 0, 1, 1);
        }
        active = false;
    }

}
