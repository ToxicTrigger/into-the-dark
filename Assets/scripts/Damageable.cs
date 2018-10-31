using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float Hp, Max_Hp;
    public bool has_hit;
    public bool Dead;
    Transform origin;

    public GameObject Hit_particle;

    [SerializeField]
    private bool only_player;

    //실질적으로 모든 데미지 연산은 해당 변수를 거쳐가는데
    //간단히 AttackDamage * ArmorPower 의 연산임.
    //중요한 것은 해당 방어 수치나 공격력이 소수로 보이면 플레이 입장에서 흥미가 생기지 않기 때문에
    //표시할 땐 무조건 result * 10 하기를 권장함
    [Tooltip("해당 값은 0일 때 받는 데미지를 100% 받아들입니다.")]
    [Range(-1 , 1)]
    public float armor_power;

    public bool is_invincibility;

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
                        Damaged(attack.Damage , attack.attackTick, transform);
                        if(other.gameObject.CompareTag("Arrow"))
                        {
                            Destroy(other.gameObject);
                        }
                    }
                }
                else
                {
                    Damaged(attack.Damage , attack.attackTick, transform);
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
        if (Hp <= 0) Dead = true;
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


    public void Damaged(float dam, float tick)
    {
        if(!is_invincibility)
            StartCoroutine(attack_this(dam, tick));
    }

    public void Damaged(float dam, float tick, Transform other)
    {
        if (!is_invincibility)
        {
            Vector3 pos = other.position;
            pos.y += 1f;
            GameObject t = Instantiate(Hit_particle, pos, Quaternion.identity, null);

            Destroy(t, 1.0f);
            StartCoroutine(attack_this(dam, tick));
        }
    }
}
