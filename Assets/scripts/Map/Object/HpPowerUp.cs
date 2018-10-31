using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HpPowerUp : Item
{
    public float hp_grow_point;

    public override void do_work()
    {
        this.Player.GetComponent<Damageable>().Max_Hp += hp_grow_point;
        Destroy(gameObject);
    }
}
