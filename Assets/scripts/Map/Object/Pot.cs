using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Damageable
{
    [SerializeField]
    private List<MeshCollider> particles;
    bool ok;

    public void Update()
    {
        if (this.Dead)
        {
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
