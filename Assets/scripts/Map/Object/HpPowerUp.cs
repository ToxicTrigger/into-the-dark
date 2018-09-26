using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HpPowerUp : Item
{
    public float hp_grow_point;

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.5f);
        this.Player.GetComponent<Damageable>().Max_Hp += hp_grow_point;
        Destroy(gameObject);
    }

    public override void do_work()
    {
        StartCoroutine(wait());
    }
}
