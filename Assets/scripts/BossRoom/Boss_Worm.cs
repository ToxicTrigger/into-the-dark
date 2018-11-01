using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Worm : MonoBehaviour
{
    //
    public int max_hp;
    public int hp;
    //

    BossRoomManager manager;
    Boss_State state;

    bool is_dead;

    void Start()
    {
        is_dead = false;
        hp = max_hp;
        state = GetComponent<Boss_State>();
        manager = BossRoomManager.get_instance();
    }

    void OnTrrigerEnter(Collider other)
    {
        if (!is_dead)
        {
            if (other.CompareTag("Arrow"))
            {
                add_damage();
            }
            if (other.CompareTag("Sword"))
            {
                add_damage();
            }
        }
    }

    public void add_damage()
    {
        hp -= 50;

        int num = Random.Range((int)SoundManager.SoundList.hit_boss_one, (int)SoundManager.SoundList.hit_boss_two + 1);
        SoundManager.get_instance().play_sound((SoundManager.SoundList)num);

        switch (manager.phase)
        {
            case BossRoomManager.Phase.one:
                if (hp < max_hp * 0.5 && state.get_state() == Boss_State.State.Groggy)
                {
                    manager.increase_pahse(true);
                }
                break;
            case BossRoomManager.Phase.two:
                if (hp <= 0)
                {
                    manager.game_clear();
                    is_dead = true;
                }
                break;
        }

    }

    public int get_hp()
    {
        return hp;
    }
    public int get_max_hp()
    {
        return max_hp;
    }
    public void set_hp(int _hp)
    {
        hp = _hp;
    }
}


