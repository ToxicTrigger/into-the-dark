using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
    public float Hp;
    public bool has_hit;
    //public GameObject Hit_Effect;

    IEnumerator attack_this(float damage, float tick)
    {
        Hp -= damage;

        has_hit = true;
        yield return new WaitForSeconds(tick);
        has_hit = false;
    }

    public void Damaged(float dam, float tick)
    {
        StartCoroutine(attack_this(dam, tick));
    }
}
