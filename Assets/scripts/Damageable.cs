using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float Hp, Max_Hp;
    public bool has_hit;
    public bool Dead;
    Transform origin;

    [SerializeField]
    private bool only_player;

    //실질적으로 모든 데미지 연산은 해당 변수를 거쳐가는데
    //간단히 AttackDamage * ArmorPower 의 연산임.
    //중요한 것은 해당 방어 수치나 공격력이 소수로 보이면 플레이 입장에서 흥미가 생기지 않기 때문에
    //표시할 땐 무조건 result * 10 하기를 권장함
    [Tooltip("해당 값은 0일 때 받는 데미지를 100% 받아들입니다.")]
    [Range(-1 , 1)]
    public float armor_power;

    public void Start()
    {
        origin = this.transform;
        Hp = Max_Hp;
    }

    public void OnCollisionEnter(Collision other)
    {
        Attackable attack = other.gameObject.GetComponent<Attackable>();
        if( attack != null )
        {
            Debug.Log("this : " + gameObject.name + " | actor : " + other.gameObject.name);
            if( Hp > 0 )
            {
                Dead = false;
                if( only_player )
                {
                    if( other.gameObject.CompareTag("Arrow") || other.gameObject.CompareTag("Sword") )
                    {
                        Damaged(attack.Damage , attack.attackTick);
                    }
                }
                else
                {
                    Damaged(attack.Damage , attack.attackTick);
                }
            }
            else
            {
                Dead = true;
            }
        }
    }
    public float per;
    public void FixedUpdate()
    {
        per = armor_power == 0 ? 1 : armor_power + 1;
        per = armor_power < 0 ? armor_power - 1 : per;
        if( Hp > Max_Hp ) Hp = Max_Hp;
    }

    IEnumerator attack_this(float damage , float tick)
    {
        float dam = per != 1 ? damage * armor_power : damage;

        Hp -= dam;
        has_hit = true;

        yield return new WaitForSeconds(tick);
        if( Hp <= 0 ) Dead = true;
        has_hit = false;
    }

    public void Damaged(float dam , float tick)
    {
        StartCoroutine(attack_this(dam , tick));
    }
}
