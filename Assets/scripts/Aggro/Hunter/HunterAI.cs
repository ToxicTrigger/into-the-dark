using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAI : AggroAI {
    public float tick = 0;

    public override void FSM(AggroAI ai)
    {
        switch (cur_ani)
        {
            case "idle":
                break;

            case "Die":
                
                break;

            case "hit":
                
                break;

            case "targeting":
                break;

            case "attack":
                if(tick <= ai.attack.attackTick)
                {
                    tick += Time.deltaTime;
                }
                else
                {
                    tick = 0;
                    ai.target.transform.parent.GetComponent<Damageable>().Hp -= ai.attack.Damage;
                }
                break;
        }

    }
}
