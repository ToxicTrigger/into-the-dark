using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
    public float Hp;
    public bool has_hit;
    public bool Dead;

    // 피격받아야 하는 객체는 앞으로 해당 클래스를 상속받아 구현하도록 합니다.
    // 읽으셨다면 주석은 삭제 해주세요!
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
        Hp -= damage;

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
