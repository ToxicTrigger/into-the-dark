using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : Observer {
    //보스룸 정문

    public float open_speed;
    public int loop_cnt;
    public int clear_max_cnt;
    public int clear_cnt;
    public bool be_open;

	void Start () {
		
	}

    public override void notify(Observable obs)
    {
        BasicSwitch puzzle_switch = obs as BasicSwitch;
        Debug.Log(obs.name);
        if(puzzle_switch.get_switch())
        {
            clear_cnt++;
            if(clear_cnt == clear_max_cnt)
            {
                be_open = true;
                StartCoroutine(door_open());
            }
        }
        else
        {
            if(0<clear_cnt)
            {
                clear_cnt--;
            }
        }

    }

    IEnumerator door_open()
    {
        for (int i = 0; i <= loop_cnt; i++)
        {
            gameObject.transform.position += Vector3.up * open_speed;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
