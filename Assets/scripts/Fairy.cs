using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Fairy : MonoBehaviour
{
    Vector3 pos, cur;
    public Transform target;
    public Damageable PlayerHp, Hp;
    public SphereCollider Range;
    public float Range_per, Hp_per;
    public Material particle_color;
    public Color black, cur_color;
    public Gradient ggg;
    public float col;
    public PostProcessingBehaviour ppb;
    public float smoothness;

    private void Start()
    {
        PlayerHp = FindObjectOfType<Player>().GetComponent<Damageable>();
        Hp = GetComponent<Damageable>();
        Range = GameObject.Find("PlayerHpRange").GetComponent<SphereCollider>();
        Range_per = Range.radius;
        cur_color = particle_color.color;
    }

    void Update()
    {
        calc_pos();
        calc_range();
    }
    float down_tick;
    float up_tick;
    public bool vignette_done, v_off;
    // 5 inten , Ran 7
    // 2 base Light Range
    // 2 / 100 * LR = Hp
    void calc_range()
    {
        if(PlayerHp != null)
        Hp_per = PlayerHp.Hp / 100;

        //50per down?
        if (Hp_per <= 0.5f)
        {
            down_tick += Time.deltaTime;
            cur_color = ggg.Evaluate(down_tick % 1.0f);


            if (smoothness >= 1.0f)
            {
                vignette_done = true;
            }
            else if (smoothness <= 0.2f)
            {
                vignette_done = false;
            }

            if (vignette_done)
            {
                smoothness -= Time.deltaTime;
            }
            else
            {
                smoothness += Time.deltaTime;
            }
            
            
        }
        else
        {
            cur_color = ggg.Evaluate(0);
        }

        Range_per = Hp_per / 2 * 5f;
        if(Range != null)
        Range.radius = Range_per;
    }

    void calc_pos()
    {
        cur = transform.position;
        cur.x += Mathf.Sin(Time.time) * 0.45f;
        cur.z += Mathf.Cos(Time.time) * 0.165f;
        cur.y += Mathf.Sin(Time.time) * 0.4f;
        pos = Vector3.Lerp(cur, target.position, Time.time * 0.01f);
        this.transform.position = pos;
    }
}
