using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
    public float Hp;
    public bool has_hit;
    public bool Dead;
    Transform origin;

    //실질적으로 모든 데미지 연산은 해당 변수를 거쳐가는데
    //간단히 AttackDamage * ArmorPower 의 연산임.
    //중요한 것은 해당 방어 수치나 공격력이 소수로 보이면 플레이 입장에서 흥미가 생기지 않기 때문에
    //표시할 땐 무조건 result * 10 하기를 권장함
    public float armor_power = 1.0f;

    public void Start() 
    {
        origin = this.transform;
    }

    public void OnCollisionEnter(Collision other) 
    {
        Attackable attack = other.gameObject.GetComponent<Attackable>();
        if(attack != null)
        {
            if(Hp > 0)
            {
                Dead = false;
                Damaged(attack.Damage, attack.attackTick);
            }else{
                Dead = true;
            }
        }
    }


    IEnumerator attack_this(float damage, float tick)
    {
        float dam = damage * armor_power;
        Hp -= dam;
        has_hit = true;
        yield return new WaitForSeconds(tick);
        if(Hp <= 0) Dead = true;
        has_hit = false;
    }

    public void Damaged(float dam, float tick)
    {
        StartCoroutine(attack_this(dam, tick));
    }
}
