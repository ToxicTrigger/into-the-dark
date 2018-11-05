using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpHeal : Item
{
    [SerializeField]
    private float heal_point = 20;

    public override void do_work()
    {
        
        Player.GetComponent<Damageable>().Hp += heal_point;
        Destroy(gameObject);
    }
}
