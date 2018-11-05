using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddTotem : Item
{
    public override void do_work()
    {
        if( Player.GetComponent<Player>().installable_totems < 5)
        {
            Player.GetComponent<Player>().installable_totems += 1;
            Destroy(gameObject);
        }
        

    }
}
