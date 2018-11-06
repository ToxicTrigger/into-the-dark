using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HpPowerUp : Item
{
    public float hp_grow_point;
    public bool end;

    public override void do_work()
    {
        if(!end)
        {
            if (Player.GetComponent<Damageable>().Max_Hp < 201)
            {
                this.Player.GetComponent<Damageable>().Max_Hp += 100;
                Debug.Log(Player.GetComponent<Player>().damageable.Max_Hp + " Hp");
                end = true;
                Destroy(gameObject);
            }

            Destroy(gameObject);
        }

    }
}
