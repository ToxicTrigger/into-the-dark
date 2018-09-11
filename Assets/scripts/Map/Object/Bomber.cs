using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Damageable {
    public float explosion_range = 2f;
    public float explosion_power = 10f;
    public float Damage = 2;
    public GameObject particle;

    public PlayerCamera cam;


    void Update ()
    {
		if(this.Dead)
        {
            Collider[] targets = Physics.OverlapSphere(this.transform.position, explosion_range);

            foreach(Collider coll in targets)
            {
                Rigidbody r = coll.GetComponent<Rigidbody>();
                Damageable d = coll.GetComponent<Damageable>();

                if(d != null)
                {
                    if(!d.Equals(this))
                    {
                        d.Damaged(Damage, 1);
                    }
                }

                if(r != null)
                {
                    if(!r.Equals(this))
                    {
                        r.AddExplosionForce(explosion_power * 20, this.transform.position, explosion_range, 2400.0f);

                        r.AddForce((r.position - this.transform.position).normalized * explosion_power, ForceMode.Impulse);
                    }
                }
            }
            if(particle != null)
            {
                GameObject tmp = Instantiate(particle, this.transform.position, Quaternion.identity, null);
                Destroy(tmp, 5f);
            }

            cam.up_down_move(0.2f, 2, 0.08f, 0);
            //cam.left_right_move(0.1f, 1, 0.1f);

            Destroy(gameObject);
        }
	}
}
