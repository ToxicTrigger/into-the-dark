using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpHeal : Item
{
    [SerializeField]
    private float heal_point = 20;

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.4f);
        Player.GetComponent<Damageable>().Hp += heal_point;
        Destroy(gameObject);
    }

    public override void do_work()
    {
        StartCoroutine(wait());

    }
}
