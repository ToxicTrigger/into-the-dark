using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendCollisionMessage : MonoBehaviour {

    BossRoomManager manager;
    Boss_Worm boss;
    Boss_State boss_state;

    public enum Field
    {
        NULL,
        A,
        B
    }
    public Field field;

    void Start()
    {        
        manager = BossRoomManager.get_instance();
        boss = manager.get_boss();
        boss_state = boss.GetComponent<Boss_State>();
    }
    

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Boss") && boss_state.get_state() == Boss_State.State.Idle)
        {
                manager.set_field_info(field);

            if (field == Field.A)
            {
                if(boss_state.get_state() == Boss_State.State.Idle)
                    boss_state.set_state(Boss_State.State.Move);
            }
            //else if(field == Field.B)
            //{
            //    if(boss_state.get_state() == Boss_State.State.Move)
            //    boss_state.set_state(Boss_State.State.Rush_Attack);
            //}
        }
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Boss"))
        //{
        //    if(field == Field.A)
        //    {
        //        manager.set_field_info(Field.NULL);
        //    }
        //    else if(field == Field.B)
        //    {
        //        manager.set_field_info(Field.A);
        //    }
        //}
    }

    public Field get_field_info()
    {
        return field; 
    }

}
