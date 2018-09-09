using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpHeal : Item
{
    public float heal_point;
    public override void do_work()
    {
        Player.GetComponent<Damageable>().Hp += heal_point;
        Destroy(gameObject);
    }
}
