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
                na.enabled = true;
                tick = 0;
                break;

            case "Die":
                tick = 0;
                Debug.Log("false");
                break;

            case "hit":
                tick = 0;
                break;

            case "targeting":
                na.enabled = true;
                tick = 0;
                break;

            case "attack":
                if(tick <= ai.attack.attackTick)
                {
                    tick += Time.deltaTime;
                }
                else
                {
                    na.enabled = false;
                    tick = 0;
                    if (ai.target != null)
                    {
                        if(ai.target.transform.parent.GetComponent<Damageable>() != null)
                        {
                            var t = ai.target.transform.parent.GetComponent<Damageable>();
                            if (t != null)
                            {
                                t.Damaged(ai.attack.Damage, ai.attack.attackTick, t.transform);
                                transform.LookAt(ai.target.transform);
                            }
                        }
                    }
                }
                break;
        }

    }
}
