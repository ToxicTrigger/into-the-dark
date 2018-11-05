using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Damageable
{
    public List<MeshCollider> particles;
    public AudioSource broken_sound;
    bool ok;

    public new void Start()
    {
        base.Start();
    }

    // 아래 코드로 새롭게 충돌 처리를 해야 할 경우를 해결 할 수 있습니다.
    // 기본적인 Damageable 기능만 사용하실려면 이렇게 하지 않아도 기본 로직이 움직이고 있기 때문에!
    // 필요한 변수만 끌어다 쓰세요.
    // 다 읽으셨다면 아래 코드도 지워주세욧!
    public new void OnCollisionEnter(Collision other)
    {
        // 상속된 부모의 함수를 호출 하기 위해 base 예약어를 사용 합니다.
        base.OnCollisionEnter(other);
        
        //Do Something ```
    }
    //요기 까지만 지워주세용

    public void Update()
    {
        if (this.Dead)
        {
            broken_sound.PlayOneShot(broken_sound.clip);
            broken_sound.transform.parent = null;
            Destroy(broken_sound , 4.0f);

            if (!ok)
            {
                if (transform.childCount != 0)
                {
                    transform.GetChild(0).parent = null;
                }

                IEnumerator enumerator = particles.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    MeshCollider mc = enumerator.Current as MeshCollider;
                    if (mc != null)
                    {
                        mc.enabled = true;
                        Rigidbody rig = mc.GetComponent<Rigidbody>();
                        rig.constraints = RigidbodyConstraints.None;
                        mc.transform.parent = null;
                        
                        Destroy(mc.gameObject , 1.0f);
                    }
                }
                ok = true;
                GetComponent<BoxCollider>().enabled = false;
            }

            if (transform.localScale.x >= 0)
            {
                Vector3 scale = transform.localScale;
                scale *= 0.97f;
                transform.localScale = scale;
                
                Destroy(gameObject);
            }
        }
    }
}
