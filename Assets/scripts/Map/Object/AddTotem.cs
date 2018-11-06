using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddTotem : Item
{
    bool end;
    public override void do_work()
    {
        if(!end)
        {
            if (Player.GetComponent<Player>().installable_totems < 5)
            {
                Player.GetComponent<Player>().installable_totems += 1;
                end = true;
                Debug.Log(Player.GetComponent<Player>().installable_totems + " totem");
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
