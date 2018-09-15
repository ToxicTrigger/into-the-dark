using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Damageable
{
    [SerializeField]
    private float explosion_range = 2f;
    [SerializeField]
    private float explosion_power = 10f;
    [SerializeField]
    private float Damage = 2;
    [SerializeField]
    private GameObject particle;
    private PlayerCamera cam;

    public void Awake()
    {
        cam = FindObjectOfType<PlayerCamera>();
    }


    public void Update()
    {
        if (this.Dead)
        {
            Collider[] targets = Physics.OverlapSphere(this.transform.position, explosion_range);

            foreach (Collider coll in targets)
            {
                Rigidbody r = coll.GetComponent<Rigidbody>();
                Damageable d = coll.GetComponent<Damageable>();

                if (d != null)
                {
                    if (!d.Equals(this))
                    {
                        d.Damaged(Damage, 1);
                    }
                }

                if (r != null)
                {
                    if (!r.Equals(this))
                    {
                        r.AddExplosionForce(explosion_power * 20, this.transform.position, explosion_range, 2400.0f);

                        r.AddForce((r.position - this.transform.position).normalized * explosion_power, ForceMode.Impulse);
                    }
                }
            }
            if (particle != null)
            {
                GameObject tmp = Instantiate(particle, this.transform.position, Quaternion.identity, null);
                Destroy(tmp, 5f);
            }
            cam.up_down_move(0.2f, 2, 0.08f, 0);
            Destroy(gameObject);
        }
    }
}
