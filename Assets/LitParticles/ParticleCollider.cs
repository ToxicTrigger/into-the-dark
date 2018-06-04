using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//폐기 처분 진지하게 고려 중.

[ExecuteInEditMode]
public class ParticleCollider : MonoBehaviour {
    private static ParticleCollider me;
    public static ParticleCollider instance
    {
        get
        {
            if(!me)
            {
                me = GameObject.FindObjectOfType(typeof(ParticleCollider)) as ParticleCollider;
                if(!me)
                {
                    GameObject tmp = new GameObject();
                    tmp.name = "FogManager";
                    me = tmp.AddComponent(typeof(ParticleCollider)) as ParticleCollider;
                }
            }
            return me;
        }
    }


    public ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    public float time;

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        int numExit = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = inside[i];
            float c = p.remainingLifetime;
            
            c -= time;
            p.remainingLifetime = c;
            inside[i] = p;
        }
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Arrow"))
        {
            if(!other.GetComponent<Arrow>().has_targeting_totem)
            {
                Destroy(other);
            }
        }
    }

}
